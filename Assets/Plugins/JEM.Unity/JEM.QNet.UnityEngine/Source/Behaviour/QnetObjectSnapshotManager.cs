//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

namespace JEM.QNet.UnityEngine.Behaviour
{
    /// <summary>
    ///     A snapshot manager.
    /// </summary>
    public static class QNetObjectSnapshotManager
    {
        internal static void Simulate()
        {
            if (!QNetManager.IsServerActive) return;
            for (var index = 0; index < QNetObjectBehaviour.SpawnedBehaviours.Length; index++)
            {
                var obj = QNetObjectBehaviour.SpawnedBehaviours[index];
                obj.SendSnapshot();
                obj.LastSnapshotTime = QNetTime.ServerTime;
            }
        }
    }
}
