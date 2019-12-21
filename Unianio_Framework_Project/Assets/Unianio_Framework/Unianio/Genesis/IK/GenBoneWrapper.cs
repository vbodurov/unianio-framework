using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis.IK.Input;
using Unianio.Human.Input;
using Unianio.Rigged;
using Unianio.Rigged.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis.IK
{
    public class GenBoneWrapper : Manipulator3DBase, IInitialOrientationHolder
    {
        readonly Transform _bone;
        Vector3 _iniLocalPos, _iniModelPos, _iniLocalUp, _iniLocalFw, _iniModelUp, _iniModelFw;
        Quaternion _iniLocalRot, _iniModelRot;
        readonly HumanBoneInput _input;

        public GenBoneWrapper(Transform model, Transform unwrappedBone, Vector3 forwardInWorldSpace, Vector3 upInWorldSpace) : base(HumanoidPart.Nothing)
        {
            _input = new HumanBoneInput(HumanoidPart.Nothing, model, unwrappedBone);
            _bone = wrapTransformInHolder(unwrappedBone.parent, unwrappedBone, forwardInWorldSpace, forwardInWorldSpace.GetRealUp(upInWorldSpace));
            RecalculateOriginals();
        }
        public GenBoneWrapper(HumanoidPart part, Transform model, Transform existingWrapper) : base(part)
        {
            _input = new HumanBoneInput(part, model, existingWrapper);
            _bone = existingWrapper;
            RecalculateOriginals();
        }
        public GenBoneWrapper(HumanBoneInput definition, Vector3 forwardInWorldSpace, Vector3 upInWorldSpace) : base(definition.Part)
        {
            _input = definition;
            _bone = wrapTransformInHolder(definition.Bone.parent, definition.Bone, forwardInWorldSpace, forwardInWorldSpace.GetRealUp(upInWorldSpace));
            RecalculateOriginals();
        }
        public GenBoneWrapper RecalculateOriginals()
        {
            _iniLocalPos = _bone.localPosition;
            _iniModelPos = _bone.position.AsLocalPoint(_input.Model);
            _iniLocalRot = _bone.localRotation;
            _iniLocalUp = _bone.localRotation * v3.up;
            _iniLocalFw = _bone.localRotation * v3.fw;
            _iniModelUp = _bone.up.AsLocalDir(_input.Model);
            _iniModelFw = _bone.forward.AsLocalDir(_input.Model);
            _iniModelRot = Quaternion.LookRotation(_bone.forward.AsLocalDir(_input.Model), _bone.up.AsLocalDir(_input.Model));
            return this;
        }
        public Transform Holder => _bone;
        public Vector3 position { get { return _bone.position; } set { _bone.position = value; } }
        public Vector3 localPosition { get { return _bone.localPosition; } set { _bone.localPosition = value; } }
        public Quaternion rotation { get { return _bone.rotation; } set { _bone.rotation = value; } }
        public Quaternion localRotation { get { return _bone.localRotation; } set { _bone.localRotation = value; } }
        public Vector3 forward => _bone.forward;
        public Vector3 back => -_bone.forward;
        public Vector3 right => _bone.right;
        public Vector3 left => -_bone.right;
        public Vector3 up => _bone.up;
        public Vector3 down => -_bone.up;
        public Vector3 fw => _bone.forward;
        public Vector3 bk => -_bone.forward;
        public Vector3 rt => _bone.right;
        public Vector3 lt => -_bone.right;
        public Vector3 dn => -_bone.up;
        public Vector3 IniLocalPos => _iniLocalPos;
        public Vector3 IniModelPos => _iniModelPos;
        internal float IniModelY => _iniModelPos.y;
        public Quaternion IniLocalRot => _iniLocalRot;
        public Quaternion IniModelRot => _iniModelRot;
        public Vector3 IniLocalFw => _iniLocalFw;
        public Vector3 IniLocalUp => _iniLocalUp;
        public Vector3 IniModelFw => _iniModelFw;
        public Vector3 IniModelUp => _iniModelUp;
        public Vector3 IniFwInWorldSpace => (_iniLocalRot * Vector3.forward).AsWorldDir(_bone.parent);
        public Vector3 IniUpInWorldSpace => (_iniLocalRot * Vector3.up).AsWorldDir(_bone.parent);
        public Quaternion IniLocRotInWorldSpace =>
            lookAt(
                (_iniLocalRot * Vector3.forward).AsWorldDir(_bone.parent), 
                (_iniLocalRot * Vector3.up).AsWorldDir(_bone.parent));

        public GenBoneWrapper MoveTowards(Vector3 worldTarget, double step = -1)
        {
            Holder.MoveTowards(worldTarget, step);
            return this;
        }
        public GenBoneWrapper MoveTowardsLocal(Vector3 localTarget, double step = -1)
        {
            Holder.MoveTowardsLocal(localTarget, step);
            return this;
        }
        public GenBoneWrapper RotateTowards(Quaternion rot, double step = -1)
        {
            Holder.RotateTowards(rot, step);
            return this;
        }
        public Quaternion RotTo(Vector3 targetPoint, Vector3 upDir)
        {
            return lookAt(targetPoint, Holder.position, upDir);
        }
        public Quaternion RotTo(Vector3 targetPoint)
        {
            return lookAt(targetPoint, Holder.position, v3.up);
        }
        public Quaternion RotTo(Transform target)
        {
            return lookAt(target.position, Holder.position, v3.up);
        }
        public Quaternion RotTo(Transform target, Vector3 upDir)
        {
            return lookAt(target.position, Holder.position, upDir);
        }
        public GenBoneWrapper RotateTowardsLocal(Quaternion rot, double step = -1)
        {
            Holder.RotateTowardsLocal(rot, step);
            return this;
        }
        public GenBoneWrapper RotateTowards(Vector3 fwDir, Vector3 upDir, double step = -1)
        {
            Holder.RotateTowards(fwDir, upDir, step);
            return this;
        }
        public GenBoneWrapper RotateTowardsTarget(Vector3 target, Vector3 upDir, double step = -1)
        {
            Holder.RotateTowards((target - Holder.position).normalized, upDir, step);
            return this;
        }
        public GenBoneWrapper RotateTowardsTarget(Vector3 target, double step = -1)
        {
            Holder.RotateTowards((target - Holder.position).normalized, (IniLocalRot*v3.up).AsWorldDir(Holder.parent), step);
            return this;
        }
        public GenBoneWrapper RotateTowardsLocal(Vector3 fwDir, Vector3 upDir, double step = -1)
        {
            Holder.RotateTowardsLocal(fwDir, upDir, step);
            return this;
        }

        public override ManipulatorType ManipulatorType => ManipulatorType.Chain;
        public override Transform Model => _input.Model;
        public override Transform Manipulator => Holder;
        public override Vector3 ModelPos => Holder.position.AsLocalPoint(Model);
        public override Vector3 ModelFw => Holder.forward.AsLocalDir(Model);
        public override Vector3 ModelUp => Holder.up.AsLocalDir(Model);
        public override Vector3 LocalPos => Holder.localPosition;
        public override Vector3 LocalFw => Holder.parent.InverseTransformDirection(Holder.forward);
        public override Vector3 LocalUp => Holder.parent.InverseTransformDirection(Holder.up);
        public override Vector3 LocalScale => Holder.localScale;
        public Vector3 DirTo(Transform target) => Holder.DirTo(target);
        public Vector3 DirTo(Vector3 target) => Holder.DirTo(target);
    }
}