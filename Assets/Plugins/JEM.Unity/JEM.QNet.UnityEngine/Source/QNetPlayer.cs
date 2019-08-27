//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.World;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     QNet player spawn event.
    /// </summary>
    public delegate void QNetPlayerSpawn(QNetConnection connection, out QNetObjectBehaviour playerObject);

    /// <summary>
    ///     QNet player spawn position event.
    /// </summary>
    public delegate void QNetPlayerSpawnPosition(QNetPlayer player, out Vector3 position, out Quaternion rotation);

    /// <summary>
    ///     QNetServer connection.
    ///     This class contains all data about connection.
    ///     For ex. nickname, token and reference to player controller stuff.
    /// </summary>
    public class QNetPlayer
    {
        /// <summary>
        ///     Player spawn event. If set, system will use this instead of inbuild spawn method.
        /// </summary>
        public static QNetPlayerSpawn OnPlayerCustomSpawn;

        /// <summary>
        ///     Player spawn position event.
        /// </summary>
        public static QNetPlayerSpawnPosition OnPlayerSpawnPosition;

        private QNetPlayer(QNetConnection connection, string nickname, uint token)
        {
            Connection = connection;
            ConnectionIdentity = connection.ConnectionIdentity;

            Nickname = nickname;
            Token = token;
        }

        private QNetPlayer(short connectionIdentity, string nickname, uint token)
        {
            Connection = default(QNetConnection);
            ConnectionIdentity = connectionIdentity;

            Nickname = nickname;
            Token = token;
        }

        /// <summary>
        ///     Connection of this player.
        ///     This property is set only on server.
        ///     Clients will always see this property set to default.
        /// </summary>
        public QNetConnection Connection { get; set; }

        /// <summary>
        ///     Identity of connection.
        /// </summary>
        public short ConnectionIdentity { get; set; }

        /// <summary>
        ///     Nickname of player.
        /// </summary>
        public string Nickname { get; }

        /// <summary>
        ///     Token of player. Used for authorization and save loading.
        /// </summary>
        public uint Token { get; }

        /// <summary>
        ///     Defines whether this player is ready.
        ///     If set to ready, will receive all QNetPlayers and some object data.
        /// </summary>
        public bool Ready { get; set; }

        /// <summary>
        ///     Defines whether the player is loaded.
        ///     Used for map loading on fly.
        /// </summary>
        public bool Loaded { get; set; } = true;

        /// <summary>
        ///     Player network controller.
        /// </summary>
        public QNetObjectBehaviour PlayerObject { get; private set; }

        /// <summary>
        /// Gets QNetObjectBehaviour based component from PlayerObject.
        /// </summary>
        public T GetPlayerObject<T>() where T : QNetObjectBehaviour
        {
            return PlayerObject == null ? default(T) : PlayerObject.GetComponent<T>();
        }

        /// <summary>
        ///     Tags this player as ready.
        ///     An actual loading method.
        /// </summary>
        public void TagAsReady()
        {
            if (Ready)
            {
                if (!QNetManager.IsHostActive)
                    throw new InvalidOperationException("Unable to tag player as ready while is already ready.");
                return;
            }

            JEMLogger.Log($"> Tagging player {ToString()} as ready.");
            Ready = true;
            JEMLogger.Log("> (Tagging) Initializing player on server side.");

            if (OnPlayerCustomSpawn != null)
            {
                OnPlayerCustomSpawn.Invoke(Connection, out var obj);
                PlayerObject = obj;
            }
            else
            {
                var spawnPoint = Vector3.one;
                var spawnRotation = Quaternion.identity;

                if (OnPlayerSpawnPosition == null)
                {
                    QNetSpawnArea.GetRandomSpawnArea().GenerateUnreliablePoint(out spawnPoint, out var spawnForward);
                    spawnRotation = Quaternion.LookRotation(spawnForward);
                }
                else
                {
                    OnPlayerSpawnPosition?.Invoke(this, out spawnPoint, out spawnRotation);
                }

                // create player's object
                PlayerObject = QNetObjectBehaviour.SpawnWithOwner(QNetManager.Database.PlayerPrefab, spawnPoint,
                    spawnRotation, Connection);
            }

            // check for errors
            if (PlayerObject == null)
                throw new NullReferenceException("PlayerObject is missing.");

            JEMLogger.Log($"> Player {ToString()} is now ready.");
        }

        /// <summary>
        ///     Tags this player as not ready.
        ///     An actual unloading method.
        /// </summary>
        public void TagAsNotReady()
        {
            if (!Ready)
                throw new InvalidOperationException("Unable to tag player as not ready while is already not ready.");

            JEMLogger.Log($"> Tagging player {ToString()} as not ready.");

            Ready = false;

            JEMLogger.Log("> (Tagging) De-Initializing player on server side.");

            // destroy all owned objects
            QNetServerObjects.DestroyAllOwnedObjectsOfConnection(Connection);

            JEMLogger.Log($"> Player {ToString()} is now not ready.");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"QNetPlayer-{Nickname}/{ConnectionIdentity}/{Token}";
        }

        /// <summary>
        ///     Creates new QNetPlayer instance based on given connection identity.
        /// </summary>
        /// <param name="connectionIdentity">Connection identity of player.</param>
        /// <param name="nickname">Nickname of player.</param>
        /// <param name="token"></param>
        public static QNetPlayer CreateQNetPlayer(short connectionIdentity, string nickname, uint token)
        {
            var connection = QNetManager.Server.GetConnection(connectionIdentity);
            if (connection.Equals(default(QNetConnection)))
                throw new InvalidOperationException(
                    $"System was unable to create QNetPlayer on server. Connection of identity {connectionIdentity} not exist.");

            var player = new QNetPlayer(connection, nickname, token);
            if (GetQNetPlayer(player.ConnectionIdentity) != null)
                throw new InvalidOperationException(
                    "System is trying to create QNetPlayer of identity that already exist in this machine.");

            InternalQNetPlayers.Add(player);

            // rebuild array
            QNetPlayers = InternalQNetPlayers.ToArray();
            return player;
        }

        /// <summary>
        ///     Destroys QNetPlayer of given connection identity.
        /// </summary>
        public static void DestroyQNetPlayer(short connectionIdentity)
        {
            var player = GetQNetPlayer(connectionIdentity);
            if (player == null)
                throw new InvalidOperationException(
                    $"You are trying to destroy QNetPlayer of identity {connectionIdentity}, but player of this identity not exists.");

            DestroyQNetPlayer(player);
        }

        /// <summary>
        ///     Destroys given QNetPlayer.
        /// </summary>
        public static void DestroyQNetPlayer([NotNull] QNetPlayer player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            InternalQNetPlayers.Remove(player);

            // rebuild array
            QNetPlayers = InternalQNetPlayers.ToArray();
        }

        /// <summary>
        ///     Destroys all QNetPlayers.
        /// </summary>
        public static bool DestroyAllQNetPlayers()
        {
            JEMLogger.Log("QNetUnity is destroying all QNetPlayers.");
            while (InternalQNetPlayers.Count > 0)
                DestroyQNetPlayer(InternalQNetPlayers[0]);
            return true;
        }

        /// <summary>
        ///     Gets QNetPlayer by it's connection identity.
        /// </summary>
        public static QNetPlayer GetQNetPlayer(QNetConnection connection)
        {
            return GetQNetPlayer(connection.ConnectionIdentity);
        }

        /// <summary>
        ///     Gets QNetPlayer by it's connection identity.
        /// </summary>
        public static QNetPlayer GetQNetPlayer(short connectionIdentity)
        {
            return QNetPlayers.FirstOrDefault(player => player.ConnectionIdentity == connectionIdentity);
        }

        /// <summary>
        ///     Gets QNetPlayer by it's nickname.
        /// </summary>
        public static QNetPlayer GetQNetPlayerByNickname(string nickname)
        {
            return QNetPlayers.FirstOrDefault(player => player.Nickname == nickname);
        }

        /// <summary>
        ///     Gets QNetPlayer by it's token.
        /// </summary>
        public static QNetPlayer GetQNetPlayerByToken(uint token)
        {
            return QNetPlayers.FirstOrDefault(player => player.Token == token);
        }

        /// <summary>
        ///     All created QNetPlayers.
        /// </summary>
        public static QNetPlayer[] QNetPlayers { get; private set; } = new QNetPlayer[0];
        private static List<QNetPlayer> InternalQNetPlayers { get; } = new List<QNetPlayer>();
    }
}