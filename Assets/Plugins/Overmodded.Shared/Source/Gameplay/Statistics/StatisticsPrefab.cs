//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace Overmodded.Gameplay.Statistics
{
    [CreateAssetMenu(fileName = "newStatisticsPrefab", menuName = "Overmodded/Statistics", order = 0)]
    public class StatisticsPrefab : ScriptableObject
    {
        [Header("Statistics")]
        public StatisticPredef[] Items;
    }
}
