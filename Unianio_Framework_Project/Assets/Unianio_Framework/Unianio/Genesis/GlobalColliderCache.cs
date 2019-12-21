using System.Collections.Generic;
using Unianio.IK;
using UnityEngine;

namespace Unianio.Genesis
{
    internal interface IGlobalColliderCache
    {
        void RegisterCollider(Collider collider, IComplexHuman human);
        IComplexHuman GetHumanByCollider(Collider collider);
    }
    internal sealed class GlobalColliderCache : IGlobalColliderCache
    {
        private readonly IDictionary<int, IComplexHuman> _humanByInstanceId = new Dictionary<int, IComplexHuman>();
        void IGlobalColliderCache.RegisterCollider(Collider collider, IComplexHuman human)
        {
            _humanByInstanceId[collider.GetInstanceID()] = human;
        }
        IComplexHuman IGlobalColliderCache.GetHumanByCollider(Collider collider)
        {
            return _humanByInstanceId.TryGetValue(collider.GetInstanceID(), out var human) ? human : null;
        }
    }
}