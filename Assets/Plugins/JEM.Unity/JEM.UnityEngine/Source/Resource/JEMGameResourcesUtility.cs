//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.IO;

namespace JEM.UnityEngine.Resource
{
    /// <summary>
    ///     Set of utility methods for game resources.
    /// </summary>
    public static class JEMGameResourcesUtility
    {
        /// <summary>
        ///     Converts for ex. C:/Resource Name.sarp in to resource_name
        /// </summary>
        public static string ResolveResourceNameFromPath(string resourceName)
        {
            return Path.GetFileNameWithoutExtension(resourceName)?.ToLower().Replace(" ", "_");
        }

        /// <summary>
        ///     Converts for ex. C:/Resource Name.sarp in to resource_name
        /// </summary>
        public static string ResolveResourceName(string resourceName)
        {
            return resourceName.ToLower().Replace(" ", "_");
        }

        /// <summary>
        ///     Gets full path to pack.
        /// </summary>
        /// <param name="packName">Raw name of pack (without extension)</param>
        public static string ResolveMapPath(string packName)
        {
            var cfg = JEMGameResourcesConfiguration.Get();
            return $@"{cfg.PackDirectory}\{packName}{cfg.PackExtension}";
        }

        /// <summary>
        ///     Gets full path to pack description file.
        /// </summary>
        /// <param name="packName">Raw name of pack (without extension)</param>
        public static string ResolveMapDescPath(string packName)
        {
            var cfg = JEMGameResourcesConfiguration.Get();
            return $@"{cfg.PackDirectory}\{packName}{cfg.PackDescriptionExtension}";
        }
    }
}