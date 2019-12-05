using System;
using Unianio.Extensions;
using static Unianio.Static.fun;

namespace Unianio.Animations.Common
{
    public class RandomTimeFuncAni : StateHolderAni, ITimeBased
    {
        readonly TimeRange _time = new TimeRange();
        double _actFromSeconds;
        double _actToSeconds;
        double _waitFromSeconds;
        double _waitToSeconds;
        Action<RandomTimeFuncAni,float> _update;
        Action<RandomTimeFuncAni,bool> _init;
        bool _isWaiting;

        public TimeRange Range => _time;

        public RandomTimeFuncAni Set(
            double actFromSeconds,double actToSeconds,
            double waitFromSeconds,double waitToSeconds, 
            Action<RandomTimeFuncAni,float> update)
        {
            _actFromSeconds = actFromSeconds;
            _actToSeconds = actToSeconds;
            _waitFromSeconds = waitFromSeconds;
            _waitToSeconds = waitToSeconds;
            _update = update;
            return this;
        }
        public RandomTimeFuncAni Set(
            double actFromSeconds,double actToSeconds,
            double waitFromSeconds,double waitToSeconds, 
            Action<RandomTimeFuncAni,bool> init, Action<RandomTimeFuncAni,float> update)
        {
            _actFromSeconds = actFromSeconds;
            _actToSeconds = actToSeconds;
            _waitFromSeconds = waitFromSeconds;
            _waitToSeconds = waitToSeconds;
            _update = update;
            _init = init;
            return this;
        }
        public override void Initialize()
        {
            if (_update == null) _update = (a,x) => { };
            StartCycle();
        }
        public override void Update()
        {
            var x = _time.Progress().Clamp01();

            if(!_isWaiting)
            {
                _update(this, x);
            }

            if (_time.IsFinished())
            {
                _isWaiting = !_isWaiting;
                StartCycle();
            }
        }
        void StartCycle()
        {
            if (_init != null)
                _init(this, _isWaiting);

            _time.SetTime(
                _isWaiting 
                ? rnd(_waitFromSeconds, _waitToSeconds)
                : rnd(_actFromSeconds, _actToSeconds));
        }
    }
}