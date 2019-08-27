//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEngine;

namespace JEM.QNet.UnityEngine
{
    /// <summary>
    ///     
    /// </summary>
    public static class QNetTime
    {
        private static int _tickRate = 30;

        /// <summary>
        ///     The current local time.
        /// </summary>
        public static float Time => global::UnityEngine.Time.time;

        /// <summary>
        ///     The current local real time (since game's startup).
        /// </summary>
        public static float RealTime => global::UnityEngine.Time.realtimeSinceStartup;

        /// <summary>
        ///     The current local frame.
        /// </summary>
        public static uint Frame;

        /// <summary>
        ///     The frame alpha value. This is value in range [0-1] that is difference between Update time and FixedUpdate time.
        /// </summary>
        /// <remarks>
        ///     Only valid in Update function context!
        /// </remarks>
        public static float FrameAlpha => Mathf.Clamp01((global::UnityEngine.Time.time - global::UnityEngine.Time.fixedTime) / global::UnityEngine.Time.fixedDeltaTime);

        /// <summary>
        ///     Current server frame.
        /// </summary>
        public static uint ServerFrame => QNetManager.IsServerActive ? Frame : QNetSimulation.EstimatedServerFrame;

        /// <summary>
        ///     Current server time.
        /// </summary>
        public static float ServerTime => ServerFrame * TickStep;

        /// <summary>
        ///     The simulation tick step, equal to (1.0f/TickRate).
        /// </summary>
        public static float TickStep => 1.0f / TickRate;

        /// <summary>
        ///     Physics and Network tick rate, client and server should have the same rate to
        ///     avoid synchronization issues.
        /// </summary>
        public static int TickRate
        {
            get => _tickRate;
            set
            {
                _tickRate = Mathf.Clamp(value, 30, 144);

                global::UnityEngine.Time.fixedDeltaTime = 1.0f / _tickRate;
                //JEMLogger.Log($"TickRate set to '{_tickRate}Hz'", "QNET");
            }
        }

        /// <summary>
        ///     Converts given time to frame as floating point without rounding.
        /// </summary>
        /// <param name="time">The time that will be converted.</param>
        /// <returns>The resulting frame.</returns>
        public static float TimeToFFrame(float time)
        {
            return time / TickStep;
        }

        /// <summary>
        ///     Converts given time to frame (with rounding).
        /// </summary>
        /// <param name="time">The time that will be converted.</param>
        /// <returns>The resulting frame.</returns>
        public static uint TimeToFrame(float time)
        {
            return (uint)Mathf.RoundToInt(time / TickStep);
        }

        /// <summary>
        ///     Converts given frame to time.
        /// </summary>
        /// <param name="frame">The frame that will be converted.</param>
        /// <returns>The resulting time.</returns>
        public static float FrameToTime(uint frame)
        {
            return frame * TickStep;
        }
    }
}
