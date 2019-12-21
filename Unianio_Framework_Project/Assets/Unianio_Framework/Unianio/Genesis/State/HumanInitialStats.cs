using System.Collections.Concurrent;
using System.Collections.Generic;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.IK;
using UnityEngine;

namespace Unianio.Genesis.State
{
    public class HumanInitialStats
    {
        public HumanInitialStats(IComplexHuman h)
        {
            if (h.IsGenesis8())
            {
                var mouth = h.GetAbsMouthPosition();
                LowerNeckToMouthRelVec = (mouth - h.Spine.Handle.position).AsLocalVec(h.Spine.NeckLower);
                PelvisToMouthDistance = h.Pelvis.position.DistanceTo(mouth);
            }
            
            FootHeight = h.LegL.Foot.position.y - h.position.y;
            PelvisHeight = h.Pelvis.position.y - h.position.y;
            HipHeight = h.Hip.position.y - h.position.y;
            ShoulderHeight = h.ArmL.Shoulder.position.y - h.position.y;
            RelaxedArmHeight = (ShoulderHeight - h.ArmL.MaxStretch);

            ModelVecHipToSpine = (h.Spine.position - h.Hip.position).AsLocalVec(h.Model);

            LocPosHead = h.Head.Holder.localPosition;
            LocRotHead = h.Head.Holder.localRotation;
            if (h.IsGenesis8())
            {
                LocPosJaw = h.GenFace.LowerJaw.Holder.localPosition;
                LocRotJaw = h.GenFace.LowerJaw.Holder.localRotation;
            }
            else if (h.IsMakeHuman())
            {
                LocPosJaw = h.MHFace.Jaw.Holder.localPosition;
                LocRotJaw = h.MHFace.Jaw.Holder.localRotation;
            }

            ModelPosArmL = h.ArmL.position.AsLocalPoint(h.Model);
            ModelPosArmR = h.ArmR.position.AsLocalPoint(h.Model);

            LocalPosArmL = h.ArmL.Handle.localPosition;
            LocalPosArmR = h.ArmR.Handle.localPosition;

            LocRotArmL = h.ArmL.Handle.localRotation;
            LocRotArmR = h.ArmR.Handle.localRotation;

            Fingers[BodySide.Left] = new Dictionary<FingerName, IDictionary<int, PosRot>>()
            {
                { FingerName.Thumb, new Dictionary<int, PosRot>() },
                { FingerName.Index, new Dictionary<int, PosRot>() },
                { FingerName.Middle, new Dictionary<int, PosRot>() },
                { FingerName.Ring, new Dictionary<int, PosRot>() },
                { FingerName.Pinky, new Dictionary<int, PosRot>() }
            };
            Fingers[BodySide.Right] = new Dictionary<FingerName, IDictionary<int, PosRot>>()
            {
                { FingerName.Thumb, new Dictionary<int, PosRot>() },
                { FingerName.Index, new Dictionary<int, PosRot>() },
                { FingerName.Middle, new Dictionary<int, PosRot>() },
                { FingerName.Ring, new Dictionary<int, PosRot>() },
                { FingerName.Pinky, new Dictionary<int, PosRot>() }
            };
            
            foreach (var side in new[]{ BodySide.Left, BodySide.Right})
            {
                var a = side == BodySide.Left ? h.ArmL : h.ArmR;

                Fingers[side][FingerName.Thumb] = new Dictionary<int, PosRot>
                {
                    { 0, new PosRot(a.Thumb1.localPosition, a.Thumb1.localRotation) },
                    { 1, new PosRot(a.Thumb1.localPosition, a.Thumb1.localRotation) },
                    { 2, new PosRot(a.Thumb2.localPosition, a.Thumb2.localRotation) },
                    { 3, new PosRot(a.Thumb3.localPosition, a.Thumb3.localRotation) }
                };
                Fingers[side][FingerName.Index] = new Dictionary<int, PosRot>
                {
                    { 0, new PosRot(a.Index0.localPosition, a.Index0.localRotation) },
                    { 1, new PosRot(a.Index1.localPosition, a.Index1.localRotation) },
                    { 2, new PosRot(a.Index2.localPosition, a.Index2.localRotation) },
                    { 3, new PosRot(a.Index3.localPosition, a.Index3.localRotation) }
                };
                Fingers[side][FingerName.Middle] = new Dictionary<int, PosRot>
                {
                    { 0, new PosRot(a.Middle0.localPosition, a.Middle0.localRotation) },
                    { 1, new PosRot(a.Middle1.localPosition, a.Middle1.localRotation) },
                    { 2, new PosRot(a.Middle2.localPosition, a.Middle2.localRotation) },
                    { 3, new PosRot(a.Middle3.localPosition, a.Middle3.localRotation) }
                };
                Fingers[side][FingerName.Ring] = new Dictionary<int, PosRot>
                {
                    { 0, new PosRot(a.Ring0.localPosition, a.Ring0.localRotation) },
                    { 1, new PosRot(a.Ring1.localPosition, a.Ring1.localRotation) },
                    { 2, new PosRot(a.Ring2.localPosition, a.Ring2.localRotation) },
                    { 3, new PosRot(a.Ring3.localPosition, a.Ring3.localRotation) }
                };
                Fingers[side][FingerName.Pinky] = new Dictionary<int, PosRot>
                {
                    { 0, new PosRot(a.Pinky0.localPosition, a.Pinky0.localRotation) },
                    { 1, new PosRot(a.Pinky1.localPosition, a.Pinky1.localRotation) },
                    { 2, new PosRot(a.Pinky2.localPosition, a.Pinky2.localRotation) },
                    { 3, new PosRot(a.Pinky3.localPosition, a.Pinky3.localRotation) }
                };
            }
            
        }
        public readonly IDictionary<BodySide, IDictionary<FingerName, IDictionary<int, PosRot>>> Fingers = 
            new Dictionary<BodySide, IDictionary<FingerName, IDictionary<int, PosRot>>>();
        public readonly float PelvisToMouthDistance, FootHeight, PelvisHeight, HipHeight, ShoulderHeight, RelaxedArmHeight;
        public readonly Vector3 LowerNeckToMouthRelVec, LocPosHead, LocPosJaw, ModelPosArmL, ModelPosArmR, LocalPosArmL, LocalPosArmR, ModelVecHipToSpine;
        public readonly Quaternion LocRotHead, LocRotJaw, LocRotArmL, LocRotArmR;
    }
}
