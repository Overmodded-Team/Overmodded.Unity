//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.UnityEngine;
using System.Collections;
using UnityEngine;

namespace JEM.Test
{
    public class JEMAreaTest : JEMArea
    {
        [Header("Test Stuff")]
        public GameObject Obj;
        public int ObjToGenerate = 10;

        private IEnumerator Start()
        {
            int i = 0;
            while (i < ObjToGenerate)
            {
                i++;

                if (GenerateUnreliablePoint(out var point, out var forward))
                {
                    Instantiate(Obj, point, Quaternion.LookRotation(forward)).SetActive(true);
                } else Debug.LogError("Failed to generate new point.");

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
