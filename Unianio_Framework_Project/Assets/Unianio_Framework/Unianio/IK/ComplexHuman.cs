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
using Unianio.Human;
using Unianio.MakeHuman;
using Unianio.Moves;
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
        IControlHolder,
        IRotatableAroundPivot,
        ITimeProviderHolder,
        IDisposable
    {
        string Persona { get; }
        Transform Model { get; }
        IHumArmChain ArmL { get; }
        IHumArmChain ArmR { get; }
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
        Mover<HumBoneHandler> MoveHandL { get; }
        Mover<HumBoneHandler> MoveHandR { get; }
        Mover<IHumArmChain> MoveArmL { get; }
        Mover<IHumArmChain> MoveArmR { get; }
        Mover<HumLegChain> MoveLegL { get; }
        Mover<HumLegChain> MoveLegR { get; }
        Mover<HumBoneHandler> MoveFootL { get; }
        Mover<HumBoneHandler> MoveFootR { get; }
        Mover<HumBoneHandler> MoveToesL { get; }
        Mover<HumBoneHandler> MoveToesR { get; }
        Mover<HumBoneHandler> MovePelvis { get; }
        Mover<HumBoneHandler> MoveHip { get; }
        Mover<HumBoneHandler> MoveHead { get; }
        Mover<HumSpineChain> MoveSpine { get; }
        Mover<HumBoneHandler> MoveJaw { get; }
        Mover<HumBoneHandler> MoveNeckLower { get; }
        Mover<HumBoneHandler> MoveNeckUpper { get; }
        Mover<HumBoneHandler> MoveTongue { get; }
//        FacePath PathFace { get; }
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
        Mover<HumBoneHandler> _moveHandL, _moveHandR, _moveFootL, _moveFootR,
            _moveToesL, _moveToesR, _movePelvis, _moveHip, _moveHead,
            _moveJaw, _moveNeckLower, _moveNeckUpper, _moveTongue;
        Mover<HumSpineChain> _moveSpine;
        Mover<IHumArmChain> _moveArmL, _moveArmR;
        Mover<HumLegChain> _moveLegL, _moveLegR;
        ulong _id;
        HumanInitialStats _initialStats;
        IComplexHumanDefinition _definition;
        IHumArmChain _armL, _armR;
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
        
        bool _isDisposed;
        IPausableTimeProvider _pausableTime;

        internal ComplexHuman()
        {
            _this = this;
            _this.Flags = 0;
        }
        ulong IIdHolder.ID => _id;
        ulong IComplexHuman.Flags { get; set; }
        Transform IControlHolder.Control => _definition.Model;
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
            if (_definition.HumanoidType == HumanoidType.Genesis8)
            {
                _armL = new Gen8ArmChain(BodyPart.ArmL, _definition);
                _armR = new Gen8ArmChain(BodyPart.ArmR, _definition);
            }
            else //if (_definition.HumanoidType == HumanoidType.MakeHuman)
            {
                _armL = new MHArmChain(BodyPart.ArmL, _definition);
                _armR = new MHArmChain(BodyPart.ArmR, _definition);
            }
            

            _legL = new HumLegChain(BodyPart.LegL, _definition);
            _legR = new HumLegChain(BodyPart.LegR, _definition);
            var humFaceConfigSource = extenders.FirstOrDefault(e => e is HumanFaceConfigSource) as HumanFaceConfigSource; 
            if (_definition.HumanoidType == HumanoidType.Genesis8)
                _genFace = new GenFaceGroup(this, humFaceConfigSource);
            else if(_definition.HumanoidType == HumanoidType.MakeHuman)
                _mhFace = new MHFaceGroup(this, humFaceConfigSource);
            _collarL = new HumBoneHandler(BodyPart.CollarL, m, _definition.ArmL.Collar);
            _collarR = new HumBoneHandler(BodyPart.CollarR, m, _definition.ArmR.Collar);

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
        Mover<HumBoneHandler> IComplexHuman.MoveHandL => _moveHandL ?? (_moveHandL = new Mover<HumBoneHandler>(_armL.HandHandler));
        Mover<HumBoneHandler> IComplexHuman.MoveHandR => _moveHandR ?? (_moveHandR = new Mover<HumBoneHandler>(_armR.HandHandler));
        Mover<IHumArmChain> IComplexHuman.MoveArmL => _moveArmL ?? (_moveArmL = new Mover<IHumArmChain>(_armL));
        Mover<IHumArmChain> IComplexHuman.MoveArmR => _moveArmR ?? (_moveArmR = new Mover<IHumArmChain>(_armR));
        Mover<HumLegChain> IComplexHuman.MoveLegL => _moveLegL ?? (_moveLegL = new Mover<HumLegChain>(_legL));
        Mover<HumLegChain> IComplexHuman.MoveLegR => _moveLegR ?? (_moveLegR = new Mover<HumLegChain>(_legR));
        Mover<HumBoneHandler> IComplexHuman.MoveFootL => _moveFootL ?? (_moveFootL = new Mover<HumBoneHandler>(_legL.FootHandler));
        Mover<HumBoneHandler> IComplexHuman.MoveFootR => _moveFootR ?? (_moveFootR = new Mover<HumBoneHandler>(_legR.FootHandler));
        Mover<HumBoneHandler> IComplexHuman.MoveToesL => _moveToesL ?? (_moveToesL = new Mover<HumBoneHandler>(_legL.ToeHandler));
        Mover<HumBoneHandler> IComplexHuman.MoveToesR => _moveToesR ?? (_moveToesR = new Mover<HumBoneHandler>(_legR.ToeHandler));
        Mover<HumBoneHandler> IComplexHuman.MovePelvis => _movePelvis ?? (_movePelvis = new Mover<HumBoneHandler>(_pelvis));
        Mover<HumBoneHandler> IComplexHuman.MoveHip => _moveHip ?? (_moveHip = new Mover<HumBoneHandler>(_hip));
        Mover<HumBoneHandler> IComplexHuman.MoveHead => _moveHead ?? (_moveHead = new Mover<HumBoneHandler>(_head));
        Mover<HumBoneHandler> IComplexHuman.MoveTongue => _moveTongue ?? (_moveTongue = new Mover<HumBoneHandler>(_tongue));
        Mover<HumBoneHandler> IComplexHuman.MoveNeckLower => _moveNeckLower ?? (_moveNeckLower = new Mover<HumBoneHandler>(_spine.NeckLowerHandler));
        Mover<HumBoneHandler> IComplexHuman.MoveNeckUpper => _moveNeckUpper ?? (_moveNeckUpper = new Mover<HumBoneHandler>(_neckUpper));
        Mover<HumSpineChain> IComplexHuman.MoveSpine => _moveSpine ?? (_moveSpine = new Mover<HumSpineChain>(_spine));
        Mover<HumBoneHandler> IComplexHuman.MoveJaw => _moveJaw ?? (_moveJaw = new Mover<HumBoneHandler>(_this.IsGenesis8() ? _genFace.LowerJaw : _mhFace.Jaw));
        Transform IComplexHuman.GetBoneByName(string name) => _definition.BonesByName.GetOrDefault(name);
        HumanInitialStats IComplexHuman.Initial => _initialStats;
        Transform IComplexHuman.Model => _definition.Model;
        IHumArmChain IComplexHuman.ArmL => _armL;
        IHumArmChain IComplexHuman.ArmR => _armR;
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