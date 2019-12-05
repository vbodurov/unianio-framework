using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public interface IQuaternionByProgress
    {
        PathType Type { get; }
        // progress is from 0 to 1
        Quaternion GetValueByProgress(double progress);
    }
    public interface IQuaternionByProgressInRange : IQuaternionByProgress, IProviderInRange { }
}