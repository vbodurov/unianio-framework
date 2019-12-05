using System;
using Unianio.Enums;

namespace Unianio.Extensions
{
    public static class ControllerSideExtensions
    {
        public static bool ContainsAny(this ControllerSide side, ControllerSide some)
        {
            return (side & some) > 0;
        }
        public static bool IsLt(this ControllerSide side) => side == ControllerSide.Left;
        public static bool IsRt(this ControllerSide side) => side == ControllerSide.Right;
        public static BodySide ToBody(this ControllerSide side)
        {
            switch (side)
            {
                case ControllerSide.Right:
                    return BodySide.Right;
                case ControllerSide.Left:
                    return BodySide.Left;
                case ControllerSide.None:
                    return BodySide.None;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
        }
    }
}