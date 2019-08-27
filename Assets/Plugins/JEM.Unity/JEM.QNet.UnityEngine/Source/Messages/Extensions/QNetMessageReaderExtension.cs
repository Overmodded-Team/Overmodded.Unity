//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Messages.Extensions
{
    /// <summary>
    ///     Extension method to QNet message reader.
    /// </summary>
    public static class QNetMessageReaderExtension
    {
        /// <summary>
        ///     Reads vector3.
        /// </summary>
        public static Vector3 ReadVector3(this QNetMessageReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        ///     Reads vector4.
        /// </summary>
        public static Vector4 ReadVector4(this QNetMessageReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        ///     Reads vector2.
        /// </summary>
        public static Vector2 ReadVector2(this QNetMessageReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        ///     Reads fixed vector3.
        /// </summary>
        public static Vector3 ReadFixedVector3(this QNetMessageReader reader, Vector3 previousVector3)
        {
            var flag = (NetworkVectorAxis) reader.ReadByte();
            var x = previousVector3.x;
            var y = previousVector3.y;
            var z = previousVector3.z;
            if (flag.HasFlag(NetworkVectorAxis.X))
                x = reader.ReadSingle();
            if (flag.HasFlag(NetworkVectorAxis.Y))
                y = reader.ReadSingle();
            if (flag.HasFlag(NetworkVectorAxis.Z))
                z = reader.ReadSingle();
            return new Vector3(x, y, z);
        }
    }
}