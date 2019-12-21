using UnityEngine;

namespace Unianio.PhysicsAgents
{
    public interface ISoftBodyConfig
    {
        double Stiffness { get; }
        double Mass { get; }
        double Damping { get; }
        double Gravity { get; }
        double MaxDegrees { get; }
        double MaxStretch { get; }
        double MaxSqueeze { get; }
        double RelTargetAt { get; }
        double RelDownResistance { get; }
        Transform Bone { get; }
    }
}