using System;
using Unianio.Extensions;
using Unianio.Human;
using Unianio.Moves;
using Unianio.Rigged;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public class HumSpineChain : BaseIkChain, IInitialOrientationHolder
    {
        readonly IComplexHumanDefinition _definition;
        public readonly Transform Root, AbdomenLower, AbdomenUpper, ChestLower, ChestUpper, NeckLower;
        readonly Vector3[] _tempPositions, _tempFw, _handleBk, _rootUp, 
            _initialPositionsInRootSpace;
        readonly Vector3 _locRootUp, _locRootFw;
        readonly float[] _joinLengths;
        readonly Transform[] _allNodes;
        readonly float _defaultDistRootToNeck;
        Vector3 _prevHanPosInRootSpace;
        Quaternion _prevHanRotInRootSpace;
        int _bendChange, _lastBendChange, _framesAfterChange;
        public double DegreesPerSec { get; set; }
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
            return new Mover<HumSpineChain>(this).New().Local.LineTo(b => b.IniLocalPos);
        }
        public IExecutorOfProgress ToInitialLocalRotation()
        {
            return new Mover<HumSpineChain>(this).New().Local.RotateTo(b => b.IniLocalRot);
        }
        public IExecutorOfProgress ToInitialLocalScale()
        {
            return new Mover<HumSpineChain>(this).New().Local.ScaleTo(b => b.IniLocalSca);
        }
        public IExecutorOfProgress ToInitialLocal()
        {
            return new Mover<HumSpineChain>(this).New().Local
                    .LineTo(b => b.IniLocalPos)
                    .RotateTo(b => b.IniLocalRot)
                    .ScaleTo(b => b.IniLocalSca)
                ;
        }

        public readonly HumBoneHandler NeckLowerHandler;

        public Vector3 position
        {
            get { return Handle.position; }
            set { Handle.position = value; }
        }
        public Vector3 localPosition
        {
            get { return Handle.localPosition; }
            set { Handle.localPosition = value; }
        }
        public Quaternion rotation
        {
            get { return Handle.rotation; }
            set { Handle.rotation = value; }
        }
        public Quaternion localRotation
        {
            get { return Handle.localRotation; }
            set { Handle.localRotation = value; }
        }
        public float DefaultDistRootToNeck => _defaultDistRootToNeck;
        public Transform[] AllNodes => _allNodes;
        public HumSpineChain(IComplexHumanDefinition definition) : base(BodyPart.Spine)
        {
            _definition = definition;
            var spine = _definition.Spine;
            _model = _definition.Model;
            var modelFw = _model.forward;
            var modelUp = _model.up;

            Root = spine.AbdomenLower.parent;

            AbdomenLower = spine.AbdomenLower;
            AbdomenUpper = spine.AbdomenUpper;
            ChestLower = spine.ChestLower;
            ChestUpper = spine.ChestUpper;
            NeckLower = spine.NeckLower;

            _locRootUp = Root.DirTo(AbdomenLower).GetRealUp(-_model.forward).AsLocalDir(Root);
            _locRootFw = Root.DirTo(AbdomenLower).AsLocalDir(Root);
            _handle = CreateHandle(Root, BodyPart.Spine, false, NeckLower.position, modelFw, modelUp);
            NeckLowerHandler = new HumBoneHandler(BodyPart.Neck, _model, NeckLower);

            _handle.position = NeckLower.position;
            _handle.rotation = NeckLower.rotation;

            _prevHanPosInRootSpace = _handle.localPosition;
            _prevHanRotInRootSpace = _handle.localRotation;
            IniLocalPos = _handle.localPosition;
            IniModelPos = _handle.position.AsLocalPoint(_model);
            IniLocalRot = _handle.localRotation;
            IniModelRot = lookAt(_handle.forward.AsLocalDir(_model), _handle.up.AsLocalDir(_model));

            _allNodes = new[] {AbdomenLower,AbdomenUpper,ChestLower };
            _initialPositionsInRootSpace = new Vector3[]
            {
                AbdomenLower.position.AsLocalPoint(Root),
                AbdomenUpper.position.AsLocalPoint(Root),
                ChestLower.position.AsLocalPoint(Root),
                NeckLower.position.AsLocalPoint(Root)
            };
            _tempPositions = new Vector3[_initialPositionsInRootSpace.Length];//hip,AbdomenLower,AbdomenUpper,ChestLower,ChestUpper,NeckLower(handle)
            _tempFw = new Vector3[_initialPositionsInRootSpace.Length];
            _handleBk = new Vector3[_initialPositionsInRootSpace.Length];
            _rootUp = new Vector3[_initialPositionsInRootSpace.Length];
            _joinLengths = new []
            {
                vDist(AbdomenLower, AbdomenUpper),
                vDist(AbdomenUpper, ChestLower),
                vDist(ChestLower, NeckLower)
            };
            _defaultDistRootToNeck = vDist(Root.position, NeckLower.position);
        }
        public override void Update()
        {


            var handlePos = _handle.position;
            var rootPos = Root.position;
            var distToRoot = distance.Between(in handlePos, in rootPos);
            if(distToRoot < _defaultDistRootToNeck)
            {
                handlePos = rootPos.MoveTowardsCanOvershoot(in handlePos, _defaultDistRootToNeck);
            }

            var hanPosInRootSpace = _handle.localPosition;
            var hanRotInRootSpace = _handle.localRotation;

            var isSamePos = hanPosInRootSpace.IsEqual(in _prevHanPosInRootSpace, 0.00001);
            var isSameRot = _prevHanRotInRootSpace.IsEqual(in hanRotInRootSpace, 0.00001);


            var isSameBend = _lastBendChange == _bendChange;
            var hasChange = !isSamePos || !isSameRot || !isSameBend;
            if (hasChange) _framesAfterChange = 0;
            else ++_framesAfterChange;
//dbg.log(isSamePos, isSameRot, isSameBend);
            if (hasChange || _framesAfterChange < 90)
            {
                AssignDefaultValues(_tempPositions);

                const int NumFabrikIterations = 3;
                inverseKinematics.FABRIK(in handlePos, _tempPositions, _joinLengths, 4, NumFabrikIterations);

                var handleFwInWorld = _handle.forward;
                RotateBonesToTargets(_tempPositions, ref handleFwInWorld);
            }
            _lastBendChange = _bendChange;
            _prevHanPosInRootSpace = hanPosInRootSpace;
            _prevHanRotInRootSpace = hanRotInRootSpace;
        }


        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange) { throw new NotImplementedException(); }
        void AssignDefaultValues(Vector3[] points)
        {
            points[0] = AbdomenLower.position;
            for (var i = 1; i < points.Length; ++i)
            {
                points[i] = _initialPositionsInRootSpace[i].AsWorldPoint(Root);
            }
        }
        void RotateBonesToTargets(Vector3[] points, ref Vector3 handleFwInWorld)
        {
            var rootUp = _locRootUp.AsWorldDir(Root);
            //_rootUp[0] = rootUp;
            var orientFw = _locRootFw.AsWorldDir(Root);
            // compute forward directions
            for (var i = 1; i < points.Length; ++i)
            {
                _tempFw[i - 1] = (points[i] - points[i - 1]).normalized;//.ToUnit(ref ifZero);


                rootUp = _tempFw[i - 1].GetRealUp(in rootUp, in orientFw);
                _rootUp[i - 1] = rootUp;

                if (i == 1)
                {
                    orientFw = Root.position.DirTo(_handle.position);
                }

//    dbg.DrawLine(10*i + this.hc(), points[i], points[i - 1], dbg.GetColor(i));
//    dbg.DrawAxis(100*i + this.hc(), points[i - 1], _rootUp[i - 1], colorLightGreen);
            }
            _tempFw[points.Length - 1] = handleFwInWorld;
            // compute up directions
            var currUp = -handleFwInWorld;
            _handleBk[_handleBk.Length - 1] = currUp;
            for (var i = _handleBk.Length - 2; i >= 0; --i)
            {
                var currFw = _tempFw[i];
                currUp = currFw.GetRealUp(currUp);
                _handleBk[i] = currUp;
            }
//            var degPerFrame = DegreesPerSec * fun.smoothDeltaTime;
            // apply rotations
            for (var i = 0; i < _allNodes.Length; ++i)
            {
                var x = (i / (float)(_allNodes.Length - 1));
                var y = pow(sin(x * PI * 0.5), 2);

                var fw = _tempFw[i];
                var up = slerp(_rootUp[i], _handleBk[i], y);
                //var up = _rootUp[i];

                //dbg.DrawOrient(i+this.hc(), _allNodes[i].position, Quaternion.LookRotation(fw, up));
                _allNodes[i].rotation = Quaternion.LookRotation(fw, fw.GetRealUp(up));
            }
        }
    }
}