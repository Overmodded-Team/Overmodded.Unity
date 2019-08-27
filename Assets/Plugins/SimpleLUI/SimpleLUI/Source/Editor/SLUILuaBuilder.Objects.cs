//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.Editor.API;
using System;
using UnityEngine;

namespace SimpleLUI.Editor
{
    internal sealed partial class SLUILuaBuilder
    {
        private void RegisterBuilders()
        {
            RegisterBuilder(new SLUIBuilderRectTransform());
            RegisterBuilder(new SLUIBuilderCanvasRenderer());
            RegisterBuilder(new SLUIBuilderImage());
            RegisterBuilder(new SLUIBuilderText());
            RegisterBuilder(new SLUIBuilderButton());

            try
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (var t in a.GetTypes())
                        {
                            try
                            {
                                var attributes = t.GetCustomAttributes(typeof(SLUIBuilderObjectAttribute), true);
                                if (attributes.Length > 0)
                                {
                                    var instance = (SLUIBuilderObject) Activator.CreateInstance(t);
                                    RegisterBuilder(instance);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
