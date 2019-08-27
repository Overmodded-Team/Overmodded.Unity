//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.API.Core;
using SimpleLUI.API.Util;
using SimpleLUI.Editor.Util;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor.API
{
    internal class SLUIBuilderButton : SLUIBuilderObject
    {
        public SLUIBuilderButton() : base(typeof(Button)) { }

        public override void CollectObjectDefinition(Object obj)
        {
            var b = (Button) obj;
            var parentName = SLUILuaBuilderSyntax.CollectVar(b.GetComponent<RectTransform>());
            var name = SLUILuaBuilderSyntax.CollectVar(b);

            String.AppendLine($"local {name} = {parentName}:AddComponent('Button')");
        }

        public override void CollectObjectProperty(Object obj)
        {
            var b = (Button) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(b);

            String.AppendLine(
                $"{name}.targetGraphic = {(b.targetGraphic == null ? "nil" : SLUILuaBuilderSyntax.CollectVar(b.image))}");
            if (!b.interactable)
                String.AppendLine($"{name}.interactable = false");
            String.AppendLine($"{name}.normalColor = {SLUILuaBuilderSyntax.CollectColor(b.colors.normalColor)}");
            String.AppendLine(
                $"{name}.highlightedColor = {SLUILuaBuilderSyntax.CollectColor(b.colors.highlightedColor)}");
            String.AppendLine($"{name}.pressedColor = {SLUILuaBuilderSyntax.CollectColor(b.colors.pressedColor)}");
            String.AppendLine($"{name}.selectedColor = {SLUILuaBuilderSyntax.CollectColor(b.colors.selectedColor)}");
            String.AppendLine($"{name}.disabledColor = {SLUILuaBuilderSyntax.CollectColor(b.colors.disabledColor)}");

            if (b.onClick.GetPersistentEventCount() != 0)
            {
                String.AppendLine(SLUILuaBuilderSyntax.CollectEvent(b, b.onClick, b.GetComponent<SLUIUnityEventHelper>(), out var eventName));
                String.AppendLine($"{name}.onClickUnityEvent = {eventName}");
            }
        }
    }
}
