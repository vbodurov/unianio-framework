using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class RotateByProgress : IQuaternionByProgress
    {
        readonly Func<double, Quaternion> _getEachFrame;
        internal RotateByProgress(Func<double, Quaternion> getEachFrame)
        {
            _getEachFrame = getEachFrame;
        }
        public PathType Type => PathType.SphericalAnglePath;
        public Quaternion GetValueByProgress(double progress)
        {
            return _getEachFrame(progress);
        }
    }
}