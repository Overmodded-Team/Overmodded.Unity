//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using UnityEngine.Events;

#pragma warning disable 1591

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JEMTranslatorRect : MonoBehaviour
    {
        [Header("Transforms Settings")]
        public RectTransform Target;
        public RectTransform StartPoint;
        public RectTransform EndPoint;

        [Header("Effect Settings")]
        public float Speed = 5f;

        [Header("Conditions Settings")]
        public bool UpdatePosition = true;
        public bool UpdateSize;

        [Header("Events")]
        public UnityEvent OnRectUpdated;

        public bool State { get; private set; }
        
        internal Coroutine SlavePosition { get; set; }
        internal Coroutine SlaveSize { get; set; }

        private void OnDisable()
        {
            if (Target == null)
                Target = GetComponent<RectTransform>();

            // script or gameobject has been disabled
            // force new state
            if (UpdatePosition)
            {
                Target.anchoredPosition = State ? EndPoint.anchoredPosition : StartPoint.anchoredPosition;
            }

            if (UpdateSize)
            {
                Target.sizeDelta = State ? EndPoint.sizeDelta : StartPoint.sizeDelta;
            }
        }

        /// <summary>
        /// Sets state of translator.
        /// </summary>
        public void SetState(bool state)
        {
            if (State == state)
                return;
            if (Target == null)
                Target = GetComponent<RectTransform>();

            State = state;
            JEMTranslatorScript.RegenerateLocalScript();
            if (UpdatePosition)
            {
                if (SlavePosition != null) JEMTranslatorScript.Script.StopCoroutine(SlavePosition);
                SlavePosition =
                    JEMTranslatorScript.Script.StartCoroutine(
                        JEMTranslatorScript.Script.RectSlavePosition(this, state));
            }

            if (UpdateSize)
            {
                if (SlaveSize != null) JEMTranslatorScript.Script.StopCoroutine(SlaveSize);
                SlaveSize =
                    JEMTranslatorScript.Script.StartCoroutine(JEMTranslatorScript.Script.RectSlaveSize(this, state));
            }
        }
    }
}
