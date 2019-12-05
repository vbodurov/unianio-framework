using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class distanceSquared
        {
            public static float Between(in Vector2 a, in Vector2 b)
            {
                var vectorX = (double)(a.x - b.x);
                var vectorY = (double)(a.y - b.y);
                return (float)((vectorX * vectorX) + (vectorY * vectorY));
            }
            public static float Between(in Vector2 a, float bx, float by)
            {
                var vector = new Vector2(a.x - bx, a.y - by);
                return (float)(((double)vector.x * (double)vector.x) + ((double)vector.y * (double)vector.y));
            }
            public static float Between(in Vector3 a, in Vector3 b)
            {
                var vectorX = (double)(a.x - b.x);
                var vectorY = (double)(a.y - b.y);
                var vectorZ = (double)(a.z - b.z);
                return (float)(((vectorX * vectorX) + (vectorY * vectorY)) + (vectorZ * vectorZ));
            }
            public static double BetweenAsDouble(in Vector3 a, in Vector3 b)
            {
                var vectorX = (double)(a.x - b.x);
                var vectorY = (double)(a.y - b.y);
                var vectorZ = (double)(a.z - b.z);
                return (((vectorX * vectorX) + (vectorY * vectorY)) + (vectorZ * vectorZ));
            }
            public static float BetweenIgnoreY(in Vector3 a, in Vector3 b)
            {
                var vectorX = (double)(a.x - b.x);
                var vectorZ = (double)(a.z - b.z);
                return (float)((vectorX * vectorX) + (vectorZ * vectorZ));
            }
            public static float Between(float ax, float ay, float bx, float by)
            {
                var vx = ax - bx;
                var vy = ay - by;
                return (vx * vx) + (vy * vy);
            }
            public static float Between(float ax, float ay, float az, float bx, float by, float bz)
            {
                var vx = ax - bx;
                var vy = ay - by;
                var vz = az - bz;
                return (float)((((double)vx * (double)vx) + ((double)vy * (double)vy)) + ((double)vz * (double)vz));
            }
        }

    }
}