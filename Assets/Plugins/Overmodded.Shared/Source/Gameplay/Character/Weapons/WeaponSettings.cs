//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.UnityEngine;
using Overmodded.Common;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Weapons
{
    /// <summary>
    ///     Weapon type used by WeaponSettings. It defines what type of weapon controller should be used.
    /// </summary>
    public enum WeaponType
    {
        /// <summary>
        ///     Unknown weapon type. Should be used.
        /// </summary>
        Unknown,

        /// <summary>
        ///     Default rifle weapon type.
        /// </summary>
        Rifle
    }

    [CreateAssetMenu(fileName = "newWeaponSettings", menuName = "Overmodded/Weapon")]
    public class WeaponSettings : DatabaseItem
    {
        [Header("Controller")]
        public WeaponType Type = WeaponType.Rifle;
        public int AnimatorController = 0;

        [Header("Resources")]
        public WeaponModel FPPModel;
        public WeaponModel TPPModel;

        /// <summary>
        ///     ... It's RPM?
        /// </summary>
        [Header("Fire settings")]
        [Tooltip("... It's RPM?")]
        public int RoundsPerMinute = 600;

        [Header("Bullet settings")]
        public float BulletVelocity = 100f;
        public float BulletVelocityTimeout = 1f;
        public float BulletGravity = 1f;
        public float BulletRadius = 0.2f;
        public float BulletRaycastDistance = 5f;
        public float BulletDamage = 25f;
        public float BulletTimeout = 3f;

        /// <summary>
        ///     It defines limit between last and next shooting frame. If it is to high, recoil and spread will be restarted to default state.
        /// </summary>
        [Tooltip("It defines limit between last and next shooting frame. " +
                 "If it is to high, recoil and spread will be restarted to default state.")]
        public int RecoilAndSpreadFramesLimit = 30;

        // TODO: FireMode Auto/SemiAuto/Single

        /// <summary>
        ///     The amount of seconds that the reload takes.
        /// </summary>
        [Header("Reload settings")]
        [Tooltip("The amount of seconds that the reload takes.")]
        public float ReloadTime = 3.0f;

        #region CAMERA SHAKE

        /// <summary>
        ///     The camera shake interpolation from last point value.
        /// </summary>
        [Header("Camera Shake")]
        public float CameraShakeFromLast = 15.0f;

        /// <summary>
        ///     The camera shake interpolation to next point value.
        /// </summary>
        public float CameraShakeToNext = 8.0f;

        #endregion

        #region BULLET SPREAD

        /// <summary>
        ///     The spread warmup time.
        ///     It defines how much it takes to fully warmup spread effectiveness.
        /// </summary>
        [Header("Bullet spread settings")]
        [Tooltip("The spread warmup time." +
                 "It defines how much it takes to fully warmup spread effectiveness.")]
        public float SpreadWarmupTime = 15f;

        /// <summary>
        ///     The minimal horizontal bullet spread.
        ///     When SpreadTimeModEnabled is enabled this value will add horizontal spread to all fired bullets.
        /// </summary>
        [Tooltip("The minimal horizontal bullet spread." +
                 "This value will add horizontal spread to all fired bullets.")]
        public float HorizontalSpread = 3.0f;

        /// <summary>
        ///     The minimal vertical bullet spread.
        ///     When SpreadTimeModEnabled is enabled this value will add vertical spread to all fired bullets.
        /// </summary>
        [Tooltip("The minimal vertical bullet spread." +
                 "This value will add vertical spread to all fired bullets.")]
        public float VerticalSpread = 0.5f;

        /// <summary>
        ///     The overtime horizontal spread grow value.
        ///     When SpreadTimeModEnabled is enabled this value will add additional horizontal spread to all fired bullets over time.
        /// </summary>
        [Tooltip("The overtime horizontal spread grow value." +
                 "When SpreadTimeModEnabled is enabled this value will add additional horizontal spread to all fired bullets over time.")]
        public float HorizontalSpreadTimeMod = 0.1f;

        /// <summary>
        ///     The overtime horizontal spread grow cooldown value.
        /// </summary>
        [Tooltip("The overtime horizontal spread grow cooldown value.")]
        public float HorizontalSpreadTimeModCooldown = 10.0f;

        /// <summary>
        ///     The overtime vertical spread grow value.
        ///     When SpreadTimeModEnabled is enabled this value will add additional vertical spread to all fired bullets over time.
        /// </summary>
        [Tooltip("The overtime vertical spread grow value." +
                 "When SpreadTimeModEnabled is enabled this value will add additional vertical spread to all fired bullets over time.")]
        public float VerticalSpreadTimeMod = 0.1f;

        /// <summary>
        ///     The overtime vertical spread grow cooldown value.
        /// </summary>
        [Tooltip("The overtime vertical spread grow cooldown value.")]
        public float VerticalSpreadTimeModCooldown = 10.0f;

        /// <summary>
        ///     The overtime enable/disable flag.
        /// When false, SpreadTimeMod values won't add any effect.
        /// </summary>
        [Tooltip("The overtime enable/disable flag. " +
                 "When false, SpreadTimeMod values won't add any effect.")]
        public bool SpreadTimeModEnabled = true;

        #endregion

        #region BULLET RECOIL

        /// <summary>
        ///     The recoil warmup time.
        ///     It defines how much it takes to fully warmup recoil effectiveness.
        /// </summary>
        [Header("Bullet recoil settings")]
        [Tooltip("The recoil warmup time." +
                 "It defines how much it takes to fully warmup recoil effectiveness.")]
        public float RecoilWarmupTime = 15f;

        /// <summary>
        ///     The overtime horizontal recoil grow value.
        ///     This value will add horizontal recoil to all fired bullets over time.
        /// </summary>
        [Tooltip("The overtime horizontal recoil grow value." +
                 "This value will add horizontal recoil to all fired bullets over time.")]
        public float HorizontalRecoilTimeMod = 1.0f;

        /// <summary>
        ///     The overtime horizontal recoil grow cooldown value.
        /// </summary>
        [Tooltip("The overtime horizontal recoil grow cooldown value.")]
        public float HorizontalRecoilTimeModCooldown = 10.0f;

        /// <summary>
        ///     The overtime vertical recoil grow value.
        ///     This value will add vertical recoil to all fired bullets over time.
        /// </summary>
        [Tooltip("The overtime vertical recoil grow value." +
                 "This value will add vertical recoil to all fired bullets over time.")]
        public float VerticalRecoilTimeMod = 20.0f;

        /// <summary>
        ///     The overtime vertical recoil grow cooldown value.
        /// </summary>
        [Tooltip("The overtime vertical recoil grow cooldown value.")]
        public float VerticalRecoilTimeModCooldown = 7.0f;

        #endregion

        [Header("State settings")]
        public bool CanFireWhileRunning = false;

        public float RunSpreadMod = 1.3f;
        public float RunRecoilMod = 1.3f;

        public float MoveSpreadMod = 2f;
        public float MoveRecoilMod = 2f;

        public float CrouchSpreadMod = 0.8f;
        public float CrouchRecoilMod = 0.8f;

        public float AimSpreadMod = 0.7f;
        public float AimRecoilMod = 0.7f;

        /// <summary>
        ///     The fire sounds.
        /// </summary>
        [Header("Audio settings")]
        [Tooltip("The fire sounds.")]
        public AudioClip[] FireSounds;

        /// <summary>
        ///     The fire sound pitch randomization range.
        /// </summary>
        [Tooltip("The fire sound pitch randomization range.")]
        [Range(0.0f, 0.5f)]
        public float FireSoundPitchRange = 0.2f;

        private void OnValidate()
        {
            Debug.Assert(FPPModel, $"Weapon {name} is missing FPP weapon model.");
            Debug.Assert(TPPModel, $"Weapon {name} is missing TPP weapon model.");

            if (FPPModel && FPPModel.ViewType != WeaponViewType.FirstPerson)
                Debug.LogError($"Weapon {name} has set FPP weapon model but has invalid weapon view type set.");

            if (TPPModel && TPPModel.ViewType != WeaponViewType.ThirdPerson)
                Debug.LogError($"Weapon {name} has set TPP weapon model but has invalid weapon view type set.");
        }

        /// <summary>
        ///     The amount of frames that the reload takes.
        /// </summary>
        public int ReloadFrames => Mathf.CeilToInt(ReloadTime / Time.fixedDeltaTime);

        /// <summary>
        ///     The amount of frames that the fire interval takes.
        /// </summary>
        public int FireIntervalFrames => Mathf.Clamp(Mathf.CeilToInt(1.0f / (RoundsPerMinute / 60.0f) / Time.fixedDeltaTime), 1, QNetTime.TickRate);
    }
}
