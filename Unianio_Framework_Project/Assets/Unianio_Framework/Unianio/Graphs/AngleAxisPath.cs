using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public class AngleAxisPath : IVectorByProgress
    {
        readonly Vector3 _vector;
        readonly AngleAxisData _rotation;
        Func<double, double> _func;

        public AngleAxisPath(Vector3 vector, double degrees, Vector3 axis) : this (vector, degrees, axis, null) { }
        public AngleAxisPath(Vector3 vector, double degrees, Vector3 axis, Func<double,double> func)
        {
            _vector = vector;
            _func = func;
            _rotation = new AngleAxisData((float)degrees, axis);
        }
        public PathType Type => PathType.SphericalAnglePath;
        public Vector3 GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            return _rotation.Slerp(progress)*_vector;
        }
        public Func<double, double> Func
        {
            get => _func;
            set => _func = value;
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new System.NotImplementedException(); }
    }
}