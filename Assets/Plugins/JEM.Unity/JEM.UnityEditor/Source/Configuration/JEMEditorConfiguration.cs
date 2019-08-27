//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Configuration;
using JEM.Core.Debugging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace JEM.UnityEditor.Configuration
{
    /// <summary>
    ///     Internal JEM Configuration data used by JEMConfiguration.
    /// </summary>
    [Serializable]
    public class JEMEditorConfiguration
    {
        /// <summary>
        ///     Defines whether the PlayerSettings.bundleVersion should be updated by system.
        /// </summary>
        public bool UpdateBundleVersion = true;

        /// <summary>
        ///     Update work time.
        /// </summary>
        public bool UpdateWorkTime = true;

        /// <summary>
        ///     Enables/Disables operations on SetScriptingDefineSymbolsForGroup by JEMBuildFlags.
        /// </summary>
        public bool UpdateFlags = true;

        private JEMEditorConfiguration()
        {
            // private
        }

        /// <summary>
        ///     Currently loaded configuration of JEMConfiguration class.
        /// </summary>
        public static JEMEditorConfiguration Configuration { get; private set; }

        /// <summary>
        ///     Resolves file name of internal cfg.
        /// </summary>
        /// <returns></returns>
        private static string ResolveConfigurationFile()
        {
            var file = $@"{JEMConfiguration.CurrentDirectory}\JEM\JEMEditorConfiguration.json";
            var dir = Path.GetDirectoryName(file);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return file;
        }

        /// <summary>
        ///     Loads current configuration.
        /// </summary>
        public static JEMEditorConfiguration Load()
        {
            var file = ResolveConfigurationFile();
            if (File.Exists(file))
            {
                Configuration = JsonConvert.DeserializeObject<JEMEditorConfiguration>(File.ReadAllText(file));
                if (Configuration != null)
                    JEMLogger.InternalLog($"JEMEditorInternalCfg loaded data from {file}");
                else
                    JEMLogger.InternalLog($"Unable to load JEMInternalCfg from file {file}");
                return Configuration;
            }

            Configuration = new JEMEditorConfiguration();
            Save();
            return Configuration;
        }

        /// <summary>
        ///     Saves current configuration.
        /// </summary>
        public static void Save()
        {
            var file = ResolveConfigurationFile();
            JEMLogger.InternalLog($"Saving JEMEditorInternalCfg data at {file}");
            File.WriteAllText(file, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }
    }
}