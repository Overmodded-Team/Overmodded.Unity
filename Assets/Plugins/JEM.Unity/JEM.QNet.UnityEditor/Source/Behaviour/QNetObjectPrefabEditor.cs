//
// QNet for UnityEditor - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Linq;
using JEM.QNet.UnityEngine.Behaviour;
using UnityEditor;
using UnityEngine;

namespace JEM.QNet.UnityEditor.Behaviour
{
    [CustomEditor(typeof(QNetObjectPrefab))]
    internal class QNetObjectPrefabEditor : Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            // invoke base method
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Regenerate Identity", GUILayout.Height(25)))
            {
                EditorUtility.SetDirty(target);
                var prefab = (QNetObjectPrefab) target;

                var prefabsGUIDs = AssetDatabase.FindAssets($"t:{nameof(QNetObjectPrefab)}");
                var loadedPrefabs = new QNetObjectPrefab[prefabsGUIDs.Length];
                for (var index = 0; index < prefabsGUIDs.Length; index++)
                    loadedPrefabs[index] =
                        (QNetObjectPrefab) AssetDatabase.LoadAssetAtPath(
                            AssetDatabase.GUIDToAssetPath(prefabsGUIDs[index]), typeof(QNetObjectPrefab));

                var identity = (short) Random.Range(short.MinValue, short.MaxValue);
                while (loadedPrefabs.Any(p => p.PrefabIdentity == identity))
                    identity = (short) Random.Range(short.MinValue, short.MaxValue);

                prefab.PrefabIdentity = identity;

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }
}