//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using JEM.Core.Debugging;
using JEM.QNet.Messages;

namespace JEM.QNet.UnityEngine.Handlers
{
    internal static class QNetHandlerPlayerReceiver
    {
        internal static void OnPlayerCreate(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            JEMLogger.Log("QNet is receiving player create event.");

            // de-serialized message
            var serializedPlayer = reader.ReadMessage<QNetPlayerSerialized>();

            // resolve qNetPlayer       
            var qNetPlayer = QNetPlayer.GetQNetPlayer(serializedPlayer.ConnectionIdentity);
            if (qNetPlayer == null)
            {
                if (QNetManager.IsHostActive)
                    throw new InvalidOperationException(
                        $"Host is unable to GetQNetPlayer by identity {serializedPlayer.ConnectionIdentity}.");
                qNetPlayer =
                    QNetPlayer.CreateQNetPlayer(serializedPlayer.ConnectionIdentity, serializedPlayer.NickName, 0);
            }

            if (QNetManager.Client.ConnectionIdentity == serializedPlayer.ConnectionIdentity)
            {
                // do something with local player
                // we can't TagAsReady because WORLD_SERIALIZED message already do this
            }
            else
            {
                // tag player as ready
                qNetPlayer.TagAsReady();
            }
        }

        internal static void OnPlayerDelete(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            JEMLogger.Log("QNet is receiving player delete event.");
            var connectionIdentity = reader.ReadInt16();
            var player = QNetPlayer.GetQNetPlayer(connectionIdentity);
            if (player == null)
                throw new InvalidOperationException("InvalidConnectionIdentityError");

            player.TagAsNotReady();
            QNetPlayer.DestroyQNetPlayer(player);
        }

        internal static void OnPlayerQuery(QNetMessage message, QNetMessageReader reader, ref bool disallowRecycle)
        {
            JEMLogger.Log("QNet is receiving player query event.");
        }
    }
}