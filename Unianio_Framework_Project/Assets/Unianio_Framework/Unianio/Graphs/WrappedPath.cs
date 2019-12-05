using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class WrappedPath : IVectorByProgress 
    {
        readonly IVectorByProgress _child;
        readonly Func<double, IVectorByProgress, Vector3> _wrapper;
        public WrappedPath(IVectorByProgress child, Func<double, IVectorByProgress, Vector3> wrapper)
        {
            _child = child;
            _wrapper = wrapper;
        }
        public PathType Type => _child.Type;

        public Vector3 GetValueByProgress(double progress)
        {
            return _wrapper(progress, _child);
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return _child.GetDirectionByProgress(progress);
        }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}