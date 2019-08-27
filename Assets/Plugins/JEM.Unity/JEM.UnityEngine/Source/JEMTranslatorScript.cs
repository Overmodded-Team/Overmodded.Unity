//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine
{
    internal class JEMTranslatorScript : MonoBehaviour
    {
        public IEnumerator RectSlavePosition(JEMTranslatorRect rect, bool activeState)
        {
            if (rect.Target == null)
                rect.Target = GetComponent<RectTransform>();
            if (rect.StartPoint != null && rect.EndPoint != null)
            {
                var tPoint = activeState ? rect.EndPoint : rect.StartPoint;
                while (rect != null && Vector2.Distance(rect.Target.anchoredPosition, tPoint.anchoredPosition) > 0.1f)
                {
                    var t = Time.deltaTime * rect.Speed;
                    rect.Target.anchoredPosition =
                        Vector2.Lerp(rect.Target.anchoredPosition, tPoint.anchoredPosition, t);
                    rect.OnRectUpdated.Invoke();
                    yield return new WaitForEndOfFrame();
                }
            }

            if (rect != null)
            {
                rect.SlavePosition = null;
            }
        }

        public IEnumerator RectSlaveSize(JEMTranslatorRect rect, bool activeState)
        {
            if (rect.Target == null) rect.Target = GetComponent<RectTransform>();
            if (rect.StartPoint != null && rect.EndPoint != null)
            {
                var tPoint = activeState ? rect.EndPoint : rect.StartPoint;
                while (rect != null && Vector2.Distance(rect.Target.sizeDelta, tPoint.sizeDelta) > 0.1f)
                {
                    var t = Time.deltaTime * rect.Speed;
                    rect.Target.sizeDelta = Vector2.Lerp(rect.Target.sizeDelta, tPoint.sizeDelta, t);
                    rect.OnRectUpdated.Invoke();
                    yield return new WaitForEndOfFrame();
                }
            }

            if (rect != null)
            {
                rect.SlaveSize = null;
            }
        }

        internal static void RegenerateLocalScript()
        {
            if (Script != null)
                return;

            var obj = new GameObject(nameof(JEMTranslatorScript));
            DontDestroyOnLoad(obj);
            Script = obj.AddComponent<JEMTranslatorScript>();

            if (Script == null)
                throw new NullReferenceException(
                    $"System was unable to regenerate local script of {nameof(JEMObject)}@{nameof(JEMOperationScript)}");
        }

        public static JEMTranslatorScript Script { get; private set; }
    }
}
