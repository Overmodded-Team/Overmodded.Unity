//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Configuration;
using JEM.Core.Extension;
using JEM.UnityEditor.AssetBundles;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.Configuration
{
    /// <inheritdoc />
    /// <summary>
    ///     JEM Configuration Window.
    /// </summary>
    public class JEMConfigurationWindow : EditorWindow
    {
        private readonly string[] Tabs = { "About", "Core Settings", "Editor Settings", "AssetBuilder\nSettings" };

        private int SelectedTab;

        private void OnEnable()
        {
            // Load JEM editor resources
            JEMEditorResources.Load();

            // Apply Title
            titleContent = new GUIContent("JEM Configuration", JEMEditorResources.JEMIconTexture);

            // Load all configurations
            InternalJEMConfiguration.Load();
            JEMEditorConfiguration.Load();
            JEMAssetsBuilderConfiguration.Load();
        }

        // Do we need this?
        private void OnInspectorUpdate() => Repaint();
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();       
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(126));    
            SelectedTab = GUILayout.SelectionGrid(SelectedTab, Tabs, 1);
            GUILayout.FlexibleSpace();

            var save = false;
            switch (SelectedTab)
            {
                case 0:
                    break;
                case 1:
                    if (GUILayout.Button("Force Reload\nJEM Resources", GUILayout.Width(126), GUILayout.Height(40)))
                        JEMEditorResources.Load(true);
                    save = true;
                    break;
                case 2:
                case 3:
                    save = true;
                    break;
            }

            if (save)
                if (GUILayout.Button("Save", GUILayout.Width(126)))
                    SaveConfigurationData();       
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);             
            switch (SelectedTab)
            {
                case 0: // About
                    OnAboutGUI();
                    break;
                case 1: // Core Settings
                    OnCoreSettingsGUI();
                    break;
                case 2: // Editor Settings
                    OnEditorSettingsGUI();
                    break;
                case 3: // AssetBundles Settings
                    OnAssetBundlesSettingsGUI();
                    break;
                default:
                    EditorGUILayout.HelpBox("You are trying to draw page that not exist or is implemented yet :/", 
                        MessageType.Error, true);
                    break;
            } 
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void OnAboutGUI()
        {
            GUILayout.Label("Just Enough Methods", EditorStyles.boldLabel);
            GUILayout.Label("Library Extension for UnityEngine and UnityEditor");

            EditorGUILayout.Space();
            if (GUILayout.Button("GitHub", GUILayout.Height(25), GUILayout.Width(150)))
                Application.OpenURL("https://github.com/TylkoDemon/JEM.Unity");

            GUILayout.FlexibleSpace();
            GUILayout.Label("Copyright (c) 2017-2019 ADAM MAJCHEREK\nALL RIGHTS RESERVED");
        }

        private void OnCoreSettingsGUI()
        {
            var cfg = InternalJEMConfiguration.Configuration;

            GUILayout.Label("JEM Core Settings", EditorStyles.boldLabel);
            cfg.ConfigurationSaveMethod = (JEMConfigurationSaveMethod) EditorGUILayout.EnumPopup("Configuration Save Method", cfg.ConfigurationSaveMethod);
            if (cfg.ConfigurationSaveMethod == JEMConfigurationSaveMethod.UNKNOWN) cfg.ConfigurationSaveMethod = JEMConfigurationSaveMethod.JSON;
            cfg.ConfigurationAppSaveDirectory =  EditorGUILayout.TextField("App Save Directory", cfg.ConfigurationAppSaveDirectory);

            EditorGUILayout.Space();
            cfg.ConfigurationBinFileExtension = EditorGUILayout.TextField("Bin File Extension", cfg.ConfigurationBinFileExtension);

            EditorGUILayout.Space();
            cfg.ConfigurationJsonFileExtension = EditorGUILayout.TextField("Json File Extension", cfg.ConfigurationJsonFileExtension);
            cfg.JsonFormattingMethod = (Formatting)EditorGUILayout.EnumPopup("JSON Formatting Method", cfg.JsonFormattingMethod);

            EditorGUILayout.Space();
            cfg.NoLogs = EditorGUILayout.Toggle("No Logs",  cfg.NoLogs);
        }

        private void OnEditorSettingsGUI()
        {
            var cfg = JEMEditorConfiguration.Configuration;

            GUILayout.Label("JEM Editor Settings", EditorStyles.boldLabel);
            cfg.UpdateBundleVersion = EditorGUILayout.Toggle("Update Bundle Version", cfg.UpdateBundleVersion);
            cfg.UpdateWorkTime = EditorGUILayout.Toggle("Update Work Time", cfg.UpdateWorkTime);
            cfg.UpdateFlags = EditorGUILayout.Toggle("Update Flags", cfg.UpdateFlags);
        }

        private void OnAssetBundlesSettingsGUI()
        {
            var cfg = JEMAssetsBuilderConfiguration.Configuration;

            GUILayout.Label("JEM AssetBuilder Settings", EditorStyles.boldLabel);
            cfg.PackageExtension = EditorGUILayout.TextField("Package Extension", cfg.PackageExtension);

            if (cfg.PackageExtension.Length != 0 && cfg.PackageExtension[0] == '.')
            {
                EditorGUILayout.HelpBox("Package extension starts with '.'!", MessageType.Warning, true);
            }

            EditorGUILayout.TextField("Directory", cfg.PackageDirectory);
            JEMBetterEditor.DrawProperty(" ", () =>
            {
                if (GUILayout.Button("Select"))
                {
                    var directory = EditorUtility.OpenFolderPanel("Select directory of package", cfg.PackageDirectory, "");
                    cfg.PackageDirectory = ExtensionPath.ResolveRelativeFilePath(directory);
                }
            });
        }

        /// <summary>
        ///     Saves all the configurations!
        /// </summary>
        private static void SaveConfigurationData()
        {
            // save all configurations
            InternalJEMConfiguration.Save();
            JEMEditorConfiguration.Save();
            JEMAssetsBuilderConfiguration.Save();
        }

        /// <summary>
        ///     Shows the JEM Configuration Editor Window.
        /// </summary>
        [MenuItem("JEM/JEM Configuration")]
        public static void ShowWindow()
        {
            var activeWindow = GetWindow<JEMConfigurationWindow>(true, "JEM Configuration", true);
            activeWindow.maxSize = new Vector2(450, 350);
            activeWindow.minSize = new Vector2(450, 350);
        }
    }
}