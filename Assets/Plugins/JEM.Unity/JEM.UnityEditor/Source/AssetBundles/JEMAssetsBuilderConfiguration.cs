//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Configuration;
using System;

namespace JEM.UnityEditor.AssetBundles
{
    /// <summary>
    ///     Assets builder configuration.
    /// </summary>
    public class JEMAssetsBuilderConfiguration
    {
        /// <summary>
        ///     Current extension of packages.
        /// </summary>
        public string PackageExtension = "bundle";

        private JEMAssetsBuilderConfiguration() { }

        /// <summary>
        ///     Gets current extension.
        /// </summary>
        /// <returns></returns>
        public static string GetExtension()
        {
            return $".{Configuration.PackageExtension}";
        }

        /// <summary>
        ///     Loads current configuration.
        /// </summary>
        public static void LoadConfiguration()
        {
            Configuration =
                JEMConfiguration.LoadData<JEMAssetsBuilderConfiguration>(ConfigurationFile,
                    JEMConfigurationSaveMethod.JSON);
        }

        /// <summary>
        ///     Saves current configuration.
        /// </summary>
        public static void SaveConfiguration()
        {
            if (Configuration == null)
                throw new NullReferenceException($"{nameof(Configuration)} is null.");

            JEMConfiguration.WriteData(ConfigurationFile, Configuration, JEMConfigurationSaveMethod.JSON);
        }

        /// <summary>
        ///     Current configuration instance.
        /// </summary>
        public static JEMAssetsBuilderConfiguration Configuration { get; private set; }

        /// <summary>
        ///     Patch to asset builder configuration
        /// </summary>
        public static string ConfigurationFile => JEMConfiguration.ResolveJEMFilePath("AssetBuilderConfiguration");
    }
}