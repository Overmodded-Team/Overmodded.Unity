//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Level.Materials;
using Overmodded.Unity.Editor.Common;
using Overmodded.Unity.Editor.SharedSystem;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(MaterialDef))]
    internal class MaterialDefEditor : UnityEditor.Editor
    {
        private MaterialDef _target;

        private void OnEnable()
        {
            _target = (MaterialDef)target;

            // Try to refresh the database!
            SharedEditorDataManager.TryRefreshDatabaseAtStart();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _target.MaterialIdentity = EditorGUILayoutGameUtility.MaterialSettingsField("Material Settings", _target.MaterialIdentity);
        }
    }
}
