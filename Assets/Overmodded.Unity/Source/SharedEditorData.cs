//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using JetBrains.Annotations;
using Overmodded.Common;
using Overmodded.Gameplay.Character;
using Overmodded.Gameplay.Character.Rendering;
using Overmodded.Gameplay.Character.Weapons;
using Overmodded.Gameplay.Level.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Overmodded.Unity
{
    /// <inheritdoc />
    /// <summary>
    ///     Overmodded shared editor data. Thanks to this object you can reference resources and settings from other unity projects.
    /// </summary>
    [CreateAssetMenu(fileName = "newSharedEditorData", menuName = "Overmodded/Shared/Shared Editor Data", order = 0)]
    public class SharedEditorData : ScriptableObject
    {
        [Serializable]
        public class UniqueDatabase
        {
            public string TypeName;
            public List<SharedDatabaseData> Databases = new List<SharedDatabaseData>();
        }

        /// <summary>
        ///     A unique GUID.
        ///     Once SharedEditorData has been shipped, should never be regenerated.
        /// </summary>
        [Header("Identity Settings")]
        public string UniqueGUID;

        /// <summary>
        ///     Level Spawn Names list.
        ///     List that contains all custom defines names for LevelSpawn component.
        ///     NOTE: At the end, to the every spawn name the UniqueGUID is added to make sure that the spawn name is always Unique.
        /// </summary>
        public List<string> LevelSpawnNames = new List<string>();

        /// <summary>
        ///     Unique Databases List.
        ///     List of all databases in the game.
        /// </summary>
        public List<UniqueDatabase> UniqueDatabases = new List<UniqueDatabase>();

        public List<SharedDatabaseData> GetCharacters() => GetRecords<CharacterDatabase, CharacterSettings>();
        public List<SharedDatabaseData> GetMaterials() => GetRecords<MaterialsDatabase, MaterialSettings>();
        public List<SharedDatabaseData> GetWeapons() => GetRecords<WeaponDatabase, WeaponSettings>();
        public List<SharedDatabaseData> GetAnimations() => GetRecords<CharacterAnimatorDatabase, CharacterAnimatorPrefab>();

        /// <summary>
        ///     Gets the list of shared databases of given type.
        /// </summary>
        public List<SharedDatabaseData> GetRecords<TDatabase, TItem>() where TDatabase : DatabaseManager<TItem> where TItem : DatabaseItem
        {
            return (from db in UniqueDatabases where db.TypeName == typeof(TDatabase).FullName select db.Databases).FirstOrDefault();
        }

        /// <summary>
        ///     Refresh all databases.
        /// </summary>
        public void RefreshDatabases()
        {
            UniqueDatabases.Clear();          
            WriteDatabase<CharacterDatabase, CharacterSettings>();
            WriteDatabase<MaterialsDatabase, MaterialSettings>();
            WriteDatabase<WeaponDatabase, WeaponSettings>();
            WriteDatabase<CharacterAnimatorDatabase, CharacterAnimatorPrefab>();
        }

        private void WriteDatabase<TDatabase, TItem>() where TDatabase : DatabaseManager<TItem>
            where TItem : DatabaseItem
        {
            var newUnique = new UniqueDatabase
            {
                TypeName = typeof(TDatabase).FullName,
            };

            ForEachAsset<TDatabase>(db =>
            {
                var newDatabaseData = new SharedDatabaseData {Name = db.name};
                newDatabaseData.Setup(db);
                newUnique.Databases.Add(newDatabaseData);
            });

            UniqueDatabases.Add(newUnique);
        }

        /// <summary>
        ///     Finds all assets of given Type and runs onEvent for each of them.
        /// </summary>
        private static void ForEachAsset<T>([NotNull] Action<T> onEvent) where T : Object
        {
            if (onEvent == null) throw new ArgumentNullException(nameof(onEvent));
            ForEachAsset(typeof(T), obj => onEvent.Invoke((T) obj));
        }

        /// <summary>
        ///     Finds all assets of given Type and runs onEvent for each of them.
        /// </summary>
        private static void ForEachAsset(Type t, [NotNull] Action<Object> onEvent)
        {
            if (onEvent == null) throw new ArgumentNullException(nameof(onEvent));
            var assets = AssetDatabase.FindAssets($"t:{t.Name}");
            foreach (var guid in assets)
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), t);
                if (asset == null)
                    continue;
                onEvent.Invoke(asset);
            }
        }
    }
}
#endif