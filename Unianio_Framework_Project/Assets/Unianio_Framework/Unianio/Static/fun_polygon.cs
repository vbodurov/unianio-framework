using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class polygon
        {
            public static bool IsPointWithin(Vector2 point, Vector2[] points)
            {
                var result = false;
                int curr;
                var count = points.Length;
                var prevPoint = points[points.Length - 1];
                for (curr = 0; curr < count; curr++)
                {
                    var currPoint = points[curr];

                    if (((currPoint.y > point.y) != (prevPoint.y > point.y)) &&
                        (point.x < (prevPoint.x - currPoint.x) * (point.y - currPoint.y) / (prevPoint.y - currPoint.y) + currPoint.x))
                        result = !result;

                    prevPoint = currPoint;
                }
                return result;
            }
            public static bool IsPointWithinHorzPoly(Vector3 point, Vector3[] points)
            {
                var result = false;
                int curr;
                var count = points.Length;
                var prevPoint = points[points.Length - 1];
                for (curr = 0; curr < count; curr++)
                {
                    var currPoint = points[curr];

                    if (((currPoint.z > point.z) != (prevPoint.z > point.z)) &&
                        (point.x < (prevPoint.x - currPoint.x) * (point.z - currPoint.z) / (prevPoint.z - currPoint.z) + currPoint.x))
                        result = !result;

                    prevPoint = currPoint;
                }
                return result;
            }
        }

    }
}