//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using SimpleLUI.Editor.API;
using System;
using System.Collections.Generic;
using System.Text;
using Object = UnityEngine.Object;

namespace SimpleLUI.Editor
{
    internal sealed partial class SLUILuaBuilder
    {
        public StringBuilder String { get; }
        public bool PrettyPrint { get; }
        public string ResourcesPath { get; }
        public string ResourcesPathFull { get; }

        private List<SLUIBuilderObject> Builders { get; } = new List<SLUIBuilderObject>();

        internal SLUILuaBuilder(bool prettyPrint, string resourcesPath, string resourcesPathFull)
        {
            String = new StringBuilder();
            PrettyPrint = prettyPrint;
            ResourcesPath = resourcesPath;
            ResourcesPathFull = resourcesPathFull;

            RegisterBuilders();
        }

        private void RegisterBuilder<T1>(T1 builder) where T1 : SLUIBuilderObject
        {
            builder.Parent = this;
            Builders.Add(builder);
        }

        public bool CheckForSupport(Type t)
        {
            foreach (var b in Builders)
            {
                if (b.Type != t) continue;
                return true;
            }

            return false;
        }

        public void Import(string lib, string nameSpace)
        {
            String.AppendLine($"import ('{lib}', '{nameSpace}')");
        }

        public void Label(string label)
        {
            if (!PrettyPrint)
                return;

            if (!string.IsNullOrEmpty(label))
                String.AppendLine($"-- {label}");
        }

        public void Space(string label = null)
        {
            if (!PrettyPrint)
                return;

            String.AppendLine();
            Label(label);
        }

        public void AppendDefinition<T>(T t) where T : Object
        {
            foreach (var b in Builders)
            {
                if (b.Type != t.GetType()) continue;
                b.String.Clear();
                b.CollectObjectDefinition(t);
                var str = b.String.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (PrettyPrint) Label($"{t.name}({t.GetType().Name})");
                    String.Append(str);
                }
                return;
            }

            throw new InvalidOperationException("(Definition)");
        }

        public void AppendDefinitionExtras<T>(T t) where T : Object
        {
            foreach (var b in Builders)
            {
                if (b.Type != t.GetType()) continue;
                b.String.Clear();
                b.CollectObjectDefinitionExtras(t);
                var str = b.String.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (PrettyPrint) Label($"{t.name}({t.GetType().Name})");
                    String.Append(str);
                }
                return;
            }

            throw new InvalidOperationException("(DefinitionExtras)");
        }

        public void AppendProperty<T>(T t) where T : Object
        {
            foreach (var b in Builders)
            {
                if (b.Type != t.GetType()) continue;
                b.String.Clear();
                b.CollectObjectProperty(t);
                var str = b.String.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (PrettyPrint) Label($"{t.name}({t.GetType().Name})");
                    String.Append(str);
                }
                return;
            }

            throw new InvalidOperationException("(Property)");
        }

        public void AppendExtras<T>(T t) where T : Object
        {
            foreach (var b in Builders)
            {
                if (b.Type != t.GetType()) continue;
                b.String.Clear();
                b.CollectExtras(t);
                var str = b.String.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (PrettyPrint) Label($"{t.name}({t.GetType().Name})");
                    String.Append(str);
                }

                return;
            }

            throw new InvalidOperationException("(Extras)");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return String.ToString();
        }
    }
}
