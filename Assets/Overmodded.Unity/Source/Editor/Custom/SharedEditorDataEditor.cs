//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Unity.Editor.Common;
using Overmodded.Unity.Editor.Objects;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(SharedEditorData))]
    public class SharedEditorDataEditor : UnityEditor.Editor
    {
        private SharedEditorData _target;

        private SavedBool _drawIdentitySettings;
        private SavedBool _drawContent;

        private SerializedProperty UniqueGUID;
        private SerializedProperty LevelSpawnNames;

        private void OnEnable()
        {
            _target = (SharedEditorData) target;

            _drawIdentitySettings = new SavedBool($"{nameof(SharedEditorDataEditor)}.DrawIdentitySettings", false);
            _drawContent = new SavedBool($"{nameof(SharedEditorDataEditor)}.DrawContent", false);

            UniqueGUID = serializedObject.FindProperty(nameof(UniqueGUID));
            LevelSpawnNames = serializedObject.FindProperty(nameof(LevelSpawnNames));
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            if (!CustomEditorStyles.IsCustomStyleLoaded()) return;
            serializedObject.Update();

            // Draw Identity settings
            _drawIdentitySettings.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawIdentitySettings.value, "Identity Settings");
            if (_drawIdentitySettings.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                UniqueGUID.stringValue = EditorGameUtility.GUIDField("Unique GUID", UniqueGUID.stringValue);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // Draw Content
            _drawContent.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawContent.value, "Content");
            if (_drawContent.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(LevelSpawnNames, true);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // Draw extras
            EditorGameUtility.DrawTargetExtras(target);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
