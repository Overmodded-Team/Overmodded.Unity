//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.World;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Game
{
    /// <summary>
    ///     QNet map state.
    /// </summary>
    public enum QNetMapState
    {
        /// <summary>
        ///     Any map is currently loaded.
        ///     Most likely user is currently in main menu.
        /// </summary>
        NotLoaded,

        /// <summary>
        ///     Map is currently under loading process.
        /// </summary>
        Loading,

        /// <summary>
        ///     Map is loaded.
        /// </summary>
        Loaded,

        /// <summary>
        ///     Current map is currently under unload.
        /// </summary>
        Unloading
    }

    /// <inheritdoc />
    /// <summary>
    ///     Script of that contains two methods of initializing whole game as server or as client.
    /// </summary>
    public partial class QNetGameInitializer : MonoBehaviour
    {
        /// <summary>
        ///     Defines maximum time of sleep that de-serialization process can get.
        /// </summary>
        public const int DeserializingTimeout = 10;

        /// <summary>
        ///     Defines maximum time of sleep that de-serialization process on one object can get.
        /// </summary>
        public const int DeserializingOnceTimeout = 4;

        /// <summary>
        ///     Name of default map that should be loaded by game.
        /// </summary>
        public string DefaultMapName = "Map_Test";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            ServerNextMapName = DefaultMapName;
        }

        private static IEnumerator InternalRunServer(QNetConfiguration configuration)
        {
            var sw = Stopwatch.StartNew();
            var targetLevel = ServerNextMapName;

            ServerIsInitializing = true;

            // load world fist
            LastMapState = QNetMapState.Loading;
            OnMapStateChanged?.Invoke(QNetMapState.Loading);
            var isLevelLoading = true;
            QNetLevelLoader.Load(targetLevel, () => { isLevelLoading = false; });
            while (isLevelLoading)
                yield return new WaitForEndOfFrame();

            OnLevelLoaded?.Invoke(targetLevel);
            yield return new WaitForEndOfFrame();

            // then load serialized object in memory
            // TODO: Write objects from save in to memory

            var time = DateTime.Now;
            var isWorldSerializing = true;
            var worldSerializingLastAction = "not defined";
            QNetWorldSerializer.DeSerializeObjectsInMemory(() => { isWorldSerializing = false; }, action =>
            {
                time = DateTime.Now;
                worldSerializingLastAction = action;
            });
            while (isWorldSerializing)
                yield return new WaitForEndOfFrame();

            if ((DateTime.Now - time).Seconds >= DeserializingTimeout)
            {
                ShutdownInitializing(worldSerializingLastAction);
                yield break;
            }

            LastMapState = QNetMapState.Loaded;
            OnMapStateChanged?.Invoke(QNetMapState.Loaded);

            // the initialize server
            QNetManager.StartServer(configuration);

            GameIsInitializing = false;
            ServerIsInitializing = false;
            GameInitialized = true;

            bool isWorldReady = false;
            OnWorldAndNetworkReady?.Invoke(() => { isWorldReady = true; });
            while (!isWorldReady)
            {
                yield return new WaitForEndOfFrame();
            }

            // we need to call OnNetworkActive event
            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.SpawnedBehaviours[index];
                obj.OnNetworkActive();
            }

            for (var index = 0; index < QNetObjectBehaviour.PredefinedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.PredefinedBehaviours[index];
                obj.OnInternalSpawned();
                obj.OnNetworkActive();
            }

            JEMLogger.Log($"QNetUnity ServerRun main work took {sw.Elapsed.Milliseconds:0.00}ms.");
        }

        private static IEnumerator InternalRunClient(QNetGameInitializerData data, Action onDone)
        {
            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING LEVEL", $"Loading {data.LevelName}");
            OnClientLoadingStart?.Invoke();

            yield return new WaitForSeconds(0.6f); // wait some time, lol

            var sw = Stopwatch.StartNew();

            // load world fist
            LastMapState = QNetMapState.Loading;
            OnMapStateChanged?.Invoke(QNetMapState.Loading);
            var isLevelLoading = true;
            QNetLevelLoader.Load(data.LevelName, () => { isLevelLoading = false; });
            while (isLevelLoading)
                yield return new WaitForEndOfFrame();

            OnLevelLoaded?.Invoke(data.LevelName);
            yield return new WaitForEndOfFrame();

            // update lading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING WORLD", "Waiting for server.");

            GameIsInitializing = false;
            GameInitialized = true;
            JEMLogger.Log($"QNetUnity ClientRun main work took {sw.Elapsed.Milliseconds:0.00}ms.");
            onDone?.Invoke();
        }

        /// <summary>
        ///     Method run from QNeyHandlerWorldReceiver from message of header WORLD_SERIALIZATION.
        /// </summary>
        private static IEnumerator InternalRunLateClientWorldSerializer(Action onDone)
        {
            var sw = Stopwatch.StartNew();

            // update lading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING WORLD",
                $"DeSerializing {QNetWorldSerializer.SerializedObjectsInMemory} world objects.");

            // load serialized object in memory
            var time = DateTime.Now;
            var isWorldSerializing = true;
            var worldSerializingLastAction = "not defined";
            QNetWorldSerializer.DeSerializeObjectsInMemory(() => { isWorldSerializing = false; }, action =>
            {
                time = DateTime.Now;
                worldSerializingLastAction = action;
            });
            while (isWorldSerializing && (DateTime.Now - time).Seconds < DeserializingTimeout)
                yield return new WaitForEndOfFrame();

            if ((DateTime.Now - time).Seconds >= DeserializingTimeout)
            {
                ShutdownInitializing(worldSerializingLastAction);
                yield break;
            }

            LastMapState = QNetMapState.Loaded;
            OnMapStateChanged?.Invoke(QNetMapState.Loaded);

            // update loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "READY", "Setting up player.");

            GameIsInitializing = false;
            GameInitialized = true;
            OnClientLoadingEnd?.Invoke();

            // the initialize client
            OnLoadClientSideContent?.Invoke();

            bool isWorldReady = false;
            OnWorldAndNetworkReady?.Invoke(() => { isWorldReady = true; });
            while (!isWorldReady)
            {
                yield return new WaitForEndOfFrame();
            }

            JEMLogger.Log($"QNetUnity ClientLateRun main work took {sw.Elapsed.Milliseconds:0.00}ms.");
            onDone?.Invoke();
        }

        private static IEnumerator InternalRunHost(QNetConfiguration configuration)
        {
            // load world first
            var targetLevel = ServerNextMapName;

            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING LEVEL", $"Loading {targetLevel}");
            OnClientLoadingStart?.Invoke();

            yield return new WaitForSeconds(0.6f); // wait some time, lol

            var sw = Stopwatch.StartNew();
            ServerIsInitializing = true;
            HostIsInitializing = true;

            LastMapState = QNetMapState.Loading;
            OnMapStateChanged?.Invoke(LastMapState);

            var isLevelLoading = true;
            QNetLevelLoader.Load(targetLevel, () => { isLevelLoading = false; });
            while (isLevelLoading)
                yield return new WaitForEndOfFrame();

            OnLevelLoaded?.Invoke(targetLevel);
            yield return new WaitForEndOfFrame();

            // TODO: Write objects from save in to memory
            // TODO: Remove block of code below (DeSerializingObjects)

            // update lading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING WORLD",
                $"DeSerializing {QNetWorldSerializer.SerializedObjectsInMemory} world objects.");

            var isWorldSerializing = true;
            var time = DateTime.Now;
            var worldSerializingLastAction = "not defined";
            QNetWorldSerializer.DeSerializeObjectsInMemory(() => { isWorldSerializing = false; }, action =>
            {
                time = DateTime.Now;
                worldSerializingLastAction = action;
            });
            while (isWorldSerializing)
                yield return new WaitForEndOfFrame();

            if ((DateTime.Now - time).Seconds >= DeserializingTimeout)
            {
                ShutdownInitializing(worldSerializingLastAction);
                yield break;
            }

            LastMapState = QNetMapState.Loaded;
            OnMapStateChanged?.Invoke(LastMapState);

            // update loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "READY", "Setting up player.");

            // then initialize host
            QNetManager.StartHost(configuration);

            GameIsInitializing = false;
            ServerIsInitializing = false;
            HostIsInitializing = false;
            GameInitialized = true;
            OnClientLoadingEnd?.Invoke();

            // the initialize client
            OnLoadClientSideContent?.Invoke();

            bool isWorldReady = false;
            OnWorldAndNetworkReady?.Invoke(() => { isWorldReady = true; });
            while (!isWorldReady)
            {
                yield return new WaitForEndOfFrame();
            }

            // we need to call OnNetworkActive event
            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.SpawnedBehaviours[index];
                obj.OnNetworkActive();
            }

            for (var index = 0; index < QNetObjectBehaviour.PredefinedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.PredefinedBehaviours[index];
                obj.OnInternalSpawned();
                obj.OnNetworkActive();
            }

            JEMLogger.Log($"QNetUnity RunHost main work took {sw.Elapsed.Milliseconds:0.00}ms.");
        }

        private static IEnumerator InternalDeInitialize(Action onDone)
        {
            var sw = Stopwatch.StartNew();

            // the initialize client
            OnUnloadClientSideContent?.Invoke();

            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "UNLOADING WORLD", "Destroying players.");
            OnClientLoadingStart?.Invoke();

            LastMapState = QNetMapState.Unloading;
            OnMapStateChanged?.Invoke(LastMapState);

            // destroy players
            yield return QNetPlayer.DestroyAllQNetPlayers();

            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "UNLOADING WORLD",
                $"Destroying {QNetWorldSerializer.SerializedAndInstancedObjects.Count} objects.");

            // destroy world objects
            var isDestroyingWorldObjects = true;
            QNetWorldSerializer.DestroySerializedObjects(() => { isDestroyingWorldObjects = false; });
            while (isDestroyingWorldObjects)
                yield return new WaitForEndOfFrame();

            // try to destroy rest of QNet objects just for sure
            if (QNetObjectBehaviour.SpawnedBehaviours.Length != 0)
                JEMLogger.Log(
                    $"QNetUnity find and will destroy {QNetObjectBehaviour.SpawnedBehaviours.Length} additional objects that has been created not by QNetWorldSerializer.");

            while (QNetObjectBehaviour.SpawnedBehaviours.Length > 0)
            {
                QNetObjectBehaviour.InternalDestroy(QNetObjectBehaviour.SpawnedBehaviours[0]);
                yield return new WaitForEndOfFrame();
            }

            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "UNLOADING LEVEL", "Unloading level.");

            // unload world
            var isLevelUnLoading = true;
            QNetLevelLoader.UnLoad(() => { isLevelUnLoading = false; });
            while (isLevelUnLoading)
                yield return new WaitForEndOfFrame();

            // clear behaviours just for sure
            QNetObjectBehaviour.ClearBehaviours();

            LastMapState = QNetMapState.NotLoaded;
            OnMapStateChanged?.Invoke(LastMapState);

            GameIsDeInitializing = false;
            GameInitialized = false;
            OnClientLoadingEnd?.Invoke();

            JEMLogger.Log($"QNetUnity DeInitialization main work took {sw.Elapsed.Milliseconds:0.00}ms.");
            onDone?.Invoke();
        }

        /// <summary>
        ///     Shutdown game initialization because of objects timeout.
        /// </summary>
        internal static void ShutdownInitializing(string str)
        {
            QNetManager.StopCurrentConnection($"{nameof(QNetWorldSerializer.DeSerializeObjectsInMemory)} timeout.");
            JEMLogger.LogError($"QNetUnity is shutting down game, because of game initializing error. ({str})");
            Application.Quit();
        }

        /// <summary>
        ///     Runs server.
        ///     This method starts all server loading processes and starts QNetServer at the end of loading.
        /// </summary>
        public static void RunServer(QNetConfiguration configuration)
        {
            if (GameInitialized)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while game is already initialized.");
            if (GameIsInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while other process is already running.");
            if (GameIsDeInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization while de-initialization process is running.");

            JEMLogger.Log("QNetUnity is starting server.");

            GameIsInitializing = true;
            Instance.StartCoroutine(InternalRunServer(configuration));
        }

        /// <summary>
        ///     Runs client.
        ///     This methods starts all client loading processes.
        ///     Note that this method can only be called by server from LEVEL_LOADING message.
        /// </summary>
        internal static void RunClient(QNetGameInitializerData data, Action onDone)
        {
            if (GameInitialized)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while game is already initialized.");
            if (GameIsInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while other process is already running.");
            if (GameIsDeInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization while de-initialization process is running.");

            JEMLogger.Log("QNetUnity is starting client.");

            GameIsInitializing = true;
            Instance.StartCoroutine(InternalRunClient(data, onDone));
        }

        /// <summary>
        ///     Method run from QNeyHandlerWorldReceiver from message of header WORLD_SERIALIZATION.
        /// </summary>
        internal static void RunLateClientWorldSerializer(Action onDone)
        {
            Instance.StartCoroutine(InternalRunLateClientWorldSerializer(onDone));
        }

        /// <summary>
        ///     Runs host.
        ///     This method starts all server then client loading processes and at the end runs QNetServer and QNetClient peers.
        /// </summary>
        public static void RunHost(QNetConfiguration configuration)
        {
            if (GameInitialized)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while game is already initialized.");
            if (GameIsInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization process while other process is already running.");
            if (GameIsDeInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't run game initialization while de-initialization process is running.");

            JEMLogger.Log("QNetUnity is starting host.");

            GameIsInitializing = true;
            Instance.StartCoroutine(InternalRunHost(configuration));
        }

        /// <summary>
        ///     Method to de-initialize all initialized content of the game.
        ///     If server, this method will exit the application.
        ///     If client or host, this method will exit to main menu.
        /// </summary>
        internal static void DeInitialize(Action onDone)
        {
            if (!GameInitialized)
                throw new InvalidOperationException("QNetUnity can't de-initialize game while is not initialized.");
            if (GameIsInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't de-initialize game while other process of initialization is already running.");
            if (GameIsDeInitializing)
                throw new InvalidOperationException(
                    "QNetUnity can't de-initialize game while other process of de-initialization is already running.");

            JEMLogger.Log("QNetUnity is de-initializing.");

            GameIsDeInitializing = true;
            Instance.StartCoroutine(InternalDeInitialize(onDone));
        }

        /// <summary>
        ///     Defines next map that server will load next.
        /// </summary>
        public static string ServerNextMapName { get; set; } = "<UNDEFINED>";

        /// <summary>
        ///     Defines last state of map.
        /// </summary>
        public static QNetMapState LastMapState { get; private set; }

        /// <summary>
        ///     Defines whether the game is currently under initialization.
        /// </summary>
        public static bool GameIsInitializing { get; private set; }

        /// <summary>
        ///     Defines whether the server is currently under initialization.
        /// </summary>
        public static bool ServerIsInitializing { get; private set; }

        /// <summary>
        ///     Defines whether the hos is currently under initialization.
        /// </summary>
        public static bool HostIsInitializing { get; private set; }

        /// <summary>
        ///     Defines whether the game is currently under de-initialization.
        /// </summary>
        public static bool GameIsDeInitializing { get; private set; }

        /// <summary>
        ///     Defines whether the game is initialized.
        /// </summary>
        public static bool GameInitialized { get; private set; }

        /// <summary>
        ///     Current instance of script.
        /// </summary>
        public static QNetGameInitializer Instance { get; private set; }
    }
}