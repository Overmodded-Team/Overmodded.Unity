//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor.API
{
    internal class SLUIBuilderCanvasRenderer : SLUIBuilderObject
    {
        public SLUIBuilderCanvasRenderer() : base(typeof(CanvasRenderer)) { }

        public override void CollectObjectDefinition(Object obj)
        {
            // ignore
        }

        public override void CollectObjectProperty(Object obj)
        {
           // ignore
        }
    }
}
