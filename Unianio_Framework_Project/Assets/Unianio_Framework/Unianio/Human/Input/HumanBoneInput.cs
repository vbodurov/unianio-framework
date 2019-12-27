using Unianio.Rigged;
using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanBoneInput
    {
        public HumanBoneInput(BodyPart part, Transform model, Transform bone)
        {
            Part = part;
            Model = model;
            Bone = bone;
        }
        public readonly BodyPart Part;
        public readonly Transform Model;
        public readonly Transform Bone;
    }
}