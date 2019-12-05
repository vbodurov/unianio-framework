using UnityEngine;

namespace Unianio.Static
{
    public static partial class fun
    {
        public static class cross
        {
            public static Vector3 Product(in Vector3 leftVector, in Vector3 rightVector)
            {
                return new Vector3(leftVector.y * rightVector.z - leftVector.z * rightVector.y, leftVector.z * rightVector.x - leftVector.x * rightVector.z, leftVector.x * rightVector.y - leftVector.y * rightVector.x);
            }
            public static Vector3 Product(Vector3 leftVector, Vector3 rightVector)
            {
                return new Vector3(leftVector.y * rightVector.z - leftVector.z * rightVector.y, leftVector.z * rightVector.x - leftVector.x * rightVector.z, leftVector.x * rightVector.y - leftVector.y * rightVector.x);
            }
            public static void Product(in Vector3 leftVector, in Vector3 rightVector, out Vector3 result)
            {
                result = new Vector3(leftVector.y * rightVector.z - leftVector.z * rightVector.y, leftVector.z * rightVector.x - leftVector.x * rightVector.z, leftVector.x * rightVector.y - leftVector.y * rightVector.x);
            }
        }
    }
}