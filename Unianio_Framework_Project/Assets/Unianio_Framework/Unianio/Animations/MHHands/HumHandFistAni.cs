using Unianio;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Animations.MHHands
{
    public class HumHandFistAni : BaseHumHandAni, IHandAni
    {
        IHandAni IHandAni.Set(IComplexHuman human, BodySide side, double seconds)
        {
            return Set(human, side, seconds);
        }
        public HumHandFistAni Set(IComplexHuman human, BodySide side, double seconds = 1)
        {
            Init(human, side);

            if (human.IsGenesis8())
            {
                RotFingerToLocal(FingerName.Thumb, 1, fw_dn_sd(10, -12), v3.up, funFastToSlow2);
                RotFingerToLocal(FingerName.Index, 1, fw_dn_sd(100, 5), v3.fw);// fw and not up because the new up is what it was fw
                RotFingerToLocal(FingerName.Middle, 1, fw_dn(100), v3.fw);
                RotFingerToLocal(FingerName.Ring, 1, fw_dn(100), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 1, fw_dn_sd(100, -10), v3.fw);

                RotFingerToLocal(FingerName.Thumb, 2, fw_dn(30), v3.fw);
                RotFingerToLocal(FingerName.Index, 2, fw_dn(86), v3.fw);
                RotFingerToLocal(FingerName.Middle, 2, fw_dn(87), v3.fw);
                RotFingerToLocal(FingerName.Ring, 2, fw_dn(88), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 2, fw_dn(89), v3.fw);

                RotFingerToLocal(FingerName.Thumb, 3, fw_dn(75), fw_sd(-30));
                RotFingerToLocal(FingerName.Index, 3, fw_dn(120), v3.fw);
                RotFingerToLocal(FingerName.Middle, 3, fw_dn(120), v3.fw);
                RotFingerToLocal(FingerName.Ring, 3, fw_dn(120), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 3, fw_dn(120), v3.fw);
            }
            else
            {
                RotFingerToLocal(FingerName.Thumb, 1, fw_dn_sd(10, 0), v3.up, funFastToSlow2);
                RotFingerToLocal(FingerName.Index, 1, fw_dn_sd(70, 15), v3.fw);// fw and not up because the new up is what it was fw
                RotFingerToLocal(FingerName.Middle, 1, fw_dn(70), v3.fw);
                RotFingerToLocal(FingerName.Ring, 1, fw_dn(70), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 1, fw_dn_sd(70, -10), v3.fw);

                RotFingerToLocal(FingerName.Thumb, 2, fw_dn(30), v3.fw);
                RotFingerToLocal(FingerName.Index, 2, fw_dn(75), v3.fw);
                RotFingerToLocal(FingerName.Middle, 2, fw_dn(75), v3.fw);
                RotFingerToLocal(FingerName.Ring, 2, fw_dn(75), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 2, fw_dn(75), v3.fw);

                RotFingerToLocal(FingerName.Thumb, 3, v3.fw.RotUp(10), v3.up);
                RotFingerToLocal(FingerName.Index, 3, fw_dn(80), v3.fw);
                RotFingerToLocal(FingerName.Middle, 3, fw_dn(80), v3.fw);
                RotFingerToLocal(FingerName.Ring, 3, fw_dn(80), v3.fw);
                RotFingerToLocal(FingerName.Pinky, 3, fw_dn(80), v3.fw);
            }

                

            StartFingerRotation(seconds);

            return this.AsUniqueNamed((unique.Hand + side) + human.ID);
        }
    }
}