//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods. Math.
    /// </summary>
    public static class JEMMath
    {
        /// <summary>
        ///     Spring function for Vector3.
        /// </summary>
        /// <param name="x">Value.</param>
        /// <param name="v">Velocity.</param>
        /// <param name="xt">Target value.</param>
        /// <param name="h">Time step.</param>
        /// <param name="zeta">Damping ratio.</param>
        /// <param name="omega">Angular frequency.</param>
        public static void SpringVector3(ref Vector3 x, ref Vector3 v, Vector3 xt, float h, float zeta = 0.23f,
            float omega = 8.0f * Mathf.PI)
        {
            var x1 = x.x;
            var x2 = x.y;
            var x3 = x.z;
            var v1 = v.x;
            var v2 = v.y;
            var v3 = v.z;

            Spring(ref x1, ref v1, xt.x, h, zeta, omega);
            Spring(ref x2, ref v2, xt.y, h, zeta, omega);
            Spring(ref x3, ref v3, xt.z, h, zeta, omega);

            x.x = x1;
            x.y = x2;
            x.z = x3;
            v.x = v1;
            v.y = v2;
            v.z = v3;
        }

        /// <summary>
        ///     Spring function for Vector2.
        /// </summary>
        /// <param name="x">Value.</param>
        /// <param name="v">Velocity.</param>
        /// <param name="xt">Target value.</param>
        /// <param name="h">Time step.</param>
        /// <param name="zeta">Damping ratio.</param>
        /// <param name="omega">Angular frequency.</param>
        public static void SpringVector2(ref Vector2 x, ref Vector2 v, Vector2 xt, float h, float zeta = 0.23f,
            float omega = 8.0f * Mathf.PI)
        {
            var x1 = x.x;
            var x2 = x.y;
            var v1 = v.x;
            var v2 = v.y;

            Spring(ref x1, ref v1, xt.x, h, zeta, omega);
            Spring(ref x2, ref v2, xt.y, h, zeta, omega);

            x.x = x1;
            x.y = x2;
            v.x = v1;
            v.y = v2;
        }

        /// <summary>
        ///     Spring function.
        /// </summary>
        /// <param name="x">Value.</param>
        /// <param name="v">Velocity.</param>
        /// <param name="xt">Target value.</param>
        /// <param name="h">Time step.</param>
        /// <param name="zeta">Damping ratio.</param>
        /// <param name="omega">Angular frequency.</param>
        public static void Spring(ref float x, ref float v, float xt, float h, float zeta = 0.23f,
            float omega = 8.0f * Mathf.PI)
        {
            var f = 1.0f + 2.0f * h * zeta * omega;
            var oo = omega * omega;
            var hoo = h * oo;
            var hhoo = h * hoo;
            var detInv = 1.0f / (f + hhoo);
            var detX = f * x + h * v + hhoo * xt;
            var detV = v + hoo * (xt - x);
            x = detX * detInv;
            v = detV * detInv;
        }
    }
}