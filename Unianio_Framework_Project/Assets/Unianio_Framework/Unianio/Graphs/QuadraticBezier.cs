using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class QuadraticBezier : IStartEndVectorByProgress
    {
        Vector3 _start;
        Vector3 _control;
        Vector3 _end;
        Func<double, double> _func;
        static readonly Func<double, double> Linear = x => x;

        public QuadraticBezier() : this(in v3.zero, in v3.zero, in v3.zero, null) { }
        public QuadraticBezier(in Vector3 start, in Vector3 control, in Vector3 end) : this(in start, in control, in end, null) { }
        public QuadraticBezier(in Vector3 start, in Vector3 control, in Vector3 end, Func<double,double> f)
        {
            _start = start;
            _control = control;
            _end = end;
            _func = f ?? Linear;
        }
        public PathType Type => PathType.QuadraticBezier;

        public Vector3 Start
        {
            get => _start;
            set => _start = value;
        }
        public Vector3 Control
        {
            get => _control;
            set => _control = value;
        }
        public Vector3 End
        {
            get => _end;
            set => _end = value;
        }
        public Func<double,double> Func
        {
            get => _func;
            set => _func = value ?? Linear;
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return BezierFunc.GetFirstDerivativeQuadratic(_func(progress), in _start, in _control, in _end);
        }
        public Vector3 GetValueByProgress(double progress)
        {
            return BezierFunc.GetPointQuadratic(_func(progress), in _start, in _control, in _end);
        }
        public QuadraticBezier SetStart(Vector3 start)
        {
            _start = start;
            return this;
        }
        public QuadraticBezier SetControl(Vector3 control)
        {
            _control = control;
            return this;
        }
        public QuadraticBezier SetEnd(Vector3 end)
        {
            _end = end;
            return this;
        }
        public QuadraticBezier SetFunction(Func<double,double> f)
        {
            _func = f;
            return this;
        }
        public QuadraticBezier SetStart(Func<QuadraticBezier,Vector3> start)
        {
            _start = start(this);
            return this;
        }
        public QuadraticBezier SetControl(Func<QuadraticBezier,Vector3> control)
        {
            _control = control(this);
            return this;
        }
        public QuadraticBezier SetEnd(Func<QuadraticBezier,Vector3> end)
        {
            _end = end(this);
            return this;
        }
    }

}