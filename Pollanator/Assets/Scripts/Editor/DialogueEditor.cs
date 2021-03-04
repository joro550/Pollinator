using System;
using Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueEditor : EditorWindow
    {
        [SerializeField] private DialogueNode creatingNode;
        
        [NonSerialized] private GUIStyle _nodeStyle;
        [NonSerialized] private bool _draggingCanvas;
        [NonSerialized] private Vector2 _draggingOffset;
        [NonSerialized] private GUIStyle _playerNodeStyle;
        [NonSerialized] private Vector2 _draggingCanvasOffset;
        [NonSerialized] private DialogueNode _draggingNode;
        [NonSerialized] private DialogueNode _deletingNode;
        [NonSerialized] private DialogueNode _linkingParentNode;

        private Toolbar _toolbar;
        private Vector2 _scrollPosition;
        private Dialogue.Dialogue _selectedDialogue;

        private const float CanvasSize = 4000;
        private const float BackgroundSize = 50;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow() 
            => GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID)
                as Dialogue.Dialogue;
            if (dialogue == null) 
                return false;
            
            ShowEditorWindow();
            return true;
        }

        private void OnEnable()
        {
            
            Selection.selectionChanged += OnSelectionChanged;

            var firstNode = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle = new GUIStyle
            {
                normal = {background = firstNode, textColor = Color.white},
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };

            var secondNode = EditorGUIUtility.Load("node1");
            _playerNodeStyle = new GUIStyle
            {
                normal = {background = secondNode as Texture2D, textColor = Color.white},
                padding = new RectOffset(20, 20, 20, 20),
                border = new RectOffset(12, 12, 12, 12)
            };
        }

        private void OnSelectionChanged()
        {
            var newDialogue = Selection.activeObject as Dialogue.Dialogue;

            if (newDialogue == null) 
                return;
            
            if(_toolbar != null)
                rootVisualElement.Remove(_toolbar);
                
            _selectedDialogue = newDialogue;
            Repaint();
                
            _toolbar = new Toolbar();
            var button = new Button(() => _selectedDialogue.CreateNode(null)) 
            {
                text =  "Create Node",
            };
            _toolbar.Add(button);
            rootVisualElement.Add(_toolbar);
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                ProcessEvents();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                var canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                var backgroundTex = Resources.Load("background") as Texture2D;
                var texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }

                foreach (var node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    _selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if (_deletingNode != null)
                {
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (Event.current.type)
            {
                case EventType.MouseDown when _draggingNode == null:
                {
                    _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
                    if (_draggingNode != null)
                    {
                        _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
                        Selection.activeObject = _draggingNode;
                    }
                    else
                    {
                        _draggingCanvas = true;
                        _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                        Selection.activeObject = _selectedDialogue;
                    }

                    break;
                }
                case EventType.MouseDrag when _draggingNode != null:
                    _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
                    GUI.changed = true;
                    break;
                case EventType.MouseDrag when _draggingCanvas:
                    _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;

                    GUI.changed = true;
                    break;
                case EventType.MouseUp when _draggingNode != null:
                    _draggingNode = null;
                    break;
                case EventType.MouseUp when _draggingCanvas:
                    _draggingCanvas = false;
                    break;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            var style = _nodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = _playerNodeStyle;
            }

            GUILayout.BeginArea(node.GetRect(), style);
            EditorGUI.BeginChangeCheck();

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();
            node.SetPlayerIsSpeaking(GUILayout.Toggle(node.IsPlayerSpeaking(), "Player Speaking"));
            
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x"))
            {
                _deletingNode = node;
            }

            DrawLinkButtons(node);
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    _linkingParentNode = node;
                }
            }
            else if (_linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.GetChildren().Contains(node.name))
            {
                if (!GUILayout.Button("unlink")) 
                    return;
                
                _linkingParentNode.RemoveChild(node.name);
                _linkingParentNode = null;
            }
            else
            {
                if (!GUILayout.Button("child")) 
                    return;
                
                Undo.RecordObject(_selectedDialogue, "Add Dialogue Link");
                _linkingParentNode.AddChild(node.name);
                _linkingParentNode = null;
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            var rect = node.GetRect();
            Vector3 startPosition = new Vector2(rect.xMax, rect.center.y);
            
            foreach (var childNode in _selectedDialogue.GetAllChildren(node))
            {
                var childRect = childNode.GetRect();
                Vector3 endPosition = new Vector2(childRect.xMin, childRect.center.y);
                
                var controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                
                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (var node in _selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }

            return foundNode;
        }
    }
}