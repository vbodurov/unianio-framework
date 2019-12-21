using Unianio;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.IK;
using static Unianio.Static.fun;

namespace Unianio.Animations.MHHands
{
    public class HumHandPointingAni : BaseHumHandAni, IHandAni
    {
        IHandAni IHandAni.Set(IComplexHuman human, BodySide side, double seconds)
        {
            return Set(human, side, seconds);
        }
        public HumHandPointingAni Set(IComplexHuman human, BodySide side, double seconds = 1)
        {
            Init(human, side);
            


            RotFingerToLocal(FingerName.Thumb, 1, fw_dn_sd(12, -5), v3.up);
            RotFingerToLocal(FingerName.Index, 1, fw_dn_sd(1, 5), v3.up);
            RotFingerToLocal(FingerName.Middle, 1, fw_dn(90), v3.fw);
            RotFingerToLocal(FingerName.Ring, 1, fw_dn(90), v3.fw);
            RotFingerToLocal(FingerName.Pinky, 1, fw_dn_sd(90, -10), v3.fw);

            RotFingerToLocal(FingerName.Thumb, 2, v3.forward, v3.fw);
            RotFingerToLocal(FingerName.Index, 2, fw_dn(1), v3.up);
            RotFingerToLocal(FingerName.Middle, 2, fw_dn(87), v3.fw);
            RotFingerToLocal(FingerName.Ring, 2, fw_dn(88), v3.fw);
            RotFingerToLocal(FingerName.Pinky, 2, fw_dn(89), v3.fw);

            RotFingerToLocal(FingerName.Thumb, 3, v3.forward, v3.fw);
            RotFingerToLocal(FingerName.Index, 3, fw_dn(1), v3.up);
            RotFingerToLocal(FingerName.Middle, 3, fw_dn(80), v3.fw);
            RotFingerToLocal(FingerName.Ring, 3, fw_dn(80), v3.fw);
            RotFingerToLocal(FingerName.Pinky, 3, fw_dn(80), v3.fw);

            StartFingerRotation(seconds);

            return this.AsUniqueNamed((unique.Hand + side) + human.ID);
        }
    }
}