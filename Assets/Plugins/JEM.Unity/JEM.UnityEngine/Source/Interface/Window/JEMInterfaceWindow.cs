//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;
using UnityEngine.Events;

namespace JEM.UnityEngine.Interface.Window
{
    /// <inheritdoc />
    /// <summary>
    ///     Draggable and re-sizable window for unity's UI system.
    /// </summary>
    [DisallowMultipleComponent]
    public class JEMInterfaceWindow : MonoBehaviour
    {
        /// <summary>
        ///     An unique window name.
        /// </summary>
        [Header("Identity Settings")]
        public string UniqueWindowName;

        /// <summary>
        ///     Root window transform.
        /// </summary>
        [Header("Base Settings")]
        public RectTransform RootTransform;

        /// <summary>
        ///     Allow to drag this window.
        /// </summary>
        public bool AllowDragging;

        /// <summary>
        ///     Allow to resize this window.
        /// </summary>
        public bool AllowResize;

        /// <summary>
        ///     If set to true, the window will always move on top of others if moved.
        /// </summary>
        public bool AlwaysMoveOnTop = true;

        /// <summary>
        ///     Defines whether the window size clamp feature should be enabled.
        /// </summary>
        [Header("Size Settings")]
        public bool ClampWindowSize = true;

        /// <summary>
        ///     Maximal possible window size.
        /// </summary>
        public Vector2 MaxWindowSize;

        /// <summary>
        ///     Minimal possible window size.
        /// </summary>
        public Vector2 MinWindowSize;

        /// <summary>
        ///     If set to true, system will not activate gameobject of this window. Good for custom activation behaviour.
        /// </summary>
        [Header("Behaviour")]
        public bool ShouldActivateGameObject = true;

        /// <summary>
        ///     Called on window activate.
        /// </summary>
        [Header("Events")]
        public UnityEvent OnWindowActivated;

        /// <summary>
        ///     Called on window deactivate.
        /// </summary>
        public UnityEvent OnWindowDeactivated;

        /// <summary>
        ///     Current canvas of this window.
        /// </summary>
        public Canvas WindowCanvas { get; private set; }

        /// <summary>
        ///     Defines whether the window is active..
        /// </summary>
        public bool WindowActive { get; private set; }

        /// <summary>
        ///     Current anchor of window.
        /// </summary>
        public JEMInterfaceWindowAnchorName CurrentAnchorName { get; private set; }

        private void Start()
        {
            if (RootTransform == null) RootTransform = GetComponent<RectTransform>();
            CurrentAnchorName = JEMInterfaceWindowUtil.GetAnchorName(RootTransform);
        }

        private void OnEnable()
        {
            if(SafeWindowUpdate()) WindowActive = true;
        }

        private void OnDisable()
        {
            WindowActive = false;
        }

        /// <summary>
        ///     Changes window active state.
        /// </summary>
        /// <param name="active">Active state.</param>
        public void SetActive(bool active)
        {
            if (ShouldActivateGameObject)
            {
                gameObject.SetActive(active);
            }
            else
            {
                WindowActive = active;
            }

            if (active)
            {
                OnWindowActivated.Invoke();
            }
            else
            {
                OnWindowDeactivated.Invoke();
            }
        }

        /// <summary>
        ///     Register window canvas.
        /// </summary>
        public void RegisterWindowCanvas(Canvas canvas)
        {
            WindowCanvas = canvas;
        }

        /// <summary>
        ///     Safe window update...
        /// </summary>
        public bool SafeWindowUpdate()
        {
            if (WindowCanvas == null) RegisterWindowCanvas(GetComponentInParent<Canvas>());
            if (WindowCanvas == null) return false;
            UpdateDisplay();
            return true;
        }

        /// <summary>
        ///     Updates window.
        /// </summary>
        public void UpdateDisplay()
        {
            if (WindowCanvas == null) throw new NullReferenceException(nameof(WindowCanvas));
            var canvasTransform = WindowCanvas.GetComponent<RectTransform>();
            if (canvasTransform == null) return;

            #region SIZE_CLAMP

            if (ClampWindowSize)
            {
                if (RootTransform.sizeDelta.x < MinWindowSize.x)
                    RootTransform.sizeDelta = new Vector2(MinWindowSize.x, RootTransform.sizeDelta.y);
                if (RootTransform.sizeDelta.y < MinWindowSize.y)
                    RootTransform.sizeDelta = new Vector2(RootTransform.sizeDelta.x, MinWindowSize.y);
                if (RootTransform.sizeDelta.x > MaxWindowSize.x)
                    RootTransform.sizeDelta = new Vector2(MaxWindowSize.x, RootTransform.sizeDelta.y);
                if (RootTransform.sizeDelta.y > MaxWindowSize.y)
                    RootTransform.sizeDelta = new Vector2(RootTransform.sizeDelta.x, MaxWindowSize.y);
            }

            #endregion

            #region SCREEN_CLAMP

            switch (CurrentAnchorName)
            {
                case JEMInterfaceWindowAnchorName.Unknown:
                    break;
                case JEMInterfaceWindowAnchorName.TopLeft:
                {
                    var minScreen = new Vector2(RootTransform.sizeDelta.x / 2f, -(RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f,
                        -(canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.Top:
                {
                    var minScreen = new Vector2(-(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f),
                        -(RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f,
                        -(canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.TopRight:
                {
                    var minScreen = new Vector2(-RootTransform.sizeDelta.x / 2f, -(RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x > minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(-(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f),
                        -(canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x < maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.MiddleLeft:
                {
                    var minScreen = new Vector2(RootTransform.sizeDelta.x / 2f,
                        canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f,
                        -(canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.Middle:
                {
                    var minScreen = new Vector2(
                        -(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f),
                        canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f);
                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f,
                        -(canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.MiddleRight:
                {
                    var minScreen = new Vector2(-RootTransform.sizeDelta.x / 2f,
                        canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y > minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x > minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(-(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f),
                        -(canvasTransform.sizeDelta.y / 2f - RootTransform.sizeDelta.y / 2f));

                    if (RootTransform.anchoredPosition.y < maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x < maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.BottomLeft:
                {
                    var minScreen = new Vector2(RootTransform.sizeDelta.x / 2f, RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y < minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f,
                        canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y > maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.Bottom:
                {
                    var minScreen = new Vector2(
                        -(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f),
                        RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y < minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x < minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(canvasTransform.sizeDelta.x / 2f - RootTransform.sizeDelta.x / 2f,
                        canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y > maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x > maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                case JEMInterfaceWindowAnchorName.BottomRight:
                {
                    var minScreen = new Vector2(-RootTransform.sizeDelta.x / 2f, RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y < minScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, minScreen.y);
                    if (RootTransform.anchoredPosition.x > minScreen.x)
                        RootTransform.anchoredPosition = new Vector2(minScreen.x, RootTransform.anchoredPosition.y);

                    var maxScreen = new Vector2(-(canvasTransform.sizeDelta.x - RootTransform.sizeDelta.x / 2f),
                        canvasTransform.sizeDelta.y - RootTransform.sizeDelta.y / 2f);

                    if (RootTransform.anchoredPosition.y > maxScreen.y)
                        RootTransform.anchoredPosition = new Vector2(RootTransform.anchoredPosition.x, maxScreen.y);
                    if (RootTransform.anchoredPosition.x < maxScreen.x)
                        RootTransform.anchoredPosition = new Vector2(maxScreen.x, RootTransform.anchoredPosition.y);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion
        }

        private void LateUpdate()
        {
            if (!AlwaysMoveOnTop) return;
            var l = RootTransform.InverseTransformPoint(Input.mousePosition);
            if (!RootTransform.rect.Contains(l)) return;
            if (Input.GetMouseButtonDown(0))
            {
                //MoveOnTop();
            }
        }

        /// <summary>
        ///     Restarts this window.
        /// </summary>
        public void Restart()
        {
            if (AnyWindowIsUnderMotion) JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);

            var headers = GetComponentsInChildren<JEMInterfaceWindowHeader>();
            foreach (var h in headers) h.Restart();

            var size = GetComponentsInChildren<JEMInterfaceWindowResize>();
            foreach (var s in size) s.Restart();
        }

        /// <summary>
        ///     Gets Rect of this window.
        /// </summary>
        public Rect GetRect()
        {
            return new Rect(RootTransform.anchoredPosition, RootTransform.sizeDelta);
        }

        /// <summary>
        ///     Sets rect of this window.
        /// </summary>
        public void SetRect(Rect rect)
        {
            RootTransform.anchoredPosition = rect.position;
            RootTransform.sizeDelta = rect.size;
        }

        /// <summary>
        ///     Moves window on top in local container.
        /// </summary>
        public void MoveOnTop()
        {
            RootTransform.SetAsLastSibling();
        }

        /// <summary>
        ///     Restarts all windows.
        /// </summary>
        public static void RestartAll()
        {
            var windows = FindObjectsOfType<JEMInterfaceWindow>();
            foreach (var window in windows) window.Restart();
        }

        /// <summary>
        ///     True if any window is currently moving or re-sized by user.
        /// </summary>
        public static bool AnyWindowIsUnderMotion => JEMInterfaceWindowHeader.AnyHeaderIsDragging || JEMInterfaceWindowResize.AnyWindowIsResized;
    }
}