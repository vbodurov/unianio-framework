using System.Collections.Generic;
using Unianio.Extensions;
using Unianio.RSG;
using Unianio.Services;
using UnityEngine;

namespace Unianio.Animations.Common
{
    public class WaitAni : StateHolderAni
    {
        public static readonly WaitAni Complete = new WaitAni().Set(0).Pipe(wa => wa.Finish());
        readonly TimeRange _time = new TimeRange();
        float _seconds;
        ITimeProvider _timeProvider;

        /// <param name="seconds">
        /// if seconds is 0 then it will end right away and will execute all followup actions,
        /// if seconds is less than zero it will not execute any follow up actions
        /// </param>
        public WaitAni Set(double seconds)
        {
//dbg.log("Set GenFace ",seconds,Label,Time.time);
            _seconds = (float)seconds;
            return this;
        }
        public WaitAni SetTimeProvider(ITimeProvider tp)
        {
            _timeProvider = tp;
            return this;
        }
        public override void Initialize()
        {
            if (_seconds <= 0.0001)
            {
                if (_seconds < 0)
                {
                    IsForcedToFinish = true;
                    ClearAllFollowUpActions();
                    Finish();
                }
                else
                {
                    Finish();
                }
                return;
            }
            if (_timeProvider != null) _time.ChangeTimeProvider(_timeProvider);
            _time.SetTime(_seconds);
        }
        public override void Update()
        {
            if (_time.IsFinished())
            {
                Finish();
            }
        }
    }
}