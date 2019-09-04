//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to Transform class.
    /// </summary>
    public static class JEMExtensionTransform
    {
        /// <summary>
        ///     Look at smooth
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LookAtSmooth([NotNull] this Transform transform, [NotNull] Transform target, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            if (target == null) throw new ArgumentNullException(nameof(target));
            LookAtSmooth(transform, target.position, time);
        }

        /// <summary>
        ///     Look at smooth.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LookAtSmooth([NotNull] this Transform transform, Vector3 point, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(point - transform.position), Time.deltaTime * time);
        }

        /// <summary>
        ///     Interpolate transform position.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpPosition([NotNull] this Transform transform, Vector3 point, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * time);
        }

        /// <summary>
        ///     Interpolate transform rotation.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpRotation([NotNull] this Transform transform, Quaternion rotation, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * time);
        }

        /// <summary>
        ///     Interpolate transform rotation.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpRotation([NotNull] this Transform transform, Vector3 euler, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(euler), Time.deltaTime * time);
        }

        /// <summary>
        ///     Interpolate transform rotation.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpRotation([NotNull] this Transform transform, float x, float y, float z, float time = 10f)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(x, y, z), Time.deltaTime * time);
        }
    }
}