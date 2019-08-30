//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace JEM.UnityEditor.Extension
{
    /// <summary>
    ///     Set of utility methods: Animator
    /// </summary>
    public static class JEMExtensionAnimatorEditor
    {
        /// <summary>
        ///     Checks if parameter with given name exists in animator.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="parameterName">Name of parameter.</param>
        /// <returns>Exist state.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static bool ParameterExist([NotNull] this Animator animator, string parameterName)
        {
            if (animator == null) throw new ArgumentNullException(nameof(animator));
            var controller = animator.runtimeAnimatorController as AnimatorController;
            if (controller == null)
                throw new NullReferenceException(nameof(controller));
            return controller.parameters.Any(p => p.name == parameterName);
        }
    }
}