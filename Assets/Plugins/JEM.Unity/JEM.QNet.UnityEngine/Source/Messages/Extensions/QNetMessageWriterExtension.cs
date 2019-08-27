//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using System;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Messages.Extensions
{
    /// <summary>
    ///     Vector3 network axis.
    /// </summary>
    [Flags]
    public enum NetworkVectorAxis : byte
    {
        /// <summary/>
        None,

        /// <summary/>
        X,

        /// <summary/>
        Y,

        /// <summary/>
        Z
    }

    /// <summary>
    ///     Extension method to QNet message writer.
    /// </summary>
    public static class QNetMessageWriterExtension
    {
        /// <summary>
        ///     Writes vector3.
        /// </summary>
        public static void WriteVector3(this QNetMessageWriter writer, Vector3 vector)
        {
            writer.WriteSingle(vector.x);
            writer.WriteSingle(vector.y);
            writer.WriteSingle(vector.z);
        }

        /// <summary>
        ///     Writes vector4.
        /// </summary>
        public static void WriteVector4(this QNetMessageWriter writer, Vector4 vector)
        {
            writer.WriteSingle(vector.x);
            writer.WriteSingle(vector.y);
            writer.WriteSingle(vector.z);
            writer.WriteSingle(vector.w);
        }

        /// <summary>
        ///     Writes vector2.
        /// </summary>
        public static void WriteVector2(this QNetMessageWriter writer, Vector2 vector)
        {
            writer.WriteSingle(vector.x);
            writer.WriteSingle(vector.y);
        }

        /// <summary>
        ///     Writes vector3.
        /// </summary>
        public static void WriteFixedVector3(this QNetMessageWriter writer, Vector3 previous, Vector3 vector,
            float step = 0.1f)
        {
            var flag = NetworkVectorAxis.None;
            if (Mathf.Abs(previous.x - vector.x) > step)
                flag |= NetworkVectorAxis.X;

            if (Mathf.Abs(previous.y - vector.y) > step)
                flag |= NetworkVectorAxis.Y;

            if (Mathf.Abs(previous.z - vector.z) > step)
                flag |= NetworkVectorAxis.Z;

            writer.WriteByte((byte) flag);
            if (flag.HasFlag(NetworkVectorAxis.X))
                writer.WriteSingle(vector.x);
            if (flag.HasFlag(NetworkVectorAxis.Y))
                writer.WriteSingle(vector.y);
            if (flag.HasFlag(NetworkVectorAxis.Z))
                writer.WriteSingle(vector.z);
        }
    }
}