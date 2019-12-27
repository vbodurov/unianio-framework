using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class DynamicWrappedRotationMove : IQuaternionByProgress
    {
        readonly Func<float, Quaternion, Quaternion> _func;
        readonly IQuaternionByProgress _source;
        public DynamicWrappedRotationMove(Func<float, Quaternion, Quaternion> func, IQuaternionByProgress source)
        {
            _func = func ?? throw new ArgumentException("DynamicWrappedRotationMove requires function");
            _source = source ?? throw new ArgumentException("DynamicWrappedRotationMove requires source IQuaternionByProgress"); ;
        }
        public MoveType Type => MoveType.Rotate;
        public Quaternion GetValueByProgress(double progress)
        {
            return _func((float)progress, _source.GetValueByProgress(progress));
        }
    }
}