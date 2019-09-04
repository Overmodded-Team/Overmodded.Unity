//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Character.Weapons;
using System;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Rendering
{
    /// <summary>
    ///     Contains bone references for hands models.
    ///     It is used to synchronize any hands animations with any models.
    /// </summary>
    [Serializable]
    public class HandsBonesReferences
    {
        [Header("Base")]
        public Transform Root;

        [Header("Left")]
        public Transform LeftArm;
        public Transform LeftElbow;
        public Transform LeftHand;

        [Header("Right")]
        public Transform RightArm;
        public Transform RightElbow;
        public Transform RightHand;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Hands model helper.
    ///     Contains all references of hands object used by the game.
    /// </summary>
    public class HandsModel : ObjectModelBehaviour
    {
        [Header("References")]
        public Transform LeftWeaponDeploy;
        public Transform RightWeaponDeploy;

        [Header("Basic Bones")]
        public HandsBonesReferences BasicBonesReferences = new HandsBonesReferences();

        [Header("Weapon Bones")]
        public WeaponBonesReferences WeaponBonesReferences = new WeaponBonesReferences();

        private void OnValidate()
        {
            Debug.Assert(LeftWeaponDeploy, $"Hands {name} does not have LeftWeaponDeploy transform set.");
            Debug.Assert(RightWeaponDeploy, $"Hands {name} does not have RightWeaponDeploy transform set.");

            // TODO: Validate references (brah..)
        }

        /// <inheritdoc />
        protected override void OnComponentsCollected(bool isFirst)
        {
            if (isFirst)
                return;

            // all hands renderers need have layer mask set to 'Hands'
            foreach (var r in Renderers)
            {
                if(r) r.gameObject.layer = LayerMask.NameToLayer("Hands");
            }

            // update render type
            SetModelRenderType(ModelRenderType.DefaultCulledNoMotionVectors);
        }
    }
}
