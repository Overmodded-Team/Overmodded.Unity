//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEditor.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace JEM.UnityEditor.VersionManagement
{
    /// <summary>
    ///     Build flag.
    /// </summary>
    public struct BuildFlag
    {
        /// <summary>
        ///     Name of flag.
        /// </summary>
        public string Name;

        /// <summary>
        ///     Flag enable state.
        /// </summary>
        public bool Enabled;

        /// <summary>
        ///     Creates new instance of this flag.
        /// </summary>
        public BuildFlag New()
        {
            return new BuildFlag
            {
                Name = Name,
                Enabled = Enabled
            };
        }
    }

    /// <summary>
    ///     Compilation flags.
    /// </summary>
    public static class JEMBuildFlags
    {
        /// <summary>
        ///     Current flags.
        /// </summary>
        public static List<BuildFlag> Flags { get; } = new List<BuildFlag>();

        /// <summary>
        ///     Local file where flags are stored.
        /// </summary>
        public static string File => $@"{Environment.CurrentDirectory}\JEM\Compilation\Flags.txt";

        /// <summary>
        ///     Loads flags from local file.
        /// </summary>
        public static void LoadFlags()
        {
            Flags.Clear();

            if (!System.IO.File.Exists(File))
            {
                SaveFlags();
            }
            else
            {
                var text = System.IO.File.ReadAllText(File);
                var data = JsonConvert.DeserializeObject<SavedBuildFlags>(text);
                Flags.AddRange(data.Flags);
            }
        }

        /// <summary>
        ///     Saves flags to local file.
        /// </summary>
        public static void SaveFlags()
        {
            var directory = Path.GetDirectoryName(File);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            System.IO.File.WriteAllText(File, JsonConvert.SerializeObject(new SavedBuildFlags
            {
                Flags = Flags.ToArray()
            }));
        }

        private static BuildFlag GetFlag(string flagName)
        {
            int index;
            return GetFlag(flagName, out index);
        }

        private static BuildFlag GetFlag(string flagName, out int index)
        {
            index = -1;
            for (var i = 0; i < Flags.Count; i++)
            {
                var flag = Flags[i];
                if (flag.Name == flagName)
                {
                    index = i;
                    return flag;
                }
            }

            return default(BuildFlag);
        }

        /// <summary>
        ///     Adds flag of given name.
        /// </summary>
        public static bool AddFlag(string flag)
        {
            if (!GetFlag(flag).Equals(default(BuildFlag)))
                return false;

            Flags.Add(new BuildFlag
            {
                Name = flag,
                Enabled = true
            });

            return true;
        }

        /// <summary>
        ///     Removes flag of given name.
        /// </summary>
        public static void RemoveFlag(string flag)
        {
            var flagData = GetFlag(flag);
            if (flagData.Equals(default(BuildFlag)))
                return;
            Flags.Remove(flagData);
        }

        /// <summary>
        ///     Sets active state of given flag.
        /// </summary>
        public static void SetFlagActive(string flag, bool activeState)
        {
            var flagData = GetFlag(flag, out var index);
            if (flagData.Equals(default(BuildFlag)))
                return;

            var newFlag = flagData.New();
            newFlag.Enabled = activeState;
            Flags[index] = newFlag;
        }

        /// <summary>
        ///     Apply current flags to unity.
        /// </summary>
        public static void ApplyFlags()
        {
            if (!(JEMEditorConfiguration.Configuration?.UpdateFlags ?? false))
                return;

            var defines = string.Empty;
            foreach (var flag in Flags)
            {
                if (!flag.Enabled)
                    continue;
                var empty = string.IsNullOrEmpty(defines);
                if (!empty)
                    defines += ";";
                defines += $"{flag.Name}";
            }

            //JEMLogger.LogWarning($"JEM is applying scripting define symbols -> `{defines}`");
            InternalApplyFlags(BuildTargetGroup.Standalone, defines);
            InternalApplyFlags(BuildTargetGroup.Android, defines);
            InternalApplyFlags(BuildTargetGroup.WebGL, defines);
            InternalApplyFlags(BuildTargetGroup.Switch, defines);
        }

        private static void InternalApplyFlags(BuildTargetGroup group, string defines)
        {
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            if (currentDefines == defines)
                return;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines);
        }

        [Serializable]
        private class SavedBuildFlags
        {
            public BuildFlag[] Flags;
        }
    }
}