using System;
using System.Collections.Generic;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Rigged.IK
{
    public interface IThreeJoinChain
    {
        Transform Root { get; }
        Transform Join0 { get; }
        Transform Join1 { get; }
        Transform Join2 { get; }
        float Length { get; }
        float Join0Len { get; }
        float Join1Len { get; }
        float Join2Len { get; }
        Transform[] AllJoins { get; }
    }

    public sealed class FingerChain : BaseChain, IThreeJoinChain, IInitialOrientationHolder
    {
        readonly FingerName _finger;
        readonly BodySide _side;
        readonly Transform _root,_join0,_join1,_join2;
        readonly Transform[] _allJoins;
        readonly float _len0, _len1, _len2, _length;
        readonly Vector3[] _points = new Vector3[4];
        readonly float[] _lengths;
        public FingerChain(
            FingerName finger, BodySide side, Transform model, Transform parent, 
            Transform srcJoin0, Transform srcJoin1, Transform srcJoin2, Vector3 upDirInWorld,double lastJoinDist)
            : base(ToHumanPart(finger, side))
        {
            _finger = finger;
            _side = side;
            _model = model;
            var sideSuffix = side == BodySide.Left ? "L" : "R";
            _root = new GameObject(finger+sideSuffix+HolderSuffix).transform;
            _handle = new GameObject(finger+sideSuffix+HandleSuffix).transform;
            _root.position = srcJoin0.position;
            _root.rotation = Quaternion.LookRotation((srcJoin1.position - _root.position).normalized, upDirInWorld);
            _root.SetParent(parent);
            
            _canChangeRotation = false;
            
            _join0 = WrapInHolder(_root, srcJoin0, srcJoin1.position, upDirInWorld);
            _join1 = WrapInHolder(_join0, srcJoin1, srcJoin2.position, upDirInWorld);
            _join2 = WrapInHolder(_join1, srcJoin2, srcJoin2.position + srcJoin2.forward, upDirInWorld);
            _allJoins = new []{_join0,_join1,_join2};
            _len0 = distance.Between(_join0.position, _join1.position);
            _len1 = distance.Between(_join1.position, _join2.position);
            _len2 = (float)lastJoinDist;
            _length = _len0 + _len1 + _len2;
            _handle.position = _join2.position + _join2.forward * _len2;
            _handle.SetParent(_root);
            _iniLocalPos = _handle.localPosition;
            _iniModelPos = _handle.position.AsLocalPoint(_model);
            _iniLocalFw = _handle.localRotation * v3.fw;
            _iniLocalUp = _handle.localRotation * v3.up;
            _iniLocalRot = _handle.localRotation;
            _iniModelRot = lookAt(_handle.forward.AsLocalDir(_model), _handle.up.AsLocalDir(_model));
            _iniModelFw = _iniModelRot * v3.fw;
            _iniModelUp = _iniModelRot * v3.up;
            _lengths = new[] { _len0, _len1, _len2 };
        }

        static HumanoidPart ToHumanPart(FingerName finger, BodySide side)
        {
            var isLeft = side == BodySide.Left;
            if(finger == FingerName.Thumb) return isLeft ? HumanoidPart.FingerThumbL : HumanoidPart.FingerThumbR;
            if(finger == FingerName.Index) return isLeft ? HumanoidPart.FingerIndexL : HumanoidPart.FingerIndexR;
            if(finger == FingerName.Middle) return isLeft ? HumanoidPart.FingerMiddleL : HumanoidPart.FingerMiddleR;
            if(finger == FingerName.Ring) return isLeft ? HumanoidPart.FingerRingL : HumanoidPart.FingerRingR;
            if(finger == FingerName.Pinky) return isLeft ? HumanoidPart.FingerPinkyL : HumanoidPart.FingerPinkyR;
            throw new ArgumentException("Unknown finger="+finger+" and side="+side);
        }
        public BodySide BodySide => _side;
        public FingerName Finger => _finger;
        public Transform Root => _root;
        public Transform Join0 => _join0;
        public Transform Join1 => _join1;
        public Transform Join2 => _join2;
        public float Length => _length;
        public float Join0Len => _len0;
        public float Join1Len => _len1;
        public float Join2Len => _len2;
        public Vector3 Loc(double degreesSide, double up, double fw)
        {
            var sideSign = _side.IsLeft() ? 1 : -1;
            return v3.forward.RotateAbout(v3.up, degreesSide*sideSign) * (float)(fw * _length) + v3.up * (float)(up * _length);
        }
        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange)
        {
            var oriPo = _root.position;
            var oriFw = _root.forward;
            var oriUp = _root.up;
            var tarPo = _handle.position;

            Vector3 join1, join2, norm;
            inverseKinematics.Finger(oriPo, oriFw, oriUp, tarPo, _points, _lengths, out join1, out join2, out norm);

            var toJoin1 = (join1 - oriPo).normalized;
            var toJoin2 = (join2 - join1).normalized;
            var toTarg =  (tarPo - join2).normalized;
//dbg.DrawChain(GetHashCode(),dbg.GetColor(this), oriPo, join1, join2, tarPo);
            _join0.rotation = Quaternion.LookRotation(toJoin1, norm);
            _join1.rotation = Quaternion.LookRotation(toJoin2, toJoin2.GetRealUp(_join0.rotation * v3.up, _join0.rotation * v3.forward));
            _join2.rotation = Quaternion.LookRotation(toTarg, toTarg.GetRealUp(_join1.rotation * v3.up, _join1.rotation * v3.forward));
        }
        public IEnumerable<Transform> AllJoinsWithHandle
        {
            get
            {
                yield return _join0;
                yield return _join1;
                yield return _join2;
                yield return _handle;
            }
        }
        public override Transform[] AllJoins => _allJoins;
        public override Transform Model => _model;
        public override Transform Manipulator => _handle;

        Vector3 _iniLocalPos, _iniModelPos, _iniLocalFw, _iniLocalUp, _iniModelFw, _iniModelUp;
        Quaternion _iniLocalRot, _iniModelRot;
        public Vector3 IniLocalPos => _iniLocalPos;
        public Vector3 IniModelPos => _iniModelPos;
        public Quaternion IniLocalRot => _iniLocalRot;
        public Quaternion IniModelRot => _iniModelRot;
        public Vector3 IniLocalFw => _iniLocalFw;
        public Vector3 IniLocalUp => _iniLocalUp;
        public Vector3 IniModelFw => _iniModelFw;
        public Vector3 IniModelUp => _iniModelUp;
    }
}