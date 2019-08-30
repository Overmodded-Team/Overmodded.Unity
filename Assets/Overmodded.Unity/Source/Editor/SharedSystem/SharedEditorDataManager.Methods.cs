//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using Overmodded.Gameplay.Character;
using Overmodded.Gameplay.Character.Weapons;
using Overmodded.Gameplay.Level.Materials;
using System;
using System.Collections.Generic;

namespace Overmodded.Unity.Editor.SharedSystem
{
    /// <summary/>
    public static partial class SharedEditorDataManager
    {
        public static List<Tuple<string, string, int>> GetCharacterNames() => GetRecords<CharacterDatabase, CharacterSettings>();
        public static List<Tuple<string, string, int>> GetMaterialsNames() => GetRecords<MaterialsDatabase, MaterialSettings>();
        public static List<Tuple<string, string, int>> GetWeaponsNames() => GetRecords<WeaponDatabase, WeaponSettings>();
        public static List<Tuple<string, string, int>> GetAnimationsNames() => GetRecords<CharacterAnimatorDatabase, CharacterAnimatorSettings>();

        /// <summary>
        ///     Gets list of database records. [DatabaseGUID, RecordName, RecordIdentity]
        /// </summary>
        public static List<Tuple<string, string, int>> GetRecords<TDatabase, TItem>() where TDatabase : DatabaseManager<TItem> where TItem : DatabaseItem
        {
            var list = new List<Tuple<string, string, int>>();
            foreach (var editor in LoadedEditorsData)
            {
                var records = editor.GetRecords<TDatabase, TItem>();
                if (records == null)
                    continue;

                foreach (var database in records)
                foreach (var record in database.Records)
                    list.Add(new Tuple<string, string, int>(editor.UniqueGUID, record.Name, record.Identity));
            }

            return list;
        }

        /// <summary>
        ///     Gets list of level spawn names.
        /// </summary>
        public static List<Tuple<string, string>> GetLevelSpawnNames()
        {
            var list = new List<Tuple<string, string>>();
            foreach (var editor in LoadedEditorsData)
                foreach (var record in editor.LevelSpawnNames)
                    list.Add(new Tuple<string, string>(editor.UniqueGUID, record));

            return list;
        }

        /// <summary>
        ///     Gets name of Shared Editor Data by GUID.
        /// </summary>
        public static string GUIDToEditorDataName(string guid)
        {
            string name = "Name N/A";
            for (var index = 0; index < LoadedEditorsData.Count; index++)
            {
                var e = LoadedEditorsData[index];
                if (e == null)
                    continue;

                if (e.UniqueGUID == guid)
                {
                    name = e.name;
                    break;
                }
            }

            return name;
        }
    }
}
