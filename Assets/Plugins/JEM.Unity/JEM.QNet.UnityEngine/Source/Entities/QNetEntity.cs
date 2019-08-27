//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;
using JEM.QNet.UnityEngine.Behaviour;
using JEM.QNet.UnityEngine.Messages;
using System;
using System.Collections.Generic;

namespace JEM.QNet.UnityEngine.Entities
{
    /// <summary>
    ///     QNetEntity's network incoming message data.
    /// </summary>
    public struct QNetEntityIncomingMessage
    {
        /// <summary>
        ///     Index of message.
        /// </summary>
        public byte MessageIndex;

        /// <summary>
        ///     QNet message reader.
        /// </summary>
        public QNetMessageReader Reader;

        /// <summary>
        ///     Defines whether the source of message is a client.
        /// </summary>
        public bool IsClient;

        /// <summary>
        ///     Defines whether the source of message is a server.
        /// </summary>
        public bool IsServer;

        /// <summary>
        ///     Defines whether  the source of message is a owner of object.
        /// </summary>
        public bool IsOwner;

        /// <summary>
        /// Gets QNetPlayer using Reader's Connection.
        /// </summary>
        public QNetPlayer GetPlayer()
        {
            var player = QNetPlayer.GetQNetPlayer(Reader.Connection);
            return player;
        }
    }

    /// <summary>
    ///     QNetEntity's network outgoing message data.
    /// </summary>
    public struct QNetEntityOutgoingMessage
    {
        /// <summary>
        ///     Source entity of this message.
        /// </summary>
        public QNetEntity Entity { get; }

        /// <summary>
        ///     Message is send from server.
        /// </summary>
        public bool FromServer { get; }

        /// <summary>
        ///     Message is send from client.
        /// </summary>
        public bool FromClient { get; }

        /// <summary>
        ///     Writer of message.
        /// </summary>
        public QNetMessageWriter Writer { get; }

        /// <summary>
        ///     Target connection of message.
        /// </summary>
        public QNetConnection Target { get; }

        /// <summary>
        ///     Network channel.
        /// </summary>
        public byte Channel { get; set; }

        /// <summary>
        ///     QNet entity outgoing message constructor.
        /// </summary>
        public QNetEntityOutgoingMessage(QNetEntity entity, bool isServer, QNetConnection target)
        {
            Entity = entity;
            FromServer = isServer;
            FromClient = !isServer;
            Target = target;
            Writer = isServer
                ? QNetManager.Server.GenerateOutgoingMessage((ushort) QNetUnityLocalHeader.ENTITY_QUERY)
                : QNetManager.Client.GenerateOutgoingMessage((ushort) QNetUnityLocalHeader.ENTITY_QUERY);
            Channel = (byte) QNetUnityLocalChannel.ENTITY_QUERY;

            Writer.WriteInt16(Entity.ObjectIdentity);
        }

        /// <summary>
        ///     Apply data and send to target peer.
        /// </summary>
        public void ApplyAndSend()
        {
            ApplyAndSend(QNetMessageMethod.Unknown);
        }

        /// <summary>
        ///     Apply data and send to target peer.
        /// </summary>
        public void ApplyAndSend(QNetMessageMethod sendMethod)
        {
            ApplyAndSend(new QNetConnection(), sendMethod);
        }

        /// <summary>
        ///     Apply data and send to target peer.
        /// </summary>
        public void ApplyAndSend(QNetConnection exclude, QNetMessageMethod sendMethod = QNetMessageMethod.Unreliable)
        {
            if (FromServer)
            {
                if (Target.Equals(default(QNetConnection)))
                    QNetManager.Server.SendToAll(exclude, Channel, sendMethod, Writer);
                else
                    QNetManager.Server.Send(Target, Channel, sendMethod, Writer);
            }
            else if (FromClient)
            {
                QNetManager.Client.Send(Channel, sendMethod, Writer);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    /// <summary>
    ///     QNetEntity's network message event.
    /// </summary>
    /// <param name="incomingMessage">Message data.</param>
    public delegate void QNetEntityNetworkMessage(QNetEntityIncomingMessage incomingMessage);

    /// <inheritdoc />
    /// <summary>
    ///     QNetEntity's base class.
    /// </summary>
    public abstract class QNetEntity : QNetObjectBehaviour
    {
        /// <summary>
        ///     List of registered network messages
        /// </summary>
        private Dictionary<byte, QNetEntityNetworkMessage> NetworkMessages { get; } =
            new Dictionary<byte, QNetEntityNetworkMessage>();

        /// <inheritdoc />
        public override void OnInternalSpawned()
        {
            // invoke base method
            base.OnInternalSpawned();

            // register network messages from there
            RegisterNetworkMessages();
        }

        /// <summary>
        ///     Registers entity's message event delegate.
        /// </summary>
        /// <param name="messageDelegate">Event delegate.</param>
        /// <returns>Index of this message delegate. Network message is sent based on this index.</returns>
        public byte RegisterNetworkMessage(QNetEntityNetworkMessage messageDelegate)
        {
            var index = (byte) NetworkMessages.Count;
            NetworkMessages.Add(index, messageDelegate);
            return index;
        }

        /// <summary>
        ///     Register network messages of this entity.
        /// </summary>
        protected virtual void RegisterNetworkMessages()
        {
            // here, register some entity network messages
        }

        /// <summary>
        ///     Sends network message from client to server.
        ///     Note that only owner of this entity can send messages.
        /// </summary>
        /// <param name="index">Index of message.</param>
        public QNetEntityOutgoingMessage SendNetworkClientMessage(byte index)
        {
            if (!QNetManager.IsClientActive)
                throw new InvalidOperationException("QNetEntity network client message can only be send from client.");

            var outgoingMessage = new QNetEntityOutgoingMessage(this, false, default(QNetConnection));
            outgoingMessage.Writer.WriteByte(index);
            return outgoingMessage;
        }

        /// <summary>
        ///     Sends network message from server to all clients.
        /// </summary>
        /// <param name="index">Index of message.</param>
        public QNetEntityOutgoingMessage SendNetworkServerMessage(byte index)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("QNetEntity network server message can only be send from server.");

            var outgoingMessage = new QNetEntityOutgoingMessage(this, true, default(QNetConnection));
            if (IsServer)
            {
                // write server frame
                outgoingMessage.Writer.WriteUInt32(QNetTime.ServerFrame);
            }

            outgoingMessage.Writer.WriteByte(index);
            return outgoingMessage;
        }

        /// <summary>
        ///     Sends network message from server to given connection.
        /// </summary>
        /// <param name="targetConnection">Target connection of message.</param>
        /// <param name="index">Index of message.</param>
        public QNetEntityOutgoingMessage SendNetworkServerDedicatedMessage(QNetConnection targetConnection, byte index)
        {
            if (!QNetManager.IsServerActive)
                throw new InvalidOperationException("QNetEntity network server message can only be send from server.");
            if (targetConnection.Equals(default(QNetConnection)))
                throw new InvalidOperationException("Default target connection received in server dedicated message.");

            var outgoingMessage = new QNetEntityOutgoingMessage(this, true, targetConnection);
            if (IsServer)
            {
                // write server frame
                outgoingMessage.Writer.WriteUInt32(QNetTime.ServerFrame);
            }

            outgoingMessage.Writer.WriteByte(index);
            return outgoingMessage;
        }

        /// <summary>
        ///     Share given message to rest of clients.
        /// </summary>
        public void ShareNetworkServerMessage(QNetEntityIncomingMessage incomingMessage)
        {
            if (!IsServer)
                throw new InvalidOperationException("QNet network server message can only be shared from server.");
            if (!incomingMessage.IsClient)
                throw new InvalidOperationException("Server can only share message send from client.");

            var outgoingMessage = SendNetworkServerMessage(incomingMessage.MessageIndex);
            outgoingMessage.Writer.WriteBytesRaw(incomingMessage.Reader.GetBytesRaw(5));
            outgoingMessage.ApplyAndSend(OwnerConnection);
        }

        /// <summary>
        ///     Invokes network messages.
        /// </summary>
        /// <param name="index">Index of message.</param>
        /// <param name="reader">Reader of message.</param>
        public void InvokeNetworkMessage(byte index, QNetMessageReader reader)
        {
            if (!NetworkMessages.ContainsKey(index))
                return; //  throw new InvalidOperationException($"QNetEntity of type {GetType()} does not have registered network message of identity {index}.");
                        // trowing a exception disallowing to register behaviours that relay on QNetEntity

            var isClient = reader.Connection.ConnectionIdentity != 0;
            var isServer = reader.Connection.ConnectionIdentity == 0 || QNetManager.IsHostActive &&
                           reader.Connection.ConnectionIdentity == QNetManager.Client.ConnectionIdentity;
            if (!QNetManager.IsServerActive && QNetManager.IsClientActive)
            {
                isClient = false;
                isServer = true;
            }

            var isOwner = reader.Connection.ConnectionIdentity == OwnerIdentity;

            NetworkMessages[index].Invoke(new QNetEntityIncomingMessage
            {
                MessageIndex = index,
                Reader = reader,
                IsClient = isClient,
                IsServer = isServer,
                IsOwner = isOwner
            });
        }
    }
}