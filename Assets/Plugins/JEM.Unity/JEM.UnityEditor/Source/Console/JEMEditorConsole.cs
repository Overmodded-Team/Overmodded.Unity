//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor.Console
{
    /// <summary>
    ///     JEM Console source filters.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum JEMConsoleSourceFilters
    {
        /// <summary>
        ///     All sources.
        /// </summary>
        ALL,

        /// <summary>
        ///     Internal sources.
        /// </summary>
        INTERNAL,

        /// <summary>
        ///     External sources.
        /// </summary>
        EXTERNAL
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class JEMEditorConsole : EditorWindow
    {
        private struct ConsoleItem
        {
            public string Text;
            public string StackTrace;
            public string Source;
        }

        private void UpdateTitle()
        {
            // load JEM editor resources
            JEMEditorResources.Load();

            titleContent = new GUIContent("JEM Console", JEMEditorResources.JEMIconTexture);
        }

        private void OnEnable()
        {
            UpdateTitle();

            JEMLogger.OnLogAppended += OnLogAppended;
            // JEMLogger.InternalLog("Hello, JEM Console!");
        }

        private void OnDisable()
        {
            JEMLogger.OnLogAppended -= OnLogAppended;
        }

        private void OnLogAppended(string source, JEMLogType type, string value, string stackTrace)
        {
            switch (type)
            {
                case JEMLogType.Log:
                    consoleText.Add(new ConsoleItem
                    {
                        Text = $"[LOG][{source}]: <color=white>{value}</color>",
                        StackTrace = stackTrace,
                        Source = source
                    });
                    break;
                case JEMLogType.Warning:
                    consoleText.Add(new ConsoleItem
                    {
                        Text = $"[WARNING][{source}]: <color=yellow>{value}</color>",
                        StackTrace = stackTrace,
                        Source = source
                    });
                    break;
                case JEMLogType.Error:
                    consoleText.Add(new ConsoleItem
                    {
                        Text = $"[ERROR][{source}]: <color=red>{value}</color>",
                        StackTrace = stackTrace,
                        Source = source
                    });
                    break;
                case JEMLogType.Exception:
                    consoleText.Add(new ConsoleItem
                    {
                        Text = $"[EXCEPTION][{source}]: <color=orange>{value}</color>",
                        StackTrace = stackTrace,
                        Source = source
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    _selectedIndex = -1;
                    consoleText.Clear();
                }

                _consoleFilters = (JEMConsoleSourceFilters) EditorGUILayout.EnumPopup(_consoleFilters,
                    EditorStyles.toolbarPopup, GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                _search = EditorGUILayout.TextArea(_search, EditorStyles.toolbarTextField, GUILayout.Width(150));
            }
            EditorGUILayout.EndHorizontal();
            _scrollPosition1 = EditorGUILayout.BeginScrollView(_scrollPosition1, EditorStyles.helpBox);
            {
                var style = new GUIStyle(EditorStyles.label) {richText = true};
                for (var index = 0; index < consoleText.Count; index++)
                {
                    if (!string.IsNullOrEmpty(_search) && !consoleText[index].Text.Contains(_search))
                        continue;

                    switch (_consoleFilters)
                    {
                        case JEMConsoleSourceFilters.ALL:
                            break;
                        case JEMConsoleSourceFilters.INTERNAL:
                            if (consoleText[index].Source != "INTERNAL")
                                continue;
                            break;
                        case JEMConsoleSourceFilters.EXTERNAL:
                            if (consoleText[index].Source != "EXTERNAL")
                                continue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (GUILayout.Button(consoleText[index].Text, style))
                        _selectedIndex = index;
                }
            }
            EditorGUILayout.EndScrollView();
            _scrollPosition2 = EditorGUILayout.BeginScrollView(_scrollPosition2, EditorStyles.helpBox);
            {
                if (_selectedIndex != -1 && _selectedIndex <= consoleText.Count - 1)
                {
                    var item = consoleText[_selectedIndex];
                    var text = $"Full Trace:{item.StackTrace}";
                    var style = new GUIStyle(EditorStyles.label) {richText = true};
                    if (GUILayout.Button(text, style))
                    {
                        if (Time.time - _selectedLastClick < .3f)
                        {
                            //TODO
                            //InternalEditorUtility.OpenFileAtLineExternal(trace.CallingFilePath, trace.CallingFileLineNumber);
                        }

                        _selectedLastClick = Time.time;
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        ///     Show window.
        /// </summary>
        [MenuItem("Tools/JEM/Console")]
        public static void ShowWindow()
        {
            _activeWindow = GetWindow(typeof(JEMEditorConsole)) as JEMEditorConsole;
            if (_activeWindow == null) return;
            _activeWindow.UpdateTitle();
            _activeWindow.Show();
        }

        private static JEMEditorConsole _activeWindow;
        private static Vector2 _scrollPosition1;
        private static Vector2 _scrollPosition2;
        private static int _selectedIndex;
        private static float _selectedLastClick;
        private static JEMConsoleSourceFilters _consoleFilters = JEMConsoleSourceFilters.ALL;
        private static string _search;

        private static List<ConsoleItem> consoleText { get; } = new List<ConsoleItem>();
    }
}