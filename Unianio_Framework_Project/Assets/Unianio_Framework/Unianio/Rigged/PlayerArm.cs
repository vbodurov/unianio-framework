//TODO:enable
/*using System;
using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.Genesis.IK;
using Unianio.Graphs;
using Unianio.IK;
using Unianio.Rigged.IK;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;
using UnityEngine.Rendering;
using static Unianio.Static.fun;

namespace Unianio.Rigged
{
    public interface IPlayerArm : IUpdatable
    {
        IPlayerManager Player { get; }
        Transform Model { get; }
        Vector3 Ix0Pos { get; }
        GenBoneWrapper LowerArm { get; }
        GenBoneWrapper Wrist { get; }
        GenBoneWrapper Thumb0 { get; }
        GenBoneWrapper Thumb1 { get; }
        HumBoneHandler Thumb2 { get; }
        GenBoneWrapper Index0 { get; }
        GenBoneWrapper Middle0 { get; }
        GenBoneWrapper Ring0 { get; }
        GenBoneWrapper Pinky0 { get; }
        FingerChain IndexChain { get; }
        FingerChain MiddleChain { get; }
        FingerChain RingChain { get; }
        FingerChain PinkyChain { get; }
        BodySide Side { get; }
        HandlePath PathIndex { get; }
        HandlePath PathMiddle { get; }
        HandlePath PathRing { get; }
        HandlePath PathPinky { get; }
        Transform Controller { get; }
        HandlePath PathThumb0 { get; }
        HandlePath PathThumb1 { get; }
        HandlePath PathThumb2 { get; }
        void ApplyAllFingerPaths(double x);
        IAnimation HandAni { get; set; }
    }
    public sealed class PlayerArm : IPlayerArm
    {
        const float HeadToNeck = 0.25f;

        readonly IPlayerManager _player;
        readonly Transform _head, _controller, _model;
        readonly GenBoneWrapper
            _lowerArm, _wrist, _thumb0, _thumb1, _index0, _middle0, _ring0, _pinky0
            ;

        readonly HumBoneHandler _thumb2;
        //        readonly ThumbChain _thumbChain;
        readonly FingerChain _indexChain, _middleChain, _ringChain, _pinkyChain;
        readonly BodySide _side;
        readonly Vector3 _relShoulder;
        readonly Quaternion _rotControllerToHand;
        readonly Vector3 _posControllerToHand;
        HandlePath _pathIndex,_pathMiddle,_pathRing,_pathPinky, _pathThumb0, _pathThumb1, _pathThumb2;
        readonly IPlayerArmExtender _extender;
        readonly IUpdatable _updatableExtender;



        public PlayerArm(IPlayerManager player, IResourceManager resources, Transform head, Transform controller, BodySide side)
        {
            _player = player;
            _head = head;
            _controller = controller;
            _side = side;
            _model = 
                resources.LoadAndInstantiate(models.Path + (side == BodySide.LT ? models.LeftArmPlayer : models.RightArmPlayer))
                    .transform;
            _model.gameObject.layer = layers.BitNumber_Characters;
            _model.position = _controller.position;
            _model.rotation = _controller.rotation;
            _model.GetComponentInChildren<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            var t =
                _model.ForEachChild(
                    new Dictionary<string, Transform>(StringComparer.InvariantCultureIgnoreCase),
                    (tr, dic) => dic[tr.name] = tr);
            
            IPlayerArmBonesExtracter ext = new PlayerArmBonesExtracter(side, t);

            var up = point.GetNormalSameSideAs(ext.Index1.position, ext.Wrist.position, ext.Pinky1.position, _model.up);
            var fw = ext.LowerArm1.DirTo(ext.LowerArm2);
            var sn = _side.MapRightLeft(-1, 1);
            //            _rotControllerToHand =
            //                Quaternion.AngleAxis(10 * sn, v3.up) *
            //                Quaternion.AngleAxis(80 * sn, v3.fw)
            //                
            //                ;
            _rotControllerToHand =
                Quaternion.AngleAxis(5 * sn, v3.up) 
                *
                Quaternion.AngleAxis(30, v3.rt) 
                *
                Quaternion.AngleAxis(90 * sn, v3.fw)
                ;
            _posControllerToHand = V3(0, 0, -0.15);
            _relShoulder = new Vector3(0.45f * _side.MapRightLeft(1, -1), 0, 0);
            //            _handRtCorrection = Quaternion.Euler(0, 0, 30 * (_side.IsLeft() ? -1 : 1));//(_side.MapRtLt() == BodySide.LT ? -1 : 1)

            _lowerArm = new GenBoneWrapper(_model, ext.LowerArm2, -fw, up);
            _wrist = new GenBoneWrapper(_model, ext.Wrist, fw, up);
            _thumb0 = new GenBoneWrapper(_model, ext.Thumb0, ext.Thumb0.DirTo(ext.Thumb1), -ext.Thumb0.right);
            _thumb1 = new GenBoneWrapper(_model, ext.Thumb1, ext.Thumb1.DirTo(ext.Thumb2), -ext.Thumb1.right);
            _thumb2 = new HumBoneHandler(_model, ext.Thumb2);
            _index0 = new GenBoneWrapper(_model, ext.Index0, fw, up);
            _middle0 = new GenBoneWrapper(_model, ext.Middle0, fw, up);
            _ring0 = new GenBoneWrapper(_model, ext.Ring0, fw, up);
            _pinky0 = new GenBoneWrapper(_model, ext.Pinky0, fw, up);

            _model.FindFirst("RightArmPlayerModel", "LeftArmPlayerModel").gameObject.layer = 
                layers.BitNumber_Characters;

            const float lastJoinRelLen = 0.98f;

            var wristPos = _wrist.position;
            var middleRootPos = _middle0.Holder.position;
            point.GetNormalSameSideAs(_index0.Holder.position, in wristPos, in middleRootPos, in up, out var indexUp);
            point.GetNormalSameSideAs(_ring0.Holder.position, in wristPos, in middleRootPos, in up, out var ringUp);
            point.GetNormalSameSideAs(_pinky0.Holder.position, in wristPos, in middleRootPos, in up, out var pinkyUp);

//            _thumbChain = new ThumbChain(_side, _model, _wrist.Holder, _thumb0.Holder, ext.Thumb1, ext.Thumb2);
            _indexChain = new FingerChain(FingerName.Index, _side, _model, _index0.Holder, ext.Index1, ext.Index2, ext.Index3, indexUp, vDist(ext.Index2, ext.Index3) * lastJoinRelLen);
            _middleChain = new FingerChain(FingerName.Middle, _side, _model, _middle0.Holder, ext.Middle1, ext.Middle2, ext.Middle3, up, vDist(ext.Middle2, ext.Middle3) * lastJoinRelLen);
            _ringChain = new FingerChain(FingerName.Ring, _side, _model, _ring0.Holder, ext.Ring1, ext.Ring2, ext.Ring3, ringUp, vDist(ext.Ring2, ext.Ring3) * lastJoinRelLen);
            _pinkyChain = new FingerChain(FingerName.Pinky, _side, _model, _pinky0.Holder, ext.Pinky1, ext.Pinky2, ext.Pinky3, pinkyUp, vDist(ext.Pinky2, ext.Pinky3)* lastJoinRelLen);

            _extender = get<IPlayerArmExtender>() ?? VoidPlayerArmExtender.Instance;
            _extender.Initialize(this);
            _updatableExtender = _extender as IUpdatable;

            fire(new PlayerArmInitialized(this));
        }
        IPlayerManager IPlayerArm.Player => _player;
        BodySide IPlayerArm.Side => _side;
        Transform IPlayerArm.Model => _model;
        Transform IPlayerArm.Controller => _controller;
        Vector3 IPlayerArm.Ix0Pos => _index0.position;
        GenBoneWrapper IPlayerArm.LowerArm => _lowerArm;
        GenBoneWrapper IPlayerArm.Wrist => _wrist;
        GenBoneWrapper IPlayerArm.Thumb0 => _thumb0;
        GenBoneWrapper IPlayerArm.Thumb1 => _thumb1;
        HumBoneHandler IPlayerArm.Thumb2 => _thumb2;
        GenBoneWrapper IPlayerArm.Index0 => _index0;
        GenBoneWrapper IPlayerArm.Middle0 => _middle0;
        GenBoneWrapper IPlayerArm.Ring0 => _ring0;
        GenBoneWrapper IPlayerArm.Pinky0 => _pinky0;
        FingerChain IPlayerArm.IndexChain => _indexChain;
        FingerChain IPlayerArm.MiddleChain => _middleChain;
        FingerChain IPlayerArm.RingChain => _ringChain;
        FingerChain IPlayerArm.PinkyChain => _pinkyChain;
        HandlePath IPlayerArm.PathThumb0 => _pathThumb0 ?? (_pathThumb0 = new HandlePath(_thumb0));
        HandlePath IPlayerArm.PathThumb1 => _pathThumb1 ?? (_pathThumb1 = new HandlePath(_thumb1));
        HandlePath IPlayerArm.PathThumb2 => _pathThumb2 ?? (_pathThumb2 = new HandlePath(_thumb2));
        HandlePath IPlayerArm.PathIndex => _pathIndex ?? (_pathIndex = new HandlePath(_indexChain));
        HandlePath IPlayerArm.PathMiddle => _pathMiddle ?? (_pathMiddle = new HandlePath(_middleChain));
        HandlePath IPlayerArm.PathRing => _pathRing ?? (_pathRing = new HandlePath(_ringChain));
        HandlePath IPlayerArm.PathPinky => _pathPinky ?? (_pathPinky = new HandlePath(_pinkyChain));
        void IPlayerArm.ApplyAllFingerPaths(double x)
        {
            _pathThumb0?.Apply(x);
            _pathThumb1?.Apply(x);
            _pathThumb2?.Apply(x);
            _pathIndex?.Apply(x);
            _pathMiddle?.Apply(x);
            _pathRing?.Apply(x);
            _pathPinky?.Apply(x);
        }
        IAnimation IPlayerArm.HandAni { get; set; }
        void IUpdatable.Update()
        {
            const float MaxHandDistFromHead = 0.7f;
            var targHandPos = _controller.position + _controller.rotation * _posControllerToHand;
            var distHeadToHand = fun.distance.Between(_head.position, in targHandPos);
            if (distHeadToHand > MaxHandDistFromHead)
                targHandPos = _head.position.MoveTowards(targHandPos, MaxHandDistFromHead);
            var neck = _head.position + _head.rotation * new Vector3(0, -HeadToNeck, 0);
            var shoulder = neck + _head.rotation * _relShoulder;
            var rot = _controller.rotation * _rotControllerToHand;
            var elbow = shoulder + v3.dn.By(0.33);


            var handUp = rot * v3.up;
            _model.position = targHandPos;
            _model.rotation = rot;
            var dirToElbow = elbow - _lowerArm.Holder.position;
            _lowerArm.Holder.rotation = lookAt(in dirToElbow, dirToElbow.GetRealUp(in handUp, rot * v3.bk));
            

            _indexChain.Update();
            _middleChain.Update();
            _ringChain.Update();
            _pinkyChain.Update();

            _updatableExtender?.Update();
        }
    }
}*/