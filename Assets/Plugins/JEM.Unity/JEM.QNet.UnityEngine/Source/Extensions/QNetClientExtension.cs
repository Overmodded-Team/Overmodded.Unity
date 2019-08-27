//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Extensions
{
    internal static class QNetClientExtension
    {
        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetClient client, QNetLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header, params object[] args)
        {
            client.Send((byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to server.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header">Header.</param>
        public static void Send(this QNetClient client, QNetLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            client.Send((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to server.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetClient client, QNetLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            client.Send((byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetClient client, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header, params object[] args)
        {
            client.Send((byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to server.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header">Header.</param>
        public static void Send(this QNetClient client, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            client.Send((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to server.
        /// </summary>
        /// <param name="client">The Client.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetClient client, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            client.Send((byte) channel, method, writer);
        }
    }
}