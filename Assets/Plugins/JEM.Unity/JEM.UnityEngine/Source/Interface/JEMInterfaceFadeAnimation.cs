//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Extension;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JEM.UnityEngine.Interface
{
    /// <summary>
    ///     Fade animation mode.
    /// </summary>
    public enum JEMFadeAnimationMode
    {
        /// <summary>
        ///     Transform localScale will be used to animate fade.
        /// </summary>
        UsingLocalScale,

        /// <summary>
        ///     RectTransform sizeDelta will be used to animate fade.
        /// </summary>
        UsingSizeDelta,

        /// <summary>
        ///     Disables animation on object's scale.
        /// </summary>
        Disabled
    }

    /// <inheritdoc />
    /// <summary>
    ///     Activate/Deactivate interface fade animations.
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform), typeof(JEMInterfaceFadeElement))]
    public sealed class JEMInterfaceFadeAnimation : MonoBehaviour
    {
        /// <summary>
        ///     Default fade animation speed.
        /// </summary>
        [Header("Animation Settings")]
        public float AnimationSpeed = 10f;

        /// <summary>
        ///     AnimationEnterScale.
        /// </summary>
        [Space]
        public float AnimationEnterScale = 0.9f;

        /// <summary>
        ///     AnimationExitScale.
        /// </summary>
        public float AnimationExitScale = 1.1f;

        /// <summary>
        ///     Defines current animation mode.
        /// </summary>
        [Space]
        public JEMFadeAnimationMode AnimationMode = JEMFadeAnimationMode.UsingLocalScale;

        /// <summary>
        ///     RectTransform.
        /// </summary>
        public RectTransform RectTransform { get; private set; }

        /// <summary>
        ///     Start rectTransform sizeDelta of object.
        /// </summary>
        public Vector2 StartSizeDelta { get; private set; }

        /// <summary>
        ///     Start transform size of object.
        /// </summary>
        public Vector3 StartLocalSize { get; private set; }

        /// <summary>
        ///     Defines whether the script is loaded.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        ///     Window active state.
        /// </summary>
        public bool TargetActive { get; private set; }

        /// <summary>
        ///     Animations working state.
        /// </summary>
        public bool IsWorking { get; private set; }

        /// <summary>
        ///     Current wort session identity.
        /// </summary>
        public short WorkSessionID { get; private set; }

        /// <summary>
        ///     Current coroutine of work.
        /// </summary>
        public Coroutine WorkingCoroutine { get; private set; }

        // awake event
        private void Awake()
        {
            InternalLoadScript();
        }

        private void InternalLoadScript()
        {
            if (IsLoaded)
                return;

            RectTransform = GetComponent<RectTransform>();
            StartSizeDelta = RectTransform.sizeDelta;
            StartLocalSize = transform.localScale;

            IsLoaded = true;
        }

        /// <summary>
        ///     Sets window active state.
        /// </summary>
        /// <param name="activeState">New active state.</param>
        public void SetWindowActive(bool activeState)
        {
            SetActive(activeState);
        }

        /// <summary>
        ///     Sets window active state.
        /// </summary>
        /// <param name="activeState">New active state.</param>
        /// <param name="forced">If true, operation will be forced.</param>
        public bool SetActive(bool activeState, bool forced = false)
        {
            // check if gameobject is already activated
            if (TargetActive == activeState)
            {
                // if so, force current active state, just to make sure that all ui elements are displayed
                ForceActiveState();
                return false;
            }

            // update target active state
            TargetActive = activeState;

            if (forced)
            {
                ForceActiveState();
                return true;
            }

            // load script just for sure
            InternalLoadScript();
            RegenerateLocalScript();

            if (IsWorking) StopLastWork();

            IsWorking = true;
            WorkSessionID = (short) Random.Range(short.MinValue, short.MaxValue);
            WorkingCoroutine = _script.StartCoroutine(_script.InternalSetActive(this, WorkSessionID));

            return WorkingCoroutine != null;
        }

        /// <summary>
        ///     Forces deactivation.
        /// </summary>
        public void ForceDeactivation()
        {
            StopLastWork();
            TargetActive = false;
            ForceActiveState();
        }

        /// <summary>
        ///     Restarts current work.
        /// </summary>
        internal void RestartWork()
        {
            IsWorking = false;
            WorkSessionID = 0;
        }

        /// <summary>
        ///     Forces given active state.
        /// </summary>
        public void ForceActiveState()
        {
            gameObject.LiteSetActive(TargetActive, () => { gameObject.ForceGroup(TargetActive); });

            if (!IsLoaded)
                return;

            switch (AnimationMode)
            {
                case JEMFadeAnimationMode.UsingLocalScale:
                    gameObject.transform.localScale = StartLocalSize;
                    break;
                case JEMFadeAnimationMode.UsingSizeDelta:
                    RectTransform.sizeDelta = StartSizeDelta;
                    break;
            }
        }

        private void StopLastWork()
        {
            if (WorkingCoroutine != null) _script.StopCoroutine(WorkingCoroutine);

            WorkingCoroutine = null;
            WorkSessionID = 0;
            IsWorking = false;
        }

        internal static void RegenerateLocalScript()
        {
            if (_script != null)
                return;

            var obj = new GameObject(nameof(JEMInterfaceFadeAnimationScript));
            DontDestroyOnLoad(obj);
            _script = obj.AddComponent<JEMInterfaceFadeAnimationScript>();

            if (_script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMInterfaceFadeAnimation)}@{nameof(JEMInterfaceFadeAnimationScript)}");
        }

        private static JEMInterfaceFadeAnimationScript _script;
    }
}