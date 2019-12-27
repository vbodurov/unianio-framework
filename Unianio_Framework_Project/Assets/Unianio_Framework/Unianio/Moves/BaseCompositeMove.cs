using System;
using Unianio.Extensions;

namespace Unianio.Moves
{
    public abstract class BaseCompositeMove<T> where T : IRangeOfNumbers
    {
        protected static void ValidateChildren(T[] all)
        {
            if (all == null || all.Length == 0)
                throw new ArgumentException("CompositeMove requires 1 or more " + typeof(T) + " sub paths");

            var last = float.MinValue;
            for (var i = 0; i < all.Length; ++i)
            {
                var current = all[i].To;
                if (current < last)
                    throw new ArgumentException("CompositeMove requires all " + typeof(T) + " to be in order");
                last = current;
            }
        }
        protected static float RatioBetween(double n, double min, double max)
        {
            var range = max - min;
            if (range.IsZero()) return 0;
            return (float)((n - min) / range);
        }
        protected static T GetCurrent(ref T last, T[] all, double progress)
        {
            if (progress.IsBetween(last.From, last.To)) return last;

            var index = all.Length - 1;
            if (progress <= 0)
            {
                index = 0;
            }
            else if (progress < 1)
            {
                for (var i = 0; i < all.Length; ++i)
                {
                    if (progress < all[i].To)
                    {
                        index = i;
                        break;
                    }
                }
            }
            // that's why we pass it as ref so we can assign it
            last = all[index];
            return last;
        }
    }
}