//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JEM.UnityEngine
{
    internal class JEMObjectScript : MonoBehaviour
    {
        public IEnumerator InternalLiteDestroy(Object obj, Action onDone)
        {
            Destroy(obj);
            yield return new WaitForEndOfFrame();
            onDone?.Invoke();
        }

        public IEnumerator InternalLiteInstantiate(Object original, Vector3 position, Quaternion orientation,
            Action<Object> onDone)
        {
            var obj = Instantiate(original, position, orientation);
            yield return obj;
            onDone?.Invoke(obj);
        }
    }
}