//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Character;
using Overmodded.Unity.Editor.Common;
using Overmodded.Unity.Editor.SharedSystem;
using UnityEditor;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(CharacterSettingsReference))]
    public class CharacterSettingsReferenceEditor : UnityEditor.Editor
    {
        private CharacterSettingsReference _target;

        private void OnEnable()
        {
            _target = (CharacterSettingsReference) target;

            // Try to refresh the database!
            SharedEditorDataManager.TryRefreshDatabaseAtStart();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _target.CharacterSettingsIdentity = EditorGameUtility.CharacterSettingsField("Character Settings", _target.CharacterSettingsIdentity);
        }
    }
}
