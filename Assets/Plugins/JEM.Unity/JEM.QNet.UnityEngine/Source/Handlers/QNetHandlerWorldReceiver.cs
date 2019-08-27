//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Linq;
using JEM.Core.Debugging;
using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.Extensions;
using JEM.QNet.UnityEngine.Game;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Handlers
{
    internal static class QNetHandlerWorldReceiver
    {
        internal static void OnServerLevelLoading(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // write new object in to serializer
            if (QNetManager.IsServerActive)
            {
                JEMLogger.Log(
                    $"We are on server. Client initialization from {nameof(OnServerLevelLoading)} will be ignored.");
                // send level loaded message instantly
                QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                    QNetUnityLocalHeader.LEVEL_LOADED);
            }
            else
            {
                QNetGameInitializer.RunClient(new QNetGameInitializerData {LevelName = reader.ReadString()},
                    () =>
                    {
                        QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                            QNetUnityLocalHeader.LEVEL_LOADED);
                    });
            }
        }

        internal static void OnClientLevelLoaded(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // SERVER: player loads level
            if (QNetManager.IsHostActive)
                if (QNetManager.Client.ConnectionIdentity == reader.Connection.ConnectionIdentity)
                {
                    // loaded level message has been send from host client, send only WORLD_SERIALIZATION message
                    QNetManager.Server.Send(reader.Connection, QNetLocalChannel.DEFAULT,
                        QNetMessageMethod.ReliableOrdered, QNetUnityLocalHeader.WORLD_SERIALIZATION);
                    return;
                }

            JEMLogger.Log($"Connection {reader.Connection} loads level. Sending world objects.");

            // send WORLD_SERIALIZING message to prepare client
            QNetManager.Server.Send(reader.Connection, QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                QNetUnityLocalHeader.WORLD_SERIALIZING);

            // send all objects to client
            QNetServerObjects.SendAllObjectsToConnection(reader.Connection);

            // send WORLD_SERIALIZATION message to start object loading on client
            QNetManager.Server.Send(reader.Connection, QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                QNetUnityLocalHeader.WORLD_SERIALIZATION);
        }

        internal static void OnServerWorldSerializing(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // CLIENT: server starts to send qnet objects
            if (QNetManager.IsServerActive)
                throw new InvalidOperationException(
                    "OnWorldSerializing message has been received while server(host) is active.");

            // we can activate now some loading algorithms to view progress
        }

        internal static void OnServerWorldSerialization(QNetMessage message, QNetMessageReader reader,
            ref bool disallowRecycle)
        {
            // CLIENT: server qnet object send ends
            // server ends sending of serialized objects
            // now client can starts it's de-serialization but if client is also host, this operation is completely ignored
            // because all object was serialized at server startup

            if (QNetManager.IsServerActive)
            {
                // ignoring all operations and continuing by sending WORLD_SERIALIZED message
                JEMLogger.Log("Ignoring QNetObject serialization. (Host)");
                QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                    QNetUnityLocalHeader.WORLD_SERIALIZED);
            }
            else
            {
                // run late client world serializer to load all serialized objects from memory
                JEMLogger.Log("Client received all QNetObjects. Starting serialization.");
                QNetGameInitializer.RunLateClientWorldSerializer(() =>
                {
                    // send WORLD_SERIALIZED message
                    JEMLogger.Log("QNetObject serialized.");
                    QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                        QNetUnityLocalHeader.WORLD_SERIALIZED);
                });
            }
        }

        internal static void OnClientWorldSerialized(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // SERVER: player loads all sent qnet objects
            // prepare this client's controller and send final message so the player can play
            // we will tag this connection as ready, so the system should send this player to all connections on server including owner
            // if owner receive it's player instance from server, they will initialize local systems including camera and movement controller
            var player = QNetPlayer.GetQNetPlayer(reader.Connection);
            if (player == null)
            {
                JEMLogger.LogError("QNet encounter an unexcepted error. Connection QNetPlayer not exists.");
                QNetManager.Server.CloseConnection(reader.Connection, "QNetUnexceptedError");
            }
            else
            {
                player.TagAsReady();
            }
        }

        public static void OnLevelLoadingOnFly(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            /*
            // write new object in to serializer
            if (QNetManager.IsServerActive)
            {
                JEMLogger.Log(
                    $"We are on server. Client initialization from {nameof(OnServerLevelLoading)} will be ignored.");
                // send level loaded message instantly
                QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                    QNetUnityLocalHeader.LEVEL_LOADED_ON_FLY);
            }
            else
            {
                var levelName = reader.ReadString();
                QNetGameInitializer.LoadLevelOnFly(levelName, () =>
                {
                    QNetManager.Client.Send(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                        QNetUnityLocalHeader.LEVEL_LOADED_ON_FLY);
                });
            }
            */
        }

        public static void OnClientLevelLoadedOnFly(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            var player = QNetPlayer.GetQNetPlayer(reader.Connection);
            if (player == null)
                throw new NullReferenceException("player");
            if (player.Loaded)
                return;
        }

        public static void OnServerLevelLoadingOnFlyStarting(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            
        }

        public static void OnServerLevelLoadingOnFlyRun(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            
        }
    }
}