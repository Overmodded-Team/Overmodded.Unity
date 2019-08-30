//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using Overmodded.Gameplay.Character.Rendering;
using UnityEngine;

namespace Overmodded.Gameplay.Character
{
    [CreateAssetMenu(fileName = "newAnimatorSettings", menuName = "Overmodded/Character/Animator Settings", order = 0)]
    public class CharacterAnimatorSettings : DatabaseItem
    {
        [Header("Settings")]
        public RuntimeAnimatorController CharacterController;
        public HandsModel HandsSimulationModel;

        [Header("Triggers")]
        public string AnimatorFire = "Fire";
        public string AnimatorReload = "Reload";
        public string AnimatorHide = "Hide";
        public string AnimatorAim = "Aim";
    }
}
