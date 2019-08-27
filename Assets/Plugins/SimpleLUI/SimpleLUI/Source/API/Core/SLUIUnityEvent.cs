//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace SimpleLUI.API.Core
{
    /// <summary>
    ///     Unity's event handler for SLUI.
    /// </summary>
    public class SLUIEventItem
    {
        /// <summary>
        ///     Target object of event.
        /// </summary>
        public SLUIObject target;

        /// <summary>
        ///     Name of target method in target.
        /// </summary>
        public string methodName;

        /// <summary>
        ///     Additional arguments to pass while invoking target method.
        /// </summary>
        public List<object> args;

        /// <summary/>
        public SLUIEventItem() { }

        /// <summary/>
        public SLUIEventItem(SLUIObject target, string method)
        {
            this.target = target;
            this.methodName = method;
            args = new List<object>();
        }

        /// <summary>
        ///     Adds new argument of Int32 type.
        /// </summary>
        public void Add(int i)
        {
            args.Add(i);
        }

        /// <summary>
        ///     Adds new argument of Boolean type.
        /// </summary>
        public void Add(bool b)
        {
            args.Add(b);
        }

        /// <summary>
        ///     Adds new argument of Single type.
        /// </summary>
        public void Add(float f)
        {
            args.Add(f);
        }

        /// <summary>
        ///     Adds new argument of String type.
        /// </summary>
        public void Add(string str)
        {
            args.Add(str);
        }
    }

    /// <summary>
    ///     Group of unity's event.
    /// </summary>
    public class SLUIUnityEvent
    {
        /// <summary/>
        public List<SLUIEventItem> items { get; } = new List<SLUIEventItem>();

        /// <summary/>
        public SLUIUnityEvent() { }

        /// <summary>
        ///     Adds new event to group.
        /// </summary>
        public void Add([NotNull] SLUIEventItem eventItem)
        {
            if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));
            items.Add(eventItem);
        }
    }
}
