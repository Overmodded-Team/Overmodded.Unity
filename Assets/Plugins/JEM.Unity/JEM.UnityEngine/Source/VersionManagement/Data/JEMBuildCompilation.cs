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
    ///     Build compilation data.
    /// </summary>
    [Serializable]
    public class JEMBuildCompilation
    {
        /// <summary>
        ///     Number of build.
        /// </summary>
        [SerializeField]
        public int BuildNumber;

        /// <summary>
        ///     Date of last build.
        /// </summary>
        [SerializeField]
        public DateTime BuildTime;

        /// <summary>
        ///     Number of compilation.
        /// </summary>
        [SerializeField]
        public int CompilationNumber;

        /// <summary>
        ///     Date of last compilation.
        /// </summary>
        [SerializeField]
        public DateTime CompilationTime;

        /// <summary>
        ///     Time in second that defines, how much time dev spend on project.
        /// </summary>
        public int SessionTime;
    }
}