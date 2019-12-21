using System;
using System.Net.Mail;
using Unianio.Animations;
using Unianio.Animations.Common;
using Unianio.Enums;
using Unianio.Genesis;
using Unianio.Genesis.IK;
using Unianio.IK;
using Unianio.Rigged.IK;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class ComplexHumanExtensions
    {
        public static bool IsMakeHuman(this IComplexHuman h)
            => h.Definition.HumanoidType == HumanoidType.MakeHuman;
        public static bool IsGenesis8(this IComplexHuman h)
            => h.Definition.HumanoidType == HumanoidType.Genesis8;
        public static ulong AddFlag(this IComplexHuman human, ulong flag)
        {
            human.Flags |= flag;
            return human.Flags;
        }
        public static ulong RemoveFlag(this IComplexHuman human, ulong flag)
        {
            human.Flags &= ~flag;
            return human.Flags;
        }

        public static bool IsInAction(this IComplexHuman h, ulong action) => h.AniEntireBody.IsRunningWithLabel(action);
        public static ulong GetActionLabel(this IComplexHuman h) => h.AniEntireBody.GetLabel();
        // 0f, -0.5f, 0.8660254f = Debug.Log(v3.fw.ToDn(30).s());
        // 0f,-0.7071068f,0.7071067f = Debug.Log(">>"+v3.fw.ToDn(45).s());
        public static Vector3 DownForward(this IComplexHuman h) => h.rotation * new Vector3(0f, -0.7071068f, 0.7071067f);
        public static bool IsForwardFacing(this IComplexHuman h) => vector.PointSameDirection(h.rotation * new Vector3(0f, -0.7071068f, 0.7071067f), in v3.fw);
        public static bool IsForwardFacingHorizontal(this IComplexHuman h) => vector.PointSameDirection(h.forward, in v3.fw);

        public static Vector3 GetCurrentLookTarget(this IComplexHuman h) =>
            h.IsGenesis8() 
                ? h.GenFace.EyeR.position + h.GenFace.EyeR.forward.By(3) 
                : h.MHFace.EyeR.position + h.MHFace.EyeR.forward.By(3);
        public static Vector3 GetCurrentFaceTarget(this IComplexHuman h) =>
            h.NeckUpper.position + h.NeckUpper.forward.By(3);
        static double GetBlinkBaseByTargetDirection(this IComplexHuman h, ref Vector3 targetDir)
        {
            var iniHeadUp = h.Head.up;
            var dp = dot(in targetDir, in iniHeadUp);
            var blinkBaseTarget = 0.20;
            if (dp > 0.050) blinkBaseTarget = 0.00;
            if (dp > 0.025) blinkBaseTarget = 0.10;
            if (dp < -0.025) blinkBaseTarget = 0.30;
            else if (dp < -0.050) blinkBaseTarget = 0.40;
            return blinkBaseTarget;
        }
        public static bool IsPointProjectedToUpperBody(this IComplexHuman h, Vector3 point)
        {
            fun.point.ProjectOnLine(in point, h.position, h.Head.position, out var proj);
            return fun.point.IsAbovePlane(in proj, h.up, h.Spine.AbdomenUpper.position);
        }
        /*public static bool TryLookAtTarget(this IComplexHuman h, double seconds, Func<Vector3> getTarget, Action onEnd = null)
        {
            onEnd = onEnd ?? new Action(() => { });


            var eyeL = h.GenFace.EyeL;
            var eyeR = h.GenFace.EyeR;
            var iniFw = eyeL.position.DirTo(getTarget());
            var blinkBaseTarget = h.GetBlinkBaseByTargetDirection(ref iniFw);

            h.AniLook?.ClearAllFollowUpActions().StopIfRunning();
            h.AniLook = play<FuncAni>().Set(seconds, _ =>
            {
                h.FaceExp.BlinkBase01 = h.FaceExp.BlinkBase01.MoveTowards(blinkBaseTarget, fun.smoothDeltaTime * 2);
                if (h.AniBlink.IsNotRunning()) h.FaceExp.Blink01 = 0;
                
                var p = getTarget();
                var fwL = eyeL.position.DirTo(p);
                var fwR = eyeR.position.DirTo(p);
                
                if(angleIsLessThan(fwL, h.Head.forward, 30))
                {
                    var hUp = h.Head.up;
                    eyeL.RotateTowards(Quaternion.LookRotation(fwL, hUp), fun.smoothDeltaTime * 90);
                    eyeR.RotateTowards(Quaternion.LookRotation(fwR, hUp), fun.smoothDeltaTime * 90);
                }
            })
            .AsUniqueNamed(unique.Look + h.ID)
            .Then(onEnd)
            ;
            return true;
        }*/
        
        public static Vector3 GetAbsMouthPosition(this IComplexHuman h)
        {
            var head = h.Head.Holder;
            return Vector3.Lerp(h.GenFace.LipCornerL.position, h.GenFace.LipCornerR.position, 0.5f) + head.forward * 0.03f + head.up * -0.005f;
        }
        public static Vector3 GetAbsLipBoneCenterPosition(this IComplexHuman h)
        {
            return Vector3.Lerp(h.GenFace.LipCornerL.position, h.GenFace.LipCornerR.position, 0.5f);
        }
        public static IComplexHuman RotateTowards(this IComplexHuman h, Quaternion rotation, double stepDegrees = -1)
        {
            h.Model.RotateTowards(rotation, stepDegrees);
            return h;
        }
        public static IComplexHuman RotateTowards(this IComplexHuman h, Vector3 forward, Vector3 up, double stepDegrees = -1)
        {
            h.Model.RotateTowards(forward, up, stepDegrees);
            return h;
        }
        public static IComplexHuman MoveTowards(this IComplexHuman h, Vector3 target, double step = -1)
        {
            h.Model.MoveTowards(target, step);
            return h;
        }
    }
}
