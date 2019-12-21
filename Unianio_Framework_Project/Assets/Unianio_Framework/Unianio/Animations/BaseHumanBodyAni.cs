using System;
using Unianio.Animations.Common;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis.IK;
using Unianio.Graphs;
using Unianio.IK;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Animations
{
    public abstract class BaseHumanBodyAni<T> : AnimationManager where T : BaseHumanBodyAni<T>
    {
        public IComplexHuman Human { get; private set; }

        public FuncAni GetBlinkAni(double seconds, Func<double, double> func = null)
            => blinkAni(Human, seconds, func);
        protected void ReleaseCustomElbowBendDirArmL(double forSeconds = 0.5)
        {
            var last = ArmL.LastBenDir;
            if (ArmL.CustomBendDir == null) return;
            if ((last.sqrMagnitude) < 0.0001) return;
            StartFuncAni(forSeconds, x =>
            {
                ArmL.CustomBendDir = lerp(last, ArmL.ComputedBendDir, smoothstep(x));
            })
                .Then(() => ArmL.CustomBendDir = null);
        }
        protected void ReleaseCustomElbowBendDirArmR(double forSeconds = 0.5)
        {
            var last = ArmR.LastBenDir;
            if (ArmR.CustomBendDir == null) return;
            if ((last.sqrMagnitude) < 0.0001) return;
            StartFuncAni(forSeconds, x =>
            {
                ArmR.CustomBendDir = lerp(last, ArmR.ComputedBendDir, smoothstep(x));
            })
                .Then(() => ArmR.CustomBendDir = null);
        }
        protected void ReleaseCustomElbowBendDir(double forSeconds = 0.5)
        {
            var lastR = ArmR.LastBenDir;
            var lastL = ArmL.LastBenDir;
            if (ArmR.CustomBendDir == null && ArmL.CustomBendDir == null) return;
            if((lastR.sqrMagnitude + lastL.sqrMagnitude) < 0.0001) return; 

            StartFuncAni(forSeconds, x =>
                {
                    x = smoothstep(x);
                    ArmR.CustomBendDir = lerp(lastR, ArmR.ComputedBendDir, x);
                    ArmL.CustomBendDir = lerp(lastL, ArmL.ComputedBendDir, x);
                })
                .Then(() =>
                {
                    ArmR.CustomBendDir = null;
                    ArmL.CustomBendDir = null;
                });
        }
        protected void ScaleElbowBendFactorTo(double factor, double forSeconds = 1)
        {
            var iniL = ArmL.ElbowBendFactor;
            var iniR = ArmR.ElbowBendFactor;
            if (iniL.IsEqual(factor) && iniR.IsEqual(factor)) return;
            StartFuncAni(forSeconds, x =>
            {
                ArmL.ElbowBendFactor = lerp(iniL, factor, x);
                ArmR.ElbowBendFactor = lerp(iniR, factor, x);
            });
        }
        protected void SetNeckPathInModel(in Vector3 fw, in Vector3 up)
        {
            var midFw = slerp(in fw, in v3.fw, 0.5);
            var midUp = slerp(in up, in v3.up, 0.5);

            PathNeckLower.New.ModelRotToTarget(midFw, midUp).SetCondition(() => Human.AniFace.IsNotRunning());
            PathNeckUpper.New.ModelRotToTarget(fw, up).SetCondition(() => Human.AniFace.IsNotRunning());
        }
        protected void SetNeckPathInLocal(double degreesDown = 0, double degreesRight = 0)
        {
            PathNeckLower.New.LocalRotToTarget(NeckLower.IniLocalFw.RotBk(degreesDown / 2.0).RotRt(degreesRight / 2), NeckLower.IniLocalUp).SetCondition(() => Human.AniFace.IsNotRunning());
            PathNeckUpper.New.LocalRotToTarget(NeckUpper.IniLocalFw.RotDn(degreesDown).RotRt(degreesRight), NeckUpper.IniLocalUp).SetCondition(() => Human.AniFace.IsNotRunning());
        }
        protected float FootY => Human.Initial.FootHeight;
        protected float ArmY => Human.Initial.RelaxedArmHeight;
        protected virtual Vector3 RelaxedModelPosArmL => v3.up.By(0.95) + v3.lt.By(0.22);
        protected virtual Vector3 RelaxedModelPosArmR => v3.up.By(0.95) + v3.rt.By(0.22);
        protected void EnsureLegsOnGround()
        {
            var h = Human;
            if (LegL.position.y < h.Initial.FootHeight) LegL.position = LegL.position.WithY(h.Initial.FootHeight);
            if (LegR.position.y < h.Initial.FootHeight) LegR.position = LegR.position.WithY(h.Initial.FootHeight);
            if (LegL.position.y < h.Initial.FootHeight.By(1.6)) LegL.Handle.RotateTowards(h.fw.ToHorzUnit(), v3.up, fun.smoothDeltaTime * 360);
            if (LegR.position.y < h.Initial.FootHeight.By(1.6)) LegR.Handle.RotateTowards(h.fw.ToHorzUnit(), v3.up, fun.smoothDeltaTime * 360);
        }
        protected FuncAni GetCloseEyesAni(double seconds)
        {
            return GetBlinkAni(seconds, x => bezier(x, 0.50, 0.00, 0.50, 1.00));
        }
        protected FuncAni GetOpenEyesAni(double seconds)
        {
            return GetBlinkAni(seconds, x => bezier(x, 0.00, 1.00, 0.50, 1.00, 0.50, 0.01, 1.00, 0.00));
        }
        protected void PivotStarts(Vector3 pivotPoint, out Vector3 vecPivotIn)
        {
            vecPivotIn = (pivotPoint - Human.position);
        }
        protected void PivotEnds(Vector3 pivotPoint, Vector3 vecPivotIn)
        {
            var vecPivotOut = (Human.position - pivotPoint);
            Human.position += vecPivotIn + vecPivotOut;
        }


        public HandlePath PathCollarL => Human.PathCollarL;
        public HandlePath PathCollarR => Human.PathCollarR;
        public HandlePath PathHandL => Human.PathHandL;
        public HandlePath PathHandR => Human.PathHandR;
        public DirectionPath PathBendDirArmL => Human.PathBendDirArmL;
        public DirectionPath PathBendDirArmR => Human.PathBendDirArmR;
        public NumericPath PathElbowArmL => Human.PathElbowArmL;
        public NumericPath PathElbowArmR => Human.PathElbowArmR;
        public HandlePath PathArmL => Human.PathArmL;
        public HandlePath PathArmR => Human.PathArmR;
        public HandlePath PathLegL => Human.PathLegL;
        public HandlePath PathLegR => Human.PathLegR;
        public HandlePath PathFootL => Human.PathFootL;
        public HandlePath PathFootR => Human.PathFootR;
        public HandlePath PathToesL => Human.PathToesL;
        public HandlePath PathToesR => Human.PathToesR;
        public HandlePath PathPelvis => Human.PathPelvis;
        public HandlePath PathHip => Human.PathHip;
        public HandlePath PathHead => Human.PathHead;
        public HandlePath PathNeckLower => Human.PathNeckLower;
        public HandlePath PathNeckUpper => Human.PathNeckUpper;
        public HandlePath PathSpine => Human.PathSpine;
        public HandlePath PathJaw => Human.PathJaw;
        public HandlePath PathEyeL => Human.GenFace.PathEyeL;
        public HandlePath PathEyeR => Human.GenFace.PathEyeR;
        public FacePath PathFace => Human.PathFace;
        public HandlePath GetPath(HumLegChain leg) => leg.Side == BodySide.Left ? PathLegL : PathLegR;
        public HandlePath GetPath(HumArmChain arm) => arm.Side == BodySide.Left ? PathArmL : PathArmR;
        protected HandlePath OtherLegPath(HandlePath path) => path == PathLegL ? PathLegR : PathLegL;
        protected HandlePath OtherArmPath(HandlePath path) => path == PathArmL ? PathArmR : PathArmL;
        protected HumArmChain GetArm(HandlePath path) => path == PathArmL ? ArmL : ArmR;
        protected HumLegChain GetLeg(HandlePath path) => path == PathLegL ? LegL : LegR;
        public HumLegChain OtherLeg(HumLegChain leg) => leg.Side == BodySide.Left ? LegR : LegL;
        public HumArmChain OtherArm(HumArmChain arm) => arm.Side == BodySide.Left ? ArmR : ArmL;
        public HumArmChain GetArm(BodySide side) => side == BodySide.Left ? ArmL : ArmR;
        public HumLegChain GetLeg(BodySide side) => side == BodySide.Left ? LegL : LegR;
        public GenFaceGroup Face => Human.GenFace;
        public HumArmChain ArmL => Human.ArmL;
        public HumArmChain ArmR => Human.ArmR;
        public HumLegChain LegL => Human.LegL;
        public HumLegChain LegR => Human.LegR;
        public HumSpineChain Spine => Human.Spine;
        public HumBoneHandler Jaw => Human.IsGenesis8() ? Human.GenFace.LowerJaw : Human.MHFace.Jaw;
        public HumBoneHandler Pelvis => Human.Pelvis;
        public HumBoneHandler Hip => Human.Hip;
        public HumBoneHandler NeckLower => Human.Spine.NeckLowerHandler;
        public HumBoneHandler NeckUpper => Human.NeckUpper;
        public HumBoneHandler CollarL => Human.CollarL;
        public HumBoneHandler CollarR => Human.CollarR;
        public HumBoneHandler Head => Human.Head;
        public HumBoneHandler ToesL => Human.LegL.ToeHandler;
        public HumBoneHandler ToesR => Human.LegR.ToeHandler;
        public HumBoneHandler FootL => Human.LegL.FootHandler;
        public HumBoneHandler FootR => Human.LegR.FootHandler;
        public HumBoneHandler HandL => Human.ArmL.HandHandler;
        public HumBoneHandler HandR => Human.ArmR.HandHandler;
        public HumBoneHandler EyeL => Human.GenFace.EyeL;
        public HumBoneHandler EyeR => Human.GenFace.EyeR;
        protected void SetPathElbowTarget(double target)
        {
            PathElbowArmL.New.ToTarget(target);
            PathElbowArmR.New.ToTarget(target);
        }
        protected void ApplyElbowPaths(double x)
        {
            PathElbowArmL.Apply(x);
            PathElbowArmR.Apply(x);
        }
        protected virtual T SetHuman(IComplexHuman human)
        {
            Human = human;
            return (T)this;
        }
        protected T SetAsRoot(IComplexHuman human, ulong action = 0)
        {
            SetHuman(human);
            Human.AniEntireBody = this.SetLabel(action);
            return (T)this.AsUniqueNamed(unique.RootAni + human.ID);
        }
        protected T SetAsRootObserver(IComplexHuman human)
        {
            SetHuman(human);
            return (T)this.AsUniqueNamed(unique.RootObserverAni + human.ID);
        }
        protected void SetLegs(bool isRightOn,
            out HandlePath onLegPath, out HandlePath offLegPath)
        {
            onLegPath = isRightOn ? PathLegR : PathLegL;
            offLegPath = isRightOn ? PathLegL : PathLegR;
        }
        protected void SetLegs(bool isRightOn,
            out HumLegChain onLeg, out HumLegChain offLeg,
            out HandlePath onLegPath, out HandlePath offLegPath)
        {
            onLeg = isRightOn ? Human.LegR : Human.LegL;
            offLeg = isRightOn ? Human.LegL : Human.LegR;
            onLegPath = isRightOn ? PathLegR : PathLegL;
            offLegPath = isRightOn ? PathLegL : PathLegR;
        }
        protected void SetArms(bool isRightOn,
            out HandlePath onArmPath, out HandlePath offArmPath)
        {
            onArmPath = isRightOn ? PathArmR : PathArmL;
            offArmPath = isRightOn ? PathArmL : PathArmR;
        }
        protected void SetArms(bool isRightOn,
            out HumArmChain onArm, out HumArmChain offArm,
            out HandlePath onArmPath, out HandlePath offArmPath)
        {
            onArm = isRightOn ? Human.ArmR : Human.ArmL;
            offArm = isRightOn ? Human.ArmL : Human.ArmR;
            onArmPath = isRightOn ? PathArmR : PathArmL;
            offArmPath = isRightOn ? PathArmL : PathArmR;
        }
        public override string ToString()
        {
            return base.ToString() + "_" + Human.Persona;
        }
    }

}