//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.API.Core.Math;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SimpleLUI.API.Core
{
    /// <summary>
    ///     Import settings for image's sprites.
    ///     Image style should always be bundled with your image.
    ///         For ex. If you want to define style for unity.jpg you should create unity.jpg.style
    ///     Style data is loaded via JSON!
    /// </summary>
    [Serializable]
    public class SLUIImageStyle
    {
        /// <summary>
        ///     Location of the Sprite's center point in the Rect on the original Texture, specified in pixels.
        /// </summary>
        public SLUIVector2 Pivot = new SLUIVector2(0.5f, 0.5f);

        /// <summary>
        ///     The number of pixels in the sprite that correspond to one unit in world space.
        /// </summary>
        public float PixelPerUnit = 100f;

        /// <summary>
        ///     Returns the border sizes of the sprite.
        /// </summary>
        public SLUIQuaternion Border = new SLUIQuaternion(0f, 0f, 0f, 0f);

        /// <summary>
        ///     Amount by which the sprite mesh should be expanded outwards.
        /// </summary>
        public uint Extrude = 1;

        /// <summary>
        ///     Controls the type of mesh generated for the sprite.
        /// </summary>
        public SpriteMeshType MeshType = SpriteMeshType.Tight;
    }

    /// <summary>
    ///     Displays a Sprite for the UI System.
    /// </summary>
    public sealed class SLUIImage : SLUIMaskableGraphic
    {
        private string _sprite;

        /// <summary>
        ///     The sprite that is used to render this image.
        /// </summary>
        public string sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                if (!sprite.Contains(":"))
                {
                    sprite = $"{Manager.Directory}//{sprite}";
                }

                Manager.CanvasHelper.StartCoroutine(LoadFile(sprite, Original));
            }
        }

        /// <summary>
        ///     Whether this image should preserve its Sprite aspect ratio.
        /// </summary>
        public bool preserveAspect
        {
            get => Original.preserveAspect;
            set => Original.preserveAspect = value;
        }

        /// <summary>
        ///     Whether or not to render the center of a Tiled or Sliced image.
        /// </summary>
        public bool fillCenter
        {
            get => Original.fillCenter;
            set => Original.fillCenter = value;
        }

        /// <summary>
        ///     How to display the image.
        /// </summary>
        public string imageType
        {
            get => Original.type.ToString();
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (Enum.TryParse<Image.Type>(value, true, out var t))
                {
                    SetType(t);
                }
                else Debug.LogError($"Failed to parse '{value}' in to {typeof(Image.Type)}");
            }
        }

        internal new Image Original { get; private set; }

        /// <summary/>
        public SLUIImage() { }

        /// <inheritdoc />
        public override Component OnLoadOriginalComponent()
        {
            return Original = OriginalGameObject.CollectComponent<Image>();
        }

        /// <summary>
        ///     Sets the image type using enum.
        /// </summary>
        public void SetType(Image.Type type)
        {
            Original.type = type;
        }

        private static IEnumerator LoadFile(string sprite, Image i)
        {
            var url = $"file:///{sprite}";
            url = url.Replace("\\", "//");

            using (var r = UnityWebRequestTexture.GetTexture(url))
            {
                yield return r.SendWebRequest();

                if (r.isNetworkError || r.isHttpError)
                    Debug.LogError($"Failed to load {url}. {r.error}");
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(r);
                    var styleFile = sprite + ".style";
                    var style = new SLUIImageStyle();
                    if (File.Exists(styleFile))
                    {
                        style = JsonUtility.FromJson<SLUIImageStyle>(File.ReadAllText(styleFile));
                    }

                    i.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                        style.Pivot.ToRealVector(), style.PixelPerUnit, style.Extrude, style.MeshType,
                        new Vector4(style.Border.x, style.Border.y, style.Border.z, style.Border.w));
                    i.sprite.name = Path.GetFileNameWithoutExtension(sprite) ?? throw new InvalidOperationException();

                    yield return i.sprite;
                }
            }
        }
    }
}
