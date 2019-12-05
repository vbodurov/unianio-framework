using System;
using Unianio.Animations.Common;
using Unianio.Extensions;
using Unianio.Services;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Animations
{
    public class AnimationManager : AnimationHolder
    {
        protected readonly IGlobalFactory Factory;

        //        public static AnimationManager Empty => new AnimationManager();

        public AnimationManager() : this(GlobalFactory.Default) { }
        public AnimationManager(IGlobalFactory factory) 
        {
            Factory = factory;
        }
        public TAni PlayHere<TAni>() where TAni : IAnimation
            => get<TAni>().PlayOn(this);
        public NextFrameAni GoToNextFrame()
            => get<NextFrameAni>().PlayOn(this);
        public SkipFramesAni SkipFrames(int numberFrames)
            => get<SkipFramesAni>().Set(numberFrames).PlayOn(this);
        public AnimationManager OnesIn(int numberTimes) 
            => onesIn(numberTimes, this);
        public FuncAni StartFuncAni(double seconds, Action<float> action)
            => get<FuncAni>().Set(seconds, action).PlayOn(this);
        public FuncAni StartFuncAni(double seconds, Action<FuncAni> init, Action<float> update)
            => get<FuncAni>().Set(seconds, init, update).PlayOn(this);
        public CycleAni StartCycleAni(double seconds, Action<float> action)
            => get<CycleAni>().Set(seconds, action).PlayOn(this);
        public CycleAni StartCycleAni(double seconds, Action<CycleAni, int> init, Action<float> action)
            => get<CycleAni>().Set(seconds, init, action).PlayOn(this);
        public YoyoAni StartYoyoAni(double seconds, Action<float> action)
            => get<YoyoAni>().Set(seconds, action).PlayOn(this);
        public YoyoAni StartYoyoAni(double seconds, Action<YoyoAni, int> onStart, Action<float> action)
            => get<YoyoAni>().Set(seconds, onStart, action).PlayOn(this);
        public EndlessFuncAni StartEndlessFuncAni(Action<EndlessFuncAni> action)
            => get<EndlessFuncAni>().Set(action).PlayOn(this);
        public EndlessDrawFuncAni StartEndlessDrawFuncAni(Action<EndlessDrawFuncAni> action)
            => get<EndlessDrawFuncAni>().Set(action).PlayOn(this);
        public IntervalAni StartIntervalAni(double seconds, Action<IntervalAni> onTick)
            => get<IntervalAni>().Set(seconds, onTick).PlayOn(this);
        public EndlessFuncAni OnKeyUp(KeyCode code, Action action)
            => get<EndlessFuncAni>()
                    .Set(ani =>
                    {
                        if (Input.GetKeyUp(code)) action();
                    })
                .PlayOn(this);
        public EndlessFuncAni OnKeyDown(KeyCode down, KeyCode pressed, Action action)
            => get<EndlessFuncAni>()
                    .Set(ani =>
                    {
                        if (Input.GetKeyDown(down) && Input.GetKey(pressed)) action();
                    })
                .PlayOn(this);
        public FramesPerSecondAni StartFramesPerSecondAni(Action<float> setFpsUi)
            => get<FramesPerSecondAni>()
                    .Set(setFpsUi)
                .PlayOn(this);
        public T StartAni<T>() where T : IAnimation
            => get<T>().PlayOn(this);
        public IAnimation StartAni(Type t)
            => ((IAnimation)get(t)).PlayOn(this);
        public T StartAni<T>(T ani) where T : IAnimation
            => ani.PlayOn(this);
        public NextFrameAni NextFrameAni()
            => get<NextFrameAni>();
        public FuncAni FuncAni(double seconds, Action<float> update)
            => get<FuncAni>().Set(seconds, update);
        public FuncAni FuncAni(double seconds, Action<FuncAni> init, Action<float> update)
            => get<FuncAni>().Set(seconds, init, update);
        public EndlessFuncAni EndlessFuncAni(Action<EndlessFuncAni> update)
            => get<EndlessFuncAni>().Set(update);
        public IntervalAni IntervalAni(double seconds, Action<IntervalAni> onTick)
            => get<IntervalAni>().Set(seconds, onTick);
        public EndlessFuncAni EndlessFuncAni(Action<EndlessFuncAni> init, Action<EndlessFuncAni> update)
            => get<EndlessFuncAni>().Set(init, update);
        public EndlessDrawFuncAni EndlessDrawFuncAni(Action<EndlessDrawFuncAni> action)
            => get<EndlessDrawFuncAni>().Set(action);
        public WaitAni WaitFor(double seconds)
            => get<WaitAni>().Set(seconds).PlayOn(this);
        public T WaitForThenStart<T>(double seconds) where T : IAnimation
            => get<WaitAni>().Set(seconds).PlayOn(this).ThenStart<T>();
        public EndlessFuncAni WaitUntil(Func<bool> endCondition)
            => get<EndlessFuncAni>().Set(e =>
            {
                if (endCondition()) e.Finish();
            }).PlayOn(this);

        public override void Finish()
        {
            base.Finish();

            ClearAll();
        }
    }
}