//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Bezier renderer script.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class JEMBezierRenderer : MonoBehaviour
    {
        /// <summary>
        ///     Transform of point A.
        /// </summary>
        [Header("Settings")]
        public Transform PointA;

        /// <summary>
        ///     Transform of point B.
        /// </summary>
        public Transform PointB;

        /// <summary>
        ///     Segment count.
        /// </summary>
        public int SegmentCount = 50;

        /// <summary>
        ///     Defines whether the bezier should be updated in edit mode.
        /// </summary>
        [Header("Debug Settings")]
        public bool UpdateInEditMode;

        /// <summary>
        ///     Left tangent.
        /// </summary>
        public Vector3 LeftTangent = Vector3.right * 50;

        /// <summary>
        ///     Right tangent.
        /// </summary>
        public Vector3 RightTangent = Vector3.left * 50;

        private LineRenderer LineRenderer { get; set; }
        private Vector3 _lastA;
        private Vector3 _lastB;

        private void LateUpdate()
        {
            if (!Application.isPlaying && !UpdateInEditMode) return;

            UpdateBezier();
        }

        private void UpdateBezier()
        {
            if (PointA == null || PointB == null || PointA == PointB) return;
            if (LineRenderer == null) LineRenderer = GetComponent<LineRenderer>();
            var isActive = PointA.gameObject.activeInHierarchy || PointB.gameObject.activeInHierarchy;
            if (isActive)
            {
                LineRenderer.enabled = true;
            }
            else
            {
                LineRenderer.enabled = false;
                return;
            }

            if (_lastA == PointA.position && _lastB == PointB.position) return;
            _lastA = PointA.position;
            _lastB = PointB.position;
            LineRenderer.useWorldSpace = true;
            LineRenderer.loop = false;

            var startPos = PointA.position;
            var endPos = PointB.position;
            var startTan = startPos + LeftTangent;
            var endTan = endPos + RightTangent;

            LineRenderer.positionCount = SegmentCount + 1;
            LineRenderer.SetPosition(0, startPos);

            for (var i = 0; i < SegmentCount; i++)
            {
                var t = i / (float) SegmentCount;
                var pixel = CalculateCubicBezierPoint(t, startPos, endPos, startTan, endTan);
                LineRenderer.SetPosition(i, pixel);
            }

            LineRenderer.SetPosition(SegmentCount, endPos);
        }

        private static Vector3 CalculateCubicBezierPoint(float time, Vector3 startPosition, Vector3 endPosition,
            Vector3 startTangent, Vector3 endTangent)
        {
            var u = 1 - time;
            var tt = time * time;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * time;

            var p = uuu * startPosition;
            p += 3 * uu * time * startTangent;
            p += 3 * u * tt * endTangent;
            p += ttt * endPosition;

            return p;
        }
    }
}