//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using JetBrains.Annotations;
using Overmodded.Common;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Overmodded.Unity
{
    /// <summary>
    ///     Record used by SharedDatabaseData.
    /// </summary>
    [Serializable]
    public class SharedDatabaseRecord
    {
        [Serializable]
        public class Property
        {
            public string Key;
            public string Value;
        }

        /// <summary>
        ///     Name of the record in shared database.
        /// </summary>
        public string Name;

        /// <summary>
        ///     Identity of the record.
        /// </summary>
        public int Identity;

        /// <summary>
        ///     List of properties of original object.
        /// </summary>
        public List<Property> Properties;
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

        /// <summary>
        ///     Setup database data.
        /// </summary>
        public void Setup<T>([NotNull] DatabaseManager<T> database) where T : DatabaseItem
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            Name = database.name;

            Records.Clear();

            var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var i in database.Items)
            {
                var newRecord = new SharedDatabaseRecord
                {
                    Name = i.name,
                    Identity = i.Identity,
                    Properties = new List<SharedDatabaseRecord.Property>()
                };

                foreach (var f in fields)
                {
                    if (f.IsPrivate)
                        continue;
                    if (f.Name.Equals("Identity"))
                        continue; // no need to serialize Identity field

                    var value = f.GetValue(i);
                    newRecord.Properties.Add(new SharedDatabaseRecord.Property
                    {
                        Key = f.Name,
                        Value = value == null ? "null" : value.ToString()
                    });
                }

                Records.Add(newRecord);
            }
        }
    }
}
#endif
