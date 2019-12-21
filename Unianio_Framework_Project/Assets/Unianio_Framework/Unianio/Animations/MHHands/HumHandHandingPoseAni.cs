using Unianio;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.IK;
using static Unianio.Static.fun;

namespace Unianio.Animations.MHHands
{
    public class HumHandHandingPoseAni : BaseHumHandAni, IHandAni
    {
        IHandAni IHandAni.Set(IComplexHuman human, BodySide side, double seconds)
        {
            return Set(human, side, seconds);
        }
        public HumHandHandingPoseAni Set(IComplexHuman human, BodySide side, double seconds = 1)
        {
            Init(human, side);

            RotFingerToLocal(FingerName.Thumb, 1, fw_dn_sd(20, 10), up_sd(-10));
            RotFingerToLocal(FingerName.Index, 1, fw_dn_sd(10, 5), v3.up);
            RotFingerToLocal(FingerName.Middle, 1, fw_dn(20), v3.up);
            RotFingerToLocal(FingerName.Ring, 1, fw_dn_sd(40, -5), v3.up);
            RotFingerToLocal(FingerName.Pinky, 1, fw_dn_sd(50, -10), v3.up);

            RotFingerToLocal(FingerName.Thumb, 2, v3.forward, v3.up);
            RotFingerToLocal(FingerName.Index, 2, fw_dn(20), v3.up);
            RotFingerToLocal(FingerName.Middle, 2, fw_dn(40), v3.up);
            RotFingerToLocal(FingerName.Ring, 2, fw_dn(90), v3.forward);
            RotFingerToLocal(FingerName.Pinky, 2, fw_dn(110), v3.forward);

            RotFingerToLocal(FingerName.Thumb, 3, v3.forward, v3.up);
            RotFingerToLocal(FingerName.Index, 3, fw_dn(20), v3.up);
            RotFingerToLocal(FingerName.Middle, 3, fw_dn(30), v3.up);
            RotFingerToLocal(FingerName.Ring, 3, fw_dn(50), v3.up);
            RotFingerToLocal(FingerName.Pinky, 3, fw_dn(60), v3.up);

            StartFingerRotation(seconds);

            return this.AsUniqueNamed((unique.Hand + side) + human.ID);
        }
    }
}
