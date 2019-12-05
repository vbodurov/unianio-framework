using System;
using System.Collections.Generic;

namespace Unianio.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list) 
        {
            if (list == null || list.Count <= 1) return list;

            var rand = new Random(list.GetHashCode());
            for (var i = 0; i < list.Count; ++i)
            {
                var replaceIndex = rand.Next(0, list.Count);
                if (i == replaceIndex) continue;
                // swap
                var temp = list[replaceIndex];
                list[replaceIndex] = list[i];
                list[i] = temp;
            }
            return list;
        }
    }
}