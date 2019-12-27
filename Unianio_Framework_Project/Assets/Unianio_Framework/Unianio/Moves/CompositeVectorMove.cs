using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class CompositeVectorMove : BaseCompositeMove<IVectorByProgressInRange>, IVectorByProgress
    {
        readonly IVectorByProgressInRange[] _subPaths;
        IVectorByProgressInRange _last;

        public CompositeVectorMove(params IVectorByProgressInRange[] subPaths)
        {
            ValidateChildren(subPaths);
            _subPaths = subPaths;
            _last = subPaths[0];
        }
        public MoveType Type => _subPaths[0].Type;
        public Vector3 GetValueByProgress(double progress)
        {
            var current = GetCurrent(ref _last, _subPaths, progress);
            var x = RatioBetween(progress, current.From, current.To);
            return current.GetValueByProgress(x);
        }
    }
}