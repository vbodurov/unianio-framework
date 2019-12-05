using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class point
        {
            const float epsilon = 0.0000001f;

            public static bool IsWithinAxisAlignedBoundingBox(in Vector3 point, in Vector3 bbCenter, in Vector3 bbSize)
            {
                var hasOverlapOnX = Math.Abs(point.x - bbCenter.x) <= (bbSize.x / 2f);
                var hasOverlapOnY = Math.Abs(point.y - bbCenter.y) <= (bbSize.y / 2f);
                var hasOverlapOnZ = Math.Abs(point.z - bbCenter.z) <= (bbSize.z / 2f);
                return hasOverlapOnX && hasOverlapOnY && hasOverlapOnZ;
            }

            public static bool IsWithinSphere(in Vector3 point, in Vector3 sphereCenter, double sphereRadius)
                => distance.Between(in point, in sphereCenter) <= sphereRadius;

            public static bool IsWithinCapsule(
                in Vector3 point, in Vector3 cpu, in Vector3 cpd, double capsuleRadius)
            {
                ProjectOnLineSegmentOrGetClosest(in point, in cpu, in cpd, out var closest);

                return distance.Between(in point, in closest) <= capsuleRadius;
            }

            public static Vector3 EnsurePointNotInCapsule(in Vector3 point, in Vector3 cpu, in Vector3 cpd, double capsuleRadius)
            {
                ProjectOnLineSegmentOrGetClosest(in point, in cpu, in cpd, out var closest);

                if (distance.Between(in point, in closest) < capsuleRadius)
                {
//dbg.DrawLine(1, point, closest, Color.black);
//dbg.DrawLine(2, closest, closest + closest.DirTo(in point) * (float)capsuleRadius, Color.cyan);
                    return closest + closest.DirTo(in point) * (float)capsuleRadius;
                }

                return point;
            }

            public static bool EnforceWithinSphere(
                in Vector3 currPoint, in Vector3 sphereCenter, double radius, out Vector3 newPoint)
            {
                var dir = sphereCenter.DirTo(in currPoint, out float dist);
                if (dist > radius)
                {
                    newPoint = sphereCenter + dir * (float)radius;
                    return true;
                }
                else
                {
                    newPoint = currPoint;
                    return false;
                }
            }
            public static bool EnforceAbovePlane(
                in Vector3 currPoint, in Vector3 normal, in Vector3 surface,
                out Vector3 newPoint)
            {
                if (IsAbovePlane(in currPoint, in normal, in surface))
                {
                    newPoint = currPoint;
                    return false;
                }
                else
                {
                    ProjectOnPlane(in currPoint, in normal, in surface, out newPoint);
                    return true;
                }
            }
            public static float DistanceToLine(in Vector3 point, in Vector3 line1, in Vector3 line2)
            {
                Vector3 proj;
                ProjectOnLine(in point, in line1, in line2, out proj);
                return distance.Between(in point, in proj);
            }
            public static void GetClosestBetweenLineAndDisk(
                in Vector3 diskNormal, in Vector3 diskCenter, double diskRadius,
                in Vector3 line1, in Vector3 line2, out Vector3 closestOnDisk, out Vector3 closestOnLine)
            {
                ProjectOnLine(in diskCenter, in line1, in line2, out closestOnLine);
                Vector3 diskCenOnLineProj;
                ProjectOnPlane(in closestOnLine, in diskNormal, in diskCenter, out diskCenOnLineProj);
                var distSqrToCen = distanceSquared.Between(in diskCenOnLineProj, in diskCenter);
                if (distSqrToCen < (diskRadius * diskRadius))
                {
                    closestOnDisk = diskCenOnLineProj;
                    return;
                }
                closestOnDisk = diskCenter.MoveTowards(in diskCenOnLineProj, diskRadius);
            }

            public static void ToLocal(in Vector3 worldPoint, in Quaternion worldRotation, in Vector3 worldPosition, out Vector3 localPoint)
            {
                localPoint = Quaternion.Inverse(worldRotation) * (worldPoint - worldPosition);
            }
            public static Vector3 ToLocal(in Vector3 worldPoint, in Quaternion worldRotation, in Vector3 worldPosition)
            {
                return Quaternion.Inverse(worldRotation) * (worldPoint - worldPosition);
            }


            public static void ToWorld(in Vector3 localPoint, in Quaternion worldRotation, in Vector3 worldPosition, out Vector3 worldPoint)
            {
                worldPoint = worldRotation * localPoint + worldPosition;
            }
            public static Vector3 ToWorld(in Vector3 localPoint, in Quaternion worldRotation, in Vector3 worldPosition)
            {
                return worldRotation * localPoint + worldPosition;
            }

            public static bool IsOn2DSegment(in Vector2 segStart, in Vector2 point, in Vector2 segEnd)
            {
                if (point.x <= max(segStart.x, segEnd.x) + epsilon &&
                    point.x >= min(segStart.x, segEnd.x) - epsilon &&
                    point.y <= max(segStart.y, segEnd.y) + epsilon &&
                    point.y >= min(segStart.y, segEnd.y) - epsilon)
                    return true;

                return false;
            }

            public static bool IsOnSegment(in Vector3 segStart, in Vector3 point, in Vector3 segEnd)
            {
                if (point.x <= max(segStart.x, segEnd.x) + epsilon &&
                    point.x >= min(segStart.x, segEnd.x) - epsilon &&
                    point.y <= max(segStart.y, segEnd.y) + epsilon &&
                    point.y >= min(segStart.y, segEnd.y) - epsilon &&
                    point.z <= max(segStart.z, segEnd.z) + epsilon &&
                    point.z >= min(segStart.z, segEnd.z) - epsilon)
                    return true;

                return false;
            }

            public static bool ClosestOnLineSegment(in Vector3 p, in Vector3 line1, in Vector3 line2, out Vector3 closest)
            {
                point.ProjectOnLine(in p, in line1, in line2, out closest);
                if (!IsOnSegment(in line1, in closest, in line2))
                {
                    var d1 = distanceSquared.Between(in line1, in closest);
                    var d2 = distanceSquared.Between(in line2, in closest);
                    closest = d1 < d2 ? line1 : line2;
                    return false;
                }
                return true;
            }
            public static void ClosestOnTwoLineSegments(
                in Vector3 line1p1, in Vector3 line1p2,
                in Vector3 line2p1, in Vector3 line2p2,
                out Vector3 closestPointLine1, out Vector3 closestPointLine2)
            {
                var dir1 = (line1p2 - line1p1).normalized;
                var dir2 = (line2p2 - line2p1).normalized;

                ClosestOnTwoLinesByPointAndDirection(
                        in line1p1, in dir1, in line2p1, in dir2,
                        out closestPointLine1, out closestPointLine2);

                var isClosest1Within = IsOnSegment(in line1p1, in closestPointLine1, in line1p2);
                var isClosest2Within = IsOnSegment(in line2p1, in closestPointLine2, in line2p2);

                // if the closest points are within finish here
                if (isClosest1Within && isClosest2Within)
                {
                    return;
                }

                // project each end
                Vector3 line1p1Proj;
                ProjectOnLineSegmentOrGetClosest(in line1p1, in line2p1, in line2p2, out line1p1Proj);
                var line1p1DistSqr = distanceSquared.Between(in line1p1, in line1p1Proj);
                var smallestDistSqr = line1p1DistSqr;
                byte smallestIndex = 0;

                Vector3 line1p2Proj;
                ProjectOnLineSegmentOrGetClosest(in line1p2, in line2p1, in line2p2, out line1p2Proj);
                var line1p2DistSqr = distanceSquared.Between(in line1p2, in line1p2Proj);
                if (line1p2DistSqr < smallestDistSqr)
                {
                    smallestDistSqr = line1p2DistSqr;
                    smallestIndex = 1;
                }

                Vector3 line2p1Proj;
                ProjectOnLineSegmentOrGetClosest(in line2p1, in line1p1, in line1p2, out line2p1Proj);
                var line2p1DistSqr = distanceSquared.Between(in line2p1, in line2p1Proj);
                if (line2p1DistSqr < smallestDistSqr)
                {
                    smallestDistSqr = line2p1DistSqr;
                    smallestIndex = 2;
                }

                Vector3 line2p2Proj;
                ProjectOnLineSegmentOrGetClosest(in line2p2, in line1p1, in line1p2, out line2p2Proj);
                var line2p2DistSqr = distanceSquared.Between(in line2p2, in line2p2Proj);
                if (line2p2DistSqr < smallestDistSqr)
                {
                    //                    smallestDistSqr = line2p2DistSqr;
                    smallestIndex = 3;
                }

                switch (smallestIndex)
                {
                    case 0:
                        closestPointLine1 = line1p1;
                        closestPointLine2 = line1p1Proj;
                        break;
                    case 1:
                        closestPointLine1 = line1p2;
                        closestPointLine2 = line1p2Proj;
                        break;
                    case 2:
                        closestPointLine1 = line2p1Proj;
                        closestPointLine2 = line2p1;
                        break;
                    case 3:
                        closestPointLine1 = line2p2Proj;
                        closestPointLine2 = line2p2;
                        break;
                    default:
                        throw new ArgumentException("Invalid Index");
                }
            }
            // if the lines are parallel then any point is closest so we return false
            public static bool ClosestOnTwoLinesByPointAndDirection(
                in Vector3 line1p1, in Vector3 line1Direction, in Vector3 line2p1, in Vector3 line2Direction,
                out Vector3 closestPointLine1, out Vector3 closestPointLine2)
            {

                var a = dot(in line1Direction, in line1Direction);
                var b = dot(in line1Direction, in line2Direction);
                var e = dot(in line2Direction, in line2Direction);

                var d = a * e - b * b;
                //lines are not parallel
                if (d > 0.00001 || d < -0.00001)
                {

                    var r = line1p1 - line2p1;
                    var c = dot(in line1Direction, in r);
                    var f = dot(in line2Direction, in r);

                    var s = (b * f - c * e) / d;
                    var t = (a * f - c * b) / d;

                    closestPointLine1 = line1p1 + line1Direction * s;
                    closestPointLine2 = line2p1 + line2Direction * t;
                    return true;
                }

                closestPointLine1 = line1p1;
                var line2p2 = line2p1 + line2Direction;
                point.ProjectOnLine(in line1p1, in line2p1, in line2p2, out closestPointLine2);
                return false;
            }

            public static bool ClosestOnTwoLinesByPoints(
                in Vector3 line1p1, in Vector3 line1p2, in Vector3 line2p1, in Vector3 line2p2,
                out Vector3 closestPointLine1, out Vector3 closestPointLine2)
            {
                var line1Direction = (line1p2 - line1p1).normalized;
                var line2Direction = (line2p2 - line2p1).normalized;

                return ClosestOnTwoLinesByPointAndDirection(
                    in line1p1, in line1Direction,
                    in line2p1, in line2Direction,
                    out closestPointLine1, out closestPointLine2);
            }



            public static bool IsLeftOfLine2D(in Vector2 point, in Vector2 linePoint1, in Vector2 linePoint2)
            {
                return ((linePoint2.x - linePoint1.x) * (point.y - linePoint1.y) - (linePoint2.y - linePoint1.y) * (point.x - linePoint1.x)) > 0;
            }
            public static bool IsLeftOfLine2D(Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
            {
                return ((linePoint2.x - linePoint1.x) * (point.y - linePoint1.y) - (linePoint2.y - linePoint1.y) * (point.x - linePoint1.x)) > 0;
            }
            public static bool IsAbovePlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                var vectorToPlane = (point - planePoint).normalized;
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance < 0;
            }
            public static bool IsAbovePlane(in Vector3 point, in Vector3 planeNormal)
            {
                return IsAbovePlane(in point, in planeNormal, in v3.zero);
            }


            public static bool IsBelowPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                var vectorToPlane = (point - planePoint).normalized;
                var distance = -dot(in vectorToPlane, in planeNormal);
                return distance > 0;
            }
            public static bool IsBelowPlane(in Vector3 point, in Vector3 planeNormal)
            {
                return IsBelowPlane(in point, in planeNormal, in v3.zero);
            }


            public static bool AreOnSameSidesOfPlane(in Vector3 point1, in Vector3 point2, in Vector3 planeNormal, in Vector3 planePoint)
            {
                return IsAbovePlane(in point1, in planeNormal, in planePoint) == IsAbovePlane(in point2, in planeNormal, in planePoint);
            }

            public static float DistanceToPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                var vectorToPlane = point - planePoint;
                var distance = dot(in planeNormal, in vectorToPlane);
                return abs(distance);
            }

            public static bool IsFirstCloser(in Vector3 first, in Vector3 second, in Vector3 reference)
            {
                var d1 = distanceSquared.Between(in first, in reference);
                var d2 = distanceSquared.Between(in second, in reference);
                return d1 < d2;
            }
            public static bool IsFirstCloser(Vector3 first, Vector3 second, Vector3 reference)
            {
                var d1 = distanceSquared.Between(in first, in reference);
                var d2 = distanceSquared.Between(in second, in reference);
                return d1 < d2;
            }
            public static bool IsFirstCloser2d(in Vector2 first, in Vector2 second, in Vector2 reference)
            {
                var d1 = distanceSquared.Between(in first, in reference);
                var d2 = distanceSquared.Between(in second, in reference);
                return d1 < d2;
            }
            public static bool IsFirstCloser2d(Vector2 first, Vector2 second, Vector2 reference)
            {
                var d1 = distanceSquared.Between(in first, in reference);
                var d2 = distanceSquared.Between(in second, in reference);
                return d1 < d2;
            }

            public static Vector3 ProjectOnPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                var vectorToPlane = point - planePoint;
                var distance = -dot(in planeNormal, in vectorToPlane);
                return point + planeNormal * distance;
            }
            public static void ProjectOnPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint, out Vector3 projection)
            {
                var norm = planeNormal.normalized;
                var vectorToPlane = point - planePoint;
                var distance = -dot(in norm, in vectorToPlane);
                projection = point + norm * distance;
            }
            public static Vector3 ProjectOnLine(in Vector3 point, in Vector3 line1, in Vector3 line2)
            {
                var pointToLine = point - line1;
                var lineVector = line2 - line1;
                Vector3 onNormal;
                vector.ProjectOnNormal(in pointToLine, in lineVector, out onNormal);
                return onNormal + line1;
            }
            public static void ProjectOnLine(in Vector3 point, in Vector3 line1, in Vector3 line2, out Vector3 projection)
            {
                var pointToLine = point - line1;
                var lineVector = line2 - line1;
                Vector3 onNormal;
                vector.ProjectOnNormal(in pointToLine, in lineVector, out onNormal);
                projection = onNormal + line1;
            }
            public static bool TryProjectOnLineSegment(in Vector3 point, in Vector3 line1, in Vector3 line2, out Vector3 projection)
            {
                var pointToLine = point - line1;
                var lineVector = line2 - line1;
                Vector3 onNormal;
                vector.ProjectOnNormal(in pointToLine, in lineVector, out onNormal);
                projection = onNormal + line1;
                if (!IsOnSegment(in line1, in projection, in line2))
                {
                    projection = Vector3.zero;
                    return false;
                }
                return true;
            }
            public static void ProjectOnLineSegmentOrGetClosest(in Vector3 point, in Vector3 line1, in Vector3 line2, out Vector3 projection)
            {
                var pointToLine = point - line1;
                var lineVector = line2 - line1;
                Vector3 onNormal;
                vector.ProjectOnNormal(in pointToLine, in lineVector, out onNormal);
                projection = onNormal + line1;
                if (!IsOnSegment(in line1, in projection, in line2))
                {
                    var d1 = distanceSquared.Between(in line1, in projection);
                    var d2 = distanceSquared.Between(in line2, in projection);
                    projection = d1 < d2 ? line1 : line2;
                }
            }
            public static void ProjectOnLine2D(in Vector2 point, in Vector2 linePoint1, in Vector2 linePoint2, out Vector2 result)
            {
                var line = linePoint1 - linePoint2;
                var newPoint = point - linePoint2;

                result = ((dot2D(in newPoint, in line) / dot2D(in line, in line)) * line) + linePoint2;
            }
            public static Vector3 ReflectOfPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
            {
                Vector3 projection;
                ProjectOnPlane(in point, in planeNormal, in planePoint, out projection);
                var distance = fun.distance.Between(in point, in projection);
                Vector3 mirrorPoint;
                fun.point.TryMoveAbs(in point, in projection, distance * 2, out mirrorPoint);
                return mirrorPoint;
            }
            public static Vector3 ReflectOfPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint)
            {
                Vector3 projection;
                ProjectOnPlane(in point, in planeNormal, in planePoint, out projection);
                var distance = fun.distance.Between(in point, in projection);
                Vector3 mirrorPoint;
                fun.point.TryMoveAbs(in point, in projection, distance * 2, out mirrorPoint);
                return mirrorPoint;
            }
            public static void ReflectOfPlane(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint, out Vector3 mirrorPoint)
            {
                Vector3 projection;
                ProjectOnPlane(in point, in planeNormal, in planePoint, out projection);
                var distance = fun.distance.Between(in point, in projection);
                fun.point.TryMoveAbs(in point, in projection, distance * 2, out mirrorPoint);
            }

            /// <summary>
            /// if you are looking from above the normal: counter clockwise
            /// </summary>
            public static Vector3 GetNormal(in Vector3 p1, in Vector3 p2, in Vector3 p3)
            {
                var lhs = p1 - p2;
                var rhs = p3 - p2;
                Vector3 r;
                vector.GetNormal(in lhs, in rhs, out r);
                return r;
            }
            public static Vector3 GetNormalSameSideAs(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 sameSideAs)
            {
                var lhs = p1 - p2;
                var rhs = p3 - p2;
                Vector3 r;
                vector.GetNormal(in lhs, in rhs, out r);
                vector.EnsurePointSameDirAs(in r, in sameSideAs, out r);
                return r;
            }

            /// <summary>
            /// if you are looking from above the normal: counter clockwise
            /// </summary>
            public static void GetNormal(in Vector3 p1, in Vector3 p2, in Vector3 p3, out Vector3 r)
            {
                var lhs = p1 - p2;
                var rhs = p3 - p2;
                vector.GetNormal(in lhs, in rhs, out r);
            }
            public static bool TryGetNormal(in Vector3 p1, in Vector3 p2, in Vector3 p3, out Vector3 r)
            {
                var lhs = p1 - p2;
                var rhs = p3 - p2;
                return vector.TryGetNormal(in lhs, in rhs, out r);
            }
            public static void GetNormalSameSideAs(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 sameSideAs, out Vector3 normal)
            {
                var lhs = p1 - p2;
                var rhs = p3 - p2;
                vector.GetNormal(in lhs, in rhs, out normal);
                vector.EnsurePointSameDirAs(in normal, in sameSideAs, out normal);
            }
            public static Vector2 Lerp2D(Vector2 from, Vector2 to, double ratio)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    return from;
                }
                return new Vector2
                {
                    x = from.x + (to.x - from.x) * r,
                    y = from.y + (to.y - from.y) * r
                };
            }
            public static void Lerp2D(in Vector2 from, in Vector2 to, double ratio, out Vector2 result)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    result = from;
                    return;
                }
                result = new Vector2
                {
                    x = from.x + (to.x - from.x) * r,
                    y = from.y + (to.y - from.y) * r
                };
            }
            public static Vector3 Lerp(in Vector3 from, in Vector3 to, double ratio)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    return from;
                }
                return new Vector3(from.x + (to.x - from.x) * r, from.y + (to.y - from.y) * r, from.z + (to.z - from.z) * r);
            }
            public static void Lerp(in Vector3 from, in Vector3 to, double ratio, out Vector3 result)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    result = from;
                    return;
                }
                result = new Vector3(from.x + (to.x - from.x) * r, from.y + (to.y - from.y) * r, from.z + (to.z - from.z) * r);
            }
            public static Vector3 Lerp(Vector3 from, Vector3 to, double ratio)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    return from;
                }
                return new Vector3(from.x + (to.x - from.x) * r, from.y + (to.y - from.y) * r, from.z + (to.z - from.z) * r);
            }

            public static Vector3 Lerp(Vector3 from, Vector3 to, double ratio,
                Func<float, float> xFunc, Func<float, float> yFunc, Func<float, float> zFunc)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    return from;
                }
                if (xFunc == null) xFunc = n => n;
                if (yFunc == null) yFunc = n => n;
                if (zFunc == null) zFunc = n => n;
                return new Vector3(
                    from.x + (to.x - from.x) * xFunc(r),
                    from.y + (to.y - from.y) * yFunc(r),
                    from.z + (to.z - from.z) * zFunc(r)
                );
            }

            public static bool TryMoveRel2D(in Vector2 from, in Vector2 to, double ratio, out Vector2 result)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    result = from;
                    return false;
                }
                result = new Vector2
                {
                    x = from.x + (to.x - from.x) * r,
                    y = from.y + (to.y - from.y) * r
                };
                return true;
            }
            public static bool TryMoveRel(in Vector3 from, in Vector3 to, double ratio, out Vector3 result)
            {
                var r = (float)ratio;
                if (r.IsEqual(0))
                {
                    result = from;
                    return false;
                }
                result = new Vector3
                {
                    x = from.x + (to.x - from.x) * r,
                    y = from.y + (to.y - from.y) * r,
                    z = from.z + (to.z - from.z) * r
                };
                return true;
            }
            public static bool TryMoveAbs(float fromX, float fromY, float toX, float toY, double distance, out float x, out float y)
            {
                if (distance.IsEqual(0))
                {
                    x = fromX;
                    y = fromY;
                    return false;
                }

                var preDistance = fun.distance.Between(fromX, fromY, toX, toY);

                if (preDistance.IsEqual(0))
                {
                    x = toX;
                    y = toY;
                    return false;
                }

                var amount = (float)distance / preDistance;
                x = fromX + (toX - fromX) * amount;
                y = fromY + (toY - fromY) * amount;
                return true;
            }
            public static bool TryMoveAbs2D(in Vector2 from, in Vector2 to, double distance, out Vector2 result)
            {
                if (distance.IsEqual(0))
                {
                    result = from;
                    return false;
                }

                var preDistance = fun.distance.Between2D(in from, in to);

                if (preDistance.IsEqual(0))
                {
                    result = to;
                    return false;
                }

                var amount = (float)distance / preDistance;
                result = new Vector2
                {
                    x = from.x + (to.x - from.x) * amount,
                    y = from.y + (to.y - from.y) * amount
                };
                return true;
            }
            public static bool TryMoveAbs(in Vector3 from, in Vector3 to, double distance, out Vector3 result)
            {
                if (distance.IsEqual(0))
                {
                    result = from;
                    return false;
                }

                var dir = (to - from);
                var mag = vector.Magnitude(in dir);
                if (mag < 0.00001)
                {
                    result = to;
                    return false;
                }
                dir = dir / mag;

                result = from + dir * (float)distance;
                return true;
            }
            public static Vector2 MoveAbs2D(Vector2 from, Vector2 to, double distance)
            {
                if (distance.IsEqual(0))
                {
                    return from;
                }

                var dir = (to - from);
                var mag = vector.Magnitude2D(in dir);
                if (mag < 0.00001) return to;

                dir = dir / mag;

                return from + dir * (float)distance;
            }
            public static Vector3 MoveAbs(Vector3 from, Vector3 to, double distance)
            {
                if (distance.IsEqual(0))
                {
                    return from;
                }

                var dir = (to - from);
                var mag = vector.Magnitude(in dir);
                if (mag < 0.00001)
                {
                    return to;
                }
                dir = dir / mag;

                return from + dir * (float)distance;
            }
            public static Vector3 MoveAbs(in Vector3 from, in Vector3 to, double distance)
            {
                if (distance.IsEqual(0))
                {
                    return from;
                }

                var dir = (to - from);
                var mag = vector.Magnitude(in dir);
                if (mag < 0.00001)
                {
                    return to;
                }
                dir = dir / mag;

                return from + dir * (float)distance;
            }
            public static bool MoveAbs(in Vector3 from, in Vector3 to, double distance, out Vector3 result)
            {
                if (distance.IsEqual(0))
                {
                    result = from;
                    return false;
                }

                var dir = (to - from);
                var mag = vector.Magnitude(in dir);
                if (mag < 0.00001)
                {
                    result = to;
                    return false;
                }
                dir = dir / mag;

                result = from + dir * (float)distance;
                return true;
            }

            public static void Middle2D(in Vector2 a, in Vector2 b, out Vector2 output)
            {
                output = new Vector2((b.x - a.x) * 0.5f + a.x, (b.y - a.y) * 0.5f + a.y);
            }
            public static void Middle(in Vector3 a, in Vector3 b, out Vector3 output)
            {
                output = new Vector3((b.x - a.x) * 0.5f + a.x, (b.y - a.y) * 0.5f + a.y, (b.z - a.z) * 0.5f + a.z);
            }
            public static Vector2 Middle2D(in Vector2 a, in Vector2 b)
            {
                return new Vector2((b.x - a.x) * 0.5f + a.x, (b.y - a.y) * 0.5f + a.y);
            }
            public static Vector3 Middle(in Vector3 a, in Vector3 b)
            {
                return new Vector3((b.x - a.x) * 0.5f + a.x, (b.y - a.y) * 0.5f + a.y, (b.z - a.z) * 0.5f + a.z);
            }

            public static void EnforceWithin(ref Vector2 current, in Vector2 start, in Vector2 end)
            {
                var isWithin =
                    current.x <= max(start.x, end.x) + epsilon && current.x >= min(start.x, end.x) - epsilon &&
                    current.y <= max(start.y, end.y) + epsilon && current.y >= min(start.y, end.y) - epsilon;

                if (!isWithin)
                {
                    var dSq1 = distanceSquared.Between(in current, in start);
                    var dSq2 = distanceSquared.Between(in current, in end);

                    current = dSq1 < dSq2 ? start : end;
                }
            }

            public static void GetPlaneNormalOfTheSideOf(in Vector3 planeNormal, in Vector3 planeCenter, in Vector3 point, out Vector3 planeNormalOfTheSideOfPoint)
            {
                var toPoint = (point - planeCenter).normalized;
                planeNormal.Normalize();

                var dp = dot(in toPoint, in planeNormal);
                if (dp > 0)
                {
                    planeNormalOfTheSideOfPoint = planeNormal;
                }
                else
                {
                    planeNormalOfTheSideOfPoint = -planeNormal;
                }
            }


            public static void LiftAboveTowards(in Vector3 point, in Vector3 planeNormal, in Vector3 planePoint, in Vector3 towardsPoint, double liftAmount, out Vector3 pointLifted)
            {
                var normal = IsAbovePlane(in towardsPoint, in planeNormal, in planePoint) ? planeNormal : -planeNormal;
                pointLifted = point + normal * (float)liftAmount;
            }

            public static bool IsBetweenTwo(in Vector3 point, in Vector3 limitA, in Vector3 limitB)
            {
                var vec = limitA - limitB;
                var normMag = vec.magnitude;
                if (normMag < 0.00001)
                {
                    return false;
                }
                var normB = vec / normMag;
                var normA = -normB;

                return IsAbovePlane(in point, in normB, in limitB) && IsAbovePlane(in point, in normA, in limitA);
            }

            public static Vector3 MidAwayFromAxis(in Vector3 point1, in Vector3 point2, double awayMeters, in Vector3 axis)
            {
                return MidAwayFromAxis(in point1, in point2, awayMeters, in axis, in v3.zero);
            }

            public static Vector3 MidAwayFromAxis(in Vector3 point1, in Vector3 point2, double awayMeters, in Vector3 axis, in Vector3 axisPoint)
            {
                Vector3 mid;
                Lerp(in point1, in point2, 0.5, out mid);

                Vector3 proj;
                ProjectOnLine(in mid, in axisPoint, in axis, out proj);

                var dir = (mid - proj).normalized;

                return mid + dir * (float)awayMeters;
            }

            public static bool IsFirstCloserToLine(in Vector3 first, in Vector3 second, in Vector3 linePoint1, in Vector3 linePoint2)
            {
                Vector3 firstOnLine, secondOnLine;
                ProjectOnLine(in first, in linePoint1, in linePoint2, out firstOnLine);
                ProjectOnLine(in second, in linePoint1, in linePoint2, out secondOnLine);

                return distanceSquared.Between(in first, in firstOnLine) <
                       distanceSquared.Between(in second, in secondOnLine);
            }

            public static void GetClosestToPoint(in Vector3 targetPoint, in Vector3 p1, in Vector3 p2, out Vector3 closestToTarget)
            {
                var dSq1 = fun.distanceSquared.Between(in targetPoint, in p1);
                var dSq2 = fun.distanceSquared.Between(in targetPoint, in p2);
                closestToTarget = dSq1 <= dSq2 ? p1 : p2;
            }
        }

    }
}