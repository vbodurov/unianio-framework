using System;
using Unianio.Moves;

namespace Unianio.Moves
{
    public sealed class NumericMover : IExecutorOfProgress
    {
        readonly Action<double> _setter;
        readonly Func<double> _getter;
        readonly Func<double, double> _func;
        IScalarByProgress _change;

        public NumericMover(Action<double> setter, Func<double> getter, Func<double,double> func = null)
        {
            _setter = setter ?? throw new ArgumentException("Setter is required");
            _getter = getter ?? throw new ArgumentException("Getter is required");
            _func = func ?? (Func<double, double>)(n => n);
        }
        public int Tag { get; set; }
        public NumericMover Clone => new NumericMover(_setter, _getter);
        public NumericMover New()
        {
            var from = _getter();
            _change = new NumberMove(from, from, _func);
            return this;
        }
        public NumericMover ToTarget(double target, Func<double, double> func = null)
        {
            _change = new NumberMove(_getter(), target, func ?? _func);
            return this;
        }
        public void Apply(double progress)
        {
            _setter(_change.GetValueByProgress(progress));
        }
        public void Apply(double progress, Func<double, double> func)
        {
            progress = func(progress);
            _setter(_change.GetValueByProgress(progress));
        }
    }
}