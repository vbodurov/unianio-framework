using System;
using System.Collections.Generic;

namespace Unianio.Extensions
{
    public static class DictionaryExtensions
    {
        public static void PushInStack<TKey, TValue>(this IDictionary<TKey, Stack<TValue>> dict, TKey key, TValue value)
        {
            if (!dict.TryGetValue(key, out var stack))
            {
                stack = new Stack<TValue>();
                dict[key] = stack;
            }
            if(!stack.Contains(value)) stack.Push(value);
        }


        public static IDictionary<int, string> AddOrAppend(
            this IDictionary<int, string> dict, int key, string str, char limiter = ';')
        {
            if (dict == null) return dict;
            if (dict.TryGetValue(key, out var value))
            {
                dict[key] = dict[key] + limiter + str;
            }
            else
            {
                dict[key] = str;
            }
            return dict;
        }
        public static int IndexOfKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        where TKey : IEquatable<TKey>
        {
            var i = 0;
            foreach (var kvp in dict)
            {
                if(kvp.Key.Equals(key))
                {
                    return i;
                }
                ++i;
            }
            return -1;
        }
        public static TValue GetFirst<TKey, TValue>(this IDictionary<TKey, TValue> dict, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                if (dict.TryGetValue(key, out var value)) return value;
            }
            return default(TValue);
        }
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out var value) ? value : default(TValue);
        }
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out var value) ? value : default(TValue);
        }
    }
}