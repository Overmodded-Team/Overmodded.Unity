//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to Text class.
    /// </summary>
    public static class JEMExtensionText
    {
        private static JEMExtensionTextScript _script;

        internal static void RegenerateLocalScript()
        {
            if (_script != null)
                return;

            var obj = new GameObject(nameof(JEMExtensionTextScript));
            Object.DontDestroyOnLoad(obj);
            _script = obj.AddComponent<JEMExtensionTextScript>();

            if (_script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMExtensionText)}@{nameof(JEMExtensionTextScript)}");
        }

        /// <summary>
        ///     Insert given text char by char.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textToInsert">Text to insert.</param>
        /// <param name="speed">Speed of inserting.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void InsertText([NotNull] this Text text, [NotNull] string textToInsert, float speed)
        {
            InsertText(text, textToInsert, speed, null);
        }

        /// <summary>
        ///     Insert given text char by char.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textToInsert">Text to insert.</param>
        /// <param name="speed">Speed of inserting.</param>
        /// <param name="onComplete"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void InsertText([NotNull] this Text text, [NotNull] string textToInsert, float speed,
            Action onComplete)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (textToInsert == null) throw new ArgumentNullException(nameof(textToInsert));
            RegenerateLocalScript();
            _script.StartCoroutine(_script.InternalInsertText(text, textToInsert, speed, onComplete));
        }
    }
}