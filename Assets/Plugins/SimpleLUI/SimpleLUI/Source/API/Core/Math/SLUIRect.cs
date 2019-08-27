//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace SimpleLUI.API.Core.Math
{
    /// <summary>
    ///     A 2D Rectangle defined by X and Y position, width and height.
    /// </summary>
    public class SLUIRect
    {
        /// <summary>
        ///     The X coordinate of the rectangle.
        /// </summary>
        public float x;

        /// <summary>
        ///     The Y coordinate of the rectangle.
        /// </summary>
        public float y;

        /// <summary>
        ///     The height of the rectangle, measured from the Y position.
        /// </summary>
        public float height;

        /// <summary>
        ///    The width of the rectangle, measured from the X position. 
        /// </summary>
        public float width;

        /// <summary/>
        public SLUIRect() { }

        /// <summary/>
        public SLUIRect(float x, float y, float height, float width)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }
    }

    /// <summary/>
    public static class SLUIRectUtil
    {
        /// <summary/>
        public static Rect ToRealRect(this SLUIRect r)
        {
            return new Rect(r.x, r.y, r.width, r.height);
        }

        /// <summary/>
        public static SLUIRect ToSLUIRect(this Rect r)
        {
            return new SLUIRect(r.x, r.y, r.height, r.width);
        }
    }
}
