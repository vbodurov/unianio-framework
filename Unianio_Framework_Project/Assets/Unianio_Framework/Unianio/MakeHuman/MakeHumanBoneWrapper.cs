using System;
using System.Collections.Generic;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Human;
using Unianio.IK;
using UnityEngine;
using mb = Unianio.MakeHuman.mhBone;
using static Unianio.Static.fun;

namespace Unianio.MakeHuman
{
    public class MakeHumanBoneWrapperProcessed : MonoBehaviour { }
    public class MakeHumanBoneWrapper : IHumanBoneWrapper
    {
        const string WrappedSuffix = "_Wrapped";
        void IHumanBoneWrapper.Wrap(Transform model, string personaId, string customTag)
        {
            var d = model.PlaceChildrenInNonStrictDictionary();

            var alreadyProcessed = model.gameObject.GetComponent<MakeHumanBoneWrapperProcessed>();
            if(alreadyProcessed != null) return;

            Spine(d, model);
            Head(d, model);
            Arms(d, model);
            Legs(d, model);

            model.gameObject.AddComponent<MakeHumanBoneWrapperProcessed>();
        }
        void Spine(IDictionary<string, Transform> d, Transform model)
        {
            var fw = model.forward;
            var up = model.up;
            
            

            // pelvis
            var pelvis = new GameObject("pelvis").transform;
            pelvis.SetParent(GetBone(d, mb.root));
            var lPelvis = GetBone(d, mb.pelvis_L);
            var rPelvis = GetBone(d, mb.pelvis_R);
            pelvis.localPosition = lPelvis.localPosition;
            pelvis.rotation = lookAt(fw, up);
            lPelvis.SetParent(pelvis);
            rPelvis.SetParent(pelvis);
            lPelvis.localPosition = v3.zero;
            rPelvis.localPosition = v3.zero;

            // hip
            RotateBone(d, mb.root, fw, up);

            RotateBone(d, mb.pelvis_L, fw, up);
            RotateBone(d, mb.pelvis_R, fw, up);

            RotateBone(d, mb.spine05, fw, up);
            RotateBone(d, mb.spine04, mb.spine03, -fw);
            RotateBone(d, mb.spine03, mb.spine02, -fw);
            RotateBone(d, mb.spine02, mb.neck01, -fw);
            RotateBone(d, mb.spine01, mb.neck01, -fw);

            var breastL = GetBone(d, mb.breast_L);
            RotateBone(d, mb.breast_L, -breastL.right, breastL.forward);
            var breastR = GetBone(d, mb.breast_R);
            RotateBone(d, mb.breast_R, -breastR.right, breastR.forward);
        }
        void Head(IDictionary<string, Transform> d, Transform model)
        {
            var fw = model.forward;
            var bk = -fw;
            var up = model.up;
            var rt = model.right;
            var lt = -rt;

            RotateBone(d, mb.neck01, fw, up);
            RotateBone(d, mb.neck02, fw, up);
            RotateBone(d, mb.neck03, fw, up);
            RotateBone(d, mb.head, fw, up);

            // those we change because they are parents of the eyes
            RotateBone(d, mb.special05_L, fw, up);
            RotateBone(d, mb.special06_L, fw, up);
            RotateBone(d, mb.special05_R, fw, up);
            RotateBone(d, mb.special06_R, fw, up);

            // mouth
            RotateBone(d, mb.jaw, fw, up);
            RotateBone(d, mb.special01, mb.oris06, up);// upper mouth
            RotateBone(d, mb.oris06, mb.oris05, fw);
            var oris05 = GetBone(d,mb.oris05);
            RotateBone(d, mb.oris05, -oris05.right, fw);


            RotateBone(d, mb.oris04_L, mb.oris03_L, up);
            var oris03_L = GetBone(d,mb.oris03_L);
            RotateBone(d, mb.oris03_L, -oris03_L.right, lt);

            RotateBone(d, mb.oris04_R, mb.oris03_R, up);
            var oris03_R = GetBone(d,mb.oris03_R);
            RotateBone(d, mb.oris03_R, -oris03_R.right, rt);

            RotateBone(d, mb.special04, mb.oris02, up);// lower mouth
            RotateBone(d, mb.oris02, mb.oris01, bk);
            var oris01 = GetBone(d,mb.oris01);
            RotateBone(d, mb.oris01, -oris01.right, bk);

            RotateBone(d, mb.oris06_L, mb.oris07_L, up);
            var oris07_L = GetBone(d,mb.oris07_L);
            RotateBone(d, mb.oris07_L, -oris07_L.right, rt);

            RotateBone(d, mb.oris06_R, mb.oris07_R, up);
            var oris07_R = GetBone(d,mb.oris07_R);
            RotateBone(d, mb.oris07_R, -oris07_R.right, lt);


            // levator smile control
            RotateBone(d, mb.levator02_L, mb.levator03_L, up);
            RotateBone(d, mb.levator03_L, mb.levator04_L, lt);
            RotateBone(d, mb.levator04_L, mb.levator05_L, lt);
            var levator05_L = GetBone(d,mb.levator05_L);
            RotateBone(d, mb.levator05_L, -levator05_L.right, lt);

            RotateBone(d, mb.levator02_R, mb.levator03_R, up);
            RotateBone(d, mb.levator03_R, mb.levator04_R, rt);
            RotateBone(d, mb.levator04_R, mb.levator05_R, rt);
            var levator05_R = GetBone(d,mb.levator05_R);
            RotateBone(d, mb.levator05_R, -levator05_R.right, rt);

            // cheeks
            RotateBone(d, mb.temporalis02_L, mb.risorius02_L, up);
            RotateBone(d, mb.risorius02_L, mb.risorius03_L, fw);
            var risorius03_L = GetBone(d,mb.risorius03_L);
            RotateBone(d, mb.risorius03_L, -risorius03_L.right, fw);

            RotateBone(d, mb.temporalis02_R, mb.risorius02_R, up);
            RotateBone(d, mb.risorius02_R, mb.risorius03_R, fw);
            var risorius03_R = GetBone(d,mb.risorius03_R);
            RotateBone(d, mb.risorius03_R, -risorius03_R.right, fw);

            // eyebrows
            RotateBone(d, mb.temporalis01_L, mb.oculi02_L, up);
            RotateBone(d, mb.oculi02_L, mb.oculi01_L, up);
            var oculi01_L = GetBone(d,mb.oculi01_L);
            RotateBone(d, mb.oculi02_L, -oculi01_L.right, up);
            RotateBone(d, mb.oculi01_L, oculi01_L.forward, oculi01_L.up);

            RotateBone(d, mb.temporalis01_R, mb.oculi02_R, up);
            RotateBone(d, mb.oculi02_R, mb.oculi01_R, up);
            var oculi01_R = GetBone(d,mb.oculi01_R);
            RotateBone(d, mb.oculi02_R, -oculi01_R.right, up);
            RotateBone(d, mb.oculi01_R, oculi01_R.forward, oculi01_R.up);

            // eyes
            var eyeL = GetBone(d,mb.eye_L);
            var eyeR = GetBone(d,mb.eye_R);
            RotateBone(d, mb.eye_L, -eyeL.right, up);
            RotateBone(d, mb.eye_R, -eyeR.right, up);


            // eyelids
            var eyelidUpR = GetBone(d,mb.orbicularis03_R);
            RotateBone(d, mb.orbicularis03_R, -eyelidUpR.right, eyelidUpR.forward);
            var eyelidDnR = GetBone(d,mb.orbicularis04_R);
            RotateBone(d, mb.orbicularis04_R, -eyelidDnR.right, eyelidDnR.forward);
            var eyelidUpL = GetBone(d,mb.orbicularis03_L);
            RotateBone(d, mb.orbicularis03_L, -eyelidUpL.right, eyelidUpL.forward);
            var eyelidDnL = GetBone(d,mb.orbicularis04_L);
            RotateBone(d, mb.orbicularis04_L, -eyelidDnL.right, eyelidDnL.forward);
        }
        void Arms(IDictionary<string, Transform> d, Transform model)
        {
            var modelFw = model.forward;
            var modelUp = model.up;

            foreach (var sd in new[] { BodySide.Right, BodySide.Left })
            {
//                var clavicle = GetBone(d,mb.clavicle_L.MHBonePx(sd)).position;
                var shoulder = GetBone(d,mb.upperarm01_L.MHBonePx(sd)).position;
                var elbow = GetBone(d,mb.lowerarm01_L.MHBonePx(sd)).position;
                var index1 = GetBone(d,mb.finger2_1_L.MHBonePx(sd)).position;
                var pinky1 = GetBone(d,mb.finger5_1_L.MHBonePx(sd)).position;
                var hand = GetBone(d,mb.wrist_L.MHBonePx(sd)).position;

                var armSide = sd.IsRight() ? model.right : -model.right;
                var upperArmFw = shoulder.DirTo(in elbow);
                var shoulderUp = upperArmFw.GetRealUp(in armSide, -modelUp);

                point.GetNormal(in shoulder, in elbow, in hand, out var elbowUp);
                vector.EnsurePointSameDirAs(in elbowUp, in modelUp, out elbowUp);

                var handUp = point.GetNormal(in index1, in pinky1, in hand);
                vector.EnsurePointSameDirAs(in handUp, in v3.up, out handUp);

                var shoulderTwistUp = slerp(in shoulderUp, in elbowUp, 0.5)
                    .ProjectOnPlaneAndNormalize(in upperArmFw);

                var lowerArmFw = elbow.DirTo(in hand);
                var forearmTwistUp = slerp(in handUp, in elbowUp, 0.5)
                    .ProjectOnPlaneAndNormalize(in lowerArmFw);

                RotateBone(d, mb.clavicle_L.MHBonePx(sd), modelFw, modelUp);
                RotateBone(d, mb.shoulder01_L.MHBonePx(sd), mb.upperarm01_L.MHBonePx(sd), modelUp);
                var fwSh = RotateBone(d, mb.upperarm01_L.MHBonePx(sd), mb.lowerarm01_L.MHBonePx(sd), shoulderUp);
                RotateBone(d, mb.upperarm02_L.MHBonePx(sd), fwSh, shoulderTwistUp);
                var fwFa = RotateBone(d, mb.lowerarm01_L.MHBonePx(sd), mb.wrist_L.MHBonePx(sd), elbowUp);
                RotateBone(d, mb.lowerarm02_L.MHBonePx(sd), fwFa, forearmTwistUp);

                RotateBone(d, mb.wrist_L.MHBonePx(sd), mb.finger3_1_L.MHBonePx(sd), handUp);

                RotateBone(d, mb.metacarpal1_L.MHBonePx(sd), mb.finger2_1_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger2_1_L.MHBonePx(sd), mb.finger2_2_L.MHBonePx(sd), handUp);
                var currFw = RotateBone(d, mb.finger2_2_L.MHBonePx(sd), mb.finger2_3_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger2_3_L.MHBonePx(sd), currFw, handUp);

                RotateBone(d, mb.metacarpal2_L.MHBonePx(sd), mb.finger3_1_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger3_1_L.MHBonePx(sd), mb.finger3_2_L.MHBonePx(sd), handUp);
                currFw = RotateBone(d, mb.finger3_2_L.MHBonePx(sd), mb.finger3_3_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger3_3_L.MHBonePx(sd), currFw, handUp);

                RotateBone(d, mb.metacarpal3_L.MHBonePx(sd), mb.finger4_1_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger4_1_L.MHBonePx(sd), mb.finger4_2_L.MHBonePx(sd), handUp);
                currFw = RotateBone(d, mb.finger4_2_L.MHBonePx(sd), mb.finger4_3_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger4_3_L.MHBonePx(sd), currFw, handUp);

                RotateBone(d, mb.metacarpal4_L.MHBonePx(sd), mb.finger5_1_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger5_1_L.MHBonePx(sd), mb.finger5_2_L.MHBonePx(sd), handUp);
                currFw = RotateBone(d, mb.finger5_2_L.MHBonePx(sd), mb.finger5_3_L.MHBonePx(sd), handUp);
                RotateBone(d, mb.finger5_3_L.MHBonePx(sd), currFw, handUp);

                var thumb1 = GetBone(d,mb.finger1_1_L.MHBonePx(sd)).position;
                var thumb2 = GetBone(d,mb.finger1_2_L.MHBonePx(sd)).position;
                var thumb3 = GetBone(d,mb.finger1_3_L.MHBonePx(sd)).position;

                var thumbUp = point.GetNormal(thumb1, index1, thumb3);
                vector.EnsurePointSameDirAs(in thumbUp, in v3.up, out thumbUp);
                var thumbInside = vector.GetNormal(thumb1.DirTo(thumb2), thumbUp);
                var insideDir = sd == BodySide.Left ? model.right : -model.right;
                vector.EnsurePointSameDirAs(in thumbInside, in insideDir, out thumbInside);
                thumbUp = thumbUp.RotateTowards(in insideDir, 22);

                RotateBone(d, mb.finger1_1_L.MHBonePx(sd), mb.finger1_2_L.MHBonePx(sd), thumbUp);
                var fwTh = RotateBone(d, mb.finger1_2_L.MHBonePx(sd), mb.finger1_3_L.MHBonePx(sd), thumbUp);
                RotateBone(d, mb.finger1_3_L.MHBonePx(sd), fwTh, thumbUp);
            }
        }
        void Legs(IDictionary<string, Transform> d, Transform model)
        {
            var fw = model.forward;
            var up = model.up;

            foreach (var sd in new[] { BodySide.Right, BodySide.Left })
            {
                RotateBone(d, mb.upperleg01_L.MHBonePx(sd), mb.lowerleg01_L.MHBonePx(sd), fw);
                RotateBone(d, mb.upperleg02_L.MHBonePx(sd), mb.lowerleg01_L.MHBonePx(sd), fw);
                RotateBone(d, mb.lowerleg01_L.MHBonePx(sd), mb.foot_L.MHBonePx(sd), fw);
                RotateBone(d, mb.lowerleg02_L.MHBonePx(sd), mb.foot_L.MHBonePx(sd), fw);
                RotateBone(d, mb.foot_L.MHBonePx(sd), fw, up);
                RotateBone(d, mb.toe1_1_L.MHBonePx(sd), fw, up);
            }
        }

        Vector3 RotateBone(IDictionary<string, Transform> d, string boneName, Vector3 fw, Vector3 up)
        {
            var bone = GetBone(d, boneName);
            var children = bone.GetDirectChildren();
            for (var i = 0; i < children.Length; ++i)
            {
                // if already wrapped
                if (children[i].name.EndsWith(WrappedSuffix))
                {
                    return bone.forward;
                }
            }
            bone.name = boneName + WrappedSuffix;
            var wrapper = new GameObject(boneName).transform;
            if(bone.parent == null) throw new InvalidOperationException($"Bone {boneName} has no parent");
            wrapper.position = bone.position;
            wrapper.rotation = lookAt(fw, fw.GetRealUp(up));
            wrapper.SetParent(bone.parent);
            bone.SetParent(wrapper);
            for (var i = 0; i < children.Length; ++i)
            {
                children[i].SetParent(wrapper);
            }
            
            return wrapper.forward;
        }
        Vector3 RotateBone(IDictionary<string, Transform> d, string boneName, string nextBone, Vector3 up)
        {
            var next = GetBone(d, nextBone);
            var curr = GetBone(d, boneName);
            return RotateBone(d, boneName, curr.DirTo(next), up);
        }
        Transform GetBone(IDictionary<string, Transform> d, string name)
        {
            return d[name] ?? d[name.Replace("_", ".")] ?? throw new ArgumentException($"Cannot find bone '{name}'");
        }
    }
}