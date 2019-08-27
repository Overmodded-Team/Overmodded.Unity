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
    ///     Representation of 2D vectors and points.
    /// </summary>
    [Serializable]
    public class SLUIVector2
    {
        /// <summary>
        ///     X component of the vector.
        /// </summary>
        public float x;

        /// <summary>
        ///     Y component of the vector.     
        /// </summary>
        public float y;

        /// <summary/>
        public SLUIVector2() { }

        /// <summary/>
        public SLUIVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///     Shorthand for writing SLUIVector2(0, 0).
        /// </summary>
        public static SLUIVector2 Zero => new SLUIVector2(0f, 0f);
    }

    /// <summary/>
    public static class SLUIVector2Util
    {
        /// <summary/>
        public static Vector2 ToRealVector(this SLUIVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary/>
        public static Vector3 ToRealVector3(this SLUIVector2 v)
        {
            return new Vector3(v.x, v.y, 0f);
        }

        /// <summary/>
        public static SLUIVector2 ToSLUIVector(this Vector2 v)
        {
            return new SLUIVector2(v.x, v.y);
        }

        /// <summary/>
        public static SLUIVector2 ToSLUIVector(this Vector3 v)
        {
            return new SLUIVector2(v.x, v.y);
        }
    }
}
