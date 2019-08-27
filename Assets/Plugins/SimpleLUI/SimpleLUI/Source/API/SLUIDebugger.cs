//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace SimpleLUI.API
{
    /// <summary>
    ///     SLUI debugger class.
    ///     Contains few methods to debug your scripts.
    ///     Debugger class can be accesses in lua by using 'debug'.
    /// </summary>
    public class SLUIDebugger
    {
        internal SLUIWorker Parent { get; }

        internal SLUIDebugger(SLUIWorker parent)
        {
            Parent = parent;
        }

        /// <summary>
        ///     Prints unity's log.
        /// </summary>
        public void Log(string str) => Debug.Log($"SLUI ({Parent.Parent.Name}) Debugger :: {str}");

        /// <summary>
        ///     Prints unity's log warning.
        /// </summary>
        public void LogWarning(string str) => Debug.LogWarning($"SLUI ({Parent.Parent.Name}) Debugger :: {str}");

        /// <summary>
        ///     Prints unity's log error.
        /// </summary>
        public void LogError(string str) => Debug.LogError($"SLUI ({Parent.Parent.Name}) Debugger :: {str}");
    }
}
