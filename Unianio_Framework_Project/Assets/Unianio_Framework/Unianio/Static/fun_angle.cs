using System;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class angle
        {
            private const double TwiceRadiansToDegrees = 2.0 * 57.2957801818848;

            /// <summary>
            /// Angle between rotations in degrees
            /// </summary>
            public static float Between(Quaternion a, Quaternion b)
            {
                return (float)((double)Math.Acos(Math.Min(Math.Abs(dot(in a, in b)), 1f)) * TwiceRadiansToDegrees);
            }
            /// <summary>
            /// Angle between rotations in degrees
            /// </summary>
            public static float Between(in Quaternion a, in Quaternion b)
            {
                return (float)((double)Math.Acos(Math.Min(Math.Abs(dot(in a, in b)), 1f)) * TwiceRadiansToDegrees);
            }
            public static float BetweenVectorsUnSignedInRadians(in Vector3 from, in Vector3 to)
            {
                return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1f, 1f));
            }
            public static float BetweenVectorsUnSignedInDegrees(in Vector3 from, in Vector3 to)
            {
                return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1f, 1f)) * RTD;
            }
            public static float BetweenVectorAndPlaneUnSignedInDegrees(in Vector3 vec, in Vector3 planeNormal)
            {
                vector.ProjectOnPlane(in vec, in planeNormal, out var vecProj);
                return BetweenVectorsUnSignedInDegrees(in vec, in vecProj);
            }
            public static float BetweenVectorsSigned2D(in Vector2 lhs, in Vector2 rhs)
            {
                var perpDot = lhs.x * rhs.y - lhs.y * rhs.x;

                return -RTD * (float)Math.Atan2(perpDot, dot2D(in lhs, in rhs));
            }
            public static float BetweenVectorsSigned2D(Vector2 lhs, Vector2 rhs)
            {
                var perpDot = lhs.x * rhs.y - lhs.y * rhs.x;

                return -RTD * (float)Math.Atan2(perpDot, dot2D(in lhs, in rhs));
            }
            public static float BetweenVectorsSigned2D(double v1x, double v1y, double v2x, double v2y)
            {
                var perpDot = v1x * v2y - v1y * v2x;

                return -RTD * (float)Math.Atan2(perpDot, dot2D(v1x, v1y, v2x, v2y));
            }
            public static float ShortestBetweenVectorsSigned(in Vector3 lhs, in Vector3 rhs)
            {
                Vector3 normal;
                fun.vector.GetNormal(in lhs, in rhs, out normal);
                var x = cross.Product(in lhs, in rhs);
                return (float)Math.Atan2(dot(in normal, in x), dot(in lhs, in rhs)) * RTD;
            }
            public static float BetweenPointsSignedIgnoreY(Vector3 lhsPoint, Vector3 centerPoint, Vector3 rhsPoint)
            {
                var lhs = lhsPoint - centerPoint;
                var rhs = rhsPoint - centerPoint;
                return BetweenVectorsSigned2D(lhs.x, lhs.z, rhs.x, rhs.z);
            }
            public static float BetweenPointsSignedIgnoreY(in Vector3 lhsPoint, in Vector3 centerPoint, in Vector3 rhsPoint)
            {
                var lhs = lhsPoint - centerPoint;
                var rhs = rhsPoint - centerPoint;
                return BetweenVectorsSigned2D(lhs.x, lhs.z, rhs.x, rhs.z);
            }

            /// <summary>
            /// angle in degrees, left to right angle will be positive, right to left negative
            /// </summary>
            public static float BetweenVectorsSignedInDegrees(in Vector3 lhs, in Vector3 rhs, in Vector3 normal)
            {
                var x = fun.cross.Product(in lhs, in rhs);
                return (float)Math.Atan2(fun.dot(in normal, in x), fun.dot(in lhs, in rhs)) * RTD;
            }
            public static float BetweenPointsSignedInDegrees(in Vector3 lhsPoint, in Vector3 centerPoint, in Vector3 rhsPoint)
            {
                Vector3 normal;
                fun.point.GetNormal(in lhsPoint, in centerPoint, in rhsPoint, out normal);
                var lhs = lhsPoint - centerPoint;
                var rhs = rhsPoint - centerPoint;
                return BetweenVectorsSignedInDegrees(in lhs, in rhs, in normal);
            }
            public static float BetweenPointsSignedInDegrees(in Vector3 lhsPoint, in Vector3 centerPoint, in Vector3 rhsPoint, in Vector3 normal)
            {
                var lhs = lhsPoint - centerPoint;
                var rhs = rhsPoint - centerPoint;
                return BetweenVectorsSignedInDegrees(in lhs, in rhs, in normal);
            }
            /// <summary>
            /// signed degrees difference for each axis
            /// </summary>
            public static void DifferenceForEachAxis(in Quaternion original, in Quaternion changed, out Vector3 diffEuler)
            {
                var aFw = original * Vector3.forward;
                var aUp = original * Vector3.up;
                var aRt = original * Vector3.right;
                var bFw = changed * Vector3.forward;
                var bUp = changed * Vector3.up;
                var bRt = changed * Vector3.right;
                Vector3 bFwProj, bUpProj, bRtProj;

                vector.ProjectOnPlane(in bRt, in aUp, out bRtProj);
                vector.ProjectOnPlane(in bUp, in aFw, out bUpProj);
                vector.ProjectOnPlane(in bFw, in aRt, out bFwProj);
                var x = 0f;
                var y = 0f;
                var z = 0f;
                if (bRtProj.sqrMagnitude > 0.000001)
                {
                    bRtProj.Normalize();
                    x = BetweenVectorsSignedInDegrees(in bRtProj, in aRt, in aUp);
                }
                if (bUpProj.sqrMagnitude > 0.000001)
                {
                    bUpProj.Normalize();
                    y = BetweenVectorsSignedInDegrees(in bUpProj, in aUp, in aFw);
                }
                if (bFwProj.sqrMagnitude > 0.000001)
                {
                    bFwProj.Normalize();
                    z = BetweenVectorsSignedInDegrees(in bFwProj, in aFw, in aRt);
                }
                diffEuler = new Vector3(x, y, z);
            }
            /// <summary>
            /// returns angle in degrees
            /// </summary>
            public static float BetweenVectorsProjectedOnPlane(in Vector3 vec1, in Vector3 vec2, in Vector3 planeNormal)
            {
                Vector3 proj1, proj2;
                fun.vector.ProjectOnPlane(in vec1, in planeNormal, out proj1);
                fun.vector.ProjectOnPlane(in vec2, in planeNormal, out proj2);
                proj1.Normalize();
                proj2.Normalize();
                return fun.angle.BetweenVectorsUnSignedInDegrees(in proj1, in proj2);
            }
        }
    }
}