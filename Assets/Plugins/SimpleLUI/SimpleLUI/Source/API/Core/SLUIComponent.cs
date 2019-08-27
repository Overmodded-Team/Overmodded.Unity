//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace SimpleLUI.API.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Base class for everything attached to GameObjects.
    /// </summary>
    public abstract class SLUIComponent : SLUIObject
    {
        /// <summary>
        ///     The game object this component is attached to. A component is always attached to a game object.
        /// </summary>
        public SLUIGameObject gameObject { get; internal set; }

        /// <summary>
        ///     The RectTransform attached to this GameObject.
        /// </summary>
        public SLUIRectTransform rectTransform => gameObject.rectTransform;

        /// <summary>
        ///     Enabled Behaviours are Updated, disabled Behaviours are not.
        /// </summary>
        public bool enable
        {
            get => OriginalBehaviour == null || OriginalBehaviour.enabled;
            set
            {
                if (OriginalBehaviour != null)
                    OriginalBehaviour.enabled = value;
            }
        }

        /// <summary>
        ///     Is the component active and enabled? Disabled components are not Updated.
        /// </summary>
        public bool isActiveAndEnabled => OriginalBehaviour == null || OriginalBehaviour.isActiveAndEnabled;

        /// <summary/>
        public GameObject OriginalGameObject => gameObject.Original;

        /// <summary/>
        public Component OriginalComponent { get; private set; }

        /// <summary/>
        public Behaviour OriginalBehaviour { get; private set; }

        /// <summary/>
        public virtual void OnComponentCreated() { }

        internal void LoadOriginalComponent(SLUIManager manager)
        {
            OriginalComponent = OnLoadOriginalComponent();
            if (OriginalComponent == null)
                throw new NullReferenceException(nameof(OriginalComponent));

            LoadSLUIObject(manager, OriginalComponent);
            if (OriginalComponent is Behaviour behaviour)
            {
                OriginalBehaviour = behaviour;
            }
        }

        /// <summary/>
        public abstract Component OnLoadOriginalComponent();

        /// <summary/>
        public virtual void OnComponentLoaded() { }

        /// <summary>
        ///     Adds a component named `componentName` to the game object.
        /// </summary>
        public SLUIComponent AddComponent(string componentName) => gameObject.AddComponent(componentName);
    }
}
