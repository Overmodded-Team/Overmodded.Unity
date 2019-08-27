//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Interface;
using SimpleLUI.API.Core;
using System;
using UnityEngine;

namespace SimpleLUI.JEM
{
    [SLUIComponent]
    public sealed class SLUIJEMFadeAnimation : SLUIComponent
    {
        public float animationSpeed
        {
            get => Original.AnimationSpeed;
            set => Original.AnimationSpeed = value;
        }

        public float animationEnterScale
        {
            get => Original.AnimationEnterScale;
            set => Original.AnimationEnterScale = value;
        }

        public float animationExitScale
        {
            get => Original.AnimationExitScale;
            set => Original.AnimationExitScale = value;
        }

        public string animationMode => Original.AnimationMode.ToString();

        internal JEMInterfaceFadeAnimation Original { get; private set; }

        public override Component OnLoadOriginalComponent()
        {
            return Original = OriginalGameObject.AddComponent<JEMInterfaceFadeAnimation>();
        }

        public void SetAnimationMode(string mode)
        {
            if (Enum.TryParse<JEMFadeAnimationMode>(mode, true, out var t))
            {
               SetAnimationMode(t);
            }
            else Debug.LogError($"Failed to parse '{mode}' in to {typeof(JEMFadeAnimationMode)}");
        }

        public void SetAnimationMode(JEMFadeAnimationMode mode) => Original.AnimationMode = mode;
    }
}
