//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Common;
using Overmodded.Gameplay.Character;
using Overmodded.Gameplay.Character.Weapons;
using Overmodded.Gameplay.Level.Materials;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.SharedSystem
{
    public class SharedContentEditorWindow : EditorWindow
    {
        private readonly string[] PagesNames = {"Level Names", "Databases"};
        private readonly string[] DatabasesNames = { nameof(CharacterDatabase), nameof(MaterialsDatabase), nameof(WeaponDatabase), nameof(CharacterAnimatorDatabase) };

        private SavedInt _activePage;
        private IList<SharedEditorData> _externalEditorData;
        private SharedEditorData _localEditorData;

        private void OnEnable()
        {
            RefreshContent();
            _activePage = new SavedInt($"{nameof(SharedContentEditorWindow)}.ActivePage", 0);
        }

        private void RefreshContent()
        {
            // Try to refresh the database!
            SharedEditorDataManager.TryRefreshDatabaseAtStart();

            _externalEditorData = SharedEditorDataManager.GetListOfExternalEditorData();
            _localEditorData = SharedEditorDataManager.GetLocalSharedData();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh Databases"))
                RefreshContent();

            _activePage.value = GUILayout.SelectionGrid(_activePage.value, PagesNames, PagesNames.Length);

            switch (_activePage.value)
            {
                case 0: // Level Names
                    OnLevelsNameGUI();
                    break;
                case 1: // Databases
                    OnDatabasesGUI();
                    break;
                default:
                    EditorGUILayout.HelpBox("Unknown or not implemented page has been selected.", MessageType.Error,
                        true);
                    break;
            }
        }

        public void NavToLevelNamesPage() => _activePage.value = 0;
        public void NavToCharactersPage() => _activePage.value = 1;

        private void OnLevelsNameGUI()
        {
            foreach (var externalEditor in _externalEditorData)
            {
                // Draw Level names from other editors (read only)
                externalEditor.LevelSpawnNames = JEMBetterEditor.ContentList(externalEditor.LevelSpawnNames, this,
                    str =>
                    {
                        str = EditorGUILayout.TextField(str);
                        return str;
                    },
                    new JEMBetterEditorStyle(externalEditor.name, $"{ nameof(SharedContentEditorWindow)}.{externalEditor.UniqueGUID}.LevelSpawnNames") { Readonly = true });
            }

            // Draw local editor
            if (_localEditorData == null)
                EditorGUILayout.HelpBox("No local editor data detected.", MessageType.Warning, true);
            else
            {
                _localEditorData.LevelSpawnNames = JEMBetterEditor.ContentList(_localEditorData.LevelSpawnNames, this,
                    str =>
                    {
                        str = EditorGUILayout.TextField(str);
                        return str;
                    },
                    new JEMBetterEditorStyle($"Local Shared Data ({_localEditorData.name})", $"{nameof(SharedContentEditorWindow)}.{_localEditorData.UniqueGUID}.LevelSpawnNames"));
            }
        }

        private void OnDatabasesGUI()
        {
            var savedDatabase = new SavedInt($"{nameof(SharedContentEditorWindow)}.Database", 0);
            savedDatabase.value = GUILayout.SelectionGrid(savedDatabase.value, DatabasesNames, 3);

            switch (savedDatabase.value)
            {
                case 0: // Character Database
                    DrawDatabases<CharacterDatabase, CharacterSettings>();
                    break;
                case 1: // Materials Database
                    DrawDatabases<MaterialsDatabase, MaterialSettings>();
                    break;
                case 2: // Weapon Database
                    DrawDatabases<WeaponDatabase, WeaponSettings>();
                    break;
                case 3: // Character Animator Database
                    DrawDatabases<CharacterAnimatorDatabase, CharacterAnimatorSettings>();
                    break;
                default:
                    EditorGUILayout.HelpBox("Unknown or not implemented database has been selected.", MessageType.Error,
                        true);
                    break;
            }
        }

        private void DrawDatabases<TDatabase, TItem>() where TDatabase : DatabaseManager<TItem> where TItem : DatabaseItem
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            var savedPosition = new SavedVector2($"{nameof(SharedContentEditorWindow)}.{typeof(TDatabase).Name}.ScrollPosition", Vector2.zero);
            savedPosition.value = EditorGUILayout.BeginScrollView(savedPosition.value);

            // Draw External Editor
            foreach (var externalEditor in _externalEditorData)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label(externalEditor.name, EditorStyles.boldLabel);
                DrawDatabaseRecord<TDatabase, TItem>(externalEditor);
                EditorGUILayout.EndVertical();
            }

            // Draw Local Editor
            if (_localEditorData == null)
                EditorGUILayout.HelpBox("No locale editor data detected.", MessageType.Warning, true);
            else
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label($"Local ({_localEditorData.name})", EditorStyles.boldLabel);
                DrawDatabaseRecord<TDatabase, TItem>(_localEditorData);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();   
        }

        private void DrawDatabaseRecord<TDatabase, TItem>(SharedEditorData data) where TDatabase : DatabaseManager<TItem> where TItem : DatabaseItem
        {
            string perfName = $"{nameof(SharedContentEditorWindow)}.{data.UniqueGUID}.{typeof(TDatabase).Name}";
            EditorGUIUtility.labelWidth += 150;
            var records = data.GetRecords<TDatabase, TItem>();
            if (records != null)
                foreach (var c in records)
                {
                    var drawDatabase = new SavedBool($"{perfName}.{c.Name}", false);
                    drawDatabase.value = EditorGUILayout.BeginFoldoutHeaderGroup(drawDatabase.value, c.Name);
                    if (drawDatabase.value)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel++;
                        foreach (var r in c.Records)
                        {
                            var drawRecord = new SavedBool($"{perfName}.{c.Name}.{r.Identity}", false);
                            drawRecord.value = EditorGUILayout.Foldout(drawRecord.value, r.Name);
                            if (drawRecord.value)
                            {
                                EditorGUI.indentLevel++;
                                EditorGUILayout.LabelField("Name", r.Name);
                                EditorGUILayout.LabelField("Identity", r.Identity.ToString());
                                var drawProperties = new SavedBool($"{perfName}.{c.Name}.{r.Identity}.Properties", false);
                                drawProperties.value = EditorGUILayout.Foldout(drawProperties.value, "Properties");
                                if (drawProperties.value)
                                {
                                    EditorGUI.indentLevel++;
                                    foreach (var p in r.Properties)
                                        EditorGUILayout.LabelField(p.Key, p.Value);
                                    EditorGUI.indentLevel--;
                                    EditorGUILayout.Space();
                                }
                                EditorGUI.indentLevel--;
                                EditorGUILayout.Space();
                            }
                        }

                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
            EditorGUIUtility.labelWidth -= 150;
        }

        /// <summary>
        ///     Shows a Shared Content editor window on Level Spawn Names page.
        /// </summary>
        public static void ShowWindowOnSpawnNames()
        {
            ShowWindow().NavToLevelNamesPage();
        }

        /// <summary>
        ///     Shows a Shared Content editor window on Characters Settings page.
        /// </summary>
        public static void ShowWindowOnCharacters()
        {
            ShowWindow().NavToCharactersPage();
        }

        /// <summary>
        ///     Shows a Shared Content editor window.
        /// </summary>
        [MenuItem("Overmodded/Shared Content Editor")]
        public static SharedContentEditorWindow ShowWindow() => GetWindow<SharedContentEditorWindow>(true, "Shared Content Editor", true);
    }
}
