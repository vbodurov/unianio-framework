using System;

namespace Unianio.Animations.Common
{
    public class IntervalAni : StateHolderAni
    {
        private readonly TimeRange _time = new TimeRange();
        private float _remainingAfterPauseSeconds = -1;
        private bool _isPaused;
        private Action<IntervalAni> _update;

        public IntervalAni Set(double seconds, Action<IntervalAni> onTick)
        {
            IntervalSeconds = (float)seconds;
            _update = onTick;
            return this;
        }
        public IntervalAni ForceRun()
        {
            _update?.Invoke(this);
            if(_update != null) ++Repetitions;
            return this;
        }
        public float IntervalSeconds { get; set; }
        public int Repetitions { get; private set; }
        public override void Initialize()
        {
            _time.SetTime(IntervalSeconds);
            Repetitions = 0;
        }
        public override void Update()
        {
            if(_isPaused) return;

            if (_time.IsFinished())
            {
                _update(this);
                ++Repetitions;
                _time.SetTime(IntervalSeconds);
            }
        }
        public IntervalAni SetPaused(bool isPaused)
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