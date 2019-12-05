using Unianio.Enums;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class BodySideExtensions
    {
        public static ControllerSide ToController(this BodySide side)
        {
            return side == BodySide.Right ? ControllerSide.Right : ControllerSide.Left;
        }
        public static BodySide Flip(this BodySide side)
        {
            return side == BodySide.Right ? BodySide.Left : BodySide.Right;
        }
        public static string ToLetter(this BodySide side)
        {
            return side == BodySide.Right ? "R" : "L";
        }
        public static BodySide GetOtherSide(this BodySide side)
        {
            if (side == BodySide.None) return side;
            return side == BodySide.Right ? BodySide.Left : BodySide.Right;
        }
        public static bool IsLeft(this BodySide side) => side == BodySide.Left;
        public static bool IsRight(this BodySide side) => side == BodySide.Right;
        public static Vector3 ToNormal(this BodySide side) 
            => side == BodySide.Left 
                ? v3.lt 
                : side == BodySide.Right 
                    ? v3.rt
                    : v3.zero;
        public static T MapRightLeft<T>(this BodySide side, T right, T left) => side == BodySide.Right ? right : left;
    }
}