//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods. Bounds
    /// </summary>
    public static class JEMBounds
    {
        /// <summary>
        ///     Gets bounds from array of renderer based components.
        /// </summary>
        public static Bounds BoundsFromMeshRenderers<T>(T[] renderers) where T : Renderer
        {
            if (renderers == null || renderers.Length == 0)
                return default(Bounds);

            var bounds = renderers[0].bounds;
            if (bounds == default(Bounds))
                return default(Bounds);

            for (var index = 1; index < renderers.Length; index++)
            {
                var r = renderers[index];
                if (r == null)
                    continue;
                bounds.Encapsulate(r.bounds.min);
                bounds.Encapsulate(r.bounds.max);
            }

            return bounds;
        }

        /// <summary>
        ///     Gets bounds from array of collider based components.
        /// </summary>
        public static Bounds BoundsFromColliders<T>(T[] colliders) where T : Collider
        {
            if (colliders == null || colliders.Length == 0)
                return default(Bounds);

            var bounds = colliders[0].bounds;
            if (bounds == default(Bounds))
                return default(Bounds);

            for (var index = 1; index < colliders.Length; index++)
            {
                var r = colliders[index];
                if (r == null)
                    continue;
                bounds.Encapsulate(r.bounds.min);
                bounds.Encapsulate(r.bounds.max);
            }

            return bounds;
        }

        /// <summary>
        ///     Gets all 8 corners of bounds.
        /// </summary>
        public static Vector3[] GetBoundsCorners(Bounds bounds)
        {
            var c = new Vector3[8];
            c[0] = bounds.min;
            c[1] = bounds.max;
            c[2] = new Vector3(c[1].x, c[1].y, c[2].z);
            c[3] = new Vector3(c[1].x, c[2].y, c[1].z);
            c[4] = new Vector3(c[2].x, c[1].y, c[1].z);
            c[5] = new Vector3(c[1].x, c[2].y, c[2].z);
            c[6] = new Vector3(c[2].x, c[1].y, c[2].z);
            c[7] = new Vector3(c[2].x, c[2].y, c[1].z);
            return c;
        }
    }
}