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
    // source: https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html

    /// <summary>
    ///     Set of utility extensions to Material class.
    /// </summary>
    public static class JEMExtensionMaterial
    {
        /// <summary>
        ///     Material blend mode.
        /// </summary>
        public enum BlendMode
        {
            /// <summary>
            ///     Opaque.
            /// </summary>
            Opaque,

            /// <summary>
            ///     Cutout.
            /// </summary>
            Cutout,

            /// <summary>
            ///     Fade.
            /// </summary>
            Fade,

            /// <summary>
            ///     Transparent.
            /// </summary>
            Transparent
        }

        /// <summary>
        ///     Set material blend mode.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetBlendMode([NotNull] this Material material, BlendMode blendMode)
        {
            if (material == null) throw new ArgumentNullException(nameof(material));
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetFloat("_Mode", 0f);
                    material.SetInt("_SrcBlend", (int) global::UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int) global::UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    material.SetFloat("_Mode", 1f);
                    material.SetInt("_SrcBlend", (int) global::UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int) global::UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    material.SetFloat("_Mode", 2f);
                    material.SetInt("_SrcBlend", (int) global::UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int) global::UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    material.SetFloat("_Mode", 3f);
                    material.SetInt("_SrcBlend", (int) global::UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int) global::UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blendMode), blendMode, null);
            }
        }
    }
}