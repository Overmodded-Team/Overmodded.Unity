//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Common
{
    [CustomEditor(typeof(DatabaseItem), true, isFallback = true)]
    internal class DatabaseItemEditor : UnityEditor.Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            // invoke base method
            base.OnInspectorGUI();

            DatabaseItem item = (DatabaseItem)target;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal("box");
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Identity {item.Identity}");
                GUILayout.FlexibleSpace();

                EditorGUILayout.Space();
                if (GUILayout.Button("Regenerate Identity"))
                {
                    RegenerateIdentity();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        internal void RegenerateIdentity()
        {
            DatabaseItem item = (DatabaseItem)target;
            EditorUtility.SetDirty(target);

            string[] itemsGuiDs = AssetDatabase.FindAssets($"t:{nameof(DatabaseItem)}");
            DatabaseItem[] loadedItems = new DatabaseItem[itemsGuiDs.Length];
            for (var index = 0; index < itemsGuiDs.Length; index++)
                loadedItems[index] =
                    (DatabaseItem)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(itemsGuiDs[index]),
                        typeof(DatabaseItem));

            var identity = (short)Random.Range(short.MinValue, short.MaxValue);
            bool any = loadedItems.Any(p => p != null && p.Identity == identity);
            while (any)
            {
                identity = (short)Random.Range(short.MinValue, short.MaxValue);
            }

            item.Identity = identity;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}
