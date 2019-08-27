//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using SimpleLUI.API.Core;
using SimpleLUI.API.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleLUI.API
{
    /// <summary>
    ///     SLUI core class.
    ///     Contains core methods to create and destroy objects.
    ///     Core class can be accesses in lua by using 'core'.
    /// </summary>
    public class SLUICore
    {
        internal SLUIWorker Parent { get; }
        internal List<SLUIObject> Objects { get; } = new List<SLUIObject>();

        /// <summary>
        ///     Gets object of given instanceId.
        /// </summary>
        public T GetObject<T>(int instanceId) where T : SLUIObject
        {
            foreach (var o in Objects)
            {
                if (o?.Original == null)
                    continue;

                if (o.Original.GetInstanceID() == instanceId)
                    return (T) o;
            }

            return null;
        }

        internal SLUICore(SLUIWorker parent)
        {
            Parent = parent;
        }

        /// <summary>
        ///     Destroys all the content of SLUI.
        /// </summary>
        internal void DestroyContent()
        {
            foreach (var obj in Objects)
            {
                if (obj?.Original == null)
                    continue; 

                if (obj.Original is RectTransform r)
                    Object.Destroy(r.gameObject); // if rectTransform, destroy whole obj!
                else Object.Destroy(obj.Original);
            }

            Objects.Clear();
        }

        /// <summary>
        ///     Creates new gameObject.
        /// </summary>
        public SLUIGameObject Create([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return Create(name, SLUIVector2.Zero);
        }

        /// <summary>
        ///     Creates new gameObject at given anchoredPosition.
        /// </summary>
        public SLUIGameObject Create([NotNull] string name, [NotNull] SLUIVector2 anchoredPosition)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (anchoredPosition == null) throw new ArgumentNullException(nameof(anchoredPosition));
            return Create(name, anchoredPosition, SLUIVector2.Zero);
        }

        /// <summary>
        ///     Creates new gameObject at given anchoredPosition and size.
        /// </summary>
        public SLUIGameObject Create([NotNull] string name, [NotNull] SLUIVector2 anchoredPosition, [NotNull] SLUIVector2 sizeDelta)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (anchoredPosition == null) throw new ArgumentNullException(nameof(anchoredPosition));
            if (sizeDelta == null) throw new ArgumentNullException(nameof(sizeDelta));
            var newGameObject = new GameObject(name);
            InternalInitializeGameObject(newGameObject);

            var newReference = new SLUIGameObject();
            newReference.LoadSLUIObject(Parent.Parent, newGameObject);
            InternalInitializeObject(newReference);
            newReference.rectTransform.anchoredPosition = anchoredPosition;
            newReference.rectTransform.sizeDelta = sizeDelta;
            return newReference;
        }

        /// <summary>
        ///     Destroys given object.
        /// </summary>
        public void Destroy([NotNull] SLUIObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (Objects.Contains(obj))
            {
                Objects.Remove(obj);
            }

            Object.Destroy(obj.Original);
        }

        private void InternalInitializeGameObject(GameObject obj)
        {
            obj.transform.SetParent(Parent.Parent.Canvas.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.layer = LayerMask.NameToLayer("UI");
        }

        internal void InternalInitializeObject(SLUIObject obj)
        {
            if (Objects.Contains(obj))
            {
                return;
            }

            Objects.Add(obj);
        }

        /// <summary>
        ///     Execute group of unity's event.
        /// </summary>
        public void ExecuteUnityEvent([NotNull] SLUIUnityEvent unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));
            foreach (var i in unityEvent.items)
            {
                ExecuteUnityEvent(i);
            }
        }

        /// <summary>
        ///     Execute unity event.
        /// </summary>
        public void ExecuteUnityEvent([NotNull] SLUIEventItem unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));
            ExecuteUnityEvent(unityEvent.target, unityEvent.methodName, unityEvent.args.ToArray());
        }

        /// <summary>
        ///     Direct unity event execution.
        /// </summary>
        public void ExecuteUnityEvent([NotNull] SLUIObject target, [NotNull] string methodName, params object[] args)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            var obj = target.Original;

            var method = obj.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                throw new ArgumentException($"Method {methodName} does not exist in object of type {obj.GetType()}.");

            object[] a = args?.ToArray();
            method.Invoke(obj, a);
        }
    }
}
