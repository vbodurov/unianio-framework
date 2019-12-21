using System.Collections.Generic;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Rigged.IK
{
    public interface IPlayerArmBonesExtracter
    {
        Transform Root { get; }
        Transform LowerArm1 { get; }
        Transform LowerArm2 { get; }
        Transform Wrist { get; }
        Transform Index0 { get; }
        Transform Index1 { get; }
        Transform Index2 { get; }
        Transform Index3 { get; }
        Transform Middle0 { get; }
        Transform Middle1 { get; }
        Transform Middle2 { get; }
        Transform Middle3 { get; }
        Transform Pinky0 { get; }
        Transform Pinky1 { get; }
        Transform Pinky2 { get; }
        Transform Pinky3 { get; }
        Transform Ring0 { get; }
        Transform Ring1 { get; }
        Transform Ring2 { get; }
        Transform Ring3 { get; }
        Transform Thumb0 { get; }
        Transform Thumb1 { get; }
        Transform Thumb2 { get; }
    }
    public sealed class PlayerArmBonesExtracter : IPlayerArmBonesExtracter
    {
        readonly Transform _root;
        readonly Transform _lowerArm1;
        readonly Transform _lowerArm2;
        readonly Transform _hand;
        readonly Transform _index0;//M = metacarpal
        readonly Transform _index1;
        readonly Transform _index2;
        readonly Transform _index3;
        readonly Transform _middle0;//M = metacarpal
        readonly Transform _middle1;
        readonly Transform _middle2;
        readonly Transform _middle3;
        readonly Transform _pinky0;//M = metacarpal
        readonly Transform _pinky1;
        readonly Transform _pinky2;
        readonly Transform _pinky3;
        readonly Transform _ring0;//M = metacarpal
        readonly Transform _ring1;
        readonly Transform _ring2;
        readonly Transform _ring3;
        readonly Transform _thumb0;
        readonly Transform _thumb1;
        readonly Transform _thumb2;
        internal PlayerArmBonesExtracter(BodySide bodySide, IDictionary<string, Transform> bones)
        {
            var prefix = bodySide == BodySide.Left ? "l" : "r";
            _root = bones["Skeleton"];
            _lowerArm1 = bones[prefix + "ForearmBend"];
            _lowerArm2 = bones[prefix + "ForearmTwist"];
            _thumb0 = bones[prefix + "Thumb1"];
            _thumb1 = bones[prefix + "Thumb2"];
            _thumb2 = bones[prefix + "Thumb3"];
            _hand = bones[prefix + "Hand"];
            _index0 = bones[prefix + "Carpal1"];
            _index1 = bones[prefix + "Index1"];
            _index2 = bones[prefix + "Index2"];
            _index3 = bones[prefix + "Index3"];
            _middle0 = bones[prefix + "Carpal2"];
            _middle1 = bones[prefix + "Mid1"];
            _middle2 = bones[prefix + "Mid2"];
            _middle3 = bones[prefix + "Mid3"];
            _ring0 = bones[prefix + "Carpal3"];
            _ring1 = bones[prefix + "Ring1"];
            _ring2 = bones[prefix + "Ring2"];
            _ring3 = bones[prefix + "Ring3"];
            _pinky0 = bones[prefix + "Carpal4"];
            _pinky1 = bones[prefix + "Pinky1"];
            _pinky2 = bones[prefix + "Pinky2"];
            _pinky3 = bones[prefix + "Pinky3"];
        }
        Transform IPlayerArmBonesExtracter.Root => _root;
        Transform IPlayerArmBonesExtracter.LowerArm1 => _lowerArm1;
        Transform IPlayerArmBonesExtracter.LowerArm2 => _lowerArm2;
        Transform IPlayerArmBonesExtracter.Wrist => _hand;
        Transform IPlayerArmBonesExtracter.Index0 => _index0;
        Transform IPlayerArmBonesExtracter.Index1 => _index1;
        Transform IPlayerArmBonesExtracter.Index2 => _index2;
        Transform IPlayerArmBonesExtracter.Index3 => _index3;
        Transform IPlayerArmBonesExtracter.Middle0 => _middle0;
        Transform IPlayerArmBonesExtracter.Middle1 => _middle1;
        Transform IPlayerArmBonesExtracter.Middle2 => _middle2;
        Transform IPlayerArmBonesExtracter.Middle3 => _middle3;
        Transform IPlayerArmBonesExtracter.Pinky0 => _pinky0;
        Transform IPlayerArmBonesExtracter.Pinky1 => _pinky1;
        Transform IPlayerArmBonesExtracter.Pinky2 => _pinky2;
        Transform IPlayerArmBonesExtracter.Pinky3 => _pinky3;
        Transform IPlayerArmBonesExtracter.Ring0 => _ring0;
        Transform IPlayerArmBonesExtracter.Ring1 => _ring1;
        Transform IPlayerArmBonesExtracter.Ring2 => _ring2;
        Transform IPlayerArmBonesExtracter.Ring3 => _ring3;
        Transform IPlayerArmBonesExtracter.Thumb0 => _thumb0;
        Transform IPlayerArmBonesExtracter.Thumb1 => _thumb1;
        Transform IPlayerArmBonesExtracter.Thumb2 => _thumb2;
    }
}
