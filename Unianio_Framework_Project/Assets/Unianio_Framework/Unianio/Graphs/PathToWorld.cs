using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class PathToWorld : IVectorByProgress
    {
        readonly Transform _t;
        readonly IVectorByProgress _child;

        public PathToWorld(Transform t, IVectorByProgress child)
        {
            _t = t;
            _child = child;
        }
        public PathType Type { get { return _child.Type; } }
        public Func<double, double> Func
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public Vector3 GetValueByProgress(double progress)
        {
            var v = _child.GetValueByProgress(progress);
            return _t.TransformPoint(v);
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return _child.GetDirectionByProgress(progress);
        }
    }
}