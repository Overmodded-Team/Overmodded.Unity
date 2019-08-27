//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Common
{
    public static class EditorCommon
    {
        [MenuItem("Assets/Duplicate", false, priority = 15)]
        public static void Duplicate(MenuCommand c)
        {
            var g = Selection.assetGUIDs;
            if (g.Length == 0)
            {
                Debug.Log("No context to duplicate.");
                return;
            }

            foreach (var s in g)
            {
                var path = AssetDatabase.GUIDToAssetPath(s);
                if (!File.Exists(path))
                {
                    Debug.Log($"Not a file. ({path})");
                }

                var newPath = $"{Path.GetDirectoryName(path)}{JEMVar.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(path)} (Duplicate).asset";
                if (AssetDatabase.CopyAsset(path, newPath))
                {
                    Debug.Log($"{path} copied in to {newPath}");
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                    var obj = AssetDatabase.LoadAssetAtPath<Object>(newPath);
                    Selection.SetActiveObjectWithContext(obj, obj);
                }
                else
                {
                    Debug.LogError($"Failed to copy {path} in to {newPath}");
                }
            }
        }

        // [MenuItem("Assets/Copy", false, priority = 15)]
        public static void Copy(MenuCommand c)
        {
            _copiedFiles = Selection.assetGUIDs;
        }

        // [MenuItem("Assets/Paste", false, priority = 15)]
        public static void Paste(MenuCommand c)
        {
            var dirs = Selection.assetGUIDs;
            if (dirs.Length == 0)
            {
                return;
            }

            var g = _copiedFiles;
            if (g == null || g.Length == 0)
            {
                Debug.Log("Nothing to paste.");
                return;
            }

            var dir = Path.GetDirectoryName(dirs[0]);
            if (!dir.Contains("Assets"))
            {
                Debug.Log($"Invalid dir given. ({dir})");
                return;
            }

            foreach (var s in g)
            {
                var path = AssetDatabase.GUIDToAssetPath(s);
                if (!File.Exists(path))
                {
                    Debug.Log($"Not a file. ({path})");
                }

                var newPath = $"{dir}{JEMVar.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(path)} (Duplicate).asset";
                if (AssetDatabase.CopyAsset(path, newPath))
                {
                    Debug.Log($"{path} copied in to {newPath}");
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    Debug.LogError($"Failed to copy {path} in to {newPath}");
                }
            }
        }

        private static string[] _copiedFiles;
    }
}
