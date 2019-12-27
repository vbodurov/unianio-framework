using System;
using System.Collections.Generic;
using Unianio.Animations.Common;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis.IK;
using Unianio.IK;
using Unianio.Moves;
using Unianio.Rigged;
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
        protected void ToInitial(double seconds, params BodyPart[] parts)
        {
            if (parts == null || parts.Length == 0) return;
            var paths = new List<IExecutorOfProgress>();
            for (int i = 0; i < parts.Length; i++)
            {
                var ioh = GetInitialOrientationHolder(parts[i]);
                paths.Add(ioh.ToInitialLocalPosition());
                paths.Add(ioh.ToInitialLocalRotation());
            }
            StartFuncAni(seconds, x => applyAll(smoothstep(x), paths));
        }
        IInitialOrientationHolder GetInitialOrientationHolder(BodyPart bp)
        {
            switch (bp)
            {
                case BodyPart.ArmL: return ArmL;
                case BodyPart.ArmR: return ArmR;
                case BodyPart.Pelvis: return Pelvis;
                case BodyPart.LegL: return LegL;
                case BodyPart.LegR: return LegR;
                case BodyPart.Spine: return Spine;
                case BodyPart.Head: return Head;
                case BodyPart.EyeL: return EyeL;
                case BodyPart.EyeR: return EyeR;
                case BodyPart.Jaw: return Jaw;
                case BodyPart.HandL: return HandL;
                case BodyPart.HandR: return HandR;
                case BodyPart.NeckLower: return NeckLower;
                case BodyPart.Neck: return NeckUpper;
                case BodyPart.ToesL: return ToesL;
                case BodyPart.ToesR: return ToesR;
//                case BodyPart.BreastL: return BreastL;
//                case BodyPart.BreastR: return BreastR;
                case BodyPart.FootL: return FootL;
                case BodyPart.FootR: return FootR;
                case BodyPart.Hip: return Hip;
            }
            return null;
        }
        /*protected void ReleaseCustomElbowBendDirArmL(double forSeconds = 0.5)
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
        }*/
//        protected void SetNeckPathInModel(in Vector3 fw, in Vector3 up)
//        {
//            var midFw = slerp(in fw, in v3.fw, 0.5);
//            var midUp = slerp(in up, in v3.up, 0.5);
//
//            PathNeckLower.New.ModelRotToTarget(midFw, midUp).SetCondition(() => Human.AniFace.IsNotRunning());
//            PathNeckUpper.New.ModelRotToTarget(fw, up).SetCondition(() => Human.AniFace.IsNotRunning());
//        }
//        protected void SetNeckPathInLocal(double degreesDown = 0, double degreesRight = 0)
//        {
//            PathNeckLower.New.LocalRotToTarget(NeckLower.IniLocalFw.RotBk(degreesDown / 2.0).RotRt(degreesRight / 2), NeckLower.IniLocalUp).SetCondition(() => Human.AniFace.IsNotRunning());
//            PathNeckUpper.New.LocalRotToTarget(NeckUpper.IniLocalFw.RotDn(degreesDown).RotRt(degreesRight), NeckUpper.IniLocalUp).SetCondition(() => Human.AniFace.IsNotRunning());
//        }
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
        protected void HorzPivotEnds(Vector3 pivotPoint, Vector3 vecPivotIn)
        {
            var vecPivotOut = (Human.position - pivotPoint);
            Human.position += (vecPivotIn.WithY(0) + vecPivotOut.WithY(0));
        }

        public Mover<HumBoneHandler> MoveHandL => Human.MoveHandL;
        public Mover<HumBoneHandler> MoveHandR => Human.MoveHandR;
        public Mover<IHumArmChain> MoveArmL => Human.MoveArmL;
        public Mover<IHumArmChain> MoveArmR => Human.MoveArmR;
        public Mover<HumLegChain> MoveLegL => Human.MoveLegL;
        public Mover<HumLegChain> MoveLegR => Human.MoveLegR;
        public Mover<HumBoneHandler> MoveFootL => Human.MoveFootL;
        public Mover<HumBoneHandler> MoveFootR => Human.MoveFootR;
        public Mover<HumBoneHandler> MoveToesL => Human.MoveToesL;
        public Mover<HumBoneHandler> MoveToesR => Human.MoveToesR;
        public Mover<HumBoneHandler> MovePelvis => Human.MovePelvis;
        public Mover<HumBoneHandler> MoveHip => Human.MoveHip;
        public Mover<HumBoneHandler> MoveHead => Human.MoveHead;
        public Mover<HumBoneHandler> MoveNeckLower => Human.MoveNeckLower;
        public Mover<HumBoneHandler> MoveNeckUpper => Human.MoveNeckUpper;
        public Mover<HumSpineChain> MoveSpine => Human.MoveSpine;
        public Mover<HumBoneHandler> MoveJaw => Human.MoveJaw;
        public Mover<HumBoneHandler> MoveEyeL => Human.GenFace.MoveEyeL;
        public Mover<HumBoneHandler> MoveEyeR => Human.GenFace.MoveEyeR;

        protected Mover<HumLegChain> GetMove(HumLegChain leg) => leg.Side == BodySide.Left ? MoveLegL : MoveLegR;
        protected Mover<IHumArmChain> GetMove(IHumArmChain arm) => arm.Side == BodySide.Left ? MoveArmL : MoveArmR;
        protected Mover<HumLegChain> OtherLegMove(Mover<HumLegChain> Move) => Move == MoveLegL ? MoveLegR : MoveLegL;
        protected Mover<IHumArmChain> OtherArmMove(Mover<IHumArmChain> Move) => Move == MoveArmL ? MoveArmR : MoveArmL;
        public HumLegChain OtherLeg(HumLegChain leg) => leg.Side == BodySide.Left ? LegR : LegL;
        public IHumArmChain OtherArm(IHumArmChain arm) => arm.Side == BodySide.Left ? ArmR : ArmL;
        public IHumArmChain GetArm(BodySide side) => side == BodySide.Left ? ArmL : ArmR;
        public HumLegChain GetLeg(BodySide side) => side == BodySide.Left ? LegL : LegR;
        public GenFaceGroup Face => Human.GenFace;
        public IHumArmChain ArmL => Human.ArmL;
        public IHumArmChain ArmR => Human.ArmR;
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
        /*protected void SetPathElbowTarget(double target)
        {
            PathElbowArmL.New.ToTarget(target);
            PathElbowArmR.New.ToTarget(target);
        }
        protected void ApplyElbowPaths(double x)
        {
            PathElbowArmL.Apply(x);
            PathElbowArmR.Apply(x);
        }*/
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
            out Mover<HumLegChain> onLegMove, out Mover<HumLegChain> offLegMove)
        {
            onLegMove = isRightOn ? MoveLegR : MoveLegL;
            offLegMove = isRightOn ? MoveLegL : MoveLegR;
        }
        protected void SetLegs(bool isRightOn,
            out HumLegChain onLeg, out HumLegChain offLeg,
            out Mover<HumLegChain> onLegMove, out Mover<HumLegChain> offLegMove)
        {
            onLeg = isRightOn ? Human.LegR : Human.LegL;
            offLeg = isRightOn ? Human.LegL : Human.LegR;
            onLegMove = isRightOn ? MoveLegR : MoveLegL;
            offLegMove = isRightOn ? MoveLegL : MoveLegR;
        }
        protected void SetArms(bool isRightOn,
            out Mover<IHumArmChain> onArmMove, out Mover<IHumArmChain> offArmMove)
        {
            onArmMove = isRightOn ? MoveArmR : MoveArmL;
            offArmMove = isRightOn ? MoveArmL : MoveArmR;
        }
        protected void SetArms(bool isRightOn,
            out IHumArmChain onArm, out IHumArmChain offArm,
            out Mover<IHumArmChain> onArmMove, out Mover<IHumArmChain> offArmMove)
        {
            onArm = isRightOn ? Human.ArmR : Human.ArmL;
            offArm = isRightOn ? Human.ArmL : Human.ArmR;
            onArmMove = isRightOn ? MoveArmR : MoveArmL;
            offArmMove = isRightOn ? MoveArmL : MoveArmR;
        }
        public override string ToString()
        {
            return base.ToString() + "_" + Human.Persona;
        }
    }

}