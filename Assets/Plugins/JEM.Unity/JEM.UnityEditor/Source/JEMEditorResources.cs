//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEditor;
using UnityEngine;

namespace JEM.UnityEditor
{
    /// <summary>
    ///     JEM Editor resources manager.
    /// </summary>
    internal static class JEMEditorResources
    {
        /// <summary>
        ///     Loads current resources.
        /// </summary>
        public static void Load(bool forced = false)
        {
            if (ResourcesLoaded && !forced)
                return;

            var progressBarTitle = "Loading JEM Editor Resources.";
            EditorUtility.DisplayProgressBar(progressBarTitle, "Getting icons", 0f);
            JEMEditorDownloader.NewRequest("https://i.imgur.com/MtJMl05.png", texture => { JEMIconTexture = texture; });
            EditorUtility.ClearProgressBar();

            ResourcesLoaded = true;
        }

        /// <summary>
        ///     JEM icon texture.
        /// </summary>
        public static Texture JEMIconTexture { get; private set; }

        /// <summary>
        ///     Defines whether the resources are loaded.
        /// </summary>
        public static bool ResourcesLoaded { get; private set; }
    }
}