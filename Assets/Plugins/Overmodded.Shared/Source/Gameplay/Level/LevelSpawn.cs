//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.UnityEngine.World;
using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEngine;

namespace Overmodded.Gameplay.Level
{
    public class LevelSpawn : QNetSpawnArea
    {
        [Header("Level Spawn Settings")]
        public string SpawnName = "unknown";
        public bool IsDefault;

        /// <summary>
        ///     Gets random spawn area from current world.
        /// </summary>
        public new static LevelSpawn GetRandomSpawnArea() => GetRandomSpawnArea(true);
        
        /// <summary>
        ///     Gets random spawn area from current world.
        /// </summary>
        public static LevelSpawn GetRandomSpawnArea(bool onlyDefault)
        {
            var spawnPoints = FindObjectsOfType<LevelSpawn>();
            if (spawnPoints.Length == 0)
                throw new InvalidOperationException("System was unable to get spawn area. There is no spawn areas in current world.");

            if (onlyDefault)
            {
                spawnPoints = spawnPoints.Where(s => s.IsDefault).ToArray();
            }

            if (spawnPoints.Length == 0)
                throw new InvalidOperationException("System was unable to get spawn area. There is no spawn areas in current world.");

            return GetRandomSpawnAreaFromPool(spawnPoints);
        }

        /// <summary>
        ///     Gets random spawn area from current world.
        /// </summary>
        public static LevelSpawn GetRandomSpawnAreaOfName(bool onlyDefault, string spawnName)
        {
            var spawnPoints = FindObjectsOfType<LevelSpawn>();
            if (spawnPoints.Length == 0)
                throw new InvalidOperationException("System was unable to get spawn area. There is no spawn areas in current world.");

            if (onlyDefault)
            {
                spawnPoints = spawnPoints.Where(s => s.IsDefault).ToArray();
            }

            spawnPoints = spawnPoints.Where(s => s.SpawnName == spawnName).ToArray();
            if (spawnPoints.Length == 0)
                throw new InvalidOperationException($"System was unable to find any spawn area of name '{spawnName}'.");

            return GetRandomSpawnAreaFromPool(spawnPoints.Where(s => s.SpawnName == spawnName).ToArray());
        }

        /// <summary>
        ///     Gets random spawn area from given pool.
        /// </summary>
        public static LevelSpawn GetRandomSpawnAreaFromPool([NotNull] LevelSpawn[] spawnPoints)
        {
            if (spawnPoints == null) throw new ArgumentNullException(nameof(spawnPoints));
            if (spawnPoints.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(spawnPoints));

            return spawnPoints.Length == 1 ? spawnPoints[0] : spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        }
    }
}
