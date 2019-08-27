//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using NLua;
using SimpleLUI.API;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleLUI
{
    /// <summary>
    ///     A SLUI file.
    /// </summary>
    public class SLUIFile
    {
        /// <summary>
        ///     File path.
        /// </summary>
        public string File { get; }

        /// <summary>
        ///     Last file modification date.
        /// </summary>
        public DateTime LastModified { get; internal set; }

        internal SLUIFile(string file)
        {
            File = file;
        }
    }

    /// <summary>
    ///     Main SLUI manager class.
    /// </summary>
    public class SLUIManager
    {
        /// <summary/>
        public static readonly List<KeyValuePair<string, string>> AllowedNamespaces = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("SimpleLUI", "SimpleLUI.API.Core"),
            new KeyValuePair<string, string>("SimpleLUI", "SimpleLUI.API.Core.Math"),
            new KeyValuePair<string, string>("SimpleLUI", "SimpleLUI.API.Core.Extras")
        };

        /// <summary>
        ///     Root canvas of this SLUI manager instance.
        /// </summary>
        public Canvas Canvas { get; private set; }

        private SLUICanvasHelper _canvasHelper;

        /// <summary>
        ///     Helper script that executes SLUI coroutines.
        /// </summary>
        internal SLUICanvasHelper CanvasHelper
        {
            get
            {
                if (!_canvasHelper) _canvasHelper = Canvas.gameObject.CollectComponent<SLUICanvasHelper>();
                return _canvasHelper;
            }
        }

        /// <summary>
        ///     Name of the SLUI manager.
        /// </summary>
        public string Name { get; private set; } = "Unknown";

        /// <summary>
        ///     Path to root directory of SLUI manager's scripts.
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        ///     Called when manager is cleaning up.
        /// </summary>
        public event Action OnCleanup;

        /// <summary>
        ///     Called when files of the manager has been reloaded.
        /// </summary>
        public event Action OnReloaded;

        /// <summary>
        ///     List of all lua files added to manager.
        /// </summary>
        public IReadOnlyList<SLUIFile> LuaFiles => _luaFiles;
        private readonly List<SLUIFile> _luaFiles = new List<SLUIFile>();
        private readonly List<SLUIFile> _workingFiles = new List<SLUIFile>();

        internal Lua State { get; private set; }
        internal SLUIWorker Worker { get; }

        private SLUIManager()
        {
            Worker = new SLUIWorker(this);
        }

        /// <summary>
        ///     Adds list of lua files.
        /// </summary>
        public void AddFiles([NotNull] IEnumerable<string> luaFiles)
        {
            if (luaFiles == null) throw new ArgumentNullException(nameof(luaFiles));
            foreach (var l in luaFiles)
            {
                AddFile(l);
            }
        }

        /// <summary>
        ///     Adds new lua file to load and work with.
        /// </summary>
        public void AddFile([NotNull] string luaFile)
        {
            if (luaFile == null) throw new ArgumentNullException(nameof(luaFile));
            if (!luaFile.EndsWith(".lua"))
                throw new FileLoadException("Invalid file extension.", luaFile);
            if (!File.Exists(luaFile))
                throw new FileNotFoundException(null, luaFile);
            foreach (var f in _luaFiles)
                if (f.File == luaFile)
                    return;
 
            _luaFiles.Add(new SLUIFile(luaFile)
            {
                LastModified = File.GetLastWriteTime(luaFile)
            });
        }

        /// <summary>
        ///     Destroy all objects created by this SLUIManager..
        /// </summary>
        public void Cleanup()
        {
            // clear files?
            // _luaFiles.Clear();
            // _workingFiles.Clear();

            Worker?.ClearWorker();
            OnCleanup?.Invoke();
        }

        /// <summary>
        ///     Reloads the manager.
        /// </summary>
        public void Reload()
        {
            if (_luaFiles.Count == 0)
            {
                Debug.LogWarning($"Unable to reload SLUI ({Name}). No lua files has been added.");
                return;
            }

            _workingFiles.Clear();

            //Worker.ClearWorker();
            Cleanup();

            State?.Dispose();
            State = new Lua();       
            Worker.PrepareState(State);

            foreach (var f in _luaFiles)
            {
                if (CheckFileForBannedNamespaces(f.File))
                {
                    continue;
                }

                try
                {
                    State.DoFile(f.File);
                    _workingFiles.Add(f);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
            
            Debug.Log($"SLUI ({Name}) reloaded {_workingFiles.Count} files. ({_luaFiles.Count - _workingFiles.Count} failed)");

            OnReloaded?.Invoke();
        }

        private bool CheckFileForBannedNamespaces([NotNull] string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (!File.Exists(fileName))
                return false;

            try
            {
                var lines = File.ReadAllLines(fileName);
                foreach (var l in lines)
                {
                    if (l.ToLower().Contains("import"))
                    {
                        bool exist = false;
                        foreach (var c in AllowedNamespaces)
                        {
                            // check lib
                            if (!l.Contains($"'{c.Key}',")) continue;
                            if (!l.Contains($"'{c.Value}')")) continue;
                            exist = true;
                            break;
                        }

                        if (!exist)
                        {
                            Debug.LogError($"SLUI ({Name}) refused to load file '{fileName}'. Disallowed namespace detected.");
                            return true; // any of allowed namespaces exist
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }

            return false;
        }
 
        /// <summary>
        ///     Looks if any of the manager's files has been changed.
        /// </summary>
        public bool LookForChanges(bool cleanup = true)
        {
            bool hasChange = false;
            for (var index = 0; index < _luaFiles.Count; index++)
            {
                var f = _luaFiles[index];
                if (!File.Exists(f.File))
                {
                    if (cleanup)
                    {
                        Debug.Log($"SLUI ({Name}) file change detected. File '{f.File}' has been removed.");
                        _luaFiles.Remove(f);
                        index--;
                    }

                    hasChange = true;
                }
                else
                {
                    var currentWriteTime = File.GetLastWriteTime(f.File);
                    if (DateTime.Compare(f.LastModified, currentWriteTime) != 0)
                    {
                        if (cleanup)
                        {
                            Debug.Log($"SLUI ({Name}) file change detected. File '{f.File}' has been modified.");
                            f.LastModified = currentWriteTime;
                        }

                        hasChange = true;
                    }
                }
            }

            return hasChange;
        }

        /// <summary>
        ///     Creates new SLUI manager.
        /// </summary>
        public static SLUIManager CreateNew([NotNull] string name, [NotNull] Canvas root, string directory)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (root == null) throw new ArgumentNullException(nameof(root));
            var instance = new SLUIManager
            {
                Name = name,
                Canvas = root,
                Directory = directory
            };

            return instance;
        }
    }
}
