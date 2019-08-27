//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Overmodded.Gameplay
{
    public enum ModelRenderType
    {
        AnimationsShadowsOnly,
        DefaultCulled,
        AlwaysDrawAndAnimate,
        DefaultCulledNotMotionVectors,
        DefaultCulledObjectMotionVectors,
        AnimationsOnly,
    }

    public abstract class ObjectModelBehaviour : MonoBehaviour
    {
        [Header("Object Model")]
        public Animator Animator;

        public List<Renderer> Renderers { get; } = new List<Renderer>();
        public List<Collider> Colliders { get; } = new List<Collider>();
        public Hitbox Hitbox { get; private set; }

        /// <summary>
        ///     Defines whether the renderers of model are enabled.
        /// </summary>
        public bool RenderersEnabled { get; private set; }

        /// <summary>
        ///     Defines whether the colliders of model are enabled.
        /// </summary>
        public bool CollidersEnabled { get; private set; }

        private void OnValidate()
        {
            // Debug.Assert(Animator, $"Animator of model `{name}` is not set!");
        }

        private void Awake()
        {
            // collect components
            if (!_wasFirst) return;
            CollectModelComponents();
        }

        private bool _wasFirst = true;

        public void CollectModelComponents()
        {
            OnLoadObjectRenderers();
            if (_wasFirst)
            {
                RenderersEnabled = false;
                foreach (var r in Renderers)
                {
                    if (!r.enabled) continue;
                    RenderersEnabled = true; // set to true if any of renderers is enabled
                    break;
                }
            }
            else SetRenderersActive(RenderersEnabled);

            OnLoadObjectColliders();
            if (_wasFirst)
            {
                CollidersEnabled = false;
                foreach (var c in Colliders)
                {
                    if (!c.enabled) continue;
                    CollidersEnabled = true;
                    break;
                }
            }
            else SetCollidersActive(CollidersEnabled);

            OnComponentsCollected(_wasFirst);
            _wasFirst = false;
        }

        protected virtual void OnLoadObjectRenderers()
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        protected virtual void OnLoadObjectColliders()
        {
            var colliders = GetComponentsInChildren<Collider>();
            Hitbox = GetComponentInChildren<Hitbox>();
            if (Hitbox == null)
            {
                foreach (var c in colliders)
                {
                    var hitbox = c.GetComponent<Hitbox>();
                    if (hitbox == null) continue;
                    if (Hitbox != null)
                    {
                        Debug.LogWarning(
                            $"There is more than one Hitbox in this model ({name}) with may cause problems with collision detection.",
                            this);
                        continue;
                    }

                    Hitbox = hitbox;
                }
            }

            Colliders.AddRange(colliders);
        }

        protected virtual void OnComponentsCollected(bool isFirst)
        {
            if (isFirst) return;
            // refresh active render type on components collect
            SetModelRenderType(RenderType);
        }

        public void SetRenderersActive(bool enableState)
        {
            if (_wasFirst) CollectModelComponents();
            RenderersEnabled = enableState;
            foreach (var r in Renderers)
            {
                if (r) r.enabled = enableState;
            }
        }

        public void SetCollidersActive(bool enableState)
        {
            if (_wasFirst) CollectModelComponents();
            CollidersEnabled = enableState;
            foreach (var c in Colliders)
            {
                if(c) c.enabled = enableState;
            }
        }

        /// <summary>
        ///     Gets first transform parent of a renderers of this object.
        /// </summary>
        public Transform GetFirstRendererParent()
        {
            Transform f = transform;
            foreach (var r in Renderers)
            {
                if (r == null || transform.parent == null) continue;
                f = transform.parent;
                if (f != transform) // try to not get this object as a parent? is this a correct way?
                    return f;
            }

            return f;
        }

        /// <summary>
        ///     Currently active render type.
        /// </summary>
        public ModelRenderType RenderType { get; private set; } = ModelRenderType.DefaultCulled;

        /// <summary>
        ///     Sets the render type of the character.
        /// </summary>
        public void SetModelRenderType(ModelRenderType type)
        {
            RenderType = type;

            // always active!
            SetRenderersActive(true);

            switch (type)
            {
                case ModelRenderType.AnimationsShadowsOnly:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                        r.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = true;
                    }
                    break;
                case ModelRenderType.DefaultCulled:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.CullCompletely;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
                        r.shadowCastingMode = ShadowCastingMode.TwoSided;
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = false;
                    }
                    break;
                case ModelRenderType.AlwaysDrawAndAnimate:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                        r.shadowCastingMode = ShadowCastingMode.On;
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = true;
                    }
                    break;
                case ModelRenderType.DefaultCulledNotMotionVectors:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.CullCompletely;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                        r.shadowCastingMode = ShadowCastingMode.TwoSided;
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = false;
                    }
                    break;
                case ModelRenderType.DefaultCulledObjectMotionVectors:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.CullCompletely;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
                        r.shadowCastingMode = ShadowCastingMode.TwoSided;
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = false;
                    }
                    break;
                case ModelRenderType.AnimationsOnly:
                    if (Animator) Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    foreach (var r in Renderers)
                    {
                        if (!r) continue;
                        r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                        r.shadowCastingMode = ShadowCastingMode.Off;
                        r.enabled = false; // test! do not know if disabled renderer will update animation...
                        if (r is SkinnedMeshRenderer skinned)
                            skinned.updateWhenOffscreen = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
