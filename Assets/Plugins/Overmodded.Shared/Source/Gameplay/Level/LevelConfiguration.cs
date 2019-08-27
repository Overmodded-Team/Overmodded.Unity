//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Character;
using UnityEngine;

namespace Overmodded.Gameplay.Level
{
    /// <inheritdoc />
    /// <summary>
    ///     Level configuration script.
    /// </summary>
    public class LevelConfiguration : MonoBehaviour
    {
        [Header("Defaults")]
        public CharacterSettingsReference DefaultCharacterSettings;
    }
}
