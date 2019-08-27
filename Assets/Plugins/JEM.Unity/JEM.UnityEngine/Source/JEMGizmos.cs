//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods for in Editor Gizmos.
    /// </summary>
    public static class JEMGizmos
    {
        /// <summary>
        ///     Draw a field of view visual presentation.
        /// </summary>
        public static void DrawFOV(Vector3 point, Vector3 forward, float fov, float range = 10f)
        {
            var halfFov = fov / 2.0f;
            var leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
            var leftRayDirection = leftRayRotation * forward;
            var rightRayDirection = rightRayRotation * forward;
            Gizmos.DrawRay(point, leftRayDirection * range);
            Gizmos.DrawRay(point, rightRayDirection * range);
            Gizmos.DrawLine(point + leftRayDirection * range, point + rightRayDirection * range);
        }

        /// <summary>
        ///     Draw sphere wire.
        /// </summary>
        public static void DrawWireSphere(Vector3 point, float radius)
        {
            if (_internalSphereMesh == null)
                _internalSphereMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Gizmos/gizmos_Sphere.fbx");

            if (_internalSphereMesh == null)
                Gizmos.DrawWireSphere(point, radius);
            else
                Gizmos.DrawWireMesh(_internalSphereMesh, point, Quaternion.identity, Vector3.one * radius);
        }

        /// <summary>
        ///     Draw a arrow.
        /// </summary>
        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(pos, direction);

            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                        new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                       new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        /// <summary>
        ///     Draw a 2D arrow.
        /// </summary>
        public static void DrawArrow2D(Vector2 pos, Vector2 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(pos, direction);

            Vector2 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector2(0, 1);
            Vector2 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector2(0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        private static Mesh _internalSphereMesh;
    }
}
#endif