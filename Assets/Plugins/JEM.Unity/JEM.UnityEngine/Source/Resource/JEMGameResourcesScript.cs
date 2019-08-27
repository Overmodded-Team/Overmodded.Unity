//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JEM.Core.Debugging;
using UnityEngine;

namespace JEM.UnityEngine.Resource
{
    internal class JEMGameResourcesScript : MonoBehaviour
    {
        public IEnumerator InternalLoadResourcesAssets(string[] packList, bool async,
            JEMGameResources.ResourcesLoaded loadedEvent,
            JEMGameResources.ResourcesLoadProgress progressEvent)
        {
            if (packList == null || packList.Length == 0)
            {
                JEMLogger.LogWarning("New resources load process has been received but packList is null or empty.");
                yield return new WaitForEndOfFrame();
                loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.Success, new JEMGameResourcePack[0]);
                yield break;
            }

            var stopwatch = Stopwatch.StartNew();
            var cfg = JEMGameResourcesConfiguration.Get();
            var packs = new List<JEMGameResourcePack>();
            JEMLogger.Log($"New resources load process has been started at directory {cfg.PackDirectory}");

            if (Directory.Exists(cfg.PackDirectory))
            {
                foreach (var file in packList)
                {
                    yield return new WaitForEndOfFrame();
                    var packData = JEMGameResources.GetPack(file);
                    if (packData.Equals(default(JEMGameResourcePack)))
                    {
                        JEMLogger.LogError(
                            $"System was unable to load pack {file}. Not exists in loaded list of resources.");
                        loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.RegisteredResNotExist,
                            new JEMGameResourcePack[0]);
                    }
                    else
                    {
                        if (packData.Loaded)
                        {
                            packs.Add(packData);
                            JEMLogger.Log($"Resource pack {file} exists and has been already loaded. Skipping!");
                            continue;
                        }

                        var packFile = JEMGameResourcesUtility.ResolveMapPath(file);
                        var packFileInfo = new FileInfo(packFile);
                        if (packFileInfo.Exists)
                        {
                            JEMLogger.Log($"Loading -> {packFileInfo.FullName}");
                            var fileStopWatch = Stopwatch.StartNew();
                            if (async)
                            {
                                var assetBundlesRequest = AssetBundle.LoadFromFileAsync(packFileInfo.FullName);
                                yield return assetBundlesRequest;
                                yield return new WaitForEndOfFrame();

                                if (assetBundlesRequest == null || assetBundlesRequest.assetBundle == null)
                                {
                                    JEMLogger.LogError($"Resource {packFile} received but without assetBundle data.");
                                    loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.RegisteredResNotExist,
                                        new JEMGameResourcePack[0]);
                                    yield break;
                                }

                                packData.Bundle = assetBundlesRequest.assetBundle;
                                packData.Loaded = true;
                            }
                            else
                            {
                                var assetBundle = AssetBundle.LoadFromFile(packFileInfo.FullName);
                                if (assetBundle == null)
                                {
                                    JEMLogger.LogError($"Resource {packFile} received but without assetBundle data.");
                                    loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.RegisteredResNotExist,
                                        new JEMGameResourcePack[0]);
                                    yield break;
                                }

                                packData.Bundle = assetBundle;
                                packData.Loaded = true;
                            }

                            yield return new WaitForEndOfFrame();
                            packs.Add(packData);
                            fileStopWatch.Stop();
                            JEMLogger.Log($"TOOK -> {fileStopWatch.Elapsed.TotalSeconds:0.0}s");
                        }
                        else
                        {
                            JEMLogger.LogError($"Unable to load game resources. Given file {packFile} not exist.");
                            loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.RegisteredResNotExist,
                                new JEMGameResourcePack[0]);
                            yield break;
                        }
                    }
                }
            }
            else
            {
                JEMLogger.LogError(
                    $"Unable to load game resources. Base resources patch `{cfg.PackDirectory}` not exist.");
                loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.BaseDirNotExist, new JEMGameResourcePack[0]);
                yield break;
            }

            stopwatch.Stop();
            JEMLogger.Log($"Resources loaded in -> {stopwatch.Elapsed.TotalSeconds:0.0}s");
            yield return new WaitForEndOfFrame();
            loadedEvent?.Invoke(JEMGameResources.ResourcesLoadingEffect.Success,
                packs.Count == 0 ? new JEMGameResourcePack[0] : packs.ToArray());
        }
    }
}