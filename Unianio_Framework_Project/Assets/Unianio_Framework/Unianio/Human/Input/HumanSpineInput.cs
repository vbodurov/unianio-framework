using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanSpineInput
    {
        public HumanSpineInput(
            Transform model, Transform hip,
            Transform abdomenLower, Transform abdomenUpper,
            Transform chestLower, Transform chestUpper,
            Transform neckLower, Transform neckUpper)
        {
            Model = model;
            Hip = hip;
            AbdomenLower = abdomenLower;
            AbdomenUpper = abdomenUpper;
            ChestLower = chestLower;
            ChestUpper = chestUpper;
            NeckLower = neckLower;
            NeckUpper = neckUpper;
        }
        internal readonly Transform Model, Hip, AbdomenLower, AbdomenUpper, ChestLower, ChestUpper, NeckLower, NeckUpper;
    }
}