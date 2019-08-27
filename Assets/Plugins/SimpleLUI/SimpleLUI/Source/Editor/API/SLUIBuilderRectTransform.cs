//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.API.Core;
using SimpleLUI.Editor.Util;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor.API
{
    /// <summary>
    ///     NOTE: This builder is generating new GameObject!
    /// </summary>
    internal sealed class SLUIBuilderRectTransform : SLUIBuilderObject
    {
        public SLUIBuilderRectTransform() : base(typeof(RectTransform)) { }

        /// <inheritdoc />
        public override void CollectObjectDefinition(Object obj)
        {
            var t = (RectTransform) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(t);

            String.AppendLine($"local {name} = core:Create('{t.name}')");
        }

        /// <inheritdoc />
        public override void CollectObjectDefinitionExtras(Object obj)
        {
            var t = (RectTransform)obj;
            var name = SLUILuaBuilderSyntax.CollectVar(t);

            // NOTE: Parent and anchor need to be applied at the definition stage, to not lost the original rectTransform's state.
            // parent
            if (t.parent.parent != null) // only if not a child of root
            {
                var parentName = SLUILuaBuilderSyntax.CollectVar((RectTransform) t.parent);
                String.AppendLine($"{name}.rectTransform:SetParent({parentName}.rectTransform)");
            }

            // anchor
            var anchor = SLUIRectTransform.GetAnchor(t);
            if (anchor != SLUIRectAnchorName.Unknown)
            {
                String.AppendLine($"{name}.rectTransform.anchor = '{SLUIRectTransform.GetAnchor(t)}'");
            }
            else
            {
                String.AppendLine($"{name}.rectTransform.anchorMin = {SLUILuaBuilderSyntax.CollectVector2(t.anchorMin)}");
                String.AppendLine($"{name}.rectTransform.anchorMax = {SLUILuaBuilderSyntax.CollectVector2(t.anchorMax)}");
            }
        }

        /// <inheritdoc />
        public override void CollectObjectProperty(Object obj)
        {
            var t = (RectTransform) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(t);

            // activeState
            if (!t.gameObject.activeSelf)
                String.AppendLine($"{name}:SetActive(false)");

            // transform
            if (Math.Abs(t.pivot.x - 0.5f) > float.Epsilon || Math.Abs(t.pivot.y - 0.5f) > float.Epsilon)
                String.AppendLine($"{name}.rectTransform.pivot = {SLUILuaBuilderSyntax.CollectVector2(t.pivot)}");

            String.AppendLine($"{name}.rectTransform.anchoredPosition = {SLUILuaBuilderSyntax.CollectVector2(t.anchoredPosition)}");
            String.AppendLine($"{name}.rectTransform.sizeDelta = {SLUILuaBuilderSyntax.CollectVector2(t.sizeDelta)}");

            if (t.localRotation != Quaternion.identity)
                String.AppendLine($"{name}.rectTransform.localRotation = {SLUILuaBuilderSyntax.CollectQuaternion(t.localRotation)}");

            if (t.localScale != Vector3.one)
                String.AppendLine($"{name}.rectTransform.localScale = {SLUILuaBuilderSyntax.CollectVector2(t.localScale)}");
        }
    }
}
