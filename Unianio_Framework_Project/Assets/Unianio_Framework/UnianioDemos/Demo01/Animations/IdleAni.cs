using System.Collections;
using System.Collections.Generic;
using Unianio;
using Unianio.Animations;
using Unianio.Animations.MHHands;
using Unianio.Extensions;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace UnianioDemos.Demo01
{
    public class IdleAni : BaseHumanBodyAni<IdleAni>
    {
        public IdleAni Set(IComplexHuman human) => SetAsRoot(human);
        public override void Initialize() => Stand();
        void Stand()
        {
            SetLegs(LegR.position.y > LegL.position.y, 
                out var upLeg, out var dnLeg,
                out var upLegPath, out var dnLegPath);

            upLegPath.LocalPosCurveToTargetRelToMiddle(upLeg.IniLocalPos.RotDn(5), v3.up.By(0.3));
            dnLegPath.LocalPosLineToTarget(dnLeg.IniLocalPos.RotDn(5));

            PathArmL
                .LocalPosLineToTarget(ArmL.IniLocalPos.RotDn(35).RotBk(5))
                .ModelRotToTarget(v3.dn.RotLt(10), v3.lt);
            PathArmR
                .LocalPosLineToTarget(ArmR.IniLocalPos.RotDn(35).RotBk(5))
                .ModelRotToTarget(v3.dn.RotRt(10), v3.rt);

            hands<HumHandRelaxedAni>(Human);

            StartFuncAni(1, x =>
                {
                    PivotStarts(dnLeg.position, out var vecPivot);
                    {
                        apply(x, upLegPath, dnLegPath, PathArmL, PathArmR);
                    }
                    PivotEnds(dnLeg.position, vecPivot);
                })
                .Then(IdleMoves)
                ;
        }
        void IdleMoves()
        {

        }
    }
}