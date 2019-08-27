//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace JEM.UnityEngine.VersionManagement.Data
{
    /// <summary>
    ///     Build version data.
    /// </summary>
    [Serializable]
    public class JEMBuildVersion
    {
        /// <summary>
        ///     Version name.
        /// </summary>
        [SerializeField]
        public string VersionName = "notDefined";

        /// <summary>
        ///     Version release date.
        /// </summary>
        [SerializeField]
        public DateTime VersionRelease;
    }
}