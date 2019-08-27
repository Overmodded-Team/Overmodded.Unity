//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleLUI.API.Util
{
    /// <summary/>
    [ExecuteInEditMode]
    public class SLUIUnityEventHelper : MonoBehaviour
    {
        /// <summary/>
        [Serializable]
        public class Item
        {
            /// <summary/>
            public List<string> obj = new List<string>();
        }

        /// <summary/>
        public List<Item> Items = new List<Item>();
    }
}
