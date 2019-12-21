using System;
using System.Collections.Generic;
using Unianio.Rigged.IK;
//TODO:enable
//using Unianio.Rigged.IK;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class Vector3Extensions
    {
        static readonly float RTD = (float)(180 / Math.PI);
        static readonly Vector3 unitX = new Vector3(1, 0, 0);
        static readonly Vector3 unitY = new Vector3(0, 1, 0);

        public static Vector3 By(in this Vector3 vector, double multiplier) => vector * (float) multiplier;
        public static Vector3 EnsureIsWithinRangeOf(in this Vector3 target, in Vector3 pivot, double minDist, double maxDist)
        {
            var dist = fun.distance.Between(in target, in pivot);
            if (dist < minDist) return pivot.MoveTowards(in target, minDist);
            if (dist > maxDist) return pivot.MoveTowards(in target, maxDist);
            return target;
        }
        //TODO:enable
/*        public static Vector3 ProjectOnSurface(in this Vector3 point, IPhysicsManager physics, Vector3 dir, int layer)
        {
            RaycastHit rh;
            if(physics.Raycast(point, dir, float.PositiveInfinity, layer, out rh))
            {
                return rh.point;
            }
            return point;
        }*/
        public static Vector3 FlipYandZ(in this Vector3 v)
        {
            return new Vector3(v.x, v.z, v.y);
        }
        public static Vector3 SlerpTo(in this Vector3 from, in Vector3 to, double t)
        {
            return Vector3.SlerpUnclamped(from, to, (float)t);
        }
        public static Vector3 SlerpToDeg(in this Vector3 from, in Vector3 to, double t)
        {
            return Vector3.SlerpUnclamped(from, to, (float)(t/90.0));
        }
        public static Vector3 DirTo(in this Vector3 from, in Vector3 to)
        {
            return (to - from).ToUnit();
        }
        public static Vector3 DirTo(in this Vector3 from, in Vector3 to, out float distance)
        {
            return (to - from).ToUnit(out distance);
        }
        public static Vector3 DirTo(in this Vector3 from, in Vector3 to, in Vector3 dirIfZeroDist)
        {
            return (to - from).ToUnit(in dirIfZeroDist);
        }
        public static Vector3 DirTo(in this Vector3 from, in Vector3 to, in Vector3 dirIfZeroDist, out float distance)
        {
            return (to - from).ToUnit(in dirIfZeroDist, out distance);
        }

        public static Vector3 HorzDirTo(in this Vector3 from, in Vector3 to)
        {
            return (to - from).ToHorzUnit();
        }
        public static Vector3 HorzDirTo(in this Vector3 from, in Vector3 to, out float distance)
        {
            return (to - from).ToHorzUnit(out distance);
        }
        public static Vector3 HorzDirTo(in this Vector3 from, in Vector3 to, in Vector3 dirIfZeroDist)
        {
            return (to - from).ToHorzUnit(in dirIfZeroDist);
        }
        public static Vector3 HorzDirTo(in this Vector3 from, in Vector3 to, in Vector3 dirIfZeroDist, out float distance)
        {
            return (to - from).ToHorzUnit(in dirIfZeroDist, out distance);
        }



        public static Vector3 ToUnit(in this Vector3 vector)
        {
            var magnitude = fun.vector.Magnitude(in vector);
            return magnitude < 0.00001 ? Vector3.zero : vector/magnitude;
        }
        public static Vector3 ToUnit(in this Vector3 vector, out float magnitude)
        {
            magnitude = fun.vector.Magnitude(in vector);
            return magnitude < 0.00001 ? Vector3.zero : vector/magnitude;
        }
        public static Vector3 ToUnit(in this Vector3 vector, in Vector3 dirIfZero)
        {
            var mag = fun.vector.Magnitude(in vector);
            return mag < 0.00001 ? dirIfZero : vector/mag;
        }
        public static Vector3 ToUnit(in this Vector3 vector, in Vector3 dirIfZero, out float magnitude)
        {
            magnitude = fun.vector.Magnitude(in vector);
            return magnitude < 0.00001 ? dirIfZero : vector/magnitude;
        }

        public static Vector3 ProjectOnPlaneAndNormalize(in this Vector3 vector, in Vector3 planeNormal)
        {
            fun.vector.ProjectOnPlane(in vector, in planeNormal, out var outVec);
            var mag = fun.vector.Magnitude(in outVec);
            if ((double) mag > 9.99999974737875E-06) return outVec / mag;
            return Vector3.zero;
        }
        public static Vector3 NormalWith(in this Vector3 a, in Vector3 b)
        {
            return vector.GetNormal(in a, in b);
        }


        public static float[] ToArray(in this Vector3 vector) { return new []{vector.x,vector.y,vector.z}; }
        public static Vector3 GetRealUp(in this Vector3 forward, in Vector3 approximateUp)
        {
            vector.GetRealUp(in forward, in approximateUp, out var realUp);
            return realUp;
        }
        public static void GetRealUp(in this Vector3 forward, in Vector3 originalUp, in Vector3 originalFw, out Vector3 realUp)
        {
            vector.GetRealUp(in forward, in originalUp, in originalFw, out realUp);
        }
        public static Vector3 GetRealUp(in this Vector3 forward, in Vector3 originalUp, in Vector3 originalFw)
        {
            fun.vector.GetRealUp(in forward, in originalUp, in originalFw, out var realUp);
            return realUp;
        }
        public static bool IsVectorAbovePlain(in this Vector3 targetPoint, in Vector3 planeNormal)
        {
            return vector.IsAbovePlane(in targetPoint, in planeNormal);
        }
        public static bool IsVectorBelowPlain(in this Vector3 targetPoint, in Vector3 planeNormal)
        {
            return vector.IsBelowPlane(in targetPoint, in planeNormal);
        }


        public static bool IsPointInFrontOf(in this Vector3 targetPoint, IBaseManipulator manipulator)
        {
            return point.IsAbovePlane(in targetPoint, manipulator.Manipulator.forward, manipulator.Manipulator.position);
        }
        public static bool IsPointAbove(in this Vector3 targetPoint, IBaseManipulator manipulator)
        {
            return point.IsAbovePlane(in targetPoint, manipulator.Manipulator.up, manipulator.Manipulator.position);
        }
        public static bool IsPointInFrontOf(in this Vector3 targetPoint, Transform transform)
        {
            return point.IsAbovePlane(in targetPoint, transform.forward, transform.position);
        }
        public static bool IsPointAbove(in this Vector3 targetPoint, Transform transform)
        {
            return point.IsAbovePlane(in targetPoint, transform.up, transform.position);
        }
        public static bool IsPointAbovePlain(in this Vector3 targetPoint, in Vector3 planeNormal, in Vector3 planePoint)
        {
            return point.IsAbovePlane(in targetPoint, in planeNormal, in planePoint);
        }
        public static bool IsPointBelowPlain(in this Vector3 targetPoint, in Vector3 planeNormal, in Vector3 planePoint)
        {
            return point.IsBelowPlane(in targetPoint, in planeNormal, in planePoint);
        }
        public static bool IsCloserToPoint(in this Vector3 targetPoint, in Vector3 closerToPoint, in Vector3 comparedToPoint)
        {
            var d1 = fun.distanceSquared.Between(in targetPoint, in closerToPoint);
            var d2 = fun.distanceSquared.Between(in targetPoint, in comparedToPoint);
            return d1 < d2;
        }
        public static Vector3 AsNormalOr(in this Vector3 vector, in Vector3 normalIfZero)
        {
            var mag = vector.magnitude;
            if (mag < 0.00001) return normalIfZero;
            return vector/mag;
        }
        public static Vector3 AsLocalDir(in this Vector3 worldDirection, Transform obj)
        {
            return obj == null ? worldDirection : obj.InverseTransformDirection(worldDirection);
        }
        public static Vector3 AsLocalVec(in this Vector3 worldVector, Transform obj)
        {
            return obj == null ? worldVector : obj.InverseTransformVector(worldVector);
        }
        public static Vector3 AsLocalPoint(in this Vector3 worldPoint, Transform obj)
        {
            return obj == null ? worldPoint : obj.InverseTransformPoint(worldPoint);
        }

        public static Vector3 AsWorldDir(in this Vector3 localDirection, Transform obj)
        {
            return obj.TransformDirection(localDirection);
        }
        public static Vector3 AsWorldVec(in this Vector3 localVector, Transform obj)
        {
            return obj.TransformVector(localVector);
        }
        public static Vector3 AsWorldPoint(in this Vector3 localPoint, Transform obj)
        {
            return obj.TransformPoint(localPoint);
        }


        public static Vector3 AsLocalDir(in this Vector3 worldDirection, IBaseManipulator bm)
        {
            return bm.Manipulator.InverseTransformDirection(worldDirection);
        }
        public static Vector3 AsLocalVec(in this Vector3 worldVector, IBaseManipulator bm)
        {
            return bm.Manipulator.InverseTransformVector(worldVector);
        }
        public static Vector3 AsLocalPoint(in this Vector3 worldPoint, IBaseManipulator bm)
        {
            return bm.Manipulator.InverseTransformPoint(worldPoint);
        }

        public static Vector3 AsWorldDir(in this Vector3 localDirection, IBaseManipulator bm)
        {
            return bm.Manipulator.TransformDirection(localDirection);
        }
        public static Vector3 AsWorldVec(in this Vector3 localVector, IBaseManipulator bm)
        {
            return bm.Manipulator.TransformVector(localVector);
        }
        public static Vector3 AsWorldPoint(in this Vector3 localPoint, IBaseManipulator bm)
        {
            return bm.Manipulator.TransformPoint(localPoint);
        }
        public static Vector3 AsOtherLocalPoint(in this Vector3 localPoint, IBaseManipulator fromLocal, IBaseManipulator toLocal)
        {
            return toLocal.Manipulator.InverseTransformPoint(fromLocal.Manipulator.TransformPoint(localPoint));
        }
        public static Vector3 AsOtherLocalPoint(in this Vector3 localPoint, Transform fromLocal, Transform toLocal)
        {
            return toLocal.InverseTransformPoint(fromLocal.TransformPoint(localPoint));
        }
#if UNIANIO_DEBUG
        public static Vector3 AsDbg(in this Vector3 point, int id, IBaseManipulator space, Color color)
        {
            if(space != null) dbg.DrawLineTo(id, point.AsWorldPoint(space.Manipulator), color);
            else dbg.DrawLineTo(id, point, color);
            return point;
        }
#endif
        public static void AsLocalPoint(in this Vector3 worldPoint, in Quaternion worldRotation, in Vector3 worldPosition, out Vector3 localPoint)
        {
            localPoint = Quaternion.Inverse(worldRotation)*(worldPoint - worldPosition);
        }
        public static Vector3 AsLocalPoint(in this Vector3 worldPoint, in Quaternion worldRotation, in Vector3 worldPosition)
        {
            return Quaternion.Inverse(worldRotation)*(worldPoint - worldPosition);
        }

        public static Vector3 AsWorldDir(in this Vector3 localPoint, in Quaternion worldRotation)
        {
            return worldRotation * localPoint;
        }


        public static void AsWorldPoint(in this Vector3 localPoint, in Quaternion worldRotation, in Vector3 worldPosition, out Vector3 worldPoint)
        {
            worldPoint = worldRotation * localPoint + worldPosition;
        }
        public static Vector3 AsWorldPoint(in this Vector3 localPoint, in Quaternion worldRotation, in Vector3 worldPosition)
        {
            return worldRotation * localPoint + worldPosition;
        }

        public static Vector3 ProjectedOn(in this Vector3 worldVector, in Vector3 normal)
        {
            Vector3 result;
            fun.vector.ProjectOnPlane(in worldVector, in normal, out result);
            return result;
        }

//        public static Vector2 As2d(in this Vector3 v3, Vector3 normalizedX, Vector3 normalizedY)
//        {
//            return new Vector2(dot(in v3, in normalizedX),dot(in v3, in normalizedY));
//        }
        public static Vector2 As2d(in this Vector3 v3, in Vector3 normalizedX, in Vector3 normalizedY)
        {
            return new Vector2(dot(in v3, in normalizedX),dot(in v3, in normalizedY));
        }
        public static void As2d(in this Vector3 v3, in Vector3 normalizedX, in Vector3 normalizedY, out Vector2 result)
        {
            result = new Vector2(dot(in v3, in normalizedX),dot(in v3, in normalizedY));
        }

//        public static Vector2 As2d(in this Vector3 v3, Vector3 origin, Vector3 normalizedX, Vector3 normalizedY)
//        {
//            var vec = v3 - origin;
//            return new Vector2(dot(in vec, in normalizedX),dot(in vec, in normalizedY));
//        }
        public static Vector2 As2d(in this Vector3 v3, in Vector3 origin, in Vector3 normalizedX, in Vector3 normalizedY)
        {
            var vec = v3 - origin;
            return new Vector2(dot(in vec, in normalizedX),dot(in vec, in normalizedY));
        }
        public static void As2d(in this Vector3 v3, in Vector3 origin, in Vector3 normalizedX, in Vector3 normalizedY, out Vector2 result)
        {
            var vec = v3 - origin;
            result = new Vector2(dot(in vec, in normalizedX),dot(in vec, in normalizedY));
        }
        public static Vector3 Reverse(in this Vector3 dir)
        {
            return -dir;
        }
        public static Vector3 ToHorzUnit(in this Vector3 v3)
        {
            // normalize
            var mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
            if ((double) mag > 9.99999974737875E-06)
                return new Vector3(v3.x,0,v3.z)/mag;
            return Vector3.zero;
        }
//        public static Vector3 ToHorzUnit(in this Vector3 v3, Vector3 ifZero)
//        {
//            // normalize
//            var mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
//            if ((double) mag > 9.99999974737875E-06)
//                return new Vector3(v3.x, 0, v3.z) / mag;
//            return ifZero;
//        }
        public static Vector3 ToHorzUnit(in this Vector3 v3, in Vector3 ifZero)
        {
            // normalize
            var mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
            if ((double) mag > 9.99999974737875E-06)
                return new Vector3(v3.x, 0, v3.z) / mag;
            return ifZero;
        }
//        public static Vector3 ToHorzUnit(in this Vector3 v3, Vector3 ifZero, out float mag)
//        {
//            // normalize
//            mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
//            if ((double) mag > 9.99999974737875E-06)
//                return new Vector3(v3.x, 0, v3.z) / mag;
//            return ifZero;
//        }
        public static Vector3 ToHorzUnit(in this Vector3 v3, in Vector3 ifZero, out float mag)
        {
            // normalize
            mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
            if ((double) mag > 9.99999974737875E-06)
                return new Vector3(v3.x, 0, v3.z) / mag;
            return ifZero;
        }
        public static Vector3 ToHorzUnit(in this Vector3 v3, out float mag)
        {
            // normalize
            mag = (float)Math.Sqrt((float) ((double) v3.x * (double) v3.x + (double) v3.z * (double) v3.z));
            if ((double) mag > 9.99999974737875E-06)
                return new Vector3(v3.x, 0, v3.z) / mag;
            return Vector3.zero;
        }

        public static Vector3 Negate(in this Vector3 vec) => -vec;
        public static Vector3 MidWayTo(in this Vector3 a, in Vector3 b)
        {
            return fun.point.Lerp(in a, in b, 0.5);
        }

        public static Vector3 GoAbsTo(in this Vector3 current, in Vector3 target, double meters)
        {
            var vector3 = target - current;
            var magnitude = vector3.magnitude;
            return current + (vector3 / magnitude) * (float)meters;
        }
        public static Vector3 GoRelTo(in this Vector3 current, in Vector3 target, double ratio)
        {
            return lerp(in current, in target, ratio);
        }
        public static Vector3 RotDir(in this Vector3 current, in Vector3 dir, double degrees) => current.RotateTowardsCanOvershoot(in dir, degrees);
        public static Vector3 RotRt(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.rt, degrees);
        public static Vector3 RotLt(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.lt, degrees);
        public static Vector3 RotUp(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.up, degrees);
        public static Vector3 RotDn(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.dn, degrees);
        public static Vector3 RotFw(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.fw, degrees);
        public static Vector3 RotBk(in this Vector3 current, double degrees) => current.RotateTowardsCanOvershoot(in v3.bk, degrees);

        public static Vector3 AddVec(in this Vector3 current, in Vector3 dir, double meters) => current + dir.By(meters);
        public static Vector3 AddRt(in this Vector3 current, double meters) => new Vector3(current.x + (float)meters, current.y, current.z);
        public static Vector3 AddLt(in this Vector3 current, double meters) => new Vector3(current.x - (float)meters, current.y, current.z);
        public static Vector3 AddUp(in this Vector3 current, double meters) => new Vector3(current.x, current.y + (float)meters, current.z);
        public static Vector3 AddDn(in this Vector3 current, double meters) => new Vector3(current.x, current.y - (float)meters, current.z);
        public static Vector3 AddFw(in this Vector3 current, double meters) => new Vector3(current.x, current.y, current.z + (float)meters);
        public static Vector3 AddBk(in this Vector3 current, double meters) => new Vector3(current.x, current.y, current.z - (float)meters);

        public static Vector3 MoveTowards(in this Vector3 current, in Vector3 target, double maxDistanceDelta)
        {
            var vector3 = target - current;
            var unit = vector3.ToUnit(out float magnitude);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if ((double) magnitude <= maxDistanceDelta || (double) magnitude < 0.000001)
                return target;
            return current + unit * (float)maxDistanceDelta;
        }
        public static Vector3 MoveTowardsCanOvershoot(in this Vector3 current, in Vector3 target, double maxDistanceDelta)
        {
            var vector3 = target - current;
            var magnitude = vector3.magnitude;
            if ((double) magnitude < 0.000001)
            {
                vector3 = Vector3.forward;
                magnitude = 1;
            }
            return current + (vector3 / magnitude) * (float)maxDistanceDelta;
        }
        public static Vector3 RotateTowards(in this Vector3 current, in Vector3 target, double maxDegreesDelta, double maxMagnitudeDelta)
        {
            return Vector3.RotateTowards(current, target, (float)maxDegreesDelta*fun.DTR, (float)maxMagnitudeDelta);
        }
        public static Vector3 RotateTowards(in this Vector3 current, in Vector3 target, double maxDegreesDelta)
        {
            return Vector3.RotateTowards(current, target, (float)maxDegreesDelta*fun.DTR, 0.0f);
        }
        public static Vector3 ReflectOfPlane(in this Vector3 current, in Vector3 planeNormal)
        {
            return vector.ReflectOfPlane(in current, in planeNormal);
        }
        public static Vector3 RotateTowardsCanOvershoot(in this Vector3 current, in Vector3 target, double degrees)
        {
            Vector3 normal;
            fun.vector.GetNormal(in current, in target, out normal);
            Vector3 result;
            fun.rotate.Vector(in current, in normal, degrees, out result);
            return result;
        }
        public static void RotateTowardsCanOvershoot(in this Vector3 current, in Vector3 target, double degrees, out Vector3 result)
        {
            Vector3 normal;
            fun.vector.GetNormal(in current, in target, out normal);
            fun.rotate.Vector(in current, in normal, degrees, out result);
        }

        public static bool IsNan(in this Vector3 v)
        {
            return float.IsNaN(v.x);
        }
        public static bool IsNotNan(in this Vector3 v)
        {
            return !float.IsNaN(v.x);
        }
        public static bool IsZero(in this Vector3 v)
        {
            return v.x.IsZero() && v.y.IsZero() && v.z.IsZero();
        }
        public static bool IsNotZero(in this Vector3 v)
        {
            return !v.x.IsZero() || !v.y.IsZero() || v.z.IsZero();
        }

        public static bool IsEqual(in this Vector3 a, in Vector3 b)
        {
            return
                ((double)a.x).IsEqual((double)b.x) &&
                ((double)a.y).IsEqual((double)b.y) &&
                ((double)a.z).IsEqual((double)b.z);
        }
        public static bool IsEqual(in this Vector3 a, in Vector3 b, double delta)
        {
            return
                ((double)a.x).IsEqual((double)b.x, delta) &&
                ((double)a.y).IsEqual((double)b.y, delta) &&
                ((double)a.z).IsEqual((double)b.z, delta);
        }
        public static bool IsNotEqual(in this Vector3 a, in Vector3 b)
        {
            return
                ((double)a.x).IsNotEqual((double)b.x) ||
                ((double)a.y).IsNotEqual((double)b.y) ||
                ((double)a.z).IsNotEqual((double)b.z);
        }
        public static bool IsNotEqual(in this Vector3 a, in Vector3 b, double delta)
        {
            return
                ((double)a.x).IsNotEqual((double)b.x, delta) ||
                ((double)a.y).IsNotEqual((double)b.y, delta) ||
                ((double)a.z).IsNotEqual((double)b.z, delta);
        }


        public static string s(in this Vector3 v)
        {
            return v.x + "f," + v.y + "f," + v.z+"f";
        }
        public static string s(in this Vector3? v)
        {
            if (!v.HasValue) return "null";
            return v.Value.x + "f," + v.Value.y + "f," + v.Value.z + "f";
        }
        public static string s(in this Vector3 v, int digits)
        {
            return Math.Round(v.x, digits).s() + "," + Math.Round(v.y, digits).s() + "," + Math.Round(v.z, digits).s();
        }
        public static Vector2 ToV2(in this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        public static Vector3 MakeSureIsOverTerrain(in this Vector3 p)
        {
            var minY = Terrain.activeTerrain.SampleHeight(p);
            return minY > p.y ? new Vector3(p.x, minY+0.2f, p.z) : p;
        }
        public static Vector3 OverTerrain(in this Vector3 p, double height)
        {
            return new Vector3(p.x, Terrain.activeTerrain.SampleHeight(p) + (float)height, p.z);
        }
        public static Vector3 OverTerrainNoLowerThan(in this Vector3 p, double height, double noLowerThan)
        {
            return new Vector3(p.x, Mathf.Max(Terrain.activeTerrain.SampleHeight(p) + (float)height, (float)noLowerThan), p.z);
        }
        public static float HeightOverTerrain(in this Vector3 p)
        {
            return p.y - Terrain.activeTerrain.SampleHeight(p);
        }
        public static Vector3 EnsureNotUnderTerrain(in this Vector3 p)
        {
            var h = p.HeightOverTerrain();
            if (h < 0)
            {
                return p.PlusY(h.Abs() + 0.1f);
            }
            return p;
        }
        public static Vector3 Round(in this Vector3 p, int decimals = 0)
        {
            return new Vector3((float)Math.Round(p.x, decimals), (float)Math.Round(p.y, decimals), (float)Math.Round(p.z, decimals));
        }
        public static Vector3 EnsureNotUnderTerrain(in this Vector3 p, float minHeightOverTerrain)
        {
            var h = p.HeightOverTerrain();
            if (h < minHeightOverTerrain)
            {
                return p.PlusY(h.Abs() + minHeightOverTerrain);
            }
            return p;
        }
        public static Vector3 ProgressTowards(in this Vector3 from, in Vector3 to, double ratio)
        {
            var ratiof = (float) ratio;
            return new Vector3
            {
                x = from.x + (to.x - from.x) * ratiof,
                y = from.y + (to.y - from.y) * ratiof,
                z = from.z + (to.z - from.z) * ratiof
            };
        }
        public static bool IsSameDirectionAs(in this Vector3 v1, in Vector3 v2)
        {
            return fun.vector.PointSameDirection(in v1, in v2);
        }
        public static Vector3 RotateAbout(in this Vector3 point, in Vector3 pivot, float x, float y, float z)
        {
            return (Quaternion.Euler(x, y, z) * (point - pivot)) + pivot;
        }
        /// <summary>
        /// If looking from axis up towards down the positive angle results in rotation clockwise
        /// </summary>
        public static Vector3 RotateAbout(in this Vector3 point, in Vector3 pivot, in Vector3 axis, double degrees)
        {
            return (Quaternion.AngleAxis((float)degrees, axis) * (point - pivot)) + pivot;
        }
        public static Vector3 RotateAbout(in this Vector3 point,
            in Vector3 pivot1, in Vector3 axis1, double degrees1,
            in Vector3 pivot2, in Vector3 axis2, double degrees2)
        {
            return (Quaternion.AngleAxis((float)degrees2, axis2) * (((Quaternion.AngleAxis((float)degrees1, axis1) * (point - pivot1)) + pivot1) - pivot2) + pivot2);
        }
        public static Vector3 RotateAbout(in this Vector3 point, in Vector3 axis, double degrees)
        {
            return (Quaternion.AngleAxis((float)degrees, axis) * point);
        }
//        public static Vector3 RotateAbout(in this Vector3 point, 
//            Vector3 axis1, double degrees1, 
//            Vector3 axis2, double degrees2)
//        {
//            return 
//                 Quaternion.AngleAxis((float)degrees2, axis2) * 
//                    (Quaternion.AngleAxis((float)degrees1, axis1) * point);
//        }
        public static Vector3 RotateAbout(in this Vector3 point,
            in Vector3 axis1, double degrees1,
            in Vector3 axis2, double degrees2,
            in Vector3 axis3, double degrees3)
        {
            return 
                 Quaternion.AngleAxis((float)degrees3, axis3) * 
                    (Quaternion.AngleAxis((float)degrees2, axis2) * 
                        (Quaternion.AngleAxis((float)degrees1, axis1) * point));
        }
        public static Vector3 PlusX(this Vector3 v, double n)
        {
            v.x += (float)n;
            return v;
        }
        public static Vector3 PlusY(this Vector3 v, double n)
        {
            v.y += (float)n;
            return v;
        }
        public static Vector3 PlusZ(this Vector3 v, double n)
        {
            v.z += (float)n;
            return v;
        }
        public static Vector3 PlusXYZ(this Vector3 v, double x, double y, double z)
        {
            v.x += (float)x;
            v.y += (float)y;
            v.z += (float)z;
            return v;
        }
        public static Vector3 WithLength(in this Vector3 v, double length)
        {
            return v.normalized * (float)length;
        }
        public static Vector3 WithX(this Vector3 v, double n)
        {
            v.x = (float)n;
            return v;
        }
        public static Vector3 WithY(this Vector3 v, double n)
        {
            v.y = (float)n;
            return v;
        }
        public static Vector3 WithY(this Vector3 v, double y, double maxStep)
        {
            v.y = v.y.MoveTowards(y, maxStep);
            return v;
        }
        public static Vector3 WithMaxY(this Vector3 v, double n)
        {
            if(v.y > n) v.y = (float)n;
            return v;
        }
        public static Vector3 WithMinY(this Vector3 v, double n)
        {
            if(v.y < n) v.y = (float)n;
            return v;
        }
        public static Vector3 ClampY(this Vector3 v, double min, double max)
        {
            var tmin = fun.min(min, max);
            var tmax = fun.max(min, max);
            min = tmin;
            max = tmax;

            if(v.y < min) v.y = (float)min;
            else if(v.y > max) v.y = (float)max;
            return v;
        }
        public static Vector3 ClampX(this Vector3 v, double min, double max)
        {
            var tmin = fun.min(min, max);
            var tmax = fun.max(min, max);
            min = tmin;
            max = tmax;

            if (v.x < min) v.x = (float)min;
            else if (v.x > max) v.x = (float)max;
            return v;
        }
        public static Vector3 ClampZ(this Vector3 v, double min, double max)
        {
            var tmin = fun.min(min, max);
            var tmax = fun.max(min, max);
            min = tmin;
            max = tmax;

            if (v.z < min) v.z = (float)min;
            else if (v.z > max) v.z = (float)max;
            return v;
        }
        public static Vector3 WithZ(this Vector3 v, double n)
        {
            v.z = (float)n;
            return v;
        }
        public static Vector3 Clone(in this Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
        private static Vector3 Cross(in Vector3 lhs, in Vector3 rhs)
        {
            return new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));
        }
        private static void Cross(in Vector3 lhs, in Vector3 rhs, out Vector3 r)
        {
            r = new Vector3((lhs.y * rhs.z - lhs.z * rhs.y), (lhs.z * rhs.x - lhs.x * rhs.z), (lhs.x * rhs.y - lhs.y * rhs.x));
        }
        private static float AngleSigned(in Vector3 v1, in Vector3 v2, in Vector3 n)
        {
            var x = Cross(in v1, in v2);
            return (float)Math.Atan2(Dot(in n, in x), Dot(in v1, in v2)) * RTD;
        }
        private static float Dot(in Vector3 lhs, in Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }
        public static Vector3 ApproachByMaxStep(in this Vector3 from, in Vector3 to, double step)
        {
            if (step.IsEqual(0))
            {
                return from;
            }

            var preDistance = fun.distance.Between(in from, in to);

            if (preDistance.IsEqual(0))
            {
                return to;
            }

            if (step > preDistance)
            {
                step = preDistance;
            }

            var amount = (float)step / preDistance;
            return new Vector3
            {
                x = from.x + (to.x - from.x) * amount,
                y = from.y + (to.y - from.y) * amount,
                z = from.z + (to.z - from.z) * amount
            };
        }
        public static Vector3 HalfWayTo(in this Vector3 from, in Vector3 to)
        {
            return fun.point.Lerp(in from, in to, 0.5);
        }
        public static float LenXZ(in this Vector3 v)
        {
            return Mathf.Sqrt((float) ((double) v.x * (double) v.x + (double) v.z * (double) v.z));
      
        }
        public static float MaxDim(in this Vector3 v)
        {
            return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
        }
        
        public static float LengthSquared(in this Vector3 v)
        {
            return (((v.x * v.x) + (v.y * v.y)) + (v.z * v.z));
        }
        public static float DistanceTo(in this Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorY = a.y - b.y;
            var vectorZ = a.z - b.z;
            return (float)Math.Sqrt(((vectorX * vectorX) + (vectorY * vectorY)) + (vectorZ * vectorZ));
        }
        public static float DistanceToPlane(in this Vector3 p, in Vector3 normal, in Vector3 surface)
        {
            return distance.FromPointToPlane(in p, in normal, in surface);
        }
        public static float HorzDistanceTo(in this Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorZ = a.z - b.z;
            return (float)Math.Sqrt((vectorX * vectorX) + (vectorZ * vectorZ));
        }
        public static float HorzDistanceSquaredTo(in this Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorZ = a.z - b.z;
            return (float)((vectorX * vectorX) + (vectorZ * vectorZ));
        }

        public static Quaternion CalculateQuaternionTo(in this Vector3 src, in Vector3 dest)
        {
            src.Normalize();
            dest.Normalize();

            var d = Vector3.Dot(src, dest);

            if (d >= 1f)
            {
                return Quaternion.identity;
            }
            if (d < -0.9999999f)
            {
                var ux = unitX;
                Vector3 axis;
                Cross(in ux, in src, out axis);

                if (axis.LengthSquared().IsZero())
                {
                    var uy = unitY;
                    Cross(in uy, in src, out axis);
                }

                axis.Normalize();
                Quaternion q = Quaternion.AngleAxis(180, axis);
                return q;
            }
            else
            {
                var s = (float)Math.Sqrt((1 + d) * 2);
                var invS = 1 / s;
                Vector3 c;
                Cross(in src, in dest, out c);
                var v = invS*c;

                var q = new Quaternion(v.x, v.y, v.z, 0.5f * s);

                q.Normalize();

                return q;
            }
        }

        public static Vector3 ProjectedOnPlane(in this Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
        {
            //FirstT calculate the distance from the point to the plane:
            var distance = SignedDistancePlanePoint(in planeNormal, in planePoint, in point);

            //Reverse the sign of the distance
            distance *= -1;

            //Give a translation vector
            Vector3 translationVector = SetVectorLength(in planeNormal, distance);

            //Translate the point to form a projection
            return point + translationVector;
        }
        public static Vector3 ProjectedOnVector(in this Vector3 point, in Vector3 vector)
        {
            return fun.point.ProjectOnLine(in point, v3.zero, in vector);
        }
        public static Vector3 ProjectOnLine(in this Vector3 point, in Vector3 line1, in Vector3 line2)
        {
            return fun.point.ProjectOnLine(in point, in line1, in line2);
        }
        public static float SignedDistanceToPlane(in this Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
        {
            return Vector3.Dot(planeNormal, point - planePoint);
        }
        private static float SignedDistancePlanePoint(in Vector3 planeNormal, in Vector3 planePoint, in Vector3 point)
        {
            return Vector3.Dot(planeNormal, point - planePoint);
        }
        private static Vector3 SetVectorLength(in Vector3 vector, float size)
        {
            return Vector3.Normalize(vector) * size;
        }

        public static IList<Vector3> AddTo(in this Vector3 v, IList<Vector3> list)
        {
            list.Add(v);
            return list;
        }
    }
}