using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class WrappedRotation : IQuaternionByProgress
    {
        readonly IQuaternionByProgress _child;
        readonly Func<double, IQuaternionByProgress, Quaternion> _wrapper;
        public WrappedRotation(IQuaternionByProgress child, Func<double, IQuaternionByProgress, Quaternion> wrapper)
        {
            _child = child;
            _wrapper = wrapper;
        }

        public PathType Type => _child.Type;
        public Quaternion GetValueByProgress(double progress)
        {
            return _wrapper(progress, _child);
        }

        
    }
}