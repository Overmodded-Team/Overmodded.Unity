//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple script that will destroy gameobject after given time.
    /// </summary>
    public class JEMDestroyAfterTime : MonoBehaviour
    {
        /// <summary>
        ///     Defines how much time in seconds system need to wait.
        /// </summary>
        [Header("Settings")]
        public float Time = 2;

        /// <summary>
        ///     Defines whether the default hard Object.Destroy method should be used.
        /// </summary>
        public bool HardDestroy;

        private void Start()
        {
            if (HardDestroy)
            {
                Destroy(gameObject, Time);
                enabled = false;
            }
            else
                StartCoroutine(FixedDestroy());
        }

        private IEnumerator FixedDestroy()
        {
            yield return new WaitForSeconds(Time);
            Destroy(gameObject);
            yield return new WaitForEndOfFrame();
        }
    }
}