//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to Renderer class.
    /// </summary>
    public static class JEMExtensionRenderer
    {
        /// <summary>
        ///     Sets all materials of renderer to given.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetMaterials([NotNull] this Renderer renderer, Material material)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));

            var materials = renderer.materials;
            for (var index = 0; index < materials.Length; index++) materials[index] = material;
            renderer.materials = materials;
        }
    }
}