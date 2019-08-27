//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using SimpleLUI.API.Core.Math;
using System;
using System.ComponentModel;
using UnityEngine;
using Component = UnityEngine.Component;

namespace SimpleLUI.API.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Position, rotation, scale, size, anchor and pivot information for a object.
    /// </summary>
    public sealed class SLUIRectTransform : SLUIComponent
    {
        /// <summary>
        ///     The world space position of the RectTransform.
        /// </summary>
        public SLUIVector2 position
        {
            get => Original.position.ToSLUIVector();
            set => Original.position = value.ToRealVector3();
        }

        /// <summary>
        ///     Position of the rectTransform relative to the parent rectTransform.
        /// </summary>
        public SLUIVector2 localPosition
        {
            get => Original.localPosition.ToSLUIVector();
            set => Original.localPosition = value.ToRealVector3();
        }

        /// <summary>
        ///     A quaternion that stores the rotation of the RectTransform in world space.
        /// </summary>
        public SLUIQuaternion rotation
        {
            get => Original.rotation.ToSLUIQuaternion();
            set => Original.rotation = value.ToRealQuaternion();
        }

        /// <summary>
        ///     The rotation of the rectTransform relative to the rectTransform rotation of the parent.
        /// </summary>
        public SLUIQuaternion localRotation
        {
            get => Original.localRotation.ToSLUIQuaternion();
            set => Original.localRotation = value.ToRealQuaternion();
        }

        /// <summary>
        ///     The rotation as Euler angles in degrees.
        /// </summary>
        public SLUIVector2 eulerAngles
        {
            get => Original.eulerAngles.ToSLUIVector();
            set => Original.eulerAngles = value.ToRealVector3();
        }

        /// <summary>
        ///     The rotation as Euler angles in degrees relative to the parent rectTransform's rotation.
        /// </summary>
        public SLUIVector2 localEulerAngles
        {
            get => Original.localEulerAngles.ToSLUIVector();
            set => Original.localEulerAngles = value.ToRealVector3();
        }

        /// <summary>
        ///     The scale of the rectTransform relative to the parent.
        /// </summary>
        public SLUIVector2 localScale
        {
            get => Original.localScale.ToSLUIVector();
            set => Original.localScale = value.ToRealVector3();
        }

        /// <summary>
        ///     The position of the pivot of this rectTransform relative to the anchor reference point.
        /// </summary>
        public SLUIVector2 anchoredPosition
        {
            get => Original.anchoredPosition.ToSLUIVector();
            set => Original.anchoredPosition = value.ToRealVector();
        }

        /// <summary>
        ///     The normalized position in the parent rectTransform that the upper right corner is anchored to.
        /// </summary>
        public SLUIVector2 anchorMax
        {
            get => Original.anchorMax.ToSLUIVector();
            set => Original.anchorMax = value.ToRealVector();
        }

        /// <summary>
        ///     The normalized position in the parent rectTransform that the lower left corner is anchored to.
        /// </summary>
        public SLUIVector2 anchorMin
        {
            get => Original.anchorMin.ToSLUIVector();
            set => Original.anchorMin = value.ToRealVector();
        }

        /// <summary>
        ///     The offset of the upper right corner of the rectangle relative to the upper right anchor.
        /// </summary>
        public SLUIVector2 offsetMax
        {
            get => Original.offsetMax.ToSLUIVector();
            set => Original.offsetMax = value.ToRealVector();
        }

        /// <summary>
        ///     The offset of the lower left corner of the rectangle relative to the lower left anchor.
        /// </summary>
        public SLUIVector2 offsetMin
        {
            get => Original.offsetMin.ToSLUIVector();
            set => Original.offsetMin = value.ToRealVector();
        }

        /// <summary>
        ///     The normalized position in this rectTransform that it rotates around.
        /// </summary>
        public SLUIVector2 pivot
        {
            get => Original.pivot.ToSLUIVector();
            set => Original.pivot = value.ToRealVector();
        }

        /// <summary>
        ///     The size of this rectTransform relative to the distances between the anchors.
        /// </summary>
        public SLUIVector2 sizeDelta
        {
            get => Original.sizeDelta.ToSLUIVector();
            set => Original.sizeDelta = value.ToRealVector();
        }

        /// <summary>
        ///     The calculated rectangle in the local space of the RectTransform.
        /// </summary>
        public SLUIRect rect
        {
            get => Original.rect.ToSLUIRect();
            set
            {
                var r = value;
                Original.rect.Set(r.x, r.y, r.width, r.height);
            }
        }

        /// <summary>
        ///     Anchor defines how this rectTransform's position and size is behaving to it's parent.
        /// </summary>
        public string anchor
        {
            get => GetAnchor().ToString();
            set
            {
                if (Enum.TryParse<SLUIRectAnchorName>(value, true, out var t))
                {
                    SetAnchor(t);
                }
                else Debug.LogError($"Failed to parse '{value}' in to {typeof(SLUIRectAnchorName)}");
            }
        }

        /// <summary>
        ///     The parent of the RectRransform.
        /// </summary>
        public SLUIRectTransform Parent { get; private set; }
        internal new RectTransform Original { get; private set; }

        /// <summary/>
        public SLUIRectTransform()
        {
        
        }

        /// <inheritdoc />
        public override Component OnLoadOriginalComponent()
        {
            return Original = OriginalGameObject.CollectComponent<RectTransform>();
        }

        /// <summary>
        ///     Set the parent of rectTransform.
        /// </summary>
        public void SetParent(SLUIRectTransform parent)
        {
            Parent = parent;
            Original.SetParent(parent?.Original);
        }

        /// <summary>
        ///     Get the current anchor name of rectTransform.
        /// </summary>
        public SLUIRectAnchorName GetAnchor() => GetAnchor(Original);

        /// <summary>
        ///     Set current anchor of rectTransform using index of SLUIRectAnchorName.
        /// </summary>
        public void SetAnchor(int anchorIndex) => SetAnchor(Original, (SLUIRectAnchorName) anchorIndex);

        /// <summary>
        ///     Set current anchor of rectTransform using SLUIRectAnchorName.
        /// </summary>
        public void SetAnchor(SLUIRectAnchorName anchorName) => SetAnchor(Original, anchorName);

        /// <summary>
        ///     Get current anchor name of given rectTransform.
        /// </summary>
        public static SLUIRectAnchorName GetAnchor(RectTransform rectTransform)
        {
            if (rectTransform == null) return SLUIRectAnchorName.Unknown;
            var min = rectTransform.anchorMin;
            var max = rectTransform.anchorMax;
            if (min.x == 0.0f && min.y == 1.0f && max.x == 0.0f && max.y == 1.0f)
                return SLUIRectAnchorName.TopLeft;
            if (min.x == 0.5f && min.y == 1.0f && max.x == 0.5f && max.y == 1.0f)
                return SLUIRectAnchorName.Top;
            if (min.x == 1.0f && min.y == 1.0f && max.x == 1.0f && max.y == 1.0f)
                return SLUIRectAnchorName.TopRight;
            if (min.x == 0.0f && min.y == 0.5f && max.x == 0.0f && max.y == 0.5f)
                return SLUIRectAnchorName.MiddleLeft;
            if (min.x == 0.5f && min.y == 0.5f && max.x == 0.5f && max.y == 0.5f)
                return SLUIRectAnchorName.Middle;
            if (min.x == 1.0f && min.y == 0.5f && max.x == 1.0f && max.y == 0.5f)
                return SLUIRectAnchorName.MiddleRight;
            if (min.x == 0.0f && min.y == 0.0f && max.x == 0.0f && max.y == 0.0f)
                return SLUIRectAnchorName.BottomLeft;
            if (min.x == 0.5f && min.y == 0.0f && max.x == 0.5f && max.y == 0.0f)
                return SLUIRectAnchorName.Bottom;
            if (min.x == 1.0f && min.y == 0.0f && max.x == 1.0f && max.y == 0.0f)
                return SLUIRectAnchorName.BottomRight;
            if (min.x == 0.0f && min.y == 0.0f && max.x == 0.0f && max.y == 1.0f)
                return SLUIRectAnchorName.StretchLeft;
            if (min.x == 0.5f && min.y == 0.0f && max.x == 0.5f && max.y == 1.0f)
                return SLUIRectAnchorName.StretchCenter;
            if (min.x == 1.0f && min.y == 0.0f && max.x == 1.0f && max.y == 1.0f)
                return SLUIRectAnchorName.StretchRight;
            if (min.x == 0.0f && min.y == 0.0f && max.x == 1.0f && max.y == 1.0f)
                return SLUIRectAnchorName.Stretch;
            if (min.x == 0.0f && min.y == 0.0f && max.x == 1.0f && max.y == 0.0f)
                return SLUIRectAnchorName.StretchBottom;
            if (min.x == 0.0f && min.y == 0.5f && max.x == 1.0f && max.y == 0.5f)
                return SLUIRectAnchorName.StretchMiddle;
            if (min.x == 0.0f && min.y == 1.0f && max.x == 1.0f && max.y == 1.0f)
                return SLUIRectAnchorName.StretchTop;

            return SLUIRectAnchorName.Unknown;
        }

        /// <summary>
        ///     Set current anchor name of given rectTransform.
        /// </summary>
        public static void SetAnchor([NotNull] RectTransform rectTransform, SLUIRectAnchorName anchor)
        {
            if (rectTransform == null) throw new ArgumentNullException(nameof(rectTransform));
            if (!Enum.IsDefined(typeof(SLUIRectAnchorName), anchor))
                throw new InvalidEnumArgumentException(nameof(anchor), (int) anchor, typeof(SLUIRectAnchorName));

            var min = Vector2.zero;
            var max = Vector2.zero;
            switch (anchor)
            {
                case SLUIRectAnchorName.TopLeft:
                    min.y = 1f;
                    max.y = 1f;
                    break;
                case SLUIRectAnchorName.Top:
                    min.x = 0.5f;
                    min.y = 1f;
                    max.x = 0.5f;
                    max.y = 1f;
                    break;
                case SLUIRectAnchorName.TopRight:
                    min = Vector2.one;
                    max = Vector2.one;
                    break;
                case SLUIRectAnchorName.MiddleLeft:
                    min.y = 0.5f;
                    max.y = 0.5f;
                    break;
                case SLUIRectAnchorName.Middle:
                    min.x = 0.5f;
                    min.y = 0.5f;
                    max.x = 0.5f;
                    max.y = 0.5f;
                    break;
                case SLUIRectAnchorName.MiddleRight:
                    min.x = 1f;
                    min.y = 0.5f;
                    max.x = 1f;
                    max.y = 0.5f;
                    break;
                case SLUIRectAnchorName.BottomLeft:
                    // nothing lul
                    break;
                case SLUIRectAnchorName.Bottom:
                    min.x = 0.5f;
                    max.x = 0.5f;
                    break;
                case SLUIRectAnchorName.BottomRight:
                    min.x = 1f;
                    max.x = 1f;
                    break;
                case SLUIRectAnchorName.Stretch:
                    max = Vector2.one;
                    break;
                case SLUIRectAnchorName.StretchLeft:
                    max.y = 1f;
                    break;
                case SLUIRectAnchorName.StretchCenter:
                    min.x = 0.5f;
                    max.x = 0.5f;
                    max.y = 1f;
                    break;
                case SLUIRectAnchorName.StretchRight:
                    min.x = 1f;
                    max.x = 1f;
                    max.y = 1f;
                    break;
                case SLUIRectAnchorName.StretchBottom:
                    max.x = 1f;
                    break;
                case SLUIRectAnchorName.StretchMiddle:
                    min.y = 0.5f;
                    max.x = 1f;
                    max.y = 0.5f;
                    break;
                case SLUIRectAnchorName.StretchTop:
                    min.y = 1f;
                    max.x = 1f;
                    max.y = 1f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null);
            }

            rectTransform.anchorMin = min;
            rectTransform.anchorMax = max;
        }
    }
}
