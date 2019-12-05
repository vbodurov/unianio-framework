using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class circle2d
        {
            public static bool Overlap(
                in Vector3 circleCenter1, double circleRadius1,
                in Vector3 circleCenter2, double circleRadius2)
            {
                return distance.BetweenIgnoreY(in circleCenter1, in circleCenter2) <= (circleRadius1 + circleRadius2);
            }

            public static void Join(
                in Vector3 circleCenter1, double circleRadius1,
                in Vector3 circleCenter2, double circleRadius2,
                out Vector3 combinedCirCen, out float combinedCirRad)
            {
                var dist = distance.BetweenIgnoreY(in circleCenter1, in circleCenter2);
                // circle 2 is inside circle 1
                if ((dist + circleRadius2) <= circleRadius1)
                {
                    combinedCirRad = (float)circleRadius1;
                    combinedCirCen = circleCenter1;
                    return;
                }
                // circle 1 is inside circle 2
                if ((dist + circleRadius1) <= circleRadius2)
                {
                    combinedCirRad = (float)circleRadius2;
                    combinedCirCen = circleCenter2;
                    return;
                }

                combinedCirRad = (float)((circleRadius1 + circleRadius2 + dist) / 2.0);
                var circ1to2 = (circleCenter2 - circleCenter1).ToHorzUnit();
                combinedCirCen = circleCenter1 + circ1to2 * (float)-circleRadius1 + circ1to2 * combinedCirRad;
            }

            public static bool JoinIfOverlap(
                in Vector3 circleCenter1, double circleRadius1,
                in Vector3 circleCenter2, double circleRadius2,
                out Vector3 combinedCirCen, out float combinedCirRad)
            {
                var dist = distance.BetweenIgnoreY(in circleCenter1, in circleCenter2);
                // they don't overlap
                if (dist > (circleRadius1 + circleRadius2))
                {
                    combinedCirCen = Vector3.zero;
                    combinedCirRad = 0;
                    return false;
                }
                // circle 2 is inside circle 1
                if ((dist + circleRadius2) <= circleRadius1)
                {
                    combinedCirRad = (float)circleRadius1;
                    combinedCirCen = circleCenter1;
                    return true;
                }
                // circle 1 is inside circle 2
                if ((dist + circleRadius1) <= circleRadius2)
                {
                    combinedCirRad = (float)circleRadius2;
                    combinedCirCen = circleCenter2;
                    return true;
                }

                combinedCirRad = (float)((circleRadius1 + circleRadius2 + dist) / 2.0);
                var circ1to2 = (circleCenter2 - circleCenter1).ToHorzUnit();
                combinedCirCen = circleCenter1 + circ1to2 * (float)-circleRadius1 + circ1to2 * combinedCirRad;
                return true;
            }
        }

    }
}