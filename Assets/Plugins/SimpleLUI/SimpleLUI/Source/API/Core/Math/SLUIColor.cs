//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace SimpleLUI.API.Core.Math
{
    /// <summary>
    ///     Representation of RGBA colors.
    /// </summary>
    public class SLUIColor
    {
        /// <summary>
        ///     Red component of the color.
        /// </summary>
        public float r;

        /// <summary>
        ///     Green component of the color.
        /// </summary>
        public float g;

        /// <summary>
        ///     Blue component of the color.
        /// </summary>
        public float b;

        /// <summary>
        ///     Alpha component of the color (0 is transparent, 1 is opaque).
        /// </summary>
        public float a;

        /// <summary/>
        public SLUIColor() { }

        /// <summary/>
        public SLUIColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    /// <summary/>
    public static class SLUIColorUtil
    {
        /// <summary/>
        public static Color ToRealColor(this SLUIColor c)
        {
            return new Color(c.r, c.g, c.b, c.a);
        }

        /// <summary/>
        public static SLUIColor ToSLUIColor(this Color c)
        {
            return new SLUIColor(c.r, c.g, c.b, c.a);
        }
    }
}
