using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class SingleQuaternion : IQuaternionByProgress
    {
        public SingleQuaternion() : this(Quaternion.identity) { }
        public SingleQuaternion(Quaternion point)
        {
            Rotation = point;
        }
        public Quaternion Rotation { get; set; }
        public PathType Type => PathType.SingleValue;

        public Quaternion GetValueByProgress(double progress)
        {
            return Rotation;
        }
    }
}