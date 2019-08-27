//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Messages;

namespace JEM.QNet.UnityEngine.Extensions
{
    internal static class QNetServerExtension
    {
        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetLocalChannel channel,
            QNetMessageMethod method, QNetUnityLocalHeader header, params object[] args)
        {
            server.Send(connection, (byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetLocalChannel channel,
            QNetMessageMethod method, QNetUnityLocalHeader header)
        {
            server.Send(connection, (byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetLocalChannel channel,
            QNetMessageMethod method, QNetMessageWriter writer)
        {
            server.Send(connection, (byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header">Header</param>
        public static void SendToAll(this QNetServer server, QNetLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            server.SendToAll((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void SendToAll(this QNetServer server, QNetLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            server.SendToAll((byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="except">Message will be send to all connections except this.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void SendToAll(this QNetServer server, QNetConnection except, QNetLocalHeader channel,
            QNetMessageMethod method, QNetMessageWriter writer)
        {
            server.SendToAll(except, (byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        /// <param name="args">Additional parameters.</param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetUnityLocalChannel channel,
            QNetMessageMethod method, QNetUnityLocalHeader header, params object[] args)
        {
            server.Send(connection, (byte) channel, method, (ushort) header, args);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header"></param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetUnityLocalChannel channel,
            QNetMessageMethod method, QNetUnityLocalHeader header)
        {
            server.Send(connection, (byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="connection">Target connection of message.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void Send(this QNetServer server, QNetConnection connection, QNetUnityLocalChannel channel,
            QNetMessageMethod method, QNetMessageWriter writer)
        {
            server.Send(connection, (byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="header">Header</param>
        public static void SendToAll(this QNetServer server, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetUnityLocalHeader header)
        {
            server.SendToAll((byte) channel, method, (ushort) header);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void SendToAll(this QNetServer server, QNetUnityLocalChannel channel, QNetMessageMethod method,
            QNetMessageWriter writer)
        {
            server.SendToAll((byte) channel, method, writer);
        }

        /// <summary>
        ///     Sends message to given connection.
        /// </summary>
        /// <param name="server">Our Server.</param>
        /// <param name="except">Message will be send to all connections except this.</param>
        /// <param name="channel">Channel of message.</param>
        /// <param name="method">Method of message sending.</param>
        /// <param name="writer">Message to send.</param>
        public static void SendToAll(this QNetServer server, QNetConnection except, QNetUnityLocalChannel channel,
            QNetMessageMethod method, QNetMessageWriter writer)
        {
            server.SendToAll(except, (byte) channel, method, writer);
        }
    }
}