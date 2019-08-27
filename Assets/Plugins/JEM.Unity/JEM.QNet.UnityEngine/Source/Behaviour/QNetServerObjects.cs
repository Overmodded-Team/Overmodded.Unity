//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Extensions;
using JEM.QNet.UnityEngine.Messages;
using JetBrains.Annotations;
using System;

namespace JEM.QNet.UnityEngine.Behaviour
{
    /// <summary>
    ///     Some cool methods for QNetObject only for server side.
    /// </summary>
    public static class QNetServerObjects
    {
        /// <summary>
        ///     Destroys all objects that given connection owns.
        /// </summary>
        public static void DestroyAllOwnedObjectsOfConnection(QNetConnection connection)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("This methods can only be used by server.");

            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.SpawnedBehaviours[index];
                if (obj.OwnerIdentity == 0 || obj.OwnerIdentity != connection.ConnectionIdentity) continue;
                QNetObjectBehaviour.Destroy(obj);
                index--;
            }

            for (var index = 0; index < QNetObjectBehaviour.PredefinedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.PredefinedBehaviours[index];
                if (obj.OwnerIdentity == 0 || obj.OwnerIdentity != connection.ConnectionIdentity) continue;
                QNetObjectBehaviour.Destroy(obj);
                index--;
            }
        }

        /// <summary>
        ///     Sends destroy message of given object to all connections.
        /// </summary>
        /// <param name="qNetObject"></param>
        public static void DestroyObjectOnAllConnections([NotNull] QNetObjectBehaviour qNetObject)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("This methods can only be used by server.");
            if (qNetObject == null) throw new ArgumentNullException(nameof(qNetObject));
            QNetManager.Server.SendToAll(QNetUnityLocalChannel.OBJECT_QUERY, QNetMessageMethod.ReliableOrdered,
                QNetManager.Server.GenerateOutgoingMessage(QNetUnityLocalHeader.OBJECT_DELETE,
                    qNetObject.ObjectIdentity));
        }

        /// <summary>
        ///     Sends given QNetObject to given connection.
        /// </summary>
        public static void SendObjectToConnection(QNetConnection connection, [NotNull] QNetObjectBehaviour qNetObject)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("This methods can only be used by server.");
            if (qNetObject == null) throw new ArgumentNullException(nameof(qNetObject));
            QNetManager.Server.Send(connection, QNetUnityLocalChannel.OBJECT_QUERY, QNetMessageMethod.ReliableOrdered,
                ResolveObjectCreateMessage(qNetObject));
        }

        /// <summary>
        ///     Sends given QNetObject to all connections.
        /// </summary>
        public static void SendObjectToAllConnections([NotNull] QNetObjectBehaviour qNetObject)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("This methods can only be used by server.");
            if (qNetObject == null) throw new ArgumentNullException(nameof(qNetObject));
            QNetManager.Server.SendToAll(QNetUnityLocalChannel.OBJECT_QUERY, QNetMessageMethod.ReliableOrdered,
                ResolveObjectCreateMessage(qNetObject));
        }

        /// <summary>
        ///     Sends all QNetObjects in map to given connection.
        /// </summary>
        public static void SendAllObjectsToConnection(QNetConnection connection)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("This methods can only be used by server.");
            for (var index = 0; index < QNetObjectBehaviour.PredefinedBehaviours.Length; index++)
            {
                var qNetObject = QNetObjectBehaviour.PredefinedBehaviours[index];
                SendObjectToConnection(connection, qNetObject);
            }

            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var qNetObject = QNetObjectBehaviour.SpawnedBehaviours[index];
                SendObjectToConnection(connection, qNetObject);
            }
        }

        /// <summary>
        ///     Resolves object message to send to connection.
        /// </summary>
        private static QNetMessageWriter ResolveObjectCreateMessage(QNetObjectBehaviour qNetObject)
        {
            var writer = QNetManager.Server.GenerateOutgoingMessage(QNetUnityLocalHeader.OBJECT_CREATE);

            // write base message
            writer.WriteMessage(new QNetObjectSerialized
            {
                ObjectIdentity = qNetObject.ObjectIdentity,
                PrefabIdentity = qNetObject.Prefab?.PrefabIdentity ?? 0,
                OwnerIdentity = qNetObject.OwnerIdentity,
                Position = qNetObject.transform.position,
                Rotation = qNetObject.transform.rotation,
                Scale = qNetObject.transform.localScale
            });

            // and the some custom stuff of object
            qNetObject.SerializeServerObjectState(writer);

            return writer;
        }
    }
}