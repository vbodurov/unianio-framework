using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Graphs
{
    public class LinearPath : IStartEndVectorByProgress
    {
        Vector3 _start, _end;
        Func<double, double> _func;

        public LinearPath() : this(v3.zero, v3.zero, null) { }
        public LinearPath(Vector3 start, Vector3 end) : this(start, end, null) { } 
        public LinearPath(Vector3 start, Vector3 end, Func<double,double> f)
        {
            _start = start;
            _end = end;
            _func = f;
        }
        public PathType Type => PathType.Line;

        public Vector3 Start
        {
            get => _start;
            set => _start = value;
        }
        public Vector3 End
        {
            get => _end;
            set => _end = value;
        }
        public Func<double,double> Func
        {
            get => _func;
            set => _func = value;
        }
        public Vector3 GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            fun.point.Lerp(in _start, in _end, progress, out var result);
            return result;
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return (_end - _start).normalized;
        }
        public LinearPath SetFunction(Func<double,double> function)
        {
            _func = function;
            return this;
        }
    }
}