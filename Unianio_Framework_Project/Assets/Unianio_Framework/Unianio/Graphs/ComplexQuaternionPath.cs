using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class ComplexQuaternionPath : BaseComplexPath<IQuaternionByProgressInRange>, IQuaternionByProgress
    {
        readonly IQuaternionByProgressInRange[] _subPaths;
        IQuaternionByProgressInRange _last;

        public ComplexQuaternionPath(params IQuaternionByProgressInRange[] subPaths)
        {
            ValidateChildren(subPaths);
            _subPaths = subPaths;
            _last = subPaths[0];
        }
        public PathType Type => PathType.Complex;

        public Quaternion GetValueByProgress(double progress)
        {
            var current = GetCurrent(ref _last, _subPaths, progress);
            var x = RatioBetween(progress, current.From, current.To);
            return current.GetValueByProgress(x);
        }
    }
}