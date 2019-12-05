using UnityEngine;

namespace Unianio
{
    public static class v3
    {
        /// <summary>
        /// 0, 0, 0
        /// </summary>
         public static readonly Vector3 zero = new Vector3(0, 0, 0);
        /// <summary>
        /// 1, 1, 1
        /// </summary>
         public static readonly Vector3 one = new Vector3(1, 1, 1);
        /// <summary>
        /// 1, 0, 0
        /// </summary>
         public static readonly Vector3 unitX = new Vector3(1, 0, 0);
        /// <summary>
        /// 0, 1, 0
        /// </summary>
         public static readonly Vector3 unitY = new Vector3(0, 1, 0);
        /// <summary>
        /// 0, 0, 1
        /// </summary>
         public static readonly Vector3 unitZ = new Vector3(0, 0, 1);
        /// <summary>
        /// -1, 0, 0
        /// </summary>
         public static readonly Vector3 unitMinX = new Vector3(-1, 0, 0);
        /// <summary>
        /// 0, -1, 0
        /// </summary>
         public static readonly Vector3 unitMinY = new Vector3(0, -1, 0);
        /// <summary>
        /// 0, 0, -1
        /// </summary>
         public static readonly Vector3 unitMinZ = new Vector3(0, 0, -1);
        /// <summary>
        /// 0, 0, 1
        /// </summary>
         public static readonly Vector3 forward = Vector3.forward;
        /// <summary>
        /// 0, 0, 1
        /// </summary>
         public static readonly Vector3 fw = Vector3.forward;
        /// <summary>
        /// 0, 0, -1
        /// </summary>
         public static readonly Vector3 back = Vector3.back;
        /// <summary>
        /// 0, 0, -1
        /// </summary>
         public static readonly Vector3 bk = Vector3.back;
        /// <summary>
        /// -1, 0, 0
        /// </summary>
         public static readonly Vector3 left = Vector3.left;
        /// <summary>
        /// -1, 0, 0
        /// </summary>
         public static readonly Vector3 lt = Vector3.left;
        /// <summary>
        /// 1, 0, 0
        /// </summary>
         public static readonly Vector3 right = Vector3.right;
        /// <summary>
        /// 1, 0, 0
        /// </summary>
         public static readonly Vector3 rt = Vector3.right;
        /// <summary>
        /// 0, 1, 0
        /// </summary>
         public static readonly Vector3 up = Vector3.up;
        /// <summary>
        /// 0, -1, 0
        /// </summary>
         public static readonly Vector3 down = Vector3.down;
        /// <summary>
        /// 0, -1, 0
        /// </summary>
         public static readonly Vector3 dn = Vector3.down;
        /// <summary>
        /// normalized(+1, 0, +1)
        /// </summary>
         public static readonly Vector3 corner_fw_rt = new Vector3(+1, 0, +1).normalized;
        /// <summary>
        /// normalized(-1, 0, +1)
        /// </summary>
         public static readonly Vector3 corner_fw_lt = new Vector3(-1, 0, +1).normalized;
        /// <summary>
        /// normalized(+1, 0, -1)
        /// </summary>
         public static readonly Vector3 corner_bk_rt = new Vector3(+1, 0, -1).normalized;
        /// <summary>
        /// normalized(+1, 0, -1)
        /// </summary>
         public static readonly Vector3 corner_bk_lt = new Vector3(-1, 0, -1).normalized;
        /// <summary>
        /// float.NaN, float.NaN, float.NaN
        /// </summary>
         public static readonly Vector3 nan = new Vector3(float.NaN, float.NaN, float.NaN);
        /// <summary>
        /// 99999, 99999, 99999
        /// </summary>
         public static readonly Vector3 v99999 = new Vector3(99999, 99999, 99999);

    }
}
