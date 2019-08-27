//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine.Scripts
{
    /// <inheritdoc />
    /// <summary>
    ///     TPP Camera movement script.
    /// </summary>
    public class JEMThirdPersonCamera : MonoBehaviour
    {
        /// <summary>
        ///     Camera transform.
        /// </summary>
        [Header("Base")]
        public Transform Camera;

        /// <summary>
        ///     Character transform.
        /// </summary>
        public Transform Character;

        /// <summary>
        ///     Position fix speed by collision.
        /// </summary>
        public float CollisionFixSpeed = 4.0F;

        /// <summary>
        ///     Camera collision mask.
        /// </summary>
        [Header("Collision")]
        public LayerMask CollisionMask;

        /// <summary>
        ///     Current distance of camera to character.
        /// </summary>
        [Space]
        public float CurrentDistance = 8.0F;

        /// <summary>
        ///     Minimum and maximum distance of camera to character.
        /// </summary>
        public Vector2 CurrentDistanceClamp = new Vector2(1.0F, 3.0F);

        /// <summary>
        ///     Spherecast size.
        /// </summary>
        public float SpherecastSize = 1.0F;

        /// <summary>
        ///     Maximum and minimum X of camera.
        /// </summary>
        [Header("Rotation")]
        public Vector2 Border = new Vector2(-89.0F, 89.0f);

        /// <summary>
        ///     User distance of camera to character.
        /// </summary>
        [Header("Distance")]
        public float UserDistance = 3f;

        /// <summary>
        ///     Minimum and maximum user distance of camera to character.
        /// </summary>
        public Vector2 UserDistanceClamp = new Vector2(3.0F, 9.0F);

        /// <summary>
        ///     User distance smooth speed.
        /// </summary>
        public float UserDistanceSmooth = 15f;

        /// <summary>
        ///     Offset of camera on Y axis.
        /// </summary>
        [Header("Offset")]
        public float YOffset = 1.5f;

        /// <summary>
        ///     Smooth of Y axis.
        /// </summary>
        public float YSmooth = 3.0F;

        private float _eulerX;
        private float _eulerXAdd;

        private float _yFix;

        // start event
        private void Start()
        {
            Camera.transform.SetParent(transform);

            Camera.localPosition = new Vector3(0f, 0f, CurrentDistance);
            Camera.localRotation = Quaternion.identity;
        }

        // update event
        private void Update()
        {
            if (Character == null || Camera == null)
                return;

            if (Camera.transform.parent != transform)
            {
                Camera.transform.SetParent(transform);

                Camera.localPosition = new Vector3(0f, 0f, CurrentDistance);
                Camera.localRotation = Quaternion.identity;
            }

            _yFix = Mathf.Lerp(_yFix, Character.position.y + YOffset, Time.smoothDeltaTime * YSmooth);
            transform.position = new Vector3(Character.position.x, _yFix, Character.position.z);
        }

        // late update event
        private void LateUpdate()
        {
            if (Character == null || Camera == null)
                return;

            var add = Vector3.zero;
            if (Input.GetMouseButton(1))
            {
                add.x = -Input.GetAxis("Mouse Y") * 3.5F;
                add.y = Input.GetAxis("Mouse X") * 3.5F;
            }

            if (transform.rotation.eulerAngles.x < 90)
                _eulerX = transform.rotation.eulerAngles.x;
            else
                _eulerX = transform.rotation.eulerAngles.x - 360;

            _eulerXAdd = _eulerX + add.x;
            if (_eulerXAdd < Border.x)
                return;
            if (_eulerXAdd > Border.y)
                return;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + add.x,
                transform.rotation.eulerAngles.y + add.y, 0.0F);

            UserDistance -= Input.GetAxis("Mouse ScrollWheel") * 8.5f;
            UserDistance = Mathf.Clamp(UserDistance, UserDistanceClamp.x, UserDistanceClamp.y);

            RaycastHit collisionHit;
            if (Physics.SphereCast(transform.position, SpherecastSize, Camera.position - transform.position,
                out collisionHit, UserDistance, CollisionMask, QueryTriggerInteraction.Ignore))
            {
                CurrentDistance = Vector3.Distance(transform.position, collisionHit.point);
                CurrentDistance = Mathf.Clamp(CurrentDistance, CurrentDistanceClamp.x, CurrentDistanceClamp.y);
            }
            else
            {
                CurrentDistance = Mathf.Lerp(CurrentDistance, UserDistance, Time.deltaTime * UserDistanceSmooth);
            }

            Camera.localPosition = new Vector3(0f, 0f, -CurrentDistance);
        }
    }
}