using System;

namespace Unianio.Enums
{
    [Flags]
    public enum ControllerSide
    {
        None = 0,
        Right = 1 << 0,
        Left = 1 << 1
    }
}