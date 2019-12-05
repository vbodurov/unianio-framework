using System;
using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Animations.Common;
//using Unianio.Genesis;
//using Unianio.Genesis.State;
//using Unianio.IK;
//using Unianio.Rigged;
using Unianio.RSG;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class AnimationExtensions
    {
        public static T AddPromise<T>(this T ani, List<IPromise<IAnimation>> promises) where T : IAnimation
        {
            var promise = ani.CreatePromise();
            if (promises == null) return ani;
            promises.Add(promise);
            return ani;
        }
        public static T MustBeUnique<T>(this T ani, ref IAnimation reference) where T : IAnimation
        {
            reference.ForceStopIfRunning();
            reference = ani;
            return ani;
        }
        //        internal static T AssignTo<T>(this T ani, out IAnimation reference) where T : IAnimation
        //        {
        //            reference = ani;
        //            return ani;
        //        }
        public static T MustBeUnique<T>(this T ani, ref T reference) where T : IAnimation
        {
            reference.ForceStopIfRunning();
            reference = ani;
            return ani;
        }
        public static T AssignTo<T>(this T ani, out T reference) where T : IAnimation
        {
            reference = ani;
            return ani;
        }
        public static T AddPromise<T>(this T ani) where T : IAnimation
        {
            ani.CreatePromise();
            return ani;
        }

        //Ani.SpeedUpOrStopThen(fun.smoothDeltaTime*3,() => {})
        public static IAnimation SpeedUpOrStopThen(this IAnimation ani, double seconds, Action then)
        {
            if (ani == null || ani.IsFinished)
            {
                then();
                return ani;
            }

            var timeAni = ani as ITimeBased;
            if (timeAni != null)
            {
                var timePlusSeconds = Time.time + seconds;
                // if hasn't started yet
                if (timeAni.Range.From > Time.time)
                {
                    timeAni.Range.To = timeAni.Range.From + (float)seconds;
                }
                // if in the middle
                else if (timePlusSeconds >= timeAni.Range.From && timePlusSeconds <= timeAni.Range.To)
                {
                    timeAni.Range.ResizeRemainingTo(seconds);
                }
                ani.Then(then);
                return ani;
            }
            ani.Finish();
            then();
            return ani;
        }
        public static T ManuallyInitialize<T>(this T ani) where T : IAnimation
        {
            AnimationHolder.EnsureInitialization(ani);
            return ani;
        }
        public static T ManuallyUpdate<T>(this T ani) where T : IAnimation
        {
            ani.Update();
            return ani;
        }
        public static T ManuallyInitializeAndUpdate<T>(this T ani) where T : IAnimation
        {
            AnimationHolder.EnsureInitialization(ani);
            ani.Update();
            return ani;
        }
        public static bool ForceStopAnyIfRunning(this IList<IAnimation> list)
        {
            if (list == null) return false;
            var hasStopped = false;
            for(var i = 0; i < list.Count; ++i)
            {
                if(list[i] == null) continue;
                var isPlaying = !list[i].IsFinished;
                list[i].IsForcedToFinish = true;
                list[i].ClearAllFollowUpActions();
                list[i].Finish();
                if (isPlaying) hasStopped = true;
            }
            return hasStopped;
        }
        public static T Play<T>(this T ani) where T : IAnimation
        {
            fun.Messenger.Invoke(new PlayAni {Ani = ani});
            return ani;
        }
        public static T Play<T>(this T ani, IMessenger messenger) where T : IAnimation
        {
            messenger.Invoke(new PlayAni {Ani = ani});
            return ani;
        }
        public static T PlayOn<T>(this T ani, IAnimationHolder holder) where T : IAnimation
        {
            holder.AddAnimation(ani);
            return ani;
        }
        public static T OnInit<T>(this T ani, Action<IAnimation> onInit) where T : IAnimation
        {
            if(onInit != null) ani.WhenInitCall(onInit);
            return ani;
        }
        public static T OnEnd<T>(this T ani, Action<T> onEnd) where T : IAnimation
        {
            if(onEnd != null) ani.WhenEndsCall(an => onEnd((T)an));
            return ani;
        }
        public static T Init<T>(this T ani, Action onInit) where T : IAnimation
        {
            ani.WhenInitCall(a => onInit());
            return ani;
        }
        public static T Then<T>(this T ani, Action onEnd) where T : IAnimation
        {
            ani.WhenEndsCall(a => onEnd());
            return ani;
        }
        public static TNext ThenStart<TNext>(this IAnimation ani) 
            where TNext : IAnimation
        {
            var next = GlobalFactory.Default.Get<TNext>();
            ani.SetFollowing(() => next);
            return next;
        }
        public static TNext ThenStart<TNext>(this IAnimation ani, TNext next) 
            where TNext : IAnimation
        {
            ani.SetFollowing(() => next);
            return next;
        }
        public static CycleAni ThenStartCycleAni(this IAnimation ani, double seconds, Action<CycleAni, int> init, Action<float> func)
        {
            var next = GlobalFactory.Default.Get<CycleAni>().Set(seconds, init, func);
            ani.SetFollowing(() => next);
            return next;
        }
        public static FuncAni ThenStartFuncAni(this IAnimation ani, double seconds, Action<float> func) 
        {
            var next = GlobalFactory.Default.Get<FuncAni>().Set(seconds, func);
            ani.SetFollowing(() => next);
            return next;
        }
        public static FuncAni ThenStartFuncAni(this IAnimation ani, double seconds, Action<float> func, out FuncAni next)
        {
            next = GlobalFactory.Default.Get<FuncAni>().Set(seconds, func);
            var nextAni = next;
            ani.SetFollowing(() => nextAni);
            return next;
        }
        public static FuncAni ThenStartFuncAni(this IAnimation ani, double seconds, Action<FuncAni> init, Action<float> func)
        {
            var next = GlobalFactory.Default.Get<FuncAni>().Set(seconds, init, func);
            ani.SetFollowing(() => next);
            return next;
        }
        public static FuncAni ThenStartFuncAni(this IAnimation ani, double seconds, Action<FuncAni> init, Action<FuncAni, float> func)
        {
            var next = GlobalFactory.Default.Get<FuncAni>().Set(seconds, init, func);
            ani.SetFollowing(() => next);
            return next;
        }
        public static EndlessFuncAni ThenStartEndlessFuncAni(this IAnimation ani, Action<EndlessFuncAni> update)
        {
            var next = GlobalFactory.Default.Get<EndlessFuncAni>().Set(update);
            ani.SetFollowing(() => next);
            return next;
        }
        public static EndlessFuncAni ThenStartEndlessFuncAni(this IAnimation ani, Action<EndlessFuncAni> update, out EndlessFuncAni efa)
        {
            var next = GlobalFactory.Default.Get<EndlessFuncAni>().Set(update);
            efa = next;
            ani.SetFollowing(() => next);
            return next;
        }
        public static EndlessFuncAni ThenStartEndlessFuncAni(this IAnimation ani, Action<EndlessFuncAni> init, Action<EndlessFuncAni> update)
        {
            var next = GlobalFactory.Default.Get<EndlessFuncAni>().Set(init, update);
            ani.SetFollowing(() => next);
            return next;
        }
        public static WaitAni ThenWaitFor<T>(this T ani, double seconds) 
            where T : IAnimation
        {
            var wait = GlobalFactory.Default.Get<WaitAni>().Set(seconds);
            ani.SetFollowing(() => wait);
            return wait;
        }
        public static TAni ThenFire<TAni,TEvent>(this TAni ani, TEvent ev) 
            where TAni : IAnimation
            where TEvent : BaseEvent
        {
            ani.OnEnd(a =>
            {
//                Debug.Log("FIRE " + ev);
                fun.Messenger.Invoke(ev);
            });
            return ani;
        }
        public static TAni ThenFire<TAni, TEvent>(this TAni ani, Func<TEvent> func)
            where TAni : IAnimation
            where TEvent : BaseEvent
        {
            ani.OnEnd(a =>
            {
                //                Debug.Log("FIRE " + ev);
                fun.Messenger.Invoke(func());
            });
            return ani;
        }
        public static FuncAni ThenPlayFor(this IAnimation ani, double seconds, Action<float> func) 
        {
            var funcAni = GlobalFactory.Default.Get<FuncAni>().Set(seconds, func);
            ani.SetFollowing(() => funcAni);
            return funcAni;
        }
        public static FuncAni ThenPlayFor(this IAnimation ani, double seconds, 
            Action<FuncAni> init, Action<float> func)
        {
            var funcAni = GlobalFactory.Default.Get<FuncAni>().Set(seconds, init, func);
            ani.SetFollowing(() => funcAni);
            return funcAni;
        }
        public static bool StopIfRunning<T>(this T ani) where T : IAnimation
        {
            if (ani != null && !ani.IsFinished)
            {
                ani.Finish();
                return true;
            }
            return false;
        }
        public static bool ForceStopIfRunning<T>(this T ani) where T : IAnimation
        {
            if (ani != null && !ani.IsFinished)
            {
                ani.IsForcedToFinish = true;
                ani.ClearAllFollowUpActions();
                ani.Finish();
                return true;
            }
            return false;
        }
        public static bool StopIfRunningWithLabel<T>(this T ani, ulong label) where T : IAnimation
        {
            if (ani != null && !ani.IsFinished && (ani.Label & label) > 0)
            {
                ani.Finish();
                return true;
            }
            return false;
        }
        public static bool ForceStopIfRunningWithLabel<T>(this T ani, ulong label) where T : IAnimation
        {
            if (ani != null && !ani.IsFinished && (ani.Label & label) > 0)
            {
                ani.IsForcedToFinish = true;
                ani.ClearAllFollowUpActions();
                ani.Finish();
                return true;
            }
            return false;
        }
        public static bool StopAndDispose<T>(this T ani) where T : IAnimation
        {
            if (ani != null)
            {
                if(!ani.IsFinished) ani.Finish();
                if(!ani.IsDisposed) ani.Dispose();
                return true;
            }
            return false;
        }
        public static bool IsRunning<T>(this T ani) where T : IAnimation
        {
            return ani != null && !ani.IsFinished;
        }
        public static bool IsNotRunning<T>(this T ani) where T : IAnimation
        {
            return ani == null || ani.IsFinished;
        }
        public static TAni SetLabel<TAni>(this TAni ani, ulong label) where TAni : IAnimation
        {
            if (ani == null) return default(TAni);
            ani.Label |= label;
            return ani;
        }
        public static TAni SetPriority<TAni>(this TAni ani, int priority) where TAni : IAnimation
        {
            ani.Priority = priority;
            return ani;
        }
        public static TAni ClearLabel<TAni>(this TAni ani, ulong label) where TAni : IAnimation
        {
            if (ani == null) return default(TAni);
            ani.Label &= ~label;
            return ani;
        }
        public static TAni ClearAllLabels<TAni>(this TAni ani) where TAni : IAnimation
        {
            if (ani == null) return default(TAni);
            ani.Label = 0;
            return ani;
        }
        //TODO:enable
        /*
        public static TAni Register<TAni>(this TAni ani, IAnimatedHumanoid om, HumanoidPart part) where TAni : IAnimation
        {
            if (part == HumanoidPart.EntireBody) om.AniEntireBody = ani;
            if (part == HumanoidPart.Face) om.AniFace = ani;
            if (part == HumanoidPart.BothEyelids) om.AniBlink = ani;
            if (part == HumanoidPart.Spine) om.AniSpine = ani;
            if (part == HumanoidPart.Head) om.AniHead = ani;
            if (part == HumanoidPart.ArmL) om.AniArmL = ani;
            if (part == HumanoidPart.ArmR) om.AniArmR = ani;
            if (part == HumanoidPart.LegL) om.AniLegL = ani;
            if (part == HumanoidPart.LegR) om.AniLegR = ani;
            if (part == HumanoidPart.Torso) om.AniTorso = ani;
            if (part == HumanoidPart.HandL) om.AniHandL = ani;
            if (part == HumanoidPart.HandR) om.AniHandR = ani;
            if (part == HumanoidPart.BreastL) om.AniBreastL = ani;
            if (part == HumanoidPart.BreastR) om.AniBreastR = ani;
            if (part == HumanoidPart.BottomL) om.AniBottomL = ani;
            if (part == HumanoidPart.BottomR) om.AniBottomR = ani;
            return ani;
        }
        */
        public static bool HasLabel(this IAnimation ani, ulong label)
        {
            return ani != null && (ani.Label & label) > 0;
        }
        public static bool IsRunningWithLabel(this IAnimation ani, ulong label)
        {
            return ani != null && !ani.IsFinished && (ani.Label & label) > 0;
        }
        public static bool IsRunningButIsNotLabeledAs(this IAnimation ani, ulong label)
        {
            return ani != null && !ani.IsFinished && (ani.Label & label) == 0;
        }
        public static bool IsNotRunningWithLabel(this IAnimation ani, ulong label)
        {
            return ani == null || ani.IsFinished || (ani.Label & label) == 0;
        }

        public static IAnimation AsUniqueOf<TAni>(this IAnimation ani) where TAni : IAnimation
        {
            ani.UniquenessId = fun.GetUniquenessId<TAni>();
            return ani;
        }
        public static IAnimation AsUniqueOfFor<TAni>(this IAnimation ani, object id) where TAni : IAnimation
        {
            ani.UniquenessId = fun.GetUniquenessIdFor<TAni>(id);
            return ani;
        }
        public static TAni AsUnique<TAni>(this TAni ani) where TAni : IAnimation
        {
            ani.UniquenessId = fun.GetUniquenessId<TAni>();
            return ani;
        }
        public static TAni AsFinished<TAni>(this TAni ani) where TAni : IAnimation
        {
            ani.Finish();
            return ani;
        }
        public static TAni AsUniqueFor<TAni>(this TAni ani, object id) where TAni : IAnimation
        {
            ani.UniquenessId = fun.GetUniquenessIdFor<TAni>(id);
            return ani;
        }
        public static TAni AsUniqueNamed<TAni>(this TAni ani, string name) where TAni : IAnimation
        {
            ani.UniquenessId = name;
            return ani;
        }
        public static TAni AsUniqueNamedFor<TAni>(this TAni ani, string name, object id) where TAni : IAnimation
        {
            ani.UniquenessId = name + id;
            return ani;
        }

        public static TState GetState<TState>(this IAnimation a)
        {
            return (TState)((StateHolderAni) a).State;
        }
        public static TState GetState<TState,TAni>(this TAni a) where TAni : StateHolderAni
        {
            return (TState)a.State;
        }

        public static ulong GetLabel(this IAnimation ani)
        {
            return ani?.Label ?? (ulong) 0;
        }
    }
}