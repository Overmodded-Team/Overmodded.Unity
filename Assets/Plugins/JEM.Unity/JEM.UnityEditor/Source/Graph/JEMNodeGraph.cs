//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.Graph
{
    /// <inheritdoc />
    /// <summary>
    ///     Node graph window.
    /// </summary>
    public abstract class JEMNodeGraph : EditorWindow
    {
        /// <summary>
        ///     Node change event.
        /// </summary>
        public delegate void NodeChanged(Node node);

        /// <summary>
        ///     Node duplicate event.
        /// </summary>
        public delegate void NodeDuplicate(Node newNode, Node sourceNode);

        private Vector2 _editMenuWorkspaceSize;
        internal Node CurrentNode;
        internal Vector2 LastMousePos = new Vector2(-999f, -999f);

        /// <summary>
        ///     Node add event.
        /// </summary>
        public NodeChanged OnNodeAdded;

        /// <summary>
        ///     Node change event.
        /// </summary>
        public NodeChanged OnNodeChanged;

        /// <summary>
        ///     Node duplicate event.
        /// </summary>
        public NodeDuplicate OnNodeDuplicate;

        /// <summary>
        ///     Node remove event.
        /// </summary>
        public NodeChanged OnNodeRemoved;

        internal Vector2 ViewPosition;

        /// <summary>
        ///     Defines size of node graph workspace.
        /// </summary>
        public Vector2 WorkspaceSize { get; private set; } = new Vector2(1000, 1000);

        /// <summary>
        ///     Defines size of single node.
        /// </summary>
        public Vector2 DefaultNodeSize { get; set; } = new Vector2(200, 100);

        public NodePosition DefaultNodePosition = NodePosition.Horizontal;

        /// <summary>
        ///     List of all nodes in graph.
        /// </summary>
        public IReadOnlyList<Node> ListOfAllNodes => AllNodes;

        internal List<Node> AllNodes { get; } = new List<Node>();

        private void Update()
        {
            if (CurrentNode != null) Repaint();
        }

        /// <inheritdoc cref="EditorWindow" />
        protected virtual void OnGUI()
        {
            const float toolBarHeight = 20;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                DrawToolbar();
            }
            GUILayout.EndHorizontal();

            var scrollViewRect = new Rect(0f, toolBarHeight, position.width, position.height - toolBarHeight);

            ViewPosition = GUI.BeginScrollView(scrollViewRect, ViewPosition,
                new Rect(0, 0, WorkspaceSize.x, WorkspaceSize.y), false, false);
            if (CurrentNode != null)
            {
                if (Event.current.button == 1
                ) // editor errors -> || !scrollViewRect.Contains(Event.current.mousePosition))
                {
                    CurrentNode = null;
                    Repaint();
                    return;
                }

                DrawNodeCurve(CurrentNode.GetOutgoingPoint(), Event.current.mousePosition);
            }

            foreach (var node in AllNodes)
            foreach (var incomingNode in node.IncomingNodes)
                DrawNodeCurve(incomingNode.GetOutgoingPoint(), node.GetIncomingPoint());

            BeginWindows();
            for (var index = 0; index < AllNodes.Count; index++)
            {
                var node = AllNodes[index];
                var nodeRect = node.Rect;
                if (JEMNodeGraphInspector.Instance != null && JEMNodeGraphInspector.Instance.NodeTarget == node)
                {
                    GUI.color = Color.cyan;
                    GUI.Box(new Rect(nodeRect.x - 5, nodeRect.y - 10, nodeRect.width + 10, nodeRect.height + 20),
                        string.Empty, EditorStyles.helpBox);
                    GUI.color = Color.white;
                }

                GUI.color = node.BackgroundColor;
                GUI.Box(new Rect(nodeRect.x - 2.5f, nodeRect.y - 5, nodeRect.width + 5, nodeRect.height + 10),
                    string.Empty, EditorStyles.helpBox);
                GUI.color = Color.white;
                nodeRect = GUI.Window(index + 1, nodeRect, i =>
                {
                    var e = Event.current;
                    if (e.type == EventType.MouseDown && e.button == 0) SelectAndDrawNodeInspector(node);

                    DrawNodeWindowContent(node);
                    GUILayout.FlexibleSpace();

                    GUI.DragWindow();
                }, node.Name);
                node.Rect = nodeRect;

                var pointSize = new Vector2(20, 20);
                Vector2 incomingPoint;
                Vector2 outgoingPoint;
                string incomingChar;
                string outgoingChar;
                switch (DefaultNodePosition)
                {
                    case NodePosition.Horizontal:
                        incomingPoint = node.GetIncomingPoint() + new Vector2(-pointSize.x, -pointSize.y / 2f);
                        outgoingPoint = node.GetOutgoingPoint() + new Vector2(0, -pointSize.y / 2f);
                        incomingChar = "+";
                        outgoingChar = ">";
                        break;
                    case NodePosition.Vertical:
                        incomingPoint = node.GetIncomingPoint() + new Vector2(-pointSize.x / 2f, -pointSize.y);
                        outgoingPoint = node.GetOutgoingPoint() + new Vector2(-pointSize.x / 2f, 0f);
                        incomingChar = "+";
                        outgoingChar = @"\/";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (GUI.Button(new Rect(incomingPoint, pointSize), incomingChar))
                {
                    if (CurrentNode != null)
                    {
                        node.Connect(CurrentNode);
                        CurrentNode = null;
                    }
                }


                if (GUI.Button(new Rect(outgoingPoint, pointSize), outgoingChar))
                {
                    CurrentNode = node;
                }
            }

            EndWindows();

            if (Event.current.button == 1 && Event.current.type == EventType.MouseDown)
            {
                var menu = new GenericMenu();
                ResolveContextMenu(menu);
                menu.ShowAsContext();
            }
            else if (Event.current.button == 0 && Event.current.type == EventType.MouseDown ||
                     Event.current.keyCode == KeyCode.Escape && Event.current.type == EventType.KeyDown)
            {
                if (CurrentNode != null)
                {
                    CurrentNode = null;
                    Repaint();
                }

                SelectAndDrawNodeInspector(null);
            }

            GUI.EndScrollView();

            if (scrollViewRect.Contains(Event.current.mousePosition))
                if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag)
                {
                    var currentPos = Event.current.mousePosition;
                    if (Vector2.Distance(currentPos, LastMousePos) < 50)
                    {
                        var x = LastMousePos.x - currentPos.x;
                        var y = LastMousePos.y - currentPos.y;
                        ViewPosition.x += x;
                        ViewPosition.y += y;
                        Event.current.Use();
                    }

                    LastMousePos = currentPos;
                }
        }

        /// <summary>
        ///     Draws toolbar content.
        /// </summary>
        protected virtual void DrawToolbar()
        {
            if (GUILayout.Button("File", EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                var menu = new GenericMenu();
                ResolveFileMenu(menu);
                menu.ShowAsContext();
            }

            if (GUILayout.Button("Edit", EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                var menu = new GenericMenu();
                ResolveEditMenu(menu);
                menu.ShowAsContext();
            }
        }

        /// <summary>
        ///     Resolve `File` menu data.
        /// </summary>
        protected virtual void ResolveFileMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Save"), false, () => { SaveGraph(); });
        }

        /// <summary>
        ///     Resolve  `Edit` menu data.
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void ResolveEditMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Workspace/Size"), false, () =>
            {
                _editMenuWorkspaceSize = WorkspaceSize;
                JEMToolWindow.ShowWindow("Workspace Size", () =>
                {
                    _editMenuWorkspaceSize = EditorGUILayout.Vector2Field("Size", _editMenuWorkspaceSize);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Apply", GUILayout.Height(30)))
                    {
                        UpdateNodeGraphWorkspace(_editMenuWorkspaceSize);
                        JEMToolWindow.CloseWindow();
                    }
                });
            });
        }

        /// <summary>
        ///     Resolves context menu data.
        /// </summary>
        protected virtual void ResolveContextMenu(GenericMenu menu)
        {
            var mousePosition = Event.current.mousePosition;
            menu.AddItem(new GUIContent("Add Node"), false, () =>
            {
                AddNode(mousePosition);
                Repaint();
            });
        }

        /// <summary>
        ///     Draws content of node window.
        /// </summary>
        protected abstract void DrawNodeWindowContent(Node node);

        /// <summary>
        ///     Select and draw node inspector.
        /// </summary>
        /// <param name="node"></param>
        protected virtual void SelectAndDrawNodeInspector(Node node)
        {
            if (JEMNodeGraphInspector.Instance == null) JEMNodeGraphInspector.InternalShowDefaultWindow();

            if (JEMNodeGraphInspector.Instance != null) JEMNodeGraphInspector.Instance.SetInspectorTarget(node);

            Repaint();
        }

        /// <summary>
        ///     Clears all nodes without any event call.
        /// </summary>
        protected void ClearAllNodesSilently(bool clearCallbacks = false)
        {
            AllNodes.Clear();
            if (clearCallbacks)
            {
                OnNodeChanged = null;
                OnNodeAdded = null;
                OnNodeRemoved = null;
            }
        }

        /// <summary>
        ///     Saves graph data.
        /// </summary>
        protected virtual void SaveGraph()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        ///     Adds new node.
        /// </summary>
        protected Node AddNode(bool autoPositionFix = true)
        {
            return AddNode(new Vector2(), autoPositionFix);
        }

        /// <summary>
        ///     Adds new node.
        /// </summary>
        protected Node AddNode(Vector2 nodePosition, bool autoPositionFix = true)
        {
            return AddNode(nodePosition, new Vector2(), autoPositionFix);
        }

        /// <summary>
        ///     Adds new node.
        /// </summary>
        protected Node AddNode(Vector2 nodePosition, Vector2 nodeSize, bool autoPositionFix = true)
        {
            var nodeRect = new Rect(nodePosition == default(Vector2) ? new Vector2(10, 10) : nodePosition,
                nodeSize == default(Vector2) ? DefaultNodeSize : nodeSize);
            if (AllNodes.Count != 0)
            {
                var last = AllNodes[AllNodes.Count - 1];
                if (nodePosition == default(Vector2) ||
                    Vector2.Distance(last.Rect.position, nodePosition) < 15f && autoPositionFix)
                    nodeRect = new Rect(last.Rect.x + nodeRect.width / 2f, last.Rect.y + nodeRect.height / 2f,
                        nodeRect.width, nodeRect.height);
            }

            var node = new Node(this, nodeRect)
            {
                Name = $"Node #{AllNodes.Count}",
                Index = AllNodes.Count
            };
            AllNodes.Add(node);
            OnNodeAdded?.Invoke(node);
            return node;
        }

        /// <summary>
        ///     Duplicate given node.
        /// </summary>
        internal Node DuplicateMode(Node node)
        {
            var newNode = AddNode(node.Rect.position, node.Rect.size);
            OnNodeDuplicate?.Invoke(newNode, node);
            return newNode;
        }

        /// <summary>
        ///     Updates size of node graph workspace.
        /// </summary>
        protected virtual void UpdateNodeGraphWorkspace(Vector2 size)
        {
            WorkspaceSize = size;
            Repaint();
        }

        /// <summary>
        ///     Updates index in all nodes.
        /// </summary>
        private void UpdateIndexOfNodes()
        {
            var postChange = new List<Node>();
            for (var index = 0; index < AllNodes.Count; index++)
            {
                var node = AllNodes[index];
                if (node.Index == index) continue;
                node.Index = index;
                if (!postChange.Contains(node)) postChange.Add(node);

                foreach (var outgoing in node.OutgoingNodes)
                    if (!postChange.Contains(outgoing))
                        postChange.Add(outgoing);

                foreach (var incoming in node.IncomingNodes)
                    if (!postChange.Contains(incoming))
                        postChange.Add(incoming);
            }

            foreach (var node in postChange) OnNodeChanged?.Invoke(node);
        }

        private static void DrawNodeCurve(Vector2 start, Vector2 end)
        {
            var startPos = new Vector3(start.x, start.y, 0);
            var endPos = new Vector3(end.x, end.y, 0);
            var startTan = startPos + Vector3.right * 50;
            var endTan = endPos + Vector3.left * 50;
            var shadowCol = new Color(0, 0, 0, 0.06f);
            for (var i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }

        public enum NodePosition
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        ///     Node.
        /// </summary>
        public class Node
        {
            /// <summary>
            ///     Incoming nodes change event.
            /// </summary>
            public delegate void IncomingNodesChanged(Node thisNode, IReadOnlyList<Node> lastIncomingNodes,
                IReadOnlyList<Node> newIncomingModes);

            /// <summary>
            ///     Node position change.
            /// </summary>
            public delegate void NodePositionChanged(Node thisNode, Vector2 oldPosition, Vector2 newPosition);

            /// <summary>
            ///     Outgoing nodes change event.
            /// </summary>
            public delegate void OutgoingNodesChanged(Node thisNode, IReadOnlyList<Node> lastOutgoingNodes,
                IReadOnlyList<Node> newOutgoingModes);

            private Rect _rect;

            /// <summary>
            ///     Color of node window.
            /// </summary>
            public Color BackgroundColor = Color.white;

            /// <summary>
            ///     Incoming point offset.
            /// </summary>
            public Vector2 IncomingPoint;

            /// <summary>
            ///     Name of node.
            /// </summary>
            public string Name;

            /// <summary>
            ///     Incoming nodes change event.
            /// </summary>
            public IncomingNodesChanged OnIncomingNodesChanged;

            /// <summary>
            ///     Node position change event.
            /// </summary>
            public NodePositionChanged OnNodePositionChanged;

            /// <summary>
            ///     Outgoing nodes change event.
            /// </summary>
            public OutgoingNodesChanged OnOutgoingNodesChanged;

            /// <summary>
            ///     Outgoing point offset.
            /// </summary>
            public Vector2 OutgoingPoint;


            /// <summary>
            ///     Constructor.
            /// </summary>
            public Node(JEMNodeGraph graph, Rect rect)
            {
                Graph = graph;
                _rect = rect;
                UpdatePointsPosition();
            }

            /// <summary>
            ///     Rect of node.
            /// </summary>
            public Rect Rect
            {
                get => _rect;
                set
                {
                    var oldRest = _rect;
                    _rect = value;
                    UpdatePointsPosition();
                    if (oldRest != _rect) OnNodePositionChanged?.Invoke(this, oldRest.position, _rect.position);
                }
            }

            private void UpdatePointsPosition()
            {
                switch (Graph.DefaultNodePosition)
                {
                    case NodePosition.Horizontal:
                        OutgoingPoint = new Vector2(_rect.width, _rect.height / 2f);
                        IncomingPoint = new Vector2(0, _rect.height / 2f);
                        break;
                    case NodePosition.Vertical:
                        OutgoingPoint = new Vector2(_rect.width / 2f, _rect.height);
                        IncomingPoint = new Vector2(_rect.width / 2f, 0f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            /// <summary>
            ///     Index of this node.
            /// </summary>
            public int Index { get; internal set; }

            /// <summary>
            ///     List of nodes that connection was created by this node.
            /// </summary>
            public List<Node> OutgoingNodes { get; } = new List<Node>();

            /// <summary>
            ///     List of nodes that connection was created not by this node.
            /// </summary>
            public List<Node> IncomingNodes { get; } = new List<Node>();

            /// <summary>
            ///     Source node graph.
            /// </summary>
            public JEMNodeGraph Graph { get; }

            /// <summary>
            ///     Destroys this node.
            /// </summary>
            public void Destroy()
            {
                Graph.OnNodeRemoved?.Invoke(this);
                Graph.AllNodes.Remove(this);
                Graph.UpdateIndexOfNodes();

                foreach (var node in OutgoingNodes)
                {
                    if (!node.IncomingNodes.Contains(this)) continue;
                    node.IncomingNodes.Remove(this);
                    Graph.OnNodeChanged?.Invoke(node);
                }

                foreach (var node in IncomingNodes)
                {
                    if (!node.OutgoingNodes.Contains(this)) continue;
                    node.OutgoingNodes.Remove(this);
                    Graph.OnNodeChanged?.Invoke(node);
                }
            }

            /// <summary>
            ///     Connects given node with this node.
            /// </summary>
            public void Connect(Node node)
            {
                if (IncomingNodes.Contains(node)) return;
                if (OutgoingNodes.Contains(node)) return;
                if (node == this) return;

                var lastIncomingNodes = new List<Node>(IncomingNodes);
                IncomingNodes.Add(node);
                OnIncomingNodesChanged?.Invoke(this, lastIncomingNodes, IncomingNodes);

                var lastOutgoingNodes = new List<Node>(node.OutgoingNodes);
                node.OutgoingNodes.Add(this);
                OnOutgoingNodesChanged?.Invoke(this, lastOutgoingNodes, OutgoingNodes);

                Graph.OnNodeChanged?.Invoke(this);
                Graph.OnNodeChanged?.Invoke(node);
            }

            /// <summary>
            ///     Disconnects given node from this node.
            /// </summary>
            public void Disconnect(Node node)
            {
                if (!IncomingNodes.Contains(node)) return;

                var lastIncomingNodes = new List<Node>(IncomingNodes);
                IncomingNodes.Remove(node);
                OnIncomingNodesChanged?.Invoke(this, lastIncomingNodes, IncomingNodes);

                var lastOutgoingNodes = new List<Node>(node.OutgoingNodes);
                node.OutgoingNodes.Remove(this);
                OnOutgoingNodesChanged?.Invoke(this, lastOutgoingNodes, OutgoingNodes);

                Graph.OnNodeChanged?.Invoke(this);
                Graph.OnNodeChanged?.Invoke(node);
            }

            /// <summary>
            ///     Returns array of incoming names.
            /// </summary>
            /// <returns></returns>
            public string[] GetArrayOfIncomingNames()
            {
                var names = new string[IncomingNodes.Count];
                for (var index = 0; index < names.Length; index++) names[index] = IncomingNodes[index].Name;

                return names;
            }

            internal Vector2 GetOutgoingPoint()
            {
                return Rect.position + OutgoingPoint;
            }

            internal Vector2 GetIncomingPoint()
            {
                return Rect.position + IncomingPoint;
            }
        }
    }
}