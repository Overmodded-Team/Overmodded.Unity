//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.AssetBundles
{
    /// <inheritdoc />
    /// <summary>
    ///     JEM Asset Builder add package window.
    /// </summary>
    public class JEMAssetBuilderAddPackageWindow : EditorWindow
    {
        private string _packageName;

        private void OnEnable()
        {
            // Try to load configuration!
            JEMAssetsBuilderConfiguration.TryLoadConfiguration();
        }

        // Do we really need this??
        private void OnInspectorUpdate() => Repaint();
        
        private void OnGUI()
        {
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            _packageName = EditorGUILayout.TextField("Package Name", _packageName);
            GUILayout.Label(JEMAssetsBuilderConfiguration.GetExtension());
            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add", GUILayout.Height(22)))
            {
                AddPackage();
            }
        }

        /// <summary>
        ///     Add the package!
        /// </summary>
        private void AddPackage()
        {
            if (string.IsNullOrEmpty(_packageName) || string.IsNullOrWhiteSpace(_packageName))
            {
                EditorUtility.DisplayDialog("Oops.", "Please, enter the package name.", "Ok");
                return;
            }

            var packageData = JEMAssetBuilderWindow.GetPackage(_packageName);
            if (packageData != null)
            {
                EditorUtility.DisplayDialog("Oops.", $"Package of the name `{_packageName}` already exists.", "Ok");
                return;
            }

            var newPackage = new JEMAssetBuilderPackage
            {
                Name = _packageName
            };

            JEMAssetBuilderWindow.AddPackage(newPackage);

            Close();
        }

        /// <summary>
        ///     Show asset builder add window.
        /// </summary>
        public static void ShowWindow()
        {
            var activeWindow = GetWindow<JEMAssetBuilderAddPackageWindow>(true, "Add New Package", true);
            activeWindow.maxSize = new Vector2(350, 56);
            activeWindow.minSize = new Vector2(350, 56);
            activeWindow._packageName = string.Empty;
        }
    }
}