using Unianio;
using Unianio.Animations;
using Unianio.Animations.MHHands;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Moves;
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
                    hip => hip.IniLocalPos,
                    0.5,
                    hip => hip.IniLocalPos.By(0.94),
                    x => smootherstep(x),
                    x => smootherstep(x)
                );

            MoveSpine.Local
                .RotateTo(v3.fw.RotDir(stepSide, 15), v3.up)
                ;

            stepLegMove.Model
                .CurveRelToEnd(
                    stepSide.By(0.07).AddFw(0.32).WithY(FootY), v3.up, 0.5, x => smoothstep(x))
                .RotateTo2(v3.fw.RotDn(25), v3.up, 0.5, v3.fw.RotUp(25), v3.up)
                ; 
            pushLegMove.Model
                .LineTo(
                    pushSide.By(0.07).AddBk(0.30).WithY(FootY))
                    .WrapPos((x, v) => new Vector3(v.x, v.y + 0.08f * sharpstep(0.80, 1.00, x), v.z))
                .RotateTo2(
                    v3.fw, v3.up, 0.80, v3.fw.RotDn(30), v3.up, x => (x * 10).Clamp01()
                    , null)
                ;

            stepArmMove.Local.CurveRelToMid(
                stepArm.IniLocalPos.RotDn(30).RotBk(37).By(0.95), v3.dn, 0.2)
                .NaturalHandRotation();
            pushArmMove.Local.CurveRelToMid(
                pushArm.IniLocalPos.RotDn(30).RotFw(10).By(0.92), v3.dn, 0.2)
                .NaturalHandRotation();

            hands<HumHandRelaxedAni>(Human);
            var iniRot = Human.rotation;
            StartFuncAni(1, x =>
                {
                    PivotStarts(pushLeg.Toe.position, out var vecPivot);
                    {
                        apply(x, stepLegMove, pushLegMove, stepArmMove, pushArmMove, MoveSpine, MoveHip);
                        update(stepLeg, pushLeg);
                        if (_step.IsDivisibleBy(6))
                            Human.rotation = iniRot * Quaternion.AngleAxis(-120 * smootherstep(x), v3.up);
                    }
                    HorzPivotEnds(pushLeg.Toe.position, vecPivot);
                })
                .Then(() => Step(!stepWithRightFoot))
                ;
            ++_step;
        }
    }
}