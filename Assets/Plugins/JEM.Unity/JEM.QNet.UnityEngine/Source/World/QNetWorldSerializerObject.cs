//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Behaviour;

#pragma warning disable 1591

namespace JEM.QNet.UnityEngine.World
{
    /// <summary>
    ///     Object to to create of world serializer.
    /// </summary>
    public struct QNetWorldSerializerObject
    {
        public QNetObjectSerialized Object;
        public QNetMessageReader SerializedServerState;
    }
}