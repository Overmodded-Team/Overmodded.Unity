//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JEM.UnityEditor.AssetBundles
{
    /// <summary>
    ///     Asset builder exporter class.
    ///     Core AssetBundles export methods of JEMAssetBuilder.
    /// </summary>
    public static class JEMAssetBuilderExporter
    {
        public const BuildAssetBundleOptions BundleOptions = BuildAssetBundleOptions.None;
        public const BuildTarget BundleBuildTarget = BuildTarget.StandaloneWindows;

        /// <summary>
        ///     Exports selected assets directly.
        /// </summary>
        [MenuItem("Assets/Export Assets Directly", false, 2)]
        public static void ExportSelectedDirectly(MenuCommand menuCommand)
        {
            if (Selection.objects.Length == 0)
                EditorUtility.DisplayDialog("Oops.", "No assets selected to build.", "Ok");
            else
            {
                // Try to load configuration first
                JEMAssetsBuilderConfiguration.TryLoadConfiguration();

                // Get save file path
                var path = EditorUtility.SaveFilePanel("Export Assets", Environment.CurrentDirectory, "myAssetBundle", JEMAssetsBuilderConfiguration.GetExtension().Remove(0, 1));
                if (string.IsNullOrEmpty(path))
                    return;
            
                // Export directly.
                ExportDirectly(path, Selection.objects);
            }
        }

        /// <summary>
        ///     Exports directly target array of objects.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static void ExportDirectly([NotNull] string filePath, [NotNull] Object[] objects)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            if (objects.Length == 0) return;
            if (string.IsNullOrEmpty(filePath)) return;
  
            // Try to load configuration first
            JEMAssetsBuilderConfiguration.Load();

            // Create package from received data
            var package = new JEMAssetBuilderPackage
            {
                Name = Path.GetFileName(filePath).Remove(Path.GetFileName(filePath).Length - JEMAssetsBuilderConfiguration.GetExtension().Length, 
                    JEMAssetsBuilderConfiguration.GetExtension().Length)
            };

            // Write objects to new package
            foreach (var obj in objects)
                package.AddAsset(obj);

            // Export package.
            ExportPackages(Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException(), new[] {package});
        }

        /// <summary>
        ///     Export received array of packages.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static void ExportPackages([NotNull] string directory, [NotNull] JEMAssetBuilderPackage[] packages)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));
            if (packages == null) throw new ArgumentNullException(nameof(packages));
            if (packages.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(packages));
            EditorUtility.DisplayProgressBar("JEM Asset Builder", "Starting up.", 0);

            try
            {
                var bundleBuildDirectory = JEMAssetsBuilderConfiguration.GetDirectory();
                var bundleBuildList = new List<AssetBundleBuild>();
                for (var index = 0; index < packages.Length; index++)
                {
                    var package = packages[index];
                    EditorUtility.DisplayProgressBar("JEM Asset Builder", "Preparing to export: " + package.Name,
                        (float) index / packages.Length * 100f);

                    var filePath = package.GetFile();
                    var fileName = Path.GetFileName(filePath);
                    var bundleBuildData = new AssetBundleBuild
                    {
                        assetBundleName = fileName,
                        assetNames = package.GetPathToAssets()
                    };

                    bundleBuildList.Add(bundleBuildData);
                }

                EditorUtility.DisplayProgressBar("JEM Asset Builder", "Starting Unity's AssetBundles building.", 0f);

                var manifest = BuildPipeline.BuildAssetBundles(bundleBuildDirectory, bundleBuildList.ToArray(), BundleOptions, BundleBuildTarget);
                if (manifest != null && manifest.GetAllAssetBundles().Length != 0)
                {
                    Debug.Log($"JEM Asset Builder successfully build {bundleBuildList.Count} packages!");
                   
                    // Show target directory in File Explorer
                    Process.Start(bundleBuildDirectory);
                }
                else
                {
                    Debug.LogError($"JEM Asset Builder failed to build {bundleBuildList.Count} packages of Asset Bundles.");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}