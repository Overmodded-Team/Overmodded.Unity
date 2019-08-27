//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace SimpleLUI.API.Core.Math
{
    /// <summary>
    ///     Quaternions are used to represent rotations.
    /// </summary>
    [Serializable]
    public class SLUIQuaternion
    {
        /// <summary>
        ///     X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        /// </summary>
        public float x;

        /// <summary>
        ///     Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        /// </summary>
        public float y;

        /// <summary>
        ///     Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        /// </summary>
        public float z;

        /// <summary>
        ///     W component of the Quaternion. Do not directly modify quaternions.
        /// </summary>
        public float w;

        /// <summary/>
        public SLUIQuaternion() { }

        /// <summary/>
        public SLUIQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    /// <summary/>
    public static class SLUIQuaternionUtil
    {
        /// <summary/>
        public static Quaternion ToRealQuaternion(this SLUIQuaternion q)
        {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }

        /// <summary/>
        public static SLUIQuaternion ToSLUIQuaternion(this Quaternion q)
        {
            return new SLUIQuaternion(q.x, q.y, q.z, q.w);
        }
    }
}
