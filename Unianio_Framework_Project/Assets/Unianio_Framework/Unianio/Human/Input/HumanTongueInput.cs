using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanTongueInput
    {
        public readonly Transform Model, Tongue1, Tongue2, Tongue3, Tongue4;

        public HumanTongueInput(Transform model, Transform tongue1, Transform tongue2, Transform tongue3, Transform tongue4)
        {
            Model = model;
            Tongue1 = tongue1;
            Tongue2 = tongue2;
            Tongue3 = tongue3;
            Tongue4 = tongue4;
        }
    }
}