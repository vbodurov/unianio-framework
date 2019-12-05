using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unianio.Collections;
using UnityEngine;

namespace Unianio.Services
{
    public interface IGlobalFactory
    {
        T Get<T>();
        object Get(Type type);
        T Get<T>(string name);
        object Get(string name, Type type);
        Func<object> CreateFactory(Type type);
        void SetInstance<T>(T instance);
        void SetInstance<T>(string name, T instance);
        void RemoveInstanceOfType(Type type);
        void RemoveInstanceOfType(string name, Type type);
    }
    public static class GlobalFactory
    {
        internal static readonly Type FactoryType = 
            Type.GetType(UnianioConfig.FactoryType, false) 
            ?? 
            typeof(UnianioBasicFactory);

        static readonly IGlobalFactory _default =
            (IGlobalFactory)Activator.CreateInstance(FactoryType,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
        public static IGlobalFactory Default => _default;
    }
}