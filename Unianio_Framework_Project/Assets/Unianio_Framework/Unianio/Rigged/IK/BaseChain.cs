using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Rigged.IK
{
    public abstract class BaseChain : Manipulator3DBase
    {
        protected Vector3 _prevHandlePos;
        protected Quaternion _prevHandleRot;
        protected Transform _model;
        protected Transform _handle;
        protected bool _canChangeRotation = true;

        public Transform Handle => _handle;
        public abstract Transform[] AllJoins { get; }
        public Vector3 HandlePositionInLocalSpace => _handle.localPosition;
        public Vector3 HandleForwardInLocalSpace => _handle.parent.InverseTransformDirection(_handle.forward);
        public Vector3 HandleUpInLocalSpace => _handle.parent.InverseTransformDirection(_handle.up);
        public Vector3 HandleScaleInLocalSpace => _handle.localScale;
        public Vector3 HandlePositionInModelSpace => _model.InverseTransformPoint(_handle.position);
        public Vector3 HandleForwardInModelSpace => _model.InverseTransformDirection(_handle.forward);
        public Vector3 HandleUpInModelSpace => _model.InverseTransformDirection(_handle.up);
        public override Vector3 ModelPos => HandlePositionInModelSpace;
        public override Vector3 ModelFw => HandleForwardInModelSpace;
        public override Vector3 ModelUp => HandleUpInModelSpace;
        public override Vector3 LocalPos => HandlePositionInLocalSpace;
        public override Vector3 LocalFw => HandleForwardInLocalSpace;
        public override Vector3 LocalUp => HandleUpInLocalSpace;
        public override Vector3 LocalScale => HandleScaleInLocalSpace;
        public override ManipulatorType ManipulatorType => ManipulatorType.Chain;
        protected BaseChain(HumanoidPart part) : base(part) { }

        protected abstract void ProcessMove(bool hasPositionChange, bool hasRotationChange);

        public virtual bool Update()
        {
            var currHandlePos = _handle.localPosition;
            var currHandleRot = _handle.localRotation;

            var hasPositionChange = !_prevHandlePos.IsEqual(in currHandlePos,0.00002);
            var hasRotationChange = _canChangeRotation && !_prevHandleRot.IsEqual(in currHandleRot,0.00002);
            var hasChange = hasPositionChange || hasRotationChange;

            if (hasChange)
            {
                ProcessMove(hasPositionChange, hasRotationChange);
            }
            _prevHandlePos = currHandlePos;
            _prevHandleRot = currHandleRot;

            return hasChange;
        }

        protected static Transform CreateRoot(HumanoidPart part, Transform first)
        {
            var root = first.parent;
            var holderGameObj = new GameObject(part + HolderSuffix);
            var holder = holderGameObj.transform;
            holder.position = root.position;
            holder.LookAt(first.position, v3.up);
            holder.SetParent(root);
            first.SetParent(holder);
            return holder;
        }
        public static Transform CreateRoot(HumanoidPart part, Transform first, Vector3 worldPos, Vector3 worldLookAtTarget, Vector3 worldUp)
        {
            var parent = first.parent;
            var holderGameObj = new GameObject(part + HolderSuffix);
            var holder = holderGameObj.transform;
            holder.position = worldPos;
            holder.LookAt(worldLookAtTarget, worldUp);
            holder.SetParent(parent);
            first.SetParent(holder);
            return holder;
        }
        public static Transform CreateHandle(
            Transform handleParent, HumanoidPart part, bool isHandleVisibled,
            Vector3 worldPosition, Vector3 worldForward, Vector3 worldUp)
        {
            GameObject handle;
            if (isHandleVisibled)
            {
                handle = 
                    fun.meshes.CreatePointyCone(new DtCone {name = part+HandleSuffix})
                    .SetMaterial(m => m.color = ControlColor);//m.SetStandardShaderRenderingModeTransparent()
                const float scale = 0.1f;
                handle.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                handle = new GameObject(part+HandleSuffix);
            }
            handle.transform.SetParent(handleParent);
            handle.transform.position = worldPosition;
            handle.transform.LookAt(worldPosition + worldForward, worldUp);
            return handle.transform;
        }
        public static Transform WrapInHolder(Transform parent, Transform current, Vector3 lookAtWorldPoint, Vector3 currWorldUp)
        {
            // already wrapped
//if(current.parent != null) dbg.log(current.name, current.parent.name);
            if (current.parent != null && current.parent.name == current.name + HolderSuffix)
            {
                return current.parent;
            }

            var currentHolder = new GameObject(current.name + HolderSuffix);
            var holder = currentHolder.transform;
            holder.position = current.position;
            holder.LookAt(lookAtWorldPoint, currWorldUp);
            holder.SetParent(parent);
            current.SetParent(holder);
            return holder;
        }
        public static bool IsWithinLimit(
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
    }
}