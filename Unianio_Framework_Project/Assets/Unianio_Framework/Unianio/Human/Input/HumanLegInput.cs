using Unianio.Rigged;
using UnityEngine;

namespace Unianio.Human.Input
{
    public class HumanLegInput
    {
        public HumanLegInput(BodyPart part, Transform model, Transform thighBend, Transform thighTwist,
            Transform shin, Transform foot, Transform footHolder, Transform metatarsals, Transform toe, Transform toeHolder, Transform bigToe, Transform bigToe2,
            Transform smallToe11, Transform smallToe12, Transform smallToe21, Transform smallToe22, Transform smallToe31,
            Transform smallToe32, Transform smallToe41, Transform smallToe42)
        {
            Part = part;
            Model = model;
            ThighBend = thighBend;
            ThighTwist = thighTwist;
            Shin = shin;
            Foot = foot;
            FootHolder = footHolder;
            Metatarsals = metatarsals;
            Toe = toe;
            ToeHolder = toeHolder;
            BigToe = bigToe;
            BigToe2 = bigToe2;
            SmallToe11 = smallToe11;
            SmallToe12 = smallToe12;
            SmallToe21 = smallToe21;
            SmallToe22 = smallToe22;
            SmallToe31 = smallToe31;
            SmallToe32 = smallToe32;
            SmallToe41 = smallToe41;
            SmallToe42 = smallToe42;
        }
        public readonly BodyPart Part;
        public readonly Transform Model, ThighBend, ThighTwist, Shin, Foot, FootHolder,
            Metatarsals, Toe, ToeHolder, BigToe, BigToe2, SmallToe11, SmallToe12, SmallToe21,
            SmallToe22, SmallToe31, SmallToe32, SmallToe41, SmallToe42;
    }
}