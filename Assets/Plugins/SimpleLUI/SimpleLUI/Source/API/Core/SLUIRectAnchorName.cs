//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

namespace SimpleLUI.API.Core
{
    /// <summary>
    ///     RectAnchorName is used to define how rectTransform's position and size should behave to it's parent by default.
    /// </summary>
    public enum SLUIRectAnchorName
    {
        /// <summary>
        ///     Unknown anchor that is most likely custom defined by user. 
        /// </summary>
        Unknown,

        /// <summary>
        ///     Anchored to top left of parent.
        /// </summary>
        TopLeft,

        /// <summary>
        ///     Anchored to top of parent.
        /// </summary>
        Top,

        /// <summary>
        ///     Anchored to top right of parent.
        /// </summary>
        TopRight,

        /// <summary>
        ///     Anchored to middle left of parent.
        /// </summary>
        MiddleLeft,

        /// <summary>
        ///     Anchored to middle of parent.
        /// </summary>
        Middle,

        /// <summary>
        ///     Anchored to middle right left of parent.
        /// </summary>
        MiddleRight,

        /// <summary>
        ///     Anchored to bottom left left of parent.
        /// </summary>
        BottomLeft,

        /// <summary>
        ///     Anchored to bottom of parent.
        /// </summary>
        Bottom,

        /// <summary>
        ///     Anchored to bottom right of parent.
        /// </summary>
        BottomRight,

        /// <summary>
        ///     Stretch on parent.
        /// </summary>
        Stretch,

        /// <summary>
        ///     Stretch to left of parent.
        /// </summary>
        StretchLeft,

        /// <summary>
        ///     Stretch to center of parent.
        /// </summary>
        StretchCenter,

        /// <summary>
        ///     Stretch to right of parent.
        /// </summary>
        StretchRight,

        /// <summary>
        ///     Stretch to bottom of parent.
        /// </summary>
        StretchBottom,

        /// <summary>
        ///     Stretch to middle of parent.
        /// </summary>
        StretchMiddle,

        /// <summary>
        ///     Stretch to top of parent.
        /// </summary>
        StretchTop
    }
}