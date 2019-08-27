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
    ///     Unity Script of JEMTimer class.
    /// </summary>
    public class JEMTimerScript : MonoBehaviour
    {
        /// <summary>
        /// </summary>
        /// <param name="targetTime"></param>
        /// <param name="onTick"></param>
        /// <returns></returns>
        public IEnumerator CuntDownTimer(int targetTime, Action<int> onTick)
        {
            var time = 0;
            while (time < targetTime)
            {
                onTick?.Invoke(time);
                time++;
                yield return new WaitForSeconds(1);
            }
        }
    }
}