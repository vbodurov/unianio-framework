using System;
using System.Linq;

namespace Unianio.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetDefaultImplementationType(this Type type)
        {
            if (!type.IsInterface || !type.Name.StartsWith("I")) return null;
            var nameToFind = type.Name.Substring(1);
            if(!AssemblyReflector.TypeByName.TryGetValue(nameToFind, out var list))
            {
                return null;
            }
            return list.FirstOrDefault(t => 
                        type.IsAssignableFrom(t) && 
                        t.IsClass && !t.IsAbstract);
        }
    }
}