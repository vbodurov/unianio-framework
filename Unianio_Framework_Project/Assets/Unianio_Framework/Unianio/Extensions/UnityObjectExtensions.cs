namespace Unianio.Extensions
{
    public static class UnityObjectExtensions
    {
        public static bool Destroy(this UnityEngine.Object obj)
        {
            if (obj == null || !obj) return false;
            UnityEngine.Object.Destroy(obj);
            return true;
        }
        public static bool DestroyImmediate(this UnityEngine.Object obj)
        {
            if (obj == null || !obj) return false;
            UnityEngine.Object.DestroyImmediate(obj);
            return true;
        }
    }
}