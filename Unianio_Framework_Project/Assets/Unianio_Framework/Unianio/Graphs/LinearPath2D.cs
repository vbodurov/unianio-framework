using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Graphs
{
    public class LinearPath2D : IVector2DByProgress
    {
        Vector2 _start, _end;
        Func<double, double> _func;

        public LinearPath2D() : this(v3.zero, v3.zero, null) { }
        public LinearPath2D(Vector2 start, Vector2 end) : this(start, end, null) { }
        public LinearPath2D(Vector2 start, Vector2 end, Func<double, double> f)
        {
            _start = start;
            _end = end;
            _func = f;
        }
        public PathType Type => PathType.Line;

        public Vector2 Start
        {
            get { return _start; }
            set { _start = value; }
        }
        public Vector2 End
        {
            get { return _end; }
            set { _end = value; }
        }
        public Func<double, double> Func
        {
            get { return _func; }
            set { _func = value; }
        }
        public Vector2 GetValueByProgress(double progress)
        {
            Vector2 result;
            if (_func != null) progress = _func(progress);
            fun.point.Lerp2D(in _start, in _end, progress.Clamp01(), out result);
            return result;
        }
        public Vector2 GetDirectionByProgress(double progress)
        {
            return (_end - _start).normalized;
        }
        public LinearPath2D SetFunction(Func<double, double> function)
        {
            _func = function;
            return this;
        }
    }
}