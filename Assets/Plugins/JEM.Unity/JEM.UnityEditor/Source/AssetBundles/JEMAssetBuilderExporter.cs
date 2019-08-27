//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JEM.UnityEditor.AssetBundles
{
    /// <summary>
    ///     Asset builder exporter class.
    /// </summary>
    public static class JEMAssetBuilderExporter
    {
        /// <summary>
        ///     Exports selected assets directly.
        /// </summary>
        [MenuItem("Assets/Export Assets Directly", false, 2)]
        public static void ExportSelectedDirectly(MenuCommand menuCommand)
        {
            if (Selection.objects.Length == 0)
            {
                EditorUtility.DisplayDialog("Oops.", "No assets selected to build.", "Ok");
            }
            else
            {
                JEMAssetsBuilderConfiguration.LoadConfiguration();

                var path = EditorUtility.SaveFilePanel("Export assets", Environment.CurrentDirectory, "assets",
                    JEMAssetsBuilderConfiguration.GetExtension().Remove(0, 1));
                if (string.IsNullOrEmpty(path))
                    return;

                ExportDirectly(path, Selection.objects);
            }
        }

        /// <summary>
        ///     Exports given objects directly.
        /// </summary>
        public static void ExportDirectly(string path, Object[] objects)
        {
            if (objects.Length == 0)
                return;
            if (string.IsNullOrEmpty(path))
                return;
            JEMAssetsBuilderConfiguration.LoadConfiguration();

            var package = new JEMAssetBuilderPackage
            {
                Name = Path.GetFileName(path)
                    .Remove(Path.GetFileName(path).Length - JEMAssetsBuilderConfiguration.GetExtension().Length,
                        JEMAssetsBuilderConfiguration.GetExtension().Length),
                Directory = Path.GetDirectoryName(path),
                Assets = new List<JEMAssetBuilderPackage.Asset>()
            };

            foreach (var file in objects.Select(AssetDatabase.GetAssetPath))
                package.Assets.Add(new JEMAssetBuilderPackage.Asset
                {
                    Path = file,
                    Include = true
                });

            ExportPackages(new[] {package});
        }

        /// <summary>
        ///     Exports list of packages.
        /// </summary>
        /// <param name="packages"></param>
        public static void ExportPackages(JEMAssetBuilderPackage[] packages)
        {
            EditorUtility.DisplayProgressBar("Exporting asset packages.", "Preparing to export.", 0);

            for (var index = 0; index < packages.Length; index++)
            {
                var package = packages[index];
                EditorUtility.DisplayProgressBar("Exporting asset packages.", "Exporting " + package.Name,
                    (float) index / packages.Length);

                var file = package.GetFile();
                var fileName = Path.GetFileName(file);
                var directoryName = Path.GetDirectoryName(file);
                var bundleBuild = new AssetBundleBuild[1];

                bundleBuild[0].assetBundleName = fileName;
                var assets = new List<string>();
                foreach (var asset in package.Assets)
                    if (asset.Include)
                        assets.Add(asset.Path);
                bundleBuild[0].assetNames = assets.ToArray();

                Debug.LogWarning($"PACKAGE EXPORT to file {package.GetFile()}");
                var manifest = BuildPipeline.BuildAssetBundles(directoryName, bundleBuild,
                    BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
                if (manifest != null && manifest.GetAllAssetBundles().Length != 0)
                {
                    Debug.Log($"PACKAGE EXPORT ({package.Name}) SUCCESS!");
                    if (directoryName != null)
                        Process.Start(directoryName);
                }
                else
                {
                    Debug.LogError($"PACKAGE EXPORT ERROR! [ IsNULL -> {manifest == null} ]");
                }
            }

            EditorUtility.ClearProgressBar();
        }
    }
}