//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor
{
    public static partial class JEMBetterEditor
    {
        /// <summary>
        ///     Gets fixed property name.
        /// </summary>
        public static string FixedPropertyName(string name)
        {
            if (name == null)
                return string.Empty;
            if (name.Length <= 2)
                return name;

            for (var index = 1; index < name.Length; index++)
            {
                var c = name[index];
                var p = name[index - 1];

                if (!char.IsWhiteSpace(p) && !char.IsWhiteSpace(c) && !char.IsUpper(p) && char.IsUpper(c))
                {
                    name = name.Insert(index, " ");
                    index++;
                }
            }

            return name;
        }

        /// <summary/>
        public static void FixedEndFadeGroup(float aValue)
        {
            if (Math.Abs(aValue) < float.Epsilon || Math.Abs(aValue - 1f) < float.Epsilon)
                return;
            EditorGUILayout.EndFadeGroup();
        }

        /// <summary>
        ///     Draw a sprite.
        /// </summary>
        public static void DrawSprite(Sprite aSprite, float size = 0f)
        {
            if (aSprite == null)
                return;

            var c = aSprite.rect;
            var spriteW = Math.Abs(size) < float.Epsilon ? c.width : size;
            var spriteH = Math.Abs(size) < float.Epsilon ? c.height : size;
            var rect = GUILayoutUtility.GetRect(spriteW, spriteH);
            if (Event.current.type != EventType.Repaint)
                return;

            var tex = aSprite.texture;
            c.xMin /= tex.width;
            c.xMax /= tex.width;
            c.yMin /= tex.height;
            c.yMax /= tex.height;
            GUI.DrawTextureWithTexCoords(rect, tex, c);
        }
    }
}
