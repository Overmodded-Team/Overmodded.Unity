//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.AssetBundles
{
    /// <inheritdoc />
    /// <summary>
    ///     Asset builder window.
    /// </summary>
    public class JEMAssetBuilderWindow : EditorWindow
    {
        private Vector2 _assetsScroll;
        private Vector2 _packagesScroll;

        private void UpdateTitle()
        {
            // load JEM editor resources
            JEMEditorResources.Load();

            titleContent = new GUIContent("JEM Assets", JEMEditorResources.JEMIconTexture);
        }

        private void OnEnable()
        {
            JEMAssetsBuilderConfiguration.LoadConfiguration();
            LoadPackages();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        ///     Loads packages cfg from local disc.
        /// </summary>
        public static void LoadPackages()
        {
            Packages.Clear();
            if (!Directory.Exists(PackagesCfgDirectory))
            {
                Directory.CreateDirectory(PackagesCfgDirectory);
            }
            else
            {
                var files = Directory.GetFiles(PackagesCfgDirectory);
                foreach (var fileName in files)
                {
                    if (!fileName.EndsWith(".json"))
                        continue;

                    JEMAssetBuilderPackage package = null;
                    try
                    {
                        package = JsonConvert.DeserializeObject<JEMAssetBuilderPackage>(File.ReadAllText(fileName));
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(
                            $"System was unable to parse {fileName} in to AssetBuilderPackage class. {e.Message}");
                    }

                    if (package == null)
                        continue;

                    Packages.Add(package);
                }
            }
        }

        /// <summary>
        ///     Saves packages cfg to local disc.
        /// </summary>
        public static void SavePackages()
        {
            foreach (var package in Packages)
            {
                var file = $@"{PackagesCfgDirectory}\{package.Name}.json";
                var dir = Path.GetDirectoryName(file);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(file, JsonConvert.SerializeObject(package, Formatting.Indented));
            }
        }

        /// <summary>
        ///     Gets package by name.
        /// </summary>
        public static JEMAssetBuilderPackage GetPackage(string packageName)
        {
            foreach (var pack in Packages)
                if (pack.Name == packageName)
                    return pack;
            return null;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                var PackagesWidth = 200;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(PackagesWidth));
                {
                    _packagesScroll = EditorGUILayout.BeginScrollView(_packagesScroll, EditorStyles.helpBox,
                        GUILayout.Width(PackagesWidth));
                    if (Packages.Count == 0)
                        EditorGUILayout.HelpBox("No packages found :/", MessageType.None, true);
                    else
                        for (var index = 0; index < Packages.Count; index++)
                        {
                            var package = Packages[index];
                            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(PackagesWidth - 8));
                            GUILayout.Box(package.GetFile(), EditorStyles.helpBox);
                            if (SelectedPackage == package.Name)
                            {
                                GUILayout.Box(package.Name + " (Selected)", GUILayout.Width(PackagesWidth - 17));
                            }
                            else
                            {
                                if (GUILayout.Button(package.Name, GUILayout.Width(PackagesWidth - 17)))
                                    SelectedPackage = package.Name;
                            }

                            EditorGUILayout.EndVertical();
                        }

                    EditorGUILayout.EndScrollView();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Add New")) JEMAssetBuilderAddPackageWindow.ShowWindow();

                    if (GUILayout.Button("Save")) SavePackages();

                    if (GUILayout.Button("Force reload")) LoadPackages();

                    if (GUILayout.Button("Export All")) JEMAssetBuilderExporter.ExportPackages(Packages.ToArray());
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    if (!string.IsNullOrEmpty(SelectedPackage))
                    {
                        var package = GetPackage(SelectedPackage);
                        if (package == null)
                        {
                            EditorGUILayout.HelpBox(
                                "System detect selected package but was unable to resolve it's data.",
                                MessageType.Error, true);
                            Debug.LogWarning("System detect selected package but was unable to resolve it's data.");
                            SelectedPackage = string.Empty;
                        }
                        else
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                {
                                    _assetsScroll = EditorGUILayout.BeginScrollView(_assetsScroll);
                                    for (var index = 0; index < package.Assets.Count; index++)
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Label(package.Assets[index].Path);
                                        GUILayout.FlexibleSpace();
                                        package.Assets[index].Include =
                                            EditorGUILayout.Toggle(package.Assets[index].Include);
                                        if (GUILayout.Button("Remove", GUILayout.Width(100)))
                                        {
                                            package.Assets.RemoveAt(index);
                                            index--;
                                        }

                                        EditorGUILayout.EndHorizontal();
                                    }

                                    EditorGUILayout.EndScrollView();
                                }
                                EditorGUILayout.EndVertical();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(PackagesWidth));
                                {
                                    if (GUILayout.Button("Add Selected Assets", GUILayout.Height(60)))
                                    {
                                        if (Selection.objects.Length == 0)
                                        {
                                            EditorUtility.DisplayDialog("Oops.",
                                                "To add selected assets first you need to select assets. (lul)", "Ok");
                                        }
                                        else
                                        {
                                            var selectedObjects = Selection.objects.Select(AssetDatabase.GetAssetPath)
                                                .ToArray();
                                            if (selectedObjects.Length == 0)
                                                EditorUtility.DisplayDialog("Oops.", "Unexcpeted objects list error.",
                                                    "Ok");
                                            else
                                                foreach (var obj in selectedObjects)
                                                    if (package.Exist(obj))
                                                        Debug.LogWarning(
                                                            $"Copy of {obj} detected. System will skip this object.");
                                                    else
                                                        package.AddAsset(obj);
                                        }
                                    }

                                    if (GUILayout.Button("Export Assets"))
                                    {
                                        if (package.Assets.Count == 0)
                                            EditorUtility.DisplayDialog("Oops.", "Can't export empty package..", "Ok");
                                        else
                                            JEMAssetBuilderExporter.ExportPackages(new[] {package});
                                    }

                                    GUILayout.FlexibleSpace();
                                    if (GUILayout.Button("Delete Package")) Packages.Remove(package);
                                }
                                EditorGUILayout.EndVertical();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Show window.
        /// </summary>
        [MenuItem("Tools/JEM/Assets/Builder")]
        public static void ShowWindow()
        {
            _activeWindow = GetWindow(typeof(JEMAssetBuilderWindow)) as JEMAssetBuilderWindow;
            if (_activeWindow == null) return;
            _activeWindow.UpdateTitle();
            _activeWindow.Show();
        }

        /// <summary>
        ///     Local file where flags are stored.
        /// </summary>
        public static string PackagesCfgDirectory => $@"{Environment.CurrentDirectory}\JEM\AssetBuilder\";

        /// <summary>
        ///     Current packages
        /// </summary>
        public static List<JEMAssetBuilderPackage> Packages { get; } = new List<JEMAssetBuilderPackage>();

        /// <summary>
        ///     Currently selected package.
        /// </summary>
        public static string SelectedPackage;

        private static JEMAssetBuilderWindow _activeWindow;
    }
}