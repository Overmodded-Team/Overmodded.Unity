//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Attribute;
using UnityEngine;

namespace JEM.QNet.UnityEngine.Behaviour
{
    /// <inheritdoc />
    /// <summary>
    ///     QNet object prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "New Prefab", menuName = "QNet/New Prefab", order = 1)]
    public class QNetObjectPrefab : ScriptableObject
    {
        /// <summary>
        ///     Our prefab.
        /// </summary>
        public QNetObjectBehaviour Prefab;

        /// <summary>
        ///     Network identity of prefab.
        /// </summary>
        [JEMReadOnly] public short PrefabIdentity;
    }
}