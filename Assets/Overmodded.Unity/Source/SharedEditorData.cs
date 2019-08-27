//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Overmodded.Unity
{
    /// <inheritdoc />
    /// <summary>
    ///     Overmodded shared editor data. Thanks to this object you can reference resources and settings from other unity projects.
    /// </summary>
    [CreateAssetMenu(fileName = "newSharedEditorData", menuName = "Overmodded/Shared/Shared Editor Data", order = 0)]
    public class SharedEditorData : ScriptableObject
    {
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
    }
}
#endif