using System;
using Unianio.Enums;
using Unianio.Moves;
using UnityEngine;

namespace Unianio.Moves
{
    public class CubicBezierMove : IVectorByProgress
    {
        readonly Func<CubicBezierMove, Vector3> _from, _to, _control1, _control2;
        readonly Func<double, double> _func;
        public CubicBezierMove(Vector3 from, Vector3 control1, Vector3 control2, Vector3 to,
            Func<double, double> func = null) : this(c => from, c => control1, c => control2, c => to, func)
        {
        }
        public CubicBezierMove(
            Func<CubicBezierMove, Vector3> from, 
            Func<CubicBezierMove, Vector3> control1, 
            Func<CubicBezierMove, Vector3> control2,
            Func<CubicBezierMove, Vector3> to, Func<double, double> func = null)
        {
            _from = from ?? throw new ArgumentException("LineMove requires 'from' function");
            _control1 = control1 ?? throw new ArgumentException("LineMove requires 'control1' function");
            _control2 = control2 ?? throw new ArgumentException("LineMove requires 'control2' function");
            _to = to ?? throw new ArgumentException("LineMove requires 'to' function");
            _func = func;
        }
        public MoveType Type => MoveType.CubicBezier;
        public float X { get; private set; }
        public Vector3 From { get; private set; }
        public Vector3 Control1 { get; private set; }
        public Vector3 Control2 { get; private set; }
        public Vector3 To { get; private set; }
        public Vector3 GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            X = (float)progress;
            From = _from(this);
            To = _to(this);
            Control1 = _control1(this);
            Control2 = _control2(this);
            return BezierFunc.GetPointCubic(X, From, Control1, Control2, To);
        }
    }
}