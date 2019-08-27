//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace Overmodded.Gameplay
{
    /// <summary>
    ///     Body part type. It is used to generate fixed damage for different parts.
    /// </summary>
    public enum BodyPart
    {
        Unknown,

        Head,
        Chest,
        Spine,

        LeftArm,
        LeftShoulder,
        LeftHand,
        LeftKnee,
        LeftFoot,

        RightArm,
        RightShoulder,
        RightHand,
        RightKnee,
        RightFoot
    }

    public class HitboxBodyPart : MonoBehaviour
    {
        [Header("Body Part")]
        public BodyPart Type = BodyPart.Unknown;

        /// <summary>
        ///     Gets damage multiplier based on body type.
        /// </summary>
        [Obsolete("GetDamageMultiplier need to be replaced with file def.")]
        public float GetDamageMultiplier()
        {
            switch (Type)
            {
                case BodyPart.Unknown:
                    return 1.0f;
                case BodyPart.Head:
                    return 2.0f;
                case BodyPart.Chest:
                case BodyPart.Spine:
                    return 1.4f;
                case BodyPart.LeftArm:
                case BodyPart.LeftShoulder:
                case BodyPart.LeftHand:
                    return 0.8f;
                case BodyPart.LeftKnee:
                case BodyPart.LeftFoot:
                    return 0.6f;
                case BodyPart.RightArm:
                case BodyPart.RightShoulder:
                case BodyPart.RightHand:
                    return 0.8f;
                case BodyPart.RightKnee:
                case BodyPart.RightFoot:
                    return 0.6f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
