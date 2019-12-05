using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class intersection
        {
            /// <summary>
            /// intersection between two axis alligned bounding boxes
            /// </summary>
            /// <param name="center1">center of the first box</param>
            /// <param name="size1">width of each dimension of the first bounding box</param>
            /// <param name="center2">center of the second box</param>
            /// <param name="size2">width of each dimension of the second bounding box</param>
            /// <returns>return true if they overlap</returns>
            public static bool BetweenAxisAlignedBoxes(in Vector3 center1, in Vector3 size1, in Vector3 center2, in Vector3 size2)
            {
                var hasOverlapOnX = Math.Abs(center1.x - center2.x) <= ((size1.x / 2f) + (size2.x / 2f));
                var hasOverlapOnY = Math.Abs(center1.y - center2.y) <= ((size1.y / 2f) + (size2.y / 2f));
                var hasOverlapOnZ = Math.Abs(center1.z - center2.z) <= ((size1.z / 2f) + (size2.z / 2f));
                return hasOverlapOnX && hasOverlapOnY && hasOverlapOnZ;
            }


            /// <summary>
            /// intersection between two spheres is a circle
            /// </summary>
            /// <param name="sphere1Center">sphere 1 center</param>
            /// <param name="sphere1Radius">sphere 1 radius</param>
            /// <param name="sphere2Center">sphere 2 center</param>
            /// <param name="sphere2Radius">sphere 2 radius</param>
            /// <param name="intersectCircleCenter">intersection circle center</param>
            /// <param name="intersectCircleRadius">intersection circle radius</param>
            /// <returns>return true if the 2 spheres intersect their surfaces, if one is inside the other returns false</returns>
            public static bool BetweenSpheres(in Vector3 sphere1Center, double sphere1Radius, in Vector3 sphere2Center, double sphere2Radius, out Vector3 intersectCircleCenter, out float intersectCircleRadius)
            {
                var r1 = (float)sphere1Radius;
                var r2 = (float)sphere2Radius;
                var d = distance.Between(in sphere1Center, in sphere2Center);
                // Two separate spheres
                if (d > (r1 + r2))
                {
                    intersectCircleCenter = lerp(in sphere1Center, in sphere2Center, sphere1Radius / sphere2Radius);
                    intersectCircleRadius = 0;
                    return false;
                }
                // Outer tangency
                if (d.IsEqual(r1 + r2))
                {
                    intersectCircleCenter = sphere1Center + (sphere2Center - sphere1Center).normalized * r2;
                    intersectCircleRadius = 0;
                    return true;
                }
                // Inner tangency
                if (d.IsEqual(abs(r1 - r2)))
                {
                    intersectCircleCenter =
                        r1 > r2
                        ? sphere1Center + (sphere2Center - sphere1Center).ToUnit(Vector3.forward) * r1
                        : sphere2Center + (sphere1Center - sphere2Center).ToUnit(Vector3.forward) * r2;
                    intersectCircleRadius = 0;
                    return true;
                }
                // One sphere inside the other
                if (d < abs(r1 - r2))
                {
                    intersectCircleCenter = lerp(in sphere1Center, in sphere2Center, 0.5);
                    intersectCircleRadius = 0;
                    return false;
                }
                var d1 = (d * d - r2 * r2 + r1 * r1) / (2 * d);
                var ratio = d1 / r1;
                var cr = sqrt(1f - ratio * ratio) * r1;
                intersectCircleRadius = cr;
                var dir = (sphere2Center - sphere1Center).ToUnit(Vector3.forward);
                intersectCircleCenter = sphere1Center + dir * d1;
                return true;
            }

            /// <summary>
            /// Check if disk and plane cross each other
            /// </summary>
            /// <param name="planeNormal">the normal of the plane</param>
            /// <param name="planePoint">any point on the plane</param>
            /// <param name="sphereCenter">the center of the sphere</param>
            /// <param name="sphereRadius">sphere radius</param>
            /// <param name="collision">collision point</param>
            /// <returns></returns>
            public static bool BetweenPlaneAndSphere(in Vector3 planeNormal, in Vector3 planePoint, in Vector3 sphereCenter, float sphereRadius, out Vector3 collision)
            {
                Vector3 sphereCenterProj;
                point.ProjectOnPlane(in sphereCenter, in planeNormal, in planePoint, out sphereCenterProj);
                var h = distance.Between(in sphereCenter, in sphereCenterProj);
                if (h > sphereRadius)
                {
                    // sphere does not intersect the plane
                    collision = Vector3.zero;
                    return false;
                }
                collision = sphereCenterProj;
                return true;
            }


            /// <summary>
            /// Check if disk and plane cross each other
            /// </summary>
            /// <param name="p1">point on the plane 1</param>
            /// <param name="p2">point on the plane 2</param>
            /// <param name="p3">point on the plane 3</param>
            /// <param name="sphereCenter">the center of the sphere</param>
            /// <param name="sphereRadius">sphere radius</param>
            /// <param name="intersectPoint">collision point</param>
            /// <returns></returns>
            public static bool BetweenPlaneAndSphere(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 sphereCenter, float sphereRadius, out Vector3 intersectPoint)
            {
                Vector3 planeNormal;
                point.GetNormal(in p1, in p2, in p3, out planeNormal);
                return BetweenPlaneAndSphere(in planeNormal, in p1, in sphereCenter, sphereRadius, out intersectPoint);
            }

            /// <summary>
            /// Check if disk and sphere cross each other
            /// </summary>
            /// <param name="diskPlaneNormal">the normal of the disk 1 plane</param>
            /// <param name="diskCenter">the center of the disk</param>
            /// <param name="diskRadius">disk radius</param>
            /// <param name="sphereCenter">the center of the sphere</param>
            /// <param name="sphereRadius">sphere radius</param>
            /// <param name="collision">collision point</param>
            /// <returns></returns>
            public static bool BetweenDiskAndSphere(in Vector3 diskPlaneNormal, in Vector3 diskCenter, float diskRadius, in Vector3 sphereCenter, float sphereRadius, out Vector3 collision)
            {
                Vector3 sphereCenterProj;
                point.ProjectOnPlane(in sphereCenter, in diskPlaneNormal, in diskCenter, out sphereCenterProj);
                var h = distance.Between(in sphereCenter, in sphereCenterProj);
                if (h > sphereRadius)
                {
                    // sphere does not intersect disk plane
                    collision = Vector3.zero;
                    return false;
                }
                var projCirCenDist = distance.Between(in sphereCenterProj, in diskCenter);
                if (projCirCenDist > (diskRadius + sphereRadius))
                {
                    // maximum extent sphere circle interection with disk plane cannot reach disk plane
                    collision = Vector3.zero;
                    return false;
                }

                var projCirRad = (float)Math.Sqrt(sphereRadius * sphereRadius - h * h);
                if (projCirCenDist > (diskRadius + projCirRad))
                {
                    // real extent sphere circle interection with disk plane cannot reach disk plane
                    collision = Vector3.zero;
                    return false;
                }
                var overlap = diskRadius + projCirRad - projCirCenDist;
                point.MoveAbs(in sphereCenterProj, in diskCenter, projCirRad - overlap * 0.5f, out collision);
                return true;
            }

            /// <summary>
            /// Check if two disks cross each other
            /// </summary>
            /// <param name="disk1PlaneNormal">the normal of the disk 1 plane</param>
            /// <param name="disk1Center">the center of disk 1</param> 
            /// <param name="disk1Radius">the radius of disk 1</param>
            /// <param name="disk2PlaneNormal">the normal of the disk 2 plane</param>
            /// <param name="disk2Center">the center of disk 2</param> 
            /// <param name="disk2Radius">the radius of disk 2</param>
            /// <param name="collision">returns the collision point</param>
            /// <returns></returns>
            public static bool BetweenDiskAndDisk(
                in Vector3 disk1PlaneNormal, in Vector3 disk1Center, double disk1Radius,
                in Vector3 disk2PlaneNormal, in Vector3 disk2Center, double disk2Radius,
                out Vector3 collision)
            {
                var max = (float)(disk1Radius + disk2Radius);
                var distBetweenCenters = distance.Between(in disk1Center, in disk2Center);
                // the centers are too far and the disks could not overlap
                if (distBetweenCenters > max)
                {
                    collision = Vector3.zero;
                    return false;
                }

                var dpOfDir = dot(in disk1PlaneNormal, in disk2PlaneNormal);
                // if disks point up same or opposite direction
                if (dpOfDir > 0.99999 || dpOfDir < -0.99999)
                {

                    var centerToCenter = disk2Center - disk1Center;
                    if (centerToCenter.sqrMagnitude < 0.0001f)
                    {
                        collision = disk1Center;
                        return true;
                    }
                    centerToCenter = centerToCenter.normalized;
                    var dpOf2 = dot(in disk1PlaneNormal, in centerToCenter);
                    // if the vector between centers and the up vectors are orthogonal (90 degrees) or dot product = 0
                    if (dpOf2 < 0.000001f && dpOf2 > -0.000001f)
                    {
                        // check they are far enough to touch
                        collision = disk1Center + (disk2Center - disk1Center).normalized * (((float)disk1Radius / max) * distBetweenCenters);
                        return true;
                    }
                    collision = Vector3.zero;
                    return false;
                }

                Vector3 normal;
                BetweenPlanes(in disk1PlaneNormal, in disk1Center, in disk2PlaneNormal, in disk2Center, out collision, out normal);

                var collisionPlusNorm = collision + normal;

                Vector3 norm1X, norm1Y;
                vector.ComputeRandomXYAxesForPlane(in disk1PlaneNormal, out norm1X, out norm1Y);
                var a = (collision - disk1Center).As2d(in norm1X, in norm1Y);
                var b = (collisionPlusNorm - disk1Center).As2d(in norm1X, in norm1Y);
                Vector2 projection1;
                var has1 = IsLineGettingCloserToOriginThan(in a, in b, (float)disk1Radius, out projection1);
                if (!has1)
                {
                    collision = Vector3.zero;
                    return false;
                }
                Vector3 norm2X, norm2Y;
                vector.ComputeRandomXYAxesForPlane(in disk2PlaneNormal, out norm2X, out norm2Y);
                a = (collision - disk2Center).As2d(in norm2X, in norm2Y);
                b = (collisionPlusNorm - disk2Center).As2d(in norm2X, in norm2Y);
                Vector2 projection2;
                var has2 = IsLineGettingCloserToOriginThan(in a, in b, (float)disk2Radius, out projection2);
                if (!has2)
                {
                    collision = Vector3.zero;
                    return false;
                }

                Vector3 perp1, perp2;
                projection1.As3d(in disk1Center, in norm1X, in norm1Y, out perp1);
                projection2.As3d(in disk2Center, in norm2X, in norm2Y, out perp2);
                //Debug.DrawLine(perp1,disk1Center,Color.yellow,0,false);
                //Debug.DrawLine(perp2,disk2Center,Color.white,0,false);
                //Debug.DrawLine(collision,collision+normal,Color.blue,0,false);
                //Debug.DrawLine(collision,collision-normal,Color.blue,0,false);
                var perp1to2 = perp2 - perp1;
                if (perp1to2.sqrMagnitude < 0.00001f)
                {
                    collision = perp1;
                    return true;
                }
                // in 2d relative to radius from center to perpendicular
                var relCenToPerp1 = projection1.magnitude / disk1Radius;
                var relCenToPerp2 = projection2.magnitude / disk2Radius;

                var absPerpToEdge1 = sqrt(1f - relCenToPerp1 * relCenToPerp1) * disk1Radius;
                var absPerpToEdge2 = sqrt(1f - relCenToPerp2 * relCenToPerp2) * disk2Radius;

                if (perp1to2.magnitude <= (absPerpToEdge1 + absPerpToEdge2))
                {
                    collision = point.Lerp(in perp1, in perp2, disk1Radius / max);
                    return true;
                }

                collision = Vector3.zero;
                return false;
            }


            /// <summary>
            /// The points of the capsule are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="csb">capsule sphere below center</param>
            /// <param name="csa">capsule sphere above center</param>
            /// <param name="capsuleRadius">capsule radius</param>
            /// <param name="diskCenter">The center point of the disk</param>
            /// <param name="diskNormal">disk up normal</param>
            /// <param name="diskRadius">disk radius</param>
            /// <returns></returns>
            public static bool BetweenCapsuleAndDisk(
                in Vector3 csb, in Vector3 csa, double capsuleRadius,
                in Vector3 diskNormal, in Vector3 diskCenter, double diskRadius)
            {
                // see if disk center is within the capsule cylinder
                Vector3 diskCenterOnAxis;
                var diskCenIsWithinCylPl =
                    point.TryProjectOnLineSegment(in diskCenter, in csb, in csa, out diskCenterOnAxis);
                if (diskCenIsWithinCylPl &&
                    distance.Between(in diskCenterOnAxis, in diskCenter) <= capsuleRadius)
                {
                    return true;
                }
                var capsuleUp = (csa - csb).normalized;
                var maxDistance = capsuleRadius + diskRadius;

                // middle part
                Vector3 diskCenOnCapPl;
                point.ProjectOnPlane(in diskCenter, in capsuleUp, in csb, out diskCenOnCapPl);
                var capToDisk = (diskCenOnCapPl - csb).normalized * (float)capsuleRadius;
                var csaShifted = csa + capToDisk;
                var csbShifted = csb + capToDisk;
                Vector3 c1;
                if (BetweenPlaneAndLineSegment(in diskNormal, in diskCenter, in csaShifted, in csbShifted, out c1))
                {
                    if (distance.Between(in c1, in diskCenter) <= diskRadius)
                    {
                        return true;
                    }
                }


                // test collision with below sphere
                Vector3 capEndOnDiskPl;
                point.ProjectOnPlane(in csb, in diskNormal, in diskCenter, out capEndOnDiskPl);
                var diskToSph = capEndOnDiskPl - diskCenter;
                var diskToSphMag = vector.Magnitude(in diskToSph);
                if (diskToSphMag < 0.00001f)
                {
                    if (distance.Between(in csb, in diskCenter) < maxDistance)
                    {
                        return true;
                    }
                }
                else
                {
                    diskToSph = diskToSph / diskToSphMag;//normalize
                    Vector3 c2;
                    if (BetweenRayAndSphere(in diskToSph, in diskCenter, in csb, capsuleRadius, out c2))
                    {
                        if (distance.Between(in c2, in diskCenter) <= diskRadius)
                        {
                            return true;
                        }
                    }
                }
                // test collision with above sphere
                point.ProjectOnPlane(in csa, in diskNormal, in diskCenter, out capEndOnDiskPl);
                diskToSph = capEndOnDiskPl - diskCenter;
                diskToSphMag = vector.Magnitude(in diskToSph);
                if (diskToSphMag < 0.00001f)
                {
                    if (distance.Between(in csa, in diskCenter) < maxDistance)
                    {
                        return true;
                    }
                }
                else
                {
                    diskToSph = diskToSph / diskToSphMag;//normalize
                    Vector3 c3;
                    if (BetweenRayAndSphere(in diskToSph, in diskCenter, in csa, capsuleRadius, out c3))
                    {
                        if (distance.Between(in c3, in diskCenter) <= diskRadius)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// The points of the capsule are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="csb">capsule sphere below center</param>
            /// <param name="csa">capsule sphere above center</param>
            /// <param name="capsuleRadius">capsule radius</param>
            /// <param name="diskCenter">The center point of the disk</param>
            /// <param name="diskNormal">disk up normal</param>
            /// <param name="diskRadius">disk radius</param>
            /// <param name="collision">the point best describing the collision point - might not be precise!</param>
            /// <returns></returns>
            public static bool BetweenCapsuleAndDisk(
                in Vector3 csb, in Vector3 csa, float capsuleRadius,
                in Vector3 diskNormal, in Vector3 diskCenter, float diskRadius, out Vector3 collision)
            {
                // see if disk center is within the capsule cylinder
                Vector3 diskCenterOnAxis;
                var diskCenIsWithinCylPl =
                    point.TryProjectOnLineSegment(in diskCenter, in csb, in csa, out diskCenterOnAxis);
                if (diskCenIsWithinCylPl &&
                    distance.Between(in diskCenterOnAxis, in diskCenter) <= capsuleRadius)
                {
                    collision = diskCenter;
                    return true;
                }
                var capsuleUp = (csa - csb).normalized;
                var maxDistance = capsuleRadius + diskRadius;

                // middle part
                Vector3 diskCenOnCapPl;
                point.ProjectOnPlane(in diskCenter, in capsuleUp, in csb, out diskCenOnCapPl);
                var capToDisk = (diskCenOnCapPl - csb).normalized * capsuleRadius;
                var csaShifted = csa + capToDisk;
                var csbShifted = csb + capToDisk;
                if (BetweenPlaneAndLineSegment(in diskNormal, in diskCenter, in csaShifted, in csbShifted, out collision))
                {
                    if (distance.Between(in collision, in diskCenter) <= diskRadius)
                    {
                        return true;
                    }
                }


                // test collision with below sphere
                Vector3 capEndOnDiskPl;
                point.ProjectOnPlane(in csb, in diskNormal, in diskCenter, out capEndOnDiskPl);
                var diskToSph = capEndOnDiskPl - diskCenter;
                var diskToSphMag = vector.Magnitude(in diskToSph);
                if (diskToSphMag < 0.00001f)
                {
                    if (distance.Between(in csb, in diskCenter) < maxDistance)
                    {
                        var diff = (csb - diskCenter);
                        var diffMag = vector.Magnitude(in diff);
                        collision = (diffMag < 0.00001) ? diskCenter : diskCenter + (diff / diffMag) * min(diffMag / 2, diskRadius);
                        return true;
                    }
                }
                else
                {
                    diskToSph = diskToSph / diskToSphMag;//normalize
                    if (BetweenRayAndSphere(in diskToSph, in diskCenter, in csb, capsuleRadius, out collision))
                    {
                        if (distance.Between(in collision, in diskCenter) <= diskRadius)
                        {
                            Vector3 collisionOnAxis;
                            if (point.TryProjectOnLineSegment(in collision, in csb, in csa, out collisionOnAxis))
                            {
                                Vector3 diskOnPlane;
                                point.ProjectOnPlane(in diskCenter, in capsuleUp, in collisionOnAxis, out diskOnPlane);
                                collision = (diskOnPlane - collisionOnAxis).normalized * capsuleRadius + collisionOnAxis;
                            }
                            return true;
                        }
                    }
                }
                // test collision with above sphere
                point.ProjectOnPlane(in csa, in diskNormal, in diskCenter, out capEndOnDiskPl);
                diskToSph = capEndOnDiskPl - diskCenter;
                diskToSphMag = vector.Magnitude(in diskToSph);
                if (diskToSphMag < 0.00001f)
                {
                    if (distance.Between(in csa, in diskCenter) < maxDistance)
                    {
                        var diff = (csa - diskCenter);
                        var diffMag = vector.Magnitude(in diff);
                        collision = diffMag < 0.00001 ? diskCenter : diskCenter + (diff / diffMag) * min(diffMag / 2, diskRadius);
                        return true;
                    }
                }
                else
                {
                    diskToSph = diskToSph / diskToSphMag;//normalize
                    if (BetweenRayAndSphere(in diskToSph, in diskCenter, in csa, capsuleRadius, out collision))
                    {
                        if (distance.Between(in collision, in diskCenter) <= diskRadius)
                        {
                            Vector3 collisionOnAxis;
                            if (point.TryProjectOnLineSegment(in collision, in csb, in csa, out collisionOnAxis))
                            {
                                Vector3 diskOnPlane;
                                point.ProjectOnPlane(in diskCenter, in capsuleUp, in collisionOnAxis, out diskOnPlane);
                                collision = (diskOnPlane - collisionOnAxis).normalized * capsuleRadius + collisionOnAxis;
                            }
                            return true;
                        }
                    }
                }

                collision = Vector3.zero;
                return false;
            }


            public static bool BetweenRayAndSphere(
                in Vector3 rayFw, in Vector3 rayOr,
                in Vector3 sphereCenter, double sphereRadius,
                out Vector3 collision)
            {
                var radiusSquared = sphereRadius * sphereRadius;
                var rayToSphere = sphereCenter - rayOr;
                var tca = fun.dot(in rayToSphere, in rayFw);
                var d2 = fun.dot(in rayToSphere, in rayToSphere) - tca * tca;
                if (d2 > radiusSquared)
                {
                    collision = Vector3.zero;
                    return false;
                }
                var thc = (float)Math.Sqrt(radiusSquared - d2);
                var t0 = tca - thc;
                var t1 = tca + thc;

                if (t0 > t1)
                {
                    var temp = t0;
                    t0 = t1;
                    t1 = temp;
                }

                if (t0 < 0)
                {
                    t0 = t1; // if t0 is negative, let's use t1 instead 
                    if (t0 < 0)
                    {
                        collision = Vector3.zero;
                        return false; // both t0 and t1 are negative 
                    }
                }

                var t = t0;

                collision = rayOr + rayFw * t;

                return true;
            }
            public static bool BetweenRayAndCapsule(
                in Vector3 rayFw, in Vector3 rayOr,
                in Vector3 cpu, in Vector3 cpd, float capsuleRadius,
                out Vector3 collision)
            {
                var capDir = (cpu - cpd);
                if (capDir.sqrMagnitude < 0.00001f)
                {
                    // the two capsule ends are so close so it is esencially a sphere
                    return BetweenRayAndSphere(in rayFw, in rayOr, in cpu, capsuleRadius, out collision);
                }
                capDir.Normalize();
                Vector3 pl1, pl2;
                point.ClosestOnTwoLinesByPointAndDirection(in rayOr, in rayFw, in cpd, in capDir, out pl1, out pl2);
                if (!point.IsOnSegment(in cpu, in pl2, in cpd))
                {
                    if (BetweenRayAndSphere(in rayFw, in rayOr, in cpu, capsuleRadius, out collision)) return true;
                    if (BetweenRayAndSphere(in rayFw, in rayOr, in cpd, capsuleRadius, out collision)) return true;
                    return false;
                }

                var d = distance.Between(in pl1, in pl2);
                var hasCollision = d <= capsuleRadius;
                if (hasCollision)
                {
                    var n = Math.Sqrt(capsuleRadius * capsuleRadius - d * d);
                    point.MoveAbs(in pl1, in rayOr, (float)n, out collision);
                    return true;
                }
                collision = Vector3.zero;
                return false;
            }

            /// <summary>
            /// The points of the capsule are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="csb">capsule sphere below center</param>
            /// <param name="csa">capsule sphere above center</param>
            /// <param name="capsuleRadius">capsule radius</param>
            /// <param name="sphereCenter">sphere center point</param>
            /// <param name="sphereRadius">sphere radius</param>
            /// <returns></returns>
            public static bool BetweenCapsuleAndSphere(
                in Vector3 csb, in Vector3 csa, double capsuleRadius,
                in Vector3 sphereCenter, double sphereRadius)
            {
                var maxDistance = capsuleRadius + sphereRadius;
                // is in the middle
                Vector3 proj;
                if (point.TryProjectOnLineSegment(in sphereCenter, in csb, in csa, out proj))
                {
                    return distance.Between(in sphereCenter, in proj) <= maxDistance;
                }
                var aboveVec = (csa - csb).normalized;
                // is above
                if (point.IsAbovePlane(in sphereCenter, in aboveVec, in csa))
                {
                    return distance.Between(in sphereCenter, in csa) <= maxDistance;
                }
                var belowVec = -aboveVec;
                // is below
                if (point.IsAbovePlane(in sphereCenter, in belowVec, in csb))
                {
                    return distance.Between(in sphereCenter, in csb) <= maxDistance;
                }
                return false;
            }

            /// <summary>
            /// The points of the capsule are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="csb">capsule sphere below center</param>
            /// <param name="csa">capsule sphere above center</param>
            /// <param name="capsuleRadius">capsule radius</param>
            /// <param name="sphereCenter">sphere center point</param>
            /// <param name="sphereRadius">sphere radius</param>
            /// <param name="collision">the point of collision</param>
            /// <returns></returns>
            public static bool BetweenCapsuleAndSphere(
                in Vector3 csb, in Vector3 csa, double capsuleRadius,
                in Vector3 sphereCenter, double sphereRadius, out Vector3 collision)
            {
                var maxDistance = capsuleRadius + sphereRadius;
                // is in the middle
                Vector3 proj;
                if (point.TryProjectOnLineSegment(in sphereCenter, in csb, in csa, out proj))
                {
                    return HasOverlapOfTwoSpheres(
                        in sphereCenter, in proj, maxDistance, sphereRadius, out collision);
                }
                var aboveVec = (csa - csb).normalized;
                // is above
                if (point.IsAbovePlane(in sphereCenter, in aboveVec, in csa))
                {
                    return HasOverlapOfTwoSpheres(
                        in sphereCenter, in csa, maxDistance, sphereRadius, out collision);
                }
                var belowVec = -aboveVec;
                // is below
                if (point.IsAbovePlane(in sphereCenter, in belowVec, in csb))
                {
                    return HasOverlapOfTwoSpheres(
                        in sphereCenter, in csb, maxDistance, sphereRadius, out collision);
                }
                collision = Vector3.zero;
                return false;
            }

            private static bool HasOverlapOfTwoSpheres(
                in Vector3 sphereCenter1, in Vector3 sphereCenter2,
                double sumOfRadii, double sphereRadius1, out Vector3 collision)
            {
                var d = distance.Between(in sphereCenter1, in sphereCenter2);
                var has = d <= sumOfRadii;
                if (has)
                {
                    var ratio = sphereRadius1 / sumOfRadii;
                    point.TryMoveAbs(in sphereCenter1, in sphereCenter2, d * ratio, out collision);
                    return true;
                }
                collision = Vector3.zero;
                return false;
            }


            /// <summary>
            /// The points are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="c1sb">capsule 1 sphere below center</param>
            /// <param name="c1sa">capsule 1 sphere above center</param>
            /// <param name="radius1">radius of capsule 1 sphere</param>
            /// <param name="c2sb">capsule 2 sphere below center</param>
            /// <param name="c2sa">capsule 2 sphere above center</param>
            /// <param name="radius2">radius of capsule 2 sphere</param>
            /// <returns>true if there is overlap false otherwise</returns>
            public static bool BetweenCapsules(
                in Vector3 c1sb, in Vector3 c1sa, double radius1,
                in Vector3 c2sb, in Vector3 c2sa, double radius2)
            {
                Vector3 closest1, closest2;
                point.ClosestOnTwoLineSegments(in c1sb, in c1sa, in c2sb, in c2sa, out closest1, out closest2);
                var dist = distance.Between(in closest1, in closest2);
                if (dist < (radius1 + radius2))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// The points are the centers of the spheres at the end of capsules
            /// </summary>
            /// <param name="c1sb">capsule 1 sphere below center</param>
            /// <param name="c1sa">capsule 1 sphere above center</param>
            /// <param name="radius1">radius of capsule 1 sphere</param>
            /// <param name="c2sb">capsule 2 sphere below center</param>
            /// <param name="c2sa">capsule 2 sphere above center</param>
            /// <param name="radius2">radius of capsule 2 sphere</param>
            /// <param name="collision">The point of collision</param>
            /// <returns>true if there is overlap false otherwise</returns>
            public static bool BetweenCapsules(
                in Vector3 c1sb, in Vector3 c1sa, double radius1,
                in Vector3 c2sb, in Vector3 c2sa, double radius2, out Vector3 collision)
            {
                Vector3 closest1, closest2;
                point.ClosestOnTwoLineSegments(in c1sb, in c1sa, in c2sb, in c2sa, out closest1, out closest2);
                var dist = distance.Between(in closest1, in closest2);
                if (dist < (radius1 + radius2))
                {
                    if (dist < 0.000001)
                    {
                        collision = closest1;
                        return true;
                    }

                    collision = closest1 + (closest2 - closest1).normalized * (float)radius1;
                    return true;
                }
                collision = Vector3.zero;
                return false;
            }

            public static bool BetweenSpheres(
                in Vector3 sphereCenter1, double radius1,
                in Vector3 sphereCenter2, double radius2)
            {
                var sum = radius1 + radius2;
                if (sum < 0.0001)
                {
                    return sphereCenter1.IsEqual(sphereCenter2, 0.0001);
                }
                var dist = fun.distance.Between(in sphereCenter1, in sphereCenter2);
                return dist <= (radius1 + radius2);
            }

            public static bool BetweenSpheres(
                in Vector3 sphereCenter1, double radius1,
                in Vector3 sphereCenter2, double radius2, out Vector3 middle)
            {
                var sum = radius1 + radius2;
                if (sum < 0.0001)
                {
                    middle = sphereCenter1;
                    return sphereCenter1.IsEqual(sphereCenter2, 0.0001);
                }
                var dist = fun.distance.Between(in sphereCenter1, in sphereCenter2);
                middle = sphereCenter1.MoveTowards(sphereCenter2, (radius1 / sum) * dist);
                return dist <= (radius1 + radius2);
            }



            private static bool IsLineGettingCloserToOriginThan(in Vector2 a, in Vector2 b, float maxDist, out Vector2 projection)
            {
                var k = ((b.y - a.y) * -a.x - (b.x - a.x) * -a.y) / ((b.y - a.y) * (b.y - a.y) + (b.x - a.x) * (b.x - a.x));
                projection = new Vector2(-k * (b.y - a.y), k * (b.x - a.x));
                return projection.magnitude <= maxDist;
            }






            public static bool BetweenLines(in Vector3 ray1Origin, in Vector3 ray1Dir, in Vector3 ray2Origin, in Vector3 ray2Dir, out Vector3 intersection)
            {
                var lineVec3 = ray2Origin - ray1Origin;
                var crossVec1and2 = cross.Product(in ray1Dir, in ray2Dir);
                var crossVec3and2 = cross.Product(in lineVec3, in ray2Dir);

                var planarFactor = dot(in lineVec3, in crossVec1and2);

                //is coplanar, and not parrallel
                if (abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
                {
                    var s = dot(in crossVec3and2, in crossVec1and2) / crossVec1and2.sqrMagnitude;
                    intersection = ray1Origin + (ray1Dir * s);
                    return true;
                }
                intersection = Vector3.zero;
                return false;
            }

            public static bool BetweenTriangleAndLineSegment(
                in Vector3 p1, in Vector3 p2, in Vector3 p3,
                in Vector3 line1, in Vector3 line2)
            {
                var diff = line1 - line2;
                var rayForward = diff.normalized;
                var rayOrigin = line2;
                var rayCollides = BetweenTriangleAndRay(in p1, in p2, in p3, in rayForward, in rayOrigin);
                if (rayCollides)
                {
                    Vector3 planeNormal;
                    point.GetNormal(in p1, in p2, in p3, out planeNormal);
                    Vector3 collision;
                    if (BetweenPlaneAndRay(in planeNormal, in p1, in rayForward, in rayOrigin, out collision))
                    {
                        return (collision - rayOrigin).sqrMagnitude <= diff.sqrMagnitude;
                    }
                }
                return false;
            }

            public static bool BetweenTriangleAndLineSegment(
                in Vector3 p1, in Vector3 p2, in Vector3 p3,
                in Vector3 line1, in Vector3 line2, out Vector3 collision)
            {
                var diff = line1 - line2;
                var rayForward = diff.normalized;
                var rayOrigin = line2;
                var rayCollides = BetweenTriangleAndRay(in p1, in p2, in p3, in rayForward, in rayOrigin);
                if (rayCollides)
                {
                    Vector3 planeNormal;
                    point.GetNormal(in p1, in p2, in p3, out planeNormal);
                    if (BetweenPlaneAndRay(in planeNormal, in p1, in rayForward, in rayOrigin, out collision))
                    {
                        return (collision - rayOrigin).sqrMagnitude <= diff.sqrMagnitude;
                    }
                }
                collision = Vector3.zero;
                return false;
            }

            public static bool BetweenTriangleAndRay(in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 rayForward, in Vector3 rayOrigin)
            {
                //Find vectors for two edges sharing vertex/point p1
                var e1 = p2 - p1;
                var e2 = p3 - p1;

                // calculating determinant 
                var p = cross.Product(in rayForward, in e2);

                //Calculate determinat
                var det = dot(in e1, in p);

                //if determinant is near zero, ray lies in plane of triangle otherwise not
                if (det > -0.000001 && det < 0.000001) { return false; }
                var invDet = 1.0f / det;

                //calculate distance from p1 to ray origin
                var t = rayOrigin - p1;

                //Calculate u parameter
                var u = dot(in t, in p) * invDet;

                //Check for ray hit
                if (u < 0 || u > 1) { return false; }

                //Prepare to test v parameter
                var q = cross.Product(in t, in e1);

                //Calculate v parameter
                var v = dot(in rayForward, in q) * invDet;

                //Check for ray hit
                if (v < 0 || u + v > 1) { return false; }

                if ((dot(in e2, in q) * invDet) > 0.000001)
                {
                    //ray does intersect
                    return true;
                }

                // No hit at all
                return false;
            }

            public static bool BetweenPlaneAndLine(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 lineA, in Vector3 lineB, out Vector3 intersection)
            {
                planeNormal.Normalize();// that changes state but plane normal should always be normal
                var rayOrigin = lineA;
                var rayNormal = lineB - lineA;
                // if line points are the same
                if (rayNormal.sqrMagnitude < 0.00001)
                {
                    point.ProjectOnPlane(in lineA, in planeNormal, in planePoint, out intersection);
                    return true;
                }
                rayNormal.Normalize();
                var planeDistance = -dot(in planeNormal, in planePoint);
                var a = dot(in rayNormal, in planeNormal);
                var num = -dot(in rayOrigin, in planeNormal) - planeDistance;
                // if line is parallel to the plane
                if (a < 0.000001 && a > -0.000001)
                {
                    intersection = Vector3.zero;
                    return false;
                }
                var distanceToCollision = num / a;
                intersection = lineA + rayNormal * distanceToCollision;
                return true;
            }

            public static bool BetweenPlaneAndRay(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 rayNormal, in Vector3 rayOrigin, out float distanceToCollision)
            {
                var norm = planeNormal.normalized;
                var planeDistance = -dot(in norm, in planePoint);
                var a = dot(in rayNormal, in norm);
                var num = -dot(in rayOrigin, in norm) - planeDistance;
                if (a < 0.000001f && a > -0.000001f)
                {
                    distanceToCollision = 0.0f;
                    return false;
                }
                distanceToCollision = num / a;
                return distanceToCollision > 0.0;
            }

            public static bool BetweenPlaneAndRay(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 rayNormal, in Vector3 rayOrigin,
                out Vector3 collisionPoint)
            {
                var norm = planeNormal.normalized;
                var planeDistance = -dot(in norm, in planePoint);
                var a = dot(in rayNormal, in norm);
                var num = -dot(in rayOrigin, in norm) - planeDistance;
                if (a < 0.000001f && a > -0.000001f)
                {
                    collisionPoint = Vector3.zero;
                    return false;
                }
                var distanceToCollision = num / a;
                if (distanceToCollision > 0.0)
                {
                    collisionPoint = rayOrigin + rayNormal * distanceToCollision;
                    return true;
                }
                collisionPoint = Vector3.zero;
                return false;
            }

            public static bool BetweenPlaneAndRay(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 rayNormal, in Vector3 rayOrigin,
                out float distanceToCollision, out Vector3 collisionPoint)
            {
                var norm = planeNormal.normalized;
                var planeDistance = -dot(in norm, in planePoint);
                var a = dot(in rayNormal, in norm);
                var num = -dot(in rayOrigin, in norm) - planeDistance;
                if (a < 0.000001f && a > -0.000001f)
                {
                    collisionPoint = Vector3.zero;
                    distanceToCollision = 0;
                    return false;
                }
                distanceToCollision = num / a;
                if (distanceToCollision > 0.0)
                {
                    collisionPoint = rayOrigin + rayNormal * distanceToCollision;
                    return true;
                }
                distanceToCollision = 0;
                collisionPoint = Vector3.zero;
                return false;
            }

            public static bool BetweenPlaneAndLineSegment(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 line1, in Vector3 line2, out Vector3 collisionPoint)
            {
                var distanceToCollision = 0f;
                return
                    BetweenPlaneAndLineSegment(in planeNormal, in planePoint,
                        in line1, in line2,
                        out distanceToCollision, out collisionPoint);
            }
            public static bool BetweenPlaneAndLineSegment(
                in Vector3 planeNormal, in Vector3 planePoint,
                in Vector3 line1, in Vector3 line2, out float distanceToCollision, out Vector3 collisionPoint)
            {
                var rayNormal = (line2 - line1).normalized;
                var norm = planeNormal.normalized;
                var planeDistance = -dot(in norm, in planePoint);
                var a = dot(in rayNormal, in norm);
                var num = -dot(in line1, in norm) - planeDistance;
                if (a < 0.000001f && a > -0.000001f)
                {
                    collisionPoint = Vector3.zero;
                    distanceToCollision = 0;
                    return false;
                }
                distanceToCollision = num / a;
                if (distanceToCollision > 0.0000001 || distanceToCollision < -0.0000001)
                {
                    collisionPoint = line1 + rayNormal * distanceToCollision;
                    if (!point.IsOnSegment(in line1, in collisionPoint, in line2))
                    {
                        collisionPoint = Vector3.zero;
                        distanceToCollision = 0;
                        return false;
                    }
                    distanceToCollision = abs(distanceToCollision);
                    return true;
                }
                distanceToCollision = 0;
                collisionPoint = Vector3.zero;
                return false;
            }

            public static int BetweenLineAndUnitCircle(
                Vector2 point1, Vector2 point2,
                out Vector2 intersection1, out Vector2 intersection2)
            {
                float t;

                var dx = point2.x - point1.x;
                var dy = point2.y - point1.y;

                var a = dx * dx + dy * dy;
                var b = 2 * (dx * point1.x + dy * point1.y);
                var c = point1.x * point1.x + point1.y * point1.y - 1;

                var determinate = b * b - 4 * a * c;
                if ((a <= 0.0000001) || (determinate < -0.0000001))
                {
                    // No real solutions.
                    intersection1 = new Vector2(float.NaN, float.NaN);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 0;
                }
                if (determinate < 0.0000001 && determinate > -0.0000001)
                {
                    // One solution.
                    t = -b / (2 * a);
                    intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 1;
                }

                // Two solutions.
                t = (float)((-b + Math.Sqrt(determinate)) / (2 * a));
                intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                t = (float)((-b - Math.Sqrt(determinate)) / (2 * a));
                intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                return 2;
            }
            public static int BetweenLineAndCircle2d(
                Vector2 circleCenter, float circleRadius,
                Vector2 point1, Vector2 point2,
                out Vector2 intersection1, out Vector2 intersection2)
            {
                float t;

                var dx = point2.x - point1.x;
                var dy = point2.y - point1.y;

                var a = dx * dx + dy * dy;
                var b = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
                var c = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) + (point1.y - circleCenter.y) * (point1.y - circleCenter.y) - circleRadius * circleRadius;

                var determinate = b * b - 4 * a * c;
                if ((a <= 0.0000001) || (determinate < -0.0000001))
                {
                    // No real solutions.
                    intersection1 = new Vector2(float.NaN, float.NaN);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 0;
                }
                if (determinate < 0.0000001 && determinate > -0.0000001)
                {
                    // One solution.
                    t = -b / (2 * a);
                    intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 1;
                }

                // Two solutions.
                t = (float)((-b + Math.Sqrt(determinate)) / (2 * a));
                intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                t = (float)((-b - Math.Sqrt(determinate)) / (2 * a));
                intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                return 2;
            }

            public static int BetweenLineAndCircle2d(
                in Vector2 circleCenter, double circleRadius,
                in Vector2 point1, in Vector2 point2,
                out Vector2 intersection1, out Vector2 intersection2)
            {
                float t;

                var dx = point2.x - point1.x;
                var dy = point2.y - point1.y;

                var a = dx * dx + dy * dy;
                var b = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
                var c = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) + (point1.y - circleCenter.y) * (point1.y - circleCenter.y) - circleRadius * circleRadius;

                var determinate = b * b - 4 * a * c;
                if ((a <= 0.0000001) || (determinate < -0.0000001))
                {
                    // No real solutions.
                    intersection1 = new Vector2(float.NaN, float.NaN);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 0;
                }
                if (determinate < 0.0000001 && determinate > -0.0000001)
                {
                    // One solution.
                    t = -b / (2 * a);
                    intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                    intersection2 = new Vector2(float.NaN, float.NaN);
                    return 1;
                }

                // Two solutions.
                t = (float)((-b + Math.Sqrt(determinate)) / (2 * a));
                intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                t = (float)((-b - Math.Sqrt(determinate)) / (2 * a));
                intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                return 2;
            }

            public static bool BetweenLinesIgnoreY(
                   in Vector3 line1point1, in Vector3 line1point2,
                   in Vector3 line2point1, in Vector3 line2point2,
                   out Vector3? intersection)
            {
                var a1 = line1point2.z - line1point1.z;
                var b1 = line1point1.x - line1point2.x;
                var c1 = a1 * line1point1.x + b1 * line1point1.z;

                var a2 = line2point2.z - line2point1.z;
                var b2 = line2point1.x - line2point2.x;
                var c2 = a2 * line2point1.x + b2 * line2point1.z;

                var det = a1 * b2 - a2 * b1;
                const float epsilon = 0.00001f;
                if (det < epsilon && det > -epsilon)
                {
                    // Lines are parallel
                    intersection = null;
                    return false;
                }
                var x = (b2 * c1 - b1 * c2) / det;
                var z = (a1 * c2 - a2 * c1) / det;


                intersection = new Vector3(x, (line1point1.y + line1point2.y) / 2f, z);

                var maxX1 = Math.Max(line1point1.x, line1point2.x);
                var minX1 = Math.Min(line1point1.x, line1point2.x);
                var maxZ1 = Math.Max(line1point1.z, line1point2.z);
                var minZ1 = Math.Min(line1point1.z, line1point2.z);
                var maxX2 = Math.Max(line2point1.x, line2point2.x);
                var minX2 = Math.Min(line2point1.x, line2point2.x);
                var maxZ2 = Math.Max(line2point1.z, line2point2.z);
                var minZ2 = Math.Min(line2point1.z, line2point2.z);

                return x >= minX1 && x <= maxX1 &&
                       z >= minZ1 && z <= maxZ1 &&
                       x >= minX2 && x <= maxX2 &&
                       z >= minZ2 && z <= maxZ2;
            }

            public static bool BetweenPlanes(
                in Vector3 plane1Normal,
                in Vector3 plane2Normal,
                out Vector3 intersectionNormal)
            {
                intersectionNormal = cross.Product(in plane1Normal, in plane2Normal);
                intersectionNormal = intersectionNormal.normalized;
                return true;
            }
            public static bool BetweenPlanes(
                in Vector3 plane1Normal, in Vector3 plane1Position,
                in Vector3 plane2Normal, in Vector3 plane2Position,
                out Vector3 intersectionPoint, out Vector3 intersectionNormal)
            {
                intersectionPoint = Vector3.zero;

                //We can get the direction of the line of intersection of the two planes by calculating the 
                //cross product of the normals of the two planes. Note that this is just a direction and the line
                //is not fixed in space yet. We need a point for that to go with the line vector.
                cross.Product(in plane1Normal, in plane2Normal, out intersectionNormal);
                intersectionNormal.Normalize();


                //Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
                //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
                //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
                //the cross product of the normal of plane2 and the lineDirection.		
                Vector3 ldir = cross.Product(in plane2Normal, in intersectionNormal);

                var denominator = dot(in plane1Normal, in ldir);
                //Prevent divide by zero
                if (abs(denominator) > 0.00001f)
                {

                    var plane1ToPlane2 = plane1Position - plane2Position;
                    var t = dot(in plane1Normal, in plane1ToPlane2) / denominator;
                    intersectionPoint = plane2Position + t * ldir;
                    intersectionNormal = intersectionNormal.normalized;
                    return true;
                }
                return false;
            }

            public static float BetweenRayAndSphere(
                in Vector3 rayDirection,
                in Vector3 rayOrigin,
                in Vector3 sphereCenter,
                double radius)
            {
                var xDiff = sphereCenter.x - rayOrigin.x;
                var yDiff = sphereCenter.y - rayOrigin.y;
                var zDiff = sphereCenter.z - rayOrigin.z;
                var sumOfSquares = ((xDiff * xDiff) + (yDiff * yDiff)) + (zDiff * zDiff);
                var squareOfRadius = radius * radius;
                if (sumOfSquares <= squareOfRadius)
                {
                    return 0f;
                }
                var num = ((xDiff * rayDirection.x) + (yDiff * rayDirection.y)) + (zDiff * rayDirection.z);
                if (num < 0f)
                {
                    return Single.NaN;
                }
                var sqDiff = sumOfSquares - (num * num);
                if (sqDiff > squareOfRadius)
                {
                    return Single.NaN;
                }
                return num - (float)Math.Sqrt(squareOfRadius - sqDiff);
            }

            public static bool TryBetweenRayAndSphere(
                in Vector3 rayDirection,
                in Vector3 rayOrigin,
                in Vector3 sphereCenter,
                double radius)
            {
                var xDiff = sphereCenter.x - rayOrigin.x;
                var yDiff = sphereCenter.y - rayOrigin.y;
                var zDiff = sphereCenter.z - rayOrigin.z;
                var sumOfSquares = ((xDiff * xDiff) + (yDiff * yDiff)) + (zDiff * zDiff);
                var squareOfRadius = radius * radius;
                if (sumOfSquares <= squareOfRadius)
                {
                    return true;
                }
                var num = ((xDiff * rayDirection.x) + (yDiff * rayDirection.y)) + (zDiff * rayDirection.z);
                if (num < 0f)
                {
                    return false;
                }
                var sqDiff = sumOfSquares - (num * num);
                if (sqDiff > squareOfRadius)
                {
                    return false;
                }
                return true;
            }

            public static bool BetweenRayAndDisk(
                in Vector3 rayDirection,
                in Vector3 rayOrigin,
                in Vector3 diskNormal,
                in Vector3 diskCenter,
                double diskRadius,
                out Vector3 crossPoint)
            {
                Vector3 intersectPlane;
                var hasIntersection =
                    BetweenPlaneAndRay(
                        in diskNormal, in diskCenter,
                        in rayDirection, in rayOrigin, out intersectPlane);

                // if ray lies in the disk plane, then turn it into 2d problem
                if (abs(dot(in diskNormal, in rayDirection)) < 0.0001f)
                {
                    Vector3 x2d, y2d;
                    vector.ComputeRandomXYAxesForPlane(in diskNormal, out x2d, out y2d);

                    var line1 = rayOrigin.As2d(in diskCenter, in x2d, in y2d);
                    var line2 = (rayOrigin + rayDirection).As2d(in diskCenter, in x2d, in y2d);

                    var cirCen = Vector2.zero;
                    Vector2 int1, int2;

                    var found = BetweenLineAndCircle2d(in cirCen, diskRadius, in line1, in line2, out int1, out int2) > 0;

                    if (found)
                    {
                        var ds1 = distanceSquared.Between(in int1, in line1);
                        var ds2 = distanceSquared.Between(in int2, in line1);
                        crossPoint = (ds1 > ds2 ? int1 : int2).As3d(in diskCenter, in x2d, in y2d);
                    }
                    else
                    {
                        crossPoint = Vector3.zero;
                    }

                    return found;
                }

                if (!hasIntersection)
                {
                    crossPoint = Vector3.zero;
                    return false;
                }

                // the ray is intersecting disk plane in a point outside the disk
                if (distance.Between(in intersectPlane, in diskCenter) > diskRadius)
                {
                    crossPoint = Vector3.zero;
                    return false;
                }

                crossPoint = intersectPlane;
                return true;
            }

            // https://searchcode.com/codesearch/view/16225010/
            public static bool BetweenLineSegmentAndCone(
                in Vector3 lineStart, in Vector3 lineEnd,
                in Vector3 coneBase, in Vector3 coneUp, double coneRadius, double coneHeight,
                out Vector3 collision)
            {
                const float epsilon = 0.000001f;
                if (coneHeight < epsilon)
                {
                    if (coneRadius < epsilon)
                    {
                        Vector3 coneBaseOnLine;
                        point.ProjectOnLine(in coneBase, in lineEnd, in lineStart, out coneBaseOnLine);

                        var hasCollision = distance.Between(in coneBaseOnLine, in coneBase) < epsilon;
                        collision = hasCollision ? coneBaseOnLine : Vector3.zero;
                        return hasCollision;
                    }

                    return BetweenLineSegmentAndDisk(in lineEnd, in lineStart, in coneUp, in coneBase, coneRadius, out collision);
                }
                var coneNor = coneUp.normalized;
                var coneTop = coneBase + coneNor * (float)coneHeight;
                if (coneRadius < epsilon)
                {
                    Vector3 closest1, closest2;
                    point.ClosestOnTwoLineSegments(in lineEnd, in lineStart, in coneBase, in coneTop, out closest1, out closest2);

                    var hasCollision = distance.Between(in closest1, in closest2) < epsilon;
                    collision = hasCollision ? closest1 : Vector3.zero;
                    return hasCollision;
                }

                var coneRadiusSqr = coneRadius * coneRadius;
                var coneHeightSqr = coneHeight * coneHeight;

                var fac = coneRadiusSqr / coneHeightSqr;

                var rayOr = lineStart;
                var rayVec = lineEnd - lineStart;
                var rayMag = vector.Magnitude(in rayVec);
                var rayFw = rayMag < 0.000001 ? Vector3.up : rayVec / rayMag;


                // cylinder part
                var yA = (fac) * rayFw.y * rayFw.y;
                var yB = (2 * fac * rayOr.y * rayFw.y) - (2 * coneRadiusSqr / coneHeight) * rayFw.y;
                var yC = (fac * rayOr.y * rayOr.y) - ((2 * coneRadiusSqr / coneHeight) * rayOr.y) + coneRadiusSqr;

                var A = (rayFw.x * rayFw.x) + (rayFw.z * rayFw.z) - yA;
                var B = (2 * rayOr.x * rayFw.x) + (2 * rayOr.z * rayFw.z) - yB;
                var C = (rayOr.x * rayOr.x) + (rayOr.z * rayOr.z) - yC;

                var d = (B * B) - (4 * A * C);

                var t = new float[] { 0.0f, 0.0f };
                var t_near = float.MaxValue;

                var near = Vector3.zero;
                var tempNear = Vector3.zero;

                if (d >= 0)
                {
                    t[0] = (float)((-B - Math.Sqrt(d)) / (2 * A));
                    t[1] = (float)((-B + Math.Sqrt(d)) / (2 * A));

                    for (var i = 0; i < 2; ++i)
                    {
                        if (t[i] > epsilon)
                        {
                            // So it doesn't cast shadows on itself
                            // find intersection
                            tempNear = rayOr + t[i] * rayFw;
                            if (tempNear.y <= coneHeight && tempNear.y >= 0) // valid intersection point
                            {

                                if (t[i] < t_near)
                                {
                                    t_near = t[i];
                                    near = tempNear;
                                    if (point.IsOnSegment(in lineEnd, in near, in lineStart))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // base
                var capCen = new Vector3(0, 0, 0);
                var capNor = new Vector3(0, -1, 0);

                Vector3 col;
                var hit = BetweenRayAndCircle(in rayFw, in rayOr, in capCen, coneRadius, in capNor, out col);
                //if(hit) Debug.DrawLine(Vector3.one*999,col, Color.magenta);
                var t_cap = distance.Between(in rayOr, in col);

                if (hit)
                {
                    if (t_cap > t_near)
                    {
                        t_near = t_cap;
                        near = tempNear;
                    }
                    else
                    {
                        t_near = t_cap;
                        near = col;
                    }
                }
                //Debug.Log("t_near "+t_near+"|"+near);
                if (t_near < float.MaxValue)
                {
                    //Debug.DrawLine(Vector3.one*999,near, Color.magenta,9999,false);
                    if (point.IsOnSegment(in lineEnd, in near, in lineStart))
                    {
                        collision = near;
                        return true;
                    }
                }

                if (BetweenPointAndCone(in lineStart, in coneBase, in coneNor, coneRadius, coneHeight) &&
                    BetweenPointAndCone(in lineEnd, in coneBase, in coneNor, coneRadius, coneHeight))
                {
                    collision = lineStart;
                    return true;
                }

                collision = Vector3.zero;
                return false;
            }

            private static bool BetweenPointAndCone(in Vector3 testPoint, in Vector3 coneBase, in Vector3 coneNor, double coneRadius, double coneHeight)
            {
                var coneTop = coneBase + coneNor * (float)coneHeight;
                Vector3 testPointProj;
                point.ProjectOnLine(in testPoint, in coneBase, in coneTop, out testPointProj);

                var ratio = (1f - (distance.Between(in testPointProj, in coneBase) / (float)coneHeight)).Clamp01();
                var currRadius = coneRadius * ratio;

                return distance.Between(in testPoint, in testPointProj) <= currRadius;
            }

            // https://searchcode.com/file/16224990/src/utils.cpp
            public static bool BetweenRayAndCircle(
                in Vector3 rayDirection, in Vector3 rayOrigin,
                in Vector3 circleCenter, double circleRadius, in Vector3 circleNormal, out Vector3 collision)
            {
                var denom = dot(in rayDirection, in circleNormal);
                var v1 = circleCenter - rayOrigin;
                var num = dot(in v1, in circleNormal);

                const float epsilon = 0.00001f;

                if ((denom <= -epsilon || denom >= epsilon) && (num <= -epsilon || num >= epsilon))
                {
                    var t = num / denom;
                    if (t > epsilon)
                    {
                        // find intersection
                        var near = rayOrigin + t * rayDirection;
                        if ((near.x * near.x) + (near.z * near.z) <= (circleRadius * circleRadius))
                        {
                            collision = near;
                            return true;
                        }

                    }
                }
                collision = Vector3.zero;
                return false;
            }


            public static bool BetweenLineSegmentAndDisk(
                in Vector3 lineEnd1,
                in Vector3 lineEnd2,
                in Vector3 diskNormal,
                in Vector3 diskCenter,
                double diskRadius,
                out Vector3 crossPoint)
            {
                Vector3 rayDiskIntersection;
                var rayDirection = (lineEnd1 - lineEnd2);
                // if 2 ends of a line are the same point, then check if that point lies on the disk
                if (rayDirection.sqrMagnitude < 0.00001)
                {
                    Vector3 projection;
                    point.ProjectOnPlane(in lineEnd1, in diskNormal, in diskCenter, out projection);
                    if (distanceSquared.Between(in projection, in lineEnd1) < 0.0001)
                    {
                        crossPoint = lineEnd1;
                        return true;
                    }
                    crossPoint = Vector3.zero;
                    return false;
                }
                var hasIntersection =
                    BetweenRayAndDisk(
                        in rayDirection,
                        in lineEnd2,
                        in diskNormal,
                        in diskCenter,
                        diskRadius,
                        out rayDiskIntersection);
                if (!hasIntersection)
                {
                    crossPoint = Vector3.zero;
                    return false;
                }
                // the ray is intersecting disk within that line segment
                if (point.IsOnSegment(in lineEnd1, in rayDiskIntersection, in lineEnd2))
                {
                    crossPoint = rayDiskIntersection;
                    return true;
                }
                crossPoint = Vector3.zero;
                return false;
            }

            public static bool BetweenLineSegmentAndDisk(
                in Vector3 lineEnd1,
                in Vector3 lineEnd2,
                in Vector3 diskNormal,
                in Vector3 diskCenter,
                double diskRadius)
            {
                Vector3 rayDiskIntersection;
                var rayDirection = (lineEnd1 - lineEnd2);
                // if 2 ends of a line are the same point, then check if that point lies on the disk
                if (rayDirection.sqrMagnitude < 0.00001)
                {
                    Vector3 projection;
                    point.ProjectOnPlane(in lineEnd1, in diskNormal, in diskCenter, out projection);
                    if (distanceSquared.Between(in projection, in lineEnd1) < 0.0001)
                    {
                        return true;
                    }
                    return false;
                }
                var hasIntersection =
                    BetweenRayAndDisk(
                        in rayDirection,
                        in lineEnd2,
                        in diskNormal,
                        in diskCenter,
                        diskRadius,
                        out rayDiskIntersection);
                if (!hasIntersection)
                {
                    return false;
                }
                // the ray is intersecting disk within that line segment
                if (point.IsOnSegment(in lineEnd1, in rayDiskIntersection, in lineEnd2))
                {
                    return true;
                }
                return false;
            }

            public static bool BetweenLineSegmentAndPlane(
                in Vector3 lineEnd1,
                in Vector3 lineEnd2,
                in Vector3 planeNormal,
                in Vector3 planePoint,
                out Vector3 crossPoint)
            {
                Vector3 rayDiskIntersection;
                var rayDirection = (lineEnd1 - lineEnd2);
                // if 2 ends of a line are the same point, then check if that point lies on the disk
                if (rayDirection.sqrMagnitude < 0.00001)
                {
                    Vector3 projection;
                    point.ProjectOnPlane(in lineEnd1, in planeNormal, in planePoint, out projection);
                    if (distanceSquared.Between(in projection, in lineEnd1) < 0.0001)
                    {
                        crossPoint = lineEnd1;
                        return true;
                    }
                    crossPoint = Vector3.zero;
                    return false;
                }
                var hasIntersection =
                    BetweenPlaneAndRay(
                        in planeNormal,
                        in planePoint,
                        in rayDirection,
                        in lineEnd2,
                        out rayDiskIntersection);
                if (!hasIntersection)
                {
                    crossPoint = Vector3.zero;
                    return false;
                }
                // the ray is intersecting disk within that line segment
                if (point.IsOnSegment(in lineEnd1, in rayDiskIntersection, in lineEnd2))
                {
                    crossPoint = rayDiskIntersection;
                    return true;
                }
                crossPoint = Vector3.zero;
                return false;
            }


            public static bool Between2DLines(Vector2 line1p1, Vector2 line1p2, Vector2 line2p1, Vector2 line2p2)
            {
                return Between2DLines(in line1p1, in line1p2, in line2p1, in line2p2);
            }
            public static bool Between2DLines(in Vector2 line1p1, in Vector2 line1p2, in Vector2 line2p1, in Vector2 line2p2)
            {
                // Find the four orientations needed for general and
                // special cases
                var o1 = Orientation(in line1p1, in line1p2, in line2p1);
                var o2 = Orientation(in line1p1, in line1p2, in line2p2);
                var o3 = Orientation(in line2p1, in line2p2, in line1p1);
                var o4 = Orientation(in line2p1, in line2p2, in line1p2);

                // General case
                if (o1 != o2 && o3 != o4)
                    return true;

                // Special Cases : colinear
                if (o1 == 0 && point.IsOn2DSegment(in line1p1, in line2p1, in line1p2)) return true;

                if (o2 == 0 && point.IsOn2DSegment(in line1p1, in line2p2, in line1p2)) return true;

                if (o3 == 0 && point.IsOn2DSegment(in line2p1, in line1p1, in line2p2)) return true;

                if (o4 == 0 && point.IsOn2DSegment(in line2p1, in line1p2, in line2p2)) return true;

                return false; // Doesn't fall in any of the above cases
            }
            public static bool Between2DLines(Vector2 lineA1, Vector2 lineA2, Vector2 lineB1, Vector2 lineB2, out Vector2 intersect)
            {
                var a1 = (double)lineA2.y - (double)lineA1.y;
                var b1 = (double)lineA1.x - (double)lineA2.x;
                var c1 = a1 * (double)lineA1.x + b1 * (double)lineA1.y;

                var a2 = (double)lineB2.y - (double)lineB1.y;
                var b2 = (double)lineB1.x - (double)lineB2.x;
                var c2 = a2 * (double)lineB1.x + b2 * (double)lineB1.y;

                var det = a1 * b2 - a2 * b1;
                // if lines are parallel
                if (det < 0.00001f && det > -0.00001f)
                {
                    intersect = Vector2.zero;
                    return false;
                }
                var x = (b2 * c1 - b1 * c2) / det;
                var y = (a1 * c2 - a2 * c1) / det;
                intersect = new Vector2((float)x, (float)y);

                return
                    intersect.x >= Mathf.Min(lineA1.x, lineA2.x) &&
                    intersect.x <= Mathf.Max(lineA1.x, lineA2.x) &&
                    intersect.y >= Mathf.Min(lineA1.y, lineA2.y) &&
                    intersect.y <= Mathf.Max(lineA1.y, lineA2.y) &&
                    intersect.x >= Mathf.Min(lineB1.x, lineB2.x) &&
                    intersect.x <= Mathf.Max(lineB1.x, lineB2.x) &&
                    intersect.y >= Mathf.Min(lineB1.y, lineB2.y) &&
                    intersect.y <= Mathf.Max(lineB1.y, lineB2.y);
            }
            public static bool Between2DLines(in Vector2 lineA1, in Vector2 lineA2, in Vector2 lineB1, in Vector2 lineB2, out Vector2 intersect)
            {
                var a1 = (double)lineA2.y - (double)lineA1.y;
                var b1 = (double)lineA1.x - (double)lineA2.x;
                var c1 = a1 * (double)lineA1.x + b1 * (double)lineA1.y;

                var a2 = (double)lineB2.y - (double)lineB1.y;
                var b2 = (double)lineB1.x - (double)lineB2.x;
                var c2 = a2 * lineB1.x + b2 * lineB1.y;
                var det = a1 * b2 - a2 * b1;
                // if lines are parallel
                if (det < 0.00001 && det > -0.00001)
                {
                    intersect = Vector2.zero;
                    return false;
                }
                var x = (b2 * c1 - b1 * c2) / det;
                var y = (a1 * c2 - a2 * c1) / det;
                intersect = new Vector2((float)x, (float)y);
                return
                    intersect.x >= Mathf.Min(lineA1.x, lineA2.x) &&
                    intersect.x <= Mathf.Max(lineA1.x, lineA2.x) &&
                    intersect.y >= Mathf.Min(lineA1.y, lineA2.y) &&
                    intersect.y <= Mathf.Max(lineA1.y, lineA2.y) &&
                    intersect.x >= Mathf.Min(lineB1.x, lineB2.x) &&
                    intersect.x <= Mathf.Max(lineB1.x, lineB2.x) &&
                    intersect.y >= Mathf.Min(lineB1.y, lineB2.y) &&
                    intersect.y <= Mathf.Max(lineB1.y, lineB2.y);
            }


            // To find orientation of ordered triplet (p, q, r).
            // The function returns following values
            // 0 --> p, q and r are colinear
            // 1 --> Clockwise
            // 2 --> Counterclockwise
            private static int Orientation(in Vector2 p, in Vector2 q, in Vector2 r)
            {
                // See http://www.geeksforgeeks.org/orientation-3-ordered-points/
                // for details of below formula.
                var val = ((double)q.y - (double)p.y) * ((double)r.x - (double)q.x) - ((double)q.x - (double)p.x) * ((double)r.y - (double)q.y);

                if (val < 0.000001 && val > -0.000001) return 0;  // colinear

                return (val > 0) ? 1 : 2; // clock or counterclock wise
            }

            public static bool BetweenTriangleAndSphere(
                in Vector3 t1, in Vector3 t2, in Vector3 t3,
                in Vector3 sphereCenter, double sphereRadius,
                out Vector3 collision)
            {
                Vector3 triangleNormal;
                point.GetNormal(in t1, in t2, in t3, out triangleNormal);

                Vector3 proj;
                point.ProjectOnPlane(in sphereCenter, in triangleNormal, in t1, out proj);
                var x2d = (t2 - t1).normalized;
                Vector3 y2d;
                vector.GetNormal(in x2d, in triangleNormal, out y2d);

                var dist = distance.Between(in sphereCenter, in proj);
                var ratio = dist / sphereRadius;
                if (ratio <= 1.0)
                {

                    var t1in2d = Vector2.zero;
                    var t2in2d = (t2 - t1).As2d(in x2d, in y2d);
                    var t3in2d = (t3 - t1).As2d(in x2d, in y2d);
                    var spin2d = (sphereCenter - t1).As2d(in x2d, in y2d);
                    var radius2d = (float)Math.Sqrt(1.0 - ratio * ratio) * sphereRadius;
                    if (HasCircleTriangleCollision2D(in spin2d, radius2d, in t1in2d, in t2in2d, in t3in2d))
                    {
                        Vector2 int2d;
                        GetCircleTriangle2DIntersectionPoint(in spin2d, radius2d, in t1in2d, in t2in2d, in t3in2d, out int2d);
                        //for (var a = 0; a < 360; a += 5)
                        //{
                        //Debug.DrawLine(
                        //    spin2d+(Vector2)((Vector3)(Vector2.right*radius2d)).RotateAbout(Vector3.forward, a), 
                        //    spin2d+(Vector2)((Vector3)(Vector2.right*radius2d)).RotateAbout(Vector3.forward, a+5), 
                        //    Color.red,0, false);
                        //}
                        //Debug.DrawLine(t1in2d,t2in2d,Color.black,0, false);
                        //Debug.DrawLine(t2in2d,t3in2d,Color.black,0, false);
                        //Debug.DrawLine(t3in2d,t1in2d,Color.black,0, false);
                        //Debug.DrawLine(Vector2.one*100,int2d,Color.magenta,0, false);
                        int2d.As3d(in t1, in x2d, in y2d, out collision);
                        return true;
                    }
                }
                collision = Vector3.zero;
                return false;
            }

            public static bool BetweenTriangleAndDisk(
                in Vector3 t1, in Vector3 t2, in Vector3 t3,
                in Vector3 diskNormal, in Vector3 diskCenter, float diskRadius,
                out Vector3 intersect)
            {
                Vector3 triangleNormal;
                point.GetNormal(in t1, in t2, in t3, out triangleNormal);
                diskNormal.Normalize(); // ensure disk normal is unit vector
                // they are parallel
                if (abs(dot(in triangleNormal, in diskNormal)) > 0.999999f)
                {
                    var diskCenterPlus = diskCenter + diskNormal;
                    Vector3 proj;
                    point.ProjectOnLine(in t1, in diskCenter, in diskCenterPlus, out proj);
                    // they lie on the same plane
                    if (distanceSquared.Between(in proj, in diskCenter) < 0.0001)
                    {
                        Vector3 x2d, y2d;
                        vector.ComputeRandomXYAxesForPlane(in diskNormal, out x2d, out y2d);

                        var t1in2d = Vector2.zero;
                        var t2in2d = (t2 - t1).As2d(in x2d, in y2d);
                        var t3in2d = (t3 - t1).As2d(in x2d, in y2d);
                        var dcin2d = (diskCenter - t1).As2d(in x2d, in y2d);

                        if (HasCircleTriangleCollision2D(in dcin2d, diskRadius, in t1in2d, in t2in2d, in t3in2d))
                        {
                            Vector2 int2d;
                            GetCircleTriangle2DIntersectionPoint(in dcin2d, diskRadius, in t1in2d, in t2in2d, in t3in2d, out int2d);
                            intersect = int2d.As3d(in t1, in x2d, in y2d);
                            return true;
                        }
                    }
                }
                Vector3 intPoint, intNorm;
                var linesIntersect = BetweenPlanes(in triangleNormal, in t1, in diskNormal, in diskCenter, out intPoint, out intNorm);
                if (linesIntersect)
                {
                    var distToInt = distance.Between(in intPoint, in diskCenter);
                    var ratio = distToInt / diskRadius;
                    if (ratio <= 1.0)
                    {
                        var side = (float)Math.Sqrt(1.0 - ratio * ratio) * diskRadius;
                        var xPos = diskCenter + (intPoint - diskCenter);
                        var w1 = xPos + intNorm * side;
                        var w2 = xPos + intNorm * -side;

                        var x2d = intNorm;
                        Vector3 y2d;
                        vector.GetNormal(in intNorm, in triangleNormal, out y2d);

                        var t1in2d = Vector2.zero;
                        var t2in2d = (t2 - t1).As2d(in x2d, in y2d);
                        var t3in2d = (t3 - t1).As2d(in x2d, in y2d);
                        var w1in2d = (w1 - t1).As2d(in x2d, in y2d);
                        var w2in2d = (w2 - t1).As2d(in x2d, in y2d);
                        //Debug.DrawLine(Vector3.one*100, w1in2d, Color.red, 0, false);
                        //Debug.DrawLine(Vector3.one*100, w2in2d, Color.black, 0, false);

                        Vector2 int2d;
                        if (Between2DTriangleAndLineSegment(
                            in t1in2d, in t2in2d, in t3in2d,
                            in w1in2d, in w2in2d, out int2d))
                        {
                            intersect = int2d.As3d(in t1, in x2d, in y2d);
                            return true;
                        }
                    }
                }
                intersect = Vector3.zero;
                return false;
            }
            public static bool Between2DTriangleAndLineSegment(
                in Vector2 t1, in Vector2 t2, in Vector2 t3,
                in Vector2 line1, in Vector2 line2,
                out Vector2 intersect)
            {
                Vector2 curr;
                var c = Vector2.zero;
                var b1 = false;
                var b2 = false;
                var b3 = false;
                if (Between2DLines(in t1, in t2, in line1, in line2, out curr))
                {
                    b1 = true;
                    c = curr;
                }
                if (Between2DLines(in t2, in t3, in line1, in line2, out curr))
                {
                    b2 = true;
                    c = b1 ? point.Middle2D(c, curr) : curr;
                }
                if (Between2DLines(in t3, in t1, in line1, in line2, out curr))
                {
                    b3 = true;
                    c = b1 || b2 ? point.Middle2D(c, curr) : curr;
                }
                if (b1 || b2 || b3)
                {
                    intersect = c;
                    return true;
                }

                if (triangle.IsPointInside(in line1, in t1, in t2, in t3) ||
                    triangle.IsPointInside(in line2, in t1, in t2, in t3))
                {
                    Vector2 wcin2d;
                    point.Middle2D(in line1, in line2, out wcin2d);
                    intersect = wcin2d;
                    return true;
                }
                intersect = Vector2.zero;
                return false;
            }
            private static bool GetCircleTriangle2DIntersectionPoint(
                in Vector2 circleCenter, double radius,
                in Vector2 t1, in Vector2 t2, in Vector2 t3, out Vector2 intersect)
            {
                intersect = Vector2.zero;
                var n1 = GetLineCircleIntersectionPoint(
                        false, in circleCenter, radius, in t1, in t2, ref intersect);
                var n2 = GetLineCircleIntersectionPoint(
                        n1 > 0, in circleCenter, radius, in t2, in t3, ref intersect);
                var n3 = GetLineCircleIntersectionPoint(
                        n1 + n2 > 0, in circleCenter, radius, in t3, in t1, ref intersect);
                var hasIntersection = (n1 + n2 + n3) > 0;
                if (hasIntersection)
                {
                    return true;
                }

                // if the circle is inside the triangle
                if (distanceSquared.Between(in t1, in circleCenter) > radius * radius)
                {
                    intersect = circleCenter;
                    return false;
                }
                // if the triangle is inside the circle
                Vector2 centroid;
                triangle.GetCentroid2D(in t1, in t2, in t3, out centroid);
                intersect = centroid;
                return false;
            }
            private static int GetLineCircleIntersectionPoint(
                bool hasPrevious,
                in Vector2 circleCenter, double curcleRadius,
                in Vector2 point1, in Vector2 point2, ref Vector2 output)
            {
                Vector2 intersection1, intersection2;
                var num =
                    BetweenLineSegmentAndCircle2D(
                        in circleCenter, curcleRadius,
                        in point1, in point2,
                        out intersection1, out intersection2);
                var current = Vector2.zero;
                if (num == 1) current = intersection1;
                else if (num == 2) point.Middle2D(in intersection1, in intersection2, out current);
                if (num > 0)
                {
                    if (hasPrevious) point.Middle2D(in output, in current, out output);
                    else output = current;
                }
                return num;
            }
            public static int BetweenLineSegmentAndCircle2D(
                in Vector2 circleCenter, double circleRadius,
                in Vector2 point1, in Vector2 point2,
                out Vector2 intersection1, out Vector2 intersection2)
            {
                // test to see if line is inside the circle
                var dsq = distanceSquared.Between(in circleCenter, in point1);
                var circleRadiusSqr = circleRadius * circleRadius;
                if (dsq < circleRadiusSqr)
                {
                    dsq = distanceSquared.Between(in circleCenter, in point2);
                    if (dsq < circleRadiusSqr)
                    {
                        intersection1 = Vector2.zero;
                        intersection2 = Vector2.zero;
                        return 0;
                    }
                }

                var dx = point2.x - point1.x;
                var dy = point2.y - point1.y;

                var a = dx * dx + dy * dy;
                var b = 2 * (dx * (point1.x - circleCenter.x) + dy * (point1.y - circleCenter.y));
                var c = (point1.x - circleCenter.x) * (point1.x - circleCenter.x) + (point1.y - circleCenter.y) * (point1.y - circleCenter.y) - circleRadius * circleRadius;

                var determinate = b * b - 4 * a * c;
                if ((a <= 0.0000001) || (determinate < -0.0000001))
                {
                    // No real solutions.
                    intersection1 = Vector2.zero;
                    intersection2 = Vector2.zero;
                    return 0;
                }
                float t;
                if (determinate < 0.0000001 && determinate > -0.0000001)
                {
                    // One solution.
                    t = -b / (2 * a);
                    intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                    point.EnforceWithin(ref intersection1, in point1, in point2);
                    intersection2 = Vector2.zero;
                    return 1;
                }

                // Two solutions.
                t = (float)((-b + Math.Sqrt(determinate)) / (2 * a));
                intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                point.EnforceWithin(ref intersection1, in point1, in point2);
                t = (float)((-b - Math.Sqrt(determinate)) / (2 * a));
                intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
                point.EnforceWithin(ref intersection2, in point1, in point2);
                return 2;
            }
            private static bool HasCircleTriangleCollision2D(in Vector2 centre, double radius, in Vector2 t1, in Vector2 t2, in Vector2 t3)
            {
                //
                // TEST 1: Vertex within circle
                //
                var c1x = centre.x - t1.x;
                var c1y = centre.y - t1.y;

                var radiusSqr = radius * radius;
                var c1sqr = c1x * c1x + c1y * c1y - radiusSqr;

                if (c1sqr <= 0) return true;

                var c2x = centre.x - t2.x;
                var c2y = centre.y - t2.y;
                var c2sqr = c2x * c2x + c2y * c2y - radiusSqr;

                if (c2sqr <= 0) return true;

                var c3x = centre.x - t3.x;
                var c3y = centre.y - t3.y;
                var c3sqr = c3x * c3x + c3y * c3y - radiusSqr;

                if (c3sqr <= 0) return true;

                //
                // TEST 2: Circle centre within triangle
                //

                //
                // Calculate edges
                //
                if (fun.triangle.IsPointInside(in centre, in t1, in t2, in t3)) return true;


                //
                // TEST 3: Circle intersects edge
                //
                var e1x = t2.x - t1.x;
                var e1y = t2.y - t1.y;

                var e2x = t3.x - t2.x;
                var e2y = t3.y - t2.y;

                var e3x = t1.x - t3.x;
                var e3y = t1.y - t3.y;

                var k = c1x * e1x + c1y * e1y;

                if (k > 0)
                {
                    var len = e1x * e1x + e1y * e1y;     // squared len

                    if (k < len)
                    {
                        if ((c1sqr * len) <= k * k)
                            return true;
                    }
                }

                // Second edge
                k = c2x * e2x + c2y * e2y;

                if (k > 0)
                {
                    var len = e2x * e2x + e2y * e2y;

                    if (k < len)
                    {
                        if ((c2sqr * len) <= k * k)
                            return true;
                    }
                }

                // Third edge
                k = c3x * e3x + c3y * e3y;

                if (k > 0)
                {
                    var len = e3x * e3x + e3y * e3y;

                    if (k < len)
                    {
                        if ((c3sqr * len) <= k * k)
                            return true;
                    }
                }
                // We're done, no intersection
                return false;
            }
            /// <summary>
            /// note the collision point returned is not precise, but is good enough for my needs
            /// </summary>
            public static bool BetweenTriangles(
                in Vector3 t1p1, in Vector3 t1p2, in Vector3 t1p3,
                in Vector3 t2p1, in Vector3 t2p2, in Vector3 t2p3,
                out Vector3 collision)
            {
                if (triangle.Overlap(
                    in t1p1, in t1p2, in t1p3,
                    in t2p1, in t2p2, in t2p3))
                {
                    Vector3 normalT1, normalT2;
                    point.GetNormal(in t1p1, in t1p2, in t1p3, out normalT1);
                    point.GetNormal(in t2p1, in t2p2, in t2p3, out normalT2);

                    // then are on the same plane
                    if (abs(dot(in normalT1, in normalT2)) > 0.999999f)
                    {
                        Vector3 cen1, cen2;
                        triangle.GetCentroid(in t1p1, in t1p2, in t1p3, out cen1);
                        triangle.GetCentroid(in t2p1, in t2p2, in t2p3, out cen2);

                        point.Middle(in cen1, in cen2, out collision);

                        return true;
                    }

                    Vector3 intPoint, intNormal;
                    BetweenPlanes(in normalT1, in t1p1, in normalT2, in t2p1, out intPoint, out intNormal);

                    var l1 = (intPoint + intNormal * 999);
                    var l2 = (intPoint - intNormal * 999);

                    Vector3 x2d, y2d;
                    vector.ComputeRandomXYAxesForPlane(in normalT1, out x2d, out y2d);
                    var t12d = Vector2.zero;
                    var t22d = (t1p2 - t1p1).As2d(in x2d, in y2d);
                    var t32d = (t1p3 - t1p1).As2d(in x2d, in y2d);
                    var w12d = (l1 - t1p1).As2d(in x2d, in y2d);
                    var w22d = (l2 - t1p1).As2d(in x2d, in y2d);

                    Vector2 int2d;
                    Between2DTriangleAndLineSegment(in t12d, in t22d, in t32d, in w12d, in w22d, out int2d);
                    var p1 = int2d.As3d(in t1p1, in x2d, in y2d);

                    vector.ComputeRandomXYAxesForPlane(in normalT2, out x2d, out y2d);
                    t22d = (t2p2 - t2p1).As2d(in x2d, in y2d);
                    t32d = (t2p3 - t2p1).As2d(in x2d, in y2d);
                    w12d = (l1 - t2p1).As2d(in x2d, in y2d);
                    w22d = (l2 - t2p1).As2d(in x2d, in y2d);

                    Between2DTriangleAndLineSegment(in t12d, in t22d, in t32d, in w12d, in w22d, out int2d);
                    var p2 = int2d.As3d(in t2p1, in x2d, in y2d);
                    //Debug.DrawLine(Vector3.one*100, p1, Color.red, 0, false);
                    //Debug.DrawLine(Vector3.one*100, p2, Color.black, 0, false);
                    point.Middle(in p1, in p2, out collision);
                    return true;
                }

                collision = Vector3.zero;
                return false;
            }

            public static bool BetweenCircles2D(in Vector2 p0, float radius0, in Vector2 p1, float radius1, out Vector2 cross0, out Vector2 cross1)
            {
                var d = distance.Between2D(in p0, in p1);
                var radiusesSum = radius0 + radius1;
                const float delta = 0.00001f;
                var radiusesDiff = abs(radius0 - radius1);
                if (d > radiusesSum || d < (radiusesDiff + delta))
                {
                    cross0 = cross1 = Vector2.zero;
                    return false;
                }
                var a = (radius0 * radius0 - radius1 * radius1 + d * d) / (2 * d);
                var h = sqrt(radius0 * radius0 - a * a);
                var p2 = ((p1 - p0) * (a / d)) + p0;
                var x3 = p2.x + h * (p1.y - p0.y) / d;
                var y3 = p2.y - h * (p1.x - p0.x) / d;
                var x4 = p2.x - h * (p1.y - p0.y) / d;
                var y4 = p2.y + h * (p1.x - p0.x) / d;

                cross0 = new Vector2(x3, y3);
                cross1 = new Vector2(x4, y4);
                return true;
            }
        }

    }
}