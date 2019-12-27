using Unianio.Enums;
using Unianio.Rigged.IK;
using UnityEngine;

namespace Unianio.IK
{
    public interface IHumArmChain : IInitialOrientationHolder, IUpdatable, IManipulator3D
    {
        HumBoneHandler HandHandler { get; }
        bool AutoPositionHand { get; set; }
        Transform Model { get; }
        Transform ArmRoot { get; }
        Transform Collar { get; }
        Transform Shoulder { get; }
        Transform Shoulder2 { get; }
        Transform ShoulderTwist { get; }
        Transform Forearm { get; }
        Transform ForearmTwist { get; }
        Transform Hand { get; }
        Transform Index0 { get; }
        Transform Index1 { get; }
        Transform Index2 { get; }
        Transform Index3 { get; }
        Transform Middle0 { get; }
        Transform Middle1 { get; }
        Transform Middle2 { get; }
        Transform Middle3 { get; }
        Transform Ring0 { get; }
        Transform Ring1 { get; }
        Transform Ring2 { get; }
        Transform Ring3 { get; }
        Transform Pinky0 { get; }
        Transform Pinky1 { get; }
        Transform Pinky2 { get; }
        Transform Pinky3 { get; }
        Transform Thumb1 { get; }
        Transform Thumb2 { get; }
        Transform Thumb3 { get; }
        BodySide Side { get; }
        Vector3 SideDir { get; }
        float MaxStretch { get; }
        void CalculateArmBend(out Vector3 midPoint, out float lengthToElbow);
    }

}