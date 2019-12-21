using Unianio.Rigged;
using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanBoneInput
    {
        public HumanBoneInput(HumanoidPart part, Transform model, Transform bone)
        {
            Part = part;
            Model = model;
            Bone = bone;
        }
        public readonly HumanoidPart Part;
        public readonly Transform Model;
        public readonly Transform Bone;
    }
}