using UnityEngine;

namespace Unianio.IK
{
    public interface IRotatableAroundPivot
    {
        void HorzPivotStarts(in Vector3 pivotPoint, out Vector3 vecPivotIn);
        void HorzPivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn);
        void HorzPivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn, in Quaternion iniPivotRot, in Quaternion currPivotRot);
        void PivotStarts(in Vector3 pivotPoint, out Vector3 vecPivotIn);
        void PivotEnds(in Vector3 pivotPoint, in Vector3 vecPivotIn);
    }
}