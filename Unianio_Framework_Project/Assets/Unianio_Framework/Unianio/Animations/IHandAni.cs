using Unianio.Enums;
using Unianio.Genesis;
using Unianio.IK;

namespace Unianio.Animations
{
    public interface IHandAni
    {
        IHandAni Set(IComplexHuman human, BodySide side, double seconds = 1);
    }
}