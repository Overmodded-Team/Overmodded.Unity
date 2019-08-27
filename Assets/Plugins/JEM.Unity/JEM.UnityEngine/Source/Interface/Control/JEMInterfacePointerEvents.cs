//
// RPG Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JEM.UnityEngine.Interface.Control
{
    /// <inheritdoc cref="MonoBehaviour" />
    /// <summary>
    ///     Interface pointer events chandler.
    /// </summary>
    public class JEMInterfacePointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Events")]
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        public UnityEvent OnDown;
        public UnityEvent OnUp;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            OnExit.Invoke();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnDown.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnUp.Invoke();
        }
    }
}
