//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;

namespace JEM.QNet.UnityEngine.Game
{
    /// <summary>
    ///     Client loading state change delegate.
    /// </summary>
    public delegate void QNetClientLoadingChange();

    /// <summary>
    ///     Client loading info updated delegate.
    /// </summary>
    /// <param name="isLoading">Defines whether the game is loading or unloading.</param>
    /// <param name="titleText">Title of current action.</param>
    /// <param name="descriptionText">Description of current action.</param>
    public delegate void QNetClientLoadingInfoUpdated(bool isLoading, string titleText, string descriptionText);

    /// <summary>
    ///     Load client side content delegate.
    /// </summary>
    public delegate void QNetLoadClientSideContent();

    /// <summary>
    ///     QNet level load world event.
    /// </summary>
    public delegate void QNetLevelLoaded(string levelName);

    /// <summary>
    ///     System map state changed delegate.
    /// </summary>
    public delegate void QNetSystemMapStateChanged(QNetMapState mapState);

    /// <summary>
    ///     Unload client side content delegate.
    /// </summary>
    public delegate void QNetUnloadClientSideContent();

    /// <summary>
    ///     World and network is ready. Called on both client and server. (once on host)
    /// </summary>
    public delegate void QNetWorldAndNetworkReady(Action onDone);

    public partial class QNetGameInitializer
    {
        /// <summary>
        ///     Client loading stare change event.
        /// </summary>
        public static event QNetClientLoadingChange OnClientLoadingEnd;

        /// <summary>
        ///     Client loading info updated event.
        /// </summary>
        public static event QNetClientLoadingInfoUpdated OnClientLoadingInfoUpdated;

        /// <summary>
        ///     Client loading state change event.
        /// </summary>
        public static event QNetClientLoadingChange OnClientLoadingStart;

        /// <summary>
        ///     Load client side content event.
        ///     You can hook up to this event, that will for ex. enable in game interface.
        /// </summary>
        public static event QNetLoadClientSideContent OnLoadClientSideContent;

        /// <summary>
        ///     System map state changed event.
        /// </summary>
        public static event QNetSystemMapStateChanged OnMapStateChanged;

        /// <summary>
        ///     Server load saved world event.
        /// </summary>
        public static event QNetLevelLoaded OnLevelLoaded;

        /// <summary>
        ///     Unload client side content event.
        ///     You can hook up to this event, that will for ex. disable in-game interface and enable menu.
        /// </summary>
        public static event QNetUnloadClientSideContent OnUnloadClientSideContent;

        /// <summary>
        ///     World and network is ready. Called on both client and server. (once on host)
        /// </summary>
        public static event QNetWorldAndNetworkReady OnWorldAndNetworkReady;
    }
}
