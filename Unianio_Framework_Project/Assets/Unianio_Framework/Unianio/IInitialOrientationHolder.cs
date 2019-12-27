using Unianio.Moves;
using Unianio.Rigged.IK;
using UnityEngine;

namespace Unianio
{
    public interface IInitialOrientationHolder : IControlHolder
    {
        Vector3 IniLocalPos { get; }
        Vector3 IniModelPos { get; }
        Quaternion IniLocalRot { get; }
        Quaternion IniModelRot { get; }
        Vector3 IniLocalSca { get; }
        Vector3 IniLocalFw { get; }
        Vector3 IniLocalUp { get; }
        Vector3 IniModelFw { get; }
        Vector3 IniModelUp { get; }
        IExecutorOfProgress ToInitialLocalPosition();
        IExecutorOfProgress ToInitialLocalRotation();
        IExecutorOfProgress ToInitialLocalScale();
        IExecutorOfProgress ToInitialLocal();
    }
}