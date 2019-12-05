using System;
using static Unianio.Static.fun;

namespace Unianio.Animations.Common
{
    public class RandomIntervalAni : StateHolderAni
    {
        private readonly TimeRange _time = new TimeRange();
        private float _remainingAfterPauseSeconds = -1;
        private bool _isPaused;
        private Action<RandomIntervalAni> _update;

        public RandomIntervalAni Set(double secondsFrom, double seconsTo, Action<RandomIntervalAni> onTick)
        {
            IntervalFromSeconds = min(secondsFrom,seconsTo);
            IntervalToSeconds = min(secondsFrom,seconsTo);
            _update = onTick;
            return this;
        }
        public float IntervalFromSeconds { get; set; }
        public float IntervalToSeconds { get; set; }
        public int Repetitions { get; private set; }
        public override void Initialize()
        {
            _time.SetTime(rnd(IntervalFromSeconds,IntervalToSeconds));
            Repetitions = 0;
        }
        public override void Update()
        {
            if(_isPaused) return;

            if (_time.IsFinished())
            {
                _update(this);
                ++Repetitions;
                _time.SetTime(rnd(IntervalFromSeconds,IntervalToSeconds));
            }
        }
        public RandomIntervalAni SetPaused(bool isPaused)
        {
            IsPaused = isPaused;
            return this;
        }
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (value == _isPaused)
                {
                    return;
                }
                // on pause
                if (value)
                {
                    _remainingAfterPauseSeconds = _time.Distance();
                }
                // on un-pause
                else
                {
                    _time.SetTime(_remainingAfterPauseSeconds);
                }
                _isPaused = value;
            }
        }
    }
}