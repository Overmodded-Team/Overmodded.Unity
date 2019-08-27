//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleLUI.API.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     The default Graphic to draw font data to screen.
    /// </summary>
    public sealed class SLUIText : SLUIMaskableGraphic
    {
        /// <summary>
        ///     The string value this Text displays.
        /// </summary>
        public string text
        {
            get => Original.text;
            set => Original.text = value;
        }

        /// <summary>
        ///     The size that the Font should render at.
        /// </summary>
        public int fontSize
        {
            get => Original.fontSize;
            set => Original.fontSize = value;
        }

        /// <summary>
        ///     FontStyle used by the text.
        /// </summary>
        public string fontStyle
        {
            get => Original.fontStyle.ToString();
            set
            {
                if (Enum.TryParse<SLUIFontStyle>(value, true, out var t))
                {
                    SetFontStyle(t);
                }
                else Debug.LogError($"Failed to parse '{value}' in to {typeof(SLUIFontStyle)}");
            }
        }

        /// <summary>
        ///     The positioning of the text reliative to its RectTransform.
        /// </summary>
        public string alignment
        {
            get => Original.alignment.ToString();
            set
            {
                if (Enum.TryParse<SLUITextAnchor>(value, true, out var t))
                {
                    SetAlignment(t);
                }
                else Debug.LogError($"Failed to parse '{value}' in to {typeof(SLUITextAnchor)}");
            }
        }

        /// <summary>
        ///     Should the text be allowed to auto resized.
        /// </summary>
        public bool resizeTextForBestFit
        {
            get => Original.resizeTextForBestFit;
            set => Original.resizeTextForBestFit = value;
        }

        /// <summary>
        ///     	The minimum size the text is allowed to be.
        /// </summary>
        public int resizeTextMinSize
        {
            get => Original.resizeTextMinSize;
            set => Original.resizeTextMinSize = value;
        }

        /// <summary>
        ///     The maximum size the text is allowed to be. 1 = infinitly large.
        /// </summary>
        public int resizeTextMaxSize
        {
            get => Original.resizeTextMaxSize;
            set => Original.resizeTextMaxSize = value;
        }

        internal new Text Original { get; private set; }

        /// <inheritdoc />
        public override Component OnLoadOriginalComponent()
        {
            return Original = OriginalGameObject.CollectComponent<Text>();
        }

        /// <inheritdoc />
        public override void OnComponentLoaded()
        {
            // invoke base method
            base.OnComponentLoaded();

            // default
            Original.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        }

        /// <summary>
        ///     Set the font style using enum.
        /// </summary>
        public void SetFontStyle(SLUIFontStyle s)
        {
            Original.fontStyle = (FontStyle) s;
        }

        /// <summary>
        ///     Set the text alignment using enum.
        /// </summary>
        public void SetAlignment(SLUITextAnchor a)
        {
            Original.alignment = (TextAnchor) a;
        }
    }
}
