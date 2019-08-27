//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;

namespace Overmodded.Gameplay.Character.Rendering
{
    [CreateAssetMenu(fileName = "newCharacterModelIKSettings", menuName = "Overmodded/Character/IK/Model IK Settings", order = 0)]
    public class CharacterModelIKSettings : ScriptableObject
    {
        [Serializable]
        public class CameraVector
        {
            public bool FlipZ;
            public bool YIsX;
        }

        [Serializable]
        public class Look
        {
            [Serializable]
            public class WeightSettings
            {
                [Range(0f, 1f)]
                public float BaseWeight = 1f;
                [Range(0f, 1f)]
                public float BodyWeight = 0.5f;
                [Range(0f, 1f)]
                public float HeadWeight = 1f;
                [Range(0f, 1f)]
                public float EyesWeight = 1f;
            }

            [Header("Settings")]
            public WeightSettings LookAtWeightSettings;

            [Header("Look At Point (for PlayerCam)")]
            public float LookWeightTime = 1f;
            public float LookPointTime = 5f;
            [Range(1.5f, 2.5f)]
            public float PointOffset = 2f;
            public float PointHeight = 1f;
            public float PointForward = 0f;
        }

        [Serializable]
        public class Feet
        {
            [Range(0, 2)]
            public float HeightFromGroundRaycast = 0.2f;
            [Range(0, 2)]
            public float RaycastDownDistance = 0.2f;
            public LayerMask EnvironmentLayer;
            public float PelvisOffset = 0f;
            [Range(0, 1)]
            public float PelvisUpAndDownSpeed = 0.28f;
            [Range(0, 1)]
            public float FeetToIkPositionSpeed = 0.5f;

            public bool ShowSolverDebug = true;
        }

        [Header("Camera VectorSettings")]
        public bool ShouldUseCameraVector = true;
        public CameraVector CameraVectorSettings;

        [Header("Look At Settings")]
        public bool ShouldUseLookAt = true;
        public Look LookSettings;

        [Header("Feet IK")]
        public bool ShouldUseFeetIk = true;
        public Feet FeetSettings;
    }
}
