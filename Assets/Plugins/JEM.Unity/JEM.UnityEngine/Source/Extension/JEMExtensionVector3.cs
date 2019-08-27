//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility methods: Vector3
    /// </summary>
    public static class JEMExtensionVector3
    {
        /// <summary>
        ///     Compare distance between point A + B and A + C.
        /// </summary>
        public static float CompareDistance(Vector3 pointA, Vector3 pointB, Vector3 pointC)
        {
            var s1 = Vector3.Distance(pointA, pointB);
            var s2 = Vector3.Distance(pointA, pointC);
            return Mathf.Abs(s1 - s2);
        }

        /// <summary>
        ///     Multiply two vectors axis by axis.
        /// </summary>
        public static Vector3 Multiply(Vector3 vectorA, Vector3 vectorB) => new Vector3(vectorA.x * vectorB.x, vectorA.y * vectorB.y, vectorA.z * vectorB.z);
        
        /// <summary>
        ///     Snap a Vector3.
        /// </summary>
        public static Vector3 Snap(this Vector3 v, float value) => new Vector3(v.x - v.x % value, v.y - v.y % value, v.z - v.z % value);  

        /// <summary/>
        public static Vector3 Horizontal(this Vector3 vector) => new Vector3(vector.x, 0.0f, vector.z);
        
        /// <summary/>
        public static Vector3 Vertical(this Vector3 vector) => new Vector3(0.0f, vector.y, 0.0f);    

        // TODO: Move to JEMExtensionVector2.
        /// <summary>
        ///     Snap a Vector2.
        /// </summary>
        public static Vector2 Snap(this Vector2 v, float value)
        {
            return new Vector2(v.x - v.x % value, v.y - v.y % value);
        }
    }
}
