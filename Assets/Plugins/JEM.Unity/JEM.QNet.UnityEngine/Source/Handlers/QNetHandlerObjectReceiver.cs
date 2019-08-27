//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.Entities;
using JEM.QNet.UnityEngine.World;
using System;

namespace JEM.QNet.UnityEngine.Handlers
{
    internal static class QNetHandlerObjectReceiver
    {
        internal static void OnServerObjectCreate(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // write new object in to serializer
            var obj = reader.ReadMessage<QNetObjectSerialized>();
            //JEMLogger.Log($"Local QNet received object create message. PrefabIdentity -> {obj.PrefabIdentity}, ObjectIdentity -> {obj.ObjectIdentity}, OwnerIdentity -> {obj.OwnerIdentity} at position -> {obj.Position}");
            QNetWorldSerializer.WriteSerializedObjectToMemory(new QNetWorldSerializerObject
            {
                Object = obj,
                SerializedServerState = reader
            });

            // disallow to recycle this message
            disallowRecycle = true;
        }

        internal static void OnServerObjectDelete(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            // read object identity to remove
            var objectIdentity = reader.ReadInt16();
            // and do it!
            QNetWorldSerializer.RemoveSerializedObjectFromMemory(objectIdentity);
        }

        internal static void OnServerObjectState(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            if (!QNetWorldSerializer.WorldIsSerialized)
                return; // system can't receive query data while initializing,

            var objectIdentity = reader.ReadInt16();
            var qNetObject = QNetObjectBehaviour.GetObject(objectIdentity);
            if (qNetObject == null)
            {
                if (QNetManager.PrintNetworkWarnings)
                    JEMLogger.LogWarning($"Local machine received QNetEntity state update message but object of identity {objectIdentity} not exists in local world.");
                return;
            }

            qNetObject.DeSerializeServerState(reader);
        }

        public static void OnClientEntityQuery(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            if (!QNetWorldSerializer.WorldIsSerialized)
                return; // system can't receive entity query data while initializing,

            var objectIdentity = reader.ReadInt16();
            var qNetObject = QNetObjectBehaviour.GetObject(objectIdentity);
            if (qNetObject == null)
            {
                if (QNetManager.PrintNetworkWarnings)
                    JEMLogger.LogWarning(
                        $"Local machine received QNetEntity query message but object of identity {objectIdentity} not exists in local world.");
                return;
            }

            var entity = qNetObject.GetComponent<QNetEntity>();
            if (entity == null)
                throw new NullReferenceException(
                    $"QNetEntity query target exists but does not have {nameof(QNetEntity)} based script.");

            if (message.IsClientMessage)
            {
                QNetSimulation.ReceivedServerFrame = reader.ReadUInt32();
                QNetSimulation.AdjustServerFrames = QNetSimulation.ReceivedServerFrame > QNetTime.ServerFrame;
            }

            var index = reader.ReadByte();
            entity.InvokeNetworkMessage(index, reader);
        }
    }
}