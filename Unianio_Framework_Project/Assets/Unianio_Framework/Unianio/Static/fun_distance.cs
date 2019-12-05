using System;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class distance
        {
            public static float Between2D(in Vector2 a, in Vector2 b)
            {
                var vectorX = a.x - b.x;
                var vectorY = a.y - b.y;
                return (float)Math.Sqrt((((double)vectorX * (double)vectorX) + ((double)vectorY * (double)vectorY)));
            }
            public static float Between2D(in Vector2 a, float bx, float by)
            {
                var vector = new Vector2(a.x - bx, a.y - by);
                return (float)Math.Sqrt((((double)vector.x * (double)vector.x) + ((double)vector.y * (double)vector.y)));
            }
            public static float Between(in Vector3 a, in Vector3 b)
            {
                var vectorX = a.x - b.x;
                var vectorY = a.y - b.y;
                var vectorZ = a.z - b.z;
                return (float)Math.Sqrt((((double)vectorX * (double)vectorX) + ((double)vectorY * (double)vectorY)) + ((double)vectorZ * (double)vectorZ));
            }
            public static float BetweenIgnoreY(in Vector3 a, in Vector3 b)
            {
                var vectorX = a.x - b.x;
                var vectorZ = a.z - b.z;
                return (float)Math.Sqrt(((double)vectorX * (double)vectorX) + ((double)vectorZ * (double)vectorZ));
            }
            public static float Between(float ax, float ay, float bx, float by)
            {
                var vx = ax - bx;
                var vy = ay - by;
                return (float)Math.Sqrt(((vx * vx) + (vy * vy)));
            }
            public static float Between(float ax, float ay, float az, float bx, float by, float bz)
            {
                var vx = ax - bx;
                var vy = ay - by;
                var vz = az - bz;
                return (float)Math.Sqrt((((double)vx * (double)vx) + ((double)vy * (double)vy)) + ((double)vz * (double)vz));
            }

            public static float FromPointToPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                var vectorToPlane = point - planePoint;
                var distance = dot(in planeNormal, in vectorToPlane);
                return abs(distance);
            }
        }

    }
}