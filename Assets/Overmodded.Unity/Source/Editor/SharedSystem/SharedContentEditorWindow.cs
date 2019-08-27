//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections.Generic;
using JEM.UnityEditor;
using Overmodded.Unity.Editor.Objects;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.SharedSystem
{
    public class SharedContentEditorWindow : EditorWindow
    {
        private readonly string[] PagesNames = {"Level Names"};
        private SavedInt ActivePage;

        private IList<SharedEditorData> _externalEditorData;
        private SharedEditorData _localEditorData;

        private void OnEnable()
        {
            RefreshContent();
            ActivePage = new SavedInt($"{nameof(SharedContentEditorWindow)}.ActivePage", 0);
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

            ActivePage.value = GUILayout.SelectionGrid(ActivePage.value, PagesNames, PagesNames.Length);

            switch (ActivePage.value)
            {
                case 0: // Level Names
                    foreach (var externalEditor in _externalEditorData)
                    {
                        // Draw Level names from other editors (read only)
                        externalEditor.LevelSpawnNames = JEMBetterEditor.ContentList(externalEditor.LevelSpawnNames, this,
                        str =>
                        {
                            str = EditorGUILayout.TextField(str);
                            return str;
                        },
                        new JEMBetterEditorStyle(externalEditor.name, $"{ nameof(SharedContentEditorWindow)}.{externalEditor.UniqueGUID}.LevelSpawnNames"){Readonly = true});
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
                        new JEMBetterEditorStyle($"Local Shared Data ({_localEditorData.name})", $"{ nameof(SharedContentEditorWindow)}.{_localEditorData.UniqueGUID}.LevelSpawnNames"));
                    }
                    break;
                default:
                    EditorGUILayout.HelpBox("Unknown or not implemented page has been selected.", MessageType.Error,
                        true);
                    break;
            }
        }

        public void NavToLevelNamesPage() => ActivePage.value = 0;

        /// <summary>
        ///     Shows a Shared Content editor window on Level Spawn Names page.
        /// </summary>
        public static void ShowWindowOnSpawnNames()
        {
            var window = ShowWindow();
            window.NavToLevelNamesPage();
        }

        /// <summary>
        ///     Shows a Shared Content editor window.
        /// </summary>
        [MenuItem("Tools/Overmodded/Shared Content Editor")]
        public static SharedContentEditorWindow ShowWindow() => GetWindow<SharedContentEditorWindow>(true, "Shared Content Editor", true);
    }
}
