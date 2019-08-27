//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Interface;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace JEM.UnityEditor.Interface
{
    [CustomEditor(typeof(JEMInterfaceCursor)), SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class JEMInterfaceCursorEditor : Editor
    {
        private SerializedProperty CursorIconDefault;
        private SerializedProperty CursorIconMove;
        private SerializedProperty CursorIconResize;
        private SerializedProperty CursorIconRotate;
        private SerializedProperty CursorImage;

        private JEMInterfaceCursor script;

        protected virtual void OnEnable()
        {
            CursorImage = serializedObject.FindProperty(nameof(CursorImage));
            CursorIconDefault = serializedObject.FindProperty(nameof(CursorIconDefault));
            CursorIconMove = serializedObject.FindProperty(nameof(CursorIconMove));
            CursorIconRotate = serializedObject.FindProperty(nameof(CursorIconRotate));
            CursorIconResize = serializedObject.FindProperty(nameof(CursorIconResize));

            InternalUpdateScript();
        }

        protected virtual bool InternalReady()
        {
            return script != null;
        }

        protected virtual void InternalUpdateScript()
        {
            script = target as JEMInterfaceCursor;
            if (script == null)
                return;
        }

        protected AnimBool RegisterAnimatedBool(AnimBool animBool, bool defaultState)
        {
            if (animBool == null)
                animBool = new AnimBool(defaultState);
            animBool.valueChanged.AddListener(Repaint);
            return animBool;
        }

        public override void OnInspectorGUI()
        {
            if (!InternalReady())
            {
                InternalUpdateScript();
                EditorGUILayout.HelpBox(
                    $"Unable to draw {nameof(JEMInterfaceCursor)} inspector gui. Target script is null.",
                    MessageType.Error);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(CursorImage);
            if (CursorImage.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("CursorImage is not set. System will use default system cursor.",
                    MessageType.Info, true);
            }
            else
            {
                EditorGUILayout.PropertyField(CursorIconDefault);
                EditorGUILayout.PropertyField(CursorIconMove);
                EditorGUILayout.PropertyField(CursorIconResize);
                EditorGUILayout.PropertyField(CursorIconRotate);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}