//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Interface;
using SimpleLUI.Editor.API;
using SimpleLUI.Editor.Util;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleLUI.JEM.Editor
{
    [SLUIBuilderObject]
    public sealed class SLUIJEMFadeAnimationEditor : SLUIBuilderObject
    {
        public SLUIJEMFadeAnimationEditor() : base(typeof(JEMInterfaceFadeAnimation)) { }

        public override void CollectObjectDefinition(Object obj)
        {
            var i = (JEMInterfaceFadeAnimation) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(i);
            var parentName = SLUILuaBuilderSyntax.CollectVar(i.GetComponent<RectTransform>());

            String.AppendLine($"local {name} = {parentName}:AddComponent('{nameof(SLUIJEMFadeAnimation)}')");
        }

        public override void CollectObjectProperty(Object obj)
        {
            var i = (JEMInterfaceFadeAnimation) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(i);

            if (i.AnimationSpeed != 10f)
                String.AppendLine($"{name}.animationSpeed = {i.AnimationSpeed.ToString(CultureInfo.InvariantCulture)}");
            if (i.AnimationEnterScale != 0.9f)
                String.AppendLine($"{name}.animationEnterScale = {i.AnimationEnterScale.ToString(CultureInfo.InvariantCulture)}");
            if (i.AnimationEnterScale != 1.1f)
                String.AppendLine($"{name}.animationExitScale = {i.AnimationExitScale.ToString(CultureInfo.InvariantCulture)}");
            if (i.AnimationMode != JEMFadeAnimationMode.UsingLocalScale)
                String.AppendLine($"{name}:SetAnimationMode('{i.AnimationMode.ToString()}')");
        }
    }
}
