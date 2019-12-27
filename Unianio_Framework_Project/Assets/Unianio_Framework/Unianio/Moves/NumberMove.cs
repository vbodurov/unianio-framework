using System;
using Unianio.Enums;
using Unianio.Extensions;

namespace Unianio.Moves
{
    public sealed class NumberMove : IScalarByProgress
    {
        readonly Func<NumberMove, double> _from, _to;
        readonly Func<double, double> _func;
        public NumberMove() : this(0, 1) { }
        public NumberMove(double from, double to, Func<double, double> f = null) : this(x => from, x => to, f) { }
        public NumberMove(Func<NumberMove, double> from, in Func<NumberMove, double> to, Func<double, double> f = null)
        {
            _from = from ?? throw new ArgumentException("NumberMove requires 'from' function"); ;
            _to = to ?? throw new ArgumentException("NumberMove requires 'to' function"); ;
            _func = f;
        }
        public MoveType Type => MoveType.Scalar;
        public float X { get; private set; }
        public double From { get; private set; }
        public double To { get; private set; }
        public float GetValueByProgress(double progress)
        {
            if (_func != null) progress = _func(progress);
            X = (float) progress;
            From = _from(this);
            To = _to(this);
            return X.From01ToRange(From, To);
        }
    }
}