//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Overmodded.Unity
{
    /// <summary>
    ///     Record used by SharedDatabaseData.
    ///     NOTE: We can only share name and the identity of original object!
    ///     TODO: Serialize selected properties and save them in like:. Dictionary<string, string>
    /// </summary>
    public class SharedDatabaseRecord
    {
        /// <summary>
        ///     Name of the record in shared database.
        /// </summary>
        public string Name;

        /// <summary>
        ///     Identity of the record.
        /// </summary>
        public int Identity;
    }

    [Serializable]
    public class SharedDatabaseData
    {
        /// <summary>
        ///     Original name of shared database.
        /// </summary>
        public string Name = "Unnamed";

        /// <summary>
        ///     List of database records.
        /// </summary>
        public List<SharedDatabaseRecord> Records = new List<SharedDatabaseRecord>();
    }
}
#endif
