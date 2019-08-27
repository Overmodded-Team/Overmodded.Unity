//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages.Extensions;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Behaviour
{
    /// <inheritdoc />
    /// <summary>
    ///     QNet serialized object data.
    /// </summary>
    public class QNetObjectSerialized : QNetSerializedMessage
    {
        /// <summary>
        ///     Object identity.
        /// </summary>
        public short ObjectIdentity;

        /// <summary>
        ///     Owner identity.
        /// </summary>
        public short OwnerIdentity;

        /// <summary>
        ///     Spawn position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        ///     Prefab identity.
        /// </summary>
        public short PrefabIdentity;

        /// <summary>
        ///     Spawn rotation.
        /// </summary>
        public Quaternion Rotation;

        /// <summary>
        ///     Spawn scale.
        /// </summary>
        public Vector3 Scale;

        /// <inheritdoc />
        public override void Serialize(QNetMessageWriter writer)
        {
            writer.WriteInt16(ObjectIdentity);
            writer.WriteInt16(PrefabIdentity);
            writer.WriteInt16(OwnerIdentity);
            writer.WriteVector3(Position);
            writer.WriteVector3(Rotation.eulerAngles);
            writer.WriteVector3(Scale);
        }

        /// <inheritdoc />
        public override void DeSerialize(QNetMessageReader reader)
        {
            ObjectIdentity = reader.ReadInt16();
            PrefabIdentity = reader.ReadInt16();
            OwnerIdentity = reader.ReadInt16();
            Position = reader.ReadVector3();
            Rotation = Quaternion.Euler(reader.ReadVector3());
            Scale = reader.ReadVector3();
        }
    }
}