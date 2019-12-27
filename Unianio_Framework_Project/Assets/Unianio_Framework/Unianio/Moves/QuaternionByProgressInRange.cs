using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public class QuaternionByProgressInRange : IQuaternionByProgressInRange
    {
        readonly double _from;
        readonly double _to;
        readonly IQuaternionByProgress _source;

        public QuaternionByProgressInRange(double from, double to, IQuaternionByProgress source)
        {
            _from = from;
            _to = to;
            _source = source;
        }
        public MoveType Type => _source.Type;
        public Quaternion GetValueByProgress(double progress) => _source.GetValueByProgress(progress);
        public float From => (float)_from;
        public float To => (float)_to;
    }
}