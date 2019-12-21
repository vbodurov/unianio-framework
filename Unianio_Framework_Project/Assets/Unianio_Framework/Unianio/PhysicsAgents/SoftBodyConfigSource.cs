using Unianio.Genesis;
using Unianio.IK;

namespace Unianio.PhysicsAgents
{
    public class SoftBodyConfigSource : IHumanExtender
    {
        readonly SoftBodyConfig _config;
        public SoftBodyConfigSource(SoftBodyConfig config) => _config = config;
        public SoftBodyConfig GetConfig() => _config;
        public void Setup(IComplexHuman human) { }
    }
    public class BreastConfigSource : SoftBodyConfigSource
    {
        public BreastConfigSource(SoftBodyConfig config) : base(config) { }
    }
}