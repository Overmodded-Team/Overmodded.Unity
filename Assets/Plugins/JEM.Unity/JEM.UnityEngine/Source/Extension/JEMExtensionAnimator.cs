//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility methods: Animator
    /// </summary>
    public static class JEMExtensionAnimator
    {
        /// <summary>
        ///     Interpolate float value of animator.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpFloat([NotNull] this Animator animator, [NotNull] string parName, float value, float time = 10f)
        {
            if (animator == null) throw new ArgumentNullException(nameof(animator));
            if (parName == null) throw new ArgumentNullException(nameof(parName));
            animator.SetFloat(parName, Mathf.Lerp(animator.GetFloat(parName), value, Time.deltaTime * time));
        }

        /// <summary>
        ///     Interpolate float value of animator.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LerpFloatT([NotNull] this Animator animator, [NotNull] string parName, float value, float time)
        {
            if (animator == null) throw new ArgumentNullException(nameof(animator));
            if (parName == null) throw new ArgumentNullException(nameof(parName));
            animator.SetFloat(parName, Mathf.Lerp(animator.GetFloat(parName), value, time));
        }
    }
}