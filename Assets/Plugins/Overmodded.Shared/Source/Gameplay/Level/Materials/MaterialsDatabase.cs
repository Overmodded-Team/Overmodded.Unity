//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Linq;
using Overmodded.Common;
using UnityEngine;

namespace Overmodded.Gameplay.Level.Materials
{
    /// <summary>
    ///     Materials database.
    ///     Contains array of materials.
    /// </summary>
    [CreateAssetMenu(fileName = "newMaterialsDatabase", menuName = "Overmodded/Database/Materials Database", order = 0)]
    public class MaterialsDatabase : DatabaseManager<MaterialSettings>
    {
        /// <summary>
        ///     Gets item by name.
        /// </summary>
        public MaterialSettings GetItem(string materialName)
        {
            return Items.FirstOrDefault(t => t.name == materialName);
        }
    }
}
