//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace JEM.UnityEditor.AssetBundles
{
    /// <summary>
    ///     JEM Asset Builder item.
    /// </summary>
    [Serializable]
    public class JEMAssetBuilderItem
    {
        /// <summary>
        ///     GUID of target asset.
        /// </summary>
        public string Guid = string.Empty;

        /// <summary>
        ///     Defines if this asset should be included in AssetBundle.
        /// </summary>
        public bool Include = true;
    }

    /// <summary>
    ///     JEM Asset Builder package.
    /// </summary>
    [Serializable]
    public class JEMAssetBuilderPackage
    {
        /// <summary>
        ///     Name of the package.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        ///     List of assets added to this package.
        /// </summary>
        public List<JEMAssetBuilderItem> Assets = new List<JEMAssetBuilderItem>();

        /// <summary>
        ///     Gets full path to file of this package.
        /// </summary>
        public string GetFile() => $"{JEMAssetsBuilderConfiguration.GetDirectory()}\\{Name}{JEMAssetsBuilderConfiguration.GetExtension()}";

        /// <summary>
        ///     Gets full path to package configuration file.
        /// </summary>
        public string GetConfigurationFile() => $@"{JEMAssetBuilderWindow.PackagesConfigurationDirectory}\{Name}.json";

        /// <summary>
        ///     Returns array of paths to all assets added to this Package.
        /// </summary>
        public string[] GetPathToAssets() => Assets.Select(asset => AssetDatabase.GUIDToAssetPath(asset.Guid)).ToArray();     

        /// <summary>
        ///     Adds new asset to this package.
        /// </summary>
        public void AddAsset([NotNull] global::UnityEngine.Object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var objGuid = GetObjectGUID(obj);
            if (Exist(objGuid))
                return;

            var newAsset = new JEMAssetBuilderItem
            {
                Guid = objGuid,
                Include = true
            };

            Assets.Add(newAsset);
        }

        /// <summary>
        ///     Checks if given asset is added to this package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public bool Exist([NotNull] global::UnityEngine.Object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var guid = GetObjectGUID(obj);
            return Assets.Any(asset => asset.Guid == guid);
        }

        /// <summary>
        ///     Checks if asset of given GUID is added to this package.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public bool Exist([NotNull] string guid)
        {
            if (guid == null) throw new ArgumentNullException(nameof(guid));
            return Assets.Any(asset => asset.Guid == guid);
        }

        /// <exception cref="NullReferenceException">Received when AssetDatabase fails to find GUID of target Object.</exception>
        /// <exception cref="ArgumentNullException"/>
        internal static string GetObjectGUID([NotNull] global::UnityEngine.Object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out var guid, out long _))
            {
                return guid;
            }

            throw new NullReferenceException($"We failed to get a GUID of Object of type {obj.GetType()}");
        }
    }
}