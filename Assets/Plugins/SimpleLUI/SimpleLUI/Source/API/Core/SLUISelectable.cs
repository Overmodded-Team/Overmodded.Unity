//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.API.Core.Math;
using UnityEngine.UI;

namespace SimpleLUI.API.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple selectable object - derived from to create a selectable control.
    /// </summary>
    public abstract class SLUISelectable : SLUIComponent
    {
        /// <summary>
        ///     Use to enable or disable the ability to select a selectable UI element (for example, a Button).
        /// </summary>
        public bool interactable
        {
            get => Original.interactable;
            set => Original.interactable = value;
        }

        /// <summary>
        ///     Graphic that will be transitioned upon.
        /// </summary>
        public SLUIImage targetGraphic
        {
            get => Original.image == null ? null : Core.GetObject<SLUIImage>(Original.image.GetInstanceID());
            set => Original.image = value?.Original;
        }

        /// <summary>
        ///     Normal Color.
        /// </summary>
        public SLUIColor normalColor
        {
            get => Original.colors.normalColor.ToSLUIColor();
            set => Original.colors = new ColorBlock()
            {
                normalColor = value.ToRealColor(),
                highlightedColor = Original.colors.highlightedColor,
                pressedColor = Original.colors.pressedColor,
                selectedColor = Original.colors.selectedColor,
                disabledColor = Original.colors.disabledColor,
                colorMultiplier = Original.colors.colorMultiplier,
                fadeDuration = Original.colors.fadeDuration
            };
        }

        /// <summary>
        ///     Highlighted Color.
        /// </summary>
        public SLUIColor highlightedColor
        {
            get => Original.colors.normalColor.ToSLUIColor();
            set => Original.colors = new ColorBlock()
            {
                normalColor = Original.colors.normalColor,
                highlightedColor = value.ToRealColor(),
                pressedColor = Original.colors.pressedColor,
                selectedColor = Original.colors.selectedColor,
                disabledColor = Original.colors.disabledColor,
                colorMultiplier = Original.colors.colorMultiplier,
                fadeDuration = Original.colors.fadeDuration
            };
        }

        /// <summary>
        ///     Pressed Color.
        /// </summary>
        public SLUIColor pressedColor
        {
            get => Original.colors.normalColor.ToSLUIColor();
            set => Original.colors = new ColorBlock()
            {
                normalColor = Original.colors.normalColor,
                highlightedColor = Original.colors.highlightedColor,
                pressedColor = value.ToRealColor(),
                selectedColor = Original.colors.selectedColor,
                disabledColor = Original.colors.disabledColor,
                colorMultiplier = Original.colors.colorMultiplier,
                fadeDuration = Original.colors.fadeDuration
            };
        }

        /// <summary>
        ///     Selected Color.
        /// </summary>
        public SLUIColor selectedColor
        {
            get => Original.colors.normalColor.ToSLUIColor();
            set => Original.colors = new ColorBlock()
            {
                normalColor = Original.colors.normalColor,
                highlightedColor = Original.colors.highlightedColor,
                pressedColor = Original.colors.pressedColor,
                selectedColor = value.ToRealColor(),
                disabledColor = Original.colors.disabledColor,
                colorMultiplier = Original.colors.colorMultiplier,
                fadeDuration = Original.colors.fadeDuration
            };
        }

        /// <summary>
        ///     Disabled Color.
        /// </summary>
        public SLUIColor disabledColor
        {
            get => Original.colors.normalColor.ToSLUIColor();
            set => Original.colors = new ColorBlock()
            {
                normalColor = Original.colors.normalColor,
                highlightedColor = Original.colors.highlightedColor,
                pressedColor = Original.colors.pressedColor,
                selectedColor = Original.colors.selectedColor,
                disabledColor = value.ToRealColor(),
                colorMultiplier = Original.colors.colorMultiplier,
                fadeDuration = Original.colors.fadeDuration
            };
        }

        internal new Selectable Original { get; private set; }

        /// <inheritdoc />
        public override void OnComponentLoaded()
        {
            Original = (Selectable) OriginalComponent;
        }
    }
}
