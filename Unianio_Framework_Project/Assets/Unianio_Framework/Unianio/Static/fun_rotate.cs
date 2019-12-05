using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class rotate
        {
            public static void AngleAndAxisToQuaternion(float degrees, in Vector3 axis, out Quaternion quaternion)
            {
                var radians = degrees * DTR;
                var s = (float)Math.Sin(radians / 2f);
                var x = axis.x * s;
                var y = axis.y * s;
                var z = axis.z * s;
                var w = (float)Math.Cos(radians / 2);
                quaternion = new Quaternion(x, y, z, w);
            }
            /// <summary>
            /// If looking from axis up towards down the positive angle results in rotation clockwise
            /// </summary>
            public static void Vector(in Vector3 vector, in Vector3 aboutAxis, double degrees, out Vector3 result)
            {
                //var rotation = Quaternion.AngleAxis(degrees, aboutAxis);
                Quaternion rotation;
                AngleAndAxisToQuaternion((float)degrees, in aboutAxis, out rotation);

                var num1 = rotation.x * 2f;
                var num2 = rotation.y * 2f;
                var num3 = rotation.z * 2f;
                var num4 = rotation.x * num1;
                var num5 = rotation.y * num2;
                var num6 = rotation.z * num3;
                var num7 = rotation.x * num2;
                var num8 = rotation.x * num3;
                var num9 = rotation.y * num3;
                var num10 = rotation.w * num1;
                var num11 = rotation.w * num2;
                var num12 = rotation.w * num3;
                Vector3 vector3;
                vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)vector.x + ((double)num7 - (double)num12) * (double)vector.y + ((double)num8 + (double)num11) * (double)vector.z);
                vector3.y = (float)(((double)num7 + (double)num12) * (double)vector.x + (1.0 - ((double)num4 + (double)num6)) * (double)vector.y + ((double)num9 - (double)num10) * (double)vector.z);
                vector3.z = (float)(((double)num8 - (double)num11) * (double)vector.x + ((double)num9 + (double)num10) * (double)vector.y + (1.0 - ((double)num4 + (double)num5)) * (double)vector.z);
                result = vector3;
            }

            public static void Point2dAbout(in Vector2 pointToRotate, in Vector2 privotPoint, double degrees, out Vector2 result)
            {
                var s = sin(degrees * DTR);
                var c = cos(degrees * DTR);

                var px = pointToRotate.x;
                var py = pointToRotate.y;

                // translate point back to origin:
                px -= privotPoint.x;
                py -= privotPoint.y;

                // rotate point
                var xnew = px * c - py * s;
                var ynew = px * s + py * c;

                // translate point back:
                px = xnew + privotPoint.x;
                py = ynew + privotPoint.y;
                result = new Vector2(px, py);
            }
            public static void PointAbout(in Vector3 rotatePoint, in Vector3 pivot, in Vector3 aboutAxis, double degrees, out Vector3 result)
            {
                var rotation = Quaternion.AngleAxis((float)degrees, aboutAxis);

                var point = rotatePoint - pivot;

                var num1 = rotation.x * 2f;
                var num2 = rotation.y * 2f;
                var num3 = rotation.z * 2f;
                var num4 = rotation.x * num1;
                var num5 = rotation.y * num2;
                var num6 = rotation.z * num3;
                var num7 = rotation.x * num2;
                var num8 = rotation.x * num3;
                var num9 = rotation.y * num3;
                var num10 = rotation.w * num1;
                var num11 = rotation.w * num2;
                var num12 = rotation.w * num3;
                Vector3 vector3;
                vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y + ((double)num8 + (double)num11) * (double)point.z);
                vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y + ((double)num9 - (double)num10) * (double)point.z);
                vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x + ((double)num9 + (double)num10) * (double)point.y + (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
                result = vector3 + pivot;
            }

            public static Vector3 PointAbout(in Vector3 rotatePoint, in Vector3 pivot, in Vector3 aboutAxis, double degrees)
            {
                var rotation = Quaternion.AngleAxis((float)degrees, aboutAxis);

                var point = rotatePoint - pivot;

                var num1 = rotation.x * 2f;
                var num2 = rotation.y * 2f;
                var num3 = rotation.z * 2f;
                var num4 = rotation.x * num1;
                var num5 = rotation.y * num2;
                var num6 = rotation.z * num3;
                var num7 = rotation.x * num2;
                var num8 = rotation.x * num3;
                var num9 = rotation.y * num3;
                var num10 = rotation.w * num1;
                var num11 = rotation.w * num2;
                var num12 = rotation.w * num3;
                Vector3 vector3;
                vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y + ((double)num8 + (double)num11) * (double)point.z);
                vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y + ((double)num9 - (double)num10) * (double)point.z);
                vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x + ((double)num9 + (double)num10) * (double)point.y + (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
                return vector3 + pivot;
            }

            public static void Vector(in Vector3 vector, in Vector3 aboutAxis, in Quaternion rotation, out Vector3 result)
            {
                var num1 = rotation.x * 2f;
                var num2 = rotation.y * 2f;
                var num3 = rotation.z * 2f;
                var num4 = rotation.x * num1;
                var num5 = rotation.y * num2;
                var num6 = rotation.z * num3;
                var num7 = rotation.x * num2;
                var num8 = rotation.x * num3;
                var num9 = rotation.y * num3;
                var num10 = rotation.w * num1;
                var num11 = rotation.w * num2;
                var num12 = rotation.w * num3;
                Vector3 vector3;
                vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)vector.x + ((double)num7 - (double)num12) * (double)vector.y + ((double)num8 + (double)num11) * (double)vector.z);
                vector3.y = (float)(((double)num7 + (double)num12) * (double)vector.x + (1.0 - ((double)num4 + (double)num6)) * (double)vector.y + ((double)num9 - (double)num10) * (double)vector.z);
                vector3.z = (float)(((double)num8 - (double)num11) * (double)vector.x + ((double)num9 + (double)num10) * (double)vector.y + (1.0 - ((double)num4 + (double)num5)) * (double)vector.z);
                result = vector3;
            }

            public static void PointAbout(in Vector3 rotatePoint, in Vector3 pivot, in Vector3 aboutAxis, in Quaternion rotation, out Vector3 result)
            {
                var point = rotatePoint - pivot;

                var num1 = rotation.x * 2f;
                var num2 = rotation.y * 2f;
                var num3 = rotation.z * 2f;
                var num4 = rotation.x * num1;
                var num5 = rotation.y * num2;
                var num6 = rotation.z * num3;
                var num7 = rotation.x * num2;
                var num8 = rotation.x * num3;
                var num9 = rotation.y * num3;
                var num10 = rotation.w * num1;
                var num11 = rotation.w * num2;
                var num12 = rotation.w * num3;
                Vector3 vector3;
                vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y + ((double)num8 + (double)num11) * (double)point.z);
                vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y + ((double)num9 - (double)num10) * (double)point.z);
                vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x + ((double)num9 + (double)num10) * (double)point.y + (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
                result = vector3 + pivot;
            }

            public static Vector3 FwToUpAndNormal(double degreesFwToUp, double degreesAroundNormal)
            {
                var dir = Vector3.forward;
                var hasMoveToUp = abs(degreesFwToUp) >= 0.001;
                if (hasMoveToUp)
                {
                    dir = dir.RotateTowards(Vector3.up, degreesFwToUp);
                }

                var hasRotationAroundNormal = abs(degreesAroundNormal) >= 0.001;
                if (hasRotationAroundNormal)
                {
                    var norm0 = hasMoveToUp ? vector.GetNormal(Vector3.forward, in dir) : Vector3.right;
                    var norm1 = vector.GetNormal(in dir, in norm0);
                    dir = dir.RotateAbout(in norm1, degreesAroundNormal);
                }
                return dir;
            }

            public static Vector3 FwToRtAndNormal(double degreesFwToRt, double degreesAroundNormal)
            {
                var dir = Vector3.forward;
                var hasMoveToUp = abs(degreesFwToRt) >= 0.001;
                if (hasMoveToUp)
                {
                    dir = dir.RotateTowards(Vector3.right, degreesFwToRt);
                }

                var hasRotationAroundNormal = abs(degreesAroundNormal) >= 0.001;
                if (hasRotationAroundNormal)
                {
                    var norm0 = hasMoveToUp ? vector.GetNormal(Vector3.forward, in dir) : Vector3.up;
                    var norm1 = vector.GetNormal(in dir, in norm0);
                    dir = dir.RotateAbout(in norm1, degreesAroundNormal);
                }
                return dir;
            }
        }

    }
}