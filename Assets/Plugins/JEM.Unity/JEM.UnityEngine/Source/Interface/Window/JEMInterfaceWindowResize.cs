//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using UnityEngine.EventSystems;

namespace JEM.UnityEngine.Interface.Window
{
    /// <inheritdoc cref="MonoBehaviour" />
    /// <summary>
    ///     Interface resize (button) element.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class JEMInterfaceWindowResize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        private const float PositionMultiplier = 0.5f;
        private bool _isMouseDown;
        private bool _isMouseOver;

        private bool _isWindowMoved;
        private Vector3 _mouseStartPosition;

        private Vector3 _startPosition;
        private Vector3 _startSize;

        /// <summary>
        ///     Target window of this size element.
        /// </summary>
        [Header("Interface Size Element")]
        public JEMInterfaceWindow Window;

        /// <summary>
        ///     True if this size element can work with current window.
        /// </summary>
        public bool CanWorkWithWindow => Window != null && Window.AllowResize && Window.RootTransform != null;

        /// <summary>
        ///     True if any window is currently re-sized by user.
        /// </summary>
        public static bool AnyWindowIsResized { get; private set; }

        /// <inheritdoc />
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!CanWorkWithWindow) return;
            if (JEMInterfaceWindow.AnyWindowIsUnderMotion) return;
            AnyWindowIsResized = true;

            if (Window.AlwaysMoveOnTop) Window.MoveOnTop();
            _isMouseDown = true;

            _startPosition = Window.RootTransform.position;
            _startSize = Window.RootTransform.sizeDelta;
            _mouseStartPosition = Input.mousePosition;

            JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Resize);
        }

        /// <inheritdoc />
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!CanWorkWithWindow) return;
            _isMouseOver = true;
            if (!JEMInterfaceWindow.AnyWindowIsUnderMotion)
                JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Resize);
        }

        /// <inheritdoc />
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _isMouseOver = false;
            if (!_isWindowMoved)
                JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);
        }

        /// <inheritdoc />
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            AnyWindowIsResized = false;

            _isMouseDown = false;
            _isWindowMoved = false;

            JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);
        }

        /// <summary>
        ///     Restarts this window element.
        /// </summary>
        public void Restart()
        {
            if (_isWindowMoved || _isMouseDown)
                JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);

            _isWindowMoved = false;
            _isMouseOver = false;
            _isMouseDown = false;
            AnyWindowIsResized = false;
        }

        private void OnEnable()
        {
            Restart();
        }

        private void OnDisable()
        {
            Restart();
        }

        private void Update()
        {
            if (!CanWorkWithWindow) return;
            if (!_isMouseOver && !_isWindowMoved || !_isMouseDown) return;

            _isWindowMoved = true;
            var mousePoint = Input.mousePosition;
            var delta = new Vector2(mousePoint.x - _mouseStartPosition.x, mousePoint.y - _mouseStartPosition.y);

            Window.RootTransform.position = new Vector3(_startPosition.x + delta.x * PositionMultiplier,
                _startPosition.y + delta.y * PositionMultiplier, 0f);
            Window.RootTransform.sizeDelta = new Vector3(_startSize.x + delta.x, _startSize.y - delta.y, 0f);
            Window.UpdateDisplay();
        }
    }
}