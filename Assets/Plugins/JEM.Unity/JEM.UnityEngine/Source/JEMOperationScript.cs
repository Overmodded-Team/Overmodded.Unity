//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Unity Script of JEMOperation class.
    /// </summary>
    public class JEMOperationScript : MonoBehaviour
    {
        /// <summary>
        /// </summary>
        /// <param name="sleep"></param>
        /// <param name="targetAction"></param>
        /// <returns></returns>
        public IEnumerator InternalInvokeAction(float sleep, Action targetAction)
        {
            yield return new WaitForSeconds(sleep);
            targetAction?.Invoke();
        }
    }
}