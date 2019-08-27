//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.UnityEngine.Extension;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace JEM.UnityEngine.Interface
{
    /// <inheritdoc />
    /// <summary>
    ///     Canvas Renderer Fade animation script.
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(CanvasRenderer))]
    public sealed class JEMInterfaceFadeElement : MonoBehaviour
    {
        /// <summary>
        ///     The element will ignore operations invoked by InterfaceFadeGroup.
        /// </summary>
        [Header("Interface Fade Element")]
        [Tooltip("The element will ignore operations invoked by InterfaceFadeGroup.")]
        public bool IgnoreGroup;

        /// <summary>
        ///     Target raycast of current graphic will be always false while starting fade process.
        /// </summary>
        [Tooltip("Target raycast of current graphic will be always false while starting fade process.")]
        public bool AlwaysFalseRaycast;

        /// <summary>
        ///     If GameObject of this InterfaceFadeElement is inactive, script will automaticly set it activate.
        /// </summary>
        [Tooltip("If GameObject of this InterfaceFadeElement is inactive, script will automaticly set it activate. ")]
        public bool ApplyStateToGameObject = true;

        /// <summary>
        ///     Disallow GameObject lite activation.
        ///     It's sometimes helps to remove `GameObject is already being activated or deactivated.` error.
        /// </summary>
        public bool DisallowLiteActivation;

        /// <summary>
        ///     User defined graphic alpha target.
        /// </summary>
        [Tooltip("User defined graphic alpha target.")]
        public float FixedAlphaTarget = 1f;

        /// <summary>
        ///     Defines whether the element is loaded.
        /// </summary>
        public bool IsLoaded { get; private set; }

        private CanvasRenderer CanvasRenderer { get; set; }
        private Graphic ElementGraphic { get; set; }
        private Selectable ElementSelectable { get; set; }

        internal bool StartRaycastTarget { get; set; }
        internal ColorBlock StartSelectableColorBlock { get; set; }
        internal Image StartSelectableRootImage { get; set; }
        internal float StartSelectableColorAlpha { get; set; }

        internal bool WorkActive { get; set; }
        internal bool WorkState { get; set; }
        internal Action WorkCallback { get; set; }
        internal short WorkSessionId { get; set; }

        /// <summary>
        ///     Current graphic alpha of of fade element.
        /// </summary>
        internal float GraphicAlpha { get; private set; }

        // awake event
        private void Awake()
        {
            InternalLoadElement();
        }

        // on disable event
        private void OnDisable()
        {
            if (WorkActive) Force(WorkState);
        }

        private void InternalLoadElement()
        {
            if (IsLoaded)
                return;

            IsLoaded = true;

            if (GetComponents<JEMInterfaceFadeElement>().Length > 1)
            {
                enabled = false;
                throw new NotSupportedException(
                    $"Somehow, there is more than one InterfaceFadeElement on gameObject {gameObject.GetParentPath()}.");
            }

            CanvasRenderer = GetComponent<CanvasRenderer>();
            if (CanvasRenderer == null)
            {
                enabled = false;
                throw new NotSupportedException(
                    $"Somehow, on object({gameObject.GetParentPath()}) of {nameof(JEMInterfaceFadeElement)} there is no component of type {nameof(global::UnityEngine.CanvasRenderer)}.");
            }

            ElementGraphic = GetComponent<Graphic>();
            if (ElementGraphic != null) StartRaycastTarget = ElementGraphic.raycastTarget;

            ElementSelectable = GetComponent<Selectable>();
            if (ElementSelectable != null)
            {
                StartSelectableRootImage = ElementSelectable.GetComponent<Image>();
                if (StartSelectableRootImage != null) StartSelectableColorAlpha = StartSelectableRootImage.color.a;

                StartSelectableColorBlock = new ColorBlock
                {
                    normalColor = ElementSelectable.colors.normalColor,
                    highlightedColor = ElementSelectable.colors.highlightedColor,
                    pressedColor = ElementSelectable.colors.pressedColor,
                    disabledColor = ElementSelectable.colors.disabledColor,
                    selectedColor = ElementSelectable.colors.selectedColor,
                    colorMultiplier = ElementSelectable.colors.colorMultiplier,
                    fadeDuration = ElementSelectable.colors.fadeDuration
                };
            }

            // update graphic alpha just for sure
            InternalUpdateGraphicAlpha(isActiveAndEnabled ? 1f : 0f);
        }

        private bool InternalCheckObject()
        {
            if (Math.Abs(Time.timeScale) < float.Epsilon)
            {
                JEMLogger.InternalLogError(
                    "You are trying to run interface fade while Time.timeScale is set to zero. Fade can't work while Time.timeScale is set to zero!");
                return false;
            }

            if (CanvasRenderer == null) InternalLoadElement();

            if (CanvasRenderer == null)
            {
                JEMLogger.InternalLogError("Unable to fade element. Target canvas renderer is null.");
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Breaks/Stops current work.
        /// </summary>
        internal void InternalBreakWork()
        {
            if (!WorkActive) return;

            // reset last work
            WorkActive = false;
            WorkSessionId = 0;
            WorkCallback?.Invoke();
            WorkCallback = null;
        }

        /// <summary>
        ///     Forces new state of this element.
        /// </summary>
        /// <param name="state"></param>
        public void Force(bool state)
        {
            // check if object is correctly loaded and can work
            if (!InternalCheckObject())
                return;

            // check if work is already active, if so, stop it!
            if (WorkActive)
                InternalBreakWork();

            // set graphic alpha
            InternalUpdateGraphicAlpha(state ? 1f : 0f);

            // apply state to game object
            if (ApplyStateToGameObject && !DisallowLiteActivation)
                if (!gameObject.activeSelf && state)
                    if (gameObject.GetComponent<JEMInterfaceFadeAnimation>() == null && gameObject.GetComponent<Scrollbar>() == null)
                        gameObject.LiteSetActive(true);

            // fix raycast target
            InternalSetRaycastTarget(StartRaycastTarget);
        }

        /// <summary>
        ///     Fade this element.
        /// </summary>
        /// <param name="state">Fading state.</param>
        /// <param name="speed">Speed of fading.</param>
        /// <param name="delay">The delay with which the fade will start.</param>
        /// <param name="fixedSmooth">Fade will use smooth delta time.</param>
        /// <param name="callBack">Event, called when all elements ends fading.</param>
        public void Fade(bool state, float speed, float delay = 0f, bool fixedSmooth = false, Action callBack = null)
        {
            // check if object is correctly loaded and can work
            if (!InternalCheckObject())
                return;

            // check if work is already active, if so, stop it!
            if (WorkActive)
                InternalBreakWork();

            if (isActiveAndEnabled)
            {
                // regenerate local script
                RegenerateLocalScript();

                // generate new work session identity
                WorkSessionId = (short) Random.Range(short.MinValue, short.MaxValue);

                // and run new fade work
                _script.StartCoroutine(_script.InternalFadeWork(this, WorkSessionId, state, speed, delay, fixedSmooth,
                    callBack));
            }
            else
            {
                // object or script is inactive, force new state because continues can't work while script is inactive
                // TODO: Add ability to set if system should force new state or ignore with warning message.
                Force(state);

                // invoke callback
                callBack?.Invoke();
            }
        }

        /// <summary>
        ///     Sets raycast target of current graphic active.
        /// </summary>
        /// <param name="state"></param>
        internal void InternalSetRaycastTarget(bool state)
        {
            if (ElementGraphic == null)
                return;
            ElementGraphic.raycastTarget = state;
        }

        /// <summary>
        ///     Updates current graphic alpha.
        /// </summary>
        internal void InternalUpdateGraphicAlpha(float alpha)
        {
            GraphicAlpha = alpha;
            if (ElementSelectable == null)
            {
                CanvasRenderer.SetAlpha(GraphicAlpha);
            }
            else
            {
                if (StartSelectableRootImage != null)
                    StartSelectableRootImage.color = new Color(StartSelectableRootImage.color.r,
                        StartSelectableRootImage.color.g, StartSelectableRootImage.color.b,
                        StartSelectableColorAlpha * alpha);

                ElementSelectable.colors = new ColorBlock
                {
                    normalColor = StartSelectableColorBlock.normalColor.MultiplyAlpha(GraphicAlpha),
                    highlightedColor = StartSelectableColorBlock.highlightedColor.MultiplyAlpha(GraphicAlpha),
                    pressedColor = StartSelectableColorBlock.pressedColor.MultiplyAlpha(GraphicAlpha),
                    disabledColor = StartSelectableColorBlock.disabledColor.MultiplyAlpha(GraphicAlpha),
                    selectedColor = StartSelectableColorBlock.selectedColor.MultiplyAlpha(GraphicAlpha),
                    colorMultiplier = StartSelectableColorBlock.colorMultiplier * GraphicAlpha,
                    fadeDuration = StartSelectableColorBlock.fadeDuration * GraphicAlpha
                };
            }
        }

        internal static void RegenerateLocalScript()
        {
            if (_script != null)
                return;

            var obj = new GameObject(nameof(JEMInterfaceFadeElementScript));
            DontDestroyOnLoad(obj);
            _script = obj.AddComponent<JEMInterfaceFadeElementScript>();

            if (_script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMInterfaceFadeElement)}@{nameof(JEMInterfaceFadeElementScript)}");
        }

        private static JEMInterfaceFadeElementScript _script;
    }
}