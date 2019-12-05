using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class inverseKinematics
        {
            public static void Finger(
                Vector3 oriPo, Vector3 oriFw, Vector3 oriUp, Vector3 tarPo,
                Vector3[] points, // 4 points (including target)
                float[] lengths, // 3 floats
                out Vector3 join1, out Vector3 join2, out Vector3 chainUpDir)
            {
                if (points == null || points.Length != 4) throw new ArgumentException("Expected 4 points");
                if (lengths == null || lengths.Length != 3) throw new ArgumentException("Expected 3 lengths");

                var toTargDir = (tarPo - oriPo).ToUnit(out var distToTarg);
                var lenAll = (float)(lengths[0] + lengths[1] + lengths[2]);
                if (distToTarg > lenAll)
                {
                    tarPo = oriPo + toTargDir * lenAll;
                    distToTarg = lenAll;
                }
                var relDist01 = distToTarg / lenAll;
                GetPlacementDir(in oriFw, in oriUp, in toTargDir, out chainUpDir);

                var iniDir = chainUpDir.RotateTowards(toTargDir, relDist01.Clamp01().From01ToRange(40, 55));

                //dbg.DrawLine(points.hc(), oriPo, oriPo+ chainUpDir, Color.green);
                //dbg.DrawLine(points.hc()+1,oriPo, oriPo+ iniDir, Color.magenta);
                points[0] = oriPo;
                points[1] = oriPo + iniDir * lengths[0];
                points[2] = points[1] + iniDir * lengths[1];
                points[3] = points[2] + iniDir * lengths[2];

                FABRIK(tarPo, points, lengths, 4, (int)relDist01.From01ToRange(3, 50));

                join1 = points[1];
                join2 = points[2];
            }
            static void GetPlacementDir(in Vector3 oriFw, in Vector3 oriUp, in Vector3 toTargDir, out Vector3 placementDir)
            {
                /*Vector3 oriRt;
                fun.vector.GetNormal(in oriFw, in oriUp, out oriRt);
                var dp01 = dot(in oriUp, in toTargDir).Abs();
                var rhsDir = slerp(in oriUp, in oriFw, dp01);
                Vector3 planeNorm;
                fun.vector.GetNormal(in toTargDir, in rhsDir, out planeNorm);
                fun.vector.EnsurePointSameDirAs(in planeNorm, in oriRt, out planeNorm);
                fun.vector.GetNormal(in planeNorm, in toTargDir, out placementDir);*/
                Vector3 oriRt;
                fun.vector.GetNormal(in oriFw, in oriUp, out oriRt);
                Vector3 toTargOnRtDir;
                fun.vector.ProjectOnPlane(in toTargDir, in oriRt, out toTargOnRtDir);
                fun.vector.GetNormal(in oriRt, in toTargOnRtDir, out placementDir);
            }

            // fun.invserseKinematics.ThreeJoinOnVertPlane(in oriPo, in oriFw, in oriUp, in tarPo, len1, len2, out j0, out j1);
            public static void ThreeJoinOnVertPlane(
                Vector3 oriPo, Vector3 oriFw, Vector3 oriUp, Vector3 tarPo,
                double len0, double len1, out Vector3 join1, out Vector3 join2)
            {
                float distToTarg;
                var toTargDir = (tarPo - oriPo).ToUnit(out distToTarg);

                if (dot(in toTargDir, in oriUp).Abs() < 0.999)
                {
                    Vector3 planeNormToBe;
                    vector.GetNormal(in toTargDir, in oriUp, out planeNormToBe);
                    vector.ProjectOnPlane(in oriFw, in planeNormToBe, out oriFw);
                }

                if (point.IsBelowPlane(in tarPo, in oriFw, in oriPo))
                {
                    oriUp *= -1;
                    oriFw *= -1;
                }

                var lenAll = (float)(len0 + len1 + len0);
                if (distToTarg >= lenAll)
                {
                    join1 = oriPo + toTargDir * (float)len0;
                    join2 = join1 + toTargDir * (float)len1;
                    return;
                }

                Vector3 realUp;
                vector.GetRealUp(in toTargDir, in oriUp, in oriFw, out realUp);

                var halfRemaining = ((distToTarg - len1) / 2f);

                var deg = Math.Acos(halfRemaining / len0) * RTD;

                var toJ0 = toTargDir.RotateTowardsCanOvershoot(in realUp, deg);
                join1 = oriPo + toJ0 * (float)len0;

                var toJ1 = (-toTargDir).RotateTowardsCanOvershoot(in realUp, deg);
                join2 = tarPo + toJ1 * (float)len0;
            }

            public static void TwoJoinsOnVertPlane(Vector3 oriPo, Vector3 oriFw, Vector3 oriUp, Vector3 tarPo, double len1, double len2, out Vector3 join)
            {
                float distToTarg;
                var toTargDir = (tarPo - oriPo).ToUnit(out distToTarg);
                var lenAll = (float)(len1 + len2);
                if (distToTarg >= lenAll)
                {
                    join = oriPo + toTargDir * (float)len1;
                    return;
                }
                Vector3 realUp;
                toTargDir.GetRealUp(in oriUp, in oriFw, out realUp);
                Vector3 planeNor;
                vector.GetNormal(in toTargDir, in realUp, out planeNor);

                var degNearLen1 = triangle.GetDegreesBySides(len1, distToTarg, len2);
                var toJoin = toTargDir.RotateTowardsCanOvershoot(in realUp, degNearLen1);
                join = oriPo + toJoin * (float)len1;
            }


            /*

            points = "Thigh", "Shin", "Foot"
            initialLimit = 1 repetitions = 1
            IN:2(Foot) FW: 1(Shin)| BK: 1(Shin) 2(Foot)
            initialLimit = 1 repetitions = 2
                IN:2(Foot) FW: 1(Shin)| BK: 1(Shin) 2(Foot)
                IN:2(Foot) FW: 1(Shin)| BK: 1(Shin) 2(Foot)

            points = "AbdomenLower", "AbdomenUpper", "ChestLower", "ChestUpper", "NeckLower"
            initialLimit = 1 repetitions = 1
                IN:4(NeckLower) FW:3(ChestUpper)| BK:3(ChestUpper) 4(NeckLower)
                IN:4(NeckLower) FW:3(ChestUpper) 2(ChestLower)| BK:2(ChestLower) 3(ChestUpper) 4(NeckLower)
                IN:4(NeckLower) FW:3(ChestUpper) 2(ChestLower) 1(AbdomenUpper)| BK:1(AbdomenUpper) 2(ChestLower) 3(ChestUpper) 4(NeckLower)             

            initialLimit = 3 repetitions = 1
                IN:4(NeckLower) FW:3(ChestUpper) 2(ChestLower) 1(AbdomenUpper)| BK:1(AbdomenUpper) 2(ChestLower) 3(ChestUpper) 4(NeckLower)

            initialLimit = 3 repetitions = 2
                IN:4(NeckLower) FW:3(ChestUpper) 2(ChestLower) 1(AbdomenUpper)| BK:1(AbdomenUpper) 2(ChestLower) 3(ChestUpper) 4(NeckLower)
                IN:4(NeckLower) FW:3(ChestUpper) 2(ChestLower) 1(AbdomenUpper)| BK:1(AbdomenUpper) 2(ChestLower) 3(ChestUpper) 4(NeckLower)
            */
            public static void FABRIK(
                in Vector3 handlePos,
                Vector3[] points,
                float[] lengths,
                int initialLimit = 1,
                int repetitions = 1)
            {
                var lastIndex = points.Length - 1;

                for (var j = 0; j < repetitions; ++j)
                {
                    var limitIndex = (int)max(lastIndex - initialLimit, 1);
                    while (limitIndex >= 1)
                    {
                        points[lastIndex] = handlePos;
                        // forward (neck) to (spine0)
                        for (var i = lastIndex; i > limitIndex; --i)
                        {
                            var len = lengths[i - 1];

                            var dir = (points[i - 1] - points[i]).normalized;
                            points[i - 1] = points[i] + dir * len;
                        }
                        // and backward (spine0) to J4 (neck)
                        for (var i = limitIndex - 1; i < lastIndex; ++i)
                        {
                            var len = lengths[i];

                            var dir = (points[i + 1] - points[i]).normalized;
                            points[i + 1] = points[i] + dir * len;
                        }

                        const double MinDist = 0.001 * 0.001;
                        if (distanceSquared.BetweenAsDouble(in points[lastIndex], in handlePos) < MinDist)
                        {
                            return;
                        }

                        limitIndex--;
                    }
                }
            }

            public static void Legacy_FABRIK(
                Vector3 handlePos,
                Vector3[] points,
                float[] lengths,
                int numberIterations,
                Action<int, int, Vector3[], float[]> onAfterAssignForward = null,
                Action<int, int, Vector3[], float[]> onAfterAssignBack = null)
            {
                var lastIndex = points.Length - 1;
                for (var n = 0; n < numberIterations; ++n)
                {
                    points[points.Length - 1] = handlePos;
                    // forward J4 (head) to J0 (spine1)
                    for (var i = lastIndex; i > 1; --i)
                    {
                        var len = lengths[i - 1];

                        var dir = (points[i - 1] - points[i]).normalized;
                        points[i - 1] = points[i] + dir * len;

                        onAfterAssignForward?.Invoke(i, n, points, lengths);
                    }
                    // and backward (J0 (spine1) to J4 (head))
                    for (var i = 0; i < lastIndex; ++i)
                    {
                        var len = lengths[i];

                        var dir = (points[i + 1] - points[i]).normalized;
                        points[i + 1] = points[i] + dir * len;

                        onAfterAssignBack?.Invoke(i, n, points, lengths);
                    }
                }
            }

            public static bool ValidateEllipseConstraint(
                in Vector3 originalDir, in Vector3 normal, in Vector3 currentDir,
                double maxDegreesTowardsNormal, double maxDegreesAwayFromNormal,
                out float currentDeg,
                out float maxAllowed)
            {
                if (maxDegreesTowardsNormal < 0 || maxDegreesAwayFromNormal < 0)
                    throw new ArgumentException($"Constraint limits cannot be negative maxDegreesTowardsNormal={maxDegreesTowardsNormal} maxDegreesAwayFromNormal={maxDegreesAwayFromNormal}");
                currentDeg = angle.BetweenVectorsUnSignedInDegrees(in originalDir, in currentDir);
                if (currentDeg < 0.0001)
                {
                    maxAllowed = min(maxDegreesTowardsNormal, maxDegreesAwayFromNormal);
                    return true;
                }
                vector.ProjectOnPlane(in currentDir, in originalDir, out var currProj);
                currProj.Normalize();
                var realNormal = originalDir.GetRealUp(in normal);
                var ellipse01 = abs(angle.BetweenVectorsUnSignedInDegrees(in currProj, in realNormal) / 180f).Clamp01();
                maxAllowed = ellipse01.From01ToRange(maxDegreesTowardsNormal, maxDegreesAwayFromNormal);
                return maxAllowed >= currentDeg;
            }
        }

    }
}