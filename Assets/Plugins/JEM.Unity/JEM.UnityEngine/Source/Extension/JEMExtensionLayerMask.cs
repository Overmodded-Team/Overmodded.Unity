//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to LayerMask class.
    /// </summary>
    public static class JEMExtensionLayerMask
    {
        /// <summary>
        ///     Check if given layer exists in this LayerMask.
        /// </summary>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}