using System;
using Unianio.Enums;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Graphs
{
    public class CirclePath : IVectorByProgress
    {
        Vector3 _point, _center, _normal;
        readonly float _degrees;

        public CirclePath(Vector3 point, Vector3 center, Vector3 normal, double degrees)
        {
            _point = point;
            _center = center;
            _degrees = (float)degrees;
            var radial = (point - center).normalized;
            _normal = fun.vector.ProjectOnPlane(normal, radial);
        }
        public PathType Type { get { return PathType.CirclePath; } }
        public Vector3 GetValueByProgress(double progress)
        {
            return fun.rotate.PointAbout(in _point, in _center, in _normal, _degrees*((float) progress));
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new System.NotImplementedException(); }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}