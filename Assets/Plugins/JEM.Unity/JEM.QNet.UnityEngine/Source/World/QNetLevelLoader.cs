//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JEM.QNet.UnityEngine.World
{
    public delegate void QNetResolveLevel(string levelName, Action onLevelResolved);

    /// <inheritdoc />
    /// <summary>
    ///     Script of base level(map) loading.
    /// </summary>
    public class QNetLevelLoader : MonoBehaviour
    {
        /// <summary>
        ///     Name of default level of the game.
        /// </summary>
        [Header("Base Settings")]
        public string NoneLevelName = "None";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private IEnumerator InternalLoad(string levelName, Action onDone)
        {
            JEMLogger.Log($"Loading level {levelName}");
            if (OnResolveLevel != null)
            {
                bool isResolving = true;
                OnResolveLevel.Invoke(levelName, () => { isResolving = false; });
                while (isResolving)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            var asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
            yield return asyncOperation;
            LevelName = levelName;
            LevelLoaded = true;
            IsLoadingLevel = false;
            JEMLogger.Log("Level loaded.");
            onDone?.Invoke();
        }

        private IEnumerator InternalUnload(Action onDone)
        {
            JEMLogger.Log($"Unloading level {LevelName}");
            var asyncOperation = SceneManager.LoadSceneAsync(NoneLevelName, LoadSceneMode.Single);
            yield return asyncOperation;
            LevelName = null;
            LevelLoaded = false;
            IsUnloadingLevel = false;
            JEMLogger.Log("Level unloaded.");
            onDone?.Invoke();
        }

        /// <summary>
        ///     Loads given level.
        /// </summary>
        public static void Load(string levelName, Action onDone)
        {
            if (LevelLoaded)
                throw new InvalidOperationException("System is unable to load level while one is currently loaded.");
            if (IsLoadingLevel)
                throw new InvalidOperationException(
                    "System is unable to load level while other loading process is active.");
            if (IsUnloadingLevel)
                throw new InvalidOperationException(
                    "System is unable to load level while level unloading process is running.");

            IsLoadingLevel = true;
            Instance.StartCoroutine(Instance.InternalLoad(levelName, onDone));
        }

        /// <summary>
        ///     Unloads current level by loading new half empty main scene.
        ///     Half empty main scene means that there is no Management and Interface objects on it. Only default camera with video
        ///     background.
        /// </summary>
        public static void UnLoad(Action onDone)
        {
            if (IsLoadingLevel)
                throw new InvalidOperationException(
                    "System is unable to unload level because level load process is active.");
            if (!LevelLoaded)
                throw new InvalidOperationException("System can't unload level while there is no level loaded.");
            if (IsUnloadingLevel)
                throw new InvalidOperationException(
                    "System is unable to unload level while other unloading process is active.");

            IsUnloadingLevel = true;
            Instance.StartCoroutine(Instance.InternalUnload(onDone));
        }

        public static event QNetResolveLevel OnResolveLevel;

        /// <summary>
        ///     Name of currently loaded level.
        /// </summary>
        public static string LevelName { get; private set; }

        /// <summary>
        ///     Defines whether some level is currently loaded.
        /// </summary>
        public static bool LevelLoaded { get; private set; }

        /// <summary>
        ///     Defines whether any level is currently under loading.
        /// </summary>
        public static bool IsLoadingLevel { get; private set; }

        /// <summary>
        ///     Defines whether the current level is currently unloading.
        /// </summary>
        public static bool IsUnloadingLevel { get; private set; }

        /// <summary>
        ///     Current instance of script.
        /// </summary>
        public static QNetLevelLoader Instance { get; private set; }

    }
}