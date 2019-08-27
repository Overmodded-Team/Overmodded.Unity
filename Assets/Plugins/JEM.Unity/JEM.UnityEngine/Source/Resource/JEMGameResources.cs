//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JEM.UnityEngine.Resource
{
    /// <summary>
    ///     Loaded resource pack.
    /// </summary>
    public class JEMGameResourcePack
    {
        internal JEMGameResourcePack(string name, string description, string file)
        {
            Name = name;
            Description = description;
            File = file;
            Bundle = null;
            Loaded = false;
        }

        /// <summary>
        ///     Name of loaded resource.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Description of loaded resource.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Local patch.
        /// </summary>
        public string File { get; }

        /// <summary>
        ///     Loaded resources.
        /// </summary>
        public AssetBundle Bundle { get; set; }

        /// <summary>
        ///     Defines whether this resource pack is loaded.
        /// </summary>
        public bool Loaded { get; set; }

        /// <summary>
        ///     Checks if asset with given name exist.
        /// </summary>
        public bool AssetExist([NotNull] string resourceName)
        {
            if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));
            return Bundle.Contains(resourceName);
        }

        /// <summary>
        ///     Get asset by name.
        /// </summary>
        /// <typeparam name="T">Type of resource.</typeparam>
        /// <param name="resourceName">Name of resource</param>
        public JEMGameAsset<T> GetAsset<T>(string resourceName) where T : Object
        {
            var asset = InternalGetAsset(resourceName, typeof(T));
            return asset == null ? default(JEMGameAsset<T>) : new JEMGameAsset<T>(resourceName, asset, this);
        }

        /// <summary>
        ///     Get asset by name.
        /// </summary>
        /// <param name="resourceName">Name of resource</param>
        /// <param name="resourceType">Type of resource.</param>
        private Object InternalGetAsset([NotNull] string resourceName, [NotNull] Type resourceType)
        {
            if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));
            if (resourceType == null) throw new ArgumentNullException(nameof(resourceType));
            return Bundle.Contains(resourceName) ? Bundle.LoadAsset(resourceName, resourceType) : null;
        }
    }

    /// <summary>
    ///     Game resources class.
    /// </summary>
    public static class JEMGameResources
    {
        /// <summary>
        ///     Delegate for resources load effect.
        /// </summary>
        public delegate void ResourcesLoaded(ResourcesLoadingEffect effect, JEMGameResourcePack[] packs);

        /// <summary>
        ///     Delegate for resources load progress.
        /// </summary>
        /// <param name="name">Name of resource that currently being loaded.</param>
        public delegate void ResourcesLoadProgress(string name, int index, int total);

        /// <summary>
        ///     Resources loading effect used by OnResourcesLoaded delegate.
        /// </summary>
        public enum ResourcesLoadingEffect
        {
            /// <summary>
            ///     Unknown.
            /// </summary>
            Unknown,

            /// <summary>
            ///     Success.
            /// </summary>
            Success,

            /// <summary>
            ///     Base directory not exist.
            /// </summary>
            BaseDirNotExist,

            /// <summary>
            ///     Registered resource not exist.
            /// </summary>
            RegisteredResNotExist
        }

        private static JEMGameResourcesScript script;
        private static readonly List<JEMGameResourcePack> InternalGameResourcePacks = new List<JEMGameResourcePack>();

        /// <summary>
        ///     List of loaded resources packs.
        /// </summary>
        public static IReadOnlyList<JEMGameResourcePack> GameResourcePacks => InternalGameResourcePacks;

        internal static void RegenerateLocalScript()
        {
            if (script != null)
                return;

            var obj = new GameObject(nameof(JEMGameResourcesScript));
            Object.DontDestroyOnLoad(obj);
            script = obj.AddComponent<JEMGameResourcesScript>();

            if (script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMGameResources)}@{nameof(JEMGameResourcesScript)}");
        }

        /// <summary>
        ///     Get whole resource bank.
        /// </summary>
        /// <param name="packName">Name of loaded resource bank.</param>
        public static JEMGameResourcePack GetPack(string packName)
        {
            return InternalGameResourcePacks.FirstOrDefault(r => r.Name == packName);
        }

        /// <summary>
        ///     Get array of packs from array of packs names.
        /// </summary>
        public static JEMGameResourcePack[] FromStringArray(string[] packsNames)
        {
            return packsNames.Select(GetPack).Where(p => !p.Equals(default(JEMGameResourcePack))).ToArray();
        }

        /// <summary>
        ///     Get array of packs names form array of packs.
        /// </summary>
        public static string[] ToStringArray(JEMGameResourcePack[] packs)
        {
            return packs.Select(name => name.Name).ToArray();
        }

        /// <summary>
        ///     Loads base info about resources that game can load.
        /// </summary>
        public static bool LoadResourcesInfo()
        {
            var stopwatch = Stopwatch.StartNew();
            var cfg = JEMGameResourcesConfiguration.Get();
            JEMLogger.Log($"New resources info load process has been started at directory {cfg.PackDirectory}");

            if (Directory.Exists(cfg.PackDirectory))
            {
                foreach (var file in Directory.GetFiles(cfg.PackDirectory))
                {
                    if (!file.EndsWith(cfg.PackExtension))
                        continue;

                    var fileStopWatch = Stopwatch.StartNew();
                    JEMLogger.Log($"Reading File -> {file}");
                    var packName = JEMGameResourcesUtility.ResolveResourceNameFromPath(file);
                    var packDescription = string.Empty;
                    var packInfoFile = JEMGameResourcesUtility.ResolveMapDescPath(file);
                    JEMLogger.Log($"Searching For -> {packInfoFile}");
                    var packInfoFileInfo = new FileInfo(packInfoFile);
                    if (packInfoFileInfo.Exists)
                    {
                        var lines = File.ReadAllLines(packInfoFile);
                        if (lines.Length != 0)
                        {
                            packName = JEMGameResourcesUtility.ResolveResourceName(lines[0]);
                            var list = new List<string>(lines);
                            list.RemoveAt(0);
                            packDescription = string.Join(Environment.NewLine, list.ToArray());
                        }
                    }

                    InternalGameResourcePacks.Add(new JEMGameResourcePack(packName, packDescription, file));

                    fileStopWatch.Stop();
                    JEMLogger.Log($"TOOK -> {fileStopWatch.Elapsed.TotalSeconds:0.0}s");
                }
            }
            else
            {
                JEMLogger.LogError(
                    $"Unable to load game resources info. Base resources patch `{cfg.PackDirectory}` not exist.");
                return false;
            }

            stopwatch.Stop();
            JEMLogger.Log(
                $"Resources info loaded in -> {stopwatch.Elapsed.TotalSeconds:0.0}s. Total resources loaded: {InternalGameResourcePacks.Count}");

            return true;
        }

        /// <summary>
        ///     Resources initialization function, used in start of the game.
        /// </summary>
        /// <param name="packName">Name of resource to load.</param>
        /// <param name="async">Loading process will run asynchronously.</param>
        /// <param name="loadedEvent">Loading callback with argument of effect.</param>
        /// <param name="progressEvent">Loading progress event.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LoadResourcesAssets(string packName, bool async, ResourcesLoaded loadedEvent,
            ResourcesLoadProgress progressEvent)
        {
            LoadResourcesAssets(new[] {packName}, async, loadedEvent, progressEvent);
        }

        /// <summary>
        ///     Resources initialization function, used in start of the game.
        /// </summary>
        /// <param name="packList">List of resources to load.</param>
        /// <param name="async">Loading process will run asynchronously.</param>
        /// <param name="loadedEvent">Loading callback with argument of effect.</param>
        /// <param name="progressEvent">Loading progress event.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void LoadResourcesAssets([NotNull] string[] packList, bool async, ResourcesLoaded loadedEvent,
            ResourcesLoadProgress progressEvent)
        {
            if (packList == null) throw new ArgumentNullException(nameof(packList));
            RegenerateLocalScript();
            script.StartCoroutine(script.InternalLoadResourcesAssets(packList, async, loadedEvent, progressEvent));
        }
    }
}