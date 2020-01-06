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
    public class MHArmChain : BaseIkChain, IHumArmChain
    {
        readonly float _distHandToElbow, _distElbowToShoulder, _iniDegToRelPoint;
        readonly Quaternion _iniCollarLocRot, _maxCollarLocRot;
        float _shake01;
        IShakeableArm _shakeable;

        public HumBoneHandler HandHandler { get; }
        public bool AutoPositionHand { get; set; }
        public Transform ArmRoot { get; }
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
        public BodySide Side { get; }
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
            return new Mover<MHArmChain>(this).New().Local.LineTo(b => b.IniLocalPos);
        }
        public IExecutorOfProgress ToInitialLocalRotation()
        {
            return new Mover<MHArmChain>(this).New().Local.RotateTo(b => b.IniLocalRot);
        }
        public IExecutorOfProgress ToInitialLocalScale()
        {
            return new Mover<MHArmChain>(this).New().Local.ScaleTo(b => b.IniLocalSca);
        }
        public IExecutorOfProgress ToInitialLocal()
        {
            return new Mover<MHArmChain>(this).New().Local
                    .LineTo(b => b.IniLocalPos)
                    .RotateTo(b => b.IniLocalRot)
                    .ScaleTo(b => b.IniLocalSca)
                ;
        }
        public Vector3 SideDir { get; }

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
        public bool Shakeable
        {
            get => _shakeable != null;
            set
            {
                if (!value && _shakeable != null)
                {
                    _shakeable = null;
                }
                else if (value && _shakeable == null)
                {
                    _shakeable = get<IShakeableArm>().Set(this);
                }
            }
        }
        public float Shake01
        {
            get => _shake01;
            set => _shake01 = value.Clamp01();
        }
        public float MaxStretch { get; }
        public MHArmChain(BodyPart part, IComplexHumanDefinition definition) : base(part)
        {
            Side = part == BodyPart.ArmR ? BodySide.RT : BodySide.LT;
            SideDir = Side.IsRight() ? v3.rt : v3.lt;
            _model = definition.Model;
            var arm = Side.IsRight() ? definition.ArmR : definition.ArmL;

            ArmRoot = arm.ArmRoot;
            Collar = arm.Collar;
            Shoulder = arm.Shoulder;
            Shoulder2 = arm.Shoulder2;
            ShoulderTwist = arm.ShoulderTwist;
            Forearm = arm.Forearm;
            ForearmTwist = arm.ForearmTwist;
            AutoPositionHand = true;

            Hand = arm.Hand;
            _handle = CreateHandle(ArmRoot, Part, false, Hand.position, Hand.forward, Hand.up);

            HandHandler = new HumBoneHandler(Side.IsRight() ? BodyPart.HandR : BodyPart.HandL, _model, Hand);

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

            _distHandToElbow = distance.Between(Hand.position, Forearm.position);
            _distElbowToShoulder = distance.Between(Forearm.position, Shoulder.position);
            MaxStretch = (_distHandToElbow + _distElbowToShoulder);
            _iniDegToRelPoint = CalculateDegreesToRelPoint(Handle.position, Handle.rotation * SideDir);
            _iniCollarLocRot = Collar.localRotation;
            _maxCollarLocRot = lookAt(_iniCollarLocRot * v3.fw.RotUp(30), _iniCollarLocRot * v3.up);

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
        }
        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange)
        {
            // 1. find hypothetical elbow
            var handlePos = _handle.position;
            var handleRot = _handle.rotation;
            var handleSide = handleRot * SideDir;

            // 2.find angle to hypothetical elbow (rel point)
            var degToRelPoint = CalculateDegreesToRelPoint(in handlePos, in handleSide);
            
            // 3. if angle is more than initial lift collar
            var liftCollar01 = degToRelPoint.FromRangeTo01(_iniDegToRelPoint, 120).Clamp01();
            Collar.localRotation = 
                liftCollar01 <= 0 
                    ? _iniCollarLocRot 
                    : slerp(in _iniCollarLocRot, in _maxCollarLocRot, smoothstep(liftCollar01));

            // 4. calculate elbow position
            var shoulderPos = Shoulder.position;
            var shoulderToHandDir = shoulderPos.DirTo(in handlePos);
            CalculateArmBend(out var midPos, out var lengthToElbow);
            var elbowDir = vector.ProjectOnPlane(in handleSide, in shoulderToHandDir);
            var elbow = midPos + elbowDir * lengthToElbow;

            // 5. position bones
           
            var upperArmFw = (elbow - shoulderPos).normalized;
            var shoulderUp = upperArmFw.GetRealUp(Collar.rotation * v3.fw, Collar.rotation * v3.dn);//.RotBk(liftCollar01 * 20);
            var lowerArmFw = (handlePos - elbow).normalized;
            var forearmTwistUp = handleRot * v3.up;
            var elbowUp = forearmTwistUp;
            var shoulderTwistUp = 
                vector.ProjectOnPlaneAndGetMiddle(in shoulderUp, in forearmTwistUp, in upperArmFw);
            Shoulder.rotation = Quaternion.LookRotation(upperArmFw, shoulderUp);
            ShoulderTwist.rotation = Quaternion.LookRotation(upperArmFw, shoulderTwistUp);
            Forearm.rotation = Quaternion.LookRotation(lowerArmFw, elbowUp);
            ForearmTwist.rotation = Quaternion.LookRotation(lowerArmFw, forearmTwistUp);
            if (AutoPositionHand)
            {
                Hand.rotation = 
                    _shakeable != null 
                        ? slerp(_handle.rotation, _shakeable.Shakeable.rotation, _shake01) 
                        : _handle.rotation;
            }
        }
        //     [Forearm] (elbow)
            //         ^
            //        /|\  
            //     a / |h\ b
            //      /__|__\ angle "beta"
            //       c1 c2
            // [Hand]  c  [Shoulder]
        public void CalculateArmBend(out Vector3 midPoint, out float lengthToElbow)
        {
            var handlePos = _handle.position;
            var shoulderPos = Shoulder.position;
            var a = _distHandToElbow;
            var b = _distElbowToShoulder;
            var c = shoulderPos.DistanceTo(in handlePos);
            var h = triangle.GetHeight(a, b, c);
            var c1 = sqrt(a * a - h * h);
            midPoint = handlePos.MoveTowards(in shoulderPos, c1);
            lengthToElbow = h;
        }
        float CalculateDegreesToRelPoint(in Vector3 handlePos, in Vector3 handleSide)
        {
            var refPoint = handlePos + handleSide * _distHandToElbow;
            var toRelPoint = (refPoint - ArmRoot.position).normalized;
            var rootDn = ArmRoot.rotation * v3.dn;
            return angle.BetweenVectorsUnSignedInDegrees(in toRelPoint, in rootDn);
        }
    }
}