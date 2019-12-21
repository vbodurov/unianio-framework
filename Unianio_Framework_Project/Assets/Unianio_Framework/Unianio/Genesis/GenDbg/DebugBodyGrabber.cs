// TODO:enable
/*using System;
using Unianio.Animations;
using Unianio.Animations.Common;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis.GenDbg
{
    public sealed class DebugBodyGrabber
    {
        readonly IPlayerManager _player;
        IComplexHuman _human;
        Vector3 _relPivot, _absVecToPivot;
        IAnimation _ani;
        bool _isRotationOn;
        public DebugBodyGrabber(IPlayerManager player)
        {
            _player = player;
        }
        public DebugBodyGrabber Set(IComplexHuman human)
        {
            _human = human;
            return Initialize();
        }
        DebugBodyGrabber Initialize()
        {
            subscribe<BtnTriggerPress>(OnBtnTriggerPress, this);
            return this;
        }
        void OnBtnTriggerPress(BtnTriggerPress e)
        {
            _isRotationOn = e.Side.IsRt();
            ifElseDo(e.IsPressed, OnGrabStarts, OnGrabEnds);
        }
        void OnGrabStarts()
        {
            var m = _human.Model;
            var controller = _isRotationOn ? _player.RightControllerTransform : _player.LeftControllerTransform;
            var cp = controller.position;
            var h1 = _human.Head.position;
            var h2 = _human.Pelvis.position;
            fun.point.ClosestOnLineSegment(in cp, in h1, in h2, out var closest);
            _absVecToPivot = closest - cp;
            _relPivot = (closest - _human.position).AsLocalVec(m);
            var rotDiff = controller.rotation.DifferenceTo(_human.rotation);


            _ani.StopIfRunning();
            _ani = play<EndlessFuncAni>().Set(a =>
            {
                var absPivot = controller.position + _absVecToPivot;
                if (_isRotationOn)
                {
                    _human.position += absPivot;
                    _human.rotation = controller.rotation * rotDiff;
                    _human.position -= absPivot;
                }
                _human.position = absPivot - _relPivot.AsWorldVec(m);
            });
        }
        void OnGrabEnds()
        {
#if UNIANIO_DEBUG
            dbg.ClearLine(GetHashCode());
#endif
            _ani.StopIfRunning();
        }
        bool HasGrab => _ani.IsRunning();
    }
}*/