using System;

namespace Unianio.Graphs
{
    public sealed class NumericPath : IExecutorOfProgress
    {
        readonly Action<double> _setter;
        readonly Func<double> _getter;
        readonly Func<double, double> _func;
        IScalarByProgress _change;
        Func<bool> _canApply;

        public NumericPath(Action<double> setter, Func<double> getter, Func<double,double> func = null)
        {
            _setter = setter ?? throw new ArgumentException("Setter is required");
            _getter = getter ?? throw new ArgumentException("Getter is required");
            _func = func ?? (Func<double, double>)(n => n);
        }
        public int ID { get; set; }
        public NumericPath Clone => new NumericPath(_setter, _getter);
        public NumericPath New
        {
            get
            {
                var from = _getter();
                _change = new RangeByProgress(from, from, _func);
                _canApply = null;
                return this;
            }
        }
        public NumericPath ToTarget(double target, Func<double, double> func = null)
        {
            _change = new RangeByProgress(_getter(), target, func ?? _func);
            return this;
        }
        public NumericPath SetCondition(Func<bool> condition)
        {
            _canApply = condition;
            return this;
        }
        public void Apply(double progress)
        {
            if(_canApply != null && !_canApply()) return;
            _setter(_change.GetValueByProgress(progress));
        }
        public void Apply(double progress, Func<double, double> func)
        {
            if (_canApply != null && !_canApply()) return;
            progress = func(progress);
            _setter(_change.GetValueByProgress(progress));
        }
    }
}