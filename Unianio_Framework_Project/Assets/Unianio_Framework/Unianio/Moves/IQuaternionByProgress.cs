using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public interface IQuaternionByProgress
    {
        MoveType Type { get; }
        // progress is from 0 to 1
        Quaternion GetValueByProgress(double progress);
    }
    public interface IQuaternionByProgressInRange : IQuaternionByProgress, IRangeOfNumbers { }
}