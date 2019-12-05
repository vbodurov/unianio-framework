using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public class SphericalAnglePathWithMovingEnd : IVectorByProgress
    {
        public SphericalAnglePathWithMovingEnd() : this(v3.zero, v3.zero, v3.zero, null) { }
        public SphericalAnglePathWithMovingEnd(Vector3 startVector, Vector3 end0Vector, Vector3 end1Vector) : this(startVector, end0Vector, end1Vector, null) { }
        public SphericalAnglePathWithMovingEnd(Vector3 startVector, Vector3 end0Vector, Vector3 end1Vector, Func<double, double> func)
        {
            Start = startVector;
            End0 = end0Vector;
            End1 = end1Vector;
            Func = func;
        }
        public PathType Type => PathType.SphericalAnglePath;

        public Vector3 Start { get; set; }

        public Vector3 End0 { get; set; }

        public Vector3 End1 { get; set; }

        public Func<double, double> Func { get; set; }

        public Vector3 GetValueByProgress(double progress)
        {
            if (Func != null) progress = Func(progress);
            var x = (float)progress.Clamp01();
            var end = Vector3.Slerp(End0, End1, x);
            return Vector3.Slerp(Start, end, x);
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new NotImplementedException(); }
    }
}