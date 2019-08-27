//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Game;
using JEM.QNet.UnityEngine.Handlers;
using JEM.UnityEngine.VersionManagement;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JEM.QNet.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     QNet general manager.
    /// </summary>
    public partial class QNetManager : MonoBehaviour
    {
        /// <summary>
        ///     Currently used database of prefabs.
        /// </summary>
        [Header("Base Settings")]
        public QNetDatabase TargetDatabase;

        private void Awake()
        {
            if (Instance != null)
                return;

            Instance = this;
            Database = TargetDatabase;
            if (Database == null) Debug.LogError("Target QNetDatabase is not set!", this);
        }

        private void FixedUpdate()
        {
            if (!IsNetworkActive)
                return;

            // receive messages
            PollMessages();

            // try to sync server frames
            QNetSimulation.AdjustFrames();

            // dispatch messages
            DispatchMessages();

            // simulate
            QNetSimulation.Simulate();
        }

        private void PollMessages()
        {
            for (var index = 0; index < RunningPeers.Length; index++)
            {
                var peer = RunningPeers[index];
                peer.PollMessages();
            }
        }

        private void DispatchMessages()
        {
            for (var index = 0; index < RunningPeers.Length; index++)
            {
                var peer = RunningPeers[index];
                peer.DispatchMessages();
            }
        }

        private void OnApplicationQuit()
        {
            // make sure that all open connection will be shut down on application quit
            ShuttingDownByApplicationQuit = true;
            if (InternalRunningPeers.Count != 0)
                StopCurrentConnection(IsServerActive ? "Server shutdown by ApplicationQuit" : "Quit");
        }

        /// <summary>
        ///     Starts given peer instance.
        /// </summary>
        /// <param name="peer">Peer to start.</param>
        /// <param name="configuration">Configuration of peer.</param>
        /// <param name="onRegisterHandlers"></param>
        private static T InternalStartPeer<T>([NotNull] T peer, [NotNull] QNetConfiguration configuration,
            Action onRegisterHandlers)
            where T : QNetPeer
        {
            if (peer == null) throw new ArgumentNullException(nameof(peer));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            // first register peer handlers
            peer.RegisterPeerHandlers();
            // then register peer local handlers
            onRegisterHandlers.Invoke();
            // then start peer
            peer.Start(configuration);
            // and at the end, add new running peer
            InternalRunningPeers.Add(peer);
            // rebuild array
            RunningPeers = InternalRunningPeers.ToArray();
            // clear network time
            QNetTime.Frame = 0;
            QNetSimulation.EstimatedServerFrame = 0;
            return peer;
        }

        /// <summary>
        ///     Stops given peer.
        /// </summary>
        private static void InternalStopPeer([NotNull] QNetPeer peer, string stopReason)
        {
            if (peer == null) throw new ArgumentNullException(nameof(peer));
            if (!InternalRunningPeers.Contains(peer))
                throw new InvalidOperationException(
                    "Unable to stop given peer. Given peer can't be stopped while not started.");
            // remove this peer
            InternalRunningPeers.Remove(peer);
            // rebuild array
            RunningPeers = InternalRunningPeers.ToArray();
            // and then stop
            peer.Stop(stopReason);
        }

        /// <summary>
        ///     Starts local server based on given or local configuration.
        /// </summary>
        public static void StartServer(QNetConfiguration configuration = null)
        {
            if (IsServerActive)
                throw new InvalidOperationException(
                    "QNet is unable to start server while there is already active instance of server.");
            if (configuration == null) configuration = new QNetConfiguration();

            var hostIsActive = IsHostActive;
            Server = new QNetServer();
            Server = InternalStartPeer(Server, configuration, () =>
            {
                QNetHandlerStack.RegisterServerHandlers(Server);
                OnServerRegisterHeaders?.Invoke();
            });
            OnServerPrepare?.Invoke();

            Server.OnBeforeMessage += message =>
            {
                // server should always send the server frame to the client!
                message.Write(QNetTime.ServerFrame);
            };

            Server.OnConnectionAuthorizing += (QNetConnection connection, QNetMessageWriter writer, ref bool refuse) =>
            {
                writer.WriteInt32(QNetTime.TickRate);
                writer.WriteUInt32(QNetTime.ServerFrame);

                OnServerAuthorizePlayer?.Invoke(connection, writer, ref refuse);
            };

            Server.OnConnectionReady += reader =>
            {
                // read player nickname and create its QNetPlayer instance         
                var nickname = reader.ReadString();
                var token = reader.ReadUInt32();
                var version = reader.ReadString();
                if (JEMBuild.BuildVersion != version)
                {
                    JEMLogger.LogError(
                        $"Newly received connection don't have right version of the game -> {version} ( is {JEMBuild.BuildVersion} )");
                    Server.CloseConnection(reader.Connection, "InvalidBuildVersion");
                }
                else
                {

                    if (IsHostActive && (reader.Connection.ConnectionIdentity == 0 ||
                                         reader.Connection.ConnectionIdentity == Client.ConnectionIdentity))
                    {
                        HostClientConnection = reader.Connection;
                        JEMLogger.Log("QNetUnity received host client connection.");

                    }

                    if (QNetPlayer.GetQNetPlayerByToken(token) != null)
                    {
                        JEMLogger.LogError("Newly received connection is using already used token. Disconnecting!");
                        Server.CloseConnection(reader.Connection, "TokenAlreadyInUse");
                        return;
                    }

                    var qNetPlayer = QNetPlayer.CreateQNetPlayer(reader.Connection.ConnectionIdentity, nickname, token);
                    if (qNetPlayer == null)
                    {
                        JEMLogger.LogError(
                            "Newly received connection don't have his QNetPlayer instance. Disconnecting!");
                        Server.CloseConnection(reader.Connection, "InternalQNetPlayerError");
                        return;
                    }

                    OnServerNewPlayer?.Invoke(qNetPlayer, reader);
                    QNetServerConnectionInit.PrepareNewConnection(reader.Connection);
                }
            };

            Server.OnConnectionLost += (connection, reason) =>
            {
                var qNetPlayer = QNetPlayer.GetQNetPlayer(connection);
                if (qNetPlayer != null) // if QNetPlayer of this connection not exists, just ignore
                {
                    OnServerPlayerLost?.Invoke(qNetPlayer, reason);

                    // the only thing to do here is to tag player as not ready (if ready)
                    if (qNetPlayer.Ready)
                        qNetPlayer.TagAsNotReady();

                    // and remove QNetPlayer from local machine
                    QNetPlayer.DestroyQNetPlayer(qNetPlayer);
                }
            };

            Server.OnServerStop += reason =>
            {
                // server has been stopped, try to de-initialize game
                if (!ShuttingDownByApplicationQuit)
                    QNetGameInitializer.DeInitialize(() => { OnServerShutdown?.Invoke(hostIsActive); });
                else
                    OnServerShutdown?.Invoke(hostIsActive);

                IsServerActive = false;
                IsHostActive = false;
            };

            IsServerActive = true;
            OnServerStarted?.Invoke();
        }

        /// <summary>
        ///     Starts client connection.
        /// </summary>
        public static void StartClient([NotNull] string ipAddress, ushort port, string password)
        {
            if (IsClientActive)
                throw new InvalidOperationException(
                    "QNet is unable to start client while there is already active instance of client.");
            if (ipAddress == null) throw new ArgumentNullException(nameof(ipAddress));

            var configuration = new QNetConfiguration
            {
                IpAddress = ipAddress,
                Port = port,
                MaxConnections = 2
            };

            if (OnClientPrepare == null)
                throw new NullReferenceException("QNet is unable to start client. OnClientPrepare event is not set.");

            Client = new QNetClient();
            Client = InternalStartPeer(Client, configuration, () =>
            {
                QNetHandlerStack.RegisterClientHandlers(Client);
                OnClientRegisterHeaders?.Invoke();
            });
            OnClientPrepare.Invoke(out var nickname, out var token);

            Client.OnMessagePoll += reader =>
            {
                // as the server always send the server frame
                // we need to read that right here
                QNetSimulation.ReceivedServerFrame = reader.ReadUInt32();
                QNetSimulation.AdjustServerFrames = QNetSimulation.ReceivedServerFrame > QNetTime.ServerFrame;
            }; 

            Client.OnConnectionReady += (reader, writer) =>
            {
                var tickRate = reader.ReadInt32();
                var frameNumber = reader.ReadUInt32();

                // set TickRate
                QNetTime.TickRate = tickRate;

                // initialize server frame count
                QNetSimulation.ReceivedServerFrame = frameNumber;
                QNetSimulation.EstimatedServerFrame = frameNumber;

                // write player data
                writer.WriteString(nickname);
                writer.WriteUInt32(token);
                writer.WriteString(JEMBuild.BuildVersion);
                OnClientReady?.Invoke(reader, writer);
            };

            Client.OnDisconnection += (lostConnection, reason) =>
            {
                // stop peer
                if (InternalRunningPeers.Contains(Client))
                    InternalStopPeer(Client, reason);

                // update active state
                IsClientActive = false;

                // and de-initialize game
                if (!IsHostActive && QNetGameInitializer.GameInitialized && !QNetGameInitializer.GameIsDeInitializing
                ) // ignore if host, server will de-initialize it anyway
                {
                    QNetGameInitializer.DeInitialize(() =>
                    {
                        OnClientDisconnected?.Invoke(IsHostActive, lostConnection, reason);
                    });
                }
                else
                {
                    if (!QNetGameInitializer.GameInitialized && !QNetGameInitializer.GameIsInitializing)
                        OnClientDisconnected?.Invoke(IsHostActive, lostConnection, reason);
                }
            };

            IsClientActive = true;
        }

        /// <summary>
        ///     Starts local host based on given configuration.
        /// </summary>
        public static void StartHost([NotNull] QNetConfiguration configuration)
        {
            //if (!Nickname.IsSet)
            //    throw new InvalidOperationException("QNet is unable to start host while nickname is not set.");
            if (IsHostActive)
                throw new InvalidOperationException(
                    "QNet is unable to start host while there is already active instance of host.");
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            JEMLogger.Log("QNet is starting local host.");

            IsHostActive = true;

            StartServer(configuration);
            StartClient(configuration.IpAddress, configuration.Port, string.Empty);
            Server.SetUpHostServer(Client.OriginalConnection);

            Client.OnDisconnection += (lostConnection, reason) =>
            {
                reason = $"(Client) {reason}";
                JEMLogger.Log("QNet is stopping host because of client disconnection.");
                // stop all peers
                StopCurrentConnection(reason);
            };
        }

        /// <summary>
        ///     Stops current connections.
        ///     This method will stop all current connection, so if we are host (with client and server) this will stop it all.
        /// </summary>
        public static void StopCurrentConnection(string stopReason = null)
        {
            JEMLogger.Log("QNet is Stopping all current connections.");

            // check if reason message is empty or null
            if (string.IsNullOrEmpty(stopReason))
                stopReason = "Stopping*";

            // and just stop all peers
            while (InternalRunningPeers.Count > 0)
                InternalStopPeer(InternalRunningPeers[0], stopReason);

            Client = null;
            Server = null;

            IsClientActive = false;
            IsServerActive = false;
            IsHostActive = false;
        }

        /// <summary>
        ///     Defines whether the QNet is shutting down by ApplicationQuit.
        /// </summary>
        public static bool ShuttingDownByApplicationQuit { get; private set; }

        /// <summary>
        ///     List of currently running peers.
        /// </summary>
        public static QNetPeer[] RunningPeers { get; private set; } = new QNetPeer[0];
        private static List<QNetPeer> InternalRunningPeers { get; } = new List<QNetPeer>();

        /// <summary>
        ///     Active client instance.
        /// </summary>
        public static QNetClient Client { get; private set; }

        /// <summary>
        ///     Active server instance.
        /// </summary>
        public static QNetServer Server { get; private set; }

        /// <summary>
        ///     Host client connection.
        /// </summary>
        public static QNetConnection HostClientConnection { get; private set; }

        /// <summary>
        ///     Defines whether the client is active.
        /// </summary>
        public static bool IsClientActive { get; private set; }

        /// <summary>
        ///     Defines whether the server is active.
        /// </summary>
        public static bool IsServerActive { get; private set; }

        /// <summary>
        ///     Defines whether the host is active.
        /// </summary>
        public static bool IsHostActive { get; private set; }

        /// <summary>
        ///     Defines whether the network is active.
        /// </summary>
        public static bool IsNetworkActive => IsClientActive || IsServerActive || IsHostActive;

        /// <summary>
        ///     Defines whether the game should be shutdown while QNetUnity receives timeout of object serialization.
        /// </summary>
        public static bool ShutdownOnSerializationTimeout { get; set; } = true;

        /// <summary>
        ///     If set to false, network warnings will not be print.
        /// </summary>
        public static bool PrintNetworkWarnings { get; set; } = true;

        /// <summary>
        ///     Current instance of script.
        /// </summary>
        internal static QNetManager Instance { get; private set; }

        /// <summary>
        ///     Current database of prefabs.
        /// </summary>
        public static QNetDatabase Database { get; private set; }
    }
}