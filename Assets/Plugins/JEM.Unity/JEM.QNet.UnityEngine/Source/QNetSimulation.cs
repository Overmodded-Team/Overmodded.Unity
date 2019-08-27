//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using JEM.QNet.UnityEngine.Behaviour;
using System;
using UnityEngine;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     
    /// </summary>
    public static class QNetSimulation
    {
        internal static void Simulate()
        {
            const int frameStepWarning = 20;

            if (_framesToStep > frameStepWarning)
            {
                Debug.LogWarning($"Simulating over 20 frames ({_framesToStep})!");
            }

            while (_framesToStep > 0)
            {
                // Process frame
                QNetObjectManager.Frame();
                Physics.autoSimulation = false;
                Physics.Simulate(QNetTime.TickStep);

                // Increment frame count
                QNetTime.Frame++;
                EstimatedServerFrame++;

                _framesToStep--;
            }
        }

        internal static void AdjustFrames()
        {
            if (QNetManager.IsServerActive)
            {
                _framesToStep = 1;
                return;
            }

            if (!QNetManager.IsClientActive || !QNetManager.IsNetworkActive)
                return;

            if (AdjustServerFrames)
            {
                _framesToStep = 1;
                QNetTime.Frame += Math.Max(0u, ReceivedServerFrame - EstimatedServerFrame);

#if DEBUG
                Debug.Log($"Frame forward ({_framesToStep} frames, local={EstimatedServerFrame}, server={ReceivedServerFrame}))");
#endif

                EstimatedServerFrame = ReceivedServerFrame;
                AdjustServerFrames = false;
            }
            else
            {
                _framesToStep = 1;
            }
        }

        private static uint _serverFrameDiff;
        private static uint _framesToStep;

        /// <summary>
        ///     
        /// </summary>
        public static uint EstimatedServerFrame { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public static uint ReceivedServerFrame { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public static bool AdjustServerFrames { get; set; }
    }
}
