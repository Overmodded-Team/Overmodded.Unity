//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JEM.UnityEngine.Scripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Free camera movement script.
    /// </summary>
    [AddComponentMenu("Controllers/Free Camera Movement")]
    public class JEMFreeCamera : MonoBehaviour
    {
        /// <summary>
        ///     Minimal value of X axis of rotation.
        /// </summary>
        [Header("Rotation Settings")]
        public float minimumX = -360F;

        /// <summary>
        ///     Maximal value of X axis of rotation.
        /// </summary>
        public float maximumX = 360F;

        /// <summary>
        ///     Minimal value of Y axis of rotation.
        /// </summary>
        [Space]
        public float minimumY = -60F;

        /// <summary>
        ///     Maximal value of Y axis of rotation.
        /// </summary>
        public float maximumY = 60F;

        /// <summary>
        ///     Camera movement will only work while cursor is locked.
        /// </summary>
        [Header("Base Settings")]
        public bool OnlyWhenCursorLocked = true;

        /// <summary>
        ///     Allow to change camera position via scroll.
        /// </summary>
        public bool AllowPositionScroll;

        /// <summary>
        /// </summary>
        public float PositionSmooth = 25f;

        /// <summary>
        ///     Camera position will can be changed by WSAD while on right mouse button.
        /// </summary>
        public bool PositionWSADOnRightMouse;

        /// <summary>
        ///     Camera will rotate only on right mouse button.
        /// </summary>
        public bool RotateOnlyOnRightMouse;

        /// <summary>
        ///     Smooth of camera rotation.
        /// </summary>
        [Space]
        public float RotationSmooth = 25f;

        /// <summary>
        ///     Sensitivity of moving camera by scroll.
        /// </summary>
        [Header("Scroll Settings")]
        public float scrollPositionSensitivity = 5f;

        /// <summary>
        ///     Power of moving camera position by mouse.
        /// </summary>
        public float scrollPositionSpeed = 0.4f;

        /// <summary>
        ///     Sensitivity of scroll.
        /// </summary>
        public float scrollSensitivity = 5f;

        /// <summary>
        ///     Smooth of moving camera by scroll.
        /// </summary>
        public float scrollSensitivitySpeed = 10f;

        /// <summary>
        ///     Power of moving camera position by wheel.
        /// </summary>
        public float scrollWheelSpeed = 0.4f;

        /// <summary>
        ///     Sensitivity of mouse on X axis.
        /// </summary>
        [Header("Mouse Settings")]
        public float sensitivityX = 5F;

        /// <summary>
        ///     Sensitivity of mouse in Y axis.
        /// </summary>
        public float sensitivityY = 5F;

        /// <summary/>
        [Header("Position Settings")]
        public float Speed = 0.3f;

        /// <summary>
        /// </summary>
        [Header("Events")]
        public UnityEvent OnInput;

        private Quaternion _originalRotation;
        private Vector3 _positionDirection;

        private float _rotationX;
        private float _rotationY;

        private Vector3 _rotationEuler;

        private void Start()
        {
            _originalRotation = transform.localRotation;
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
                UpdateRotation();

            if (OnlyWhenCursorLocked)
            {
                if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible)
                    return;
            }
            else
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
            }

            if (AllowPositionScroll)
            {
                if (Input.GetMouseButton(2))
                {
                    OnInput.Invoke();
                    var y = Input.GetAxis("Mouse Y") * scrollPositionSensitivity;
                    var x = Input.GetAxis("Mouse X") * scrollPositionSensitivity;

                    _positionDirection = Vector3.Lerp(_positionDirection,
                        transform.TransformDirection(-x, -y, 0) * scrollPositionSpeed,
                        Time.deltaTime * scrollSensitivitySpeed);
                    transform.position += _positionDirection;
                    return;
                }

                OnInput.Invoke();
                var scrollDiff = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
                _positionDirection = Vector3.Lerp(_positionDirection, transform.forward * scrollDiff * scrollWheelSpeed,
                    Time.deltaTime * scrollSensitivitySpeed);
                transform.position += _positionDirection;
            }

            if (RotateOnlyOnRightMouse)
                if (!Input.GetMouseButton(1))
                    return;

            OnInput.Invoke();
            _rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            _rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            _rotationY = ClampAngle(_rotationY, minimumY, maximumY);
            _rotationX = ClampAngle(_rotationX, minimumX, maximumX);

            var yQuaternion = Quaternion.AngleAxis(_rotationY, Vector3.left);
            var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            var q = _originalRotation * xQuaternion * yQuaternion;
            _rotationEuler = q.eulerAngles;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, q, Time.deltaTime * RotationSmooth);

            if (RotateOnlyOnRightMouse)
                if (!PositionWSADOnRightMouse)
                    return;

            var posX = Input.GetAxis("Horizontal");
            var posY = Input.GetKey(KeyCode.Space) ? 1f : Input.GetKey(KeyCode.LeftShift) ? -1f : 0f;
            var posZ = Input.GetAxis("Vertical");

            _positionDirection = Vector3.Lerp(_positionDirection,
                transform.TransformDirection(posX, posY, posZ) * Speed,
                Time.deltaTime * PositionSmooth);
            transform.position += _positionDirection;
        }

        /// <summary>
        ///     Sets rotation from transform to camera.
        /// </summary>
        public void UpdateRotation()
        {
            if (transform.eulerAngles.x < maximumY)
                _rotationY = FixAngle(-transform.eulerAngles.x - 360);
            else
                _rotationY = -(FixAngle(transform.eulerAngles.x) - 360);

            _rotationX = FixAngle(transform.eulerAngles.y);
        }

        /// <summary>
        ///     Fixes angle.
        /// </summary>
        /// <param name="angle">Angle to clamp.</param>
        public static float FixAngle(float angle)
        {
            angle = angle % 360;
            if (angle >= -360F && angle <= 360F)
            {
                if (angle < -360F) angle += 360F;

                if (angle > 360F) angle -= 360F;
            }

            return angle;
        }

        /// <summary>
        ///     Clamp angle.
        /// </summary>
        /// <param name="angle">Angle to clamp.</param>
        /// <param name="min">Minimal angle.</param>
        /// <param name="max">Maximal angle.</param>
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = FixAngle(angle);
            return Mathf.Clamp(angle, min, max);
        }
    }
}