using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class QuadraticBezierMove : IVectorByProgress
    {
        readonly Func<QuadraticBezierMove, Vector3> _from, _to, _control;
        readonly Func<double, double> _func;
        public QuadraticBezierMove(Vector3 from, Vector3 control, Vector3 to, Func<double, double> func = null) 
            : this(c => from, c => control, c => to, func) { }
        public QuadraticBezierMove(
            Func<QuadraticBezierMove, Vector3> from, 
            Func<QuadraticBezierMove, Vector3> control, 
            Func<QuadraticBezierMove, Vector3> to, Func<double, double> func = null)

        {
            _from = from ?? throw new ArgumentException("LineMove requires 'from' function");
            _control = control ?? throw new ArgumentException("LineMove requires 'control' function");
            _to = to ?? throw new ArgumentException("LineMove requires 'to' function");
            _func = func;
        }
        public MoveType Type => MoveType.QuadraticBezier;
        public float X { get; private set; }
        public Vector3 From { get; private set; }
        public Vector3 Control { get; private set; }
        public Vector3 To { get; private set; }
        public Vector3 GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            X = (float) progress;
            From = _from(this);
            To = _to(this);
            Control = _control(this);
            return BezierFunc.GetPointQuadratic(X, From, Control, To);
        }
    }
}