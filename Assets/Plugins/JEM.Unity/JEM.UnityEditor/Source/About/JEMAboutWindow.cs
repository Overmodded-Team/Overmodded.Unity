//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Configuration;
using JEM.Core.Debugging;
using JEM.UnityEditor.AssetBundles;
using JEM.UnityEditor.Configuration;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.About
{
    /// <inheritdoc />
    /// <summary>
    ///     Asset builder configuration window.
    /// </summary>
    public class JEMAboutWindow : EditorWindow
    {
        private int SelectedTab;

        private string[] Tabs { get; } = {"About", "Core", "Editor", "AssetBuilder"};

        private void UpdateTitle()
        {
            // load JEM editor resources
            JEMEditorResources.Load();

            titleContent = new GUIContent("About JEM", JEMEditorResources.JEMIconTexture);
        }

        private void OnEnable()
        {
            JEMLogger.Init();

            // load all configurations
            JEMConfigurationInternal.Load();
            JEMEditorConfiguration.Load();
            JEMAssetsBuilderConfiguration.LoadConfiguration();
        }

        private void OnSave()
        {
            // save all configurations
            JEMConfigurationInternal.Save();
            JEMEditorConfiguration.Save();
            JEMAssetsBuilderConfiguration.SaveConfiguration();

            JEMLogger.Init();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(126));
                {
                    SelectedTab = GUILayout.SelectionGrid(SelectedTab, Tabs, 1);
                    GUILayout.FlexibleSpace();

                    var save = false;
                    switch (SelectedTab)
                    {
                        case 0:
                            break;
                        case 1:
                            if (GUILayout.Button("Force Reload\nJEM Resources", GUILayout.Width(126),
                                GUILayout.Height(40))) JEMEditorResources.Load(true);
                            save = true;
                            break;
                        case 2:
                        case 3:
                            save = true;
                            break;
                    }

                    if (save)
                        if (GUILayout.Button("Save", GUILayout.Width(126)))
                            OnSave();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    switch (SelectedTab)
                    {
                        case 0:
                        {
                            GUILayout.Label("Just Enough Methods Library for Unity", EditorStyles.boldLabel);
                            EditorGUILayout.Space();
                            if (GUILayout.Button("GitHub", GUILayout.Height(25), GUILayout.Width(150)))
                                Application.OpenURL("https://github.com/TylkoDemon/JEM");

                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Copyright (c) 2017-2019 ADAM MAJCHEREK\nALL RIGHTS RESERVED");
                        }
                            break;
                        case 1:
                        {
                            GUILayout.Label("JEM Core Settings", EditorStyles.boldLabel);
                            JEMConfigurationInternal.Configuration.ConfigurationSaveMethod =
                                (JEMConfigurationSaveMethod) EditorGUILayout.EnumPopup(
                                    "Configuration Save Method",
                                    JEMConfigurationInternal.Configuration.ConfigurationSaveMethod);
                            if (JEMConfigurationInternal.Configuration.ConfigurationSaveMethod ==
                                JEMConfigurationSaveMethod.UNKNOWN)
                                JEMConfigurationInternal.Configuration.ConfigurationSaveMethod =
                                    JEMConfigurationSaveMethod.JSON;
                            JEMConfigurationInternal.Configuration.ConfigurationAppSaveDirectory =
                                EditorGUILayout.TextField("App Save Directory",
                                    JEMConfigurationInternal.Configuration.ConfigurationAppSaveDirectory);
                            EditorGUILayout.Space();
                            JEMConfigurationInternal.Configuration.ConfigurationBinFileExtension =
                                EditorGUILayout.TextField("Bin File Extension",
                                    JEMConfigurationInternal.Configuration.ConfigurationBinFileExtension);

                            EditorGUILayout.Space();
                            JEMConfigurationInternal.Configuration.ConfigurationJsonFileExtension =
                                EditorGUILayout.TextField("Json File Extension",
                                    JEMConfigurationInternal.Configuration.ConfigurationJsonFileExtension);
                            JEMConfigurationInternal.Configuration.JsonFormattingMethod =
                                (Formatting) EditorGUILayout.EnumPopup("JSON Formatting Method",
                                    JEMConfigurationInternal.Configuration.JsonFormattingMethod);
                            EditorGUILayout.Space();
                            JEMConfigurationInternal.Configuration.NoLogs =
                                EditorGUILayout.Toggle("No Logs",
                                    JEMConfigurationInternal.Configuration.NoLogs);
                            }
                            break;
                        case 2:
                        {
                            GUILayout.Label("JEM Editor Settings", EditorStyles.boldLabel);
                            JEMEditorConfiguration.Configuration.UpdateBundleVersion =
                                EditorGUILayout.Toggle("Update Bundle Version",
                                    JEMEditorConfiguration.Configuration.UpdateBundleVersion);
                            JEMEditorConfiguration.Configuration.UpdateWorkTime =
                                EditorGUILayout.Toggle("Update Work Time",
                                    JEMEditorConfiguration.Configuration.UpdateWorkTime);
                            JEMEditorConfiguration.Configuration.UpdateFlags =
                                EditorGUILayout.Toggle("Update Flags",
                                    JEMEditorConfiguration.Configuration.UpdateFlags);
                            }
                            break;
                        case 3:
                        {
                            GUILayout.Label("JEM AssetBuilder Settings", EditorStyles.boldLabel);
                            JEMAssetsBuilderConfiguration.Configuration.PackageExtension =
                                EditorGUILayout.TextField("Package Extension",
                                    JEMAssetsBuilderConfiguration.Configuration.PackageExtension);
                        }
                            break;
                        default:
                            EditorGUILayout.HelpBox("NotImplementedTab", MessageType.Error, true);
                            break;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Show window.
        /// </summary>
        [MenuItem("Tools/JEM/About")]
        public static void ShowWindow()
        {
            _activeWindow = GetWindow(typeof(JEMAboutWindow)) as JEMAboutWindow;
            _activeWindow.UpdateTitle();
            _activeWindow.maxSize = new Vector2(450, 350);
            _activeWindow.minSize = new Vector2(450, 350);
            _activeWindow.ShowPopup();
        }

        private static JEMAboutWindow _activeWindow;
    }
}