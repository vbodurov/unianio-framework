using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Rigged.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Moves
{
    public interface IMover : IExecutorOfProgress
    {

    }
    public class Mover<T> : IExecutorOfProgress where T : IManipulator3D
    {
        readonly T _obj;
        IVectorByProgress _move, _forward, _up, _scale;
        IQuaternionByProgress _rotate;
        int _initialFrame = -1;
        ObjectSpace _currentSpace = ObjectSpace.None;
        ObjectSpace _moveSpace = ObjectSpace.None;
        ObjectSpace _rotateSpace = ObjectSpace.None;
        ObjectSpace _scaleSpace = ObjectSpace.None;
        Func<int> _getFrameCount = () => Time.frameCount;
        Vector3 _startPos;
        Quaternion _startRot;
        Func<double, double> _posMergeFunc, _rotMergeFunc;
        public Mover(T obj)
        {
            _obj = obj;
        }
        public Mover<T> SetStartPos(Vector3 pos, Func<double, double> posMergeFunc)
            => SetStartPos(m => pos, posMergeFunc);
        public Mover<T> SetStartPos(Func<T, Vector3> setPos, Func<double, double> posMergeFunc)
        {
            _startPos = setPos(_obj);
            _posMergeFunc = posMergeFunc ?? throw new ArgumentException("SetStartPos requires merge function");
            return this;
        }
        public Mover<T> SetStartRot(in Vector3 fw, in Vector3 up, Func<double, double> rotMergeFunc)
        {
            return SetStartRot(lookAt(in fw, in up), rotMergeFunc);
        }
        public Mover<T> SetStartRot(Quaternion rot, Func<double, double> rotMergeFunc)
            => SetStartRot(m => rot, rotMergeFunc);
        public Mover<T> SetStartRot(Func<T, Quaternion> setRot, Func<double, double> rotMergeFunc)
        {
            _startRot = setRot(_obj);
            _rotMergeFunc = rotMergeFunc ?? throw new ArgumentException("SetStartRot requires merge function");
            return this;
        }
        public T Object => _obj;
        public Mover<T> New()
        {
            _posMergeFunc = null;
            _rotMergeFunc = null;
            _initialFrame = _getFrameCount();
            _currentSpace = ObjectSpace.None;
            _moveSpace = ObjectSpace.None;
            _rotateSpace = ObjectSpace.None;
            _scaleSpace = ObjectSpace.None;
            return this;
        }
        public Mover<T> ScaleTo(Vector3 target, Func<double, double> func = null)
            => ScaleTo(m => target, func);
        public Mover<T> ScaleTo(Func<T, Vector3> getTarget, Func<double, double> func = null)
        {
            if(_moveSpace != ObjectSpace.Local) throw new Exception("Scale can only be done in local space, pleace call .Local first");

            var sca = _obj.LocalScale;
            return SetSca(move.Line(
                m => sca,
                m => getTarget(_obj),
                func
            ));
        }

        public Mover<T> LineTo(Vector3 target, Func<double, double> func = null)
            => LineTo(m => target, func);
        public Mover<T> LineTo(Func<T, Vector3> getTarget, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Line(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => getTarget(_obj),
                func
                ));
        }

        public Mover<T> LineTo2(Vector3 target1, double start2, Vector3 target2, 
            Func<double, double> func1 = null, Func<double, double> func2 = null)
            => LineTo2(m => target1, start2, m => target2, func1, func2);
        public Mover<T> LineTo2(Func<T, Vector3> getTarget1, double start2, Func<T, Vector3> getTarget2, 
            Func<double, double> func1 = null, Func<double, double> func2 = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            var part1 = move.Line(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => getTarget1(_obj),
                func1
            );
            var part2 = move.Line(
                m => getTarget1(_obj),
                m => getTarget2(_obj),
                func2
            );
            return SetPos(
                move.Composite(
                    move.InRange(0.0, start2, part1), 
                    move.InRange(start2, 1.0, part2)));
        }

        public Mover<T> CurveRelToMid(Vector3 target, Vector3 control, Func<double, double> func = null)
            => CurveRelToMid(m => target, m => control, func);
        public Mover<T> CurveRelToMid(Func<T, Vector3> getTarget, Func<T, Vector3> getControl, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Curve(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => lerp(m.From, m.To, 0.5) + getControl(_obj),
                m => getTarget(_obj),
                func
            ));
        }

        public Mover<T> CurveRelToStart(Vector3 target, Vector3 control, Func<double, double> func = null)
            => CurveRelToStart(m => target, m => control, func);
        public Mover<T> CurveRelToStart(Func<T, Vector3> getTarget, Func<T, Vector3> getControl, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Curve(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => m.From + getControl(_obj),
                m => getTarget(_obj),
                func
            ));
        }

        public Mover<T> CurveRelToEnd(Vector3 target, Vector3 control, Func<double, double> func = null)
            => CurveRelToStart(m => target, m => control, func);
        public Mover<T> CurveRelToEnd(Func<T, Vector3> getTarget, Func<T, Vector3> getControl, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Curve(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => m.To + getControl(_obj),
                m => getTarget(_obj),
                func
            ));
        }


        public Mover<T> CurveRelToMid(Vector3 target, Vector3 control1, Vector3 control2, Func<double, double> func = null)
            => CurveRelToMid(m => target, m => control1, m => control2, func);
        public Mover<T> CurveRelToMid(Func<T, Vector3> getTarget, Func<T, Vector3> getControl1, Func<T, Vector3> getControl2, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Curve(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => lerp(m.From, m.To, 0.5) + getControl1(_obj),
                m => lerp(m.From, m.To, 0.5) + getControl2(_obj),
                m => getTarget(_obj),
                func
            ));
        }
        public Mover<T> CurveRelToEdges(Vector3 target, Vector3 control1, Vector3 control2, Func<double, double> func = null)
            => CurveRelToEdges(m => target, m => control1, m => control2, func);
        public Mover<T> CurveRelToEdges(Func<T, Vector3> getTarget, Func<T, Vector3> getControl1, Func<T, Vector3> getControl2, Func<double, double> func = null)
        {
            var pos = _obj.GetPosBySpace(_currentSpace);
            return SetPos(move.Curve(
                m => _posMergeFunc != null ? lerp(in pos, in _startPos, _posMergeFunc(m.X)) : pos,
                m => m.From + getControl1(_obj),
                m => m.To + getControl2(_obj),
                m => getTarget(_obj),
                func
            ));
        }

        public Mover<T> RotateTo(Quaternion target, Func<double, double> func = null)
            => RotateTo(m => target, func);
        public Mover<T> RotateTo(Vector3 fw, Vector3 up, Func<double, double> func = null)
            => RotateTo(m => lookAt(in fw, in up), func);
        public Mover<T> RotateTo(Func<T, Vector3> getFw, Func<T, Vector3> getUp, Func<double, double> func = null)
            => RotateTo(m => lookAt(getFw(_obj), getUp(_obj)), func);
        public Mover<T> RotateTo(Func<T, Quaternion> getTarget, Func<double, double> func = null)
        {
            var rot = _obj.GetRotBySpace(_currentSpace);
            SetRot(move.Rotate(
                m =>  _rotMergeFunc != null ? slerp(in rot, in _startRot, _rotMergeFunc(m.X)) : rot,
                m => getTarget(_obj),
                func
            ));
            return this;
        }



        public Mover<T> RotateTo2(Quaternion target1, double start2, Quaternion target2, Func<double, double> func1 = null, Func<double, double> func2 = null)
            => RotateTo2(m => target1, start2, m => target2, func1, func2);
        public Mover<T> RotateTo2(Vector3 fw1, Vector3 up1, double start2, Vector3 fw2, Vector3 up2, Func<double, double> func1 = null, Func<double, double> func2 = null)
            => RotateTo2(m => lookAt(in fw1, in up1), start2, m => lookAt(in fw2, in up2), func1, func2);
        public Mover<T> RotateTo2(Func<T, Quaternion> getTarget1, double start2, Func<T, Quaternion> getTarget2, Func<double, double> func1 = null, Func<double, double> func2 = null)
        {
            var rot = _obj.GetRotBySpace(_currentSpace);
            var part1 = move.Rotate(
                m => _rotMergeFunc != null ? slerp(in rot, in _startRot, _rotMergeFunc(m.X)) : rot,
                m => getTarget1(_obj),
                func1
            );
            var part2 = move.Rotate(
                m => getTarget1(_obj),
                m => getTarget2(_obj),
                func2
            );
            return SetRot(
                move.Composite(
                    move.InRange(0.0, start2, part1),
                    move.InRange(start2, 1.0, part2)));
        }

        /// <summary>
        /// call to not reset when calling Local, Model, or World after the initial frame
        /// </summary>
        public Mover<T> UpdateInitialFrame()
        {
            _initialFrame = Time.frameCount;
            return this;
        }
        public Mover<T> Local
        {
            get
            {
                EnsureReset();
                _currentSpace = ObjectSpace.Local;
                return this;
            }
        }
        public Mover<T> Model
        {
            get
            {
                EnsureReset();
                _currentSpace = ObjectSpace.Model;
                return this;
            }
        }
        public Mover<T> World
        {
            get
            {
                EnsureReset();
                _currentSpace = ObjectSpace.World;
                return this;
            }
        }
        public Mover<T> SetFrameCountGetter(Func<int> getFrameCount)
        {
            if(getFrameCount != null) _getFrameCount = getFrameCount;
            return this;
        }
        void EnsureReset()
        {
            var frameCount = _getFrameCount();
            if (_initialFrame < frameCount) New();
        }
        public Mover<T> SetSca(IVectorByProgress scale)
        {
            _scaleSpace = _currentSpace;
            _scale = scale;
            return this;
        }
        public Mover<T> SetRot(IQuaternionByProgress rotate)
        {
            _rotateSpace = _currentSpace;
            _rotate = rotate;
            return this;
        }
        public Mover<T> SetPos(IVectorByProgress move)
        {
            _moveSpace = _currentSpace;
            _move = move;
            return this;
        }
        public Mover<T> WrapRot(Func<float, Quaternion, Quaternion> func)
        {
            if (_rotate == null) throw new ArgumentException("Nothing to wrap, no need to call WrapRot");
            _rotate = new DynamicWrappedRotationMove(func, _rotate);
            return this;
        }
        public Mover<T> WrapPos(Func<float, Vector3, Vector3> func)
        {
            if(_move == null) throw new ArgumentException("Nothing to wrap, no need to call WrapPos");
            _move = new DynamicWrappedPositionMove(func, _move);
            return this;
        }
        public Mover<T> WrapSca(Func<float, Vector3, Vector3> func)
        {
            if (_scale == null) throw new ArgumentException("Nothing to wrap, no need to call WrapSca");
            _scale = new DynamicWrappedPositionMove(func, _scale);
            return this;
        }
        public int Tag { get; set; }
        public void Apply(double progress, Func<double, double> function = null)
        {
            if(function != null) progress = function(progress);
            TryApplyScale(progress);
            TryApplyRotation(progress);
            TryApplyPosition(progress);
        }
        bool TryApplyRotation(double progress)
        {
            if (_rotateSpace == ObjectSpace.None) return false;
            var rotChange = false;
            switch (_rotateSpace)
            {
                case ObjectSpace.World:
                    rotChange = _obj.TrySetWorldRot(_rotate.GetValueByProgress(progress));
                    break;
                case ObjectSpace.Model:
                    rotChange = _obj.TrySetModelRot(_rotate.GetValueByProgress(progress));
                    break;
                case ObjectSpace.Local:
                    rotChange = _obj.TrySetLocalRot(_rotate.GetValueByProgress(progress));
                    break;
            }
            
            return rotChange;
        }
        bool TryApplyPosition(double progress)
        {
            if (_moveSpace == ObjectSpace.None) return false;
            var posChange = false;
            switch (_moveSpace)
            {
                case ObjectSpace.World:
                    posChange = _obj.TrySetWorldPos(_move.GetValueByProgress(progress));
                    break;
                case ObjectSpace.Model:
                    posChange = _obj.TrySetModelPos(_move.GetValueByProgress(progress));
                    break;
                case ObjectSpace.Local:
                    posChange = _obj.TrySetLocalPos(_move.GetValueByProgress(progress));
                    break;
            }
            return posChange;
        }
        bool TryApplyScale(double progress)
        {
            if (_scaleSpace == ObjectSpace.None) return false;
            var scaChange = false;
            switch (_scaleSpace)
            {
                case ObjectSpace.Local:
                    scaChange = _obj.TrySetLocalScale(_scale.GetValueByProgress(progress));
                    break;
                default:
                    throw new Exception("HandlePath.Scale allows only local space");
            }
            return scaChange;
        }
    }
}