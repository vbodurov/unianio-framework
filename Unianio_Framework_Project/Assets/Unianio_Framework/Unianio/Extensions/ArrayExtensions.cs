using System;
using System.Net.Mail;
using Unianio.Static;

namespace Unianio.Extensions
{
    public static class ArrayExtensions
    {
        public static bool ContainsValue<T>(this T[] array, T value)
        {
            if (array == null || array.Length == 0) return false;
            for (var i = 0; i < array.Length; i++)
            {
                if (object.Equals(array[i], value)) return true;
            }
            return false;
;       }
        public static T GetAtClampedIndex<T>(this T[] array, int index)
        {
            if (array == null || array.Length == 0) return default(T);
            return array[index.Clamp(0, array.Length - 1)];
        }
        public static T GetOrDefault<T>(this T[] array, int index, T defaultValue)
        {
            if (array == null || array.Length == 0) return defaultValue;
            if (index < 0 || index >= array.Length) return defaultValue;
            return array[index];
        }
        public static T GetOrDefault<T>(this T[] array, int index)
        {
            if (array == null || array.Length == 0) return default(T);
            if (index < 0 || index >= array.Length) return default(T);
            return array[index];
        }
        public static T GetByFoldedIndex<T>(this T[] array, int index)
        {
            if (array == null || array.Length == 0) return default(T);
            if (index < 0) index = index.Abs();
            return array[index % array.Length];
        }
        public static int FirstIndexOf<T>(this T[] array, Func<T,bool> find)
        {
            for(var i = 0; i < array.Length; ++i)
            {
                if (find(array[i])) return i;
            }
            return -1;
        }

        public static T RandomNewIndex<T>(this T[] array, ref int lastIndex)
        {
            var index = fun.random.Index(array.Length - 1);
            if (index >= lastIndex) ++index;
            lastIndex = index;
            return array[index];
        }

        public static T[] GetShuffledCopy<T>(this T[] array)
        {
            var newArray = new T[array.Length];
            Array.Copy(array, newArray,array.Length);

            if (newArray.Length <= 1) return newArray;


            var rand = new Random(array.GetHashCode());
            
            for (var i = 0; i < newArray.Length; ++i)
            {
                var replaceIndex = rand.Next(0, newArray.Length);
                if (i == replaceIndex) continue;
                // swap
                var temp = newArray[replaceIndex];
                newArray[replaceIndex] = newArray[i];
                newArray[i] = temp;
            }
            return newArray;
        }
        public static T[] Shuffle<T>(this T[] array)
        {
            if (array == null || array.Length <= 1) return array;

            var rand = new Random(array.GetHashCode());
            for (var i = 0; i < array.Length; ++i)
            {
                var replaceIndex = rand.Next(0, array.Length);
                if (i == replaceIndex) continue;
                // swap
                var temp = array[replaceIndex];
                array[replaceIndex] = array[i];
                array[i] = temp;
            }
            return array;
        }
    }
}