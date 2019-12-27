using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class CompositeQuaternionMove : BaseCompositeMove<IQuaternionByProgressInRange>, IQuaternionByProgress
    {
        readonly IQuaternionByProgressInRange[] _subPaths;
        IQuaternionByProgressInRange _last;

        public CompositeQuaternionMove(params IQuaternionByProgressInRange[] subPaths)
        {
            ValidateChildren(subPaths);
            _subPaths = subPaths;
            _last = subPaths[0];
        }
        public MoveType Type => _subPaths[0].Type;
        public Quaternion GetValueByProgress(double progress)
        {
            var current = GetCurrent(ref _last, _subPaths, progress);
            var x = RatioBetween(progress, current.From, current.To);
            return current.GetValueByProgress(x);
        }
    }
}