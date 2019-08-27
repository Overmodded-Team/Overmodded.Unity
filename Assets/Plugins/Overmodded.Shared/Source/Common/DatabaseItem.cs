//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace Overmodded.Common
{
    public abstract class DatabaseItem : ScriptableObject
    {
        /// <summary>
        /// Unique identity of item.
        /// </summary>
        [HideInInspector]
        public int Identity;
    }
}
