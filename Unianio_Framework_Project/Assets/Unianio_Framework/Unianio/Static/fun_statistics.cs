using System;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class statistics
        {
            private const float epsilon = 0.00001f;


            public static Vector3 LerpAverageV2(Vector2 lastAverage, Vector2 current, int count)
            {
                if (count <= 1) return current;
                return Vector2.Lerp(lastAverage, current, 1 / (float)count);
            }
            public static Vector3 LerpAverageV2(in Vector2 lastAverage, in Vector2 current, int count)
            {
                if (count <= 1) return current;
                return Vector2.Lerp(lastAverage, current, 1 / (float)count);
            }
            public static void LerpAverageV2(in Vector2 lastAverage, in Vector2 current, int count, out Vector2 currentAverage)
            {
                if (count <= 1)
                {
                    currentAverage = current;
                    return;
                }
                currentAverage = Vector2.Lerp(lastAverage, current, 1 / (float)count);
            }

            public static Vector3 LerpAverage(Vector3 lastAverage, Vector3 current, int count)
            {
                if (count <= 1) return current;
                return Vector3.Lerp(lastAverage, current, 1 / (float)count);
            }
            public static Vector3 LerpAverage(in Vector3 lastAverage, in Vector3 current, int count)
            {
                if (count <= 1) return current;
                return Vector3.Lerp(lastAverage, current, 1 / (float)count);
            }
            public static void LerpAverage(in Vector3 lastAverage, in Vector3 current, int count, out Vector3 currentAverage)
            {
                if (count <= 1)
                {
                    currentAverage = current;
                    return;
                }
                currentAverage = Vector3.Lerp(lastAverage, current, 1 / (float)count);
            }


            public static Vector3 SlerpAverage(Vector3 lastAverage, Vector3 current, int count)
            {
                if (count <= 1) return current;
                return Vector3.Slerp(lastAverage, current, 1 / (float)count);
            }
            public static Vector3 SlerpAverage(in Vector3 lastAverage, in Vector3 current, int count)
            {
                if (count <= 1) return current;
                return Vector3.Slerp(lastAverage, current, 1 / (float)count);
            }
            public static void SlerpAverage(in Vector3 lastAverage, in Vector3 current, int count, out Vector3 currentAverage)
            {
                if (count <= 1)
                {
                    currentAverage = current;
                    return;
                }
                currentAverage = Vector3.Slerp(lastAverage, current, 1 / (float)count);
            }
            public static Quaternion SlerpAverage(Quaternion lastAverage, Quaternion current, int count)
            {
                if (count <= 1) return current;
                return Quaternion.Slerp(lastAverage, current, 1 / (float)count);
            }


            public static Quaternion SlerpAverage(in Quaternion lastAverage, in Quaternion current, int count)
            {
                if (count <= 1) return current;
                return Quaternion.Slerp(lastAverage, current, 1 / (float)count);
            }


            public static float Average(double lastAverage, double current, int count)
            {
                return (count <= 1.0f) ? (float)current : ((float)lastAverage * (count - 1.0f) + (float)current) / count;
            }
            public static float Average(float lastAverage, float current, int count)
            {
                return (count <= 1.0f) ? current : (lastAverage * (count - 1.0f) + current) / count;
            }
            public static float PopulationVariance(double sumOfSquared, double sum, int count)
            {
                if (count <= 0) return 0;
                return (float)((sumOfSquared - ((sum * sum) / count)) / count);
            }
            public static float PopulationStandardDeviation(double sumOfSquared, double sum, int count)
            {
                return (count <= 1)
                        ? 0.0f
                        : (float)Math.Sqrt(PopulationVariance(sumOfSquared, sum, count));
            }
        }

    }
}