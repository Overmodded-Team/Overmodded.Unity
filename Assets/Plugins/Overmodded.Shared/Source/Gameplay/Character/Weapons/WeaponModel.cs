//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Weapons
{
    [Serializable]
    public class WeaponBonesReferences
    {
        [Header("Base")]
        public Transform WeaponRoot;
        public Transform WeaponMagazine;
    }

    /// <summary>
    ///     View type of weapon used by WeaponModel. It defines how target model can be drawn.
    /// </summary>
    public enum WeaponViewType
    {
        Unknown,
        FirstPerson,
        ThirdPerson
    }

    /// <inheritdoc />
    /// <summary>
    ///     Weapon model helper.
    ///     Contains all components references that need to be used by the system.
    /// </summary>
    public class WeaponModel : ObjectModelBehaviour
    {
        /// <summary>
        ///     View type of weapon.
        /// </summary>
        [Header("View")]
        public WeaponViewType ViewType = WeaponViewType.FirstPerson;

        /// <summary>
        ///     The transform that is being used for bullet spawning.
        /// </summary>
        /// <remarks>Actually this is used only for particles and sfx.</remarks>
        [Tooltip("References")]
        public Transform MuzzleTransform;

        /// <summary>
        ///     The weapon bones references.
        /// </summary>
        [Space]
        public WeaponBonesReferences BonesReferences = new WeaponBonesReferences();

        /// <summary>
        ///     Name of fire animation trigger.
        /// </summary>
        [Header("Animation")]
        public string FireAnimation;

        private void OnValidate()
        {
            Debug.Assert(MuzzleTransform, $"Weapon model {name} does not have MuzzleTransform reference set!");
            Debug.Assert(BonesReferences.WeaponRoot, $"Weapon model {name} does not have WeaponRoot reference set!");
            Debug.Assert(BonesReferences.WeaponMagazine, $"Weapon model {name} does not have WeaponMagazine reference set!");
        }
    }
}
