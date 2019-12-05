using System;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion DifferenceTo(in this Quaternion a, in Quaternion b)
        {
            return Quaternion.Inverse(a) * b;
        }
        public static Quaternion LocalToWorld(in this Quaternion local, in Quaternion parentWorld)
        {
            return Quaternion.Inverse(local) * parentWorld;
        }
        public static string s(in this Quaternion q)
        {
            return q.x.s()+"f,"+q.y.s()+"f,"+q.z.s()+"f,"+q.w.s()+"f";
        }
        public static Vector4 ToV4(in this Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }
        public static Quaternion ToQt(in this Vector4 v)
        {
            return new Quaternion(v.x,v.y,v.z,v.w);
        }
        public static Quaternion AsHorizontal(in this Quaternion q)
        {
            var fwDir = q * Vector3.forward;
            var upDir = q * Vector3.up;
            var isUpUp = fun.vector.PointSameDirection(in upDir, in v3.up);
            upDir = isUpUp ? Vector3.up : Vector3.down;
            return Quaternion.LookRotation(fwDir.ToHorzUnit(), upDir);
        }
        public static Quaternion AsLocalRot(in this Quaternion worldRotation, Transform parent)
        {
            return parent == null ? worldRotation : Quaternion.Inverse(parent.rotation) * worldRotation;
        }
        public static Quaternion AsLocalRot(in this Quaternion worldRotation, in Quaternion parentWorld)
        {
            return Quaternion.Inverse(parentWorld) * worldRotation;
        }
        public static Quaternion AsWorldRot(in this Quaternion localRotation, Transform parent)
        {
            return parent == null ? localRotation : parent.rotation * localRotation;
        }
        public static Quaternion AsWorldRot(in this Quaternion localRotation, in Quaternion parentWorld)
        {
            return parentWorld * localRotation;
        }
        public static Quaternion RotateTowards(in this Quaternion from, in Vector3 toFw, in Vector3 toUp, double maxDegreesDelta)
        {
            return from.RotateTowards(lookAt(toFw, toUp), maxDegreesDelta);
        }
        public static Quaternion RotateTowards(in this Quaternion from, in Quaternion to, double maxDegreesDelta)
        {
            var angleDegrees = fun.angle.Between(in from, in to);
            if (Math.Abs(angleDegrees) < 0.00001 || maxDegreesDelta >= 180)
                return to;
            float t = (float)Math.Min(1f, maxDegreesDelta / angleDegrees);
            return Quaternion.Slerp(from, to, t);
        }
        public static void RotateTowards(in this Quaternion from, in Quaternion to, double maxDegreesDelta, out Quaternion output)
        {
            var angleDegrees = fun.angle.Between(in from, in to);
            if (Math.Abs(angleDegrees) < 0.00001 || maxDegreesDelta >= 180)
            {
                output = to;
                return;
            }
            float t = (float)Math.Min(1f, maxDegreesDelta / angleDegrees);
            output = Quaternion.Slerp(from, to, t);
        }

        private const double TwiseRadiansToDegrees = 2.0*57.2957801818848;
        public static float DegreesTo(in this Quaternion from, in Quaternion to)
        {
            return (float) ((double) Math.Acos(Math.Min(Math.Abs(dot(in from, in to)), 1f)) * TwiseRadiansToDegrees);
        }
        public static float RadiansTo(in this Quaternion from, in Quaternion to)
        {
            return (float) ((double) Math.Acos(Math.Min(Math.Abs(dot(in from, in to)), 1f)) * 2);
        }
        public static bool IsEqual(in this Quaternion a, in Quaternion b)
        {
            return
                ((double)a.x).IsEqual((double)b.x) &&
                ((double)a.y).IsEqual((double)b.y) &&
                ((double)a.z).IsEqual((double)b.z) &&
                ((double)a.w).IsEqual((double)b.w);
        }
        public static bool IsEqual(in this Quaternion a, in Quaternion b, double delta)
        {
            return
                ((double)a.x).IsEqual((double)b.x, delta) &&
                ((double)a.y).IsEqual((double)b.y, delta) &&
                ((double)a.z).IsEqual((double)b.z, delta) &&
                ((double)a.w).IsEqual((double)b.w, delta);
        }
        public static bool IsNotEqual(in this Quaternion a, in Quaternion b)
        {
            return
                ((double)a.x).IsNotEqual((double)b.x) ||
                ((double)a.y).IsNotEqual((double)b.y) ||
                ((double)a.z).IsNotEqual((double)b.z) ||
                ((double)a.w).IsNotEqual((double)b.w);
        }
        public static bool IsNotEqual(in this Quaternion a, in Quaternion b, double delta)
        {
            return
                ((double)a.x).IsNotEqual((double)b.x, delta) ||
                ((double)a.y).IsNotEqual((double)b.y, delta) ||
                ((double)a.z).IsNotEqual((double)b.z, delta) ||
                ((double)a.w).IsNotEqual((double)b.w, delta);
        }
        public static bool Equals(in this Quaternion a, in Quaternion b)
        {
            if (a.x.Equals(b.x) && a.y.Equals(b.y) && a.z.Equals(b.z))
                return a.w.Equals(b.w);
            return false;
        }
        public static void Normalize(this Quaternion q)
        {
            var num2 = (((q.x * q.x) + (q.y * q.y)) + (q.z * q.z)) + (q.w * q.w);
            var num = 1f / ((float)Math.Sqrt(num2));
            q.x *= num;
            q.y *= num;
            q.z *= num;
            q.w *= num;
        }
        /*
roll  = atan2(2*gety*w - 2*x*z, 1 - 2*gety*gety - 2*z*z)
pitch = atan2(2*x*w - 2*gety*z, 1 - 2*x*x - 2*z*z)
yaw   =  asin(2*x*gety + 2*z*w)
         * 
roll  = Mathf.Atan2(2*gety*w - 2*x*z, 1 - 2*gety*gety - 2*z*z);
pitch = Mathf.Atan2(2*x*w - 2*gety*z, 1 - 2*x*x - 2*z*z);
yaw   =  Mathf.Asin(2*x*gety + 2*z*w);
 */

        public static float GetYaw(in this Quaternion q)
        {
            return q.eulerAngles.y;
        }
        public static float GetPitch(in this Quaternion q)
        {
            return q.eulerAngles.x;
        }
        public static float GetRoll(in this Quaternion q)
        {
            return q.eulerAngles.z;
        }
        public static void GetYawPitchRoll(in this Quaternion q, out float yaw, out float pitch, out float roll)
        {
            yaw = q.eulerAngles.y;
            pitch = q.eulerAngles.x;
            roll = q.eulerAngles.z;
        }
    }
}