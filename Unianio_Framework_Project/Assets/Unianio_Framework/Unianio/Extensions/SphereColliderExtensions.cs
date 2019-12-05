using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class SphereColliderExtensions
    {
        public static SphereCollider Set(this SphereCollider sc, double radius, double x, double y, double z)
        {
            sc.radius = radius.Float();
            sc.center = V3(x, y, z);
            return sc;
        }
    }
}