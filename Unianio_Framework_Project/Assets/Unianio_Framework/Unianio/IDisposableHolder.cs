using System;

namespace Unianio
{
    public interface IDisposableHolder : IDisposable
    {
        bool IsDisposed { get; }
    }
}