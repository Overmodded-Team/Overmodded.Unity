//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Collections.Generic;

namespace JEM.UnityEditor.AssetBundles
{
    /// <summary>
    ///     Asset builder package.
    /// </summary>
    public class JEMAssetBuilderPackage
    {
        /// <summary>
        ///     Package asset.
        /// </summary>
        public class Asset
        {
            /// <summary>
            ///     Include flag.
            /// </summary>
            public bool Include;

            /// <summary>
            ///     Path to asset.
            /// </summary>
            public string Path;
        }

        /// <summary>
        ///     Assets of package.
        /// </summary>
        public List<Asset> Assets;

        /// <summary>
        ///     Directory of package.
        /// </summary>
        public string Directory;

        /// <summary>
        ///     Name of package.
        /// </summary>
        public string Name;

        /// <summary>
        ///     Gets file of package.
        /// </summary>
        public string GetFile()
        {
            return $"{Directory}/{Name}{JEMAssetsBuilderConfiguration.GetExtension()}";
        }

        /// <summary>
        ///     Adds new asset.
        /// </summary>
        /// <param name="assetPath"></param>
        public void AddAsset(string assetPath)
        {
            if (Exist(assetPath))
                return;

            Assets.Add(new Asset
            {
                Path = assetPath,
                Include = true
            });
        }

        /// <summary>
        ///     Checks if asset of given path exists in this package.
        /// </summary>
        public bool Exist(string assetPath)
        {
            foreach (var asset in Assets)
                if (asset.Path == assetPath)
                    return true;

            return false;
        }
    }
}