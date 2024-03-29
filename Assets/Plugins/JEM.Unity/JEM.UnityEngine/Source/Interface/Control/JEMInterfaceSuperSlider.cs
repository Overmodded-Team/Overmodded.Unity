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
    ///     InterfaceSuperSlider.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class JEMInterfaceSuperSlider : MonoBehaviour
    {
        /// <summary>
        ///     Background slider.
        /// </summary>
        [Header("References")]
        public Slider SliderBackground;

        /// <summary>
        ///     Main slider.
        /// </summary>
        public Slider SliderForeground;

        [Space]
        public Text ValueText;
        public string ValueTextFormat = "{0}/{1}";
        public string ValueSingleFormat = "{0:0}";
        public float SingleMultiplier = 1f;

        /// <summary>
        ///     Speed of super slider background value.
        /// </summary>
        [Header("Motion")]
        public float Speed = 5f;

        /// <summary>
        ///     Value of super slider.
        /// </summary>
        [Header("Value")]
        [Range(0f, 1f)]
        public float Value = 1f;

        [Obsolete("This varible is obsolete. Please use 'Value' field instead.")]
        public float fillAmount
        {
            get => Value;
            set => Value = value;
        }

        [Space]
        public bool FromFullValues;
        public float ValueA;
        public float ValueB;

        private float _lastValue;

        private void LateUpdate()
        {
            if (SliderForeground == null || SliderForeground == null)
                return;

            if (FromFullValues)
            {
                Value = ValueA / ValueB;
            }

            Value = Mathf.Clamp01(Value);
            if (float.IsInfinity(Value) || float.IsNaN(Value))
                Value = 0f;

            if (_lastValue - Value > 0.01f)
            {
                SliderForeground.value = Value;
                SliderBackground.value = _lastValue;
            }
            else if (_lastValue - Value < -0.01f)
            {
                SliderForeground.value = _lastValue;
                SliderBackground.value = Value;
            }
            else
            {
                SliderForeground.value = Mathf.Lerp(SliderForeground.value, _lastValue, Time.deltaTime * Speed);
                SliderBackground.value = Mathf.Lerp(SliderBackground.value, _lastValue, Time.deltaTime * Speed);
            }

            _lastValue = Mathf.Lerp(_lastValue, Value, Time.deltaTime * Speed);
            if (ValueText != null && ValueText.isActiveAndEnabled)
            {
                if (FromFullValues)
                    ValueText.text = string.Format(ValueTextFormat, string.Format(ValueSingleFormat, ValueB * SliderForeground.value * SingleMultiplier), string.Format(ValueSingleFormat, ValueB * SingleMultiplier));
                else
                {
                    ValueText.text = string.Format(ValueSingleFormat, SliderForeground.value * SingleMultiplier);
                }
            }
        }

        /// <summary>
        ///     Forces new state of sliders.
        /// </summary>
        public void ForceNewState()
        {
            SliderForeground.value = Value;
            SliderBackground.value = Value;
        }
    }
}