using Unianio.Rigged;
using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanArmInput
    {
        public HumanArmInput(BodyPart part, Transform model, Transform armRoot,
            Transform collar, Transform shoulder, Transform shoulder_2, 
            Transform shoulderTwist, Transform forearm, 
            Transform forearmTwist, Transform hand,
            Transform index0, Transform index1, Transform index2, Transform index3,
            Transform middle0, Transform middle1, Transform middle2, Transform middle3,
            Transform ring0, Transform ring1, Transform ring2, Transform ring3,
            Transform pinky0, Transform pinky1, Transform pinky2, Transform pinky3,
            Transform thumb1, Transform thumb2, Transform thumb3)
        {
            Part = part;
            Model = model;
            ArmRoot = armRoot;
            Collar = collar;
            Shoulder = shoulder;
            Shoulder2 = shoulder_2;
            ShoulderTwist = shoulderTwist;
            Forearm = forearm;
            ForearmTwist = forearmTwist;
            Hand = hand;
            Index0 = index0;
            Index1 = index1;
            Index2 = index2;
            Index3 = index3;
            Middle0 = middle0;
            Middle1 = middle1;
            Middle2 = middle2;
            Middle3 = middle3;
            Ring0 = ring0;
            Ring1 = ring1;
            Ring2 = ring2;
            Ring3 = ring3;
            Pinky0 = pinky0;
            Pinky1 = pinky1;
            Pinky2 = pinky2;
            Pinky3 = pinky3;
            Thumb1 = thumb1;
            Thumb2 = thumb2;
            Thumb3 = thumb3;
        }
        public readonly BodyPart Part;
        public readonly Transform Model, ArmRoot, Collar, Shoulder, Shoulder2, 
            ShoulderTwist, Forearm, ForearmTwist, Hand,
            Index0, Index1, Index2, Index3,
            Middle0, Middle1, Middle2, Middle3,
            Ring0, Ring1, Ring2, Ring3,
            Pinky0, Pinky1, Pinky2, Pinky3,
            Thumb1, Thumb2, Thumb3;
    }
}