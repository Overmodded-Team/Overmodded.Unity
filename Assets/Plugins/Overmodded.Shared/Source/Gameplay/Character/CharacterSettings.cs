//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.Core.Text;
using Overmodded.Common;
using Overmodded.Gameplay.Character.Rendering;
using Overmodded.Gameplay.Statistics;
using UnityEngine;

namespace Overmodded.Gameplay.Character
{
    /// <summary>
    ///     Character settings asset.
    /// </summary>
    [CreateAssetMenu(fileName = "newCharacterSettings", menuName = "Overmodded/Character/Settings", order = 0)]
    public class CharacterSettings : DatabaseItem
    {
        [Header("Settings")]
        public Sprite Icon;
        public string LocaleName = "Undefined";
        public StatisticsPrefab StatisticsPrefab;

        [Header("Resources")]
        public CharacterModel ModelPrefab;
        public HandsModel HandsPrefab;

        [Header("Animation")]
        public int DefaultAnimatorController = 0;

        /// <summary>
        ///     Gets name of this character from current locale.
        /// </summary>
        public string GetLocaleName()
        {
            return JEMLocale.Resolve(GameVar.LocaleFileCharacter, $"{LocaleName}_NAME");
        }

        private void OnValidate()
        {
            Debug.Assert(ModelPrefab, $"ModelPrefab of characterSettings {name} is not set!");
            Debug.Assert(HandsPrefab, $"HandsPrefab of characterSettings {name} is not set.");
        }
    }
}
