//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System.Linq;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     Class thanks to QNetStatistics are available.
    /// </summary>
    public static class QNetStatistics
    {
        /// <summary>
        ///     Gets global QNet statistic value from all running peers.
        /// </summary>
        public static float GetValue(QNetStatisticName name)
        {
            return QNetManager.RunningPeers.Sum(peer => peer.GetStatisticValue(name));
        }
    }
}