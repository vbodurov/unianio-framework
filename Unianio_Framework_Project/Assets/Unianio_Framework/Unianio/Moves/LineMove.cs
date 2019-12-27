using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Moves
{
    public class LineMove : IVectorByProgress
    {
        readonly Func<LineMove, Vector3> _from, _to;
        readonly Func<double, double> _func;
        public LineMove(Vector3 from, Vector3 to, Func<double, double> func = null) 
            : this(l => from, l => to, func) { }
        public LineMove(Func<LineMove, Vector3> from, Func<LineMove, Vector3> to, Func<double, double> func = null)
        {
            _from = from ?? throw new ArgumentException("LineMove requires 'from' function");
            _to = to ?? throw new ArgumentException("LineMove requires 'to' function");
            _func = func;
        }
        public MoveType Type => MoveType.Rotate;
        public float X { get; private set; }
        public Vector3 From { get; private set; }
        public Vector3 To { get; private set; }
        public Vector3 GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            X = (float) progress;
            From = _from(this);
            To = _to(this);
            return Vector3.LerpUnclamped(From, To, X);
        }
    }
}