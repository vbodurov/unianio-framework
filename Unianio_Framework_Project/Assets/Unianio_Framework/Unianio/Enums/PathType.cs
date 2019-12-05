using System;

namespace Unianio.Enums
{
    [Flags]
    public enum PathType
    {
        Line = 1,
        QuadraticBezier = 2,
        CubicBezier = 4,
        AllBezier = QuadraticBezier | CubicBezier,
        SingleValue = 8,
        SphericalAnglePath = 16,
        CirclePath = 32,
        DynamicPath = 64,
        DynamicRotation = 128,
        Complex = 1024,
        Scalar = 2048
    }
}