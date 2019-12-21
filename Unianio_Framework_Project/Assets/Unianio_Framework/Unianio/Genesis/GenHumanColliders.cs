using System.Collections.Generic;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis
{
    public interface IGenHumanColliders
    {
        IGenHumanColliders Initialize(IComplexHuman complexHuman);
        CapsuleCollider AddCapsule(Transform bone, double x, double y, double z, double radius, double height, int direction);
        SphereCollider AddSphere(Transform bone, double x, double y, double z, double radius);
        GenColliderData ByCollider(Collider c);
        HashSet<GenColliderData> ByName(string name);
    }
    public enum GenColliderType
    {
        Sphere,
        Capsule    
    }
    public class GenColliderData
    {
        internal GenColliderType Type;
        internal CapsuleCollider Capsule;
        internal SphereCollider Sphere;
        internal Transform Trans;
    }
    public class GenHumanColliders : IGenHumanColliders
    {
        readonly IGlobalColliderCache _gcc;
        IComplexHuman _human;
        static readonly Quaternion[] RotationByIndex = new[] { Quaternion.AngleAxis(90, v3.unitZ), Quaternion.AngleAxis(90, v3.unitY), Quaternion.AngleAxis(90, v3.unitX) };
        readonly Dictionary<int, GenColliderData> _colliderByInstanceId = new Dictionary<int, GenColliderData>();
        readonly Dictionary<string, HashSet<GenColliderData>> _collidersByBoneName = new Dictionary<string, HashSet<GenColliderData>>();

        internal GenHumanColliders(IGlobalColliderCache gcc)
        {
            _gcc = gcc;
        }
        IGenHumanColliders IGenHumanColliders.Initialize(IComplexHuman human)
        {
            _human = human;
            return this;
        }
        GenColliderData IGenHumanColliders.ByCollider(Collider c)
        {
            GenColliderData val;
            return _colliderByInstanceId.TryGetValue(c.GetInstanceID(), out val) ? val : null;
        }
        HashSet<GenColliderData> IGenHumanColliders.ByName(string name)
        {
            HashSet<GenColliderData> val;
            return _collidersByBoneName.TryGetValue(name, out val) ? val : new HashSet<GenColliderData>();
        }

        CapsuleCollider IGenHumanColliders.AddCapsule(Transform bone, double x, double y, double z, double radius, double height, int direction)
        {
//            bone.gameObject.layer = layers.BitNumber_Characters;
            var cc = bone.gameObject.AddComponent<CapsuleCollider>();
            cc.center = V3(x, y, z);
            cc.radius = (float)radius;
            cc.height = (float)height;
            cc.direction = direction;
/*

            if (dbg.DebugDrawColliders)
            {
                var visual =
                     fun.meshes.CreateCapsule(new DtCapsule { height = height - radius * 2, radius = radius })
                     .SetStandardShaderTransparentColor(dbg.Colors[cc.GetInstanceID().Abs() % dbg.Colors.Length].WithA(0.5))
                     .transform;
                visual.position = bone.position + cc.center.AsWorldVec(bone);
                visual.SetParent(bone);
                visual.localRotation = Quaternion.LookRotation(v3.forward, v3.up) * RotationByIndex[cc.direction];
                visual.localScale = V3(1);
            }
*/

            var cd = new GenColliderData { Type = GenColliderType.Capsule, Capsule = cc, Trans = bone };
            _colliderByInstanceId[cc.GetInstanceID()] = cd;

            if (!_collidersByBoneName.ContainsKey(bone.name))
            {
                _collidersByBoneName[bone.name] = new HashSet<GenColliderData>();
            }
            _collidersByBoneName[bone.name].Add(cd);
            _gcc.RegisterCollider(cc, _human);
            return cc;
        }
        SphereCollider IGenHumanColliders.AddSphere(Transform bone, double x, double y, double z, double radius)
        {
//            bone.gameObject.layer = layers.BitNumber_Characters;
            var sc = bone.gameObject.AddComponent<SphereCollider>();
            sc.center = V3(x, y, z);
            sc.radius = (float)radius;
/*

            if (dbg.DebugDrawColliders)
            {
                var visual =
                    fun.meshes.CreateSphere(new DtSphere { radius = radius })
                    .SetStandardShaderTransparentColor(dbg.Colors[sc.GetInstanceID().Abs() % dbg.Colors.Length].WithA(0.2))
                    .transform;
                visual.position = bone.position + sc.center.AsWorldVec(bone);
                visual.SetParent(bone);
                visual.localScale = V3(1);
            }*/

            var cd = new GenColliderData { Type = GenColliderType.Sphere, Sphere = sc, Trans = bone};
            _colliderByInstanceId[sc.GetInstanceID()] = cd;
            if (!_collidersByBoneName.ContainsKey(bone.name))
            {
                _collidersByBoneName[bone.name] = new HashSet<GenColliderData>();
            }
            _collidersByBoneName[bone.name].Add(cd);
            _gcc.RegisterCollider(sc, _human);
            return sc;
        }
    }
}