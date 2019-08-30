//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor;
using Overmodded.Gameplay.Character;
using Overmodded.Gameplay.Level;
using Overmodded.Unity.Editor.Common;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.Custom
{
    [CustomEditor(typeof(LevelObject))]
    internal class LevelObjectEditor : UnityEditor.Editor
    {
        private LevelObject _target;
        private SavedBool _drawNetworkBehaviour;
        private SavedBool _drawReferences;

        private void OnEnable()
        {
            _target = (LevelObject) target;
            _drawNetworkBehaviour = new SavedBool($"{_target.GetType()}.DrawNetworkBehaviour", false);
            _drawReferences = new SavedBool($"{_target.GetType()}.DrawReferences", false);
            EditorUtility.SetDirty(target);
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            _target.Identity = (short) EditorGUILayout.IntField("Identity", _target.Identity);
            JEMBetterEditor.DrawProperty(" ", () =>
            {
                if (GUILayout.Button("Regenerate Identity"))
                {
                    RefreshIdentity();
                }
            });

            _drawNetworkBehaviour.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawNetworkBehaviour.value, "Network Behaviour");
            if (_drawNetworkBehaviour.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                _target.NetworkBehaviour.ActiveOnClient = EditorGUILayout.Toggle("Active On Client", _target.NetworkBehaviour.ActiveOnClient);
                _target.NetworkBehaviour.ActiveOnServer = EditorGUILayout.Toggle("Active On Server", _target.NetworkBehaviour.ActiveOnServer);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            _drawReferences.value = EditorGUILayout.BeginFoldoutHeaderGroup(_drawReferences.value, "References");
            if (_drawReferences.value)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;
                _target.References.ModelRoot = (GameObject) EditorGUILayout.ObjectField("Model Root", _target.References.ModelRoot, typeof(GameObject), true);
                _target.References.CharacterSettings = (CharacterSettingsReference) EditorGUILayout.ObjectField("Character Settings", _target.References.CharacterSettings, typeof(CharacterSettingsReference), false);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (_target.References.ModelRoot == null)
                EditorGUILayout.HelpBox("ModelRoot of level object need to be set!", MessageType.Error, true);
            else if (_target.References.ModelRoot == _target.gameObject)
                EditorGUILayout.HelpBox("Setting this gameObject as ModelRoot may create problems.", MessageType.Warning, true);

            EditorGUILayoutGameUtility.DrawTargetExtras(_target);
        }

        private void RefreshIdentity()
        {
            LevelObject[] loadedObjects = FindObjectsOfType<LevelObject>();
            var identity = (short)Random.Range(short.MinValue, short.MaxValue);
            while (loadedObjects.Any(p => p.Identity == identity))
            {
                identity = (short)Random.Range(short.MinValue, short.MaxValue);
            }

            _target.Identity = identity;
        }

        /// <summary>
        ///     Regenerates identity of all LevelObjects in scene.
        /// </summary>
        [MenuItem("Overmodded/Scene/Refresh All Level Objects")]
        internal static void RefreshAllOnScene()
        {
            LevelObject[] loadedObjects = FindObjectsOfType<LevelObject>();
            foreach (var o in loadedObjects)
            {
                var identity = (short)Random.Range(short.MinValue, short.MaxValue);
                while (loadedObjects.Any(p => p.Identity == identity))
                {
                    identity = (short)Random.Range(short.MinValue, short.MaxValue);
                }

                o.Identity = identity;
            }

            Debug.Log($"{loadedObjects.Length} object's identity refreshed.");
        }
    }
}
