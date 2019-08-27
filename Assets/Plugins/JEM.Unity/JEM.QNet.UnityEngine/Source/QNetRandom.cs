//
// QNet for Unity Engine - Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using System;

namespace JEM.QNet.UnityEngine
{
    /// <inheritdoc />
    /// <summary>
    ///     Pseudo-random number generator for QNet.
    /// </summary>
    public class QNetRandom : IDisposable
    {
        private readonly Random _rand;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="serverFrame"></param>
        public QNetRandom(int serverFrame)
        {
            _rand = new Random(serverFrame);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <summary>
        ///     Returns a non-negative random integer.
        /// </summary>
        public int Random()
        {
            return _rand.Next();
        }

        /// <summary>
        ///     Returns a random floating-point number that is grater than or equals to 0.0, and less than 1.0.
        /// </summary>
        public float RandomFloat()
        {
            return (float) _rand.NextDouble();
        }

        /// <summary>
        ///     Returns a random integer that is within specified range.
        /// </summary>
        public int RandomRange(int start, int end)
        {
            return _rand.Next(start, end);
        }

        /// <summary>
        ///     Returns a random float that is in within specified range.
        /// </summary>
        public float RandomRange(float start, float end)
        {
            var num = start - end;
            var num2 = _rand.NextDouble();
            var num3 = num2 * num + end;
            return (float) num3;
        }
    }
}