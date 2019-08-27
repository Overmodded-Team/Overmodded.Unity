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
    internal class SLUIBuilderImage : SLUIBuilderObject
    {
        public SLUIBuilderImage() : base(typeof(Image)) { }

        public override void CollectObjectDefinition(Object obj)
        {
            var i = (Image) obj;
            var name = SLUILuaBuilderSyntax.CollectVar(i);
            var parentName = SLUILuaBuilderSyntax.CollectVar(i.rectTransform);

            String.AppendLine($"local {name} = {parentName}:AddComponent('Image')");
        }

        public override void CollectObjectProperty(Object obj)
        {
            var i = (Image)obj;
            var name = SLUILuaBuilderSyntax.CollectVar(i);

            string sprite = null;
            if (i.sprite != null)
            {
                sprite = SLUIResourcesConverter.CollectResourceName(i.sprite);
                SLUIResourcesConverter.WriteSprite($"{Parent.ResourcesPathFull}\\{sprite}", i.sprite);
                sprite = $"{Parent.ResourcesPath}\\{sprite}".Replace("\\", "//");
            }

            String.AppendLine($"{name}.imageType = '{i.type.ToString()}'");
            if (i.color != Color.white)
                String.AppendLine($"{name}.color = {SLUILuaBuilderSyntax.CollectColor(i.color)}");
            if (!i.raycastTarget)
                String.AppendLine($"{name}.raycastTarget = false");
            if (i.preserveAspect)
                String.AppendLine($"{name}.preserveAspect = true");
            if (!i.fillCenter)
                String.AppendLine($"{name}.fillCenter = false");
            if (!string.IsNullOrEmpty(sprite))
            {
                //if (File.Exists(sprite))
                    String.AppendLine($"{name}.sprite = '{sprite}'");
                //else Debug.LogError($"Sprite generation failed. Target file not exist. ({sprite})");
            }          
        }
    }
}
