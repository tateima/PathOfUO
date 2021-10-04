/*************************************************************************
 * ModernUO                                                              *
 * Copyright (C) 2019-2021 - ModernUO Development Team                   *
 * Email: hi@modernuo.com                                                *
 * File: Timer.cs                                                        *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using Server.Diagnostics;
using Server.Logging;

namespace Server
{
    public partial class Timer
    {
        protected internal static readonly ILogger logger = LogFactory.GetLogger(typeof(Timer));

        public static void Configure()
        {
            ConfigureTimerPool();
        }

        // We need to know what ring/slot we are in so we can be removed if we are "head" of the link list.
        private int _ring;
        private int _slot;
        private long _remaining;
        private Timer _nextTimer;
        private Timer _prevTimer;

        public Timer(TimeSpan delay) => Init(delay, TimeSpan.Zero, 1);

        public Timer(TimeSpan interval, int count) => Init(interval, interval, count);

        public Timer(TimeSpan delay, TimeSpan interval, int count = 0) => Init(delay, interval, count);

        protected void Init(TimeSpan delay, TimeSpan interval, int count)
        {
            Running = false;
            Delay = delay;
            Index = 0;
            Interval = interval;
            Count = count;
            _nextTimer = null;
            _prevTimer = null;
            Next = Core.Now + Delay;

            var prof = GetProfile();

            if (prof != null)
            {
                prof.Created++;
            }
        }

        protected int Version { get; set; } // Used to determine if a timer was altered and we should abandon it.

        public DateTime Next { get; private set; }
        public TimeSpan Delay { get; set; }
        public TimeSpan Interval { get; set; }
        public int Index { get; private set; }
        public int Count { get; private set; }
        public int RemainingCount => Count - Index;
        public bool Running { get; private set; }

        public TimerProfile GetProfile() => !Core.Profiling ? null : TimerProfile.Acquire(ToString() ?? "null");

        public override string ToString() => GetType().FullName;

        public Timer Start()
        {
            if (Running)
            {
                return this;
            }

            Index = 0;
            Running = true;
            AddTimer(this, (long)Delay.TotalMilliseconds);

            var prof = GetProfile();

            if (prof != null)
            {
                prof.Started++;
            }

            return this;
        }

        public virtual void Stop()
        {
            if (!Running)
            {
                return;
            }

            // Do not detach if we are in the middle of executing the timer wheel for this ring/slot
            if (!_timerWheelExecuting || _ringIndexes[_ring] != _slot)
            {
                // We are at the head
                if (_rings[_ring][_slot] == this)
                {
                    _rings[_ring][_slot] = _nextTimer;
                }

                Detach();
            }

            Running = false;
            Version++;

            var prof = GetProfile();
            if (prof != null)
            {
                prof.Stopped++;
            }
        }

        protected virtual void OnTick()
        {
        }

        private void Attach(Timer timer)
        {
            _nextTimer = timer;
            if (timer != null)
            {
                timer._prevTimer = this;
            }
        }

        private void Detach()
        {
            if (_prevTimer != null)
            {
                _prevTimer._nextTimer = _nextTimer;
            }

            if (_nextTimer != null)
            {
                _nextTimer._prevTimer = _prevTimer;
            }

            _nextTimer = null;
            _prevTimer = null;
        }

        internal virtual void OnDetach()
        {
        }
    }
}
