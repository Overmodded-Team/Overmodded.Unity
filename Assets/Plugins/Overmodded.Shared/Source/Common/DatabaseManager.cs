//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Linq;
using UnityEngine;

namespace Overmodded.Common
{
    public abstract class DatabaseManager<T> : ScriptableObject where T : DatabaseItem
    {
        [Header("Settings")]
        public T[] Items = new T[0];

        /// <summary>
        ///     Gets item by identity.
        /// </summary>
        public T GetItem(int identity)
        {
            return Items.FirstOrDefault(t => t.Identity == identity);
        }
    }
}
