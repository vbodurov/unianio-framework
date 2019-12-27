using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Unianio.Moves;

namespace Unianio.Extensions
{
    public static class DoubleExtensions
    {
        public const double Epsilon = 9.99999974737875E-06;

        public static float Float(this double d) { return (float)d; }

        public static double FromRangeTo01(this double xOfRange, double xMin, double xMax)
        {
            var diff = xMax - xMin;
            return (Math.Abs(diff) < 0.0000001 ? 1.0 : (xOfRange - xMin)/diff);
        }
        public static double FromRangeTo01Clamped(this double xOfRange, double xMin, double xMax)
        {
            var result = xOfRange.FromRangeTo01(xMin, xMax);
            return result < 0f ? 0f : result > 1f ? 1f : result;
        }
        public static double From01ToRange(this double x01, double xMin, double xMax)
        {
            return ((xMax - xMin)*x01 + xMin);
        }
        public static double FromMin11To01(this double x)
        {
            return (x)*0.5f + 0.5f;
        }
        public static double FromMin11To01Clamped(this double x)
        {
            var result = x.FromMin11To01();
            return result < 0f ? 0f : result > 1f ? 1f : result;
        }
        public static double From01ToMin11(this double x)
        {
            return 2*x - 1f;
        }
        public static double From01ToMin11Clamped(this double x)
        {
            var result = x.From01ToMin11();
            return result < -1f ? -1f : result > 1f ? 1f : result;
        }
        public static double IfConditionTransform(this double x, Func<double, bool> condition, Func<double, float> transform)
        {
            if (condition(x))
            {
                return transform(x);
            }
            return x;
        }
        public static double Round(this double n, int digits)
        {
            return Math.Round(n, digits);
        }
        public static bool IsZero(this double n)
        {
            return n.IsZero(Epsilon);
        }
        public static bool IsZero(this double n, double epsilon)
        {
            return n < epsilon && n > -epsilon;
        }
        public static float GoTowards(this double from, double to, double step)
        {
            var diff = to - from;
            if (diff.Abs() <= step) return (float)to;
            var sign = diff < 0 ? -1 : 1;
            return (float)(from + step*sign);
        }
        public static bool IsBetween(this double n, double from, double to)
        {
            return n >= from && n <= to;
        }
        public static bool IsEqual(this double n, double m)
        {
            return n.IsEqual(m, Epsilon);
        }
        public static bool IsEqual(this double n, double m, double epsilon)
        {
            return Math.Abs(n - m) < epsilon;
        }
        public static bool IsNotEqual(this double n, double m)
        {
            return n.IsNotEqual(m, Epsilon);
        }
        public static bool IsNotEqual(this double n, double m, double epsilon)
        {
            return Math.Abs(n - m) >= epsilon;
        }
        public static double Project(this double n, IBezierGroup bg)
        {
            return bg.run(n);
        }
        public static float Project(this double n, Func<double, double> fn)
        {
            if (fn == null) return (float)n; // no function is considered linear identity function 
            return (float)fn(n);
        }

        public static double Abs(this double n)
        {
            return n >= 0 ? n : n * -1;
        }
        public static double NoMoreThan(this double a, double b) { return a > b ? b : a; }
        public static double NoLessThan(this double a, double b) { return a < b ? b : a; }
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }
        public static double Clamp01(this double value)
        {
            if (value < 0)
            {
                return 0.0;
            }
            if (value > 1)
            {
                return 1.0;
            }
            return value;
        }
        public static double ClampMin11(this double value)
        {
            if (value < -1)
            {
                return -1.0;
            }
            if (value > 1)
            {
                return 1.0;
            }
            return value;
        }

        public static float OfM11Range(this double progress, double min, double max)
        {
            progress = 0.5 * progress + 0.5;
            return (float)(min + progress*(max - min));
        }
        public static string s(this double input)
        {
            string str = input.ToString(CultureInfo.InvariantCulture);

            // if string representation was collapsed from scientific notation, just return it:
            if (str.IndexOf("E", StringComparison.OrdinalIgnoreCase) < 0) return str;

            bool negativeNumber = false;

            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                negativeNumber = true;
            }

            string sep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            char decSeparator = sep.ToCharArray()[0];

            string[] exponentParts = str.Split('E');
            string[] decimalParts = exponentParts[0].Split(decSeparator);

            // fix missing decimal point:
            if (decimalParts.Length==1) decimalParts = new string[]{exponentParts[0],"0"};

            int exponentValue = int.Parse(exponentParts[1]);

            string newNumber = decimalParts[0] + decimalParts[1];

            string result;

            if (exponentValue > 0)
            {
                result = 
                    newNumber + 
                    GetZeros(exponentValue - decimalParts[1].Length);
            }
            else // negative exponent
            {
                result = 
                    "0" + 
                    decSeparator + 
                    GetZeros(exponentValue + decimalParts[0].Length) + 
                    newNumber;

                result = result.TrimEnd('0');
            }

            if (negativeNumber)
                result = "-" + result;

            return result;
        }

        private static string GetZeros(int zeroCount)
        {
            if (zeroCount < 0) 
                zeroCount = Math.Abs(zeroCount);

            var sb = new StringBuilder();

            for (int i = 0; i < zeroCount; i++) sb.Append("0");    

            return sb.ToString();
        }
    }
}