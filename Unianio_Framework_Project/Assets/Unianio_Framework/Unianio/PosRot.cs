using UnityEngine;

namespace Unianio
{
    public struct PosRot
    {
        public PosRot(Transform t) : this (t.position, t.rotation) { }
        public PosRot(Vector3 positionValue, Quaternion rotationValue)
        {
            position = positionValue;
            rotation = rotationValue;
        }
        public readonly Vector3 position;
        public readonly Quaternion rotation;

        public PosRot AddPos(in Vector3 pos)
        {
            return new PosRot(position + pos, rotation);
        }
    }
}