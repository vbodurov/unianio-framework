using Unianio.Genesis;
using Unianio.IK;

namespace Unianio.Human
{
    public class HumanFaceConfigSource : IHumanExtender
    {
        readonly HumanFaceConfig _config;
        public HumanFaceConfigSource(HumanFaceConfig config) => _config = config;
        public HumanFaceConfig GetConfig() => _config;
        public void Setup(IComplexHuman human) { }
    }
}