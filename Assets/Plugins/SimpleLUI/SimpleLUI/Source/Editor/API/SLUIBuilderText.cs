//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.Editor.Util;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor.API
{
    internal class SLUIBuilderText : SLUIBuilderObject
    {
        public SLUIBuilderText() : base(typeof(Text)) { }

        public override void CollectObjectDefinition(Object obj)
        {
            var t = (Text) obj;
            var parentName = SLUILuaBuilderSyntax.CollectVar(t.GetComponent<RectTransform>());
            var name = SLUILuaBuilderSyntax.CollectVar(t);

            String.AppendLine($"local {name} = {parentName}:AddComponent('Text')");
        }

        public override void CollectObjectProperty(Object obj)
        {
            var t = (Text) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(t);

            String.AppendLine($"{name}.text = {'"'}{t.text}{'"'}");
            if (t.fontSize != 14)
                String.AppendLine($"{name}.fontSize = {t.fontSize}");
            if (t.fontStyle != FontStyle.Normal)
                String.AppendLine($"{name}.fontStyle = '{t.fontStyle.ToString()}'");
            if (t.alignment != TextAnchor.UpperLeft)
                String.AppendLine($"{name}.alignment = '{t.alignment.ToString()}'");
            if (t.resizeTextForBestFit)
                String.AppendLine($"{name}.resizeTextForBestFit = true");
            if (t.resizeTextMinSize != 10)
                String.AppendLine($"{name}.resizeTextMinSize = {t.resizeTextMinSize}");
            if (t.resizeTextMaxSize != 40)
                String.AppendLine($"{name}.resizeTextMaxSize = {t.resizeTextMaxSize}");
            String.AppendLine($"{name}.color = {SLUILuaBuilderSyntax.CollectColor(t.color)}");
            if (!t.raycastTarget)
                String.AppendLine($"{name}.raycastTarget = false");
        }
    }
}
