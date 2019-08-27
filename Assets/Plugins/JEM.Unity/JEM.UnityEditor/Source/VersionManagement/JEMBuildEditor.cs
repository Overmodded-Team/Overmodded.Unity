//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.IO;

namespace JEM.UnityEditor.VersionManagement
{
    /// <summary>
    ///     Data about current build and compilation number.
    /// </summary>
    public static class JEMBuildEditor
    {
        /// <summary>
        ///     Path to file of current contributor username.
        /// </summary>
        public static string UserNameFile => $@"{Environment.CurrentDirectory}\username.txt";

        /// <summary>
        ///     Check if username file exists.
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentContributorNameFileExists()
        {
            return File.Exists(UserNameFile);
        }

        /// <summary>
        ///     Resolves name of current contributor.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static string ResolveCurrentContributorName()
        {
            if (File.Exists(UserNameFile)) return File.ReadAllLines(UserNameFile)[0];

            throw new NullReferenceException(
                $"System was unable to resolve username.txt file. Please, create {UserNameFile}");
        }
    }
}