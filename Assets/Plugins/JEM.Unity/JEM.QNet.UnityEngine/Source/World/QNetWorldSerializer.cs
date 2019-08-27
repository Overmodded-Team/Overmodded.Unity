//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.Game;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JEM.QNet.UnityEngine.World
{
    /// <inheritdoc />
    /// <summary>
    ///     Script thanks to game world can be loaded on client and servers.
    /// </summary>
    public class QNetWorldSerializer : MonoBehaviour
    {
        /// <summary>
        ///     List of serialized and instanced objects by QNetWorldSerializer.
        /// </summary>
        public static IReadOnlyList<QNetObjectBehaviour> SerializedAndInstancedObjects =>
            InternalSerializedAndInstancedObjects;

        private static List<QNetObjectBehaviour> InternalSerializedAndInstancedObjects { get; } =
            new List<QNetObjectBehaviour>();

        private static List<QNetWorldSerializerObject> InternalSerializedObjectsInMemory { get; } =
            new List<QNetWorldSerializerObject>();

        internal static int SerializedObjectsInMemory => InternalSerializedObjectsInMemory.Count;

        /// <summary>
        ///     Current instance of script.
        /// </summary>
        public static QNetWorldSerializer Instance { get; private set; }

        /// <summary>
        ///     Defines whether the world is serialized.
        /// </summary>
        public static bool WorldIsSerialized { get; private set; }

        /// <summary>
        ///     Defines whether the world is currently serializing.
        /// </summary>
        public static bool IsSerializing { get; private set; }

        /// <summary>
        ///     Defines whether the world is currently de-serializing.
        /// </summary>
        public static bool IsDeSerializing { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private IEnumerator InternalDeSerializeObjectsInMemory(Action onDone, Action<string> onChange)
        {
            JEMLogger.Log($"QNetUnity is de-serializing {InternalSerializedObjectsInMemory.Count} objects in memory.");
            for (var index = 0; index < InternalSerializedObjectsInMemory.Count; index++)
            {
                var obj = InternalSerializedObjectsInMemory[index];
                if (obj.Equals(default(QNetWorldSerializerObject)))
                    continue;

                onChange?.Invoke($"fromSerializedObject '{obj.Object.ObjectIdentity}/{obj.Object.PrefabIdentity}'");

                var time = DateTime.Now;
                var isWorking = true;
                StartCoroutine(InternalFromSerializedObject(obj, () => { isWorking = false; }));
                while (isWorking && QNetGameInitializer.DeserializingOnceTimeout >= (DateTime.Now - time).Seconds)
                    yield return new WaitForEndOfFrame();

                if (isWorking)
                {
                    if (obj.SerializedServerState?.GetMessage() != null)
                        QNetManager.Client.OriginalClient.Recycle(obj.SerializedServerState.GetMessage());

                    var str =
                        $"QNetUnity failed fromSerializedObject on '{obj.Object.ObjectIdentity}/{obj.Object.PrefabIdentity}' (timeout)";
                    if (QNetManager.ShutdownOnSerializationTimeout)
                    {
                        QNetGameInitializer.ShutdownInitializing(str);
                        yield break;
                    }

                    JEMLogger.LogError(str);
                }
            }

            JEMLogger.Log($"{InternalSerializedAndInstancedObjects.Count} objects loaded.");
            JEMLogger.Log($"QNetUnity is loading {QNetObjectBehaviour.PredefinedBehaviours.Length} predefined objects.");
            for (var index = 0; index < QNetObjectBehaviour.PredefinedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.PredefinedBehaviours[index];
                onChange?.Invoke($"predefinedUpdated '{obj.ObjectIdentity}'");
                // just set set active
                obj.gameObject.SetActive(true);
                // and call spawned event
                obj.OnInternalSpawned();
                yield return new WaitForEndOfFrame();
            }

            IsSerializing = false;
            WorldIsSerialized = true;
            ClearSerializedObjectsFromMemory();
            onChange?.Invoke("finishing");

            JEMLogger.Log("All objects de-serialized. QNetUnity's world serialization is done.");
            onDone?.Invoke();
        }

        private IEnumerator InternalDestroySerializedObjects(Action onDone)
        {
            JEMLogger.Log($"QNetUnity is destroying {InternalSerializedAndInstancedObjects.Count} serialized objects.");
            for (var index = 0; index < InternalSerializedAndInstancedObjects.Count; index++)
            {
                var obj = InternalSerializedAndInstancedObjects[index];
                yield return InternalDestroySerializedObject(obj);
                index--;
            }

            IsDeSerializing = false;
            WorldIsSerialized = false;
            ClearSerializedObjectsFromMemory();
            yield return null;

            JEMLogger.Log("All serialized objects has been de-serialized.");
            onDone?.Invoke();
        }

        private static IEnumerator InternalFromSerializedObject(QNetWorldSerializerObject obj, Action onDone)
        {
            var predefined = QNetObjectBehaviour.GetPredefinedObject(obj.Object.ObjectIdentity);
            if (predefined != null)
            {
                if (predefined.Prefab != null && predefined.Prefab.PrefabIdentity != obj.Object.PrefabIdentity)
                    JEMLogger.LogError("Local predefined object does not have the same prefab identity as server.");

                // update predefined object transform
                predefined.transform.position = obj.Object.Position;
                predefined.transform.rotation = obj.Object.Rotation;

                // identity
                predefined.UpdateIdentity(predefined.ObjectIdentity, obj.Object.OwnerIdentity);

                // call network methods
                predefined.OnInternalSpawned();
                predefined.OnNetworkActive();

                // scale?
                predefined.transform.localScale = obj.Object.Scale;

                // de-serialize server state
                predefined.DeSerializeServerState(obj.SerializedServerState);

                // activate predefined object
                predefined.gameObject.SetActive(true);
                yield return predefined;

                // manual recycling
                QNetManager.Client.OriginalClient.Recycle(obj.SerializedServerState.GetMessage());
                onDone.Invoke();
                yield break;
            }

            var prefab = QNetManager.Database.GetPrefab(obj.Object.PrefabIdentity);
            if (prefab == null)
            {
                JEMLogger.Log(
                    $"World serializer is trying to create object but prefab of given id not exists in database. ({obj.Object.PrefabIdentity})");

                // manual recycling
                QNetManager.Client.OriginalClient.Recycle(obj.SerializedServerState.GetMessage());
                onDone.Invoke();
                yield break;
            }

            // create local instance of received
            var behaviour = QNetObjectBehaviour.InternalSpawn(obj.Object.ObjectIdentity, prefab, obj.Object.Position,
                obj.Object.Rotation, obj.Object.OwnerIdentity);
            yield return behaviour;

            if (behaviour != null)
            {
                // scale?
                behaviour.transform.localScale = obj.Object.Scale;

                // de-serialize server state
                behaviour.DeSerializeServerState(obj.SerializedServerState);

                // add created object to local array.
                InternalSerializedAndInstancedObjects.Add(behaviour);
            } // ignore, if system fail to spawn new object (most likely because of duplicate)

            // manual recycling
            QNetManager.Client.OriginalClient.Recycle(obj.SerializedServerState.GetMessage());
            onDone.Invoke();
        }

        private static bool InternalDestroySerializedObject([NotNull] QNetObjectBehaviour obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (InternalSerializedAndInstancedObjects.Contains(obj))
                InternalSerializedAndInstancedObjects.Remove(obj);
            QNetObjectBehaviour.InternalDestroy(obj);
            return true;
        }

        /// <summary>
        ///     DeSerializes all object that are in memory.
        ///     This is a Major loading process.
        /// </summary>
        public static void DeSerializeObjectsInMemory(Action onDone, Action<string> onChange)
        {
            if (WorldIsSerialized)
                throw new InvalidOperationException("World is already serialized.");
            if (IsSerializing)
                throw new InvalidOperationException("Serialization process already started.");
            if (IsDeSerializing)
                throw new InvalidOperationException(
                    "Unable to start serialization process while de-serialization is running.");

            IsSerializing = true;
            Instance.StartCoroutine(Instance.InternalDeSerializeObjectsInMemory(onDone, onChange));
        }

        /// <summary>
        ///     Destroys all serialized objects.
        /// </summary>
        public static void DestroySerializedObjects(Action onDone)
        {
            if (IsSerializing)
                throw new InvalidOperationException(
                    "Unable to start process of destroying serialized objects while serialization process is running.");
            if (!WorldIsSerialized)
            {
                JEMLogger.LogError("World is not serialized");
                onDone?.Invoke();
                return;
            }

            if (IsDeSerializing)
                throw new InvalidOperationException(
                    "Unable to start de-serialization process while de-serialization is already running.");

            IsDeSerializing = true;
            Instance.StartCoroutine(Instance.InternalDestroySerializedObjects(onDone));
        }

        /// <summary>
        ///     Writes new serialized object in to memory.
        /// </summary>
        public static void WriteSerializedObjectToMemory(QNetWorldSerializerObject obj)
        {
            if (QNetManager.IsHostActive) // if host is active, we will ignore this one
                return;

            if (WorldIsSerialized)
                Instance.StartCoroutine(InternalFromSerializedObject(obj, () => { }));
            else
                InternalSerializedObjectsInMemory.Add(obj);
        }

        /// <summary>
        ///     Removes serialized object from memory.
        /// </summary>
        public static void RemoveSerializedObjectFromMemory(short objectIdentity)
        {
            if (QNetManager.IsHostActive) // if host is active, we will ignore this one
                return;

            if (WorldIsSerialized)
            {
                var qNetObject = QNetObjectBehaviour.GetSpawnedObject(objectIdentity) ??
                                 QNetObjectBehaviour.GetPredefinedObject(objectIdentity);
                if (qNetObject == null)
                    JEMLogger.LogError(
                        $"System was trying to destroy de-serialized object but target of identity {objectIdentity} not exists in current world.");
                else
                    InternalDestroySerializedObject(qNetObject);
            }
            else
            {
                var serializedObject = default(QNetWorldSerializerObject);
                for (var index = 0; index < InternalSerializedObjectsInMemory.Count; index++)
                    if (InternalSerializedObjectsInMemory[index].Object.ObjectIdentity == objectIdentity)
                    {
                        serializedObject = InternalSerializedObjectsInMemory[index];
                        break;
                    }

                if (serializedObject.Equals(default(QNetWorldSerializerObject)))
                    JEMLogger.LogWarning(
                        $"System was trying to remove object from world serializer memory but target of identity {objectIdentity} not exist.");
                else
                    InternalSerializedObjectsInMemory.Remove(serializedObject);
            }
        }

        /// <summary>
        ///     Clear memory of serialized objects.
        /// </summary>
        public static void ClearSerializedObjectsFromMemory()
        {
            InternalSerializedObjectsInMemory.Clear();
        }
    }
}