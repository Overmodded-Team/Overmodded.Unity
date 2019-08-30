//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Gameplay.Character;
using Overmodded.Unity.Editor.Common;
using System;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    /// <inheritdoc />
    [CustomEditor(typeof(CharacterSettings), true, isFallback = true)]
    public class CharacterSettingsEditor : DatabaseItemEditor
    {
        private SerializedProperty Icon;
        private SerializedProperty LocaleName;
        private SerializedProperty StatisticsPrefab;

        private SerializedProperty ModelPrefab;
        private SerializedProperty HandsPrefab;

        private SerializedProperty DefaultAnimatorController;

        private SavedBool _drawMainSettings;
        private SavedBool _drawResourcesSettings;
        private SavedBool _drawAnimationSettings;
        
        /// <inheritdoc />
        protected override void OnEnable()
        {
            // invoke base method
            base.OnEnable();

            _drawMainSettings = new SavedBool($"{nameof(CharacterSettings)}.DrawMainSettings", false);
            _drawResourcesSettings = new SavedBool($"{nameof(CharacterSettings)}.DrawResourcesSettings", false);
            _drawAnimationSettings = new SavedBool($"{nameof(CharacterSettings)}.DrawAnimationSettings", false);

            Icon = serializedObject.FindProperty(nameof(Icon));
            LocaleName = serializedObject.FindProperty(nameof(LocaleName));
            StatisticsPrefab = serializedObject.FindProperty(nameof(StatisticsPrefab));

            ModelPrefab = serializedObject.FindProperty(nameof(ModelPrefab));
            HandsPrefab = serializedObject.FindProperty(nameof(HandsPrefab));

            DefaultAnimatorController = serializedObject.FindProperty(nameof(DefaultAnimatorController));
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 
            // SECTION: Main Settings
            //
            _drawMainSettings.value = DrawSectionGUI(_drawMainSettings.value, "Main Settings", () =>
            {
                EditorGUILayout.PropertyField(Icon);
                EditorGUILayout.PropertyField(LocaleName);
                EditorGUILayout.PropertyField(StatisticsPrefab);
            });

            //
            // SECTION: Resources Settings
            //
            _drawResourcesSettings.value = DrawSectionGUI(_drawResourcesSettings.value, "Resources Settings", () =>
            {
                EditorGUILayout.PropertyField(ModelPrefab);
                EditorGUILayout.PropertyField(HandsPrefab);
            });

            //
            // SECTION: Animations Settings
            //
            _drawAnimationSettings.value = DrawSectionGUI(_drawAnimationSettings.value, "Animation Settings", () =>
            {
                DefaultAnimatorController.intValue = EditorGUILayoutGameUtility.AnimationSettingsField("Default Animator", DefaultAnimatorController.intValue);
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
