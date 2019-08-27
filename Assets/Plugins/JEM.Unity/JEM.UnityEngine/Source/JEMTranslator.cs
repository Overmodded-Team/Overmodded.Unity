//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections;
using UnityEngine;

#pragma warning disable 1591

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class JEMTranslator : MonoBehaviour
    {
        [Header("Settings")]
        public Transform StartPoint;
        public Transform EndPoint;
        public float Speed = 5f;

        public bool State { get; private set; }

        private Transform _transform;
        private Coroutine _slave;

        private void OnDisable()
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();

            // script or gameobject has been disabled
            // force new state
            _transform.position = State ? EndPoint.position : StartPoint.position;
        }

        /// <summary>
        /// Sets state of translator.
        /// </summary>
        public void SetState(bool state)
        {
            if (State == state)
                return;

            State = state;
            if (_slave != null)
                StopCoroutine(_slave);
            _slave = StartCoroutine(Slave(state));
        }

        /// <summary>
        ///     Force current state.
        /// </summary>
        public void ForceState()
        {
            if (_slave != null)
                StopCoroutine(_slave);

            _transform.position = State ? EndPoint.position : StartPoint.position;
        }

        private IEnumerator Slave(bool activeState)
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();

            if (StartPoint != null && EndPoint != null)
            {
                var tPoint = activeState ? EndPoint : StartPoint;
                while (Vector3.Distance(_transform.position, tPoint.position) > 0.1f)
                {
                    var t = Time.deltaTime * Speed;
                    _transform.position = Vector3.Lerp(_transform.position, tPoint.position, t);
                    yield return new WaitForEndOfFrame();
                }
            }
            _slave = null;
        }
    }
}
