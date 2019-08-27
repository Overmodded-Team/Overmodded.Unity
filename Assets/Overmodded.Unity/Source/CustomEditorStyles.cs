//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity
{
    [CreateAssetMenu(fileName = "newCustomEditorStyles", menuName = "Overmodded/Editor/Custom Editor Style", order = 0)]
    public class CustomEditorStyles : ScriptableObject
    {
        [Header("Custom Style")]
        public GUIStyle SmallInfoBox;

        public static bool IsCustomStyleLoaded()
        {
            if (Loaded == null)
            {
                Debug.LogError($"CustomEditorStyles.Loaded returns null. Unable to find any CustomEditorStyles asset in current project.");
                return false;
            }

            return true;
        }

        private static CustomEditorStyles _loaded;
        public static CustomEditorStyles Loaded
        {
            get
            {
                if (_loaded == null)
                {
                    var assets = AssetDatabase.FindAssets($"t:{nameof(CustomEditorStyles)}");
                    if (assets.Length != 0)
                        _loaded = AssetDatabase.LoadAssetAtPath<CustomEditorStyles>(AssetDatabase.GUIDToAssetPath(assets[0]));
                }

                return _loaded;
            }
        }
    }
}
#endif