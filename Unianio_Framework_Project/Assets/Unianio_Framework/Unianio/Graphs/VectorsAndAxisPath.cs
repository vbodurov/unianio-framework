using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Graphs;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Graphs
{
    public class VectorsAndAxisPath : IVectorByProgress
    {
        readonly Func<double,double> _rotationFunc;
        readonly Func<double,double,double,double> _fromAxisFunc;
        readonly Vector3 _fromVector;
        
        public VectorsAndAxisPath(Vector3 fromVector, Vector3 toVector, Vector3 axis) : this(fromVector, toVector, axis, false) { }
        public VectorsAndAxisPath(Vector3 fromVector, Vector3 toVector, Vector3 axis, bool takeLongestPath) : this (fromVector, toVector, axis, takeLongestPath, null, null) { }
        /// <param name="fromVector">initial vector</param>
        /// <param name="toVector">target vctor</param>
        /// <param name="axis">rotation axis</param>
        /// <param name="takeLongestPath">if true rotates by longest</param>
        /// <param name="rotationFunc">rotation function (progress01 => progress01)</param>
        /// <param name="fromAxisFunc">distance from axis function (from_degrees,to_degrees,progress => degrees)</param>
        public VectorsAndAxisPath(Vector3 fromVector, Vector3 toVector, Vector3 axis, bool takeLongestPath, Func<double,double> rotationFunc, Func<double,double,double,double> fromAxisFunc)
        {
            fromVector.Normalize();
            toVector.Normalize();
            axis.Normalize();
            var q1 = Quaternion.LookRotation(fromVector, axis);
            var q2 = Quaternion.LookRotation(toVector, axis);
            var sign = fun.angle.BetweenVectorsSignedInDegrees(in fromVector, in toVector, in axis) < 0 ? -1 : 1;
            _fromVector = fromVector;
            FromDegrees = fun.angle.BetweenVectorsUnSignedInDegrees(in fromVector, in axis);
            ToDegrees = fun.angle.BetweenVectorsUnSignedInDegrees(in toVector, in axis);
            _rotationFunc = rotationFunc;
            _fromAxisFunc = fromAxisFunc;
            Degrees = fun.angle.Between(in q1, in q2)*sign;
            if (takeLongestPath)
            {
                Degrees = (360 - Degrees)*-1;
            }
            Axis = axis;
        }
        public float Degrees;
        public Vector3 Axis;
        public float FromDegrees;
        public float ToDegrees;
        public PathType Type { get { return PathType.SphericalAnglePath; } }
        public Vector3 GetValueByProgress(double progress)
        {
            var rotateProgress =  _rotationFunc?.Invoke(progress) ?? progress;
            var candidate = Quaternion.AngleAxis(Degrees*(float) rotateProgress, Axis)*_fromVector;
            var degrees = 
                _fromAxisFunc?.Invoke(FromDegrees, ToDegrees, progress) ?? lerp(FromDegrees, ToDegrees, progress);
            return Axis.RotateTowards(in candidate, degrees);

        }
        public Vector3 GetDirectionByProgress(double progress) { throw new System.NotImplementedException(); }
        private static float lerp(double a, double b, double t)
        {
            return (float)(a + (b - a) * t.Clamp01());
        }
        public Func<double, double> Func
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}