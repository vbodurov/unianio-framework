using Unianio.Static;
using UnityEngine;

namespace Unianio
{
    public struct AngleAxisData
    {
        public AngleAxisData(double degrees, Vector3 axis)
        {
            Degrees = (float)degrees;
            Axis = axis;
        }
        public AngleAxisData(Vector3 fromVector, Vector3 toVector, Vector3 axis) : this(fromVector, toVector, axis, false) { }
        /// <summary>
        /// if axis vector does not lie on the plane between the 2 vectors it will be projected there
        /// </summary>
        public AngleAxisData(Vector3 fromVector, Vector3 toVector, Vector3 axis, bool takeLongestPath)
        {
            fromVector.Normalize();
            toVector.Normalize();
            axis.Normalize();
            var planeNormal = (fromVector - toVector).normalized;
            fun.vector.ProjectOnPlane(in axis, in planeNormal, out axis);
            var q1 = Quaternion.LookRotation(fromVector, axis);
            var q2 = Quaternion.LookRotation(toVector, axis);
            var sign = fun.angle.BetweenVectorsSignedInDegrees(in fromVector, in toVector, in axis) < 0 ? -1 : 1;
            Degrees = fun.angle.Between(in q1, in q2)*sign;
            if (takeLongestPath)
            {
                Degrees = (360 - Degrees)*-1;
            }
            Axis = axis;
        }
        public float Degrees;
        public Vector3 Axis;
        public Quaternion Slerp(Quaternion start, double progress)
        {
            return start*Quaternion.AngleAxis(Degrees*(float) progress, Axis);
        }
        public Quaternion Slerp(double progress)
        {
            return Quaternion.AngleAxis(Degrees*(float) progress, Axis);
        }
    }
}