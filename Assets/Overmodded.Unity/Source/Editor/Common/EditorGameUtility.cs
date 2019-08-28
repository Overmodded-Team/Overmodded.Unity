//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using Overmodded.Gameplay.Character;
using Overmodded.Unity.Editor.Objects;
using Overmodded.Unity.Editor.SharedSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Overmodded.Unity.Editor.Common
{
    /// <summary>
    ///     A set of utility methods for Editor GUI. 
    /// </summary>
    public static class EditorGameUtility
    {
        public static int CharacterSettingsField(string label, int characterIdentity) => DatabaseItemField<CharacterDatabase, CharacterSettings>(label, characterIdentity);

        /// <summary>
        ///     Draws a Database Item field.
        /// </summary>
        public static int DatabaseItemField<TDatabase, TItem>(string label, int itemIdentity) where TDatabase : DatabaseManager<TItem> where TItem : DatabaseItem
        {
            EditorGUILayout.BeginHorizontal();
            var records = SharedEditorDataManager.GetRecords<TDatabase, TItem>();
            var recordsIdentityList = new List<int>();
            var recordsFixedNames = new List<string>();
            for (var i1 = 0; i1 < records.Count; i1++)
            {
                var s = records[i1];
                if (recordsIdentityList.Contains(s.Item3))
                {
                    Debug.LogWarning($"There is more than one item with identity {s.Item3} in database of type {typeof(TDatabase).Name}!");
                    continue;
                }

                bool has = records.Where((g2, i2) => i1 != i2 && s.Item2 == g2.Item2).Any();
                if (has)
                    recordsFixedNames.Add($"{s.Item2} ({SharedEditorDataManager.GUIDToEditorDataName(s.Item1)})");
                else recordsFixedNames.Add($"{s.Item2}");
                recordsIdentityList.Add(s.Item3);
            }

            if (recordsIdentityList.Count == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label, $"List of {typeof(TItem).Name} is empty.");
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                var index = recordsIdentityList.Contains(itemIdentity) ? recordsIdentityList.IndexOf(itemIdentity) : 0;
                index = EditorGUILayout.Popup(label, index, recordsFixedNames.ToArray());
                itemIdentity = recordsIdentityList[index];
                if (GUILayout.Button("Info", EditorStyles.miniButton, GUILayout.Width(35)))
                    SharedContentEditorWindow.ShowWindowOnCharacters();
            }

            // A general databases refresh.
            if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(50)))
                SharedEditorDataManager.RefreshDatabases();

            EditorGUILayout.EndHorizontal();
            return itemIdentity;
        }

        /// <summary>
        ///     Draws a Level Spawn name field.
        /// </summary>
        public static string LevelSpawnNameField(string label, string spawnName)
        {
            EditorGUILayout.BeginHorizontal();
            var spawnNames = SharedEditorDataManager.GetLevelSpawnNames();
            var fixedSpawnNames = new List<string>();
            var fullSpawnNames = new List<string>();
            for (var i1 = 0; i1 < spawnNames.Count; i1++)
            {
                var s = spawnNames[i1];
                bool has = spawnNames.Where((g2, i2) => i1 != i2 && s.Item2 == g2.Item2).Any();
                if (has)
                    fixedSpawnNames.Add($"{s.Item2} ({SharedEditorDataManager.GUIDToEditorDataName(s.Item1)})");
                else fixedSpawnNames.Add($"{s.Item2}");
                var fullName = $"{s.Item1}.{s.Item2}";
                fullSpawnNames.Add(fullName);
            }

            if (fullSpawnNames.Count == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label, "Spawn names list is empty.");
                if (GUILayout.Button("Add New", EditorStyles.miniButton))
                    SharedContentEditorWindow.ShowWindowOnSpawnNames();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                var index = fullSpawnNames.Contains(spawnName) ? fullSpawnNames.IndexOf(spawnName) : 0;
                index = EditorGUILayout.Popup(label, index, fixedSpawnNames.ToArray());
                spawnName = fullSpawnNames[index];
                if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(40)))
                    SharedContentEditorWindow.ShowWindowOnSpawnNames();
            }

            // A general databases refresh.
            if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(50)))
                SharedEditorDataManager.RefreshDatabases();

            EditorGUILayout.EndHorizontal();
            return spawnName;
        }

        /// <summary>
        ///     Draws a GUID field.
        /// </summary>
        public static string GUIDField(string guid)
        {
            return GUIDField(null, guid);
        }

        /// <summary>
        ///     Draws a GUID field.
        /// </summary>
        public static string GUIDField(string header, string guid)
        {
            if (!string.IsNullOrEmpty(header))
                GUILayout.Label(header, EditorStyles.boldLabel);
            else header = "Unique GUID";

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.fieldWidth += 200;
            if (string.IsNullOrEmpty(guid))
                GUILayout.Box($"{header} need to be regenerated.", CustomEditorStyles.Loaded.SmallInfoBox, GUILayout.Width(EditorGUIUtility.fieldWidth));
            else
            {
                GUILayout.Box(guid, CustomEditorStyles.Loaded.SmallInfoBox, GUILayout.Width(EditorGUIUtility.fieldWidth));
            }

            EditorGUIUtility.fieldWidth -= 100;

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Regenerate"))
            {
                bool regenerate = true;
                if (!string.IsNullOrEmpty(guid))
                {
                    regenerate = EditorUtility.DisplayDialog("Regenerate?", "Are you sure you want to regenerate the GUID of this SharedEditorData?", "Yes", "No");
                }

                if (regenerate)
                {
                    guid = GUID.Generate().ToString();
                }
            }
            EditorGUILayout.EndHorizontal();
            return guid;
        }

        /// <summary>
        ///     Draw a Extras GUI section that has additional options for any object targeted GUI Editor.
        /// </summary>
        public static void DrawTargetExtras(Object target)
        {
            // Load last extras state
            var drawExtras = new SavedBool($"{target.GetType().Name}.Extras", false);
            drawExtras.value = EditorGUILayout.BeginFoldoutHeaderGroup(drawExtras.value, "Extras");
            if (drawExtras.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                GUILayout.Label("Asset Database", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Write on Disc (Force)");
                if (GUILayout.Button("Apply"))
                {
                    // Should we set this element as dirty?
                    EditorUtility.SetDirty(target);

                    // Refresh the AssetDattabase and save...
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
