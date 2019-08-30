//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.AssetBundles
{
    /// <inheritdoc />
    /// <summary>
    ///     JEM Asset Builder window.
    /// </summary>
    public class JEMAssetBuilderWindow : EditorWindow
    {
        private SavedVector2 _assetsScroll;
        private SavedVector2 _packagesScroll;

        private void OnEnable()
        {
            // Load JEM editor resources
            JEMEditorResources.Load();

            // Apply title
            titleContent = new GUIContent("JEM Asset Builder", JEMEditorResources.JEMIconTexture);

            // Try to load configuration.
            JEMAssetsBuilderConfiguration.TryLoadConfiguration();

            // Load Saved vars.
            _assetsScroll = new SavedVector2($"{nameof(JEMAssetBuilderWindow)}.AssetsScroll", Vector2.zero);
            _packagesScroll = new SavedVector2($"{nameof(JEMAssetBuilderWindow)}.PackagesScroll", Vector2.zero);

            // Load packages!
            LoadPackages();
        }

        // We may not want to save packages on window Disable...  
        // private void OnDisable() => SavePackages();

        // Do we need this?
        private void OnInspectorUpdate() => Repaint();

        private bool _wantToExportAll;
        private JEMAssetBuilderPackage _exportPackage;
        private bool _exportPackageOnce;
        private int _exportTimeout;

        // ERROR:  EndLayoutGroup: BeginLayoutGroup must be called first. 
        private void Update()
        {
            if (_wantToExportAll)
            {
                _exportTimeout = 2; // Skip two frames after export...
                _wantToExportAll = false;
                JEMAssetBuilderExporter.ExportPackages(JEMAssetsBuilderConfiguration.GetDirectory(), Packages.ToArray());          
            }

            if (_exportPackageOnce)
            {
                _exportTimeout = 2; // Skip two frames after export...
                _exportPackageOnce = false;
                ExportAssets(_exportPackage);
                _exportPackage = null;
            }
        }
        private void OnGUI()
        {
            if (_exportTimeout > 0)
            {
                _exportTimeout--;
                return;
            }

            EditorGUILayout.BeginHorizontal();
            {
                const float packagesWidth = 200;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(packagesWidth));
                {
                    _packagesScroll.value = EditorGUILayout.BeginScrollView(_packagesScroll.value, EditorStyles.helpBox, GUILayout.Width(packagesWidth));
                    if (Packages.Count == 0)
                        EditorGUILayout.HelpBox("No packages found :/", MessageType.Info, true);
                    else
                    {
                        GUILayout.Label("Packages:", EditorStyles.boldLabel);
                        for (var index = 0; index < Packages.Count; index++)
                        {
                            var package = Packages[index];
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Width(packagesWidth - 8));
                            GUILayout.Box(package.Name, EditorStyles.helpBox);
                            if (SelectedPackage == package.Name)
                            {
                                GUI.enabled = false;
                                GUI.color = new Color(0.3f, 1.0f, 0.3f, 1.0f);
                                GUILayout.Button("Selected", GUILayout.Width(65));
                                GUI.enabled = true;
                            }
                            else
                            {
                                if (GUILayout.Button("Select", GUILayout.Width(65)))
                                {
                                    SelectedPackage = package.Name;
                                }
                            }

                            GUI.color = Color.white;
                            EditorGUILayout.EndHorizontal();
                        }

                        GUI.color = Color.white;
                    }
                    EditorGUILayout.EndScrollView();

                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Packages Options", EditorStyles.boldLabel);
                    if (GUILayout.Button("Add New", GUILayout.Height(22)))
                    {
                        JEMAssetBuilderAddPackageWindow.ShowWindow();
                    }

                    if (GUILayout.Button("Save All", GUILayout.Height(22)))
                    {
                        SavePackages();
                    }

                    if (GUILayout.Button("Reload All", GUILayout.Height(22)))
                    {
                        LoadPackages();
                    }

                    if (GUILayout.Button("Export All", GUILayout.Height(22)))
                    {
                        _wantToExportAll = true;
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    if (string.IsNullOrEmpty(SelectedPackage))
                    {
                        EditorGUILayout.HelpBox("No package selected.", MessageType.Info, true);
                    }
                    else
                    {
                        var package = GetPackage(SelectedPackage);
                        if (package == null)
                        {
                            Debug.LogWarning(
                                $"JEMAssetBuilderWindow: Selected package detected ({SelectedPackage}) but we was unable to resolve it's data :/ ");
                            SelectedPackage = string.Empty;
                        }
                        else
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);                
                                if (package.Assets.Count == 0)
                                {
                                    EditorGUILayout.HelpBox("The package is empty.", MessageType.Info, true);
                                }
                                else
                                {
                                    var allAssets = 0;
                                    _assetsScroll.value = EditorGUILayout.BeginScrollView(_assetsScroll.value);
                                    GUILayout.Label("Assets:", EditorStyles.boldLabel);
                                    for (var index = 0; index < package.Assets.Count; index++)
                                    {
                                        var asset = package.Assets[index];
                                        var assetPath = AssetDatabase.GUIDToAssetPath(asset.Guid);
                                        var assetName = Path.GetFileName(assetPath);
                                        var assetDependence = AssetDatabase.GetDependencies(assetPath);

                                        GUI.color = index % 2 == 0 ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f);
                                        if (!asset.Include)
                                        {
                                            GUI.color = GUI.color / 1.2f;
                                        }
                                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                        var drawAsset = new SavedBool($"{nameof(JEMAssetBuilderWindow)}.Package.{package.Name}.Asset.{asset.Guid}", false);
                                        if (!asset.Include)
                                        {
                                            assetName = " (Not Included) " + assetName;
                                        }

                                        var indexCopy = index;
                                        drawAsset.value = EditorGUILayout.BeginFoldoutHeaderGroup(drawAsset.value, assetName, menuAction:
                                        rect =>
                                        {
                                            var menu = new GenericMenu();                       
                                            menu.AddItem(new GUIContent("Remove"), false, () =>
                                            {
                                                package.Assets.RemoveAt(indexCopy);
                                                indexCopy--;
                                            });
                                            menu.DropDown(rect);
                                        });

                                        if (drawAsset.value)
                                        {
                                            EditorGUI.indentLevel++;
                                            EditorGUIUtility.labelWidth += 50f;
                                            asset.Include = EditorGUILayout.Toggle("Include", asset.Include);
                                            EditorGUILayout.LabelField("Full path", assetPath);
                                            EditorGUILayout.LabelField("Dependences (Amount)", assetDependence.Length.ToString());
                                            if (assetDependence.Length != 1) // do not draw if the only dependence in array is this asset
                                            { 
                                                var drawDependence = new SavedBool($"{nameof(JEMAssetBuilderWindow)}.Package.{package.Name}.Asset.{asset.Guid}.Dependence", false);
                                                drawDependence.value = EditorGUILayout.Foldout(drawDependence.value, "Dependences");
                                                if (drawDependence.value)
                                                {
                                                    EditorGUI.indentLevel++;
                                                    foreach (var d in assetDependence)
                                                    {
                                                        if (assetPath.Equals(d)) continue; // nope
                                                        EditorGUILayout.LabelField(" ", d);
                                                    }

                                                    EditorGUI.indentLevel--;
                                                }
                                            }
                                            EditorGUIUtility.labelWidth -= 50f;
                                            EditorGUI.indentLevel--;
                                        }
                                        EditorGUILayout.EndFoldoutHeaderGroup();
                                        EditorGUILayout.EndVertical();
                                        GUI.color = Color.white;

                                        if (asset.Include)
                                        {
                                            allAssets += assetDependence.Length;
                                        }
                                    }
                                    GUI.color = Color.white;
                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                    GUILayout.Label("Info", EditorStyles.boldLabel);
            
                                    EditorGUILayout.HelpBox($"Total amount of assets to build: {allAssets}", MessageType.Info, true);
                                    EditorGUILayout.HelpBox("Package Path: " + package.GetFile(), MessageType.Info, true);
                                    EditorGUILayout.EndVertical();
                                }
  
                                EditorGUILayout.EndVertical();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(packagesWidth));
                                GUILayout.Label("Selected Package", EditorStyles.boldLabel);
                                if (GUILayout.Button("Add Selected Assets", GUILayout.Height(60)))
                                {
                                    AddSelectedAssets(package);
                                }

                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Danger Zone", EditorStyles.boldLabel);
                                if (GUILayout.Button("Export Assets", GUILayout.Height(22)))
                                {
                                    _exportPackage = package;
                                    _exportPackageOnce = true;
                                }

                                if (GUILayout.Button("Delete Package", GUILayout.Height(22)))
                                {
                                    var confirm = EditorUtility.DisplayDialog("Delete?", $"Are you sure you want to remove package `{package.Name}`?", "Yes", "No");
                                    if (confirm)
                                        RemovePackage(package);
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
        ///     Adds currently selected assets to target package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        private static void AddSelectedAssets([NotNull] JEMAssetBuilderPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (Selection.objects.Length == 0)
            {
                EditorUtility.DisplayDialog("Oops.", "To add new assets in to package, first you need to select the assets. (lul)", "Ok!");
                return;
            }

            foreach (var obj in Selection.objects)
            {
                if (package.Exist(obj))
                    continue;

                package.AddAsset(obj);
            }
        }

        /// <summary>
        ///     Exports assets of target package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        private static void ExportAssets([NotNull] JEMAssetBuilderPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (package.Assets.Count == 0)
                EditorUtility.DisplayDialog("Oops.", "Can't export empty package..", "Ok");
            else
            {
                // Before export make sure that all packages are saved, we do not want to lose any data...
                SavePackages();

                // Export!
                JEMAssetBuilderExporter.ExportPackages(JEMAssetsBuilderConfiguration.GetDirectory(), new[] {package});
            }
        }

        /// <summary>
        ///     Removes target package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void RemovePackage([NotNull] JEMAssetBuilderPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (!Packages.Contains(package))
                return;

            var configurationFile = package.GetConfigurationFile();
            if (File.Exists(configurationFile))
            {
                File.Delete(configurationFile);
            }

            Packages.Remove(package);
        }

        /// <summary>
        ///     Adds target package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void AddPackage([NotNull] JEMAssetBuilderPackage package, bool select = true)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            if (Packages.Contains(package))
                return;

            Packages.Add(package);
            if (select)
            {
                SelectedPackage = package.Name;
            }
        }

        /// <summary>
        ///     Loads all packages.
        /// </summary>
        public static void LoadPackages()
        {
            Packages.Clear();

            if (!Directory.Exists(PackagesConfigurationDirectory))
                Directory.CreateDirectory(PackagesConfigurationDirectory); // Directory not exist. Just create directory...
            else
            {
                var files = Directory.GetFiles(PackagesConfigurationDirectory, "*.json");
                foreach (var fileName in files)
                {
                    JEMAssetBuilderPackage package = null;
                    try
                    {
                        package = JsonConvert.DeserializeObject<JEMAssetBuilderPackage>(File.ReadAllText(fileName));
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"System was unable to parse `{fileName}` in to JEMAssetBuilderPackage class. {e.Message}");
                    }

                    if (package == null)
                        continue;

                    Packages.Add(package);
                }
            }
        }

        /// <summary>
        ///     Saves all packages configuration files.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public static void SavePackages()
        {
            foreach (var package in Packages)
            {
                var filePath = package.GetConfigurationFile();
                var fileDirectory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory ?? throw new InvalidOperationException());

                File.WriteAllText(filePath, JsonConvert.SerializeObject(package, Formatting.Indented));
            }
        }

        /// <summary>
        ///     Gets package by name.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static JEMAssetBuilderPackage GetPackage([NotNull] string packageName)
        {
            if (packageName == null) throw new ArgumentNullException(nameof(packageName));
            foreach (var pack in Packages)
                if (pack.Name == packageName)
                    return pack;
            return null;
        }

        /// <summary>
        ///     Show JEM Asset Builder window.
        /// </summary>
        [MenuItem("JEM/JEM Asset Builder")]
        public static void ShowWindow() => GetWindow<JEMAssetBuilderWindow>(true, "JEM Asset Builder", true);

        /// <summary>
        ///     List of all packages added to JEM Asset Builder.
        /// </summary>
        public static List<JEMAssetBuilderPackage> Packages { get; } = new List<JEMAssetBuilderPackage>();

        /// <summary>
        ///     Path to directory where Asset Builder Packages configuration files are stored.
        /// </summary>
        public static string PackagesConfigurationDirectory => $@"{Environment.CurrentDirectory}\JEM\AssetBuilder\";

        /// <summary>
        ///     Currently selected package.
        /// </summary>
        public static string SelectedPackage { get; private set; }
    }
}