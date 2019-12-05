using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unianio.Enums;

namespace Unianio.Extensions
{
    public static class StringExtensions
    {
        public static string IfNullOrEmptyThen(this string str, string then)
        {
            if (str == null || str.Trim() == String.Empty) return then;
            return str;
        }
        public static string IfLongerThenTakeEnds(this string str, int maxCharsFromStart, int maxCharsFromEnd)
        {
            maxCharsFromStart = Math.Max(maxCharsFromStart, 0);
            maxCharsFromEnd = Math.Max(maxCharsFromEnd, 0);
            if (str == null || str.Length <= (maxCharsFromStart + maxCharsFromEnd)) return str;
            var sb = new StringBuilder();
            if (maxCharsFromStart > 0) sb.AppendLine(str.Substring(0, maxCharsFromStart)).AppendLine("...");
            if (maxCharsFromEnd > 0) sb.Append(str.Substring(str.Length - maxCharsFromEnd, maxCharsFromEnd));
            return sb.ToString();
        }
        public static string NoLongerThan(this string str, int maxChars)
        {
            return str == null || str.Length <= maxChars ? str : str.Substring(0, maxChars);
        }
        public static ulong ExtractLastUlong(this string str, char limiter = '_')
        {
            if (string.IsNullOrEmpty(str)) return 0;
            var arr = str.Split(limiter);
            ulong.TryParse(arr[arr.Length - 1]?.Trim(), out var n);
            return n;
        }
        public static string MHBonePx(this string str, BodySide sd) => str.Substring(0, str.Length - 2) + (sd.IsLeft() ? "_L" : "_R");
        public static string GenBonePx(this string str, BodySide sd) => (sd.IsLeft() ? 'l' : 'r') + str.Substring(1);
        public static string Args(this string str, params object[] args) => String.Format(str, args);
        public static bool IsOneOf(this string str, string a) { return str == a; }
        public static bool IsOneOf(this string str, string a, string b) { return str == a || str == b; }
        public static bool IsOneOf(this string str, string a, string b, string c) { return str == a || str == b || str == c; }
        public static bool IsOneOf(this string str, string a, string b, string c, string d) { return str == a || str == b || str == c || str == d; }
        public static bool IsOneOf(this string str, params string[] all)
        {
            for (var i = 0; i < all.Length; i++)
            {
                if (all[i] == str) return true;
            }
            return false;
        }
        public static bool Is_Not_NullOrEmpty(this string str) => !string.IsNullOrEmpty(str);
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);
        public static bool Is_Not_NullOrWhiteSpace(this string str) => !string.IsNullOrWhiteSpace(str);

        public static bool IsOneOf(this string str, HashSet<string> set)
            => set.Contains(str);

        public static bool StartsWithOneOf(this string str, string a) { return str.StartsWith(a); }
        public static bool StartsWithOneOf(this string str, string a, string b) { return str.StartsWith(a) || str.StartsWith(b); }
        public static bool StartsWithOneOf(this string str, string a, string b, string c) { return str.StartsWith(a) || str.StartsWith(b) || str.StartsWith(c); }
        public static bool StartsWithOneOf(this string str, string a, string b, string c, string d) { return str.StartsWith(a) || str.StartsWith(b) || str.StartsWith(c) || str.StartsWith(d); }
        public static bool StartsWithOneOf(this string str, string a, string b, string c, string d, string e) { return str.StartsWith(a) || str.StartsWith(b) || str.StartsWith(c) || str.StartsWith(d) || str.StartsWith(e); }
        public static bool StartsWithOneOf(this string str, string a, string b, string c, string d, string e, string f) { return str.StartsWith(a) || str.StartsWith(b) || str.StartsWith(c) || str.StartsWith(d) || str.StartsWith(e) || str.StartsWith(f); }

        public static string SubstringOrEmpty(this string str, int start)
        {
            if (string.IsNullOrEmpty(str) || start <= 0) return str;
            if (start > str.Length) return "";
            return str.Substring(start);
        }
        
        public static bool ContainsAny(this string str, params string[] strings)
        {
            if (strings == null || strings.Length == 0) throw new ArgumentException("Missing string to search for with method ContainsAny");
            return strings.Any(t => str.IndexOf(t, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }
    
    }
}