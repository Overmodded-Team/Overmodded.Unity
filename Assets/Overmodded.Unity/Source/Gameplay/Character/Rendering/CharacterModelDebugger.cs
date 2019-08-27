#if UNITY_EDITOR
//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gameplay.Character.Rendering;
using UnityEngine;

namespace Overmodded.Unity.Gameplay.Character.Rendering
{
    [RequireComponent(typeof(CharacterModel))]
    internal class CharacterModelDebugger : MonoBehaviour
    {
        internal CharacterModel Model { get; private set; }
        internal CharacterModelIKSettings Settings => Model.IKSettings;

        [Header("CameraVector Debug")]
        [Range(-90f, 90f)]
        [SerializeField] internal float SpineX;
        [Range(-90f, 90f)]
        [SerializeField] internal float SpineY;
        [Range(-90f, 90f)]
        [SerializeField] internal float SpineZ;

        private void Awake()
        {
            Model = GetComponent<CharacterModel>();
        }

        private void LateUpdate()
        {
            if (!Settings.ShouldUseCameraVector)
                return;

            var spineX = SpineX;
            var spineY = SpineY;
            var spineZ = SpineZ;

            if (Settings.CameraVectorSettings.YIsX)
            {
                spineY = -SpineX;
            }

            if (Settings.CameraVectorSettings.FlipZ)
            {
                spineZ = SpineX;
                spineX = SpineZ;
            }

            // spineX = Mathf.Clamp(spineX, -90f, 90f);
            spineZ = Mathf.Clamp(spineZ, -90f, 90f);

            if (Model.SpineTransform != null)
            {
                var spineRotation = Model.SpineTransform.localRotation;
                Model.SpineTransform.localRotation = Quaternion.Euler(spineRotation.eulerAngles.x + spineX / 3f,
                    spineRotation.eulerAngles.y + spineY,
                    spineRotation.eulerAngles.z + spineZ / 3f);
            }

            if (Model.ChestTransform != null)
            {
                var chestRotation = Model.ChestTransform.localRotation;
                Model.ChestTransform.localRotation = Quaternion.Euler(chestRotation.eulerAngles.x + spineX / 3f,
                    chestRotation.eulerAngles.y + spineY / 3f,
                    chestRotation.eulerAngles.z + spineZ / 3f);
            }

            if (Model.HeadTransform != null)
            {
                var headRotation = Model.HeadTransform.localRotation;
                Model.HeadTransform.localRotation = Quaternion.Euler(headRotation.eulerAngles.x + spineX / 3f,
                    headRotation.eulerAngles.y + spineY / 3f,
                    headRotation.eulerAngles.z + spineZ / 3f);
            }
        }
    }
}
#endif