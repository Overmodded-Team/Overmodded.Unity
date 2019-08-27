//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.API.Core;
using SimpleLUI.API.Core.Math;
using System.IO;
using UnityEngine;

namespace SimpleLUI.Editor.Util
{
    public static class SLUIResourcesConverter
    {
        public static void WriteSprite(string file, Sprite sprite)
        {
            if (File.Exists(file))
                return;

            var t = sprite.texture;
            if (sprite.texture.isReadable)
            {
                var n = t.EncodeToPNG();
                File.WriteAllBytes(file, n);
            }
            else
            {
                // Create a temporary RenderTexture of the same size as the texture
                RenderTexture tmp = RenderTexture.GetTemporary(
                    t.width,
                    t.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

                // Blit the pixels on texture to the RenderTexture
                Graphics.Blit(t, tmp);

                // Backup the currently set RenderTexture
                RenderTexture previous = RenderTexture.active;

                // Set the current RenderTexture to the temporary one we created
                RenderTexture.active = tmp;

                // Create a new readable Texture2D to copy the pixels to it
                Texture2D myTexture2D = new Texture2D(t.width, t.height);

                // Copy the pixels from the RenderTexture to the new Texture
                myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                myTexture2D.Apply();

                // Reset the active RenderTexture
                RenderTexture.active = previous;

                // Release the temporary RenderTexture
                RenderTexture.ReleaseTemporary(tmp);

                var n = myTexture2D.EncodeToPNG();
                File.WriteAllBytes(file, n);

                Debug.Log($"resource saved at " + file);
            }

            var styleFile = file + ".style";
            var style = new SLUIImageStyle
            {
                Pivot = sprite.pivot.ToSLUIVector(),
                PixelPerUnit = sprite.pixelsPerUnit,
                Border = new SLUIQuaternion(sprite.border.x, sprite.border.y, sprite.border.z, sprite.border.w)
            };

            File.WriteAllText(styleFile, JsonUtility.ToJson(style, true));
        }

        public static string CollectResourceName(Sprite sprite)
        {
            return $"{sprite.texture.name}.png";
        }
    }
}
