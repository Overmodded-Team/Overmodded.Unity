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
    ///     Base class for all visual UI Component.
    /// </summary>
    public abstract class SLUIGraphic : SLUIComponent
    {
        /// <summary>
        ///     Base color of the Graphic.
        /// </summary>
        public SLUIColor color
        {
            get => Original.color.ToSLUIColor();
            set => Original.color = value.ToRealColor();
        }

        /// <summary>
        ///     Should this graphic be considered a target for raycasting?
        /// </summary>
        public bool raycastTarget
        {
            get => Original.raycastTarget;
            set => Original.raycastTarget = value;
        }

        internal new Graphic Original { get; private set; }

        /// <inheritdoc />
        public override void OnComponentLoaded()
        {
            Original = (Graphic) OriginalComponent;
        }
    }
}
