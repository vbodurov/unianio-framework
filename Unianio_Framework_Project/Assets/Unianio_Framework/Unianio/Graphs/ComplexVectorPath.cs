using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class ComplexVectorPath : BaseComplexPath<IVectorByProgressInRange>, IVectorByProgress
    {
        readonly IVectorByProgressInRange[] _subPaths;
        IVectorByProgressInRange _last;

        public ComplexVectorPath(params IVectorByProgressInRange[] subPaths)
        {
            ValidateChildren(subPaths);
            _subPaths = subPaths;
            _last = subPaths[0];
        }
        public PathType Type => PathType.Complex;
        public Vector3 GetValueByProgress(double progress)
        {
            var current = GetCurrent(ref _last, _subPaths, progress);
            var x = RatioBetween(progress, current.From, current.To);
            return current.GetValueByProgress(x);
        }
        public Vector3 GetDirectionByProgress(double progress) => throw new NotImplementedException();
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}