using UnityEngine;

namespace Unianio.Human
{
    public interface IHumanBoneWrapper
    {
        void Wrap(Transform model, string personaId, string customTag);
    }
}