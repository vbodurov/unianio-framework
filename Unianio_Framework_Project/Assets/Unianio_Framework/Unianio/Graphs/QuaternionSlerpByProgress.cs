using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class QuaternionSlerpByProgress : IQuaternionByProgress
    {
        readonly Quaternion _from, _to;

        public QuaternionSlerpByProgress(Quaternion from, Quaternion to)
        {
            _from = from;
            _to = to;
        }

        public PathType Type => PathType.SphericalAnglePath;
        public Quaternion GetValueByProgress(double progress)
        {
            return Quaternion.Slerp(_from, _to, (float)progress);
        }
    }
}