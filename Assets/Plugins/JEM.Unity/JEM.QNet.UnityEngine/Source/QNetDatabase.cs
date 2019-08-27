//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using JEM.QNet.UnityEngine.Behaviour;
using UnityEngine;

namespace JEM.QNet.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     QNet peer database.
    ///     Database contains all data that game's networking generates an uses.
    ///     Database also contains all QNetObjectPrefabs and Scripts that can be spawned by QNet.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [CreateAssetMenu(fileName = "New Prefab Database", menuName = "QNet/New Prefab Database", order = 1)]
    public class QNetDatabase : ScriptableObject
    {
        /// <summary>
        ///     Prefab of player.
        /// </summary>
        [Header("Prefabs Settings")] public QNetObjectPrefab PlayerPrefab;

        /// <summary>
        ///     All network prefabs of the game.
        /// </summary>
        [SerializeField] public QNetObjectPrefab[] Prefabs;

        /// <summary>
        ///     Gets prefab of given identity.
        /// </summary>
        public QNetObjectPrefab GetPrefab(short prefabIdentity)
        {
            if (prefabIdentity == 0)
                throw new ArgumentOutOfRangeException(nameof(prefabIdentity), prefabIdentity,
                    "Prefab identity can't equals zero.");
            return Prefabs.FirstOrDefault(prefab => prefab != null && prefab.PrefabIdentity == prefabIdentity);
        }

        /// <summary>
        ///     Gets prefab of given name;
        /// </summary>
        public QNetObjectPrefab GetPrefab([NotNull] string prefabName)
        {
            if (prefabName == null) throw new ArgumentNullException(nameof(prefabName));
            return Prefabs.FirstOrDefault(prefab => prefab != null && prefab.name == prefabName);
        }
    }
}