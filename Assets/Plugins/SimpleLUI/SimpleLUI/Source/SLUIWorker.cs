//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using NLua;
using SimpleLUI.API;
using SimpleLUI.API.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleLUI
{
    /// <summary/>
    public class SLUIWorker
    {
        internal SLUIDebugger Debugger { get; }
        internal SLUICore Core { get; }

        internal SLUIManager Parent { get; }
        internal SLUIWorker(SLUIManager parent)
        {
            Parent = parent;

            Debugger = new SLUIDebugger(this);
            Core = new SLUICore(this);
        }

        internal void PrepareState(Lua state)
        {
            state.LoadCLRPackage();
            state["debug"] = Debugger;
            state["core"] = Core;
        }

        /// <summary>
        ///     Clears/unloads all the objects and resources created by worker.
        /// </summary>
        internal void ClearWorker()
        {
            Core.DestroyContent();
        }

        /// <summary>
        ///     Look for SLUI custom component types in currently all loaded assemblies.
        /// </summary>
        public static void LookForCustomTypes()
        {
            CustomTypes.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    var attributes = t.GetCustomAttributes(typeof(SLUIComponentAttribute), true);
                    if (attributes.Length > 0)
                    {
                        CustomTypes.Add(t.Name, t);
                    }
                }
            }

            Debug.Log($"{CustomTypes.Count} SLUI's custom component types found.");
        }

        /// <summary/>
        public static Dictionary<string, Type> CustomTypes = new Dictionary<string, Type>();
    }
}
