using System;
using System.Collections.Generic;
using System.Linq;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.Genesis.IK;
using Unianio.Genesis.State;
using Unianio.Graphs;
using Unianio.Human;
using Unianio.MakeHuman;
using Unianio.PhysicsAgents;
using Unianio.Rigged;
using Unianio.Rigged.IK;
using Unianio.Services;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public interface IComplexHuman : 
        IAnimatedHumanoid, 
        IPositionable, 
        IRotatable,
        ILateUpdatable, 
        IIdHolder, 
        IBaseManipulator,
        IRotatableAroundPivot,
        ITimeProviderHolder,
        IDisposable
    {
        string Persona { get; }
        Transform Model { get; }
        HumArmChain ArmL { get; }
        HumArmChain ArmR { get; }
        HumLegChain LegL { get; }
        HumLegChain LegR { get; }
        HumSpineChain Spine { get; }
        HumBoneHandler Tongue { get; }
        GenFaceGroup GenFace { get; }
        HumBoneHandler NeckUpper { get; }
        HumBoneHandler Head { get; }
        HumBoneHandler Pelvis { get; }
        HumBreastGroup BreastL { get; }
        HumBreastGroup BreastR { get; }
        HumBoneHandler CollarL { get; }
        HumBoneHandler CollarR { get; }
        Vector3 forward { get; }
        Vector3 back { get; }
        Vector3 right { get; }
        Vector3 left { get; }
        Vector3 up { get; }
        Vector3 down { get; }
        Vector3 localScale { get; set; }
        HumanInitialStats Initial { get; }
        HumBoneHandler Hip { get; }
        Vector3 fw { get; }
        Vector3 bk { get; }
        Vector3 rt { get; }
        Vector3 lt { get; }
        Vector3 dn { get; }
        Transform SourceNeckUpper { get; }
        Transform SourceHead { get; }
        HandlePath PathHandL { get; }
        HandlePath PathHandR { get; }
        HandlePath PathArmL { get; }
        HandlePath PathArmR { get; }
        NumericPath PathElbowArmL { get; }
        NumericPath PathElbowArmR { get; }
        DirectionPath PathBendDirArmL { get; }
        DirectionPath PathBendDirArmR { get; }
        HandlePath PathLegL { get; }
        HandlePath PathLegR { get; }
        HandlePath PathFootL { get; }
        HandlePath PathFootR { get; }
        HandlePath PathToesL { get; }
        HandlePath PathToesR { get; }
        HandlePath PathPelvis { get; }
        HandlePath PathHip { get; }
        HandlePath PathHead { get; }
        HandlePath PathSpine { get; }
        HandlePath PathJaw { get; }
        HandlePath PathNeckLower { get; }
        HandlePath PathNeckUpper { get; }
        HandlePath PathTongue { get; }
        HandlePath PathCollarL { get; }
        HandlePath PathCollarR { get; }
        FacePath PathFace { get; }
        IBlinkController Blinks { get; }
        void Disable();
        void Enable();
        float GetTimeByKey(string key);
        IComplexHuman SetTimeByKey(string key, float time);
        Transform GetBoneByName(string name);
        IComplexHuman Set(IComplexHumanDefinition definition, params IHumanExtender[] extenders);
        void LookAt(Vector3 target, Vector3 up);
        Vector3 RelRtUpFw(double up01, double fw01, double rt01);
        IComplexHumanDefinition Definition { get; }
        HumBoneHandler NippleL { get; }
        HumBoneHandler NippleR { get; }
        ulong Flags { get; set; }
        bool IsDisabled { get; }
        MHFaceGroup MHFace { get; }
        object GetObjectFromExtender(string extenderName, string objectName);
    }
    public class ComplexHuman : BaseAnimatedHumanoid, IComplexHuman
    {
        readonly IComplexHuman _this;
        readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        readonly List<IDisposable> _disposables = new List<IDisposable>();
        readonly List<INamedObjectSource> _objectSources = new List<INamedObjectSource>();
        readonly IDictionary<string, float> _timeByKey = new Dictionary<string, float>(StringComparer.InvariantCultureIgnoreCase);
        ulong _id;
        HumanInitialStats _initialStats;
        IComplexHumanDefinition _definition;
        HumArmChain _armL, _armR;
        HumLegChain _legL, _legR;
        HumSpineChain _spine;
//        GenTongueChain _tongue;
        GenFaceGroup _genFace;
        MHFaceGroup _mhFace;
        //IMHFacialExpressions _mhFaceExp;
        Transform _sourceNeckUpper, _sourceHead, _root;
        HumBoneHandler _hip, _neckUpper, _head, _pelvis, _tongue,
            _braFront,_braBack,_pantiesFront,_pantiesBack, _nippleL, _nippleR, _collarL, _collarR;
        HumBreastGroup _breastL, _breastR;
        HandlePath _pathHandL, _pathHandR, _pathArmL, _pathArmR, _pathLegL, _pathLegR, _pathFootL, 
            _pathFootR, _pathToesL, _pathToesR, _pathPelvis, _pathHip, _pathHead, _pathSpine, _pathJaw,
            _pathNeckLower, _pathNeckUpper, _pathTongue, _pathCollarL, _pathCollarR;
        FacePath _pathFace;
        NumericPath _pathElbowArmL, _pathElbowArmR;
        DirectionPath _pathBendDirArmL, _pathBendDirArmR;
        bool _isDisposed;
        IPausableTimeProvider _pausableTime;

        internal ComplexHuman()
        {
            _this = this;
            _this.Flags = 0;
        }
        ulong IIdHolder.ID => _id;
        ulong IComplexHuman.Flags { get; set; }
        Transform IBaseManipulator.Manipulator => _definition.Model;
        IComplexHuman IComplexHuman.Set(IComplexHumanDefinition definition, params IHumanExtender[] extenders)
        {
            _definition = definition;
            var m = _definition.Model;
            _id = (ulong)m.GetHashCode();
//            foreach (var e in _definition.BonesByName.Values) e.gameObject.layer = layers.BitNumber_Characters;

            _sourceNeckUpper = _definition.NeckUpper.Bone;
            _sourceHead = _definition.Head.Bone;
            _root = _definition.Root.Bone;
            _hip = new HumBoneHandler(m, _definition.Hip.Bone);
            _pelvis = new HumBoneHandler(m, _definition.Pelvis.Bone);
            _spine = new HumSpineChain(_definition);
            _neckUpper = new HumBoneHandler(m, _definition.NeckUpper.Bone);
            _head = new HumBoneHandler(m, _definition.Head.Bone);
            _tongue = new HumBoneHandler(m, _definition.Tongue.Tongue2);
            //            _tongue = new GenTongueChain(_definition);
            _armL = new HumArmChain(HumanoidPart.ArmL, _definition);
            _armR = new HumArmChain(HumanoidPart.ArmR, _definition);

            _legL = new HumLegChain(HumanoidPart.LegL, _definition);
            _legR = new HumLegChain(HumanoidPart.LegR, _definition);
            var humFaceConfigSource = extenders.FirstOrDefault(e => e is HumanFaceConfigSource) as HumanFaceConfigSource; 
            if (_definition.HumanoidType == HumanoidType.Genesis8)
                _genFace = new GenFaceGroup(this, humFaceConfigSource);
            else if(_definition.HumanoidType == HumanoidType.MakeHuman)
                _mhFace = new MHFaceGroup(this, humFaceConfigSource);
            _collarL = new HumBoneHandler(HumanoidPart.CollarL, m, _definition.ArmL.Collar);
            _collarR = new HumBoneHandler(HumanoidPart.CollarR, m, _definition.ArmR.Collar);

            if (_definition.HasCompletedImport)
            {
                _updatables.Add(_spine);
                _updatables.Add(_legL);
                _updatables.Add(_legR);
                _updatables.Add(_armL);
                _updatables.Add(_armR);
            }
            
            var modelRt = m.right;
            var modelLt = -modelRt;

            if (_definition.HasCompletedImport)
            {
                if(_definition.HasBreasts)
                {

                    var breastConfigSource = extenders.FirstOrDefault(e => e is BreastConfigSource) as BreastConfigSource;
                    _breastL = new HumBreastGroup(definition.Persona, BodySide.LT, this, _definition.BreastL.Bone, _nippleL, breastConfigSource).AddTo(_updatables);
                    _breastR = new HumBreastGroup(definition.Persona, BodySide.RT, this, _definition.BreastR.Bone, _nippleR, breastConfigSource).AddTo(_updatables);
                }
                
            }


            foreach (var ext in (extenders ?? new IHumanExtender[0]))
            {
                ext.Setup(this);

                if (ext is IUpdatable upd) _updatables.Add(upd);
                if (ext is IDisposable dis) _disposables.Add(dis);
                if (ext is INamedObjectSource nts) _objectSources.Add(nts);
            }

            _initialStats = new HumanInitialStats(this);

            return this;
        }

        bool IComplexHuman.IsDisabled => !_definition.Model || !_definition.Model.gameObject.activeSelf;
        void IComplexHuman.Disable()
        {
            if(_this.IsDisabled) return;
            
            _definition.Model.gameObject.SetActive(false);

            fire(new ComplexHumanDisabled(this));
        }
        void IComplexHuman.Enable()
        {
            if(!_this.IsDisabled) return;

            if(!_definition.Model) return;

            _definition.Model.gameObject.SetActive(true);

            fire(new ComplexHumanEnabled(this));
        }
        object IComplexHuman.GetObjectFromExtender(string extenderName, string objectName)
        {
            for (var i = 0; i < _objectSources.Count; ++i)
            {
                var e = _objectSources[i];
                if (e.IsNamed(extenderName))
                {
                    return e.GetObject(objectName);
                }
            }
            return null;
        }
        void ITimeProviderHolder.SetTimeProvider(IPausableTimeProvider pausableTime)
        {
            if(_definition == null) throw new ArgumentException("IGenHuman.SetTimeProvider must be called after Set method");
            _pausableTime = pausableTime;
            _breastL?.SetTimeProvider(_pausableTime);
            _breastR?.SetTimeProvider(_pausableTime);
            if(_updatables != null)
                foreach (var u in _updatables)
                {
                    if (u is ITimeProviderHolder tph)
                    {
                        tph.SetTimeProvider(_pausableTime);
                    }
                }

        }
        void ILateUpdatable.LateUpdate()
        {
            if(_this.IsDisabled) return;

            for (var i = 0; i < _updatables.Count; ++i)
            {
                _updatables[i].Update();
            }
//dbg.DrawAxis(1, _this.position, _this.forward, _this.IsForwardFacing() ? Color.blue : Color.red);
        }
        Vector3 IComplexHuman.RelRtUpFw(double rt01, double up01, double fw01)
        {
            return   v3.rt * (float)(rt01 * _definition.Height) 
                   + v3.up * (float)(up01 * _definition.Height) 
                   + v3.fw * (float)(fw01 * _definition.Height);
        }

        void IDisposable.Dispose()
        {
            if(_isDisposed) return;
            UnityEngine.Object.Destroy(_definition.Model.gameObject);
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
            _isDisposed = true;
        }
        public override IAnimation AniEntireBody
        {
            get => base.AniEntireBody;
            set
            {
                var prev = base.AniEntireBody;
                base.AniEntireBody = value;
                if (prev != value) fire(new RootAnimationChanged(this, prev, value));
            }
        }
        string IComplexHuman.Persona => _definition.Persona;
        IComplexHumanDefinition IComplexHuman.Definition => _definition;

        IBlinkController IComplexHuman.Blinks => _this.IsGenesis8() ? _this.GenFace : (IBlinkController)_this.MHFace;
        HandlePath IComplexHuman.PathCollarL => _pathCollarL ?? (_pathCollarL = new HandlePath(_collarL));
        HandlePath IComplexHuman.PathCollarR => _pathCollarR ?? (_pathCollarR = new HandlePath(_collarR));
        FacePath IComplexHuman.PathFace => _pathFace ?? (_pathFace = new FacePath(this));
        HandlePath IComplexHuman.PathHandL => _pathHandL ?? (_pathHandL = new HandlePath(_armL.HandHandler));
        HandlePath IComplexHuman.PathHandR => _pathHandR ?? (_pathHandR = new HandlePath(_armR.HandHandler));
        HandlePath IComplexHuman.PathArmL => _pathArmL ?? (_pathArmL = new HandlePath(_armL));
        HandlePath IComplexHuman.PathArmR => _pathArmR ?? (_pathArmR = new HandlePath(_armR));
        NumericPath IComplexHuman.PathElbowArmL => _pathElbowArmL ?? (_pathElbowArmL = new NumericPath(n => _armL.ElbowBendFactor = n, () => _armL.ElbowBendFactor));
        NumericPath IComplexHuman.PathElbowArmR => _pathElbowArmR ?? (_pathElbowArmR = new NumericPath(n => _armR.ElbowBendFactor = n, () => _armR.ElbowBendFactor));
        DirectionPath IComplexHuman.PathBendDirArmL => _pathBendDirArmL ?? (_pathBendDirArmL = new DirectionPath(v => _armL.CustomBendDir = v, () => _armL.LastBenDir, _definition.Model, _armL.Root));
        DirectionPath IComplexHuman.PathBendDirArmR => _pathBendDirArmR ?? (_pathBendDirArmR = new DirectionPath(v => _armR.CustomBendDir = v, () => _armR.LastBenDir, _definition.Model, _armR.Root));
        HandlePath IComplexHuman.PathLegL => _pathLegL ?? (_pathLegL = new HandlePath(_legL));
        HandlePath IComplexHuman.PathLegR => _pathLegR ?? (_pathLegR = new HandlePath(_legR));
        HandlePath IComplexHuman.PathFootL => _pathFootL ?? (_pathFootL = new HandlePath(_legL.FootHandler));
        HandlePath IComplexHuman.PathFootR => _pathFootR ?? (_pathFootR = new HandlePath(_legR.FootHandler));
        HandlePath IComplexHuman.PathToesL => _pathToesL ?? (_pathToesL = new HandlePath(_legL.ToeHandler));
        HandlePath IComplexHuman.PathToesR => _pathToesR ?? (_pathToesR = new HandlePath(_legR.ToeHandler));
        HandlePath IComplexHuman.PathPelvis => _pathPelvis ?? (_pathPelvis = new HandlePath(_pelvis));
        HandlePath IComplexHuman.PathHip => _pathHip ?? (_pathHip = new HandlePath(_hip));
        HandlePath IComplexHuman.PathHead => _pathHead ?? (_pathHead = new HandlePath(_head));
        HandlePath IComplexHuman.PathTongue => _pathTongue ?? (_pathTongue = new HandlePath(_tongue));
        HandlePath IComplexHuman.PathNeckLower => _pathNeckLower ?? (_pathNeckLower = new HandlePath(_spine.NeckLowerHandler));
        HandlePath IComplexHuman.PathNeckUpper => _pathNeckUpper ?? (_pathNeckUpper = new HandlePath(_neckUpper));
        Transform IComplexHuman.GetBoneByName(string name) => _definition.BonesByName.GetOrDefault(name);
        HandlePath IComplexHuman.PathSpine => _pathSpine ?? (_pathSpine = new HandlePath(_spine));
        HandlePath IComplexHuman.PathJaw => _pathJaw ?? (_pathJaw = new HandlePath(_this.IsGenesis8() ? _genFace.LowerJaw : _mhFace.Jaw));
        HumanInitialStats IComplexHuman.Initial => _initialStats;
        Transform IComplexHuman.Model => _definition.Model;
        HumArmChain IComplexHuman.ArmL => _armL;
        HumArmChain IComplexHuman.ArmR => _armR;
        HumLegChain IComplexHuman.LegL => _legL;
        HumLegChain IComplexHuman.LegR => _legR;
        HumSpineChain IComplexHuman.Spine => _spine;
//        GenTongueChain IGenHuman.Tongue => _tongue;
        HumBoneHandler IComplexHuman.NeckUpper => _neckUpper;
        HumBoneHandler IComplexHuman.Head => _head;
        HumBoneHandler IComplexHuman.Tongue => _tongue;
        HumBoneHandler IComplexHuman.Hip => _hip;
        HumBoneHandler IComplexHuman.Pelvis => _pelvis;
        HumBreastGroup IComplexHuman.BreastL => _breastL;
        HumBreastGroup IComplexHuman.BreastR => _breastR;
        HumBoneHandler IComplexHuman.NippleL => _nippleL;
        HumBoneHandler IComplexHuman.NippleR => _nippleR;
        HumBoneHandler IComplexHuman.CollarL => _collarL;
        HumBoneHandler IComplexHuman.CollarR => _collarR;
        GenFaceGroup IComplexHuman.GenFace => _genFace;
        MHFaceGroup IComplexHuman.MHFace => _mhFace;
        Transform IComplexHuman.SourceNeckUpper => _sourceNeckUpper;
        Transform IComplexHuman.SourceHead => _sourceHead;
        Vector3 IComplexHuman.forward => _definition.Model.forward;
        Vector3 IComplexHuman.back => -(_definition.Model.forward);
        Vector3 IComplexHuman.right => _definition.Model.right;
        Vector3 IComplexHuman.left => -(_definition.Model.right);
        Vector3 IComplexHuman.up => _definition.Model.up;
        Vector3 IComplexHuman.down => -(_definition.Model.up);
        Vector3 IComplexHuman.fw => _definition.Model.forward;
        Vector3 IComplexHuman.bk => -(_definition.Model.forward);
        Vector3 IComplexHuman.rt => _definition.Model.right;
        Vector3 IComplexHuman.lt => -(_definition.Model.right);
        Vector3 IComplexHuman.dn => -(_definition.Model.up);
        Vector3 IPositionable.position
        {
            get => _definition.Model.position;
            set => _definition.Model.position = value;
        }
        Quaternion IRotatable.rotation {
            get => _definition.Model.rotation;
            set => _definition.Model.rotation = value;
        }
        Vector3 IComplexHuman.localScale
        {
            get => _definition.Model.localScale;
            set => _definition.Model.localScale = value;
        }
        float IComplexHuman.GetTimeByKey(string key) => _timeByKey.TryGetValue(key, out var t) ? t : -1;
        IComplexHuman IComplexHuman.SetTimeByKey(string key, float time)
        {
            if(time < 0) _timeByKey.Remove(key);
            else _timeByKey[key] = time;
            return this;
        }
        void IComplexHuman.LookAt(Vector3 target, Vector3 up) => _definition.Model.LookAt(target, up);
//        AudioSource IGenHuman.AudioSourceHead => _audioSourceHead;
//        bool IGenHuman.IsHeadAudioPlaying => _audioSourceHead.isPlaying;
        void IRotatableAroundPivot.HorzPivotStarts(in Vector3 pivotPoint, out Vector3 vecPivotIn)
        {
            vecPivotIn = (pivotPoint - _this.position).WithY(0);
        }
        void IRotatableAroundPivot.HorzPivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn)
        {
            var vecPivotOut = (_this.position - pivotPoint).WithY(0);
            _this.position += vecPivotIn + vecPivotOut;
        }
        void IRotatableAroundPivot.HorzPivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn, in Quaternion iniPivotRot, in Quaternion currPivotRot)
        {
            _this.HorzPivotEnds(pivotPoint, vecPivotIn);

            _this.rotation *= Quaternion.Inverse(iniPivotRot) * currPivotRot;
            _this.LookAt(_this.position + _this.forward.ToHorzUnit(), v3.up);
        }
        void IRotatableAroundPivot.PivotStarts(in Vector3 pivotPoint, out Vector3 vecPivotIn)
        {
            vecPivotIn = (pivotPoint - _this.position);
        }
        void IRotatableAroundPivot.PivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn)
        {
            var vecPivotOut = (_this.position - pivotPoint);
            _this.position += vecPivotIn + vecPivotOut;
        }

        public override string ToString()
        {
            return "Human_ID="+_this.ID+ "_Persona=" + _this.Persona;
        }
    }
}