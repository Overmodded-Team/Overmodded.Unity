//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility methods: Bounds
    /// </summary>
    public static class JEMExtensionBounds
    {
        /// <summary>
        ///     Gets all 8 corners of bounds.
        /// </summary>
        public static Vector3[] GetCorners(this Bounds bounds) => JEMBounds.GetBoundsCorners(bounds);      
    }
}