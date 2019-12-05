using System;
using Unianio.Enums;

namespace Unianio.Extensions
{
    public static class MoveSpeedExtensions
    {
        public static double ToFocus(this MoveSpeed speed)
        {
            switch (speed)
            {
                case MoveSpeed.None: return 0;
                case MoveSpeed.SuperSlow: return -1;
                case MoveSpeed.Slow: return -0.5;
                case MoveSpeed.Average: return 0;
                case MoveSpeed.Fast: return 0.5;
                case MoveSpeed.SuperFast: return 1;
                default: throw new ArgumentOutOfRangeException("speed", speed, null);
            }
        }
    }
}