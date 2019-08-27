//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods. Texture2D
    /// </summary>
    public static class JEMTexture2D
    {
        /// <summary>
        ///     Default extension of JEMTexture2D that has been saved using WriteToFile method.
        /// </summary>
        public static string JEMTextureFileExtension = ".jt2";

        /// <summary>
        ///     Converts this texture2D in to sprite.
        /// </summary>
        public static Sprite ToSprite(this Texture2D texture2D)
        {
            return JEMSprite.FromTexture2D(texture2D);
        }

        /// <summary>
        ///     Converts bytes in to Texture2D.
        /// </summary>
        public static Texture2D FromBytes([NotNull] byte[] bytes, bool minimap = false)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes), "Target bytes are null.");
            if (bytes.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(bytes));
            if (bytes.Length < 3)
                throw new ArgumentOutOfRangeException(nameof(bytes), bytes.Length, "Invalid texture data.");

            var bytesList = new List<byte>(bytes);

            var size = (int) bytesList[0];
            bytesList.RemoveAt(0);

            var width = BitConverter.ToInt32(bytesList.ToArray(), 0);
            var height = BitConverter.ToInt32(bytesList.ToArray(), 4);
            var format = (TextureFormat) BitConverter.ToInt16(bytesList.ToArray(), 8);

            bytesList.RemoveRange(0, size);
            var texture = new Texture2D(width, height, format, minimap);
            texture.LoadRawTextureData(bytesList.ToArray());
            texture.Apply();
            return texture;
        }

        /// <summary>
        ///     Converts Texture2D in to bytes.
        /// </summary>
        public static byte[] ToBytes([NotNull] Texture2D texture)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));
            var width = BitConverter.GetBytes(texture.width);
            var height = BitConverter.GetBytes(texture.height);
            var format = BitConverter.GetBytes((short) texture.format);
            var bytes = new List<byte>
            {
                (byte) (width.Length + height.Length + format.Length)
            };
            bytes.AddRange(width);
            bytes.AddRange(height);
            bytes.AddRange(format);
            bytes.AddRange(texture.GetRawTextureData());
            return bytes.ToArray();
        }

        /// <summary>
        ///     Writes Texture2D to file.
        /// </summary>
        public static void WriteToFile(string file, [NotNull] Texture2D texture)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));
            File.WriteAllBytes(file, ToBytes(texture));
        }

        /// <summary>
        ///     Reads Texture2D from file.
        /// </summary>
        public static Texture2D ReadFromFile(string file)
        {
            if (!File.Exists(file))
            {
                JEMLogger.InternalLogError($"Unable to read Texture2D from file. File {file} not exists.");
                return null;
            }

            return FromBytes(File.ReadAllBytes(file));
        }
    }
}