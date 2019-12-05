using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class resolution
        {
            /// <param name="staticP0">static capsule lower center</param>
            /// <param name="staticP1">static capsule upper center</param>
            /// <param name="staticRadius">static capsule radius</param>
            /// <param name="prevP0">previous dynamic capsule lower center</param>
            /// <param name="prevP1">previous dynamic capsule upper center</param>
            /// <param name="nextP0">next dynamic capsule lower center</param>
            /// <param name="nextP1">next dynamic capsule upper center</param>
            /// <param name="nextUp">next dynamic capsule up vector</param>
            /// <param name="dynamicRadius">dynamic capsule radius</param>
            /// <param name="resolvedPos">resolved dynamic capsule lowest tip (not lowest center!)</param>
            /// <param name="resolvedRot"></param>
            /// <returns></returns>
            public static bool BetweenCapsules(
                in Vector3 staticP0, in Vector3 staticP1, double staticRadius,
                in Vector3 prevP0, in Vector3 prevP1,
                in Vector3 nextP0, in Vector3 nextP1, in Vector3 nextUp, double dynamicRadius,
                out Vector3 resolvedPos, out Quaternion resolvedRot)
            {
                var staRad = (float)staticRadius;
                var dynRad = (float)dynamicRadius;
                var minDist = staRad + dynRad;

                var hasNext = intersection.BetweenCapsules(in staticP0, in staticP1, staRad, in nextP0, in nextP1, dynRad);

                var prevDir = (prevP1 - prevP0).normalized;
                var prevRadVec = prevDir * dynRad;
                var prevTip = prevP1 + prevRadVec;
                var prevPos = prevP0 - prevRadVec;

                var nextDir = (nextP1 - nextP0).normalized;
                var nextRadVec = nextDir * dynRad;
                var nextTip = nextP1 + nextRadVec;
                var nextPos = nextP0 - nextRadVec;

                var t1 = nextTip;
                var t2 = nextPos;
                var t3 = prevTip;
                Vector3 triCol;
                var hasTri = intersection.BetweenTriangleAndLineSegment(in t1, in t2, in t3, in staticP0, in staticP1, out triCol);

                if (!hasTri)
                {
                    t1 = nextPos;
                    t2 = prevPos;
                    t3 = prevTip;
                    hasTri = intersection.BetweenTriangleAndLineSegment(in t1, in t2, in t3, in staticP0, in staticP1, out triCol);
                }

                if (!hasNext && !hasTri)
                {
                    // there is no collision so prev can become next
                    resolvedPos = nextPos;
                    resolvedRot = Quaternion.LookRotation(nextDir, nextUp);
                    return false;
                }

                Vector3 onStat, onPrev;
                point.ClosestOnTwoLineSegments(in staticP0, in staticP1, in prevPos, in prevTip, out onStat, out onPrev);

                resolvedPos = nextPos;
                var statDir = (staticP1 - staticP0).normalized;

                Vector3 target;
                if (point.IsAbovePlane(in onPrev, in statDir, in staticP1))
                {
                    target = prevP1 + (onPrev - prevP1).normalized * minDist;
                    resolvedRot = Quaternion.LookRotation((target - resolvedPos).normalized, nextUp);
                    return true;
                }
                var minStatDir = -statDir;
                if (point.IsAbovePlane(in onPrev, in minStatDir, in staticP0))
                {
                    target = prevP0 + (onPrev - prevP0).normalized * minDist;
                    resolvedRot = Quaternion.LookRotation((target - resolvedPos).normalized, nextUp);
                    return true;
                }
                var backDir = (onPrev - onStat).normalized;
                var radians = angle.BetweenVectorsUnSignedInRadians(in backDir, in statDir);
                target = onStat + backDir * abs(minDist / sin(radians));
                resolvedRot = Quaternion.LookRotation((target - resolvedPos).normalized, nextUp);
                return true;
            }
        }

    }
}