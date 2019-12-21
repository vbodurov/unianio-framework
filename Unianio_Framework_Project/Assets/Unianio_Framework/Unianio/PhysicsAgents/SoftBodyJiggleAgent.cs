using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.PhysicsAgents
{
    public interface ISoftBodyJiggleAgent
    {
        IPendulumPhysicsAgent Pendulum { get; }
        (Vector3 position, Quaternion rotation) Compute();
    }
    public class SoftBodyJiggleAgent : ISoftBodyJiggleAgent
    {
        readonly IPendulumPhysicsAgent _ppa;
        readonly ISoftBodyConfig _cfg;
        readonly Vector3 _relStaticTarget, _relIniPos, _relIniFw, _relIniUp;
        internal SoftBodyJiggleAgent(ISoftBodyConfig config)
        {
            _ppa = new PendulumPhysicsAgent(config.Stiffness, config.Mass, config.Damping, config.Gravity);
            _cfg = config;
            _relStaticTarget =
                (_cfg.Bone.position + _cfg.Bone.forward * (float)_cfg.RelTargetAt)
                .AsLocalPoint(_cfg.Bone.parent);
            _relIniPos = _cfg.Bone.position.AsLocalPoint(_cfg.Bone.parent);
            _relIniFw = _cfg.Bone.forward.AsLocalDir(_cfg.Bone.parent);
            _relIniUp = _cfg.Bone.up.AsLocalDir(_cfg.Bone.parent);
        }
        IPendulumPhysicsAgent ISoftBodyJiggleAgent.Pendulum => _ppa;
        (Vector3 position, Quaternion rotation) ISoftBodyJiggleAgent.Compute()
        {
            var staticTarget =
                _relStaticTarget.AsWorldPoint(_cfg.Bone.parent);

            var iniUp = _relIniUp.AsWorldDir(_cfg.Bone.parent);
            var iniDn = -iniUp;
            var iniFw = _relIniFw.AsWorldDir(_cfg.Bone.parent);

            var staticSource =
                _relIniPos.AsWorldPoint(_cfg.Bone.parent);

//            if (Time.realtimeSinceStartup < 3.3) return (position: staticSource, rotation: lookAt(iniFw, iniUp));


            var gravityFactor01 = 0f;
            if (abs(_cfg.Gravity) > 0.0001)
            {
                var iniUpWo = _relIniUp.AsWorldDir(_cfg.Bone.parent);
                gravityFactor01 = dot(in iniUpWo, in v3.dn).FromMin11To01().Clamp01();
            }

            var dynamicTarget =
                _ppa.Compute(in staticTarget, gravityFactor01,
                    (ref Vector3 dynamicPos, ref Vector3 velocity, ref Vector3 force) =>
                    {
                        var candidate = dynamicPos + velocity + force;
                        var curFw = staticSource.DirTo(in candidate, out var dist);
                        var degrees = fun.angle.BetweenVectorsUnSignedInDegrees(in iniFw, in curFw);
                        var maxDegrees = _cfg.MaxDegrees;
                        if (_cfg.RelDownResistance > 0)
                        {
                            var xxx = dot(in curFw, in iniDn);
                            var yyy = pow(cos(xxx.Clamp(-0.9999, 1.0) * PI * 0.5), 4);
                            maxDegrees = _cfg.MaxDegrees * (1f - _cfg.RelDownResistance * yyy);
                        }
                        

                        var x = degrees / maxDegrees;
                        var y = pow(x, 16).Clamp01();
                        if (y < 0.001)
                        {
                            return;
                        }
                        velocity = velocity * (1f - y);
                        if (degrees > maxDegrees)
                        {
                            dynamicPos = staticSource + iniFw.RotateTowards(curFw, maxDegrees) * dist;
                            velocity = Vector3.zero;
                            force = Vector3.zero;
                        }

                        return;
                    });
            var fw = (dynamicTarget - staticSource).ToUnit(out var length);
            var up = fw.GetRealUp(_relIniUp.AsWorldDir(_cfg.Bone.parent));
            var rotation = Quaternion.LookRotation(fw, up);

//            dbg.DrawAxis(_cfg.Bone.hc(), _cfg.Bone.position, up, Color.green);
//            dbg.DrawAxis(_cfg.Bone.hc()*1000, _cfg.Bone.position, v3.up, Color.white);

            var xx = (length / _cfg.RelTargetAt).FromRangeTo01(0, 2);

            var yy = bezier(xx, 0.00, -1.00, 0.50, -1.00, 0.50, 1.00, 1.00, 1.00);

            var pos = staticSource;

            var span = yy < 0 ? _cfg.MaxSqueeze : _cfg.MaxStretch;
            if (abs(span) > 0.0001)
            {
                const int MaxUpDegForStretch = 15;

                var angleFactor = 
                    1f - angle.BetweenVectorsUnSignedInDegrees(in up, in v3.up)
                        .FromRangeTo01(0, MaxUpDegForStretch)
                        .Clamp01();

                pos = pos + iniFw * (yy * (float)span * angleFactor);
            }
            return (
                position: pos,
                rotation: rotation
                );
        }
    }
}