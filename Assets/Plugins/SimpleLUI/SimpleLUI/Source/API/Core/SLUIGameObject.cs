//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleLUI.API.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Base class of all entities in Unity Scenes.
    /// </summary>
    public sealed class SLUIGameObject : SLUIObject
    {
        /// <summary>
        ///     The RectTransform attached to this GameObject.
        /// </summary>
        public SLUIRectTransform rectTransform { get; private set; }

        /// <summary>
        ///     The local active state of this GameObject.
        /// </summary>
        public bool activeSelf => Original.activeSelf;

        /// <summary/>
        public new GameObject Original { get; private set; }

        /// <summary/>
        public List<SLUIComponent> Components { get; } = new List<SLUIComponent>();

        internal override void LoadSLUIObject(SLUIManager manager, Object original)
        {
            // invoke base method
            base.LoadSLUIObject(manager, original);

            // get original
            Original = (GameObject) original;

            // collect components
            rectTransform = AddComponent<SLUIRectTransform>();
        }

        // Casting in lua is not possible
        // Can only return just a SLUIComponent
        /// <summary>
        ///     Adds a component named `componentName` to the game object.
        /// </summary>
        public SLUIComponent AddComponent(string componentName)
        {
            var localComponentName = componentName;
            if (!localComponentName.StartsWith("SLUI"))
            {
                localComponentName = $"SLUI{localComponentName}";
            }

            var type = Type.GetType($"SimpleLUI.API.Core.{localComponentName}");
            if (type == null)
            {
                if (SLUIWorker.CustomTypes.ContainsKey(componentName))
                {
                    type = SLUIWorker.CustomTypes[componentName];
                }

                if (type == null)
                    throw new ArgumentException($"Failed to find component of type {componentName}");
            }

            return AddComponent(type);
        }

        internal T AddComponent<T>() where T : SLUIComponent
        {
            return (T) AddComponent(typeof(T));
        }

        internal SLUIComponent AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(SLUIComponent)))
                throw new ArgumentException($"Type {type.FullName} is not a subclass of {typeof(SLUIComponent)}");

            var newComponent = (SLUIComponent) Activator.CreateInstance(type);
            newComponent.gameObject = this;
            newComponent.OnComponentCreated();
            newComponent.LoadOriginalComponent(Manager);
            Components.Add(newComponent);
            newComponent.OnComponentLoaded();
            Core.InternalInitializeObject(newComponent);
            return newComponent;
        }

        /// <summary>
        ///     Activates/Deactivates the GameObject, depending on the given true or false value.
        /// </summary>
        public void SetActive(bool activeState) => Original.SetActive(activeState);
    }
}
