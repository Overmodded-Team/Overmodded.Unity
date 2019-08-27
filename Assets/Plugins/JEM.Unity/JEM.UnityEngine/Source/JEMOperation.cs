//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Set of utility methods. Operations.
    /// </summary>
    public static class JEMOperation
    {
        private static JEMOperationScript _script;

        internal static void RegenerateLocalScript()
        {
            if (_script != null)
                return;

            var obj = new GameObject(nameof(JEMOperationScript));
            Object.DontDestroyOnLoad(obj);
            _script = obj.AddComponent<JEMOperationScript>();

            if (_script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMObject)}@{nameof(JEMOperationScript)}");
        }

        /// <summary>
        ///     Wait given time and invoke target action.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void InvokeAction(float sleep, [NotNull] Action targetAction)
        {
            if (targetAction == null) throw new ArgumentNullException(nameof(targetAction));

            RegenerateLocalScript();
            _script.StartCoroutine(_script.InternalInvokeAction(sleep, targetAction));
        }

        /// <summary>
        ///     Starts given coroutine on locale JEM script.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void StartCoroutine([NotNull] IEnumerator routine)
        {
            if (routine == null) throw new ArgumentNullException(nameof(routine));

            RegenerateLocalScript();
            _script.StartCoroutine(routine);
        }
    }
}