//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JEM.UnityEngine
{
    /// <summary>
    ///     JEM Area Mode.
    ///     Defines how JEMArea should generate a point.
    /// </summary>
    public enum JEMAreaMode
    {
        /// <summary>
        ///     Unknown area mode. Should never be used!
        /// </summary>
        Unknown,

        /// <summary>
        ///     The point will be generated in local space (random point inside BoxCollider bounds).
        /// </summary>
        Space,

        /// <summary>
        ///     The point will be generated using hit mask (random point casted from the highest Y of BoxCollider bounds).
        /// </summary>
        HitMask
    }

    /// <summary>
    ///     JEM Area Forward Mode.
    ///     Defines how JEMAreas forward vector should be generated.
    /// </summary>
    public enum JEMAreaForwardMode
    {
        /// <summary>
        ///     The Forward vector will be same as JEMArea.
        /// </summary>
        Identity,

        /// <summary>
        ///     Default. The Forward vector will be generate to the outside of the JEM Area center.
        /// </summary>
        Forward,

        /// <summary>
        ///     The Forward vector will be generated to the center of the JEM Area.
        /// </summary>
        ForwardCenter
    }

    /// <inheritdoc />
    /// <summary>
    ///     JEM Area.
    ///     A 3D area component for random point generation using BoxCollider component.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class JEMArea : MonoBehaviour
    {
        /// <summary>
        ///     Target JEM Area Mode.
        /// </summary>
        [Header("Area Settings")]
        public JEMAreaMode Mode = JEMAreaMode.HitMask;

        /// <summary>
        ///     Size of target Agent. 
        /// </summary>
        public float AgentHeight = 1.8f;

        /// <summary>
        ///     Radius of target Agent.
        /// </summary>
        public float AgentSize = 0.4f;

        /// <summary>
        ///     Target JEM Area Forward Mode.
        /// </summary>
        public JEMAreaForwardMode ForwardMode = JEMAreaForwardMode.Forward;

        /// <summary>
        ///     If true, Forward vector.y will be always set to zero.
        /// </summary>
        public bool ResetForwardY = true;

        /// <summary>
        ///     Hit layer mask.
        /// </summary>
        [Header("Hit Mask Settings")]
        public LayerMask HitMask;

        private Vector3 BoxSize => new Vector3(AgentSize, AgentHeight, AgentSize);
        private BoxCollider _collider;

        private void Awake() => _collider = GetComponent<BoxCollider>();      
        private void Reset()
        {
            Mode = JEMAreaMode.HitMask;
            AgentHeight = 1.8f;
            AgentSize = 0.4f;

            HitMask = LayerMask.GetMask("Default");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(gameObject.isStatic, "JEMArea object is not static!", this);
            if (_collider) Debug.Assert(_collider.isTrigger, "BoxCollider of JEMArea isTrigger is not set to true!", this);
        }

        private void OnDrawGizmos()
        {
            if (!_collider) _collider = GetComponent<BoxCollider>();

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            switch (Mode)
            {
                case JEMAreaMode.Space:
                    Gizmos.color = new Color(0.6f, 0.0f, 0.3f, 0.3f);
                    Gizmos.DrawCube(_collider.center, _collider.size);
                    Gizmos.color = new Color(0.7f, 0.0f, 0.3f, 0.7f);
                    Gizmos.DrawWireCube(_collider.center, _collider.size);

                    Gizmos.color = new Color(0.1f, 1.0f, 0.1f, 0.3f);
                    Gizmos.DrawCube(_collider.center, new Vector3(AgentSize, AgentHeight, AgentSize));
                    break;
                case JEMAreaMode.HitMask:
                    Gizmos.color = new Color(0.1f, 0.7f, 0.9f, 0.3f);
                    var centerPoint = new Vector3(_collider.center.x, _collider.center.y + _collider.size.y / 2f, _collider.center.z);
                    Gizmos.DrawCube(centerPoint, new Vector3(_collider.size.x, 0.05f, _collider.size.z));
                    Gizmos.DrawCube(centerPoint + new Vector3(_collider.size.x / 2f - 0.05f, -_collider.size.y / 2f, _collider.size.z / 2f - 0.05f),
                        new Vector3(0.1f, _collider.size.y - 0.05f, 0.1f));
                    Gizmos.DrawCube(centerPoint + new Vector3(-_collider.size.x / 2f + 0.05f, -_collider.size.y / 2f, _collider.size.z / 2f - 0.05f),
                        new Vector3(0.1f, _collider.size.y - 0.05f, 0.1f));
                    Gizmos.DrawCube(centerPoint + new Vector3(_collider.size.x / 2f - 0.05f, -_collider.size.y / 2f, -_collider.size.z / 2f + 0.05f),
                        new Vector3(0.1f, _collider.size.y - 0.05f, 0.1f));
                    Gizmos.DrawCube(centerPoint + new Vector3(-_collider.size.x / 2f + 0.05f, -_collider.size.y / 2f, -_collider.size.z / 2f + 0.05f),
                        new Vector3(0.1f, _collider.size.y - 0.05f, 0.1f));

                    JEMGizmos.DrawArrow(centerPoint, Vector3.down * _collider.size.y);

                    var hasHit = Physics.BoxCast(transform.position + centerPoint, BoxSize / 2f, -transform.up, out var hit, transform.rotation, _collider.size.y -AgentHeight / 2f, HitMask, QueryTriggerInteraction.Ignore);
                    if (!hasHit)
                    {
                        var point = _collider.center - new Vector3(0f, _collider.size.y / 2f - AgentHeight / 2f, 0f);

                        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
                        Gizmos.DrawCube(point, BoxSize);
                        UnityEditor.Handles.matrix = Gizmos.matrix;
                        UnityEditor.Handles.Label(point, "FAIL: Unable to detect any hit from center point!");
                    }
                    else
                    {
                        var point = _collider.center - new Vector3(0f, transform.position.y + _collider.center.y - hit.point.y - AgentHeight / 2f, 0f);

                        Gizmos.color = new Color(0.1f, 1.0f, 0.1f, 0.3f);
                        Gizmos.DrawCube(point, BoxSize);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_collider) _collider = GetComponent<BoxCollider>();
            // TODO: Safe check if collider has any hit.
        }
#endif

        /// <summary>
        ///     Try to random point from this JEM Area component.
        ///     Point generation is always reliable and will always end with successful effect. In case if HitMask mode fail to get point, new point will be generated using Space mode instead.
        /// </summary>
        /// <param name="point"/>
        /// <param name="forward"/>
        public void GenerateReliablePoint(out Vector3 point, out Vector3 forward)
        {
            if (!GenerateUnreliablePoint(out point, out forward))
            {
                if (Mode == JEMAreaMode.Space)
                {
                    // hey! we somehow failed to generate point using Space mode. Wut?
                    throw new NotSupportedException("Fatal. JEMArea failed to generate reliable point using (asDefault)Space mode.");
                }
                else GenerateUnreliablePoint(out point, out forward, JEMAreaMode.Space);
            }
        }

        /// <summary>
        ///     Try to random point from this JEM Area component.
        ///     Point generation is unreliable and can end with unsuccessful effect.
        /// </summary>
        /// <param name="point"/>
        /// <param name="forward"/>
        /// <param name="customMode"/>
        public bool GenerateUnreliablePoint(out Vector3 point, out Vector3 forward, JEMAreaMode customMode = JEMAreaMode.Unknown)
        {
            var prevMode = Mode;
            if (customMode != JEMAreaMode.Unknown) Mode = customMode;

            point = Vector3.zero;
            forward = transform.forward;

            var maxX = _collider.size.x - AgentSize / 2f;
            var maxZ = _collider.size.z - AgentSize / 2f;
            var maxY = _collider.size.y - AgentHeight / 2f;

            switch (Mode)
            {
                case JEMAreaMode.Space:
                    var randomPointInSpace = _collider.center + new Vector3
                    {
                        x = Random.Range(-maxX / 2f, maxX / 2f),
                        y = Random.Range(-maxY / 2f, maxY / 2f),
                        z = Random.Range(-maxZ / 2f, maxZ / 2f)
                    };
                    randomPointInSpace = transform.TransformPoint(randomPointInSpace);

                    point = randomPointInSpace;

                    switch (ForwardMode)
                    {
                        case JEMAreaForwardMode.Identity:
                            forward = transform.forward;
                            break;
                        case JEMAreaForwardMode.Forward:
                            forward = point - transform.position + _collider.center;
                            break;
                        case JEMAreaForwardMode.ForwardCenter:
                            forward = transform.position + _collider.center - point;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (ResetForwardY) forward.y = 0f;
                    Mode = prevMode;
                    return true;
                case JEMAreaMode.HitMask:
                    var randomPointInTop = _collider.center + new Vector3
                    {
                        x = Random.Range(-maxX / 2f, maxX / 2f),
                        y = maxY / 2f,
                        z = Random.Range(-maxZ / 2f, maxZ / 2f)
                    };
                    randomPointInTop = transform.TransformPoint(randomPointInTop);

                    bool hasHit = false;
                    if (RunHitTest(randomPointInTop, out var hitPoint1))
                    {
                        point = hitPoint1;
                        hasHit = true;
                    }
                    else
                    {
                        var centerPoint = new Vector3(_collider.center.x, _collider.center.y + _collider.size.y / 2f, _collider.center.z);
                        if (RunHitTest(centerPoint, out var hitPoint2))
                        {
                            point = hitPoint2;
                            hasHit = true;
                        }
                    }

                    if (hasHit)
                    {
                        point.y += AgentHeight / 2f;

                        switch (ForwardMode)
                        {
                            case JEMAreaForwardMode.Identity:
                                forward = transform.forward;
                                break;
                            case JEMAreaForwardMode.Forward:
                                forward = point - transform.position + _collider.center;
                                break;
                            case JEMAreaForwardMode.ForwardCenter:
                                forward = transform.position + _collider.center - point;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (ResetForwardY) forward.y = 0f;

                        Mode = prevMode;
                        return true;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Mode = prevMode;
            return false;
        }

        private bool RunHitTest(Vector3 origin, out Vector3 point)
        {
            point = Vector3.zero;
            var hasHit = Physics.BoxCast(origin, BoxSize / 2f, -transform.up, out var hit, transform.rotation,
                _collider.size.y - AgentHeight / 2, HitMask, QueryTriggerInteraction.Ignore);
            if (hasHit)
            {
                point = hit.point;
            }

            return hasHit;
        }
    }
}