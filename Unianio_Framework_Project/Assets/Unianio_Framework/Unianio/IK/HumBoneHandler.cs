using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis.IK.Input;
using Unianio.Human.Input;
using Unianio.Moves;
using Unianio.Rigged;
using Unianio.Rigged.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public class HumBoneHandler : Manipulator3DBase, IInitialOrientationHolder
    {
        readonly Transform _bone;
        readonly HumanBoneInput _input;


        public HumBoneHandler(Transform model, Transform bone) : base(BodyPart.Nothing)
        {
            _input = new HumanBoneInput(BodyPart.Nothing, model, bone);
            _bone = bone;
            RecalculateOriginals();
        }
        public HumBoneHandler(BodyPart part, Transform model, Transform bone) : base(part)
        {
            _input = new HumanBoneInput(part, model, bone);
            _bone = bone;
            RecalculateOriginals();
        }
        void RecalculateOriginals()
        {
            IniLocalSca = _bone.localScale;
            IniLocalPos = _bone.localPosition;
            IniModelPos = _bone.position.AsLocalPoint(_input.Model);
            IniLocalRot = _bone.localRotation;
            IniModelRot = lookAt(_bone.forward.AsLocalDir(_input.Model), _bone.up.AsLocalDir(_input.Model));
        }
        public Transform Holder => _bone;
        public Vector3 position
        {
            get => _bone.position;
            set => _bone.position = value;
        }
        public Vector3 localPosition {
            get => _bone.localPosition;
            set => _bone.localPosition = value;
        }
        public Quaternion rotation {
            get => _bone.rotation;
            set => _bone.rotation = value;
        }
        public Quaternion localRotation {
            get => _bone.localRotation;
            set => _bone.localRotation = value;
        }
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
        public Vector3 IniLocalPos { get; private set; }
        public Vector3 IniModelPos { get; private set; }
        public Quaternion IniLocalRot { get; private set; }
        public Quaternion IniModelRot { get; private set; }
        public Vector3 IniLocalSca { get; private set; }
        public Vector3 IniLocalFw => IniLocalRot * Vector3.forward;
        public Vector3 IniLocalUp => IniLocalRot * Vector3.up;
        public Vector3 IniModelFw => IniModelRot * Vector3.forward;
        public Vector3 IniModelUp => IniModelRot * Vector3.up;
        public IExecutorOfProgress ToInitialLocalPosition()
        {
            return new Mover<HumBoneHandler>(this).New().Local.LineTo(b => b.IniLocalPos);
        }
        public IExecutorOfProgress ToInitialLocalRotation()
        {
            return new Mover<HumBoneHandler>(this).New().Local.RotateTo(b => b.IniLocalRot);
        }
        public IExecutorOfProgress ToInitialLocalScale()
        {
            return new Mover<HumBoneHandler>(this).New().Local.ScaleTo(b => b.IniLocalSca);
        }
        public IExecutorOfProgress ToInitialLocal()
        {
            return new Mover<HumBoneHandler>(this).New().Local
                    .LineTo(b => b.IniLocalPos)
                    .RotateTo(b => b.IniLocalRot)
                    .ScaleTo(b => b.IniLocalSca)
                ;
        }
        public HumBoneHandler MoveTowards(in Vector3 worldTarget, double step = 360)
        {
            Holder.MoveTowards(worldTarget, step);
            return this;
        }
        public HumBoneHandler MoveTowardsLocal(Vector3 localTarget, double step = 360)
        {
            Holder.MoveTowardsLocal(localTarget, step);
            return this;
        }
        public HumBoneHandler RotateTowards(Quaternion rot, double step = 360)
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
        public HumBoneHandler RotateTowardsLocal(Quaternion rot, double step = 360)
        {
            Holder.RotateTowardsLocal(rot, step);
            return this;
        }
        public HumBoneHandler RotateTowards(in Vector3 fwDir, in Vector3 upDir, double step = 360)
        {
            Holder.RotateTowards(in fwDir, in upDir, step);
            return this;
        }
        public HumBoneHandler LookAt(in Vector3 target, in Vector3 upDir, double step = 360)
        {
            Holder.RotateTowards(Holder.position.DirTo(in target), in upDir, step);
            return this;
        }
        public HumBoneHandler RotateTowardsTarget(Vector3 target, Vector3 upDir, double step = 360)
        {
            Holder.RotateTowards((target - Holder.position).normalized, upDir, step);
            return this;
        }
        public HumBoneHandler RotateTowardsTarget(Vector3 target, double step = 360)
        {
            Holder.RotateTowards((target - Holder.position).normalized, (IniLocalRot * v3.up).AsWorldDir(Holder.parent), step);
            return this;
        }
        public HumBoneHandler RotateTowardsLocal(Vector3 fwDir, Vector3 upDir, double step = 360)
        {
            Holder.RotateTowardsLocal(fwDir, upDir, step);
            return this;
        }

        public override ManipulatorType ManipulatorType => ManipulatorType.Chain;
        public override Transform Model => _input.Model;
        public override Transform Control => Holder;
        public override Vector3 ModelPos => Holder.position.AsLocalPoint(Model);
        public override Vector3 ModelFw => Holder.forward.AsLocalDir(Model);
        public override Vector3 ModelUp => Holder.up.AsLocalDir(Model);
        public override Vector3 LocalPos => Holder.localPosition;
        public override Vector3 LocalFw => Holder.parent.InverseTransformDirection(Holder.forward);
        public override Vector3 LocalUp => Holder.parent.InverseTransformDirection(Holder.up);
        public override Vector3 LocalScale => Holder.localScale;
        public Vector3 DirTo(Transform target) => Holder.DirTo(target);
        public Vector3 DirTo(in Vector3 target) => Holder.DirTo(in target);
    }
}