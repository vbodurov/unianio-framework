using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class DynamicQuadraticBezier : IVectorByProgress
    {
        Vector3 _start;
        Func<Vector3> _getControl, _getEnd;
        Func<double, double> _func;
        static Func<double, double> Linear = x => x;

        public DynamicQuadraticBezier() : this(v3.zero, () => v3.zero, () => v3.zero, null) { }
        public DynamicQuadraticBezier(Vector3 start, Func<Vector3> getControl, Func<Vector3> getEnd) : this(start, getControl, getEnd, null) { }
        public DynamicQuadraticBezier(Vector3 start, Func<Vector3> getControl, Func<Vector3> getEnd, Func<double, double> f)
        {
            _start = start;
            _getControl = getControl ?? new Func<Vector3>(() => v3.zero);
            _getEnd = getEnd ?? new Func<Vector3>(() => v3.zero);
            _func = f ?? Linear;
        }
        public PathType Type { get { return PathType.QuadraticBezier; } }
        public Vector3 Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public Func<Vector3> GetControl
        {
            get { return _getControl; }
            set { _getControl = value; }
        }
        public Func<Vector3> GetEnd
        {
            get { return _getEnd; }
            set { _getEnd = value; }
        }
        public Func<double, double> Func
        {
            get { return _func; }
            set { _func = value ?? Linear; }
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            var end = _getEnd();
            var control = _getControl();
            return BezierFunc.GetFirstDerivativeQuadratic(_func(progress), in _start, in control, in end);
        }
        public Vector3 GetValueByProgress(double progress)
        {
            var end = _getEnd();
            var control = _getControl();
            return BezierFunc.GetPointQuadratic(_func(progress), in _start, in control, in end);
        }
        public DynamicQuadraticBezier SetStart(Vector3 start)
        {
            _start = start;
            return this;
        }
        public DynamicQuadraticBezier SetFunction(Func<double, double> f)
        {
            _func = f;
            return this;
        }
    }

}
