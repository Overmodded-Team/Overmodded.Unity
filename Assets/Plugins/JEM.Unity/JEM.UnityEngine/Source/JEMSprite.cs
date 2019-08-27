//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods. Sprite
    /// </summary>
    public static class JEMSprite
    {
        /// <summary>
        ///     Creates sprite from texture2D.
        /// </summary>
        /// <param name="texture2D"></param>
        /// <returns></returns>
        public static Sprite FromTexture2D(Texture2D texture2D)
        {
            return FromTexture2D(texture2D, new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        ///     Creates sprite from texture2D.
        /// </summary>
        /// <param name="texture2D"></param>
        /// <param name="pivot"></param>
        /// <returns></returns>
        public static Sprite FromTexture2D(Texture2D texture2D, Vector2 pivot)
        {
            return FromTexture2D(texture2D, pivot, 100f);
        }

        /// <summary>
        ///     Creates sprite from texture2D.
        /// </summary>
        /// <param name="texture2D"></param>
        /// <param name="pivot"></param>
        /// <param name="pixelsPerUnit"></param>
        /// <returns></returns>
        public static Sprite FromTexture2D([NotNull] Texture2D texture2D, Vector2 pivot, float pixelsPerUnit)
        {
            if (texture2D == null) throw new ArgumentNullException(nameof(texture2D), "Target texture is null.");
            return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), pivot, pixelsPerUnit);
        }
    }
}