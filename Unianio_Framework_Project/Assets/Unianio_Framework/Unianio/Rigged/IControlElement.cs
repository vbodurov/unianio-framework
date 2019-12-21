using UnityEngine;

namespace Unianio.Rigged
{
    public interface IControlElement
    {
        HumanoidPart Part { get; }
        Transform Handle { get; }
    }
}