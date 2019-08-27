//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine.Interface
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface Fade Element Script.
    /// </summary>
    internal class JEMInterfaceFadeElementScript : MonoBehaviour
    {
        internal float GetFadeTime(bool fixedSmooth, float speed)
        {
            return (fixedSmooth ? Time.smoothDeltaTime : Time.deltaTime) * speed;
        }

        internal IEnumerator InternalFadeWork(JEMInterfaceFadeElement element, short sessionID, bool state, float speed,
            float delay, bool fixedSmooth, Action callBack)
        {
            if (element != null)
            {
                // active work
                element.WorkActive = true;
                element.WorkState = state;
                element.WorkCallback = callBack;

                // disable raycast target while fade work
                element.InternalSetRaycastTarget(!element.AlwaysFalseRaycast && element.StartRaycastTarget);

                // apply state to gameobject
                if (!element.gameObject.activeSelf && element.ApplyStateToGameObject)
                    element.gameObject.SetActive(true);
            }

            // some delay
            yield return new WaitForSeconds(delay);

            // check session
            if (element != null && element.WorkSessionId != sessionID)
                yield break;

            // fade animation
            if (state)
            {
                while (element != null && element.WorkActive && element.WorkSessionId == sessionID &&
                       element.GraphicAlpha < element.FixedAlphaTarget - 0.01f)
                {
                    element.InternalUpdateGraphicAlpha(Mathf.Lerp(element.GraphicAlpha, element.FixedAlphaTarget,
                        GetFadeTime(fixedSmooth, speed)));
                    yield return new WaitForEndOfFrame();
                }

                if (element == null || element.WorkSessionId != sessionID)
                    yield break;
                element.InternalUpdateGraphicAlpha(1f);
            }
            else
            {
                while (element != null && element.WorkActive && element.WorkSessionId == sessionID &&
                       element.GraphicAlpha > 0.01f)
                {
                    element.InternalUpdateGraphicAlpha(Mathf.Lerp(element.GraphicAlpha, 0f,
                        GetFadeTime(fixedSmooth, speed)));
                    yield return new WaitForEndOfFrame();
                }

                if (element == null || element.WorkSessionId != sessionID)
                    yield break;
                element.InternalUpdateGraphicAlpha(0f);
            }

            // update raycast target
            element.InternalSetRaycastTarget(element.StartRaycastTarget);

            // apply state to gameobject
            if (element.gameObject.activeSelf != state && element.ApplyStateToGameObject)
                element.gameObject.SetActive(state);

            element.InternalBreakWork();
        }
    }
}