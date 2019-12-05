using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class vector
        {
            public static void RestrictWithinPlane(in Vector3 dir, in Vector3 planeDir, out Vector3 restrictedDir)
            {
                if (IsBelowPlane(in dir, in planeDir))
                {
                    ProjectOnPlane(in dir, in planeDir, out restrictedDir);
                    restrictedDir.Normalize();
                }
                else
                {
                    restrictedDir = dir;
                }
            }
            public static double RestrictMaxAngleFrom(in Vector3 dir, in Vector3 axisDir, double maxDegrees, out Vector3 restrictedDir)
            {
                var deg = fun.angle.BetweenVectorsUnSignedInDegrees(in dir, in axisDir);
                restrictedDir = deg > maxDegrees ? axisDir.RotateTowards(dir, maxDegrees) : dir;
                return deg;
            }
            public static void ComputeLinkedJoinsUp(in Vector3 frontJoinFw, in Vector3 frontJoinUp, in Vector3 backJoinFw, out Vector3 backJoinUp)
            {
                var dpWithUp = dot(in frontJoinUp, in backJoinFw).ClampMin11();
                var handUpRef =
                    dpWithUp < 0
                    ? Vector3.Slerp(frontJoinUp, frontJoinFw, dpWithUp * -1)
                    : Vector3.Slerp(frontJoinUp, -frontJoinFw, dpWithUp);
                ProjectOnPlane(in handUpRef, in backJoinFw, out backJoinUp);
                backJoinUp.Normalize();
                backJoinUp = backJoinFw.GetRealUp(backJoinUp);
            }

            public static Vector3 Slerp(Vector3 a, double t, Vector3 b)
            {
                return Vector3.SlerpUnclamped(a, b, (float)t);
            }
            public static Vector3 Slerp(Vector3 a, Vector3 b, double t)
            {
                return Vector3.SlerpUnclamped(a, b, (float)t);
            }
            public static Vector3 Slerp(Vector3 a, double ab, Vector3 b, double abc, Vector3 c)
            {
                return Vector3.SlerpUnclamped(Vector3.SlerpUnclamped(a, b, (float)ab), c, (float)abc);
            }
            public static Vector3 Slerp(in Vector3 a, in Vector3 b, double t)
            {
                return Vector3.SlerpUnclamped(a, b, (float)t);
            }
            public static void Slerp(in Vector3 a, in Vector3 b, double t, out Vector3 d)
            {
                d = Vector3.SlerpUnclamped(a, b, (float)t);
            }
            public static Vector3 Slerp(in Vector3 a, double ab, in Vector3 b, double abc, in Vector3 c)
            {
                return Vector3.SlerpUnclamped(Vector3.SlerpUnclamped(a, b, (float)ab), c, (float)abc);
            }
            public static float LengthOnNormal(in Vector3 vectorToPlane, in Vector3 planeNormal)
            {
                var distance = dot(in planeNormal, in vectorToPlane);
                return abs(distance);
            }
            public static void ToLocal(in Vector3 worldVector, in Quaternion worldRotation, out Vector3 localVector)
            {
                localVector = Quaternion.Inverse(worldRotation) * worldVector;
            }
            public static Vector3 ToLocal(in Vector3 worldVector, in Quaternion worldRotation)
            {
                return Quaternion.Inverse(worldRotation) * worldVector;
            }
            public static Vector3 ToLocal(Vector3 worldVector, Quaternion worldRotation)
            {
                return Quaternion.Inverse(worldRotation) * worldVector;
            }


            public static void ToWorld(in Vector3 localVector, in Quaternion worldRotation, out Vector3 worldPoint)
            {
                worldPoint = worldRotation * localVector;
            }
            public static Vector3 ToWorld(in Vector3 localVector, in Quaternion worldRotation)
            {
                return worldRotation * localVector;
            }
            public static Vector3 ToWorld(Vector3 localVector, Quaternion worldRotation)
            {
                return worldRotation * localVector;
            }


            public static bool IsAbovePlane(in Vector3 vectorToPlane, in Vector3 planeNormal)
            {
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance < 0;
            }
            public static bool IsAbovePlane(Vector3 vectorToPlane, Vector3 planeNormal)
            {
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance < 0;
            }
            public static bool IsBelowPlane(in Vector3 vectorToPlane, in Vector3 planeNormal)
            {
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance >= 0;
            }
            public static bool IsBelowPlane(Vector3 vectorToPlane, Vector3 planeNormal)
            {
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance >= 0;
            }
            public static float ProjectionLength(in Vector3 ofVector, in Vector3 ontoVector)
            {
                var unitOntoVector = ontoVector.normalized;
                return dot(in ofVector, in unitOntoVector);
            }
            public static float ProjectionLength(Vector3 ofVector, Vector3 ontoVector)
            {
                var unitOntoVector = ontoVector.normalized;
                return dot(in ofVector, in unitOntoVector);
            }
            public static Vector3 ProjectOnNormal(Vector3 vector, Vector3 onNormal)
            {
                var dotProduct = (float)((double)onNormal.x * (double)onNormal.x + (double)onNormal.y * (double)onNormal.y + (double)onNormal.z * (double)onNormal.z); ;
                if ((double)dotProduct < (double)Mathf.Epsilon)
                    return Vector3.zero;
                return onNormal * Vector3.Dot(vector, onNormal) / (float)dotProduct;
            }
            public static Vector3 ProjectOnNormal(in Vector3 vector, in Vector3 onNormal)
            {
                var dotProduct = (float)((double)onNormal.x * (double)onNormal.x + (double)onNormal.y * (double)onNormal.y + (double)onNormal.z * (double)onNormal.z); ;
                if ((double)dotProduct < (double)Mathf.Epsilon)
                    return Vector3.zero;
                return onNormal * Vector3.Dot(vector, onNormal) / (float)dotProduct;
            }
            public static void ProjectOnNormal(in Vector3 vector, in Vector3 onNormal, out Vector3 projection)
            {
                var dotProduct = (float)((double)onNormal.x * (double)onNormal.x + (double)onNormal.y * (double)onNormal.y + (double)onNormal.z * (double)onNormal.z); ;
                if ((double)dotProduct < (double)Mathf.Epsilon)
                    projection = Vector3.zero;
                else
                    projection = onNormal * Vector3.Dot(vector, onNormal) / (float)dotProduct;
            }
            public static Vector3 ProjectOnPlane(in Vector3 vector, in Vector3 planeNormal)
            {
                var normPlaneNormal = planeNormal.normalized;
                var distance = -dot(in normPlaneNormal, in vector);
                return vector + normPlaneNormal * distance;
            }
            public static void ProjectOnPlane(in Vector3 vector, in Vector3 planeNormal, out Vector3 projection)
            {
                var normPlaneNormal = planeNormal.normalized;
                var distance = -dot(in normPlaneNormal, in vector);
                projection = vector + normPlaneNormal * distance;
            }
            public static Vector3 ReflectOfPlane(in Vector3 vector, in Vector3 planeNormal)
            {
                Vector3 projection;
                ProjectOnPlane(in vector, in planeNormal, out projection);
                projection.Normalize();
                return Vector3.SlerpUnclamped(vector, projection.normalized, 2);
            }
            public static void ReflectOfPlane(in Vector3 vector, in Vector3 planeNormal, out Vector3 reflection)
            {
                Vector3 projection;
                ProjectOnPlane(in vector, in planeNormal, out projection);
                projection.Normalize();
                reflection = Vector3.SlerpUnclamped(vector, projection.normalized, 2);
            }
            public static Vector3 GetNormal(in Vector3 lhs, in Vector3 rhs)
            {
                var normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = new Vector3(0, 0, 0);
                return normal;
            }
            public static Vector3 GetNormalOrDefault(in Vector3 lhs, in Vector3 rhs, in Vector3 normalIfOnLine)
            {
                var normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = normalIfOnLine;
                return normal;
            }
            public static void GetNormalOrDefault(in Vector3 lhs, in Vector3 rhs, in Vector3 normalIfOnLine, out Vector3 normal)
            {
                normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = normalIfOnLine;
            }
            public static void GetNormal(in Vector3 lhs, in Vector3 rhs, out Vector3 normal)
            {
                normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = new Vector3(0, 0, 0);
            }
            public static bool TryGetNormal(in Vector3 lhs, in Vector3 rhs, out Vector3 normal)
            {
                normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len <= 9.99999974737875E-06)
                {
                    normal = Vector3.zero; ;
                    return false;
                }
                normal = normal / (float)len;
                return true;
            }
            public static void GetNormalWithAltRhs(in Vector3 lhs, in Vector3 mainRhs, in Vector3 altRhs, out Vector3 normal)
            {
                var dp = dot(in lhs, in mainRhs).Clamp(-1.0, 1.0);
                var rhs =
                    dp < 0
                    ? Vector3.Slerp(mainRhs, -altRhs, dp * -1)
                    : Vector3.Slerp(mainRhs, altRhs, dp);

                normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = new Vector3(0, 0, 0);
            }

            public static void GetNormal(in Vector3 lhs, in Vector3 rhs, in Vector3 normalIfOnLine, out Vector3 normal)
            {
                normal = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));

                // normalize:
                var len = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                if (len > 9.99999974737875E-06)
                    normal = normal / (float)len;
                else
                    normal = normalIfOnLine;
            }

            public static void GetRealUp(in Vector3 forward, in Vector3 rawUp, out Vector3 realUp)
            {
                Vector3 right;
                cross.Product(in rawUp, in forward, out right);
                right.Normalize();
                cross.Product(in forward, in right, out realUp);
                realUp.Normalize();
            }
            public static void GetRealUp(in Vector3 forward, in Vector3 originalUp, in Vector3 originalFw, out Vector3 realUp)
            {
                var dpWithFw = dot(in forward, in originalFw);
                var originalFw2 = originalFw;
                var originalUp2 = originalUp;
                if (dpWithFw < 0)
                {
                    originalFw2 *= -1;
                    originalUp2 *= -1;
                }

                var dpWithUp = dot(in forward, in originalUp2).Clamp(-1.0, 1.0);
                var rawUp =
                    dpWithUp < 0
                    ? Vector3.Slerp(originalUp2, originalFw2, dpWithUp * -1)
                    : Vector3.Slerp(originalUp2, -originalFw2, dpWithUp);

                Vector3 right;
                cross.Product(in rawUp, in forward, out right);
                right.Normalize();
                cross.Product(in forward, in right, out realUp);
                realUp.Normalize();
            }
            public static Vector3 GetRealUp(Vector3 forward, Vector3 rawUp)
            {
                Vector3 right, realUp;
                cross.Product(in rawUp, in forward, out right);
                right.Normalize();
                cross.Product(in forward, in right, out realUp);
                return realUp.normalized;
            }

            public static bool PointSameDirection(in Vector3 a, in Vector3 b)
            {
                return dot(in a, in b) > 0;
            }
            public static bool PointSameDirection(Vector3 a, Vector3 b)
            {
                return dot(in a, in b) > 0;
            }
            public static bool PointSameDirection2D(in Vector2 a, in Vector2 b)
            {
                return dot2D(in a, in b) > 0;
            }

            public static bool PointDifferentDirection(in Vector3 a, in Vector3 b)
            {
                return dot(in a, in b) <= 0;
            }
            public static bool PointDifferentDirection(Vector3 a, Vector3 b)
            {
                return dot(in a, in b) <= 0;
            }
            public static bool PointDifferentDirection2D(in Vector2 a, in Vector2 b)
            {
                return dot2D(in a, in b) <= 0;
            }

            public static void ComputeRandomXYAxesForPlane(in Vector3 planeNormal, out Vector3 normX, out Vector3 normY)
            {
                var fw = Vector3.right;
                cross.Product(in planeNormal, in fw, out normX);
                if (normX.sqrMagnitude < 0.001)
                {
                    var rt = Vector3.forward;
                    cross.Product(in planeNormal, in rt, out normX);
                }
                normX.Normalize();
                GetNormal(in planeNormal, in normX, out normY);
            }

            public static void EnsurePointSameDirAs(in Vector3 direction, in Vector3 mustBeCodirectionalTo, out Vector3 sameDirection)
            {
                if (PointSameDirection(in direction, in mustBeCodirectionalTo))
                {
                    sameDirection = direction;
                }
                else
                {
                    sameDirection = -direction;
                }
            }
            public static void EnsurePointDifferentDirThan(in Vector3 direction, in Vector3 mustNotBeCodirectionalTo, out Vector3 sameDirection)
            {
                if (PointSameDirection(in direction, in mustNotBeCodirectionalTo))
                {
                    sameDirection = -direction;
                }
                else
                {
                    sameDirection = direction;
                }
            }
            public static bool IsCloserToFirst(in Vector3 normalizedTargetDir, in Vector3 normalizedFirst, in Vector3 normalizedSecond)
            {
                var dp1 = dot(in normalizedTargetDir, in normalizedFirst);
                var dp2 = dot(in normalizedTargetDir, in normalizedSecond);
                return dp1 > dp2;
            }
            public static bool IsWithinCircleFormedBy(in Vector3 normalInQuestion, in Vector3 normalBoundary1, in Vector3 normalBoundary2)
            {
                var normalCenter = Vector3.Slerp(normalBoundary1, normalBoundary2, 0.5f);
                var dpMaxDiff = dot(in normalCenter, in normalBoundary1);
                var dpActDiff = dot(in normalCenter, in normalInQuestion);
                return dpActDiff >= dpMaxDiff;// remember dot product is oposit to angle, higher means closer lower further apart
            }
            public static float HorzMagnitude(in Vector3 a)
            {
                return (float)Math.Sqrt(((double)a.x * (double)a.x + (double)a.z * (double)a.z));
            }
            public static float Magnitude(in Vector3 a)
            {
                return (float)Math.Sqrt(((double)a.x * (double)a.x + (double)a.y * (double)a.y + (double)a.z * (double)a.z));
            }
            public static double MagnitudeAsDouble(in Vector3 a)
            {
                return Math.Sqrt(((double)a.x * (double)a.x + (double)a.y * (double)a.y + (double)a.z * (double)a.z));
            }
            public static float SqrMagnitude(in Vector3 a)
            {
                return (float)((double)a.x * (double)a.x + (double)a.y * (double)a.y + (double)a.z * (double)a.z);
            }
            public static float Magnitude2D(in Vector2 a)
            {
                return (float)Math.Sqrt((double)a.x * (double)a.x + (double)a.y * (double)a.y);
            }
            public static float SqrMagnitude2D(in Vector2 a)
            {
                return (float)((double)a.x * (double)a.x + (double)a.y * (double)a.y);
            }
            public static bool TryNormalize(in Vector3 vector, out Vector3 normal)
            {
                var mag = Math.Sqrt(((double)vector.x * (double)vector.x + (double)vector.y * (double)vector.y + (double)vector.z * (double)vector.z));
                if (mag < 0.00001)
                {
                    normal = Vector3.zero;
                    return false;
                }
                normal = vector / (float)mag;
                return true;
            }
            public static bool TryNormalize(in Vector3 vector, out float magnitude, out Vector3 normal)
            {
                var mag = Math.Sqrt(((double)vector.x * (double)vector.x + (double)vector.y * (double)vector.y + (double)vector.z * (double)vector.z));
                magnitude = (float)mag;
                if (mag < 0.000001)
                {
                    normal = Vector3.zero;
                    return false;
                }
                normal = vector / magnitude;
                return true;
            }
            public static bool TryNormalize(Vector3 vector, out Vector3 normal)
            {
                var mag = Math.Sqrt(((double)vector.x * (double)vector.x + (double)vector.y * (double)vector.y + (double)vector.z * (double)vector.z));
                if (mag < 0.00001)
                {
                    normal = Vector3.zero;
                    return false;
                }
                normal = vector / (float)mag;
                return true;
            }
            public static bool TryNormalize(Vector3 vector, out float magnitude, out Vector3 normal)
            {
                var mag = Math.Sqrt(((double)vector.x * (double)vector.x + (double)vector.y * (double)vector.y + (double)vector.z * (double)vector.z));
                magnitude = (float)mag;
                if (mag < 0.00001)
                {
                    normal = Vector3.zero;
                    return false;
                }
                normal = vector / magnitude;
                return true;
            }

            public static Vector3 ComputeAngularVelocity(in Quaternion prevRot, in Quaternion currRot)
            {
                Quaternion deltaRotation = currRot * Quaternion.Inverse(prevRot);
                float angle;
                Vector3 axis;
                deltaRotation.ToAngleAxis(out angle, out axis);
                return axis * angle * Mathf.Deg2Rad * (1.0f / fun.smoothDeltaTime);
            }
        }

    }
}