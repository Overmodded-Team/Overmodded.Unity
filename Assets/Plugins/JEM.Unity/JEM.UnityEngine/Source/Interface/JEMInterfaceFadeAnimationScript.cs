//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine.Extension;
using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine.Interface
{
    internal sealed class JEMInterfaceFadeAnimationScript : MonoBehaviour
    {
        internal IEnumerator InternalSetActive(JEMInterfaceFadeAnimation window, short sessionId)
        {
            if (window != null)
            {
                if (window.TargetActive) window.gameObject.LiteSetActive(true);

                if (window.TargetActive)
                {
                    window.gameObject.ForceGroup(false);
                    window.gameObject.FadeGroup(true, window.AnimationSpeed);
                }
                else
                {
                    window.gameObject.ForceGroup(true);
                    window.gameObject.FadeGroup(false, window.AnimationSpeed);
                }

                if (window.AnimationMode != JEMFadeAnimationMode.Disabled)
                    StartCoroutine(InternalAnimateCanvas(window, sessionId));

                yield return new WaitForSeconds(10f / window.AnimationSpeed);

                if (window == null || window.gameObject == null)
                    yield break;
                if (window.WorkSessionID != sessionId)
                    yield break;

                if (!window.TargetActive) window.gameObject.LiteSetActive(false);
            }

            if (window == null)
                yield break;
            if (window.WorkSessionID != sessionId)
                yield break;

            // restart current work
            window.RestartWork();

            // force window just to make sure that everything forks fine
            window.ForceActiveState();
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private IEnumerator InternalAnimateCanvas(JEMInterfaceFadeAnimation window, short sessionId)
        {
            if (window.TargetActive)
            {
                if (window.AnimationMode == JEMFadeAnimationMode.UsingLocalScale)
                {
                    window.transform.localScale = window.StartLocalSize * window.AnimationEnterScale;
                    while (window != null && window.IsWorking && window.WorkSessionID == sessionId &&
                           Vector3.Distance(window.transform.localScale, window.StartLocalSize) > 0.01f)
                    {
                        window.transform.localScale = Vector3.Lerp(window.transform.localScale, window.StartLocalSize,
                            Time.deltaTime * window.AnimationSpeed);
                        yield return new WaitForEndOfFrame();
                    }

                    if (window == null || !window.IsWorking || window.WorkSessionID != sessionId)
                        yield break;

                    window.transform.localScale = window.StartLocalSize;
                }
                else
                {
                    window.RectTransform.sizeDelta = window.StartSizeDelta * window.AnimationEnterScale;
                    while (window != null && window.IsWorking && window.WorkSessionID == sessionId &&
                           Vector2.Distance(window.RectTransform.sizeDelta, window.StartSizeDelta) > 1f)
                    {
                        window.RectTransform.sizeDelta = Vector2.Lerp(window.RectTransform.sizeDelta,
                            window.StartSizeDelta, Time.deltaTime * window.AnimationSpeed);
                        yield return new WaitForEndOfFrame();
                    }

                    if (window == null || !window.IsWorking || window.WorkSessionID != sessionId)
                        yield break;

                    window.RectTransform.sizeDelta = window.StartSizeDelta;
                }
            }
            else
            {
                if (window.AnimationMode == JEMFadeAnimationMode.UsingLocalScale)
                {
                    window.RectTransform.localScale = window.StartLocalSize;
                    while (window != null && window.WorkSessionID == sessionId &&
                           Vector3.Distance(window.transform.localScale, window.StartLocalSize * 1.1f) > 0.01f)
                    {
                        window.transform.localScale = Vector3.Lerp(window.transform.localScale,
                            window.StartLocalSize * window.AnimationExitScale, Time.deltaTime * window.AnimationSpeed);
                        yield return new WaitForEndOfFrame();
                    }

                    if (window == null || !window.IsWorking || window.WorkSessionID != sessionId)
                        yield break;

                    window.transform.localScale = window.StartLocalSize * window.AnimationExitScale;
                }
                else
                {
                    window.RectTransform.sizeDelta = window.StartSizeDelta;
                    while (window != null && window.WorkSessionID == sessionId &&
                           Vector2.Distance(window.RectTransform.sizeDelta, window.StartSizeDelta * 1.1f) > 1f)
                    {
                        window.RectTransform.sizeDelta = Vector2.Lerp(window.RectTransform.sizeDelta,
                            window.StartSizeDelta * window.AnimationExitScale, Time.deltaTime * window.AnimationSpeed);
                        yield return new WaitForEndOfFrame();
                    }

                    if (window == null || !window.IsWorking || window.WorkSessionID != sessionId)
                        yield break;

                    window.RectTransform.localScale = window.StartLocalSize * window.AnimationExitScale;
                }
            }
        }
    }
}