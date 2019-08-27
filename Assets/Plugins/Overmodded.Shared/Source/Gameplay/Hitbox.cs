//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Linq;
using UnityEngine;

namespace Overmodded.Gameplay
{
    /// <inheritdoc />
    /// <summary>
    ///     Defines hit box of an level object or game entity.
    /// </summary>
    public class Hitbox : MonoBehaviour
    {
        public GameObject Parent { get; set; }

        public Collider MainCollider { get; private set; }
        public Collider[] Colliders { get; private set; }
        public Rigidbody[] Rigidbodies { get; private set; }

        private void Awake()
        {
            Colliders = GetComponentsInChildren<Collider>(true);
            MainCollider = GetComponentInChildren<Collider>(true);
            if (MainCollider == null)
                throw new NullReferenceException("Hitbox component does not have a collider!");

            Rigidbodies = Colliders.Select(c => c.GetComponent<Rigidbody>()).Where(rg => rg != null).ToArray();

            // by default all rigidbody should by inactive
            SetRigidbodyActive(false);
        }

        /// <summary>
        ///     Gets parent script of given type.
        /// </summary>
        public T GetParent<T>() where T : MonoBehaviour
        {
            if (Parent == null)
                throw new NullReferenceException("A parent has not been set!");

            return Parent.GetComponent<T>();
        }

        /// <summary>
        ///     Sets active state all rigidbody components.
        /// </summary>
        public void SetRigidbodyActive(bool activeState)
        {
            if (Colliders == null) return;
            foreach (var r in Rigidbodies)
            {
                r.isKinematic = !activeState;
            }
        }

        public void ApplyRigidbodyForce(Vector3 velocity, Vector3 position, ForceMode mode = ForceMode.Impulse)
        {
            foreach (var r in Rigidbodies)
            {
                r.AddForceAtPosition(velocity, position, mode);
            }
        }
    }
}
