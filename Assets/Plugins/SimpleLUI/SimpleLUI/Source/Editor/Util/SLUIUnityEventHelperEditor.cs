//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using SimpleLUI.API.Util;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleLUI.Editor.Util
{
    [CustomEditor(typeof(SLUIUnityEventHelper))]
    public class SLUIUnityEventHelperEditor : UnityEditor.Editor
    {
        private SLUIUnityEventHelper _target;
        private void OnEnable()
        {
            _target = (SLUIUnityEventHelper) target;
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("SLUI UNITY EVENT HELPER - This script is needed because of UnityEvents\n" +
                                    "design that is not allowing to get or set parameters.", MessageType.Info, true);

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            _target.Items = JEMBetterEditor.ContentList(_target.Items, _target, o =>
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("Parameters", EditorStyles.boldLabel);
                o.obj = JEMBetterEditor.ContentList(o.obj, _target, s =>
                {
                    s = EditorGUILayout.TextField(s);
                    return s;
                }, new JEMBetterEditorStyle("Objects", $"{nameof(SLUIUnityEventHelperEditor)}.Obj_{_target.Items.IndexOf(o)}") { FoldoutHeaderType = JEMBetterEditorHeaderType.None });

                CheckEvent(_target.Items.IndexOf(o), o.obj);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                return o;
            }, new JEMBetterEditorStyle("Events", $"{nameof(SLUIUnityEventHelperEditor)}.Events"){FoldoutHeaderType = JEMBetterEditorHeaderType.None});

            var button = _target.GetComponent<Button>();
            if (button == null) return;
            if (_target.Items.Count != button.onClick.GetPersistentEventCount())
            {
                EditorGUILayout.HelpBox("Error. You have defined invalid amount of events.", MessageType.Error, true);
            }
        }

        private void CheckEvent(int index, List<string> parameters)
        {
            // check compatibility
            var button = _target.GetComponent<Button>();
            if (button == null) return;

            var methodName = button.onClick.GetPersistentMethodName(index);
            var methodTarget = button.onClick.GetPersistentTarget(index);
            if (string.IsNullOrEmpty(methodName))
                return;

            if (methodTarget == null)
                return;

            var methodInfo = methodTarget.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null)
            {
                EditorGUILayout.HelpBox($"Method {methodTarget.GetType().Name}.{methodName} not found.",
                    MessageType.Error, true);
            }
            else
            {
                var par = methodInfo.GetParameters();
                if (par.Length != parameters.Count)
                    EditorGUILayout.HelpBox($"Method error. Invalid number of parameters.", MessageType.Error, true);
                else
                {
                    for (int i1 = 0; i1 < par.Length; i1++)
                    {
                        var real = par[i1];
                        var str = parameters[i1];

                        var t = real.ParameterType;
                        if (t == typeof(string))
                            continue; // ok

                        bool parseSuccess = false;
                        if (t == typeof(int) && int.TryParse(str, out _)) parseSuccess = true;
                        else if (t == typeof(bool) && bool.TryParse(str, out _)) parseSuccess = true;
                        else if (t == typeof(float) && float.TryParse(str, out _)) parseSuccess = true;
                        
                        if (!parseSuccess)
                        {
                            EditorGUILayout.HelpBox(
                                $"Method error. Unable to parse value of parameter #{i1} in to {real.ParameterType.Name}",
                                MessageType.Error, true);
                            return;
                        }
                    }

                    EditorGUILayout.HelpBox(
                        $"Method OK. '{methodTarget.GetType().Name}.{methodName}({BuildParametersString(par, parameters)})'",
                        MessageType.Info, true);
                }
            }
        }

        private string BuildParametersString(ParameterInfo[] parameters, List<string> values)
        {
            var s = string.Empty;
            for (var index = 0; index < parameters.Length; index++)
            {
                var p = parameters[index];
                //s += $"{p.ParameterType.Name} {p.Name}";
                s += $"{values[index]}";
                if (index + 1 < parameters.Length)
                    s += ", ";
            }

            return s;
        }
    }
}
