//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.UnityEditor.Configuration;
using JEM.UnityEngine.VersionManagement;
using JEM.UnityEngine.VersionManagement.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Timers;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace JEM.UnityEditor.VersionManagement
{
    internal class JEMBuildWindow : EditorWindow
    {
        /// <summary>
        ///     Patch to file of name of last contributor.
        /// </summary>
        public const string LastContributorFile = "Assets/Resources/Editor/Compilation/Last.txt";

        /// <summary>
        ///     Patch to file of version.
        /// </summary>
        public const string VersionFile = "Assets/Resources/Editor/Version.json";

        private static JEMBuildWindow _activeWindow;

        private static bool _isRebuilding;

        private Vector2 _scrollPosition;

        private AnimBool _drawFlags;
        private string _newFlag;

        private Timer _t;

        /// <summary>
        ///     Patch to file of compilation number.
        /// </summary>
        public static string CompilationNumberFile =>
            $"Assets/Resources/Editor/Compilation/compNum_{JEMBuildEditor.ResolveCurrentContributorName()}.json";

        /// <summary>
        ///     Current build version data.
        /// </summary>
        public static JEMBuildVersion CurrentVersion { get; private set; }

        /// <summary>
        ///     Current compilation data.
        /// </summary>
        public static JEMBuildCompilation CurrentCompilation { get; private set; }

        /// <summary>
        ///     Current compilation number.
        /// </summary>
        public static int CurrentCompilationNumber { get; private set; }

        /// <summary>
        ///     Current build number.
        /// </summary>
        public static int CurrentBuildNumber { get; private set; }

        public static int CurrentSessionTime { get; private set; }

        [MenuItem("JEM/JEM Version Management")]
        public static void ShowWindow()
        {
            _activeWindow = GetWindow<JEMBuildWindow>();
            _activeWindow.UpdateTitle();
            _activeWindow.minSize = new Vector2(250, 240);
            _activeWindow.Show();
        }

        private void UpdateTitle()
        {
            // load JEM editor resources
            JEMEditorResources.Load();

            titleContent = new GUIContent("JEM Build", JEMEditorResources.JEMIconTexture);
        }

        private void OnEnable()
        {
            if (_t == null)
            {
                _t = new Timer(10000);
                _t.Elapsed += (sender, args) =>
                {
                    if (JEMEditorConfiguration.Configuration.UpdateWorkTime)
                        if (CurrentCompilation != null)
                        {
                            CurrentSessionTime += 10;
                            CurrentCompilation.SessionTime += 10;
                            var json = JsonConvert.SerializeObject(CurrentCompilation, Formatting.Indented);
                            File.WriteAllText(CompilationNumberFile, json);
                        }
                };
                _t.Start();
            }

            JEMLogger.Init();

            RegisterAnimations();
            RefreshLocalData();

            JEMEditorConfiguration.Load();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void RegisterAnimations()
        {
            if (_drawFlags == null)
            {
                _drawFlags = new AnimBool(false);
                _drawFlags.valueChanged.AddListener(Repaint);
            }
        }

        /// <summary>
        ///     Loads local data.
        /// </summary>
        public static void RefreshLocalData()
        {
            // load version info
            var versionData = (TextAsset) AssetDatabase.LoadAssetAtPath(VersionFile, typeof(TextAsset));
            if (versionData == null)
            {
                // initialize new version file
                CurrentVersion = new JEMBuildVersion();
                var dir = Path.GetDirectoryName(VersionFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir ?? throw new InvalidOperationException());

                File.WriteAllText(VersionFile, JsonConvert.SerializeObject(CurrentVersion, Formatting.Indented));

                AssetDatabase.Refresh(ImportAssetOptions.Default);
                AssetDatabase.SaveAssets();
                Debug.Log($"File {VersionFile} not exist. New file has been created.");
            }
            else
            {
                CurrentVersion = JsonConvert.DeserializeObject<JEMBuildVersion>(versionData.text);
            }

            if (CurrentVersion == null) Debug.LogError("Unable to resolve current version data.");

            // load compilation file
            var compilationText = !File.Exists(CompilationNumberFile)
                ? null
                : (TextAsset) AssetDatabase.LoadAssetAtPath(CompilationNumberFile, typeof(TextAsset));
            if (compilationText == null)
            {
                // initialize new compilation number file
                CurrentCompilation = new JEMBuildCompilation();
                var dir = Path.GetDirectoryName(CompilationNumberFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir ?? throw new InvalidOperationException());
                File.WriteAllText(CompilationNumberFile,
                    JsonConvert.SerializeObject(CurrentCompilation, Formatting.Indented));
                File.WriteAllText(LastContributorFile, JEMBuildEditor.ResolveCurrentContributorName());

                AssetDatabase.Refresh(ImportAssetOptions.Default);
                AssetDatabase.SaveAssets();
                Debug.Log($"File {CompilationNumberFile} not exist. New file has been created.");
            }
            else
            {
                CurrentCompilation = JsonConvert.DeserializeObject<JEMBuildCompilation>(compilationText.text);
            }

            if (CurrentCompilation == null) Debug.LogError("Unable to resolve current compilation data.");

            // apply compilation number
            CurrentCompilationNumber = JEMBuild.ResolveCurrentCompilationNumber();
            CurrentBuildNumber = JEMBuild.ResolveCurrentBuildNumber();

            CurrentSessionTime = JEMBuild.ResolveCurrentSessionTime();

            // apply bundle version
            if (JEMEditorConfiguration.Configuration?.UpdateBundleVersion ?? false) UpdateBundleVersion();

            JEMBuildFlags.LoadFlags();
            JEMBuildFlags.ApplyFlags();
        }

        /// <summary>
        ///     Updates PlayerSettings.bundleVersion
        /// </summary>
        public static void UpdateBundleVersion()
        {
            PlayerSettings.bundleVersion =
                $"{CurrentVersion?.VersionName ?? "INTERNAL_VERSION_ERROR"} @{CurrentBuildNumber}.{CurrentCompilationNumber}";
        }

        /// <summary>
        ///     Saves data to local file
        /// </summary>
        public static void SaveLocalData()
        {
            // save version info
            if (CurrentVersion != null)
            {
                var json = JsonConvert.SerializeObject(CurrentVersion, Formatting.Indented);
                var dir = Path.GetDirectoryName(VersionFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(VersionFile, json);
            }

            // save compilation info
            if (CurrentCompilation != null)
            {
                var json = JsonConvert.SerializeObject(CurrentCompilation, Formatting.Indented);
                var dir = Path.GetDirectoryName(CompilationNumberFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(CompilationNumberFile, json);
                File.WriteAllText(LastContributorFile, JEMBuildEditor.ResolveCurrentContributorName());
            }

            // save asset database
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            AssetDatabase.SaveAssets();

            // refresh local data
            RefreshLocalData();
        }

        /// <summary>
        /// Increase build number.
        /// </summary>
        public static void IncreaseBuildNumber()
        {
            var compilationText = (TextAsset)AssetDatabase.LoadAssetAtPath(CompilationNumberFile, typeof(TextAsset));
            if (compilationText != null)
                CurrentCompilation = JsonConvert.DeserializeObject<JEMBuildCompilation>(compilationText.text);
            if (CurrentCompilation == null)
            {
                Debug.LogError("Unable to load compilation data to update number.");
                return;
            }

            CurrentCompilation.BuildNumber++;
            CurrentCompilation.BuildTime = DateTime.UtcNow;

            var json = JsonConvert.SerializeObject(CurrentCompilation, Formatting.Indented);
            File.WriteAllText(CompilationNumberFile, json);
            File.WriteAllText(LastContributorFile, JEMBuildEditor.ResolveCurrentContributorName());

            AssetDatabase.Refresh(ImportAssetOptions.Default);
            AssetDatabase.SaveAssets();
            RefreshLocalData();        
        }

        private void Update()
        {
            InternalCheckForCompilation();
        }

        private void OnGUI()
        {
            // check for contributor name
            if (!JEMBuildEditor.IsCurrentContributorNameFileExists())
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox(
                    $"System was unable to resolve username.txt file. Please, create {$@"{Environment.CurrentDirectory}\username.txt"}",
                    MessageType.Error, true);
                GUI.backgroundColor = Color.white;
                return;
            }

            if (CurrentCompilation == null || CurrentVersion == null)
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.HelpBox("Failed to initialize compilation or version resource.", MessageType.Error,
                    true);
                if (GUILayout.Button("Refresh", GUILayout.Height(30))) RefreshLocalData();

                return;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            {
                InternalDrawFlags();
                InternalDrawCompilationNumber();
                EditorGUILayout.Space();
                InternalDrawVersionInfo();
                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginHorizontal(GUILayout.Height(25));
                {
                    if (GUILayout.Button("Save", GUILayout.Height(25))) SaveLocalData();

                    if (GUILayout.Button("R", GUILayout.Width(25), GUILayout.Height(25))) RefreshLocalData();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
                {
                    if (JEMEditorConfiguration.Configuration?.UpdateBundleVersion ?? false)
                    {
                        GUI.enabled = false;
                        GUILayout.Button("Update Bundle Version (auto)", GUILayout.Height(20));
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Update Bundle Version", GUILayout.Height(20))) UpdateBundleVersion();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void InternalDrawFlags()
        {
            if (!(JEMEditorConfiguration.Configuration?.UpdateFlags ?? false))
            {
                return;
            }

            if (_drawFlags.value)
            {
                if (GUILayout.Button("Flags (Hide)"))
                    _drawFlags.value = false;
            }
            else
            {
                if (GUILayout.Button("Flags (Show)"))
                    _drawFlags.value = true;
            }

            if (EditorGUILayout.BeginFadeGroup(_drawFlags.faded))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    for (var index = 0; index < JEMBuildFlags.Flags.Count; index++)
                    {
                        var flag = JEMBuildFlags.Flags[index];
                        if (flag.Equals(default(BuildFlag)))
                            continue;

                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            GUILayout.Label(flag.Name);
                            var enabled = EditorGUILayout.Toggle(flag.Enabled, GUILayout.Width(30));
                            if (enabled != flag.Enabled) JEMBuildFlags.SetFlagActive(flag.Name, enabled);

                            if (GUILayout.Button("X", GUILayout.Width(30))) JEMBuildFlags.RemoveFlag(flag.Name);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    {
                        _newFlag = EditorGUILayout.TextField(_newFlag);
                        if (GUILayout.Button("Add", GUILayout.Width(50)))
                            if (!string.IsNullOrEmpty(_newFlag) && !string.IsNullOrWhiteSpace(_newFlag))
                                if (JEMBuildFlags.AddFlag(_newFlag))
                                    _newFlag = string.Empty;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("Force Reload"))
                    {
                        JEMBuildFlags.SaveFlags();
                        JEMBuildFlags.ApplyFlags();
                    }

                    if (GUILayout.Button("Refresh/Apply"))
                    {
                        JEMBuildFlags.SaveFlags();
                        JEMBuildFlags.LoadFlags();
                        JEMBuildFlags.ApplyFlags();
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            JEMBetterEditor.FixedEndFadeGroup(_drawFlags.faded);
        }

        private void InternalDrawCompilationNumber()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Build: {CurrentBuildNumber}");
            var gameBuildPercentage = (float)CurrentCompilation.BuildNumber / CurrentBuildNumber * 100f;
            sb.AppendLine($"Local Build: {CurrentCompilation.BuildNumber} ({gameBuildPercentage:0.00}%)");
            EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info, true);
            sb.Clear();
            sb.AppendLine();
            sb.AppendLine($"Compilation: {CurrentCompilationNumber}");
            var gameCompilationPercentage = (float)CurrentCompilation.CompilationNumber / CurrentCompilationNumber * 100f;
            sb.AppendLine($"Local Compilation: {CurrentCompilation.CompilationNumber} ({gameCompilationPercentage:0.00}%)");
            EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info, true);
            sb.Clear();
            sb.AppendLine();
            var total = TimeSpan.FromSeconds(CurrentSessionTime);
            sb.AppendLine($"Work time: {total.Days:D2}d:{total.Hours:D2}h:{total.Minutes:D2}m:{total.Seconds:D2}s");
            var local = TimeSpan.FromSeconds(CurrentCompilation.SessionTime);
            var sessionTimePercentage = (float) CurrentCompilation.SessionTime / CurrentSessionTime * 100f;
            sb.AppendLine($"Local Work Time: {local.Days:D2}d:{local.Hours:D2}h:{local.Minutes:D2}m:{local.Seconds:D2}s ({sessionTimePercentage:0.00}%)");
            EditorGUILayout.HelpBox(sb.ToString(), MessageType.Info, true);
        }

        private void InternalDrawVersionInfo()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.PrefixLabel("Build:");
                CurrentVersion.VersionName = EditorGUILayout.TextField(CurrentVersion.VersionName);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    var timeStyle = new GUIStyle(EditorStyles.helpBox) {alignment = TextAnchor.MiddleCenter};
                    GUILayout.Box($"{CurrentVersion.VersionRelease:yyyy-MM-dd HH:mm:ss}", timeStyle,
                        GUILayout.Height(39));
                    if (GUILayout.Button("Update", GUILayout.Height(39), GUILayout.Width(50)))
                        CurrentVersion.VersionRelease = DateTime.UtcNow;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void InternalCheckForCompilation()
        {
            if (_isRebuilding != EditorApplication.isCompiling)
            {
                if (!_isRebuilding && EditorApplication.isCompiling)
                {
                    var compilationText =
                        (TextAsset) AssetDatabase.LoadAssetAtPath(CompilationNumberFile, typeof(TextAsset));
                    if (compilationText != null)
                        CurrentCompilation = JsonConvert.DeserializeObject<JEMBuildCompilation>(compilationText.text);
                    if (CurrentCompilation == null)
                    {
                        Debug.LogError("Unable to load compilation data to update number.");
                        return;
                    }

                    CurrentCompilation.CompilationNumber++;
                    CurrentCompilation.CompilationTime = DateTime.UtcNow;

                    var json = JsonConvert.SerializeObject(CurrentCompilation, Formatting.Indented);
                    File.WriteAllText(CompilationNumberFile, json);
                    File.WriteAllText(LastContributorFile, JEMBuildEditor.ResolveCurrentContributorName());

                    AssetDatabase.Refresh(ImportAssetOptions.Default);
                    AssetDatabase.SaveAssets();
                    RefreshLocalData();
                }

                _isRebuilding = EditorApplication.isCompiling;
            }

            Repaint();
        }
    }
}