//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace Overmodded.Gameplay.Character.Rendering
{
    /// <inheritdoc />
    /// <summary>
    ///     Character model.
    ///     Contains all references to character's objects used by game.
    /// </summary>
    [DisallowMultipleComponent]
    public class CharacterModel : ObjectModelBehaviour
    {
        [Header("References")]
        public Transform TitlePoint;

        [Space]
        public Transform LeftWeaponDeploy;
        public Transform RightWeaponDeploy;

        [Header("References (Basic Bones)")]
        public Transform SpineTransform;
        public Transform ChestTransform;
        public Transform HeadTransform;

        [Header("Locale Settings")]
        public float BaseRadius = 0.5f;

        [Header("Systems Settings")]
        public CharacterModelIKSettings IKSettings;
    }
}
