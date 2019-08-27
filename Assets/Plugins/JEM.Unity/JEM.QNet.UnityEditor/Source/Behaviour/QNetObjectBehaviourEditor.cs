//
// QNet for UnityEditor - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.UnityEngine.Behaviour;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JEM.QNet.UnityEditor.Behaviour
{
    [CustomEditor(typeof(QNetObjectBehaviour), true)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class QNetObjectBehaviourEditor : Editor
    {
        private QNetObjectBehaviour _script;
        private bool _work;
        private short Identity;

        private void OnEnable()
        {
          
            _script = (QNetObjectBehaviour) target;
            // var assetType = PrefabUtility.GetPrefabAssetType(target);
            // var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(target);
            var type = PrefabUtility.GetPrefabType(target);
            _work = type == PrefabType.None || type == PrefabType.PrefabInstance ||
                    type == PrefabType.MissingPrefabInstance || type == PrefabType.ModelPrefabInstance ||
                    type == PrefabType.DisconnectedPrefabInstance || type == PrefabType.DisconnectedModelPrefabInstance;
            if (_work)
            {
                Identity = _script.ObjectIdentity;
                Identity = FixPredefinedIdentity(Identity);
                if (!Application.isPlaying) _script.UpdateIdentity(Identity, 0);
            }
            else
            {
                _script.UpdateIdentity(0, 0);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!_work)
            {
                EditorGUILayout.HelpBox("QNetObject preview work only for objects in scene.", MessageType.Info, true);
                return;
            }

            StringBuilder sb;

            if (_script.Prefab == null || _script.Prefab.PrefabIdentity == 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Predefined QNetObject", EditorStyles.boldLabel);
                    sb = new StringBuilder();
                    sb.AppendLine($"Identity: {Identity}");
                    GUILayout.Label(sb.ToString());
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.Space();
            }

            if (!EditorApplication.isPlaying) return;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("QNetObject", EditorStyles.boldLabel);
                sb = new StringBuilder();
                sb.AppendLine($"Identity:  {_script.ObjectIdentity}");
                sb.AppendLine($"IsClient:  {QNetObjectBehaviour.isClient}");
                sb.AppendLine($"IsServer: {QNetObjectBehaviour.isServer}");
                sb.AppendLine($"IsHost:    {QNetObjectBehaviour.isHost}");
                GUILayout.Label(sb.ToString());
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("QNetOwner", EditorStyles.boldLabel);
                sb = new StringBuilder();
                sb.AppendLine($"Identity:                    {_script.OwnerIdentity}");
                sb.AppendLine($"Owner(LocalMachine): {_script.IsOwner}");
                sb.AppendLine($"OwnedByServer:        {_script.IsOwnedByServer}");
                GUILayout.Label(sb.ToString());
            }
            EditorGUILayout.EndVertical();
        }

        private static short FixPredefinedIdentity(short identity)
        {
            var objectsOnScene = FindObjectsOfType<QNetObjectBehaviour>();
            if (identity == 0)
                identity = (short) Random.Range(short.MinValue, short.MaxValue);
            var b = true;
            while (b)
            {
                var count = objectsOnScene.Count(obj => obj.ObjectIdentity == identity);
                if (count >= 2)
                    identity = (short) Random.Range(short.MinValue, short.MaxValue);
                else
                    b = false;
            }

            return identity;
        }
    }
}