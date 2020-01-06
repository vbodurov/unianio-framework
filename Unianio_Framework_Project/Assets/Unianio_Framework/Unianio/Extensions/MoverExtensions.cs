using Unianio.IK;
using Unianio.Moves;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class MoverExtensions
    {
        public static Mover<IHumArmChain> NaturalHandRotation(this Mover<IHumArmChain> mover)
        {
            var chain = mover.Object;
            var iniRot = chain.Control.rotation;
            return mover.World.SetRot(new DynamicRotationMove(x =>
            {
                chain.CalculateArmBend(out var midPos, out var length);
                var handlePos = chain.Control.position;
                var dirShoulderToHand = chain.Shoulder.DirTo(in handlePos);
                var worldUp = chain.SideDir.AsWorldDir(chain.ArmRoot);
                vector.GetNormal(in dirShoulderToHand, in worldUp, out var backDir);
                if (chain.Side.IsRight()) backDir = -backDir;
                var elbowPos = midPos + backDir * length;
                var target = lookAt(elbowPos.DirTo(in handlePos), in worldUp);
                return slerp(in iniRot, in target, 1 - pow(1 - x, 8));
            }));  
        }
    }
}