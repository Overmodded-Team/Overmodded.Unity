//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace SimpleLUI.API.Core
{
    /// <summary>
    ///     Base class for all objects SLUI can reference.
    /// </summary>
    public abstract class SLUIObject
    {
        /// <summary>
        ///     The name of the object.
        /// </summary>
        public string name
        {
            get => Original.name;
            set => Original.name = value;
        }

        internal Canvas Root => Manager.Canvas;
        internal SLUIManager Manager { get; private set; }
        internal SLUICore Core => Manager.Worker.Core;
        internal Object Original { get; private set; }

        internal virtual void LoadSLUIObject(SLUIManager manager, Object original)
        {
            Manager = manager;
            Original = original;
        }
    }
}
