using Unianio.Enums;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.MakeHuman;
using static Unianio.Static.fun;

namespace Unianio.Animations.MHHands
{
    public class MHHandFistAni : BaseMHHandAni, IHandAni
    {
        public IHandAni Set(IComplexHuman human, BodySide side, double seconds)
        {
            Init(human, side);

            RotFingerToLocal(FingerName.Thumb, 1, fw_dn_sd(5, 12), v3.up, funFastToSlow2);
            RotFingerToLocal(FingerName.Index, 1, fw_dn(70), v3.up);
            RotFingerToLocal(FingerName.Middle, 1, fw_dn(70), v3.up);
            RotFingerToLocal(FingerName.Ring, 1, fw_dn(70), v3.up);
            RotFingerToLocal(FingerName.Pinky, 1, fw_dn(70), v3.up);

            RotFingerToLocal(FingerName.Thumb, 2, fw_dn(20), v3.fw);
            RotFingerToLocal(FingerName.Index, 2, fw_dn(55), v3.up);
            RotFingerToLocal(FingerName.Middle, 2, fw_dn(60), v3.up);
            RotFingerToLocal(FingerName.Ring, 2, fw_dn(55), v3.up);
            RotFingerToLocal(FingerName.Pinky, 2, fw_dn(55), v3.up);

            RotFingerToLocal(FingerName.Thumb, 3, fw_dn(60), v3.up);
            RotFingerToLocal(FingerName.Index, 3, fw_dn(60), v3.up);
            RotFingerToLocal(FingerName.Middle, 3, fw_dn(85), v3.up);
            RotFingerToLocal(FingerName.Ring, 3, fw_dn(80), v3.up);
            RotFingerToLocal(FingerName.Pinky, 3, fw_dn(60), v3.up);

            StartFingerRotation(seconds);

            return this.AsUniqueNamed((unique.Hand + side) + human.ID);
        }
    }
}