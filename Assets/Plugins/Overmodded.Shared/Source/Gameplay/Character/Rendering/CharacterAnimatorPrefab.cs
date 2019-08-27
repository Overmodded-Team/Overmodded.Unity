//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Rendering
{
    [CreateAssetMenu(fileName = "newAnimatorPrefab", menuName = "Overmodded/Character/Animator Prefab", order = 0)]
    public class CharacterAnimatorPrefab : DatabaseItem
    {
        [Header("Settings")]
        public string Name = "Unknown";
        public RuntimeAnimatorController CharacterController;
        public HandsModel HandsSimulationModel;

        [Header("Triggers")]
        public string AnimatorFire = "Fire";
        public string AnimatorReload = "Reload";
        public string AnimatorHide = "Hide";
        public string AnimatorAim = "Aim";
    }
}
