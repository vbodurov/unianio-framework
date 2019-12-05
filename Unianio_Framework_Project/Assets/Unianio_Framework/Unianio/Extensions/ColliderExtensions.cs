using System;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class ColliderExtensions
    {

        public static bool ClosestOnSurfacePointAndNormal(this Collider collider, in Vector3 point, out Vector3 onSurface, out Vector3 normal)
        {
            var sc = collider as SphereCollider;
            if(sc != null)
            {
                var cen = sc.center.AsWorldPoint(sc.transform);
                normal = (point - cen).normalized;
                if (collider.IsPointInside(point))
                {
                    
                    var sca = sc.transform.localScale;
                    var sc1 = Math.Max(Math.Max(sca.x, sca.y), sca.z);
                    var r = sc.radius * sc1;
                    onSurface = cen + normal * r;
                    return false;
                }
                onSurface = sc.ClosestPoint(point);
                return true;
            }
            var cc = collider as CapsuleCollider;
            if(cc != null)
            {     
//Debug.DrawLine(cp1, cp2, Color.magenta, 0, true);
                if(collider.IsPointInside(point))
                {
                    var cen = cc.center.AsWorldPoint(cc.transform);
                    var sca = cc.transform.localScale;
                    var dir = cc.direction == 0 ? new Vector3(1, 0, 0) : cc.direction == 1 ? new Vector3(0, 1, 0) : new Vector3(0, 0, 1);
                    var sc1 = cc.direction == 0 ? sca.x : cc.direction == 1 ? sca.y : sca.z;
                    var sc2 = cc.direction == 0 ? Math.Max(sca.y, sca.z) : cc.direction == 1 ? Math.Max(sca.x, sca.z) : Math.Max(sca.x, sca.y);
                    var h = cc.height * sc1;
                    var r = cc.radius * sc2;
                    var vec = (cc.transform.rotation * dir) * (h / 2f - r);
                    var cp1 = cen + vec;
                    var cp2 = cen - vec;
//Debug.DrawLine(v3.one*100, point, Color.magenta, 0, true);
                    if (fun.point.IsAbovePlane(in point, in dir, in cp1))
                    {
                        normal = (point - cp1).normalized;
                        onSurface = cp1 + normal * r;
                        return false;
                    }
                    var mDir = -dir;
                    if (fun.point.IsAbovePlane(in point, in mDir, in cp2))
                    {
                        normal = (point - cp2).normalized;
                        onSurface = cp2 + normal * r;
                        return false;
                    }
                    fun.point.ProjectOnLine(in point, in cp1, in cp2, out var proj);
                    normal = (point - proj).normalized;
                    onSurface = proj + normal * r;
                    return false;
                }
                //Debug.DrawLine(v3.one*-100, point, Color.white, 0, true);
                onSurface = cc.ClosestPoint(point);
                normal = (point - onSurface).normalized;
                return true;
            }
            var bc = collider as BoxCollider;
            if(bc != null)
            {
                var cen = bc.center.AsWorldPoint(bc.transform);
                var fw = bc.transform.forward;
                var bk = -fw;
                var rt = bc.transform.right;
                var lt = -rt;
                var up = bc.transform.up;
                var dn = -up;
                var sca = bc.transform.localScale;

                var fwSf = cen + fw * (bc.size.z * sca.z * 0.5f);
                var bkSf = cen + bk * (bc.size.z * sca.z * 0.5f);
                var rtSf = cen + rt * (bc.size.x * sca.x * 0.5f);
                var ltSf = cen + lt * (bc.size.x * sca.x * 0.5f);
                var upSf = cen + up * (bc.size.y * sca.y * 0.5f);
                var dnSf = cen + dn * (bc.size.y * sca.y * 0.5f);

                onSurface = collider.ClosestPoint(point);
                var isInside = fun.distanceSquared.Between(in onSurface, in point) < 0.0001;

                var d = float.MaxValue;

                normal = (point - onSurface).normalized;
                var proj = onSurface;
                var dFw = fun.distance.FromPointToPlane(in onSurface, in fw, in fwSf);
                if (dFw < d)
                {
                    d = dFw;
                    normal = fw;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in fw, in fwSf, out proj);
                }
                var dBk = fun.distance.FromPointToPlane(in onSurface, in bk, in bkSf);
                if (dBk < d)
                {
                    d = dBk;
                    normal = bk;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in bk, in bkSf, out proj);
                }
                var dRt = fun.distance.FromPointToPlane(in onSurface, in rt, in rtSf);
                if (dRt < d)
                {
                    d = dRt;
                    normal = rt;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in rt, in rtSf, out proj);
                }
                var dLt = fun.distance.FromPointToPlane(in onSurface, in lt, in ltSf);
                if (dLt < d)
                {
                    d = dLt;
                    normal = lt;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in lt, in ltSf, out proj);
                }
                var dUp = fun.distance.FromPointToPlane(in onSurface, in up, in upSf);
                if (dUp < d)
                {
                    d = dUp;
                    normal = up;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in up, in upSf, out proj);
                }
                var dDn = fun.distance.FromPointToPlane(in onSurface, in dn, in dnSf);
                if (dDn < d)
                {
                    normal = dn;
                    if (isInside)
                        fun.point.ProjectOnPlane(in onSurface, in dn, in dnSf, out proj);
                }
                onSurface = proj;
                return isInside;
            }
            onSurface = collider.ClosestPoint(point);
            normal = (point - onSurface).normalized;
            return true;
        }
        public static Vector3 ClosestOnSurfacePoint(this Collider collider, Vector3 point)
        {
            collider.ClosestOnSurfacePointAndNormal(point, out var onSurface, out var normal);
            return onSurface;
        }
        public static bool ClosestOnSurfacePoint(this Collider collider, Vector3 point, out Vector3 onSurface)
        {
            return collider.ClosestOnSurfacePointAndNormal(point, out onSurface, out var normal);
        }
        public static bool IsPointInside(this Collider collider, Vector3 point)
        {
            var closest = collider.ClosestPoint(point);
            return fun.distanceSquared.Between(in closest, in point) < 0.0001;
        }
    } 
}