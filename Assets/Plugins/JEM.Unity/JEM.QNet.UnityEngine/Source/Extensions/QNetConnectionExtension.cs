//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Extensions
{
    internal static class QNetConnectionExtension
    {
        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetConnection connection, QNetLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header, params object[] args)
        {
            connection.Send((byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        public static void Send(this QNetConnection connection, QNetLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            connection.Send((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetConnection connection, QNetLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            connection.Send((byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetConnection connection, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header, params object[] args)
        {
            connection.Send((byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        public static void Send(this QNetConnection connection, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            connection.Send((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetConnection connection, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            connection.Send((byte) channel, method, writer);
        }
    }
}