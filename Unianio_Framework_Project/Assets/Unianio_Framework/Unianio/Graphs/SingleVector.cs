using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class SingleVector : IVectorByProgress
    {
        internal SingleVector() : this(v3.zero) { }
        internal SingleVector(Vector3 point)
        {
            Point = point;
        }
        public PathType Type => PathType.SingleValue;
        internal Vector3 Point { get; set; }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public Vector3 GetValueByProgress(double progress)
        {
            return Point;
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return Vector3.zero;
        }
        public static explicit operator SingleVector(Vector3 point)
        {
            return new SingleVector(point);
        }
    }
}