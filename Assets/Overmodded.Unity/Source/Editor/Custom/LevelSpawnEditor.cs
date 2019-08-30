//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Level;
using Overmodded.Unity.Editor.Common;
using Overmodded.Unity.Editor.SharedSystem;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(LevelSpawn))]
    public class LevelSpawnEditor : UnityEditor.Editor
    {
        private LevelSpawn _target;

        private SerializedProperty Mode;
        private SerializedProperty AgentHeight;
        private SerializedProperty AgentSize;
        private SerializedProperty ForwardMode;
        private SerializedProperty ResetForwardY;

        private SerializedProperty HitMask;

        private SerializedProperty SpawnName;
        private SerializedProperty IsDefault;

        private void OnEnable()
        {
            _target = (LevelSpawn) target;

            Mode = serializedObject.FindProperty(nameof(Mode));
            AgentHeight = serializedObject.FindProperty(nameof(AgentHeight));
            AgentSize = serializedObject.FindProperty(nameof(AgentSize));
            ForwardMode = serializedObject.FindProperty(nameof(ForwardMode));
            ResetForwardY = serializedObject.FindProperty(nameof(ResetForwardY));

            HitMask = serializedObject.FindProperty(nameof(HitMask));

            SpawnName = serializedObject.FindProperty(nameof(SpawnName));
            IsDefault = serializedObject.FindProperty(nameof(IsDefault));

            // Try to refresh the database!
            SharedEditorDataManager.TryRefreshDatabaseAtStart();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // JEM Area
            EditorGUILayout.PropertyField(Mode);
            EditorGUILayout.PropertyField(AgentHeight);
            EditorGUILayout.PropertyField(AgentSize);
            EditorGUILayout.PropertyField(ForwardMode);
            EditorGUILayout.PropertyField(ResetForwardY);

            EditorGUILayout.PropertyField(HitMask);

            // Actual Content :)
            SpawnName.stringValue = EditorGUILayoutGameUtility.LevelSpawnNameField("Spawn Name", SpawnName.stringValue);
            EditorGUILayout.PropertyField(IsDefault);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
