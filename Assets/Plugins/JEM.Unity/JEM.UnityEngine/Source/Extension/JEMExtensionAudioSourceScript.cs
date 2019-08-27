//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections;
using UnityEngine;

namespace JEM.UnityEngine.Extension
{
    internal class JEMExtensionAudioSourceScript : MonoBehaviour
    {
        public IEnumerator InternalFadeOut(AudioSource audioSource, float speed, Action onComplete)
        {
            if (speed < .1f)
                speed = .1f;

            while (audioSource.volume > 0.05f)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            audioSource.volume = 0f;
            onComplete?.Invoke();
        }

        public IEnumerator InternalFadeIn(AudioSource audioSource, float speed, Action onComplete)
        {
            if (speed < .1f)
                speed = .1f;

            while (audioSource.volume < 0.95f)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, 1f, Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }

            audioSource.volume = 1f;
            onComplete?.Invoke();
        }
    }
}