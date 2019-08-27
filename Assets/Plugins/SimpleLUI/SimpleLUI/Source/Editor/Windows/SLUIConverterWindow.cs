//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEditor;
using UnityEngine;

namespace SimpleLUI.Editor.Windows
{
    public class SLUIConverterWindow : EditorWindow
    {
        private Canvas Canvas;
        private string File;
        private bool PrettyPrint = true;

        private void OnGUI()
        {
            Canvas = (Canvas) EditorGUILayout.ObjectField("Canvas (Root)", Canvas, typeof(Canvas), true);

            EditorGUILayout.BeginHorizontal();
            {
                GUI.enabled = false;
                File = EditorGUILayout.TextField(File);
                GUI.enabled = true;
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    var file = EditorUtility.SaveFilePanel("Save Script", $"{Environment.CurrentDirectory}\\LUI", "lui_script", "lua");
                    if (!string.IsNullOrEmpty(file))
                        File = file;
                }
            }
            EditorGUILayout.EndHorizontal();

            PrettyPrint = EditorGUILayout.Toggle("Pretty Print", PrettyPrint);

            if (GUILayout.Button("Convert"))
            {
                if (string.IsNullOrEmpty(File))
                    File = EditorUtility.SaveFilePanel("Save Script", $"{Environment.CurrentDirectory}\\LUI", "lui_script", "lua");

                if (!string.IsNullOrEmpty(File))
                {
                    SLUIEngineToScriptConverter.Convert(Canvas, File, PrettyPrint);
                }
            }
        }

        [MenuItem("Tools/SLUI/SLUI Converter")]
        internal static void ShowConverter()
        {
            GetWindow<SLUIConverterWindow>(true, "UI To Lua Convert", true);
        }
    }
}
