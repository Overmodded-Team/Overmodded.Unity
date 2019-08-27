//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace JEM.UnityEngine.Scripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Hack and Slash camera movement controller.
    /// </summary>
    public class JEMExperimentalCamera : MonoBehaviour
    {
        /// <summary>
        ///     Smooth of center.
        /// </summary>
        public float CenterSmooth = 15f;

        /// <summary>
        ///     Efficiency of center transforms position.
        /// </summary>
        [Range(0f, 1.5f)] public float CenterTransformEfficiency = .5f;

        /// <summary>
        ///     List of center transforms.
        /// </summary>
        public List<Transform> CenterTransforms = new List<Transform>();

        /// <summary>
        ///     Character transform.
        /// </summary>
        [Header("Base")] public Transform Character;

        /// <summary>
        ///     Current distance od camera.
        /// </summary>
        [Header("Rest of Stuff")] [Range(.3f, 1f)]
        public float CurrentDistance = 1f;

        /// <summary>
        ///     Center offset while camera is near character.
        /// </summary>
        [Header("Distance")] public Vector3 NearCenter = new Vector3(0f, 1.5f, 0f);

        /// <summary>
        ///     Offset of camera to character.
        /// </summary>
        public Vector3 Offset = new Vector3(3f, 12f, 6f);

        private Vector3 RawCenter;

        /// <summary>
        ///     Sensitivity of scroll input.
        /// </summary>
        public float ScrollSensitivity = .5f;

        /// <summary>
        ///     Position smooth.
        /// </summary>
        [Header("Time")] public float Smooth = 15f;

        private Vector3 Center;

        private void LateUpdate()
        {
            if (Character == null) return;

            Center = NearCenter * (1f - CurrentDistance) + GetCenter();

            CurrentDistance -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
            CurrentDistance = Mathf.Clamp(CurrentDistance, .3f, 1f);

            transform.position = Vector3.Lerp(transform.position,
                Character.transform.position + Offset * CurrentDistance, Time.deltaTime * Smooth);
            transform.LookAtSmooth(Center, Smooth);
        }

        /// <summary>
        ///     Gets center point for camera look at.
        /// </summary>
        protected Vector3 GetCenter()
        {
            var target = Vector3.zero;
            if (CenterTransforms.Count != 0)
            {
                for (var index = 0; index < CenterTransforms.Count; index++)
                    if (CenterTransforms[index] == null)
                    {
                        CenterTransforms.RemoveAt(index);
                        index--;
                    }
                    else
                    {
                        target += CenterTransforms[index].transform.position;
                    }

                target /= CenterTransforms.Count;
                target -= Character.transform.position;
                target *= CurrentDistance;
                target *= CenterTransformEfficiency;
            }

            RawCenter = Vector3.Lerp(RawCenter, target, Time.smoothDeltaTime * CenterSmooth);
            return Character.transform.position + RawCenter;
        }

        /// <summary>
        ///     Removes center transform.
        /// </summary>
        /// <param name="t"></param>
        public void AddCenter(Transform t)
        {
            if (!CenterTransforms.Contains(t)) CenterTransforms.Add(t);
        }

        /// <summary>
        ///     Adds center transform.
        /// </summary>
        public void RemoveCenter(Transform t)
        {
            if (CenterTransforms.Contains(t)) CenterTransforms.Remove(t);
        }
    }
}