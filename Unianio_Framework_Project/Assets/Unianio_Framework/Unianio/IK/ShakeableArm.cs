using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public interface IShakeableArm
    {
        IShakeableArm Set(IHumArmChain arm);
        Transform Shakeable { get; }
    }
    public sealed class ShakeableArm : IShakeableArm
    {
        IHumArmChain _arm;
        Transform _shakeable;
        IShakeableArm IShakeableArm.Set(IHumArmChain arm)
        {
            _arm = arm;
            Initialize();
            return this;
        }
        void Initialize()
        {
            var go = _arm.Forearm.gameObject;
            var rb1 = go.AddComponent<Rigidbody>();
            rb1.mass = 4;
            rb1.drag = 0f;
            rb1.angularDrag = 0.05f;
            rb1.useGravity = true;
            rb1.isKinematic = false;
            rb1.interpolation = RigidbodyInterpolation.None;
            rb1.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb1.constraints = RigidbodyConstraints.FreezeAll;




//            go = fun.meshes.CreatePointyCone(
//                    new DtCone {height = 0.05, bottomRadius = 0.1, topRadius = 0.001, relNoseLen = 2});
            go = new GameObject($"Human_{_arm.Model.name}_ShakeableArm_"+ _arm.Side);
            _shakeable = go.transform;
            _shakeable.position = _arm.Hand.position;
            _shakeable.rotation = _arm.Hand.rotation;
            var rb2 = go.AddComponent<Rigidbody>();
            rb2.mass = 3.375f;
            rb2.drag = 0f;
            rb2.angularDrag = 0.05f;
            rb2.useGravity = true;
            rb2.isKinematic = false;
            rb2.interpolation = RigidbodyInterpolation.None;
            rb2.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb2.constraints = RigidbodyConstraints.None;

            var cc = go.AddComponent<CapsuleCollider>();
            cc.isTrigger = false;
            cc.center = new Vector3(0f, 0f, 0.05f);
            cc.radius = 0.05f;
            cc.height = 0.15f;
            cc.direction = 2;

            var cj = go.AddComponent<CharacterJoint>();
            cj.connectedBody = rb1;
            cj.anchor = new Vector3(0f, 0f, 0f);
            cj.axis = new Vector3(0f, 1f, 0f);
            cj.autoConfigureConnectedAnchor = true;
            cj.connectedAnchor = new Vector3(-0.005144887f, 0.2591363f, 0.01210071f);
            cj.swingAxis = new Vector3(-1f, 0f, 0f);
            cj.twistLimitSpring = new SoftJointLimitSpring { damper = 0f, spring = 0f };
            cj.lowTwistLimit = new SoftJointLimit { limit = -50, bounciness = 0f, contactDistance = 0f };
            cj.highTwistLimit = new SoftJointLimit { limit = 80, bounciness = 0f, contactDistance = 0f };
            cj.swingLimitSpring = new SoftJointLimitSpring { damper = 0f, spring = 0f };
            cj.swing1Limit = new SoftJointLimit { limit = 25f, bounciness = 0f, contactDistance = 0f };
            cj.swing2Limit = new SoftJointLimit { limit = 10f, bounciness = 0f, contactDistance = 0f };
            cj.enableProjection = false;
            cj.projectionDistance = 0.1f;
            cj.projectionAngle = 180f;
            cj.breakForce = float.PositiveInfinity;
            cj.breakTorque = float.PositiveInfinity;
            cj.enableCollision = false;
            cj.enablePreprocessing = false;
            cj.massScale = 1f;
            cj.connectedMassScale = 1f;

        }
        Transform IShakeableArm.Shakeable => _shakeable;
    }
}