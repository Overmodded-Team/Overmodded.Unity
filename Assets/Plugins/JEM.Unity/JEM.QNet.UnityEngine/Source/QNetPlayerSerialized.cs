//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;

namespace JEM.QNet.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Serialized network data of QNetPlayer to make.
    /// </summary>
    public class QNetPlayerSerialized : QNetSerializedMessage
    {
        /// <summary>
        ///     Identity of connection.
        /// </summary>
        public short ConnectionIdentity;

        /// <summary>
        ///     Name of connection.
        /// </summary>
        public string NickName;

        /// <inheritdoc />
        public override void Serialize(QNetMessageWriter writer)
        {
            writer.WriteInt16(ConnectionIdentity);
            writer.WriteString(NickName);
        }

        /// <inheritdoc />
        public override void DeSerialize(QNetMessageReader reader)
        {
            ConnectionIdentity = reader.ReadInt16();
            NickName = reader.ReadString();
        }
    }
}