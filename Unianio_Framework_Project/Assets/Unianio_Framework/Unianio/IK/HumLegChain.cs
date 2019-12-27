using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Human;
using Unianio.Moves;
using Unianio.Rigged;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public class HumLegChain : BaseIkChain, IInitialOrientationHolder
    {
        readonly BodyPart _part;
        readonly IComplexHumanDefinition _definition;
        public readonly Transform Pelvis, ThighBend, ThighTwist, Shin, Foot,
            Metatarsals, Toe,
            BigToe, BigToe2,
            SmallToe11, SmallToe12,
            SmallToe21, SmallToe22,
            SmallToe31, SmallToe32,
            SmallToe41, SmallToe42;

        public readonly HumBoneHandler FootHandler;
        public readonly HumBoneHandler ToeHandler;

        public bool AutoPositionFoot { get; set; }
        public bool AutoPositionToes { get; set; }
        public float FootToKneeLength => _footToKneeLength;
        public float KneeToHipLength => _kneeToHipLength;

        readonly float _footToKneeLength, _kneeToHipLength, _maxStretch, _toeInitialY;
        readonly Vector3 _leNormalModelSpace, _rightPlaneNormalModelSpace,
            _topPlaneNormalModelSpace,
            _leftPlaneNormalModelSpace;

        readonly bool _isRight;
        public float MaxStretch => _maxStretch;
        public Transform Model => _model;

        public HumLegChain(BodyPart part, IComplexHumanDefinition definition) : base(part)
        {
            _part = part;
            _isRight = part == BodyPart.LegR;
            _definition = definition;
            _model = definition.Model;
            var modelFw = _model.forward;
            var modelUp = _model.up;
//            var modelSide = _isRight ? _model.right : -_model.right;
            var leg = part == BodyPart.LegL ? definition.LegL : definition.LegR;

            Pelvis = leg.ThighBend.parent;

            AutoPositionFoot = true;
            AutoPositionToes = false;

            ThighBend = leg.ThighBend;
            ThighTwist = leg.ThighTwist;
            Shin = leg.Shin;

            Foot = leg.FootHolder ?? leg.Foot;
            Metatarsals = leg.Metatarsals;
            Toe = leg.ToeHolder ?? leg.Toe;

            FootHandler = new HumBoneHandler(_isRight ? BodyPart.FootR : BodyPart.FootL, _model, Foot);
            ToeHandler = new HumBoneHandler(_isRight ? BodyPart.ToesR : BodyPart.ToesL, _model, Toe);

            _handle = CreateHandle(Pelvis, _part, false, Foot.position, modelFw, modelUp);

            BigToe = leg.BigToe;
            BigToe2 = leg.BigToe2;
            SmallToe11 = leg.SmallToe11;
            SmallToe12 = leg.SmallToe12;
            SmallToe21 = leg.SmallToe21;
            SmallToe22 = leg.SmallToe22;
            SmallToe31 = leg.SmallToe31;
            SmallToe32 = leg.SmallToe32;
            SmallToe41 = leg.SmallToe41;
            SmallToe42 = leg.SmallToe42;

            _toeInitialY = Toe.position.y - _model.position.y;
            _leftPlaneNormalModelSpace = _model.InverseTransformDirection((_model.right).RotateAbout(_model.forward, -35));
            _rightPlaneNormalModelSpace = _model.InverseTransformDirection((-_model.right).RotateAbout(_model.forward, 35));
            _topPlaneNormalModelSpace = _model.InverseTransformDirection((-_model.up).RotateAbout(_model.right, -20));
            _footToKneeLength = fun.distance.Between(Foot.position, Shin.position);
            _kneeToHipLength = fun.distance.Between(Shin.position, ThighBend.position);
//dbg.DrawChain(this.GetHashCode(), Color.cyan, Foot.position, Shin.position, ThighBend.position);
            _maxStretch = _footToKneeLength + _kneeToHipLength;

            IniLocalPos = _handle.localPosition;
            IniModelPos = _handle.position.AsLocalPoint(_model);
            IniLocalRot = _handle.localRotation;
            IniModelRot = lookAt(_handle.forward.AsLocalDir(_model), _handle.up.AsLocalDir(_model));

        }
        public BodySide Side => _isRight ? BodySide.Right : BodySide.Left;
        public Quaternion rotation
        {
            get => _handle.rotation;
            set => _handle.rotation = value;
        }
        public Vector3 position
        {
            get => _handle.position;
            set => _handle.position = value;
        }
        public Vector3 IniLocalPos { get; }
        public Vector3 IniModelPos { get; }
        public Quaternion IniLocalRot { get; }
        public Quaternion IniModelRot { get; }
        public Vector3 IniLocalSca => v3.one;
        public Vector3 IniLocalFw => IniLocalRot * Vector3.forward;
        public Vector3 IniLocalUp => IniLocalRot * Vector3.up;
        public Vector3 IniModelFw => IniModelRot * Vector3.forward;
        public Vector3 IniModelUp => IniModelRot * Vector3.up;
        public IExecutorOfProgress ToInitialLocalPosition()
        {
            return new Mover<HumLegChain>(this).New().Local.LineTo(b => b.IniLocalPos);
        }
        public IExecutorOfProgress ToInitialLocalRotation()
        {
            return new Mover<HumLegChain>(this).New().Local.RotateTo(b => b.IniLocalRot);
        }
        public IExecutorOfProgress ToInitialLocalScale()
        {
            return new Mover<HumLegChain>(this).New().Local.ScaleTo(b => b.IniLocalSca);
        }
        public IExecutorOfProgress ToInitialLocal()
        {
            return new Mover<HumLegChain>(this).New().Local
                    .LineTo(b => b.IniLocalPos)
                    .RotateTo(b => b.IniLocalRot)
                    .ScaleTo(b => b.IniLocalSca)
                ;
        }

        public Vector3 SideDir => _isRight ? v3.rt : v3.lt;
        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange)
        {
            var handlePos = _handle.position;
            var handleRot = _handle.rotation;
            var upLegPos = ThighBend.position;
            var thighToHandleDir = (handlePos - upLegPos).ToUnit(out var dist);
            var handleFw = handleRot * Vector3.forward;
            var handleDn = handleRot * Vector3.down;
            var bendDir = thighToHandleDir.GetRealUp(in handleFw, in handleDn);


            //      [Shin]
            //         ^
            //        /|\  
            //     a / |h\ b
            //      /__|__\ angle "beta"
            // [Foot]  c  [ThighBend]

            var a = _footToKneeLength;
            var b = _kneeToHipLength;
            var c = dist;
            var h = triangle.GetHeight(a, b, c);
            var beta = Math.Asin(h / b) * RTD;
            var dirToShin = thighToHandleDir.RotateTowards(in bendDir, beta);
            var shinPos = upLegPos + dirToShin * _kneeToHipLength;
            var dirToFoot = (handlePos - shinPos).normalized;

            var thighUp = dirToShin.GetRealUp(Pelvis.forward, -Pelvis.up);

            ThighBend.rotation = Quaternion.LookRotation(dirToShin, slerp(in thighUp, in bendDir, 0.15));
            ThighTwist.rotation = Quaternion.LookRotation(ThighTwist.position.DirTo(shinPos), slerp(in thighUp, in bendDir, 0.5));
            Shin.rotation = Quaternion.LookRotation(dirToFoot, bendDir);

            if (AutoPositionFoot)
            {
                Foot.rotation = _handle.rotation;
            }
            if (AutoPositionToes)
            {
                var y = Toe.position.y;
                var turn01 = y.FromRangeTo01(_toeInitialY + 0.03, _toeInitialY).Clamp01();
                if (turn01 < 0.0001) Toe.rotation = Foot.rotation;
                else
                {
                    var targetRot = Quaternion.LookRotation(Foot.forward.ToHorzUnit().RotateTowards(v3.up, 10), v3.up);
                    Toe.rotation = turn01 > 0.9999 ? targetRot : slerp(Foot.rotation, targetRot, turn01);
                }
            }
        }
        /*internal HumLegChain EnsurePosition(in Vector3 candidate)
        {
            var pos = candidate;
            var cen = V3(0, -_kneeToHipLength * 0.8, 0).AsWorldPoint(_definition.Pelvis.Bone);
            var nor = -_definition.Pelvis.Bone.up;

            if (point.IsBelowPlane(in pos, in nor, in cen))
            {
                point.ProjectOnPlane(in pos, in nor, in cen, out pos);
            }

            Handle.position = pos;
            return this;
        }
        public HumLegChain SetOptimalRotation(double degreesPerSec)
        {
            var up = Handle.position.DirTo(ThighBend.position);
            var fw = fun.point.GetNormalSameSideAs(Handle.position, ThighBend.position,
                        ThighBend.position + _definition.Hip.Bone.right, _model.forward);
            var targRot = lookAt(in fw, in up);
            Handle.RotateTowards(targRot, fun.smoothDeltaTime * degreesPerSec);
            return this;
        }*/
    }
}