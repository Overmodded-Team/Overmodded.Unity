//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Interface.Window
{
    /// <summary>
    ///     Interface window anchor name.
    /// </summary>
    public enum JEMInterfaceWindowAnchorName
    {
#pragma warning disable 1591
        Unknown,
        TopLeft,
        Top,
        TopRight,
        MiddleLeft,
        Middle,
        MiddleRight,
        BottomLeft,
        Bottom,
        BottomRight
#pragma warning restore 1591
    }

    /// <summary>
    ///     Utility class for interface window.
    /// </summary>
    public static class JEMInterfaceWindowUtil
    {
        /// <summary>
        ///     Get current anchor name of given rectTransform.
        /// </summary>
        public static JEMInterfaceWindowAnchorName GetAnchorName(RectTransform rectTransform)
        {
            if (rectTransform == null) return JEMInterfaceWindowAnchorName.Unknown;
            var min = rectTransform.anchorMin;
            var max = rectTransform.anchorMax;
            if (min.x == 0.0f && min.y == 1.0f && max.x == 0.0f && min.y == 1.0f)
                return JEMInterfaceWindowAnchorName.TopLeft;
            if (min.x == 0.5f && min.y == 1.0f && max.x == 0.5f && min.y == 1.0f)
                return JEMInterfaceWindowAnchorName.Top;
            if (min.x == 1.0f && min.y == 1.0f && max.x == 1.0f && min.y == 1.0f)
                return JEMInterfaceWindowAnchorName.TopRight;
            if (min.x == 0.0f && min.y == 0.5f && max.x == 0.0f && min.y == 0.5f)
                return JEMInterfaceWindowAnchorName.MiddleLeft;
            if (min.x == 0.5f && min.y == 0.5f && max.x == 0.5f && min.y == 0.5f)
                return JEMInterfaceWindowAnchorName.Middle;
            if (min.x == 1.0f && min.y == 0.5f && max.x == 1.0f && min.y == 0.5f)
                return JEMInterfaceWindowAnchorName.MiddleRight;
            if (min.x == 0.0f && min.y == 0.0f && max.x == 0.0f && min.y == 0.0f)
                return JEMInterfaceWindowAnchorName.BottomLeft;
            if (min.x == 0.5f && min.y == 0.0f && max.x == 0.5f && min.y == 0.0f)
                return JEMInterfaceWindowAnchorName.Bottom;
            if (min.x == 1.0f && min.y == 0.0f && max.x == 1.0f && min.y == 0.0f)
                return JEMInterfaceWindowAnchorName.BottomRight;

            return JEMInterfaceWindowAnchorName.Unknown;
        }
    }
}