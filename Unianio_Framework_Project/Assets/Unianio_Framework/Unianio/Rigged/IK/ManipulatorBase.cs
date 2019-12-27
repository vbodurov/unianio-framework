using Unianio.Enums;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Rigged.IK
{
    public interface IControlHolder
    {
        Transform Control { get; }
    }
    public interface IManipulator3D : IControlHolder
    {
        ManipulatorType ManipulatorType { get; }
        BodyPart Part { get; }
        Transform Model { get; }
        Vector3 ModelPos { get; }
        Vector3 ModelFw { get; }
        Vector3 ModelUp { get; }
        Quaternion RotationInModelSpace { get; }

        Vector3 LocalPos { get; }
        Vector3 LocalFw { get; }
        Vector3 LocalUp { get; }
        Vector3 LocalScale { get; }
        Quaternion RotationInLocalSpace { get; }

        bool TrySetWorldPos(Vector3 worldPos);
        bool TrySetModelPos(Vector3 modelPos);
        bool TrySetLocalPos(Vector3 localPos);
        bool TrySetWorldRot(Vector3 worldForward, Vector3 worldUp);
        bool TrySetModelRot(Vector3 modelForward, Vector3 modelUp);
        bool TrySetLocalRot(Quaternion localRotation);
        bool TryMarkCurrentPos();
        bool TryMarkCurrentRot();
        bool TrySetModelRot(Quaternion quaternion);
        bool TrySetWorldRot(Quaternion quaternion);
        IManipulator3D ClearPosMask();
        IManipulator3D ClearRotMask();
        bool TrySetLocalScale(Vector3 getValueByProgress);
        IManipulator3D ClearScaMask();
        bool TryMarkCurrentScale();

    }
    public abstract class Manipulator3DBase : IManipulator3D
    {
        readonly BodyPart _part;
        public static readonly Color ControlColor = new Color(0.75f, 0.0f, 0.75f, 1f);
        
        int _lastFrame;
        bool _hasPosition, _hasRotation, _hasScale;

        protected Manipulator3DBase(BodyPart part) => _part = part;
        public abstract ManipulatorType ManipulatorType { get; }
        public BodyPart Part => _part;
        public abstract Transform Model { get; }
        public abstract Transform Control { get; }
        public abstract Vector3 LocalPos { get; }
        public abstract Vector3 LocalFw { get; }
        public abstract Vector3 LocalUp { get; }
        public abstract Vector3 LocalScale { get; }
        public abstract Vector3 ModelPos { get; }
        public abstract Vector3 ModelFw { get; }
        public abstract Vector3 ModelUp { get; }
        public virtual Quaternion RotationInModelSpace => lookAt(ModelFw, ModelUp);
        public virtual Quaternion RotationInLocalSpace => Control.localRotation;

        bool IManipulator3D.TrySetWorldPos(Vector3 worldPos)
        {
            if (HasAppliedPosition) return false;
            Control.position = worldPos;
            _hasPosition = true;
            return true;
        }
        bool IManipulator3D.TrySetModelPos(Vector3 modelPos)
        {
            if (HasAppliedPosition) return false;
            Control.position = Model.TransformPoint(modelPos);
            _hasPosition = true;
            return true;
        }
        bool IManipulator3D.TrySetLocalPos(Vector3 localPos)
        {
            if (HasAppliedPosition) return false;
            Control.localPosition = localPos;
            _hasPosition = true;
            return true;
        }

        bool IManipulator3D.TrySetWorldRot(Vector3 worldForward, Vector3 worldUp)
        {
            if (HasAppliedRotation) return false;
            Control.LookAt(Control.position + worldForward, worldUp);
            _hasRotation = true;
            return true;
        }
        bool IManipulator3D.TrySetWorldRot(Quaternion quaternion)
        {
            if (HasAppliedRotation) return false;
            Control.rotation = quaternion;
            _hasRotation = true;
            return true;
        }

        bool IManipulator3D.TrySetModelRot(Vector3 modelForward, Vector3 modelUp)
        {
            if (HasAppliedRotation) return false;
            var worldForward = Model.TransformDirection(modelForward);
            var worldUp = Model.TransformDirection(modelUp);
            Control.LookAt(Control.position + worldForward, worldUp);
            _hasRotation = true;
            return true;
        }
        bool IManipulator3D.TrySetModelRot(Quaternion quaternion)
        {
            if (HasAppliedRotation) return false;
            var fwWorld = Model.TransformDirection(quaternion*Vector3.forward);
            var upWorld = Model.TransformDirection(quaternion*Vector3.up);
            Control.rotation = Quaternion.LookRotation(fwWorld, upWorld);
            _hasRotation = true;
            return true;
        }

        bool IManipulator3D.TrySetLocalRot(Quaternion localRotation)
        {
            if (HasAppliedRotation) return false;
            Control.localRotation = localRotation;
            _hasRotation = true;
            return true;
        }
        bool IManipulator3D.TryMarkCurrentPos()
        {
            if (HasAppliedPosition) return false;
            _hasPosition = true;
            return true;
        }
        bool IManipulator3D.TryMarkCurrentRot()
        {
            if (HasAppliedRotation) return false;
            _hasRotation = true;
            return true;
        }
        bool IManipulator3D.TryMarkCurrentScale()
        {
            if (HasAppliedScale) return false;
            _hasScale = true;
            return true;
        }
        IManipulator3D IManipulator3D.ClearPosMask()
        {
            _hasPosition = false;
            return this;
        }
        IManipulator3D IManipulator3D.ClearRotMask()
        {
            _hasRotation = false;
            return this;
        }
        IManipulator3D IManipulator3D.ClearScaMask()
        {
            _hasScale = false;
            return this;
        }
        bool IManipulator3D.TrySetLocalScale(Vector3 localScale)
        {
            if (HasAppliedScale) return false;
            Control.localScale = localScale;
            _hasScale = true;
            return true;
        }
        bool HasAppliedPosition
        {
            get
            {
                EnsureFrame();
                return _hasPosition;
            }
        }
        bool HasAppliedRotation
        {
            get
            {
                EnsureFrame();
                return _hasRotation;
            }
        }
        bool HasAppliedScale
        {
            get
            {
                EnsureFrame();
                return _hasScale;
            }
        }
        private void EnsureFrame()
        {
            var cnt = Time.frameCount;
            if (cnt > _lastFrame)
            {
                _lastFrame = cnt;
                _hasPosition = false;
                _hasRotation = false;
                _hasScale = false;
            }
        }
    }
}