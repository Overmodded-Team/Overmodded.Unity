//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Resource
{
    /// <summary>
    ///     Game asset structure that can be get from GameResourcesPack via GetAsset method.
    /// </summary>
    public struct JEMGameAsset<T> where T : Object
    {
        /// <summary>
        ///     Name of the asset.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Asset object.
        /// </summary>
        public Object Asset { get; }

        /// <summary>
        ///     Source pack of this asset.
        /// </summary>
        public JEMGameResourcePack Pack { get; }

        /// <summary>
        ///     Creates new instance of game asset.
        /// </summary>
        public JEMGameAsset(string name, Object asset, JEMGameResourcePack pack)
        {
            Name = name;
            Asset = asset;
            Pack = pack;
        }

        /// <summary>
        ///     Gets asset via given type.
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        public T Get()
        {
            return (T) Asset;
        }
    }
}