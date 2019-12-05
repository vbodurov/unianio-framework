using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class DirectionExtensions
    {
        public static Vector3 ToNormal(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return v3.up;
                case Direction.Right: return v3.rt;
                case Direction.Down: return v3.dn;
                case Direction.Left: return v3.lt;
                case Direction.Forward: return v3.fw;
                case Direction.Back: return v3.bk;
            }
            return v3.zero;
        }
    }
}