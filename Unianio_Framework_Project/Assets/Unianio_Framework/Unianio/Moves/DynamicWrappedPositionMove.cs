using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class DynamicWrappedPositionMove : IVectorByProgress
    {
        readonly Func<float, Vector3, Vector3> _func;
        readonly IVectorByProgress _source;
        public DynamicWrappedPositionMove(Func<float, Vector3, Vector3> func, IVectorByProgress source)
        {
            _func = func ?? throw new ArgumentException("DynamicWrappedPositionMove requires function");
            _source = source ?? throw new ArgumentException("DynamicWrappedPositionMove requires source IVectorByProgress"); ;
        }
        public MoveType Type => MoveType.Rotate;
        public Vector3 GetValueByProgress(double progress)
        {
            return _func((float)progress, _source.GetValueByProgress(progress));
        }
    }
}