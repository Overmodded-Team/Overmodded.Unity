//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.Graph
{
    /// <inheritdoc />
    /// <summary>
    ///     Node graph inspector.
    /// </summary>
    public class JEMNodeGraphInspector : EditorWindow
    {
        private Vector2 _inspectorScrollPosition;

        protected Vector2 ConnectionScrollPosition;

        /// <summary>
        ///     Node target.
        /// </summary>
        public JEMNodeGraph.Node NodeTarget { get; private set; }

        /// <summary>
        ///     Current instance of node graph.
        /// </summary>
        public static JEMNodeGraphInspector Instance { get; protected set; }

        internal static void InternalShowDefaultWindow()
        {
            if (Instance != null)
            {
                Instance.Focus();
                return;
            }

            Instance = (JEMNodeGraphInspector) GetWindow(typeof(JEMNodeGraphInspector), false, "Node Inspector");
        }

        /// <inheritdoc />
        protected virtual void OnEnable()
        {
        }

        /// <inheritdoc />
        protected virtual void OnGUI()
        {
            if (NodeTarget == null)
                return;

            _inspectorScrollPosition = EditorGUILayout.BeginScrollView(_inspectorScrollPosition);
            {
                // draw toolbar
                EditorGUILayout.BeginVertical();
                {
                    DrawToolbar();
                }
                EditorGUILayout.EndVertical();

                // draw node content
                EditorGUILayout.BeginVertical();
                {
                    DrawNodeContent();
                }
                EditorGUILayout.EndVertical();

                // draw node connections
                EditorGUILayout.BeginVertical();
                {
                    DrawNodeConnections();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        ///     Draws toolbar of inspector.
        /// </summary>
        protected virtual void DrawToolbar()
        {
            var nodeTargetName = NodeTarget.Name;
            var nodeTargetRect = NodeTarget.Rect;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                nodeTargetName = EditorGUILayout.TextField(nodeTargetName);
                if (GUILayout.Button("Delete", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    var @continue =
                        EditorUtility.DisplayDialog("Delete?", "Do you want to delete this node?", "Yes", "No..");
                    if (!@continue)
                        return;

                    UnloadInspectorTarget(NodeTarget);
                    NodeTarget.Destroy();
                }

                if (GUILayout.Button("Duplicate", EditorStyles.toolbarButton, GUILayout.Width(60)))
                    NodeTarget.Graph.DuplicateMode(NodeTarget);
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = false;
            GUI.color = NodeTarget.BackgroundColor;
            GUILayout.Button("", GUILayout.Height(30));
            GUI.color = Color.white;
            GUI.enabled = true;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                var boxStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    stretchWidth = true,
                    stretchHeight = false,
                    alignment = TextAnchor.MiddleCenter
                };
                GUILayout.Box("Node Settings", boxStyle);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                nodeTargetRect.position = EditorGUILayout.Vector2Field("Position", nodeTargetRect.position);
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(this, "NodeGraphInspector toolbar change.");

                NodeTarget.Name = nodeTargetName;
                NodeTarget.Rect = nodeTargetRect;
                NodeTarget.Graph.Repaint();
            }
        }

        /// <summary>
        ///     Draws actual node content.
        /// </summary>
        protected virtual void DrawNodeContent()
        {
        }

        /// <summary>
        ///     Draws list of node connections.
        /// </summary>
        protected virtual void DrawNodeConnections()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(300));
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    var boxStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        stretchWidth = true,
                        stretchHeight = false,
                        alignment = TextAnchor.MiddleCenter
                    };
                    GUILayout.Box("Node Connections", boxStyle);
                }
                EditorGUILayout.EndVertical();

                ConnectionScrollPosition = EditorGUILayout.BeginScrollView(ConnectionScrollPosition);
                {
                    if (NodeTarget.OutgoingNodes.Count != 0)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            var boxStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                            {
                                stretchWidth = true,
                                stretchHeight = false,
                                alignment = TextAnchor.MiddleCenter
                            };
                            GUILayout.Box("Outgoing Nodes", boxStyle);
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUI.indentLevel++;
                        for (var index1 = 0; index1 < NodeTarget.OutgoingNodes.Count; index1++)
                        {
                            var outgoingNode = NodeTarget.OutgoingNodes[index1];
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            {
                                GUILayout.Label($"(Out.) this -> {outgoingNode.Name}", EditorStyles.miniLabel);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20))) outgoingNode.Disconnect(NodeTarget);

                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUI.indentLevel--;
                    }

                    if (NodeTarget.IncomingNodes.Count != 0)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            var boxStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                            {
                                stretchWidth = true,
                                stretchHeight = false,
                                alignment = TextAnchor.MiddleCenter
                            };
                            GUILayout.Box("Incoming Nodes", boxStyle);
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUI.indentLevel++;
                        for (var index1 = 0; index1 < NodeTarget.IncomingNodes.Count; index1++)
                        {
                            var incomingNode = NodeTarget.IncomingNodes[index1];
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            {
                                GUILayout.Label($"(In.) {incomingNode.Name} -> this", EditorStyles.miniLabel);
                                GUI.color = Color.red;
                                if (GUILayout.Button("X", GUILayout.Width(20))) NodeTarget.Disconnect(incomingNode);

                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUI.indentLevel--;
                    }

                    if (NodeTarget.OutgoingNodes.Count == 0 && NodeTarget.IncomingNodes.Count == 0)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            var boxStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                            {
                                stretchWidth = true,
                                stretchHeight = false,
                                alignment = TextAnchor.MiddleCenter
                            };
                            GUILayout.Box("No nodes connected", boxStyle);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        ///     Sets inspector target.
        /// </summary>
        public virtual void SetInspectorTarget(JEMNodeGraph.Node node)
        {
            if (NodeTarget != null && node == NodeTarget)
            {
                LoadInspectorTarget(node);
                return;
            }

            if (NodeTarget != null) UnloadInspectorTarget(NodeTarget);

            NodeTarget = node;
            if (NodeTarget != null) LoadInspectorTarget(node);
        }

        private void OnNodePositionChanged(JEMNodeGraph.Node thisNode, Vector2 oldPosition, Vector2 newPosition)
        {
            Repaint();
        }

        private void OnIncomingNodesChanged(JEMNodeGraph.Node thisNode, IReadOnlyList<JEMNodeGraph.Node> lastIncomingNodes,
            IReadOnlyList<JEMNodeGraph.Node> newIncomingNodes)
        {
            Repaint();
        }

        private void OnOutgoingNodesChanged(JEMNodeGraph.Node thisNode, IReadOnlyList<JEMNodeGraph.Node> lastOutgoingNodes,
            IReadOnlyList<JEMNodeGraph.Node> newOutgoingNodes)
        {
            Repaint();
        }

        /// <summary>
        ///     Sets node as inspector target.
        /// </summary>
        public virtual void LoadInspectorTarget(JEMNodeGraph.Node node)
        {
            node.OnNodePositionChanged += OnNodePositionChanged;
            node.OnOutgoingNodesChanged += OnOutgoingNodesChanged;
            node.OnIncomingNodesChanged += OnIncomingNodesChanged;
            Repaint();
        }

        /// <summary>
        ///     Uploads given node.
        /// </summary>
        public virtual void UnloadInspectorTarget(JEMNodeGraph.Node node)
        {
            node.OnNodePositionChanged -= OnNodePositionChanged;
            node.OnOutgoingNodesChanged -= OnOutgoingNodesChanged;
            node.OnIncomingNodesChanged -= OnIncomingNodesChanged;
            Repaint();
        }
    }
}