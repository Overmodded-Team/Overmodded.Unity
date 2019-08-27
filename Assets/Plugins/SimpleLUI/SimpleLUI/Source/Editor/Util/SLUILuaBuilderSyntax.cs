//
// SimpleLUI Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Globalization;
using System.Linq;
using System.Text;
using SimpleLUI.API.Util;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleLUI.Editor.Util
{
    public static class SLUILuaBuilderSyntax
    {
        public static string FixVarName(string str)
        {
            char[] banned =
            {
                ' ', '(', ')', '[', ']', '"', '"',
                "'"[0], '<', '>', ',', '.', '?', '/',
                '!', '-', '+', '='
            };

            return banned.Aggregate(str, (current, c) => current.Replace(c, '_'));
        }

        public static string CollectVar(RectTransform r)
        {
            return FixVarName($"obj{r.gameObject.GetInstanceID()}");
        }

        public static string CollectVar(Component c)
        {
            return FixVarName($"obj{c.gameObject.GetInstanceID()}_{c.GetInstanceID()}");
        }

        public static string CollectQuaternion(Quaternion q, bool simple = false)
        {
            var str = $"{q.x.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{q.y.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{q.z.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{q.w.ToString(CultureInfo.InvariantCulture)}";
            if (!simple)
            {
                str = $"SLUIQuaternion({str})";
            }
            return str;
        }

        public static string CollectVector2(Vector2 v, bool simple = false)
        {
            var str = $"{v.x.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{v.y.ToString(CultureInfo.InvariantCulture)}";
            if (!simple)
            {
                str = $"SLUIVector2({str})";
            }
            return str;
        }

        public static string CollectColor(Color c, bool simple = false)
        {
            var str = $"{c.r.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{c.g.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{c.b.ToString(CultureInfo.InvariantCulture)}, " +
                      $"{c.a.ToString(CultureInfo.InvariantCulture)}";
            if (!simple)
            {
                str = $"SLUIColor({str})";
            }
            return str;
        }

        public static string CollectEvent(Object parent, UnityEventBase eventBase, SLUIUnityEventHelper helper, out string varName)
        {
            var eventVar = FixVarName("e_" + parent.GetInstanceID());
            var sb = new StringBuilder();
            sb.AppendLine($"local {eventVar} = SLUIUnityEvent()");

            for (int index = 0; index < eventBase.GetPersistentEventCount(); index++)
            {
                var methodName = eventBase.GetPersistentMethodName(index);
                var eventTarget = eventBase.GetPersistentTarget(index);
                var eventTargetName = "nil";
                if (eventTarget is RectTransform t)
                    eventTargetName = CollectVar(t);
                else if (eventTarget is GameObject g)
                    eventTargetName = CollectVar(g.GetComponent<RectTransform>());
                else if (eventTarget is Component c)
                    eventTargetName = CollectVar(c);
                else
                {
                    Debug.LogWarning($"Event collecting problem. Unable to collect name of a target. ({eventTarget.name}({eventTarget.GetType()}))");
                }

                var methodVar = FixVarName($"{eventVar}_m{index}");
                sb.AppendLine($"local {methodVar} = SLUIEventItem({eventTargetName}, '{methodName}')");
                if (helper != null && helper.Items != null && helper.Items.Count > 0)
                {
                    var objRow = helper.Items[index].obj;
                    foreach (var s in objRow)
                    {
                        if (bool.TryParse(s, out var b))
                            sb.AppendLine($"{methodVar}:Add({b.ToString().ToLower()})");
                        else if (int.TryParse(s, out var i))
                            sb.AppendLine($"{methodVar}:Add({i})");
                        else if (float.TryParse(s, out var f))
                            sb.AppendLine($"{methodVar}:Add({f.ToString(CultureInfo.InvariantCulture)})");
                        else
                        {
                            sb.AppendLine($"{methodVar}:Add('{s}')");
                        }
                    }
                }
                sb.AppendLine($"{eventVar}:Add({methodVar})");
            }
            varName = eventVar;
            return sb.ToString();
        }
    }
}
