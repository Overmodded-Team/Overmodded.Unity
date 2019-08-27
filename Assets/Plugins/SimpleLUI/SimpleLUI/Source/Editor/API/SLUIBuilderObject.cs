//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Text;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor.API
{
    public abstract class SLUIBuilderObject
    {
        internal SLUILuaBuilder Parent { get; set; }
        internal Type Type { get; }
        public StringBuilder String { get; }
        public bool PrettyPrint => Parent.PrettyPrint;
        protected SLUIBuilderObject(Type t)
        {
            Type = t;
            String = new StringBuilder();
        }

        public abstract void CollectObjectDefinition(Object obj);
        public virtual void CollectObjectDefinitionExtras(Object obj) { }
        public abstract void CollectObjectProperty(Object obj);
        public virtual void CollectExtras(Object obj) { }

        public void Label(string label) => Parent.Label(label);
        public void Space(string label = null) => Parent.Space(label);
    }
}
