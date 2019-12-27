using Unianio.Moves;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class bezierFunc
        {
            public static float GetY(double x, double bx, double by, double cx, double cy)
            {
                return (float)BezierFunc.GetY(x, bx, by, cx, cy);
            }
            public static float GetY(double x, double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy)
            {
                return (float)BezierFunc.GetY(x, ax, ay, bx, by, cx, cy, dx, dy);
            }
            public static Vector3 GetPoint2D(double x, Vector2 start, Vector2 control1, Vector2 control2, Vector2 end)
            {
                return BezierFunc.GetPointCubic2D(x, start, control1, control2, end);
            }
            public static Vector3 GetPoint(double x, Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
            {
                return BezierFunc.GetPointCubic(x, start, control1, control2, end);
            }
            public static Vector3 GetPoint(double x, in Vector3 start, in Vector3 control1, in Vector3 control2, in Vector3 end)
            {
                return BezierFunc.GetPointCubic(x, in start, in control1, in control2, in end);
            }

            public static Vector3 GetPoint2D(double x, Vector2 start, Vector2 control, Vector2 end)
            {
                return BezierFunc.GetPointQuadratic2D(x, start, control, end);
            }
            public static Vector3 GetPoint(double x, Vector3 start, Vector3 control, Vector3 end)
            {
                return BezierFunc.GetPointQuadratic(x, start, control, end);
            }
            public static Vector3 GetPoint(double x, in Vector3 start, in Vector3 control, in Vector3 end)
            {
                return BezierFunc.GetPointQuadratic(x, in start, in control, in end);
            }
        }

    }
}