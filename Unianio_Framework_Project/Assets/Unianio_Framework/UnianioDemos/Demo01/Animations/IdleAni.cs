using System.Collections;
using System.Collections.Generic;
using Unianio;
using Unianio.Animations;
using Unianio.Animations.MHHands;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Rigged;
using static Unianio.Static.fun;

namespace UnianioDemos.Demo01
{
    public class IdleAni : BaseHumanBodyAni<IdleAni>
    {
        public IdleAni Set(IComplexHuman human) => SetAsRoot(human);
        public override void Initialize() 
        {
            SetLegs(LegR.position.y > LegL.position.y, out var upLeg, out var dnLeg,out var upLegMove, out var dnLegMove);
            
            upLegMove.Model
                .CurveRelToMid(upLeg.SideDir.By(0.07).WithY(FootY), v3.up, 1)
                .RotateTo(v3.fw, v3.up);
            dnLegMove.Model
                .LineTo(dnLeg.SideDir.By(0.07).WithY(FootY))
                .RotateTo(v3.fw, v3.up);
            MoveArmL.Local
                .LineTo(ArmL.IniLocalPos.RotDn(33).RotBk(5).By(0.95))
                .NaturalHandRotation();
            MoveArmR.Local
                .LineTo(ArmR.IniLocalPos.RotDn(33).RotBk(5).By(0.95))
                .NaturalHandRotation();
            
            ToInitial(1, BodyPart.Hip, BodyPart.Pelvis, BodyPart.Spine, BodyPart.Neck, BodyPart.Head);
            hands<HumHandRelaxedAni>(Human);
            StartFuncAni(1, x =>
                {
                    PivotStarts(dnLeg.position, out var vecPivot);
                    {
                        apply(x, upLegMove, dnLegMove, MoveArmL, MoveArmR);
                        update(upLeg, dnLeg);
                    }
                    HorzPivotEnds(dnLeg.position, vecPivot);
                })
                .Then(Finish)
                ;
        }
    }
}