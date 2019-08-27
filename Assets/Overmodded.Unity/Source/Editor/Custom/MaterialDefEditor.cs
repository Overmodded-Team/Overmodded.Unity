//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Gameplay.Level.Materials;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(MaterialDef))]
    internal class MaterialDefEditor : UnityEditor.Editor
    {
        private MaterialDef _target;
        private MaterialSettings _materialSettings;

        private void OnEnable()
        {
            _target = (MaterialDef)target;

            if (AllMaterials.Count == 0) RefreshDatabase();
            RefreshMaterial();
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _materialSettings = (MaterialSettings)EditorGUILayout.ObjectField("Material Settings", _materialSettings, typeof(MaterialSettings), false);
            _target.MaterialIdentity = _materialSettings == null ? 0 : _materialSettings.Identity;

            JEMBetterEditor.DrawProperty(" ", () =>
            {
                if (GUILayout.Button("Refresh Material"))
                {
                    RefreshDatabase();
                    RefreshMaterial();
                }
            });
        }

        private void RefreshDatabase()
        {
            AllMaterials.Clear();
            string[] items = AssetDatabase.FindAssets($"t:{typeof(MaterialSettings)}");
            foreach (string item in items)
            {
                MaterialSettings s =
                    (MaterialSettings)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item),
                        typeof(MaterialSettings));
                AllMaterials.Add(s);
            }
        }

        private void RefreshMaterial()
        {
            EditorUtility.SetDirty(target);
            foreach (var s in AllMaterials)
            {
                if (s.Identity == _target.MaterialIdentity)
                    _materialSettings = s;
            }
        }

        /// <summary>
        ///     List of all materials in scene.
        /// </summary>
        private static List<MaterialSettings> AllMaterials { get; } = new List<MaterialSettings>();
    }
}
