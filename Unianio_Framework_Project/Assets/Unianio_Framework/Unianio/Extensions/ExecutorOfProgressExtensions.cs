using Unianio.Enums;
using Unianio.Moves;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class ExecutorOfProgressExtensions
    {
        public static void Apply(this IExecutorOfProgress[] paths, double x)
        {
            for (var i = 0; i < paths.Length; ++i)
            {
                paths[i].Apply(x);
            }
        }
        public static T WithID<T>(this T eop, int id) where T : IExecutorOfProgress
        {
            eop.Tag = id;
            return eop;
        }
        
    }
}