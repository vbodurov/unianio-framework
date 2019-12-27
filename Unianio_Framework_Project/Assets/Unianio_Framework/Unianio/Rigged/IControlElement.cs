using UnityEngine;

namespace Unianio.Rigged
{
    public interface IControlElement
    {
        BodyPart Part { get; }
        Transform Handle { get; }
    }
}