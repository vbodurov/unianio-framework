using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public interface IScalarByProgress
    {
        PathType Type { get; }
        // progress is from 0 to 1
        float GetValueByProgress(double progress);
    }
}