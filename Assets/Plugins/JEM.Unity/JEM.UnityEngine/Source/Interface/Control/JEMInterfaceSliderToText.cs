//
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
    ///     Apply value from slider to text.
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Slider)), ExecuteInEditMode]
    public sealed class JEMInterfaceSliderToText : MonoBehaviour
    {
        /// <summary>
        ///     Source text.
        /// </summary>
        [Header("Settings")]
        public Text Text;

        /// <summary>
        ///     Format of a sting.
        /// </summary>
        public string Format = "Some value: {0}";

        private Slider _slider;

        // awake event
        private void OnEnable()
        {
            if (_slider != null)
                return;
            _slider = GetComponent<Slider>();
            if (_slider == null)
                throw new NullReferenceException(nameof(_slider));
            _slider.onValueChanged.AddListener(value => { OnUpdateText(); });

            OnUpdateText();
        }

        private void OnUpdateText()
        {
            if (Text == null || _slider == null)
                return;

            Text.text = string.Format(Format, _slider.value);
        }
    }
}