//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using UnityEngine.Events;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    /// Simple switch script.
    /// </summary>
    public class JEMSwitch : MonoBehaviour
    {
        /// <summary>
        /// Defines active state.
        /// </summary>
        [Header("Settings")]
        public bool IsActive;

        [Header("Events")]
        public UnityEvent OnFalse;
        public UnityEvent OnTrue;
        public UnityEvent OnUpdate;

        /// <summary>
        /// Default state of switch.
        /// </summary>
        public bool DefaultState { get; private set; }

        private void Awake()
        {
            DefaultState = IsActive;
        }

        /// <summary>
        /// Switch the switch!
        /// </summary>
        public void Switch()
        {
            Set(!IsActive);
        }

        /// <summary>
        /// Set switch state.
        /// </summary>
        public void Set(bool switchState)
        {
            if (IsActive == switchState)
                return;

            IsActive = switchState;
            if (IsActive)
            {
                OnTrue.Invoke();
            }
            else
            {
                OnFalse.Invoke();
            }

            OnUpdate.Invoke();
        }

        /// <summary>
        /// Sets default state.
        /// </summary>
        public void SetDefault()
        {
            Set(DefaultState);
        }
    }
}
