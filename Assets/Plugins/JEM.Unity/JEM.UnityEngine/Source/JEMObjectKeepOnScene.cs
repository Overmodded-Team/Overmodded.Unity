//
// JEM For Unity Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Don't destroys target GameObject on scene load.
    /// </summary>
    public sealed class JEMObjectKeepOnScene : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);     
    }
}