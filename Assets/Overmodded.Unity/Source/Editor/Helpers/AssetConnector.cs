//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Helpers
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple editor extension that let you connect selected assets.
    /// </summary>
    internal class AssetConnector : EditorWindow
    {
        private bool _disconnect;
        private bool _void;
        private ScriptableObject _obj1;
        private ScriptableObject _obj2;
        private ScriptableObject _obj3;

        private void OnGUI()
        {
            _disconnect = EditorGUILayout.Toggle("Disconnect", _disconnect);
            if (_disconnect)
            {
                _obj3 = (ScriptableObject)EditorGUILayout.ObjectField("To Disconnect", _obj3, typeof(ScriptableObject), false);
                _void = EditorGUILayout.Toggle("Void", _void);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Disconnect"))
                {
                    if (_void)
                    {
                        AssetDatabase.RemoveObjectFromAsset(_obj3);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        var file = AssetDatabase.GetAssetPath(_obj3);
                        var fileName = Path.GetFileNameWithoutExtension(file) + $"_{_obj3.GetType().Name}";
                        var path = EditorUtility.SaveFilePanelInProject("Save Disconnected Asset", fileName, "asset",
                            "",
                            Path.GetDirectoryName(file));
                        if (!string.IsNullOrEmpty(path))
                        {
                            var obj = Instantiate(_obj3);
                            obj.name = fileName;
                            AssetDatabase.CreateAsset(obj, path);
                            AssetDatabase.RemoveObjectFromAsset(_obj3);
                            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
            else
            {
                _obj1 = (ScriptableObject)EditorGUILayout.ObjectField("To Add", _obj1, typeof(ScriptableObject), false);
                _obj2 = (ScriptableObject)EditorGUILayout.ObjectField("Parent", _obj2, typeof(ScriptableObject), false);

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Connect"))
                {
                    var obj = Instantiate(_obj1);
                    obj.name = obj.name.Remove(obj.name.Length - 7, 7);

                    AssetDatabase.AddObjectToAsset(obj, _obj2);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

        [MenuItem("Tools/Asset Connector")]
        internal static void ShowWindow() => GetWindow<AssetConnector>(true, "Asset Connector", true);     
    }
}
