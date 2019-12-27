using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class RotationMove : IQuaternionByProgress
    {
        readonly Func<RotationMove, Quaternion> _from, _to;
        readonly Func<double, double> _func;
        public RotationMove(Quaternion from, Quaternion to, Func<double, double> func = null) :this(x => from, x => to, func) { }
        public RotationMove(Func<RotationMove, Quaternion> from, Func<RotationMove, Quaternion> to, Func<double, double> func = null)
        {
            _from = from ?? throw new ArgumentException("RotationMove requires 'from' function");
            _to = to ?? throw new ArgumentException("RotationMove requires 'to' function");
            _func = func;
        }
        public MoveType Type => MoveType.Rotate;
        public float X { get; private set; }
        public Quaternion From { get; private set; }
        public Quaternion To { get; private set; }
        public Quaternion GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            X = (float) progress;
            From = _from(this);
            To = _to(this);
            return Quaternion.SlerpUnclamped(From, To, X);
        }
    }
}