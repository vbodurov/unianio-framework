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
    public class Gen8ArmChain : BaseIkChain, IHumArmChain
    {
        
        readonly bool _isRight;
        readonly float _distHandToElbow,_distElbowToShoulder;
        double _elbowBendFactorM11, _prevElbowBendFactorM11;
        readonly Vector3 _sideDir, _iniCollarPoint, _iniCollarUp;
        protected Vector3? _prevCustomBendDir;
        readonly Vector3[] _iniShoulderPoints, _tempShoulderPoints;
        readonly float[] _shoulderPointLengths;
        public Vector3 LastBenDir;
        public Vector3 ComputedBendDir;
        /*
            var vecR = slerp(slerp(v3.dn, v3.bk, 0.15), v3.lt, 0.15);
            var vecL = slerp(slerp(v3.dn, v3.bk, 0.15), v3.rt, 0.15);
         */
        static readonly Vector3 RelDownAxisR = slerp(slerp(in v3.dn, in v3.bk, 0.15), in v3.lt, 0.5);
        static readonly Vector3 RelDownAxisL = slerp(slerp(in v3.dn, in v3.bk, 0.15), in v3.rt, 0.5);
        public HumBoneHandler HandHandler { get; }
        public Transform ArmRoot => Collar;
        public Transform Collar { get; }
        public Transform Shoulder { get; }
        public Transform Shoulder2 { get; }
        public Transform ShoulderTwist { get; }
        public Transform Forearm { get; }
        public Transform ForearmTwist { get; }
        public Transform Hand { get; }
        public Transform Index0 { get; }
        public Transform Index1 { get; }
        public Transform Index2 { get; }
        public Transform Index3 { get; }
        public Transform Middle0 { get; }
        public Transform Middle1 { get; }
        public Transform Middle2 { get; }
        public Transform Middle3 { get; }
        public Transform Ring0 { get; }
        public Transform Ring1 { get; }
        public Transform Ring2 { get; }
        public Transform Ring3 { get; }
        public Transform Pinky0 { get; }
        public Transform Pinky1 { get; }
        public Transform Pinky2 { get; }
        public Transform Pinky3 { get; }
        public Transform Thumb1 { get; }
        public Transform Thumb2 { get; }
        public Transform Thumb3 { get; }
        public BodySide Side => _isRight ? BodySide.Right : BodySide.Left;
        public bool IsRight => _isRight;
        public Transform Root => Collar;
        public float MaxStretch { get; }
        public bool AutoPositionHand { get; set; }
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
            return new Mover<Gen8ArmChain>(this).New().Local.LineTo(b => b.IniLocalPos);
        }
        public IExecutorOfProgress ToInitialLocalRotation()
        {
            return new Mover<Gen8ArmChain>(this).New().Local.RotateTo(b => b.IniLocalRot);
        }
        public IExecutorOfProgress ToInitialLocalScale()
        {
            return new Mover<Gen8ArmChain>(this).New().Local.ScaleTo(b => b.IniLocalSca);
        }
        public IExecutorOfProgress ToInitialLocal()
        {
            return new Mover<Gen8ArmChain>(this).New().Local
                    .LineTo(b => b.IniLocalPos)
                    .RotateTo(b => b.IniLocalRot)
                    .ScaleTo(b => b.IniLocalSca)
                ;
        }
        public Vector3 SideDir => _sideDir;
        public Vector3 ShoulderToHandDir => Shoulder.position.DirTo(Hand.position);
        public Vector3 ShoulderToHandModelDir => Shoulder.position.DirTo(Hand.position).AsLocalDir(Model);
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
        /// <summary>
        /// how to bend elbow: -1 is near body, 1 is far from body
        /// </summary>
        public double ElbowBendFactor
        {
            get => _elbowBendFactorM11;
            set => _elbowBendFactorM11 = value.ClampMin11();
        }
        
        public Vector3? CustomBendDir { get; set; }

        public void ApproachElbowBendFactor(double target, double step)
        {
            if (!_elbowBendFactorM11.IsEqual(target, 0.001))
                _elbowBendFactorM11 = _elbowBendFactorM11.GoTowards(target, step);
        }
        public void CalculateArmBend(out Vector3 midPoint, out float lengthToElbow)
        {
            throw new NotImplementedException();
        }
        public bool Shakeable
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public float Shake01
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public Gen8ArmChain(BodyPart part, IComplexHumanDefinition definition) : base(part)
        {
            _isRight = part == BodyPart.ArmR;
            _sideDir = _isRight ? v3.rt : v3.lt;
            _model = definition.Model;
            var arm = _isRight ? definition.ArmR : definition.ArmL;

            Collar = arm.Collar;
            Shoulder = arm.Shoulder;
            Shoulder2 = arm.Shoulder2;
            ShoulderTwist = arm.ShoulderTwist;
            Forearm = arm.Forearm;
            ForearmTwist = arm.ForearmTwist;
            /*if (Shoulder2 != null)
            {
                _hasComplexShoulder = true;
                _iniShoulderPoints = new[]
                {
                    Shoulder.position.AsLocalPoint(Collar),
                    Shoulder2.position.AsLocalPoint(Collar),
                    Forearm.position.AsLocalPoint(Collar)
                };
                _tempShoulderPoints = new Vector3[_iniShoulderPoints.Length];
                _shoulderPointLengths = new[]
                {
                    vDist(Shoulder.position, Shoulder2.position),
                    vDist(Shoulder2.position, Forearm.position)
                };
            }*/
            AutoPositionHand = true;
            
            _iniCollarPoint = Collar.localPosition;
            _iniCollarUp = _model.up.AsLocalDir(Collar.parent);

            Hand = arm.Hand;
            _handle = CreateHandle(Collar, Part, false, Hand.position, Hand.forward, Hand.up);

            HandHandler = new HumBoneHandler(_isRight ? BodyPart.HandR : BodyPart.HandL, _model, Hand);

            Index0 = arm.Index0;
            Index1 = arm.Index1;
            Index2 = arm.Index2;
            Index3 = arm.Index3;

            Middle0 = arm.Middle0;
            Middle1 = arm.Middle1;
            Middle2 = arm.Middle2;
            Middle3 = arm.Middle3;

            Ring0 = arm.Ring0;
            Ring1 = arm.Ring1;
            Ring2 = arm.Ring2;
            Ring3 = arm.Ring3;

            Pinky0 = arm.Pinky0;
            Pinky1 = arm.Pinky1;
            Pinky2 = arm.Pinky2;
            Pinky3 = arm.Pinky3;

            Thumb1 = arm.Thumb1;
            Thumb2 = arm.Thumb2;
            Thumb3 = arm.Thumb3;

            _distHandToElbow = fun.distance.Between(Hand.position, Forearm.position);
            _distElbowToShoulder = fun.distance.Between(Forearm.position, Shoulder.position);
            MaxStretch = (_distHandToElbow + _distElbowToShoulder);

            IniLocalPos = _handle.localPosition;
            IniModelPos = _handle.position.AsLocalPoint(_model);
            IniLocalRot = _handle.localRotation;
            IniModelRot = lookAt(_handle.forward.AsLocalDir(_model), _handle.up.AsLocalDir(_model));

            // ReSharper disable once VirtualMemberCallInConstructor
            ProcessMove(true, true);
        }
        public override void Update()
        {
            ProcessMove(true, true);

            //            var currHandlePos = _handle.localPosition;
            //            var currHandleRot = _handle.localRotation;
            //            var customBendDir = CustomBendDir;
            //
            //            var hasPositionChange = !_prevHandlePos.IsEqual(in currHandlePos, 0.00002);
            //            var hasRotationChange = !_prevHandleRot.IsEqual(in currHandleRot, 0.00002);
            //            var hasBendFactorChange = !_prevElbowBendFactorM11.IsEqual(_elbowBendFactorM11, 0.00001);
            //            var hasCustomBendDirChange = (customBendDir.HasValue != _prevCustomBendDir.HasValue) ||
            //                                         (customBendDir.HasValue &&
            //                                          customBendDir.Value.IsNotEqual(_prevCustomBendDir.Value, float.Epsilon));
            //            var hasChange = hasBendFactorChange || hasPositionChange || hasRotationChange || hasCustomBendDirChange;
            //Debug.Log(customBendDir.s()+"|"+ _prevCustomBendDir.s()+"|"+ hasCustomBendDirChange+"|"+ (!customBendDir.HasValue || !_prevCustomBendDir.HasValue ? "null" : ""+customBendDir.Value.IsNotEqual(_prevCustomBendDir.Value, 0.00001)));
            //            if (hasChange)
            //            {
            //                ProcessMove(hasPositionChange, hasRotationChange);
            //            }
            //            _prevHandlePos = currHandlePos;
            //            _prevHandleRot = currHandleRot;
            //            _prevElbowBendFactorM11 = _elbowBendFactorM11;
            //            _prevCustomBendDir = customBendDir;
        }
        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange)
        {
            var handlePos = _handle.position;
            var handleRot = _handle.rotation;
            var shoulderPos = Shoulder.position;
            var collarRot = Collar.rotation;
            var collarDn = collarRot * v3.dn;
            var shoulderToHandDir = (handlePos - shoulderPos).ToUnit(out var dist);
            var collarSide = _isRight ? collarRot * v3.rt : collarRot * v3.lt;
            
            // check if we need to lift collar bone starts here
            var collarUp = _iniCollarUp.AsWorldDir(Collar.parent);
            var collarPos = _iniCollarPoint.AsWorldPoint(Collar.parent);
            var handlProjOnCollarUp = _handle.position.ProjectOnLine(in collarPos, collarPos + collarUp);
            if (handlProjOnCollarUp.IsPointAbovePlain(in collarUp, in collarPos))
            {
                var lift01 = (handlProjOnCollarUp.DistanceToPlane(in collarUp, in collarPos) / 0.6f).Clamp01();
                Collar.localPosition = _iniCollarPoint + _iniCollarUp * (lift01 * 0.08f);
            }
            // check if we need to lift collar bone ends here

            var downAxis = collarRot * (_isRight ? RelDownAxisR : RelDownAxisL);

            if (_isRight) vector.GetNormal(in shoulderToHandDir, in downAxis, out ComputedBendDir);
            else vector.GetNormal(in downAxis, in shoulderToHandDir, out ComputedBendDir);

            var elbowBendFactor01 = _elbowBendFactorM11.Abs();
            if (elbowBendFactor01 > 0.001)
            {
                var sn = _isRight ? 1 : -1;
                ComputedBendDir = ComputedBendDir.RotateAbout(in shoulderToHandDir, 30 * _elbowBendFactorM11 * sn);
            }
//dbg.DrawAxis(this.hc() * 1000, handlePos, ComputedBendDir, Color.yellow);
            LastBenDir = CustomBendDir ?? ComputedBendDir;
//dbg.DrawAxis(this.hc(), handlePos, LastBenDir, Color.green);

            //dbg.DrawAxis(this.hc(), shoulderPos, downAxis, dbg.GetColor(this));
            //dbg.DrawAxis(this.hc()*100, shoulderPos, bendDir, Color.magenta);

            //     [Forearm] (elbow)
            //         ^
            //        /|\  
            //     a / |h\ b
            //      /__|__\ angle "beta"
            // [Hand]  c  [Shoulder]

            var a = _distHandToElbow;
            var b = _distElbowToShoulder;
            var c = dist;
            var h = triangle.GetHeight(a, b, c);
            var beta = Math.Asin(h / b) * RTD;
            var upperArmFw = shoulderToHandDir.RotateTowardsCanOvershoot(in LastBenDir, beta);
            var elbowPos = shoulderPos + upperArmFw * _distElbowToShoulder;
            var lowerArmFw = (handlePos - elbowPos).normalized;
            var handUp = handleRot * Vector3.up;
//            var handFw = handleRot * Vector3.forward;
            var shoulderUp = upperArmFw.GetRealUp(in collarSide, in collarDn);

            Vector3 elbowUp;
            if (_isRight) vector.GetNormal(in lowerArmFw, in LastBenDir, out elbowUp);
            else  vector.GetNormal(in LastBenDir, in lowerArmFw, out elbowUp);

            var shoulderTwistUp = slerp(in shoulderUp, in elbowUp, 0.5)
                .ProjectOnPlaneAndNormalize(in upperArmFw);
            var forearmTwistUp = slerp(in handUp, in elbowUp, 0.5)
                .ProjectOnPlaneAndNormalize(in lowerArmFw);
            /*if (_hasComplexShoulder)
            {
                for (var i = 0; i < _iniShoulderPoints.Length; ++i)
                    _tempShoulderPoints[i] = _iniShoulderPoints[i].AsWorldPoint(Collar);

//                dbg.DrawMultiColorChain(this.hc() * 100, _tempShoulderPoints);

                inverseKinematics.FABRIK(elbowPos, _tempShoulderPoints, _shoulderPointLengths, 2, 1);

//                dbg.DrawPointersChain(this.hc() * 200, _tempShoulderPoints);

                var p1 = _tempShoulderPoints[1];
                var p2 = _tempShoulderPoints[2];
                Shoulder.rotation = lookAt(Shoulder.position.DirTo(p1), shoulderUp);
                Shoulder2.rotation = lookAt(p1.DirTo(p2), shoulderUp);

            }
            else
            {*/
                Shoulder.rotation = Quaternion.LookRotation(upperArmFw, shoulderUp);
            //}
            ShoulderTwist.rotation = Quaternion.LookRotation(upperArmFw, shoulderTwistUp);
            Forearm.rotation = Quaternion.LookRotation(lowerArmFw, elbowUp);
            ForearmTwist.rotation = Quaternion.LookRotation(lowerArmFw, forearmTwistUp);
            if (AutoPositionHand)
            {
                Hand.rotation = _handle.rotation;
            }
        }

        public Gen8ArmChain RotateHandsAwayFromShoulder(double degreesPerSec = 90)
        {
            _handle.RotateTowards(Quaternion.LookRotation((_handle.position - Shoulder.position).normalized, Collar.forward), degreesPerSec * fun.smoothDeltaTime);
            return this;
        }
        public Gen8ArmChain RotateHandsAwayFromShoulder(in Vector3 upDirInWorld, double degreesPerSec = 90)
        {
            _handle.RotateTowards(Quaternion.LookRotation((_handle.position - Shoulder.position).normalized, upDirInWorld), degreesPerSec * fun.smoothDeltaTime);
            return this;
        }
        public Gen8ArmChain EnsurePosition(in Vector3 candidate)
        {

            var sideSign = _isRight ? -1 : 1;
            var pos = candidate;

            var planeNor1 = Collar.up.Negate().RotDir(Collar.right, -70);
            var planePos1 = Shoulder.position + Collar.right * (sideSign * _distElbowToShoulder * 0.8f);

            // if target crosses body plane1: project it on plane1
            if (point.IsBelowPlane(in pos, in planeNor1, in planePos1))
            {
                point.ProjectOnPlane(in pos, in planeNor1, in planePos1, out pos);
            }
            // if target crosses body plane2: project it on plane2
            var planeNor2 = Collar.forward.RotDir(Collar.right, sideSign * 60).RotDir(Collar.up, 30);
            var planePos2 = Shoulder.position;
            if (point.IsBelowPlane(in pos, in planeNor2, in planePos2))
            {
                point.ProjectOnPlane(in pos, in planeNor2, in planePos2, out pos);
            }
            Handle.position = pos;
            //Handle.MoveTowards(candidate, _distElbowToShoulder+_distHandToElbow);
            return this;
        }
        public Gen8ArmChain SetOptimalRotation(double degreesPerSec)
        {
            var fw = Forearm.position.DirTo(Handle.position);
            var up = point.GetNormalSameSideAs(Handle.position, Forearm.position, Shoulder.position, Collar.up);
            var targRot = lookAt(in fw, in up);
            Handle.RotateTowards(targRot, fun.smoothDeltaTime * degreesPerSec);
            return this;
        }
    }
}