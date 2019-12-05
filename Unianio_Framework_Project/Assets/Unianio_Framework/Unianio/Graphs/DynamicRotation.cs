using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class DynamicRotation : IQuaternionByProgress
    {
        readonly Func<double,Quaternion> _func;
        internal DynamicRotation(Func<double,Quaternion> func)
        {
            _func = func;
        } 

        public PathType Type { get { return PathType.DynamicRotation; } }

        public Quaternion GetValueByProgress(double progress)
        {
            return _func(progress);
        }
    }
}