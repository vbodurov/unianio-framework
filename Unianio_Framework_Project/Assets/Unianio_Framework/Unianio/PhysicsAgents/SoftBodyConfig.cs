using UnityEngine;

namespace Unianio.PhysicsAgents
{
    public class SoftBodyConfig : ISoftBodyConfig
    {
        public double Stiffness { get; set; } = 0.1;
        public double Mass { get; set; } = 0.9;
        public double Damping { get; set; } = 0.72;
        public double Gravity { get; set; } = 0.72;
        public double MaxDegrees { get; set; } = 50;
        public double MaxStretch { get; set; } = 2;
        public double MaxSqueeze { get; set; } = 1;
        public double RelTargetAt { get; set; } = 0.07;
        public double RelDownResistance { get; set; } = 0.0;
        public Transform Bone { get; set; }

        public SoftBodyConfig Clone()
        {
            return new SoftBodyConfig
            {
                Stiffness = Stiffness,
                Mass = Mass,
                Damping = Damping,
                Gravity = Gravity,
                MaxDegrees = MaxDegrees,
                MaxStretch = MaxStretch,
                MaxSqueeze = MaxSqueeze,
                RelTargetAt = RelTargetAt,
                Bone = Bone
            };
        }
    }
}