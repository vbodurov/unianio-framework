using System;
using System.Collections.Generic;
using System.Text;
using Unianio.Animations.Common;
using Unianio.Extensions;
using Unianio.RSG;
using Unianio.Services;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Animations
{
    internal static class IdSequence { internal static ulong LastId = 100; }
    public abstract class AnimationBase : IAnimation
    {
        Func<IAnimation> _followBy;
        Action<IAnimation> _onInit;
        Action<IAnimation> _onEnd;
        bool _isInitialized, _isFinished;
        Promise<IAnimation> _promise;
        int _playerStartedOnFrame = 0;


        protected AnimationBase()
        {
            ID = ++IdSequence.LastId;
        }
        public int PlayerStartedOnFrame => _playerStartedOnFrame;
        public virtual IAnimation MarkAsPlayerAction()
        {
            if (_playerStartedOnFrame > 0) return this;
            _playerStartedOnFrame = Time.frameCount;
            return this;
        }
        public ulong ID { get; private set; }
        public virtual IAnimation ClearAllFollowUpActions()
        {
            _followBy = null;
            _onEnd = null;
            return this;
        }
        public virtual IPromise<IAnimation> CreatePromise()
        {
            return _promise = new Promise<IAnimation>();
        }
        public IPromise<IAnimation> Promise => _promise;
        public virtual void Dispose()
        {
            if(!IsFinished) IsFinished = true;
            IsDisposed = true;
        }

        public bool IsFinished
        {
            get => _isFinished;
            private set
            {
                if(_isFinished) return;
                _isFinished = value;
                _promise?.Resolve(this);
            }
        }
        public bool IsForcedToFinish { get; set; }
        public virtual bool IsDisposed { get; private set; }
        public virtual int Priority { get; set; }

        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_onInit != null && !_isInitialized && value)
                {
                    _onInit(this);
                }
                else if (!value && _isInitialized)
                {
                    throw new InvalidOperationException("Attempt to revert animation initialization "+this);
                }

                _isInitialized = value;
            }
        }
        public bool IsGlDrawing { get; protected set; }
        public ulong Label { get; set; }

        public string UniquenessId { get; set; }
        public virtual void Initialize() { }
        protected virtual void OnFinishWhenNotForced() { }
        protected virtual void OnForcedToFinish() { }
        public virtual void Update() { }
        public virtual void Draw() { }
        public virtual void Finish()
        {
            if (IsFinished)
            {
                return;
            }
            IsFinished = true;
            if(IsForcedToFinish)
            {
                OnForcedToFinish();
                return;// if kicked out by other with that unique name, then do not do the finalization actions
            }
            _onEnd?.Invoke(this);
            OnFinishWhenNotForced();
        }
        public virtual void SilentFinish()
        {
            if (IsFinished)
            {
                return;
            }
            IsFinished = true;
        }
        public void SetOnEnd(Action<IAnimation> onEnd) { _onEnd = onEnd; }
        public void SetOnInit(Action<IAnimation> onInit) { _onInit = onInit; }
        public void WhenInitCall(Action<IAnimation> onInit)
        {
            if (_onInit != null)
            {
                var old = _onInit;
                _onInit = a =>
                {
                    old(a);
                    _onInit(a);
                };
            }
            else _onInit = onInit;
        }
        public void WhenEndsCall(Action<IAnimation> onEnd)
        {
            if (_onEnd != null)
            {
                var old = _onEnd;
                _onEnd = a =>
                {
                    old(a);
                    onEnd(a);
                };
            }
            else _onEnd = onEnd;
        }
        public IAnimation FollowBy()
        {
            var nextAni = _followBy?.Invoke();
            if (nextAni != null)
            {
                // if the current (source) animation is state holder with state
                if (this is StateHolderAni shaSrc && shaSrc.State != null)
                {
                    // if the next (target) animation is state holder with no state
                    if (nextAni is StateHolderAni shaTar && shaTar.State == null)
                    {
                        // pass along the current state
                        shaTar.State = shaSrc.State;
                    }
                }
            }
            return nextAni;
        }
        public void SetFollowing(Func<IAnimation> getAnimation)
        {
            _followBy = getAnimation;
        }
        public void SetFollowSequence(params AnimationBase[] animation)
        {
            if (animation == null) return;
            var prev = this;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < animation.Length; ++i)
            {
                var next = animation[i];
                prev.SetFollowing(() => next);
                prev = next;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(GetType().Name);
            sb.Append("_hash_").Append(GetHashCode());
            if(!string.IsNullOrEmpty(UniquenessId)) sb.Append("_unique_").Append(UniquenessId);
            if(Label != 0) sb.Append("_label_").Append(Label);
            if(IsFinished) sb.Append("_finished");
            if(IsDisposed) sb.Append("_disposed");
            return sb.ToString();
        }
        public void EnsureUniqueness()
        {
            if(string.IsNullOrEmpty(UniquenessId)) return;

            // push out all previous character root animations
            Messenger.Invoke(new EnsureNamedUniqueEvent(UniquenessId, this));
//Debug.Log("Subscribe>>"+ _uniquenessId, dbg.st);
            // prepare yourself to be pushed by the next animation
            Messenger.Subscribe<EnsureNamedUniqueEvent>(this, UniquenessId, OnEnsureNamedUniqueEvent);
        }

        private void OnEnsureNamedUniqueEvent(EnsureNamedUniqueEvent eue)
        {
            var subscriberAni = eue.Subscriber as IAnimation;
            if (subscriberAni == null)
            {
                Debug.LogError("EnsureNamedUniqueEvent subscriber must be IAnimation while it is '" + eue.Subscriber+"'");
                return;
            }
            if (subscriberAni.IsFinished) return;
//Debug.Log(Time.time+ " Kicked Animation:"+ subscriberAni + " Kicked by:"+eue.Requester+" frame:"+Time.frameCount);
//dbg.log(Time.time + " Kicked Animation:" + subscriberAni, " Kicked by:" + eue.Requester+" frame:"+Time.frameCount);
            if (subscriberAni.IsFinished) return;
            subscriberAni.IsForcedToFinish = true;
            subscriberAni.ClearAllFollowUpActions();
            subscriberAni.Finish();
        }

        protected VecBuilder newVec => new VecBuilder();
        protected DirBuilder newDir => new DirBuilder();
        protected struct VecBuilder
        {
            Vector3 _vec;
            public Vector3 get => _vec;
            public VecBuilder up(double n = 0)
            {
                _vec += v3.up.By(n);
                return this;
            }
            public VecBuilder dn(double n = 0)
            {
                _vec += v3.dn.By(n);
                return this;
            }
            public VecBuilder lt(double n = 0)
            {
                _vec += v3.lt.By(n);
                return this;
            }
            public VecBuilder rt(double n = 0)
            {
                _vec += v3.rt.By(n);
                return this;
            }
            public VecBuilder fw(double n = 0)
            {
                _vec += v3.fw.By(n);
                return this;
            }
            public VecBuilder bk(double n = 0)
            {
                _vec += v3.bk.By(n);
                return this;
            }
        }
        protected struct DirBuilder
        {
            Vector3 _dir;
            internal Vector3 get => _dir;
            public DirBuilder up(double n = 0)
            {
                _dir = _dir.RotUp(n);
                return this;
            }
            public DirBuilder dn(double n = 0)
            {
                _dir = _dir.RotDn(n);
                return this;
            }
            public DirBuilder lt(double n = 0)
            {
                _dir = _dir.RotLt(n);
                return this;
            }
            public DirBuilder rt(double n = 0)
            {
                _dir = _dir.RotRt(n);
                return this;
            }
            public DirBuilder fw(double n = 0)
            {
                _dir = _dir.RotFw(n);
                return this;
            }
            public DirBuilder bk(double n = 0)
            {
                _dir = _dir.RotBk(n);
                return this;
            }
        }
    }

    public class NoAni : AnimationBase
    {
        public static readonly NoAni Finished = new NoAni().Pipe(a => a.Finish());
    }
}