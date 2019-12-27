using System;

namespace Unianio.Enums
{
    [Flags]
    public enum MoveType
    {
        Line = 1,
        QuadraticBezier = 2,
        CubicBezier = 4,
        AllBezier = QuadraticBezier | CubicBezier,
        Scalar = 8,
        Rotate = 16
    }
}