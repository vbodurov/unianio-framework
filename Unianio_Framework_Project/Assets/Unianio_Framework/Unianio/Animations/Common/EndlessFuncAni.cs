using System;
using UnityEngine;

namespace Unianio.Animations.Common
{
    public class EndlessFuncAni : StateHolderAni
    {
        Action<EndlessFuncAni> _init;
        Action<EndlessFuncAni> _update;
        float _startTime;

        public EndlessFuncAni Set(Action<EndlessFuncAni> update)
        {
            _update = update;
            return this;
        }
        public EndlessFuncAni Set(Action<EndlessFuncAni> init, Action<EndlessFuncAni> update)
        {
            _init = init;
            _update = update;
            return this;
        }
        public override void Initialize()
        {
            _startTime = Time.time;
            _init?.Invoke(this);
            if (_update == null) Finish();
        }
        public override void Update()
        {
            _update(this);
        }
        public float StartTime => _startTime;
    }
}