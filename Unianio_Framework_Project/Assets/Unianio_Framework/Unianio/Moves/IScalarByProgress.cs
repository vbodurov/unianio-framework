using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public interface IScalarByProgress
    {
        MoveType Type { get; }
        // progress is from 0 to 1
        float GetValueByProgress(double progress);
    }
}