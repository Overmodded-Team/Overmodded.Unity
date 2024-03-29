﻿//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;
using UnityEngine.UI;

namespace JEM.UnityEngine.Interface.Control
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface slider gradient.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    [ExecuteInEditMode]
    public sealed class JEMInterfaceSliderGradient : MonoBehaviour
    {
        /// <summary>
        ///     Target image.
        /// </summary>
        [Header("Slider Gradient")]
        public Image Target;

        /// <summary>
        ///     Gradient.
        /// </summary>
        public Gradient Gradient;

        /// <summary>
        ///     Speed of color interpolation.
        ///     If set to zero, interpolation will be ignored.
        /// </summary>
        public float Smooth = 5f;

        /// <summary>
        ///     Current slider.
        /// </summary>
        public Slider Slider { get; private set; }

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }

        private void Update()
        {
            if (Target == null)
                return;

            if (Math.Abs(Smooth) > float.Epsilon)
                Target.color = Color.Lerp(Target.color, Gradient.Evaluate(Slider.value), Time.deltaTime * Smooth);
            else
                Target.color = Gradient.Evaluate(Slider.value);
        }
    }
}