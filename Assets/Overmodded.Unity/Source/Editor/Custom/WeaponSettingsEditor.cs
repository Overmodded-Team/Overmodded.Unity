//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Gameplay.Character.Weapons;
using Overmodded.Unity.Editor.Common;
using System;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    /// <inheritdoc />
    [CustomEditor(typeof(WeaponSettings), true, isFallback = true)]
    public class WeaponSettingsEditor : DatabaseItemEditor
    {
        private SerializedProperty Type;
        private SerializedProperty AnimatorController;

        private SerializedProperty FPPModel;
        private SerializedProperty TPPModel;

        private SerializedProperty RoundsPerMinute;
        private SerializedProperty RecoilAndSpreadFramesLimit;
        private SerializedProperty CanFireWhileRunning;

        private SerializedProperty BulletVelocity;
        private SerializedProperty BulletVelocityTimeout;
        private SerializedProperty BulletGravity;
        private SerializedProperty BulletRadius;
        private SerializedProperty BulletRaycastDistance;
        private SerializedProperty BulletDamage;
        private SerializedProperty BulletTimeout;

        private SerializedProperty ReloadTime;

        private SerializedProperty CameraShakeFromLast;
        private SerializedProperty CameraShakeToNext;

        private SerializedProperty SpreadWarmupTime;
        private SerializedProperty HorizontalSpread;
        private SerializedProperty VerticalSpread;
        private SerializedProperty HorizontalSpreadTimeMod;
        private SerializedProperty HorizontalSpreadTimeModCooldown;
        private SerializedProperty VerticalSpreadTimeMod;
        private SerializedProperty VerticalSpreadTimeModCooldown;
        private SerializedProperty SpreadTimeModEnabled;

        private SerializedProperty RecoilWarmupTime;
        private SerializedProperty HorizontalRecoilTimeMod;
        private SerializedProperty HorizontalRecoilTimeModCooldown;
        private SerializedProperty VerticalRecoilTimeMod;
        private SerializedProperty VerticalRecoilTimeModCooldown;

        private SerializedProperty RunSpreadMod;
        private SerializedProperty RunRecoilMod;
        private SerializedProperty MoveSpreadMod;
        private SerializedProperty MoveRecoilMod;
        private SerializedProperty CrouchSpreadMod;
        private SerializedProperty CrouchRecoilMod;
        private SerializedProperty AimSpreadMod;
        private SerializedProperty AimRecoilMod;

        private SerializedProperty FireSounds;
        private SerializedProperty FireSoundPitchRange;

        private SavedBool _drawControllerSettings;
        private SavedBool _drawResourcesSettings;
        private SavedBool _drawFireSettings;
        private SavedBool _drawBulletSettings;
        private SavedBool _drawReloadSettings;
        private SavedBool _drawCameraSettings;
        private SavedBool _drawBulletSpreadSettings;
        private SavedBool _drawBulletRecoilSettings;
        private SavedBool _drawStateSettings;
        private SavedBool _drawAudioSettings;

        /// <inheritdoc />
        protected override void OnEnable()
        {
            // invoke base method
            base.OnEnable();

            _drawControllerSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawControllerSettings", false);
            _drawResourcesSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawResourcesSettings", false);
            _drawFireSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawFireSettings", false);
            _drawBulletSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawBulletSettings", false);
            _drawReloadSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawReloadSettings", false);
            _drawCameraSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawCameraSettings", false);
            _drawBulletSpreadSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawBulletSpreadSettings", false);
            _drawBulletRecoilSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawBulletRecoilSettings", false);
            _drawStateSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawStateSettings", false);
            _drawAudioSettings = new SavedBool($"{nameof(WeaponSettings)}.DrawAudioSettings", false);

            Type = serializedObject.FindProperty(nameof(Type));
            AnimatorController = serializedObject.FindProperty(nameof(AnimatorController));

            FPPModel = serializedObject.FindProperty(nameof(FPPModel));
            TPPModel = serializedObject.FindProperty(nameof(TPPModel));

            RoundsPerMinute = serializedObject.FindProperty(nameof(RoundsPerMinute));
            RecoilAndSpreadFramesLimit = serializedObject.FindProperty(nameof(RecoilAndSpreadFramesLimit));
            CanFireWhileRunning = serializedObject.FindProperty(nameof(CanFireWhileRunning));

            BulletVelocity = serializedObject.FindProperty(nameof(BulletVelocity));
            BulletVelocityTimeout = serializedObject.FindProperty(nameof(BulletVelocityTimeout));
            BulletGravity = serializedObject.FindProperty(nameof(BulletGravity));
            BulletRadius = serializedObject.FindProperty(nameof(BulletRadius));
            BulletRaycastDistance = serializedObject.FindProperty(nameof(BulletRaycastDistance));
            BulletDamage = serializedObject.FindProperty(nameof(BulletDamage));
            BulletTimeout = serializedObject.FindProperty(nameof(BulletTimeout));

            ReloadTime = serializedObject.FindProperty(nameof(ReloadTime));

            CameraShakeFromLast = serializedObject.FindProperty(nameof(CameraShakeFromLast));
            CameraShakeToNext = serializedObject.FindProperty(nameof(CameraShakeToNext));

            SpreadWarmupTime = serializedObject.FindProperty(nameof(SpreadWarmupTime));
            HorizontalSpread = serializedObject.FindProperty(nameof(HorizontalSpread));
            VerticalSpread = serializedObject.FindProperty(nameof(VerticalSpread));
            HorizontalSpreadTimeMod = serializedObject.FindProperty(nameof(HorizontalSpreadTimeMod));
            HorizontalSpreadTimeModCooldown = serializedObject.FindProperty(nameof(HorizontalSpreadTimeModCooldown));
            VerticalSpreadTimeMod = serializedObject.FindProperty(nameof(VerticalSpreadTimeMod));
            VerticalSpreadTimeModCooldown = serializedObject.FindProperty(nameof(VerticalSpreadTimeModCooldown));
            SpreadTimeModEnabled = serializedObject.FindProperty(nameof(SpreadTimeModEnabled));

            RecoilWarmupTime = serializedObject.FindProperty(nameof(RecoilWarmupTime));
            HorizontalRecoilTimeMod = serializedObject.FindProperty(nameof(HorizontalRecoilTimeMod));
            HorizontalRecoilTimeModCooldown = serializedObject.FindProperty(nameof(HorizontalRecoilTimeModCooldown));
            VerticalRecoilTimeMod = serializedObject.FindProperty(nameof(VerticalRecoilTimeMod));
            VerticalRecoilTimeModCooldown = serializedObject.FindProperty(nameof(VerticalRecoilTimeModCooldown));

            RunSpreadMod = serializedObject.FindProperty(nameof(RunSpreadMod));
            RunRecoilMod = serializedObject.FindProperty(nameof(RunRecoilMod));
            MoveSpreadMod = serializedObject.FindProperty(nameof(MoveSpreadMod));
            MoveRecoilMod = serializedObject.FindProperty(nameof(MoveRecoilMod));
            CrouchSpreadMod = serializedObject.FindProperty(nameof(CrouchSpreadMod));
            CrouchRecoilMod = serializedObject.FindProperty(nameof(CrouchRecoilMod));
            AimSpreadMod = serializedObject.FindProperty(nameof(AimSpreadMod));
            AimRecoilMod = serializedObject.FindProperty(nameof(AimRecoilMod));

            FireSounds = serializedObject.FindProperty(nameof(FireSounds));
            FireSoundPitchRange = serializedObject.FindProperty(nameof(FireSoundPitchRange));
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //
            // SECTION: Controller Settings
            //
            _drawControllerSettings.value = DrawSectionGUI(_drawControllerSettings.value, "Controller Settings", () =>
            {
                EditorGUILayout.PropertyField(Type);
                AnimatorController.intValue = EditorGUILayoutGameUtility.AnimationSettingsField("Animator Controller", AnimatorController.intValue);
            });

            //
            // SECTION: Resources Settings
            //
            _drawResourcesSettings.value = DrawSectionGUI(_drawResourcesSettings.value, "Resources Settings", () =>
            {
                EditorGUILayout.PropertyField(FPPModel);
                EditorGUILayout.PropertyField(TPPModel);
            });

            //
            // SECTION: Fire Settings
            //
            _drawFireSettings.value = DrawSectionGUI(_drawFireSettings.value, "Fire Settings", () =>
            {
                EditorGUILayout.PropertyField(RoundsPerMinute);
                EditorGUILayout.PropertyField(RecoilAndSpreadFramesLimit);
                EditorGUILayout.PropertyField(CanFireWhileRunning);
            });

            //
            // SECTION: Bullet Settings
            //
            _drawBulletSettings.value = DrawSectionGUI(_drawBulletSettings.value, "Bullet Settings", () =>
            {
                EditorGUILayout.PropertyField(BulletVelocity);
                EditorGUILayout.PropertyField(BulletVelocityTimeout);
                EditorGUILayout.PropertyField(BulletGravity);
                EditorGUILayout.PropertyField(BulletRadius);
                EditorGUILayout.PropertyField(BulletRaycastDistance);
                EditorGUILayout.PropertyField(BulletDamage);
                EditorGUILayout.PropertyField(BulletTimeout);
            });

            //
            // SECTION: Reload Settings
            //
            _drawReloadSettings.value = DrawSectionGUI(_drawReloadSettings.value, "Reload Settings", () =>
            {
                EditorGUILayout.PropertyField(ReloadTime);
            });

            //
            // SECTION: Camera Settings
            // 
            _drawCameraSettings.value = DrawSectionGUI(_drawCameraSettings.value, "Camera Settings", () =>
            {
                EditorGUILayout.PropertyField(CameraShakeFromLast);
                EditorGUILayout.PropertyField(CameraShakeToNext);
            });

            //
            // SECTION: Bullet Spread Settings
            //
            _drawBulletSpreadSettings.value = DrawSectionGUI(_drawBulletSpreadSettings.value, "Bullet Spread Settings", () =>
            {
                EditorGUILayout.PropertyField(SpreadWarmupTime);
                EditorGUILayout.PropertyField(HorizontalSpread);
                EditorGUILayout.PropertyField(VerticalSpread);
                EditorGUILayout.PropertyField(HorizontalSpreadTimeMod);
                EditorGUILayout.PropertyField(HorizontalSpreadTimeModCooldown);
                EditorGUILayout.PropertyField(VerticalSpreadTimeMod);
                EditorGUILayout.PropertyField(VerticalSpreadTimeModCooldown);
                EditorGUILayout.PropertyField(SpreadTimeModEnabled);
            });

            //
            // SECTION: Bullet Recoil Settings
            //
            _drawBulletRecoilSettings.value = DrawSectionGUI(_drawBulletRecoilSettings.value, "Bullet Recoil Settings", () =>
            {
                EditorGUILayout.PropertyField(RecoilWarmupTime);
                EditorGUILayout.PropertyField(HorizontalRecoilTimeMod);
                EditorGUILayout.PropertyField(HorizontalRecoilTimeModCooldown);
                EditorGUILayout.PropertyField(VerticalRecoilTimeMod);
                EditorGUILayout.PropertyField(VerticalRecoilTimeModCooldown);
            });

            //
            // SECTION: State Settings
            //
            _drawStateSettings.value = DrawSectionGUI(_drawStateSettings.value, "State Settings", () =>
            {
                EditorGUILayout.PropertyField(RunSpreadMod);
                EditorGUILayout.PropertyField(RunRecoilMod);
                EditorGUILayout.PropertyField(MoveSpreadMod);
                EditorGUILayout.PropertyField(MoveRecoilMod);
                EditorGUILayout.PropertyField(CrouchSpreadMod);
                EditorGUILayout.PropertyField(CrouchRecoilMod);
                EditorGUILayout.PropertyField(AimSpreadMod);
                EditorGUILayout.PropertyField(AimRecoilMod);
            });

            //
            // SECTION: Audio Settings
            //
            _drawAudioSettings.value = DrawSectionGUI(_drawAudioSettings.value, "Audio Settings", () =>
            {
                EditorGUILayout.PropertyField(FireSounds, true);
                EditorGUILayout.PropertyField(FireSoundPitchRange);
            });

            // And draw the content...
            DrawDatabaseItemGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private static bool DrawSectionGUI(bool draw, string name, Action onGUI)
        {
            draw = EditorGUILayout.BeginFoldoutHeaderGroup(draw, name);
            if (draw)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                onGUI.Invoke();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            return draw;
        }
    }
}
