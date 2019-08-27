//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Common;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace Overmodded.Gameplay.Level.Materials
{
    /// <summary>
    ///     Materials settings.
    ///     Contains references to all needed resources used by material.
    /// </summary>
    [CreateAssetMenu(fileName = "newMaterialsDatabase", menuName = "Overmodded/Material", order = 0)]
    public class MaterialSettings : DatabaseItem
    {
        [Header("Resources")]
        public ParticleSystem HitParticle;
        public DecalProjectorComponent HitDecal;
    }
}
