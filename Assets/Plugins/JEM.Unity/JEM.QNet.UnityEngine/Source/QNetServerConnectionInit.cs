//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Extensions;
using JEM.QNet.UnityEngine.Messages;
using JEM.QNet.UnityEngine.World;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     Class that contains server connection initialization methods.
    /// </summary>
    public static class QNetServerConnectionInit
    {
        /// <summary>
        ///     Prepare new connection.
        ///     Preparing process includes all data that connection need to work with server and base map loading.
        ///     After preparing, connection will receive all QNetObjects that are currently created.
        /// </summary>
        public static void PrepareNewConnection(QNetConnection connection)
        {
            var qNetPlayer = QNetPlayer.GetQNetPlayer(connection.ConnectionIdentity);
            if (qNetPlayer == null)
            {
                JEMLogger.LogError("Newly received connection don't have his QNetPlayer instance. Disconnecting!");
                QNetManager.Server.CloseConnection(connection, "InternalQNetPlayerError");
            }
            else
            {
                JEMLogger.Log($"Preparing newly received connection called {qNetPlayer.Nickname}.");
                var writer = QNetManager.Server.GenerateOutgoingMessage((ushort) QNetUnityLocalHeader.LEVEL_LOADING);
                writer.WriteString(QNetLevelLoader.LevelName);
                QNetManager.Server.Send(connection, QNetLocalChannel.DEFAULT, QNetMessageMethod.ReliableOrdered,
                    writer);
            }
        }
    }
}