using System;

namespace Unianio
{
    public class NamedDependencyAttribute : Attribute
    {
        public string Name { get; }
        public NamedDependencyAttribute(string name) => Name = name;
    }
}