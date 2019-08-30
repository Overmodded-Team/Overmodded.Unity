//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Common;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Common
{
    [CustomEditor(typeof(DatabaseItem), true, isFallback = true)]
    public class DatabaseItemEditor : UnityEditor.Editor
    {
        private SavedBool _drawDatabaseItemContent;
        private SavedBool _drawDatabaseItemSettings;

        protected virtual void OnEnable()
        {
            _drawDatabaseItemContent = new SavedBool($"{GetType().Name}.DrawItemContent", false);
            _drawDatabaseItemSettings = new SavedBool($"{nameof(DatabaseItem)}.DrawItemSettings", false);
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _drawDatabaseItemContent.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawDatabaseItemContent.value, $"{JEMBetterEditor.FixedPropertyName(target.GetType().Name)} Content");
            if (_drawDatabaseItemContent.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                // invoke base method
                base.OnInspectorGUI();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            // And draw the content...
            DrawDatabaseItemGUI();
        }

        protected void DrawDatabaseItemGUI()
        {
            var item = (DatabaseItem) target;

            _drawDatabaseItemSettings.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawDatabaseItemSettings.value, "Database Item Settings");
            if (_drawDatabaseItemSettings.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Unique Identity", item.Identity.ToString());
                JEMBetterEditor.DrawProperty(" ", () =>
                {
                    if (GUILayout.Button("Regenerate"))
                    {
                        bool canRegenerate = true;
                        if (item.Identity != 0)
                        {
                            canRegenerate = EditorUtility.DisplayDialog("Regenerate?",
                                "Are you sure you want to regenerate the identity of this object? All references will be lost!",
                                "Yes", "No");
                        }

                        if (canRegenerate)
                            RegenerateIdentity();
                    }
                });

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // Draw extras
            EditorGUILayoutGameUtility.DrawTargetExtras(target);
        }

        /// <summary>
        ///     Regenerates the identity of this item.
        /// </summary>
        internal void RegenerateIdentity()
        {
            DatabaseItem item = (DatabaseItem) target;
            EditorUtility.SetDirty(target);

            string[] itemsGuiDs = AssetDatabase.FindAssets($"t:{nameof(DatabaseItem)}");
            DatabaseItem[] loadedItems = new DatabaseItem[itemsGuiDs.Length];
            for (var index = 0; index < itemsGuiDs.Length; index++)
                loadedItems[index] = (DatabaseItem) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(itemsGuiDs[index]), typeof(DatabaseItem));

            int identity = 0;
            bool any = true;
            while (any)
            {
                identity = GetRandomInt();
                any = loadedItems.Any(p => p != null && p.Identity == identity);
            }
            item.Identity = identity;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static int GetRandomInt() => (int) Random.Range(int.MinValue, int.MaxValue);
    }
}
