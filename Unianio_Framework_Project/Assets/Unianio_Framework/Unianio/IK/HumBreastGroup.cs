using Unianio.Animations;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.PhysicsAgents;
using Unianio.Rigged;
using Unianio.Services;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public class HumBreastGroup : AnimationManager, ITimeProviderHolder
    {
        readonly string _persona;
        readonly BodySide _side;
        readonly IComplexHuman _human;
        readonly Transform _pectoral;
        readonly HumBoneHandler _nipple;
        readonly ISoftBodyJiggleAgent _jiggle;
        readonly Vector3 _iniWorldUpAsLocal, 
            _iniBreastLocFw, _iniBreastLocUp, 
            _iniNippleLocPos, _iniBreastLocPos;
        readonly float _length;
        IPausableTimeProvider _pausableTime;

        public HumBreastGroup(
            string persona,
            BodySide side,
            IComplexHuman human,
            Transform pectoral,
            HumBoneHandler nipple,
            BreastConfigSource configSource)
        {
            _persona = persona;
            _side = side;
            _human = human;
            _pectoral = pectoral;
            _nipple = nipple;
            Part = side == BodySide.LT ? HumanoidPart.BreastL : HumanoidPart.BreastR;
            _iniBreastLocFw = _pectoral.forward.AsLocalDir(_pectoral.parent);
            _iniBreastLocUp = _pectoral.up.AsLocalDir(_pectoral.parent);
            _iniWorldUpAsLocal = v3.up.AsLocalDir(_pectoral.parent);
            if(_nipple != null) _iniNippleLocPos = _nipple.position.AsLocalPoint(_pectoral.parent);
            _iniBreastLocPos = _pectoral.localPosition;

            var softBodyConfig = configSource?.GetConfig() ??
                                 new SoftBodyConfig
                                    {
                                        MaxStretch = 0.10,
                                        MaxSqueeze = 0.05,
                                        MaxDegrees = 30,
                                        RelTargetAt = 0.26,
                                        Stiffness = 0.01,
                                        Mass = 0.20,
                                        Damping = 0.10,
                                        Gravity = 0.05,
                                        RelDownResistance = 0.70
                                    };
            softBodyConfig.Bone = _pectoral;

//            var softBodyConfig = new SoftBodyConfig
//            {
//                Bone = _pectoral,
//                MaxStretch = 0.10,
//                MaxSqueeze = 0.05,
//                MaxDegrees = 25,
//                RelTargetAt = 0.26,
//                Stiffness = 0.05,
//                Mass = 0.50,
//                Damping = 0.50,
//                Gravity = 0.05,
//                RelDownResistance = 0.80
//            };

            fire(new RegisterSoftBodyConfig(human, side, Part, softBodyConfig));

            _jiggle = new SoftBodyJiggleAgent(softBodyConfig);

            subscribe<CancelInertia>(human, e =>
            {
                _jiggle.Pendulum.CancelInertia();
            }, this);
        }
        public HumanoidPart Part { get; }
        public HumBoneHandler Nipple => _nipple;
        public Transform Pectoral => _pectoral;
        public void SetTimeProvider(IPausableTimeProvider pausableTime)
        {
            _pausableTime = pausableTime;
        }
        public override void Update()
        {
            if(_pausableTime != null && _pausableTime.IsPaused) return;

            base.Update();

            var r = _jiggle.Compute();

            _pectoral.rotation = r.rotation;
            _pectoral.position = r.position;
        }
        
    }
}