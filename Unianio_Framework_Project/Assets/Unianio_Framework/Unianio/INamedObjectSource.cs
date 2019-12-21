using UnityEngine;

namespace Unianio
{
    public interface INamedObjectSource
    {
        bool IsNamed(string sourceName);
        object GetObject(string objectName);
    }
}