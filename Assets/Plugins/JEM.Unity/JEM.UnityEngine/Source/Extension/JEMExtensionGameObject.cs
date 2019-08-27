//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JEM.UnityEngine.Extension
{
    /// <summary>
    ///     Set of utility extensions to GameObject class.
    /// </summary>
    public static class JEMExtensionGameObject
    {
        private static JEMExtensionGameObjectScript _script;

        internal static void RegenerateLocalScript()
        {
            if (_script != null)
                return;

            var obj = new GameObject(nameof(JEMExtensionGameObjectScript));
            Object.DontDestroyOnLoad(obj);
            _script = obj.AddComponent<JEMExtensionGameObjectScript>();

            if (_script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMExtensionGameObject)}@{nameof(JEMExtensionGameObjectScript)}");
        }

        /// <summary>
        ///     Invoke operation on all children GameObjects.
        /// </summary>
        /// <param name="gameObject">Parent of GameObjects.</param>
        /// <param name="operation">Operation to invoke.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ChildOperation<T1>([NotNull] this GameObject gameObject, [NotNull] Action<T1> operation)
            where T1 : Component
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            var elements = gameObject.GetComponentsInChildren<T1>();
            foreach (var e in elements)
                operation.Invoke(e);
        }

        /// <summary>
        ///     Gets component from parent.
        /// </summary>
        /// <typeparam name="T1">Type of component.</typeparam>
        /// <param name="go">GameObject to search in.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static T1 GetComponentInParent<T1>([NotNull] this GameObject go) where T1 : Component
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            if (go.GetComponent<T1>() != null)
                return go.GetComponent<T1>();
            var t = go.transform;
            while (t.parent != null)
            {
                var e = t.gameObject.GetComponent<T1>();
                if (e != null) return e;

                t = t.parent.transform;
            }

            return default(T1);
        }

        /// <summary>
        ///     Gets components from parent.
        /// </summary>
        /// <typeparam name="T1">Type of component.</typeparam>
        /// <param name="go">GameObject to search in.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static T1[] GetComponentsInParent<T1>([NotNull] this GameObject go) where T1 : Component
        {
            return GetComponentsListInParent<T1>(go).ToArray();
        }

        /// <summary>
        ///     Gets components from parent.
        /// </summary>
        /// <typeparam name="T1">Type of component.</typeparam>
        /// <param name="go">GameObject to search in.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static IReadOnlyList<T1> GetComponentsListInParent<T1>([NotNull] this GameObject go) where T1 : Component
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            var list = new List<T1>();

            var local = go.GetComponents<T1>();
            if (local != null && local.Length != 0)
                list.AddRange(local);

            var t = go.transform;
            while (t.parent != null)
            {
                var e = t.gameObject.GetComponents<T1>();
                if (e != null && e.Length != 0)
                    list.AddRange(e);

                t = t.parent.transform;
            }

            return list.ToArray();
        }

        /// <summary>
        ///     Creates child GameObject.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static GameObject CreateChild([NotNull] this GameObject go, string childName)
        {
            if (go == null) throw new ArgumentNullException(nameof(go));

            var child = new GameObject(childName);
            child.transform.SetParent(go.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
            return child;
        }

        /// <summary>
        ///     Gets or adds component if not exist.
        /// </summary>
        /// <typeparam name="T1">Type of component</typeparam>
        /// <param name="go"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static T1 ResolveComponent<T1>([NotNull] this GameObject go) where T1 : Component
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            var component = go.GetComponent<T1>();
            if (component == default(T1))
                component = go.AddComponent<T1>();
            return component;
        }

        /// <summary>
        ///     Activates/Deactivates the GameObject in background.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="activeState">Active state.</param>
        /// <param name="onDone">Event called at end of this background operation.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void LiteSetActive([NotNull] this GameObject go, bool activeState, Action onDone = null)
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            if (go.activeSelf == activeState)
            {
                onDone?.Invoke();
                return;
            }

            RegenerateLocalScript();
            _script.StartCoroutine(_script.InternalSetActiveEasy(go, activeState, onDone));
        }

        /// <summary>
        ///     Gets name of game object for debug.
        /// </summary>
        /// <param name="go"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetDebugName([NotNull] this GameObject go)
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            return $"{go.name}@{go.GetInstanceID()}";
        }

        /// <summary>
        ///     Gets start debug message of given game object.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetDebugMessage([NotNull] this GameObject go)
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            return $"GameObject:{go.GetDebugName()} -> ";
        }

        /// <summary>
        ///     Gets path from grand parent to this gameobject.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetParentPath([NotNull] this GameObject go)
        {
            if (go == null) throw new ArgumentNullException(nameof(go));
            var path = string.Empty;
            var p = go.transform.parent;
            while (p != null)
            {
                path = $@"\{p.name}{path}";
                p = p.parent;
            }

            path = path + $@"\{go.name}";
            path = path.Remove(0, 1);
            return path;
        }

        /// <summary>
        ///     Gets or adds new component to the object.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static T CollectComponent<T>([NotNull] this GameObject g) where T : Component
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            var i = g.GetComponent<T>();
            if (i == null)
                i = g.AddComponent<T>();

            return i;
        }

        /// <summary>
        ///     Gets or adds new component to the object.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static Component CollectComponent([NotNull] this GameObject g, [NotNull] Type t)
        {
            if (g == null) throw new ArgumentNullException(nameof(g));
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (!t.IsSubclassOf(typeof(Component)))
                throw new ArgumentException($"Type {t.FullName} is not a subclass of Component.");

            var i = g.GetComponent(t);
            if (i == null)
                i = g.AddComponent(t);

            return i;
        }
    }
}