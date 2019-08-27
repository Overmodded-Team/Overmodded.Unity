//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.IO;
using JEM.Core.Configuration;
using JEM.Core.Debugging;

namespace JEM.UnityEngine.Resource
{
    /// <summary>
    ///     Configuration of game resources system.
    /// </summary>
    public class JEMGameResourcesConfiguration
    {
        /// <summary>
        ///     Extension of every resources pack (description) in game.
        /// </summary>
        public string PackDescriptionExtension = ".txt";

        /// <summary>
        ///     Path to directory of resources packs.
        /// </summary>
        public string PackDirectory = @"\Data\Packs";

        /// <summary>
        ///     Extension of every resources pack in game.
        /// </summary>
        public string PackExtension = ".sarp";

        /// <summary>
        ///     Currently loaded configuration of game resources.
        /// </summary>
        public static JEMGameResourcesConfiguration Current { get; private set; }

        /// <summary>
        ///     Loads configuration of game resources.
        /// </summary>
        public static void Load()
        {
            JEMLogger.Log("System is loading game resources configuration.");

            var resourcesConfPath = JEMConfiguration.ResolveFilePath("jem_resources");
            if (!File.Exists(resourcesConfPath))
            {
                Current = new JEMGameResourcesConfiguration();
                JEMConfiguration.WriteData(resourcesConfPath, Current, JEMConfigurationSaveMethod.JSON);
                JEMLogger.LogWarning("Unable to find resources configuration file, system will create new one.");
            }
            else
            {
                Current = JEMConfiguration.LoadData<JEMGameResourcesConfiguration>(resourcesConfPath,
                    JEMConfigurationSaveMethod.JSON);
            }

            JEMLogger.Log("Resources configuration loading process is done.");
        }

        /// <summary>
        ///     Gets current configuration.
        /// </summary>
        public static JEMGameResourcesConfiguration Get()
        {
            if (Current == null)
                Load();
            return Current;
        }
    }
}