//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JEM.Core.Debugging;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.World;
using System.Collections;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Game
{
    public partial class QNetGameInitializer
    {
        private IEnumerator InternalLoadServerLevelOnFly(string levelName)
        {
            JEMLogger.Log($"QNetUnity is loading map '{levelName}' on fly.");
            GameIsDeInitializing = true;
            GameIsInitializing = true;
            GameInitialized = false;

            QNetManager.Server.AcceptNewConnections = false;
            for (var index = 0; index < QNetPlayer.QNetPlayers.Length; index++)
            {
                var p = QNetPlayer.QNetPlayers[index];
                p.Loaded = false;
            }

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

            // Destroy all behaviours
            yield return QNetObjectBehaviour.DestroyAll();

            // clear behaviours just for sure
            QNetObjectBehaviour.ClearBehaviours();

            // activate loading screen
            OnClientLoadingInfoUpdated?.Invoke(true, "LOADING LEVEL", "Loading level.");

            LastMapState = QNetMapState.Loading;
            OnMapStateChanged?.Invoke(LastMapState);

            var isLevelLoading = true;
            QNetLevelLoader.Load(levelName, () => { isLevelLoading = false; });
            while (isLevelLoading)
                yield return new WaitForEndOfFrame();

            LastMapState = QNetMapState.Loaded;
            OnMapStateChanged?.Invoke(LastMapState);

            if (QNetManager.IsServerActive)
            {
                // we are on server!
                // here, we need to send level change info to all clients
                //var writer = QNetManager.Server.GenerateOutgoingMessage((ushort)QNetUnityLocalHeader.LEVEL_LOAD_ON_FLY);
                //writer.WriteString(QNetLevelLoader.LevelName);
                //QNetManager.Server.SendToAll(QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered, writer);
            }

            GameIsDeInitializing = false;
            GameIsInitializing = false;
            GameInitialized = true;
            QNetManager.Server.AcceptNewConnections = true;

            JEMLogger.Log($"QNetUnity has loaded map '{levelName}' on fly.");
        }

        /// <summary>
        ///     Runs the server loading level on fly.
        ///     It stops game simulation and blocks incoming connections for loading time.
        /// </summary>
        public static void LoadServerLevelOnFly(string levelName)
        {
            Instance.StartCoroutine(Instance.InternalLoadServerLevelOnFly(levelName));
        }

    }
}
