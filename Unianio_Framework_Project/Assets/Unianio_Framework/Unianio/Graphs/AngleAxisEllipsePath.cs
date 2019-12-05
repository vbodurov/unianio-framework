using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Graphs
{
    public class AngleAxisEllipsePath : IVectorByProgress
    {
        readonly Vector3 _vector;
        readonly float _ellipseFactor;
        Vector3 _ellipseDir;
        readonly AngleAxisData _rotation;
        Func<double, double> _func;
        readonly bool _correctAxis;
        readonly Quaternion _correction;

        public AngleAxisEllipsePath(Vector3 vector, double angle, Vector3 axis, double ellipseFactor, Vector3 ellipseDir, bool correctAxis)
            : this(vector, angle, axis, ellipseFactor, ellipseDir, correctAxis, null) { }
        public AngleAxisEllipsePath(Vector3 vector, double angle, Vector3 axis, double ellipseFactor, Vector3 ellipseDir, bool correctAxis, Func<double,double> func)
        {
            _vector = vector;
            _ellipseFactor = (float)ellipseFactor;
            _ellipseDir = ellipseDir.normalized;
            axis = axis.normalized;
            fun.vector.ProjectOnPlane(in _ellipseDir, in axis, out _ellipseDir);
            _ellipseDir = _ellipseDir.normalized;
            _func = func;
            _rotation = new AngleAxisData((float)angle, axis);
            _correction = Quaternion.identity;
            _correctAxis = false;
            if (correctAxis && ellipseFactor > 0.01)
            {
                _correctAxis = true;
                Vector3 cand;
                var factor = GetFactorAndCandidateByProgress(0, out cand);
                var newAxis = slerp(_rotation.Axis, _vector, 1 - factor);
                _correction = Quaternion.FromToRotation(_rotation.Axis, newAxis);
            }
        }
        public Func<double, double> Func
        {
            get { return _func; }
            set { _func = value; }
        }
        public PathType Type { get { return PathType.SphericalAnglePath; } }
        public Vector3 GetValueByProgress(double progress)
        {
            Vector3 candidate;
            var factor = GetFactorAndCandidateByProgress(progress, out candidate);
            var vec = slerp(_rotation.Axis, candidate, factor);
            return _correctAxis ? _correction*vec : vec;
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new System.NotImplementedException(); }

        float GetFactorAndCandidateByProgress(double progress, out Vector3 candidate)
        {
            if (_func != null) progress = _func(progress);
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            candidate = _rotation.Slerp(progress.Clamp01())*_vector;
            var candidateUnit = candidate.normalized;
            var axis = _rotation.Axis;
            fun.vector.ProjectOnPlane(in candidateUnit, in axis, out var candidateUnitOnPlane);
            candidateUnitOnPlane = candidateUnitOnPlane.normalized;
            var angle = fun.angle.BetweenVectorsSignedInDegrees(in candidateUnitOnPlane, in _ellipseDir, in axis);
            return fun.ellipse.RadiusByAngle(1f, 1f - _ellipseFactor, angle);
        }
    }
}