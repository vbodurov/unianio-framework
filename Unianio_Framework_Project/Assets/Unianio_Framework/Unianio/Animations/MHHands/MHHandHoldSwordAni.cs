using Unianio.Enums;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.MakeHuman;

namespace Unianio.Animations.MHHands
{
    public class MHHandHoldSwordAni : BaseMHHandAni, IHandAni
    {
        public IHandAni Set(IComplexHuman human, BodySide side, double seconds = 1)
        {
            Init(human, side);

            RotFingerToLocal(FingerName.Thumb, 1, v3.fw, v3.up);
            RotFingerToLocal(FingerName.Index, 1, v3.fw.RotUp(50), v3.up);
            RotFingerToLocal(FingerName.Middle, 1, v3.fw.RotDn(50), v3.up);
            RotFingerToLocal(FingerName.Ring, 1, v3.fw.RotDn(60), v3.up);
            RotFingerToLocal(FingerName.Pinky, 1, v3.fw.RotDn(60), v3.up);

            RotFingerToLocal(FingerName.Thumb, 2, v3.fw, v3.up);
            RotFingerToLocal(FingerName.Index, 2, v3.fw.RotUp(50), v3.up);
            RotFingerToLocal(FingerName.Middle, 2, v3.fw.RotDn(50), v3.up);
            RotFingerToLocal(FingerName.Ring, 2, v3.fw.RotDn(55), v3.up);
            RotFingerToLocal(FingerName.Pinky, 2, v3.fw.RotDn(50), v3.up);

            RotFingerToLocal(FingerName.Thumb, 3, v3.fw, v3.up);
            RotFingerToLocal(FingerName.Index, 3, v3.fw.RotUp(20), v3.up);
            RotFingerToLocal(FingerName.Middle, 3, v3.fw.RotDn(80), v3.up);
            RotFingerToLocal(FingerName.Ring, 3, v3.fw.RotDn(80), v3.up);
            RotFingerToLocal(FingerName.Pinky, 3, v3.fw.RotDn(60), v3.up);

            StartFingerRotation(seconds);

            return this.AsUniqueNamed((unique.Hand + side) + human.ID);
        }
    }
}