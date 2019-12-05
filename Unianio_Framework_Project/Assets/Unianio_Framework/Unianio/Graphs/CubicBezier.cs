using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class CubicBezier : IStartEndVectorByProgress
    {
        Vector3 _start, _control1, _control2, _end;
        Func<double, double> _func;
        static readonly Func<double, double> Linear = x => x;

        public CubicBezier() : this(v3.zero, v3.zero, v3.zero, v3.zero, null) { }
        public CubicBezier(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end) : this(start, control1, control2, end, null) { }
        public CubicBezier(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, Func<double,double> f)
        {
            _start = start;
            _control1 = control1;
            _control2 = control2;
            _end = end;
            _func = f ?? Linear;
        }
        public PathType Type => PathType.CubicBezier;
        public Vector3 Start
        {
            get => _start;
            set => _start = value;
        }
        public Vector3 Control1
        {
            get => _control1;
            set => _control1 = value;
        }
        public Vector3 Control2
        {
            get => _control2;
            set => _control2 = value;
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
        public Vector3 GetValueByProgress(double progress)
        {
            return BezierFunc.GetPointCubic(_func(progress), in _start, in _control1, in _control2, in _end);
        }

        public Vector3 GetDirectionByProgress(double progress)
        {
            return BezierFunc.GetFirstDerivativeCubic(_func(progress), in _start, in _control1, in _control2, in _end);
        }
        public CubicBezier SetStart(Vector3 start)
        {
            _start = start;
            return this;
        }
        public CubicBezier SetControl1(Vector3 control1)
        {
            _control1 = control1;
            return this;
        }
        public CubicBezier SetControl2(Vector3 control2)
        {
            _control2 = control2;
            return this;
        }
        public CubicBezier SetEnd(Vector3 end)
        {
            _end = end;
            return this;
        }
        public CubicBezier SetFunction(Func<double,double> function)
        {
            _func = function;
            return this;
        }
    }

}