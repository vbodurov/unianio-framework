using System;

namespace Unianio.Animations.Common
{
    public class YoyoAni : StateHolderAni
    {
        readonly TimeRange _time = new TimeRange();
        float _seconds;
        Action<float> _update;
        Action<YoyoAni, int> _cycleStarts;
        int _cycleNumber;


        public YoyoAni Set(double seconds, Action<float> update)
        {
            _seconds = (float) seconds;
            _update = update;
            return this;
        }

        public YoyoAni Set(double seconds, Action<YoyoAni, int> cycleStarts, Action<float> update)
        {
            _seconds = (float) seconds;
            _update = update;
            _cycleStarts = cycleStarts;
            return this;
        }

        public override void Initialize()
        {
            ++_cycleNumber;
            _time.SetTime(_seconds);
            if (_cycleStarts != null) _cycleStarts(this, _cycleNumber);
            if (_update == null) _update = x => { };
        }

        public override void Update()
        {
            var x = _time.Progress();

            if (_cycleNumber%2 == 0) x = 1f - x;

            _update(x);

            if (_time.IsFinished())
            {
                ++_cycleNumber;
                _time.SetTime(_seconds);
                if (_cycleStarts != null) _cycleStarts(this, _cycleNumber);
            }
        }
    }
}