using System;
using Unianio.Enums;
using Unianio.Extensions;

namespace Unianio.Graphs
{
    public sealed class RangeByProgress : IScalarByProgress
    {
        static readonly Func<double, double> Linear = x => x;
        readonly Func<double, double> _func;
        public RangeByProgress() : this(0,1){}
        public RangeByProgress(double from, double to) : this(from,to,null){}
        public RangeByProgress(double from, double to, Func<double,double> f)
        {
            From = from;
            To = to;
            _func = f ?? Linear;
        }
        public double From { get; private set; }
        public double To { get; private set; }
        public PathType Type { get { return PathType.Scalar; } }
        public float GetValueByProgress(double progress)
        {
            return _func(progress).From01ToRange(From, To).Float();
        }
    }
}