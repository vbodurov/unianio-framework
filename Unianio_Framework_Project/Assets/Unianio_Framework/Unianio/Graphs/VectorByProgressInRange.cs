using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class VectorByProgressInRange : IVectorByProgressInRange
    {
        readonly double _from;
        readonly double _to;
        readonly IVectorByProgress _source;

        public VectorByProgressInRange(double from, double to, IVectorByProgress source)
        {
            _from = from;
            _to = to;
            _source = source;
        }
        public PathType Type => _source.Type;
        public Vector3 GetValueByProgress(double progress) { return _source.GetValueByProgress(progress); }
        public Vector3 GetDirectionByProgress(double progress) { return _source.GetDirectionByProgress(progress); }
        public float From => (float)_from;
        public float To => (float)_to;
        public bool IsIn(double value)
        {
            return value >= _from && value <= _to;
        }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}