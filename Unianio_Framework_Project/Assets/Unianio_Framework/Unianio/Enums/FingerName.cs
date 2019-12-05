using System;

namespace Unianio.Enums
{
    [Flags]
    public enum FingerName : byte
    {
        NONE = 0,
        ANY = Byte.MaxValue, 
        Thumb = 1,
        Index = 2,
        Middle = 4,
        Ring = 8,
        Pinky = 16
    }
}