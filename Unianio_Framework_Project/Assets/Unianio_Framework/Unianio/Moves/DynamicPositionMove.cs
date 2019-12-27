using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class DynamicPositionMove : IVectorByProgress
    {
        readonly Func<float, Vector3> _func;
        public DynamicPositionMove(Func<float, Vector3> func)
        {
            _func = func ?? throw new ArgumentException("DynamicPositionMove requires function");
        }
        public MoveType Type => MoveType.Rotate;
        public Vector3 GetValueByProgress(double progress)
        {
            return _func((float)progress);
        }
    }
}