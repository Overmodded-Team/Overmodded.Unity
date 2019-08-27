//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Handlers
{
    /// <summary>
    ///     Class with two functions dedicated only to register all handlers of given peer.
    /// </summary>
    internal static class QNetHandlerStack
    {
        /// <summary>
        ///     Registers all server side handlers.
        /// </summary>
        public static void RegisterServerHandlers([NotNull] QNetServer server)
        {
            if (server == null) throw new ArgumentNullException(nameof(server), "Server is null.");
            server.SetHandler(new QNetMessage(true, (ushort) QNetUnityLocalHeader.LEVEL_LOADED, QNetHandlerWorldReceiver.OnClientLevelLoaded));
            server.SetHandler(new QNetMessage(true, (ushort) QNetUnityLocalHeader.LEVEL_LOADED_ON_FLY, QNetHandlerWorldReceiver.OnClientLevelLoadedOnFly));
            server.SetHandler(new QNetMessage(true, (ushort) QNetUnityLocalHeader.WORLD_SERIALIZED, QNetHandlerWorldReceiver.OnClientWorldSerialized));
            server.SetHandler(new QNetMessage(true, (ushort) QNetUnityLocalHeader.ENTITY_QUERY, QNetHandlerObjectReceiver.OnClientEntityQuery));
        }

        /// <summary>
        ///     Registers all client side handlers.
        /// </summary>
        public static void RegisterClientHandlers([NotNull] QNetClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client), "Client is null.");
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.LEVEL_LOADING, QNetHandlerWorldReceiver.OnServerLevelLoading));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.LEVEL_LOAD_ON_FLY_STARTING, QNetHandlerWorldReceiver.OnServerLevelLoadingOnFlyStarting));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.LEVEL_LOAD_ON_FLY_RUN, QNetHandlerWorldReceiver.OnServerLevelLoadingOnFlyRun));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.WORLD_SERIALIZING, QNetHandlerWorldReceiver.OnServerWorldSerializing));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.WORLD_SERIALIZATION, QNetHandlerWorldReceiver.OnServerWorldSerialization));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.OBJECT_CREATE, QNetHandlerObjectReceiver.OnServerObjectCreate));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.OBJECT_DELETE, QNetHandlerObjectReceiver.OnServerObjectDelete));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.OBJECT_STATE, QNetHandlerObjectReceiver.OnServerObjectState));
            client.SetHandler(new QNetMessage(false, (ushort) QNetUnityLocalHeader.ENTITY_QUERY, QNetHandlerObjectReceiver.OnClientEntityQuery));
        }
    }
}