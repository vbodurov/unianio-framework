using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unianio.Animations;
using Unianio.Animations.Common;
using Unianio.Extensions;
using Unianio.Services;
using UnityEngine;

namespace Unianio
{
    public abstract class SimpleBaseFactory : IGlobalFactory
    {
        readonly IGlobalFactory _factory;

        readonly IDictionary<Type,object> _singletonByType = new Dictionary<Type, object>(); 
        readonly IDictionary<Type,Func<object>> _factoryByType = new Dictionary<Type, Func<object>>();
        readonly IDictionary<Type,Type> _objectTypeByInterfaceTypeForSingleton = new Dictionary<Type,Type>();
        readonly IDictionary<Type,Type> _objectTypeByInterfaceTypeForFactory = new Dictionary<Type,Type>();

        readonly IDictionary<string, object> _namedSingletonByType = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        readonly IDictionary<string, Func<object>> _namedFactoryByType = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
        readonly IDictionary<string, Type> _namedObjectTypeByInterfaceTypeForSingleton = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        readonly IDictionary<string, Type> _namedObjectTypeByInterfaceTypeForFactory = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        
        protected SimpleBaseFactory()
        {
            _factory = this;
            RegisterInstance(_factory);
            WireAnimations();
        }

        void WireAnimations()
        {
            foreach (var t in AssemblyReflector.AllTypes)
            {
                if (typeof (IAnimation).IsAssignableFrom(t) && !t.IsAbstract)
                {
                    RegisterFactory(t, _factory.CreateFactory(t));
                }
            }
        }
        Func<object> IGlobalFactory.CreateFactory(Type type)
        {
            return () => Activator.CreateInstance(type, BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance, null, 
                                GetConstructorParametersForType(type), null);
        }
        void IGlobalFactory.SetInstance<T>(T instance)
        {
            RegisterInstance(instance);
        }
        void IGlobalFactory.SetInstance<T>(string name, T instance)
        {
            RegisterInstance(name, instance);
        }
        void IGlobalFactory.RemoveInstanceOfType(Type type)
        {
            UnregisterInstanceOfType(type);
        }
        void IGlobalFactory.RemoveInstanceOfType(string name, Type type)
        {
            UnregisterInstanceOfType(name, type);
        }
        object[] GetConstructorParametersForType(Type t)
        {
            var constructors = t.GetConstructors(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            if(constructors.Length == 0)
                throw new ArgumentException("Found no constructor for type "+t);
            if(constructors.Length > 1)
                throw new ArgumentException("Found multiple constructors for type "+t+" currently supported is only one constructor");
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();

            if(parameters.Length == 0) return new object[0];

            var pars = new List<object>();
            foreach (var p in parameters)
            {
                var attrs = p.GetCustomAttributes(typeof(NamedDependencyAttribute));
                string dependencyName = null;
                if (attrs.FirstOrDefault() is NamedDependencyAttribute attr)
                {
                    dependencyName = attr.Name;
                }

                var dependency = 
                    dependencyName == null 
                        ? _factory.Get(p.ParameterType)
                        : _factory.Get(dependencyName, p.ParameterType);
                if (dependency == null)
                {
                    throw new ArgumentException("Unknown type " + p.ParameterType + " needed for construction of " + t);
                }
                pars.Add(dependency);
            }
            return pars.ToArray();
        }

        T IGlobalFactory.Get<T>()
        {
            var obj = _factory.Get(typeof(T));
            return obj == null ? default(T) : (T) obj;
        }
        object IGlobalFactory.Get(Type type)
        {
            // frequently used objects:
            if (type == typeof(FuncAni)) return new FuncAni();
            if (type == typeof(WaitAni)) return new WaitAni();
            if (type == typeof(EndlessFuncAni)) return new EndlessFuncAni();
            if (type == typeof(IntervalAni)) return new IntervalAni();

            for (var i = 0; i < 2; ++i)
            {
                if (i > 0)
                {
                    // if type has not been registered
                    if(!_objectTypeByInterfaceTypeForSingleton.ContainsKey(type) 
                       &&
                       !_objectTypeByInterfaceTypeForFactory.ContainsKey(type))
                    {
                        var asSingleton = type.IsDefined(typeof(AsSingletonAttribute), false);
                        // if type is non abstract class
                        // then
                        // register it as factory 
                        // or if marked with AsSingletonAttribute
                        // register it as singleton
                        if (type.IsClass && !type.IsAbstract)
                        {
                            if (asSingleton) _objectTypeByInterfaceTypeForSingleton[type] = type;
                            else _objectTypeByInterfaceTypeForFactory[type] = type;
                        }
                        // or if type is interface
                        // and there is non abstract class with corresponding name
                        // (like: ISomething interface and Something non-abstract class)
                        // register it as factory 
                        // or if marked with AsSingletonAttribute
                        // register it as singleton
                        else if (type.IsInterface)
                        {
                            var typeInterface = type;
                            var typeImplementation = typeInterface.GetDefaultImplementationType();
                            if(typeImplementation != null)
                            {
                                if (asSingleton) _objectTypeByInterfaceTypeForSingleton[typeInterface] = typeImplementation;
                                else _objectTypeByInterfaceTypeForFactory[typeInterface] = typeImplementation;
                            }
                        }
                    }
                    

                    if (_objectTypeByInterfaceTypeForSingleton.TryGetValue(type, out var targetType))
                    {
                        RegisterInstance(type, _factory.CreateFactory(targetType)());
                    }
                    else if (_objectTypeByInterfaceTypeForFactory.TryGetValue(type, out targetType))
                    {
                        RegisterFactory(type, _factory.CreateFactory(targetType));
                    }
                    else
                    {
                        return null;
                    }
                }

                object obj;
                if (_singletonByType.TryGetValue(type, out obj))
                {
                    return obj;
                }
                Func<object> func;
                if (_factoryByType.TryGetValue(type, out func))
                {
                    return func();
                }
            }

            
            
            return null;
        }




        T IGlobalFactory.Get<T>(string name)
        {
            var obj = _factory.Get(name, typeof(T));
            return obj == null ? default(T) : (T)obj;
        }
        object IGlobalFactory.Get(string name, Type type)
        {
            // frequently used objects:
            var key = Key(name, type);

            for (var i = 0; i < 2; ++i)
            {
                if (i > 0)
                {
                    // if type has not been registered
                    if (!_namedObjectTypeByInterfaceTypeForSingleton.ContainsKey(key)
                       &&
                       !_namedObjectTypeByInterfaceTypeForFactory.ContainsKey(key))
                    {
                        var asSingleton = type.IsDefined(typeof(AsSingletonAttribute), false);
                        // if type is non abstract class
                        // then
                        // register it as factory 
                        // or if marked with AsSingletonAttribute
                        // register it as singleton
                        if (type.IsClass && !type.IsAbstract)
                        {
                            if (asSingleton) _namedObjectTypeByInterfaceTypeForSingleton[key] = type;
                            else _namedObjectTypeByInterfaceTypeForFactory[key] = type;
                        }
                        // or if type is interface
                        // and there is non abstract class with corresponding name
                        // (like: ISomething interface and Something non-abstract class)
                        // register it as factory 
                        // or if marked with AsSingletonAttribute
                        // register it as singleton
                        else if (type.IsInterface)
                        {
                            var typeInterface = type;
                            var typeImplementation = typeInterface.GetDefaultImplementationType();
                            if (typeImplementation != null)
                            {
                                if (asSingleton) _namedObjectTypeByInterfaceTypeForSingleton[key] = typeImplementation;
                                else _namedObjectTypeByInterfaceTypeForFactory[key] = typeImplementation;
                            }
                        }
                    }


                    if (_namedObjectTypeByInterfaceTypeForSingleton.TryGetValue(key, out var targetType))
                    {
                        RegisterInstance(name, type, _factory.CreateFactory(targetType)());
                    }
                    else if (_namedObjectTypeByInterfaceTypeForFactory.TryGetValue(key, out targetType))
                    {
                        RegisterFactory(name, type, _factory.CreateFactory(targetType));
                    }
                    else
                    {
                        return null;
                    }
                }

                if (_namedSingletonByType.TryGetValue(key, out var obj))
                {
                    return obj;
                }
                if (_namedFactoryByType.TryGetValue(key, out var func))
                {
                    return func();
                }
            }

            return null;
        }
        protected void UnregisterInstanceOfType(Type type)
        {
            _singletonByType.Remove(type);
        }
        protected void UnregisterInstanceOfType(string name, Type type)
        {
            _namedSingletonByType.Remove(Key(name, type));
        }
        protected T RegisterInstance<T>(T obj)
        {
            _singletonByType[typeof(T)] = obj;
            return obj;
        }
        protected object RegisterInstance(Type type, object obj)
        {
            _singletonByType[type] = obj;
            return obj;
        }

        protected T RegisterInstance<T>(string name, T obj)
        {
            _namedSingletonByType[Key<T>(name)] = obj;
            return obj;
        }
        protected object RegisterInstance(string name, Type type, object obj)
        {
            _namedSingletonByType[Key(name, type)] = obj;
            return obj;
        }

        protected void RegisterSingletonType<TConcrete>() { RegisterSingletonType(typeof(TConcrete), typeof(TConcrete)); }
        protected void RegisterSingletonType<TInterface, TConcrete>() where TConcrete : TInterface { RegisterSingletonType(typeof(TInterface), typeof(TConcrete)); }
        protected void RegisterSingletonType(Type interfaceType, Type objectType)
        {
            if(objectType.IsAbstract || objectType.IsInterface)
                throw new ArgumentException("Expected concrete type "+objectType);
            _objectTypeByInterfaceTypeForSingleton[interfaceType] = objectType;
        }


        protected void RegisterSingletonType<TConcrete>(string name) { RegisterSingletonType(name, typeof(TConcrete), typeof(TConcrete)); }
        protected void RegisterSingletonType<TInterface, TConcrete>(string name) where TConcrete : TInterface { RegisterSingletonType(name, typeof(TInterface), typeof(TConcrete)); }
        protected void RegisterSingletonType(string name, Type interfaceType, Type objectType)
        {
            if (objectType.IsAbstract || objectType.IsInterface)
                throw new ArgumentException("Expected concrete type " + objectType);
            _namedObjectTypeByInterfaceTypeForSingleton[Key(name, interfaceType)] = objectType;
        }

        protected void RegisterFactoryType<TConcrete>()  { RegisterFactoryType(typeof(TConcrete), typeof(TConcrete)); }
        protected void RegisterFactoryType<TInterface, TConcrete>() where TConcrete : TInterface { RegisterFactoryType(typeof(TInterface), typeof(TConcrete)); }
        protected void RegisterFactoryType(Type interfaceType, Type objectType)
        {
            if(objectType.IsAbstract || objectType.IsInterface) throw new ArgumentException("Expected concrete type "+objectType);
             _objectTypeByInterfaceTypeForFactory[interfaceType] = objectType;
        }

        protected void RegisterFactoryType<TConcrete>(string name) { RegisterFactoryType(name, typeof(TConcrete), typeof(TConcrete)); }
        protected void RegisterFactoryType<TInterface, TConcrete>(string name) where TConcrete : TInterface { RegisterFactoryType(name, typeof(TInterface), typeof(TConcrete)); }
        protected void RegisterFactoryType(string name, Type interfaceType, Type objectType)
        {
            if (objectType.IsAbstract || objectType.IsInterface) throw new ArgumentException($"Expected concrete type {objectType} with name {name}");
            _namedObjectTypeByInterfaceTypeForFactory[Key(name, interfaceType)] = objectType;
        }

        protected void RegisterFactory(Type type, Func<object> func)
        {
             _factoryByType[type] = func;
        }
        protected void RegisterFactory<T>(Func<T> func)
        {
             _factoryByType[typeof(T)] = () => func();
        }

        protected void RegisterFactory(string name, Type type, Func<object> func)
        {
            _namedFactoryByType[Key(name, type)] = func;
        }
        protected void RegisterFactory<T>(string name, Func<T> func)
        {
            _namedFactoryByType[Key<T>(name)] = () => func();
        }

        string Key(string name, Type type) => type.FullName + "_" + name;
        string Key<T>(string name) => typeof(T).FullName + "_" + name;
    }
}