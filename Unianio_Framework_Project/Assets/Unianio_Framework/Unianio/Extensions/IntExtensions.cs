using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class IntExtensions
    {
        public static int Plus(this int i, int toAdd)
        {
            return i + toAdd;
        }
        public static bool MaskContains(this int mask, int value)
        {
            return (mask & value) > 0;
        }
        public static int TryParseIntOrDefault(this string str, int defaultValue)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;
            return int.TryParse(str, out var i) ? i : defaultValue;
        }
        public static int Clamp01(this int i) => i < 0 ? 0 : i > 1 ? 1 : i;
        public static int BinFlagOthers(this int i)
        {
            i = ~i;
            return i;
        }
        public static int ThrowIfNegative(this int i, string message = "Value was not expected to be negative")
        {
            if(i < 0) throw new ArgumentException(message);
            return i;
        }
        public static DateTime FromUnixTimestamp(this long stamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(stamp);
        }
        public static bool ContainsFlag(this ulong mask, ulong value)
        {
            return (mask & value) > 0;
        }
        public static bool ContainsNoFlag(this ulong mask, ulong value)
        {
            return (mask & value) == 0;
        }
        public static ulong AddToMask(this ulong mask, ulong value)
        {
            mask |= value;
            return mask;
        }
        public static ulong RemoveFromMask(this ulong mask, ulong value)
        {
            mask &= ~value;
            return mask;
        }

        public static ulong GetAddedToMask(this ulong mask, ulong value) => mask | value;
        public static ulong GetRemovedFromMask(this ulong mask, ulong value)
        {
            mask &= ~value;
            return mask;
        }
        public static string[] StringNumbersInArrayStartingWith(this int count, int startWith)
        {
            return count <= 0 ? new string[0] : Enumerable.Range(startWith, count).Select(i => i.ToString()).ToArray();
        }
        public static IEnumerable<int> ExtractBit(this ulong mask, ulong typeFlag = 0)
        {
            var temp = mask;
            temp &= ~typeFlag;
            for (var i = 0; i < 64; ++i)
            {
                var curr = ((ulong)1 << i);
                if ((mask & (curr | typeFlag)) == (curr | typeFlag))
                {
                    temp &= ~curr;
                    yield return i;
                }
                if (temp == 0)
                {
                    break;
                }
            }
        }
        static readonly IDictionary<Type,IDictionary<ulong,string>> FieldByNameForTypeCache = 
            new Dictionary<Type, IDictionary<ulong, string>>();
        public static string ToListOfLabels(this ulong mask, Type type, ulong typeFlag = 0)
        {
            IDictionary<ulong, string> fieldByName;
            if(!FieldByNameForTypeCache.TryGetValue(type, out fieldByName))
            {
                FieldByNameForTypeCache[type] = 
                    fieldByName = 
                        type.GetFields(BindingFlags.NonPublic | BindingFlags.Static)
                            .Where(f => f.FieldType == typeof(ulong) && f.IsLiteral)// const ulong
                            .ToDictionary(f => (ulong)f.GetRawConstantValue(), f => f.Name);
            }

            var sb = new StringBuilder();
            var temp = mask;
            temp &= ~typeFlag;
            for (var i = 0; i < 64; ++i)
            {
                var curr = ((ulong)1 << i);
                if ((mask & (curr | typeFlag)) == (curr | typeFlag))
                {
                    temp &= ~curr;
                    string name;
                    if(fieldByName.TryGetValue((curr | typeFlag), out name))
                    {
                        if (sb.Length > 0) sb.Append(", ");
                        sb.Append(name);
                    }
                    
                }
                if (temp == 0)
                {
                    break;
                }
            }
            return sb.ToString();
        }
        public static string PrefixZero(this int i, int length)
        {
            return i.ToString().PadLeft(length, '0');
        }
        public static string ToLabel(this int num)
        {
            var sign = num >= 0 ? "" : "-";
            var abs = num < 0 ? num*-1 : num;
            if (abs < 999) return sign + abs;
            var numToStr = abs.ToString(CultureInfo.InvariantCulture);
            var j = 0;
            var sb = new StringBuilder();

            for(var i = numToStr.Length - 1; i >= 0; --i)
            {
                var curr = numToStr[i];
                sb.Insert(0, curr);
                ++j;
                if (j % 3 == 0 && i != 0) sb.Insert(0, ' ');
            }
            return sign + sb;
        }

        public static string ToTime(this Int64 n)
        {
            if (n < 1000) return n + " ms";
            if (n < 60000) return Math.Round(n / 1000.0, 2) + " seconds";
            if (n < 3600000) return Math.Round(n / 60000.0, 2) + " minutes";
            return Math.Round(n / 3600000.0, 2) + " hours";
        }
        public static int Sign(this int i) { return Math.Sign(i); }
        public static bool Is(this int n, int a) { return n == a; }
        public static bool IsOneOf(this int n, int a) { return n == a; }
        public static bool IsOneOf(this int n, int a, int b) { return n == a || n == b; }
        public static bool IsOneOf(this int n, int a, int b, int c) { return n == a || n == b || n == c; }
        public static bool IsOneOf(this int n, int a, int b, int c, int d) { return n == a || n == b || n == c || n == d; }
        public static bool IsOneOf(this int n, int a, int b, int c, int d, int e) { return n == a || n == b || n == c || n == d || e == n; }
        public static bool IsOneOf(this int n, params int[] args) { return args.Contains(n); }
        public static bool IsEven(this int n) => n % 2 == 0;
        public static bool IsOdd(this int n) => n % 2 != 0;
        public static bool IsDivisibleBy(this int n, int divider) => n % divider == 0;
        public static bool IsOneOfBones(this int index, in BoneWeight bw, out float weight)
        {
            if(bw.boneIndex0 == index)
            {
                weight = bw.weight0; return true;
            }
            if (bw.boneIndex1 == index)
            {
                weight = bw.weight1; return true;
            }
            if (bw.boneIndex2 == index)
            {
                weight = bw.weight2; return true;
            }
            if (bw.boneIndex3 == index)
            {
                weight = bw.weight3; return true;
            }
            weight = 0;
            return false;
        }
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }
        public static int Abs(this int n)
        {
            return n >= 0 ? n : n * -1;
        }
        public static int NoMoreThan(this int a, int b) { return a > b ? b : a; }
        public static int NoLessThan(this int a, int b) { return a < b ? b : a; }
        public static int PackWithAs16Bit(this int x, int y)
        {
            if (x < -32767) throw new ArgumentException("x cannot be below -32767, it is " + x);
            if (x > +32767) throw new ArgumentException("x cannot be above +32767, it is " + x);
            if (y < -32767) throw new ArgumentException("y cannot be below -32767, it is " + y);
            if (y > +32767) throw new ArgumentException("y cannot be above +32767, it is " + y);
            return (x + 32767) | (((y + 32767) & 65535) << 16);
        }
        public static void Unpack16Bit(this int pack, out short x, out short y)
        {
            x = (short)((pack & 65535) - 32767);
            y = (short)(((pack >> 16) & 65535) - 32767);
        }

    }
}
