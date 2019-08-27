//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Debugging;
using JEM.UnityEngine.Extension;
using JEM.UnityEngine.Interface;
using JEM.UnityEngine.Resource;
using System.Diagnostics;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     Some main methods related to UnityEngine and JEM for UnityEngine.
    /// </summary>
    public static class JEMUnity
    {
        /// <summary>
        ///     Prepares JEM to work.
        ///     It mainly creates all scripts that are used by systems and need to use IEnumenator.
        /// </summary>
        public static void PrepareJEM()
        {
            var sw = Stopwatch.StartNew();
            JEMLogger.Init();

            JEMOperation.RegenerateLocalScript();

            JEMGameResources.RegenerateLocalScript();

            JEMInterfaceFadeElement.RegenerateLocalScript();
            JEMInterfaceFadeAnimation.RegenerateLocalScript();

            JEMTranslatorScript.RegenerateLocalScript();

            JEMExtensionAudioSource.RegenerateLocalScript();
            JEMExtensionGameObject.RegenerateLocalScript();
            JEMExtensionText.RegenerateLocalScript();
            sw.Stop();
            JEMLogger.InternalLog($"JEM prepare took {sw.Elapsed.TotalSeconds:0.00} seconds.");
        }
    }
}