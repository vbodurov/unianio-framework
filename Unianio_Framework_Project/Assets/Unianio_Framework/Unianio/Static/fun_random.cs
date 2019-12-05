using System;
using System.Collections.Generic;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class random
        {
            static readonly System.Random _rnd = new System.Random((int)(DateTime.UtcNow.Ticks % 1000000000));

            public static T[] SequenceOf<T>(int length, (T item, int[] repeat)[] config)
            {
                var list = new List<T>();
                var j = 0;
                while (j < length)
                {
                    ++j;
                    for (int i = 0; i < config.Length; ++i)
                    {
                        var t = config[i];

                        var n = rndOf(t.repeat);

                        for (var k = 0; k < n; ++k)
                        {
                            list.Add(t.item);
                        }
                    }
                }

                return list.ToArray();
            }
            public static float Between(double min, double max)
            {
                return Between((float)min, (float)max);
            }
            // probabilityDistribFunc => 0; would mean non altered probability, func [0-1]
            public static float Between(double min, double max, Func<double, double> probabilityDistribFunc)
            {
                return Between((float)min, (float)max, probabilityDistribFunc);
            }
            public static int Between(int min, int max)
            {
                return _rnd.Next(min, max + 1);
            }
            public static float Between(float min, float max)
            {
                var n = (float)_rnd.NextDouble();
                return (max - min) * n + min;
            }
            public static float number01 => (float)_rnd.NextDouble();

            // probabilityDistribFunc => 0; would mean non altered probability, func [0-1]
            public static float Between(float min, float max, Func<double, double> probabilityDistribFunc)
            {
                var nd = _rnd.NextDouble();
                var n = (float)probabilityDistribFunc(nd);
                var range = (max - min);
                var candidate = range * n + min;
                return candidate.Clamp(min, max);
            }
            public static int IndexExcept(int numberAll, int exceptIndex)
            {
                if (numberAll <= 0) return 0;
                if (exceptIndex < 0 || exceptIndex >= numberAll) return Between(0, numberAll - 1);
                var i = Between(0, numberAll - 2);
                return (i >= exceptIndex) ? i + 1 : i;
            }
            public static T Between<T>(IList<T> arr, int exceptIndex)
            {
                return arr[Index(arr.Count, exceptIndex)];
            }
            public static int Index(int count)
            {
                return _rnd.Next(0, count);
            }
            public static int Index(int count, int exceptIndex)
            {
                if (exceptIndex < 0 || exceptIndex >= count) return Index(count);
                var i = Index(count - 1);
                if (i >= exceptIndex) ++i;
                return i;
            }
            public static int Index(int count, Func<double, double> probabilityDistribFunc)
            {
                var nd = _rnd.NextDouble();
                var n = (float)probabilityDistribFunc(nd);
                var index = (int)Math.Round(count * n);
                return index.Clamp(0, count - 1);
            }
            public static bool Bool(double probability)
            {
                if (probability < 0.000001) return false;
                if (probability > 0.999999) return true;
                var n = (float)_rnd.NextDouble();
                return n <= probability;
            }
            public static Vector2 V2(float x1, float y1, float x2, float y2)
            {
                return new Vector2(Between(x1, x2), Between(y1, y2));
            }

            public static Vector3 PointOnPlane(Vector3 position, Vector3 normal, float radius)
            {
                Vector3 randomPoint;

                do
                {
                    randomPoint = Vector3.Cross(UnityEngine.Random.insideUnitSphere, normal);
                } while (randomPoint == Vector3.zero);

                randomPoint.Normalize();
                randomPoint *= radius;
                randomPoint += position;

                return randomPoint;
            }

            public static void PointOnPlane(in Vector3 position, in Vector3 normal, float radius, out Vector3 result)
            {
                Vector3 randomPoint;

                do
                {
                    randomPoint = Vector3.Cross(UnityEngine.Random.insideUnitSphere, normal);
                } while (randomPoint == Vector3.zero);

                randomPoint.Normalize();
                randomPoint *= radius;
                randomPoint += position;

                result = randomPoint;
            }
            public static bool TrySelectIndexNotInBit(int len, ref long mask, out int index)
            {
                if (len >= 64) throw new ArgumentException("The length of mask must be less than 32");
                var numChecked = 0;
                for (var i = 0; i < len; ++i)
                {
                    if ((mask & (1 << i)) > 0) ++numChecked;
                }
                var tempIndex = Between(0, len - numChecked);
                var count = -1;
                for (var i = 0; i < len; ++i)
                {
                    var curr = (long)1 << i;
                    if ((curr & mask) == 0) ++count;
                    if (count == tempIndex)
                    {
                        mask |= curr;
                        index = i;
                        return true;
                    }
                }
                index = 0;
                return false;
            }
        }

    }
}