//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Gameplay.Character;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(CharacterSettingsReference))]
    public class CharacterSettingsReferenceEditor : UnityEditor.Editor
    {
        private CharacterSettingsReference _target;
        private CharacterSettings _characterSettings;

        private void OnEnable()
        {
            _target = (CharacterSettingsReference) target;

            if (AllCharacters.Count == 0) RefreshDatabase();
            RefreshCharacter();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _characterSettings = (CharacterSettings) EditorGUILayout.ObjectField("Character Settings", _characterSettings, typeof(CharacterSettings), false);
            _target.CharacterSettingsIdentity = _characterSettings == null ? 0 : _characterSettings.Identity;

            JEMBetterEditor.DrawProperty(" ", () =>
            {
                if (GUILayout.Button("Refresh Character"))
                {
                    RefreshDatabase();
                    RefreshCharacter();
                }
            });
        }

        private void RefreshDatabase()
        {
            AllCharacters.Clear();
            string[] items = AssetDatabase.FindAssets($"t:{typeof(CharacterSettings)}");
            foreach (string item in items)
            {
                CharacterSettings s =
                    (CharacterSettings) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item),
                        typeof(CharacterSettings));
                AllCharacters.Add(s);
            }
        }

        private void RefreshCharacter()
        {
            EditorUtility.SetDirty(target);
            foreach (var s in AllCharacters)
            {
                if (s.Identity == _target.CharacterSettingsIdentity)
                    _characterSettings = s;
            }
        }

        /// <summary>
        ///     List of all characters in scene.
        /// </summary>
        private static List<CharacterSettings> AllCharacters { get; } = new List<CharacterSettings>();
    }
}
