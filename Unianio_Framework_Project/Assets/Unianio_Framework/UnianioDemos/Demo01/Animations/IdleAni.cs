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
            SetLegs(LegR.position.y > LegL.position.y, out var upLeg, out var dnLeg,out var upLegPath, out var dnLegPath);
            
            upLegPath.Model.CurveRelToMid(upLeg.SideDir.By(0.07).WithY(FootY), v3.up.By(0.3));
            dnLegPath.Model.LineTo(dnLeg.SideDir.By(0.07).WithY(FootY));
            MoveArmL
                .Local
                .LineTo(ArmL.IniLocalPos.RotDn(33).RotBk(5).By(0.95))
                .Model
                .RotateTo(v3.dn.RotLt(10), v3.lt);
            MoveArmR
                .Local
                .LineTo(ArmR.IniLocalPos.RotDn(33).RotBk(5).By(0.95))
                .Model
                .RotateTo(v3.dn.RotRt(10), v3.rt);
            
            ToInitial(1, BodyPart.Hip, BodyPart.Pelvis, BodyPart.Spine, BodyPart.Neck, BodyPart.Head);
            hands<HumHandRelaxedAni>(Human);
            StartFuncAni(1, x =>
                {
                    PivotStarts(dnLeg.position, out var vecPivot);
                    {
                        apply(x, upLegPath, dnLegPath, MoveArmL, MoveArmR);
                        update(upLeg, dnLeg);
                    }
                    HorzPivotEnds(dnLeg.position, vecPivot);
                })
                .Then(Finish)
                ;
        }
    }
}