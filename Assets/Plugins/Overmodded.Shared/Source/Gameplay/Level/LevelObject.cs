//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Character;
using System;
using UnityEngine;

namespace Overmodded.Gameplay.Level
{
    [Serializable]
    public class LevelObjectNetworkBehaviour
    {
        public bool ActiveOnClient = true;
        public bool ActiveOnServer = true;
    }

    [Serializable]
    public class LevelObjectReferences
    {
        /// <summary>
        ///     A root of object model content.
        /// </summary>
        public GameObject ModelRoot;

        /// <summary>
        ///     A character settings reference.
        /// </summary>
        public CharacterSettingsReference CharacterSettings;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Script that helps define level objects.
    /// </summary>
    public class LevelObject : MonoBehaviour
    {
        /// <summary>
        ///     Unique identity of this object (in scene.)
        /// </summary>
        public short Identity;

        /// <summary>
        ///     Defines how this object should behave on network level.
        /// </summary>
        public LevelObjectNetworkBehaviour NetworkBehaviour;

        /// <summary>
        ///     Contains all additional references of object that cloud be used by mods.
        /// </summary>
        public LevelObjectReferences References;
    }
}
