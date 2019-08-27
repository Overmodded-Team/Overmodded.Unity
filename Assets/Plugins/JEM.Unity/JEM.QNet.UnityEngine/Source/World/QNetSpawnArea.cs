//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#pragma warning disable 1591

using System;
using JEM.UnityEngine;
using static UnityEngine.Random;

namespace JEM.QNet.UnityEngine.World
{
    /// <inheritdoc />
    /// <summary>
    ///     QNet spawn area.
    /// </summary>
    public class QNetSpawnArea : JEMArea
    {
        /// <summary>
        ///     Gets random spawn area from current world.
        /// </summary>
        public static QNetSpawnArea GetRandomSpawnArea()
        {
            var spawnAreas = FindObjectsOfType<QNetSpawnArea>();
            if (spawnAreas.Length == 0)
                throw new InvalidOperationException(
                    "System was unable to get spawn area. There is no spawn areas in current world.");

            if (spawnAreas.Length == 1)
                return spawnAreas[0];
            return spawnAreas[Range(0, spawnAreas.Length)];
        }
    }
}