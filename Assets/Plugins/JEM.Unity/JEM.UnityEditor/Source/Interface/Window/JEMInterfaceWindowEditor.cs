//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Interface.Window;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.Interface.Window
{
    [CustomEditor(typeof(JEMInterfaceWindow)), SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class JEMInterfaceWindowEditor : Editor
    {
        private SerializedProperty UniqueWindowName;

        private SerializedProperty AllowDragging;
        private SerializedProperty AllowResize;
        private SerializedProperty AlwaysMoveOnTop;

        private SerializedProperty ClampWindowSize;
        private SerializedProperty MaxWindowSize;
        private SerializedProperty MinWindowSize;
        private SerializedProperty RootTransform;

        private SerializedProperty ShouldActivateGameObject;

        private SerializedProperty OnWindowActivated;
        private SerializedProperty OnWindowDeactivated;

        private JEMInterfaceWindow script;

        protected virtual void OnEnable()
        {
            UniqueWindowName = serializedObject.FindProperty(nameof(UniqueWindowName));

            RootTransform = serializedObject.FindProperty(nameof(RootTransform));

            AllowDragging = serializedObject.FindProperty(nameof(AllowDragging));
            AllowResize = serializedObject.FindProperty(nameof(AllowResize));
            AlwaysMoveOnTop = serializedObject.FindProperty(nameof(AlwaysMoveOnTop));

            ClampWindowSize = serializedObject.FindProperty(nameof(ClampWindowSize));
            MinWindowSize = serializedObject.FindProperty(nameof(MinWindowSize));
            MaxWindowSize = serializedObject.FindProperty(nameof(MaxWindowSize));

            ShouldActivateGameObject = serializedObject.FindProperty(nameof(ShouldActivateGameObject));

            OnWindowActivated = serializedObject.FindProperty(nameof(OnWindowActivated));
            OnWindowDeactivated = serializedObject.FindProperty(nameof(OnWindowDeactivated));

            InternalUpdateScript();
        }

        protected virtual bool InternalReady()
        {
            return script != null;
        }

        protected virtual void InternalUpdateScript()
        {
            script = target as JEMInterfaceWindow;
            if (script == null)
                return;
        }

        public override void OnInspectorGUI()
        {
            if (!InternalReady())
            {
                InternalUpdateScript();
                EditorGUILayout.HelpBox(
                    $"Unable to draw {nameof(JEMInterfaceWindow)} inspector gui. Target script is null.",
                    MessageType.Error);
                return;
            }

            if (script.RootTransform == null)
            {
                EditorGUILayout.HelpBox("Please set RootTransform property.", MessageType.Warning, true);
            }
            else
            {
                if (script.RootTransform.pivot != new Vector2(0.5f, 0.5f))
                    EditorGUILayout.HelpBox(
                        "It's looks like you are using other pivot than (0.5f, 0.5f). In current version, InterfaceWindow.UpdateDisplay may not work properly.",
                        MessageType.Error, true);
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(UniqueWindowName);

            EditorGUILayout.PropertyField(RootTransform);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(AllowDragging);
            EditorGUILayout.PropertyField(AllowResize);
            EditorGUILayout.PropertyField(AlwaysMoveOnTop);

            EditorGUILayout.PropertyField(ClampWindowSize);
            if (ClampWindowSize.boolValue)
            {
                EditorGUILayout.PropertyField(MinWindowSize);
                EditorGUILayout.PropertyField(MaxWindowSize);
            }

            EditorGUILayout.PropertyField(ShouldActivateGameObject);

            EditorGUILayout.PropertyField(OnWindowActivated);
            EditorGUILayout.PropertyField(OnWindowDeactivated);

            serializedObject.ApplyModifiedProperties();
        }
    }
}