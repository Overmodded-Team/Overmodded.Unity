//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using System.Diagnostics.CodeAnalysis;

namespace JEM.QNet.UnityEngine.Messages
{
    /// <summary>
    ///     QNet local header for unity extension.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum QNetUnityLocalHeader : ushort
    {
        /// <summary>
        ///     Level loading message send from server to client.
        /// </summary>
        LEVEL_LOADING = QNetLocalHeader.LAST_HEADER + 1,

        /// <summary>
        ///     Level loaded message send from client to server.
        /// </summary>
        LEVEL_LOADED,

        /// <summary>
        ///     Level loading message send from server to clients. (info about server's loading start process)
        /// </summary>
        LEVEL_LOAD_ON_FLY_STARTING,

        /// <summary>
        ///     Level loading message send from server to clients. (info for clients to start loading this map)
        /// </summary>
        LEVEL_LOAD_ON_FLY_RUN,

        /// <summary>
        ///     Level loading message send from client to server.
        /// </summary>
        LEVEL_LOADED_ON_FLY,

        /// <summary>
        ///     World serializing message.
        /// </summary>
        WORLD_SERIALIZING,

        /// <summary>
        ///     World serialization message.
        /// </summary>
        WORLD_SERIALIZATION,

        /// <summary>
        ///     World serialized message.
        /// </summary>
        WORLD_SERIALIZED,

        /// <summary>
        ///     Object create message.
        /// </summary>
        OBJECT_CREATE,

        /// <summary>
        ///     Object update state message.
        /// </summary>
        OBJECT_STATE,

        /// <summary>
        ///     Object delete message.
        /// </summary>
        OBJECT_DELETE,

        /// <summary>
        ///     Entity query message.
        /// </summary>
        ENTITY_QUERY,

        /// <summary>
        ///     Always last header
        /// </summary>
        LAST_HEADER
    }
}