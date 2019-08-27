//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.VersionManagement.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;

namespace JEM.UnityEngine.VersionManagement
{
    /// <summary>
    ///     Data about current build and compilation number.
    /// </summary>
    public static class JEMBuild
    {
        /// <summary>
        ///     Data about current compilation.
        /// </summary>
        public static JEMBuildCompilation LastCompilation { get; private set; }

        /// <summary>
        ///     General number of current compilation.
        /// </summary>
        public static int CompilationNumber { get; private set; }

        /// <summary>
        ///     General number of current build.
        /// </summary>
        public static int BuildNumber { get; private set; }

        /// <summary>
        ///     General session time.
        /// </summary>
        public static int SessionTime { get; private set; }

        /// <summary>
        ///     Data about current game version.
        /// </summary>
        public static Data.JEMBuildVersion Version { get; private set; }

        /// <summary>
        ///     Name of current version.
        /// </summary>
        public static string BuildVersion => Version == null ? "INTERNAL_LOAD_ERROR" : Version.VersionName;

        /// <summary>
        ///     Text of current version release date.
        /// </summary>
        public static string BuildRelease =>
            Version == null ? "INTERNAL_LOAD_ERROR" : $"{Version.VersionRelease:yyyy-MM-dd}";

        /// <summary>
        ///     Text of current compilation.
        /// </summary>
        public static string BuildCompilationText => LastCompilation == null
            ? "INTERNAL_LOAD_ERROR"
            : $"{BuildNumber}{(DrawCompilationNumber ? $".{CompilationNumber}" : string.Empty)}/{BranchName} {LastCompilation.BuildTime:yyyy.MM.dd HH:mm:ss}";

        /// <summary>
        ///     Ready build text.
        /// </summary>
        public static string BuildText => $"{BuildVersion} - {BuildRelease}\n{BuildCompilationText}";

        /// <summary>
        ///     Ready(Rich) build text.
        /// </summary>
        public static string BuildTextRich =>
            $"<size={RichTextSize}>{BuildVersion} - {BuildRelease}</size>\n{BuildCompilationText}";

        /// <summary>
        ///     Size of rich text.
        /// </summary>
        public static int RichTextSize = 17;

        /// <summary>
        ///     Defines whether the compilation number should be drawn.
        /// </summary>
        public static bool DrawCompilationNumber = true;

        /// <summary>
        /// Temporary text that defines name of current branch.
        /// </summary>
        public static string BranchName = "MAIN";

        /// <summary>
        ///     Initializes current game build.
        /// </summary>
        /// <returns></returns>
        public static void LoadBuild()
        {
            try
            {
                // get version
                var gameVersionAsset = (TextAsset) Resources.Load("Editor/Version", typeof(TextAsset));
                if (gameVersionAsset != null)
                    Version = JsonConvert.DeserializeObject<Data.JEMBuildVersion>(gameVersionAsset.text);
                if (Version == null)
                    Debug.LogError("Failed to load version data.");

                // get last contributor
                // last contributor is used to get time of build and last compilation
                var compilationText =
                    (TextAsset) Resources.Load($"Editor/Compilation/compNum_{ResolveLastContributorName()}",
                        typeof(TextAsset));
                if (compilationText != null)
                    LastCompilation = JsonConvert.DeserializeObject<JEMBuildCompilation>(compilationText.text);
                if (LastCompilation == null)
                    Debug.LogError("Failed to load compilation number.");

                CompilationNumber = ResolveCurrentCompilationNumber();
                BuildNumber = ResolveCurrentBuildNumber();
                SessionTime = ResolveCurrentSessionTime();
            }
            catch (Exception ex)
            {
                Debug.LogError("System was unable to initialize game build.");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// </summary>
        public static int ResolveCurrentCompilationNumber()
        {
            return (from TextAsset c in Resources.LoadAll("Editor/Compilation", typeof(TextAsset))
                where c.name.StartsWith("compNum_")
                select JsonConvert.DeserializeObject<JEMBuildCompilation>(c.text)
                into compilation
                where compilation != null
                select compilation.CompilationNumber).Sum();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static int ResolveCurrentBuildNumber()
        {
            return (from TextAsset c in Resources.LoadAll("Editor/Compilation", typeof(TextAsset))
                where c.name.StartsWith("compNum_")
                select JsonConvert.DeserializeObject<JEMBuildCompilation>(c.text)
                into compilation
                where compilation != null
                select compilation.BuildNumber).Sum();
        }

        /// <summary>
        /// </summary>
        public static int ResolveCurrentSessionTime()
        {
            return (from TextAsset c in Resources.LoadAll("Editor/Compilation", typeof(TextAsset))
                where c.name.StartsWith("compNum_")
                select JsonConvert.DeserializeObject<JEMBuildCompilation>(c.text)
                into compilation
                where compilation != null
                select compilation.SessionTime).Sum();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException"></exception>
        public static string ResolveLastContributorName()
        {
            var t = Resources.Load("Editor/Compilation/Last", typeof(TextAsset)) as TextAsset;
            if (t != null) return t.text;

            throw new NullReferenceException("System was unable to resolve last compilation username.");
        }
    }
}