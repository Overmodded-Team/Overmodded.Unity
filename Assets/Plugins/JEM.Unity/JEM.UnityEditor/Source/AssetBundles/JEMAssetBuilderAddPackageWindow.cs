//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.AssetBundles
{
    /// <inheritdoc />
    /// <summary>
    ///     Asset builder add package window.
    /// </summary>
    public class JEMAssetBuilderAddPackageWindow : EditorWindow
    {
        private string _packageDirectory;
        private string _packageName;

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            _packageName = EditorGUILayout.TextField(_packageName);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.TextField(_packageDirectory);
                if (GUILayout.Button("Select"))
                    _packageDirectory = EditorUtility.OpenFolderPanel("Select directory of package",
                        Environment.CurrentDirectory, "");
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add"))
            {
                if (string.IsNullOrEmpty(_packageName) || string.IsNullOrWhiteSpace(_packageName))
                {
                    EditorUtility.DisplayDialog("Oops.", "Please, enter the package name.", "Ok");
                }
                else
                {
                    if (string.IsNullOrEmpty(_packageDirectory) || !Directory.Exists(_packageDirectory))
                    {
                        EditorUtility.DisplayDialog("Oops.", "Please, select package directory.", "Ok");
                    }
                    else
                    {
                        if (JEMAssetBuilderWindow.GetPackage(_packageName) != null)
                        {
                            EditorUtility.DisplayDialog("Oops.", $"Package of name {_packageName} already exists.",
                                "Ok");
                        }
                        else
                        {
                            Uri targetDir = new Uri(_packageDirectory);
                            // Must end in a slash to indicate folder
                            Uri mainDir = new Uri(Environment.CurrentDirectory);
                            string relativePath = Uri.UnescapeDataString(mainDir.MakeRelativeUri(targetDir).ToString().Replace('/', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/');
                            var n = Path.GetFileName(Environment.CurrentDirectory);
                           // Debug.Log(Environment.CurrentDirectory + " | " + n + " | " + relativePath);

                            relativePath = relativePath.Replace($@"{n}/", string.Empty);

                            JEMAssetBuilderWindow.Packages.Add(new JEMAssetBuilderPackage
                            {
                                Name = _packageName,
                                Directory = relativePath,
                                Assets = new List<JEMAssetBuilderPackage.Asset>()
                            });
                            JEMAssetBuilderWindow.SelectedPackage = _packageName;
                            Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Show window.
        /// </summary>
        public static void ShowWindow()
        {
            _activeWindow = GetWindow(typeof(JEMAssetBuilderAddPackageWindow)) as JEMAssetBuilderAddPackageWindow;
            if (_activeWindow == null) return;
            _activeWindow.titleContent = new GUIContent("Add Package");
            _activeWindow.maxSize = new Vector2(350, 56);
            _activeWindow.minSize = new Vector2(350, 56);
            _activeWindow.ShowPopup();
            _activeWindow._packageName = string.Empty;
        }

        private static JEMAssetBuilderAddPackageWindow _activeWindow;
    }
}