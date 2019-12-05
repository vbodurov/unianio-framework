using System;
using System.Collections.Generic;

namespace Unianio.Extensions
{
    public static class GlobalExtensions
    {
        public static int hc<T>(this T obj) 
        {
            return obj.GetHashCode();
        }
        public static T1 AddTo<T1, T2>(this T1 obj, IList<T2> list) 
            where T1 : class, T2 
            where T2 : class
        {
            list.Add(obj);
            return obj;
        }
        public static TValue AddTo<TKey, TValue>(this TValue obj, Func<TValue, TKey> getKey, IDictionary<TKey, TValue> dict)
        {
            dict[getKey(obj)] = obj;
            return obj;
        }
        public static void AddTo<TKey, TValue>(this IEnumerable<TValue> collection, Func<TValue, TKey> getKey, IDictionary<TKey, TValue> dict)
        {
            foreach (var obj in collection)
            {
                dict[getKey(obj)] = obj;
            }
        }
        public static T Pipe<T>(this T obj, Action<T> process) where T : class
        {
            process(obj);
            return obj;
        }
        public static T As<T>(this object obj) where T : class 
        {
            return obj as T;
        }
        public static bool ConditionIfType<T>(this object obj, Func<T,bool> condition, bool defaultResult = false) where T : class 
        {
            var t = obj as T;
            if (t == null) return defaultResult;
            return condition(t);
        }
        public static TOut Case<TIn,TOut>(
            this TIn inVal, 
            TIn in1, TOut out1, 
            TOut elseOut) 
        {
            var comparer = EqualityComparer<TIn>.Default;

            return comparer.Equals(inVal,in1) 
                    ? out1 
                    : elseOut;
        }
        public static TOut Case<TIn,TOut>(
            this TIn inVal, 
            TIn in1, TOut out1, 
            TIn in2, TOut out2, 
            TOut elseOut) 
        {
            var comparer = EqualityComparer<TIn>.Default;

            return comparer.Equals(inVal,in1) 
                    ? out1 
                    : comparer.Equals(inVal,in2) 
                        ? out2 
                        : elseOut;
        }
        public static TOut Case<TIn,TOut>(
            this TIn inVal, 
            TIn in1, TOut out1, 
            TIn in2, TOut out2, 
            TIn in3, TOut out3, 
            TOut elseOut) 
        {
            var comparer = EqualityComparer<TIn>.Default;

            return comparer.Equals(inVal,in1) 
                    ? out1 
                    : comparer.Equals(inVal,in2) 
                        ? out2 
                        : comparer.Equals(inVal,in3) 
                            ? out3 
                            : elseOut;
        }
    }
}