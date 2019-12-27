using Unianio;
using Unianio.Animations;
using Unianio.Extensions;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace UnianioDemos.Demo01
{
    public class WalkAni : BaseHumanBodyAni<WalkAni>
    {
        static int _step;
        public WalkAni Set(IComplexHuman human) => SetAsRoot(human);
        public override void Initialize() => Step(true);
        void Step(bool stepWithRightFoot)
        {
            SetLegs(stepWithRightFoot, out var stepLeg, out var pushLeg, out var stepLegMove, out var pushLegMove);
            SetArms(stepWithRightFoot, out var stepArm, out var pushArm, out var stepArmMove, out var pushArmMove);

            var pushSide = pushArm.SideDir;
            var stepSide = stepArm.SideDir;

            MoveHip.Local
                .LineTo2(
                    hip => hip.IniLocalPos.By(0.99) + pushSide.By(0.02),
                    0.4,
                    hip => hip.IniLocalPos.By(0.92) + stepSide.By(0.02)
                );

            MoveSpine.Local
                .RotateTo(v3.fw.RotDir(stepSide, 15), v3.up)
                ;

            stepLegMove.Model
                .CurveRelToMid(
                    stepSide.By(0.07).AddFw(0.3).WithY(FootY), v3.up.By(0.5))
                .RotateTo2(v3.fw.RotDn(30), v3.up, 0.5, v3.fw, v3.up)
                ;
            pushLegMove.Model
                .LineTo2(
                    pushSide.By(0.07).AddBk(0.30).WithY(FootY),
                    0.8,
                    pushSide.By(0.07).AddBk(0.35).WithY(FootY * 1.5))
                .RotateTo2(
                    v3.fw, v3.up,
                    0.8,
                    v3.fw.RotDn(20), v3.up)
                ;

            stepArmMove.Local.LineTo(
                stepArm.IniLocalPos.RotDn(30).RotBk(20).By(0.9));
            pushArmMove.Local.LineTo(
                pushArm.IniLocalPos.RotDn(30).RotFw(20).By(0.9));

            var iniRot = Human.rotation;
            StartFuncAni(1, x =>
            {
                PivotStarts(pushLeg.position, out var vecPivot);
                {
                    apply(x, stepLegMove, pushLegMove, stepArmMove, pushArmMove, MoveSpine, MoveHip);
                    update(stepLeg, pushLeg);
                    ensureHandsRotation(stepArm, stepSide, pushArm, pushSide);
                    if (_step.IsDivisibleBy(6))
                        Human.rotation = iniRot * Quaternion.AngleAxis(-120 * smoothstep(x), v3.up);
                }
                HorzPivotEnds(pushLeg.position, vecPivot);
            })
                .Then(() => Step(!stepWithRightFoot))
                ;
            ++_step;
        }
    }
}