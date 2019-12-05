using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unianio.Services;

namespace Unianio
{
    public static class AssemblyReflector
    {
        public static readonly Type[] AllTypes = GetAllTypes();

        static Type[] GetAllTypes()
        {
            var namesHash = new HashSet<string>(UnianioConfig.Assemblies, StringComparer.InvariantCultureIgnoreCase);
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => namesHash.Contains(a.GetName().Name))
                .SelectMany(a => a.GetTypes())
                .ToArray();
        }

        public static readonly IDictionary<string, IList<Type>> TypeByName = GenerateCache();
        static IDictionary<string, IList<Type>> GenerateCache()
        {
            var typeByName = new Dictionary<string, IList<Type>>();
            foreach (var t in AllTypes)
            {
                if (!typeByName.TryGetValue(t.Name, out var list))
                {
                    list = new List<Type>();
                    typeByName[t.Name] = list;
                }
                list.Add(t);
            }
            return typeByName;
        }
    }
}