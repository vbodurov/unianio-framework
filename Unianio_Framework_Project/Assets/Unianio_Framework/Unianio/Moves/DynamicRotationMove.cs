using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class DynamicRotationMove : IQuaternionByProgress
    {
        readonly Func<float, Quaternion> _func;
        public DynamicRotationMove(Func<float, Quaternion> func)
        {
            _func = func ?? throw new ArgumentException("DynamicRotationMove requires function");
        }
        public MoveType Type => MoveType.Rotate;
        public Quaternion GetValueByProgress(double progress)
        {
            return _func((float)progress);
        }
    }
}