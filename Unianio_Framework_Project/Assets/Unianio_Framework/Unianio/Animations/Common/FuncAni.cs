using System;
using Unianio.Extensions;
using Unianio.Services;
using UnityEngine;

namespace Unianio.Animations.Common
{
    public class FuncAni : StateHolderAni, ITimeBased
    {
        readonly TimeRange _range = new TimeRange();
        float _seconds;
        Action<float> _update1;
        Action<FuncAni, float> _update2;
        Action<FuncAni> _init;
        ITimeProvider _timeProvider;

        public TimeRange Range => _range;

        public FuncAni Set(double seconds, Action<float> update)
        {
            _seconds = (float)seconds;
            _update1 = update;
            return this;
        }
        public FuncAni Set(double seconds, Action<FuncAni, float> update)
        {
            _seconds = (float)seconds;
            _update2 = update;
            return this;
        }
        public FuncAni Set(double seconds, Action<FuncAni> init, Action<float> update)
        {
            _seconds = (float)seconds;
            _update1 = update;
            _init = init;
            return this;
        }
        public FuncAni Set(double seconds, Action<FuncAni> init, Action<FuncAni, float> update)
        {
            _seconds = (float)seconds;
            _update2 = update;
            _init = init;
            return this;
        }
        public FuncAni SetProgress(double currentProgress01)
        {
            _range.SetProgress(currentProgress01);
            return this;
        }

        public FuncAni SetTimeProvider(ITimeProvider tp)
        {
            _timeProvider = tp;
            return this;
        }
        public override void Initialize()
        {
            _init?.Invoke(this);
            if (_seconds < 0.00001)
            {
                _range.SetTime(0);
                Update();
                Finish();
                return;
            }

            if(_timeProvider != null) _range.ChangeTimeProvider(_timeProvider);
            _range.SetTime(_seconds);
        }

        public override void Update()
        {
            var x = _range.Progress().Clamp01();

            _update2?.Invoke(this, x);
            _update1?.Invoke(x);

            if (_range.IsFinished())
            {
                Finish();
            }
        }
    }
}