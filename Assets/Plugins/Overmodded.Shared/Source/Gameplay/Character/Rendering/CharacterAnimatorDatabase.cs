//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using System;
using System.Linq;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Rendering
{
    [CreateAssetMenu(fileName = "newAnimatorDatabase", menuName = "Overmodded/Database/Animations Database", order = 0)]
    public class CharacterAnimatorDatabase : DatabaseManager<CharacterAnimatorPrefab>
    {
        /// <summary>
        ///     Gets animator by name.
        /// </summary>
        public CharacterAnimatorPrefab GetItem(string animatorName)
        {
            return Items.FirstOrDefault(t => string.Equals(t.Name, animatorName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
