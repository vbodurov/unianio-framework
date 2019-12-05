using System;
using System.Collections.Generic;
using System.Linq;
using Unianio.Animations;
using Unianio.RSG;
using Unianio.Static;

namespace Unianio.Extensions
{
    public static class EnumerableExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionaryOverridingSame<TKey, TValue>(
            this IEnumerable<TValue> enumerable, Func<TValue, TKey> getKey, IEqualityComparer<TKey> comparer = null)
        {
            var dict = new Dictionary<TKey, TValue>(comparer ?? EqualityComparer<TKey>.Default);
            if (enumerable == null) return dict;
            foreach (var e in enumerable)
            {
                var key = getKey(e);
                dict[key] = e;
            }
            return dict;
        }
        public static Dictionary<TKey, List<TValue>> ToDictionaryOfLists<TKey, TValue>(
            this IEnumerable<TValue> enumerable, Func<TValue, TKey> getKey, IEqualityComparer<TKey> comparer = null)
        {
            var dict = new Dictionary<TKey, List<TValue>>(comparer ?? EqualityComparer<TKey>.Default);
            foreach (var e in enumerable)
            {
                var key = getKey(e);
                if (!dict.TryGetValue(key, out var list))
                {
                    list = new List<TValue> { e };
                    dict[key] = list;
                }
                else
                {
                    list.Add(e);
                }
            }
            return dict;
        }
        public static IList<T> PopulateList<T>(this IEnumerable<T> enumerable, IList<T> list)
        {
            foreach (var e in enumerable)
            {
                list.Add(e);
            }
            return list;
        }
        public static void WhenAllAnimationsFinish(this IEnumerable<IPromise<IAnimation>> promises, Action<IEnumerable<IAnimation>> action)
        {
            Promise<IAnimation>.All(promises).Then(action);
        }
        public static HashSet<T> AddRange<T>(this HashSet<T> set, IEnumerable<T> toAdd)
        {
            foreach (var e in toAdd)
            {
                set.Add(e);
            }
            return set;
        }
        public static string JoinAsString<T>(this IEnumerable<T> enumerable, string limiter)
        {
            if (enumerable == null) return null;
            return string.Join(limiter, enumerable.Select(e => e.ToString()).ToArray());
        }
        public static T Next<T>(this IList<T> list, ref int index)
        {
            if (list == null || list.Count == 0) return default(T);
            ++index;
            if (index < 0 || index >= list.Count) index = 0;
            return list[index];
        }
        public static T Random<T>(this IList<T> list)
        {
            return list == null || list.Count == 0 ? default(T) : fun.rndOf(list);
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(enumerable, comparer);
        }
        public static HashSet<TOut> ToHashSet<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn,TOut> selector)
        {
            return new HashSet<TOut>(enumerable.Select<TIn,TOut>(selector));
        }
        public static HashSet<TOut> ToHashSet<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> selector, IEqualityComparer<TOut> comparer)
        {
            return new HashSet<TOut>(enumerable.Select<TIn, TOut>(selector), comparer);
        }
        public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> enumerable, T first)
        {
            yield return first;
            foreach (var e in enumerable)
                yield return e;
        }
        public static IEnumerable<T> AddLast<T>(this IEnumerable<T> enumerable, T last)
        {
            foreach (var e in enumerable)
                yield return e;
            yield return last;
        }
        public static IEnumerable<Tuple<T,T>> Pair<T>(this IEnumerable<T> enumerable)
        {
            using (var en = enumerable.GetEnumerator())
            {
                T previous = default(T);
                var isFirst = true;
                while (en.MoveNext())
                {
                    var current = en.Current;

                    if (isFirst) isFirst = false;
                    else yield return Tuple.Create(previous, current);

                    previous = current;
                }
            }
        }
        public static IEnumerable<T> Each<T>(this IEnumerable<T> col, Action<T> act)
        {
            if (col == null || act == null) return col;
            foreach (var e in col)
            {
                act(e);
            }
            return col;
        }
        public static T[] EachOf<T>(this T[] arr, Action<T> act)
        {
            if (arr == null || act == null) return arr;
            for (var i = 0; i < arr.Length; i++) act(arr[i]);  
            return arr;
        }
        public static T[] EachOf<T>(this T[] arr, Action<T, int> act)
        {
            if (arr == null || act == null) return arr;
            for (var i = 0; i < arr.Length; i++) act(arr[i], i);
            return arr;
        }
    }
}