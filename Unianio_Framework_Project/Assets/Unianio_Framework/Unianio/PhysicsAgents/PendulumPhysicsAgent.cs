using System.Collections.Generic;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.PhysicsAgents
{
    public delegate void ApplyConstrain(ref Vector3 dynamicPos, ref Vector3 velocity, ref Vector3 force);
    public interface IPendulumPhysicsAgent
    {
        Vector3 Compute(in Vector3 currRigidPosition, double gravity01 = 0, ApplyConstrain constrain = null);
        void CancelInertia();
        int DebugId { get; set; }
    }
    public class PendulumPhysicsAgent : IPendulumPhysicsAgent
    {
        int _frame, _debugId;
        Vector3 _force, _acc, _vel, _dynamicPos;
        readonly float _stiffness, _mass, _damping, _gravity;
        // stiffness = 9, mass = 81, damping = 68, gravity = 68
        public PendulumPhysicsAgent(
            double stiffness = 0.1f,
            double mass = 0.9f,
            double damping = 0.72f,
            double gravity = 0.72f)
        {
            _stiffness = (float)stiffness;
            _mass = (float)mass;
            _damping = (float)damping;
            _gravity = (float)gravity;
        }

        int IPendulumPhysicsAgent.DebugId
        {
            get => _debugId;
            set => _debugId = value;
        }
        Vector3 IPendulumPhysicsAgent.Compute(in Vector3 targetPos, double gravity01, ApplyConstrain constrain)
        {
            if (++_frame == 1)
            {
                return _dynamicPos = targetPos;
            }

            gravity01 = clamp01(gravity01);
            var fpsFactor = (float)(fun.smoothDeltaTime / 0.011111111);
            var stiffness = clamp(_stiffness, 0f, 1f);
            var mass = clamp(_mass, 0.00001, 1f);
            var damping = clamp((1 - _damping) * fpsFactor, 0f, 1f);
            var gravity = clamp(_gravity, 0f, 1f);
            //Debug.Log(stiffness+"|"+mass+"|"+damping+"|"+gravity);
            _force.x = (targetPos.x - _dynamicPos.x) * stiffness;
            _acc.x = _force.x / mass;
            _vel.x += _acc.x * damping;

            _force.y = (targetPos.y - _dynamicPos.y) * stiffness;
            if(gravity01 > 0.0) _force.y -= ((gravity * (float)gravity01) / 10) * fpsFactor; // Add some gravity
            _acc.y = _force.y / mass;
            _vel.y += _acc.y * damping;

            _force.z = (targetPos.z - _dynamicPos.z) * stiffness;
            _acc.z = _force.z / mass;
            _vel.z += _acc.z * damping;

            _dynamicPos = _dynamicPos + _vel + _force;

            constrain?.Invoke(ref _dynamicPos, ref _vel, ref _force);

            

            return _dynamicPos;
        }
        void IPendulumPhysicsAgent.CancelInertia()
        {
            _force = _acc = _vel = v3.zero;
        }
    }
}