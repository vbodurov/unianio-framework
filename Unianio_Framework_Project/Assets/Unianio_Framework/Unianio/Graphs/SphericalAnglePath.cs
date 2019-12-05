using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public class SphericalAnglePath : IVectorByProgress
    {
        public SphericalAnglePath() : this(v3.zero, v3.zero, null) { }
        public SphericalAnglePath(Vector3 startVector, Vector3 endVector) : this(startVector, endVector, null) { }
        public SphericalAnglePath(Vector3 startVector, Vector3 endVector, Func<double,double> func)
        {
            Start = startVector;
            End = endVector;
            Func = func;
        }
        public PathType Type => PathType.SphericalAnglePath;
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public Func<double,double> Func { get; set; }
        public Vector3 GetValueByProgress(double progress)
        {
            if (Func != null) progress = Func(progress);
            return Vector3.SlerpUnclamped(Start, End, (float)progress);
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new NotImplementedException(); }
    }
}