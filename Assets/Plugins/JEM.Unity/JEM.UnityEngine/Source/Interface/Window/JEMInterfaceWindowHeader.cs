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
    ///     Interface window header components.
    ///     Defines draggable area of window.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class JEMInterfaceWindowHeader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        /// <summary>
        ///     Target window of this window header.
        /// </summary>
        [Header("Interface Window Header")]
        public JEMInterfaceWindow Window;

        /// <summary>
        ///     True if this window header can work with current window.
        /// </summary>
        public bool CanWorkWithWindow => Window != null && Window.WindowActive && Window.AllowDragging &&
                                         Window.RootTransform != null;

        private bool _isMouseDown;
        private bool _isMouseOver;

        private bool _isWindowMoved;
        private Vector3 _mouseStartPosition;

        private Vector3 _startPosition;

        /// <inheritdoc />
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!CanWorkWithWindow) return;
            if (JEMInterfaceWindow.AnyWindowIsUnderMotion) return;
            AnyHeaderIsDragging = true;

            if (Window.AlwaysMoveOnTop) Window.MoveOnTop();
            _isMouseDown = true;

            _startPosition = Window.RootTransform.position;
            _mouseStartPosition = Input.mousePosition;

            JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Move);
        }

        /// <inheritdoc />
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!CanWorkWithWindow) return;
            _isMouseOver = true;
            if (!JEMInterfaceWindow.AnyWindowIsUnderMotion)
                JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Move);
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
            AnyHeaderIsDragging = false;

            _isMouseDown = false;
            _isWindowMoved = false;

            JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);
        }

        /// <summary>
        ///     Restarts this window header.
        /// </summary>
        public void Restart()
        {
            if (_isWindowMoved || _isMouseDown || _isMouseOver)
                JEMInterfaceCursor.SetCursorIcon(JEMCursorIconName.Default);

            _isWindowMoved = false;
            _isMouseOver = false;
            _isMouseDown = false;
            AnyHeaderIsDragging = false;
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

            Window.RootTransform.position = new Vector3(_startPosition.x + delta.x, _startPosition.y + delta.y, 0f);
            Window.UpdateDisplay();
        }

        /// <summary>
        ///     True if any window header is currently moving by user.
        /// </summary>
        public static bool AnyHeaderIsDragging { get; private set; }
    }
}