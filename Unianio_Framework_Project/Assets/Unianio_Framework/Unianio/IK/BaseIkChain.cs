using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Rigged;
using Unianio.Rigged.IK;
using Unianio.Static;
using UnityEngine;

namespace Unianio.IK
{
    public abstract class BaseIkChain : Manipulator3DBase, IUpdatable
    {
        protected Vector3 _prevHandlePos;
        protected Quaternion _prevHandleRot;
        protected Transform _handle, _model;
        

        protected BaseIkChain(BodyPart part) : base(part) {}

        public Transform Handle => _handle;
        public virtual void Update()
        {
            var currHandlePos = _handle.localPosition;
            var currHandleRot = _handle.localRotation;

            var hasPositionChange = !_prevHandlePos.IsEqual(in currHandlePos, 0.00002);
            var hasRotationChange = !_prevHandleRot.IsEqual(in currHandleRot, 0.00002);
            var hasChange = hasPositionChange || hasRotationChange;
            
            if (hasChange)
            {
                ProcessMove(hasPositionChange, hasRotationChange);
            }
            _prevHandlePos = currHandlePos;
            _prevHandleRot = currHandleRot;
        }
        protected static bool IsWithinLimit(
            ref Vector3 limitPlaneNormal, ref Vector3 pivotPoint,
            ref Vector3 point, out Vector3 correctedPoint)
        {
            if (fun.point.IsAbovePlane(in point, in limitPlaneNormal, in pivotPoint))
            {
                correctedPoint = point;
                return true;
            }
            fun.point.ProjectOnPlane(in point, in limitPlaneNormal, in pivotPoint, out correctedPoint);
            return false;
        }
        protected abstract void ProcessMove(bool hasPositionChange, bool hasRotationChange);
        public override ManipulatorType ManipulatorType => ManipulatorType.Chain;
        public override Transform Model => _model;
        public override Transform Control => Handle;
        public override Vector3 ModelPos => Handle.position.AsLocalPoint(Model);
        public override Vector3 ModelFw => Handle.forward.AsLocalDir(Model);
        
        public override Vector3 ModelUp => Handle.up.AsLocalDir(Model);
        public override Vector3 LocalPos => Handle.localPosition;
        public override Vector3 LocalFw => Handle.parent.InverseTransformDirection(Handle.forward);
        public override Vector3 LocalUp => Handle.parent.InverseTransformDirection(Handle.up);
        public override Vector3 LocalScale => Handle.localScale;
    }
}