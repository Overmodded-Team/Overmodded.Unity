//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.Messages;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     QNet server prepare event.
    /// </summary>
    public delegate void QNetServerPrepare();

    /// <summary>
    ///     QNet server new player received event.
    /// </summary>
    public delegate void QNetServerNewPlayer(QNetPlayer player, QNetMessageReader reader);

    /// <summary>
    ///     QNet server player lost event.
    /// </summary>
    public delegate void QNetServerPlayerLost(QNetPlayer player, string reason);

    /// <summary>
    ///     QNet server shutdown event.
    /// </summary>
    public delegate void QNetServerShutdown(bool hostWasActive);

    /// <summary>
    /// </summary>
    public delegate void QNetServerAuthorizePlayer(QNetConnection connection, QNetMessageWriter writer, ref bool refuse);

    /// <summary>
    /// </summary>
    public delegate void QNetServerStarted();

    /// <summary>
    ///     QNet client prepare event.
    /// </summary>
    public delegate void QNetClientPrepare(out string nickname, out uint token);

    /// <summary>
    ///     QNet client ready event.
    /// </summary>
    public delegate void QNetClientReady(QNetMessageReader reader, QNetMessageWriter writer);

    /// <summary>
    ///     QNet client disconnection event.
    /// </summary>
    public delegate void QNetClientDisconnected(bool hostWasActive, bool lostConnection, string reason);

    /// <summary>
    ///     QNet register headers.
    /// </summary>
    public delegate void QNetRegisterHeaders();

    public partial class QNetManager
    {
        /// <summary>
        ///     QNet server prepare event.
        /// </summary>
        public static QNetServerPrepare OnServerPrepare;

        /// <summary>
        ///     QNet server new player event.
        /// </summary>
        public static QNetServerNewPlayer OnServerNewPlayer;

        /// <summary>
        ///     QNet server player lost event.
        /// </summary>
        public static QNetServerPlayerLost OnServerPlayerLost;

        /// <summary>
        ///     QNet server shutdown event.
        /// </summary>
        public static QNetServerShutdown OnServerShutdown;

        /// <summary>
        /// </summary>
        public static QNetServerAuthorizePlayer OnServerAuthorizePlayer;

        /// <summary>
        /// </summary>
        public static QNetServerStarted OnServerStarted;

        /// <summary>
        ///     QNet client prepare event.
        /// </summary>
        public static QNetClientPrepare OnClientPrepare;

        /// <summary>
        ///     QNet client ready event.
        /// </summary>
        public static QNetClientReady OnClientReady;

        /// <summary>
        ///     QNet client disconnection event.
        /// </summary>
        public static QNetClientDisconnected OnClientDisconnected;

        /// <summary>
        ///     QNet server register headers.
        /// </summary>
        public static QNetRegisterHeaders OnServerRegisterHeaders;

        /// <summary>
        ///     QNet client register headers.
        /// </summary>
        public static QNetRegisterHeaders OnClientRegisterHeaders;
    }
}
