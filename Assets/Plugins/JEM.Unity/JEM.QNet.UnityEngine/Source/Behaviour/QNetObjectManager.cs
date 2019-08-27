//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

namespace JEM.QNet.UnityEngine.Behaviour
{
    /// <summary>
    ///     A object simulation manager
    /// </summary>
    public static class QNetObjectManager
    {
        internal static void Frame()
        {
            // simulate entities
            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.SpawnedBehaviours[index];
                obj.OnBeginSimulate();
                obj.Simulate();
                obj.OnFinishSimulate();
            }

            QNetObjectSnapshotManager.Simulate();
        }
    }
}
