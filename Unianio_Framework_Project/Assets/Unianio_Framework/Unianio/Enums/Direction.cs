using System;

namespace Unianio.Enums
{
    [Flags]
    public enum Direction : byte
    {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Forward = 1 << 4,
        Back = 1 << 5,
    }
}