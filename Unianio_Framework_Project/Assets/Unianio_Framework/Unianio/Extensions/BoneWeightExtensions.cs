using UnityEngine;

namespace Unianio.Extensions
{
    public static class BoneWeightExtensions
    {
        public static float WeightOfIndex(this BoneWeight bw, int index)
        {
            if (bw.boneIndex0 == index) return bw.weight0;
            if (bw.boneIndex1 == index) return bw.weight1;
            if (bw.boneIndex2 == index) return bw.weight2;
            if (bw.boneIndex3 == index) return bw.weight3;
            return 0;
        }
    }
}