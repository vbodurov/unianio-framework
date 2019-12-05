using System;
using Unianio.Services;

namespace Unianio.Animations.Common
{
    public class CycleAni : StateHolderAni
    {
        readonly TimeRange _time = new TimeRange();
        float _seconds;
        Action<float> _update;
        Action<CycleAni, int> _cycleStarts;
        int _cycleNumber;
        ITimeProvider _timeProvider;

        public CycleAni Set(double seconds, Action<float> update)
        {
            _seconds = (float) seconds;
            _update = update;
            return this;
        }

        public CycleAni Set(double seconds, Action<CycleAni, int> cycleStarts, Action<float> update)
        {
            _seconds = (float) seconds;
            _update = update;
            _cycleStarts = cycleStarts;
            return this;
        }
        public CycleAni SetTime(double seconds)
        {
            _seconds = (float) seconds;
            if (_timeProvider != null) _time.ChangeTimeProvider(_timeProvider);
            _time.SetTime(_seconds);
            return this;
        }
        public CycleAni SetTimeProvider(ITimeProvider tp)
        {
            _timeProvider = tp;
            return this;
        }
        public override void Initialize()
        {
            ++_cycleNumber;
            if (_timeProvider != null) _time.ChangeTimeProvider(_timeProvider);
            _time.SetTime(_seconds);
            _cycleStarts?.Invoke(this, _cycleNumber);
            if (_update == null) _update = x => { };
        }

        public override void Update()
        {
            var x = _time.Progress();

            _update(x);

            if (_time.IsFinished())
            {
                ++_cycleNumber;
                if (_timeProvider != null) _time.ChangeTimeProvider(_timeProvider);
                _time.SetTime(_seconds);
                _cycleStarts?.Invoke(this, _cycleNumber);
            }
        }
    }
}