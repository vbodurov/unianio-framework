using System;
using System.Collections.Generic;
using System.Linq;
using Unianio.Services;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Animations
{
    public class AnimationHolder : AnimationBase, IAnimationHolder
    {
        readonly LinkedList<IAnimation> _animations = new LinkedList<IAnimation>();
        readonly LinkedList<IAnimation> _animationsToDraw = new LinkedList<IAnimation>();

        public int NumberAnimations => _animations.Count;
        public int NumberAnimationToDraw => _animationsToDraw.Count;

        public int CountAnimationsRecursive()
        {
            var count = 0;
            foreach (var a in _animations)
            {
                var ah = a as AnimationHolder;
                if(ah == null) ++count;
                else count += ah.CountAnimationsRecursive();
            }
            return count;
        }
        public IAnimation FirstUnfinished
        {
            get
            {
                var aNode = _animations.First;
                while (aNode != null)
                {
                    var a = aNode.Value; 
                    if (!a.IsFinished)
                    {
                        return a;
                    }
                    aNode =  aNode.Next;
                }
                return null;
            }
        }

        public void AddAnimation(IAnimation a)
        {
            if (a == this) throw new InvalidOperationException("You cannot append animation to itself");
            if (a.Priority > 0)
            {
                var curr = _animations.First;
                while (curr != null && curr.Value.Priority > a.Priority)
                {
                    curr = curr.Next;
                }
                if (curr == null) _animations.AddLast(a);
                else _animations.AddBefore(curr, a);
            }
            else //if (a.Priority <= 0)
            {
                var curr = _animations.Last;
                while (curr != null && curr.Value.Priority < a.Priority)
                {
                    curr = curr.Previous;
                }
                if (curr == null) _animations.AddFirst(a);
                else _animations.AddAfter(curr, a);
            }
            if (a.IsGlDrawing)
            {
                _animationsToDraw.AddLast(a);
            }
        }

        public override void Update()
        {
            var aNode = _animations.First;
            while (aNode != null)
            {
                var a = aNode.Value;
                var next = aNode.Next;
                if (!a.IsInitialized)
                {
                    EnsureInitialization(a);
                }
                if (a.IsFinished)
                {
                    var follow = a.FollowBy();

                    a.Dispose();
                    Messenger.Unsubscribe(a);
                    if (follow == null)
                    {
                        if(aNode.List == _animations) // check if the node hasn't been already removed
                            _animations.Remove(aNode);
                    }
                    else
                    {
                        aNode.Value = follow;
                        // inlined for performance
                        if (!follow.IsInitialized)
                        {
                            EnsureInitialization(follow);
                        }
                        // check if it has not been finished in the initialize phase
                        if (!follow.IsFinished)
                        {
                            follow.Update();
                        }
                    }
                }
                else
                {
                    a.Update();
                }
                aNode = next;
            }
        }
        public override void Draw()
        {
            var aNode = _animationsToDraw.First;
            while (aNode != null)
            {
                var a = aNode.Value; 
                var next = aNode.Next;
                if (a.IsFinished)
                {
                    _animationsToDraw.Remove(aNode);
                }
                else if(a.IsInitialized)
                {
                    a.Draw();
                }
                aNode = next;
            }
        }
        public void FinishAllAnimationsInside()
        {
            // we convert it to array in case something is changing the collection in Finish()
            var array = _animations.ToArray();
            for (var i = 0; i < array.Length; ++i)
            {
                var a = array[i];
                a.ClearAllFollowUpActions();
                if (!a.IsFinished)
                {
                    a.Finish();
                }
            }
        }
        public bool AreAllAnimationsFinished
        {
            get
            {
                var aNode = _animations.First;
                while (aNode != null)
                {
                    var a = aNode.Value; 
                    if (!a.IsFinished)
                    {
                        return false;
                    }
                    aNode =  aNode.Next;
                }
                return true;
            }
        }
        public static void EnsureInitialization(IAnimation a)
        {
            if (a.IsFinished)
            {
                a.IsInitialized = true;
                return;
            }
            a.EnsureUniqueness();
            //dbg.log("Init:"+a);
            a.Initialize();
            a.IsInitialized = true;
        }


        protected void ClearAnimationWhen(Func<IAnimation, bool> condition)
        {
            var hasGlDrawAni = false;
            var node = _animations.First;
            while (node != null)
            {
                var a = node.Value;
                var next = node.Next;
                if (condition(a))
                {
                    Messenger.Unsubscribe(a); // clear any subscribtions of that animation

                    if (a.IsGlDrawing && !hasGlDrawAni) hasGlDrawAni = true;
                    a.Finish();
                    a.Dispose();
                    _animations.Remove(node);
                }
                node = next;
            }
            if (hasGlDrawAni)
            {
                var aNode = _animationsToDraw.First;
                while (aNode != null)
                {
                    var a = aNode.Value; 
                    var next = aNode.Next;
                    if (a.IsFinished)
                    {
                        _animationsToDraw.Remove(aNode);
                    }
                    aNode = next;
                }
            }
        }
        public void ClearAll()
        {
            ClearAllAnimations();
        }
        protected void ClearAllAnimations()
        {
            ClearList(_animations, Messenger);
            _animationsToDraw.Clear();
        }
        static void ClearList<TValue>(LinkedList<TValue> list, IMessenger messenger) where TValue : IAnimation
        {
            var node = list.First;
            while (node != null)
            {
                var curr = node.Value;
                var nextNode = node.Next;
                var toFollow = curr.FollowBy();
                while (toFollow != null)
                {
                    toFollow.Dispose();
                    messenger.Unsubscribe(toFollow);
                    toFollow = toFollow.FollowBy();
                }
                curr.Dispose();

                messenger.Unsubscribe(curr);

                node = nextNode;
            }
            list.Clear();
        }
        public override void Dispose()
        {
            base.Dispose();
            ClearAllAnimations();
        }

        
    }
}