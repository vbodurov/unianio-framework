using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
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
        public MoveType Type => _source.Type;
        public Vector3 GetValueByProgress(double progress) { return _source.GetValueByProgress(progress); }
        public float From => (float)_from;
        public float To => (float)_to;
    }
}