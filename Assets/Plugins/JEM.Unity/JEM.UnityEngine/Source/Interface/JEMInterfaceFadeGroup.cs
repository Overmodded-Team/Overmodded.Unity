//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Linq;
using UnityEngine;

namespace JEM.UnityEngine.Interface
{
    /// <summary>
    ///     Fade group extension for GameObject.
    /// </summary>
    public static class JEMInterfaceFadeGroup
    {
        /// <summary>
        ///     Fade groups.
        /// </summary>
        /// <param name="gameObject">Target game object.</param>
        /// <param name="state">Fading state.</param>
        /// <param name="speed">Speed of fading.</param>
        /// <param name="exclude">Array of GameObjects that will be excluded.</param>
        /// <returns>Event, when fading ends.</returns>
        public static void FadeGroup(this GameObject gameObject, bool state, float speed, params GameObject[] exclude)
        {
            var elements = gameObject.GetComponentsInChildren<JEMInterfaceFadeElement>(false);
            foreach (var e in elements)
            {
                if (e.IgnoreGroup)
                    continue;
                if (exclude.Contains(e.gameObject))
                    continue;

                e.Fade(state, speed);
            }
        }

        /// <summary>
        ///     Fade groups.
        /// </summary>
        /// <param name="gameObject">Target game object.</param>
        /// <param name="state">Fading state.</param>
        /// <param name="speed">Speed of fading.</param>
        /// <param name="callBack">Event, called when all elements ends fading.</param>
        /// <param name="exclude">Array of GameObjects that will be excluded.</param>
        /// <returns>Event, when fading ends.</returns>
        public static void FadeGroup(this GameObject gameObject, bool state, float speed, Action callBack,
            params GameObject[] exclude)
        {
            var elements = gameObject.GetComponentsInChildren<JEMInterfaceFadeElement>(false);
            var elementsState = new bool[elements.Length];
            var fadingEnds = false;
            for (var index = 0; index < elements.Length; index++)
            {
                var e = elements[index];
                if (e.IgnoreGroup)
                    continue;
                if (exclude.Contains(e.gameObject))
                    continue;

                var index1 = index;
                e.Fade(state, speed, callBack: () =>
                {
                    elementsState[index1] = true;
                    if (fadingEnds)
                        return;
                    foreach (var b in elementsState)
                    {
                        if (!b)
                            return;
                        callBack?.Invoke();
                        fadingEnds = true;
                        return;
                    }
                });
            }
        }

        /// <summary>
        ///     Fade groups.
        /// </summary>
        /// <param name="gameObject">Target game object.</param>
        /// <param name="state">Fading state.</param>
        /// <param name="speed">Speed of fading.</param>
        /// <param name="delay">he delay with which the fade will start.</param>
        /// <param name="callBack">Event, called when all elements ends fading.</param>
        /// <param name="exclude">Array of GameObjects that will be excluded.</param>
        /// <returns>Event, when fading ends.</returns>
        public static void FadeGroup(this GameObject gameObject, bool state, float speed, float delay, Action callBack,
            params GameObject[] exclude)
        {
            var elements = gameObject.GetComponentsInChildren<JEMInterfaceFadeElement>(false);
            var elementsState = new bool[elements.Length];
            var fadingEnds = false;
            for (var index = 0; index < elements.Length; index++)
            {
                var e = elements[index];
                if (e.IgnoreGroup)
                    continue;
                if (exclude.Contains(e.gameObject))
                    continue;

                var index1 = index;
                e.Fade(state, speed, delay, callBack: () =>
                {
                    elementsState[index1] = true;
                    if (fadingEnds)
                        return;
                    foreach (var b in elementsState)
                    {
                        if (!b)
                            return;
                        callBack?.Invoke();
                        fadingEnds = true;
                        return;
                    }
                });
            }
        }

        /// <summary>
        ///     Forces new state of group.
        /// </summary>
        /// <param name="gameObject">Target GameObject.</param>
        /// <param name="state">New state.</param>
        public static void ForceGroup(this GameObject gameObject, bool state)
        {
            var elements = gameObject.GetComponentsInChildren<JEMInterfaceFadeElement>(false);
            foreach (var e in elements)
                e.Force(state);
        }
    }
}