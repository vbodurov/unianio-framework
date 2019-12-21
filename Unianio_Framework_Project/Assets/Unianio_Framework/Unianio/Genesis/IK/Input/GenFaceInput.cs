using UnityEngine;

namespace Unianio.Genesis.IK.Input
{
    public class GenFaceInput
    {
        public GenFaceInput(Transform eyeL, Transform eyeR, Transform earL, Transform earR, 
            Transform lowerJaw, Transform lowerFaceRig, Transform lowerTeeth, Transform upperTeeth, Transform upperFaceRig, 
            Transform belowJaw, Transform chin, Transform cheekLowerL, Transform lipBelow, Transform lipLowerMiddle, 
            Transform jawClenchL, Transform lipCornerL, Transform lipLowerInnerL, Transform lipLowerOuterL, 
            Transform nasolabialLowerL, Transform nasolabialMouthCornerL, 
            Transform cheekLowerR, Transform jawClenchR, Transform lipCornerR, Transform lipLowerInnerR, 
            Transform lipLowerOuterR, Transform nasolabialLowerR, Transform nasolabialMouthCornerR, 
            Transform centerBrow, 
            Transform browInnerL, Transform browMidL, Transform browOuterL, Transform cheekUpperL, 
            Transform eyelidInnerL, Transform eyelidLowerL, Transform eyelidLowerInnerL, Transform eyelidLowerOuterL, 
            Transform eyelidOuterL, Transform eyelidUpperL, Transform eyelidUpperInnerL, Transform eyelidUpperOuterL, 
            Transform lipUpperMiddle, Transform lipBelowNoseL, Transform lipNasolabialCreaseL, Transform lipUpperInnerL, 
            Transform lipUpperOuterL, Transform nasolabialMiddleL, Transform nasolabialUpperL, Transform nostrilL, 
            Transform squintInnerL, Transform squintOuterL, Transform midNoseBridge, Transform nose, Transform browInnerR, 
            Transform browMidR, Transform browOuterR, Transform cheekUpperR, Transform eyelidInnerR, Transform eyelidLowerR, 
            Transform eyelidLowerInnerR, Transform eyelidLowerOuterR, Transform eyelidOuterR, Transform eyelidUpperR, 
            Transform eyelidUpperInnerR, Transform eyelidUpperOuterR, Transform lipBelowNoseR, Transform lipNasolabialCreaseR, 
            Transform lipUpperInnerR, Transform lipUpperOuterR, Transform nasolabialMiddleR, Transform nasolabialUpperR, 
            Transform nostrilR, Transform squintInnerR, Transform squintOuterR)
        {
            EyeL = eyeL;
            EyeR = eyeR;
            EarL = earL;
            EarR = earR;
            LowerJaw = lowerJaw;
            LowerFaceRig = lowerFaceRig;
            LowerTeeth = lowerTeeth;
            UpperTeeth = upperTeeth;
            UpperFaceRig = upperFaceRig;
            BelowJaw = belowJaw;
            Chin = chin;
            CheekLowerL = cheekLowerL;
            LipBelow = lipBelow;
            LipLowerMiddle = lipLowerMiddle;
            JawClenchL = jawClenchL;
            LipCornerL = lipCornerL;
            LipLowerInnerL = lipLowerInnerL;
            LipLowerOuterL = lipLowerOuterL;
            NasolabialLowerL = nasolabialLowerL;
            NasolabialMouthCornerL = nasolabialMouthCornerL;
            CheekLowerR = cheekLowerR;
            JawClenchR = jawClenchR;
            LipCornerR = lipCornerR;
            LipLowerInnerR = lipLowerInnerR;
            LipLowerOuterR = lipLowerOuterR;
            NasolabialLowerR = nasolabialLowerR;
            NasolabialMouthCornerR = nasolabialMouthCornerR;
            CenterBrow = centerBrow;
            BrowInnerL = browInnerL;
            BrowMidL = browMidL;
            BrowOuterL = browOuterL;
            CheekUpperL = cheekUpperL;
            EyelidInnerL = eyelidInnerL;
            EyelidLowerL = eyelidLowerL;
            EyelidLowerInnerL = eyelidLowerInnerL;
            EyelidLowerOuterL = eyelidLowerOuterL;
            EyelidOuterL = eyelidOuterL;
            EyelidUpperL = eyelidUpperL;
            EyelidUpperInnerL = eyelidUpperInnerL;
            EyelidUpperOuterL = eyelidUpperOuterL;
            LipUpperMiddle = lipUpperMiddle;
            LipBelowNoseL = lipBelowNoseL;
            LipNasolabialCreaseL = lipNasolabialCreaseL;
            LipUpperInnerL = lipUpperInnerL;
            LipUpperOuterL = lipUpperOuterL;
            NasolabialMiddleL = nasolabialMiddleL;
            NasolabialUpperL = nasolabialUpperL;
            NostrilL = nostrilL;
            SquintInnerL = squintInnerL;
            SquintOuterL = squintOuterL;
            MidNoseBridge = midNoseBridge;
            Nose = nose;
            BrowInnerR = browInnerR;
            BrowMidR = browMidR;
            BrowOuterR = browOuterR;
            CheekUpperR = cheekUpperR;
            EyelidInnerR = eyelidInnerR;
            EyelidLowerR = eyelidLowerR;
            EyelidLowerInnerR = eyelidLowerInnerR;
            EyelidLowerOuterR = eyelidLowerOuterR;
            EyelidOuterR = eyelidOuterR;
            EyelidUpperR = eyelidUpperR;
            EyelidUpperInnerR = eyelidUpperInnerR;
            EyelidUpperOuterR = eyelidUpperOuterR;
            LipBelowNoseR = lipBelowNoseR;
            LipNasolabialCreaseR = lipNasolabialCreaseR;
            LipUpperInnerR = lipUpperInnerR;
            LipUpperOuterR = lipUpperOuterR;
            NasolabialMiddleR = nasolabialMiddleR;
            NasolabialUpperR = nasolabialUpperR;
            NostrilR = nostrilR;
            SquintInnerR = squintInnerR;
            SquintOuterR = squintOuterR;

        }
        public readonly Transform EyeL, EyeR, EarL, EarR, LowerJaw, LowerFaceRig, LowerTeeth, 
            UpperTeeth, UpperFaceRig, BelowJaw, Chin, CheekLowerL, LipBelow, LipLowerMiddle, 
            JawClenchL, LipCornerL, LipLowerInnerL, LipLowerOuterL, NasolabialLowerL, 
            NasolabialMouthCornerL, CheekLowerR, JawClenchR, LipCornerR, LipLowerInnerR, 
            LipLowerOuterR, NasolabialLowerR, NasolabialMouthCornerR, CenterBrow, 
            BrowInnerL, BrowMidL, BrowOuterL, CheekUpperL, EyelidInnerL, EyelidLowerL, 
            EyelidLowerInnerL, EyelidLowerOuterL, EyelidOuterL, EyelidUpperL, 
            EyelidUpperInnerL, EyelidUpperOuterL, LipUpperMiddle, LipBelowNoseL, 
            LipNasolabialCreaseL, LipUpperInnerL, LipUpperOuterL, NasolabialMiddleL, 
            NasolabialUpperL, NostrilL, SquintInnerL, SquintOuterL, MidNoseBridge, 
            Nose, BrowInnerR, BrowMidR, BrowOuterR, CheekUpperR, EyelidInnerR, EyelidLowerR, 
            EyelidLowerInnerR, EyelidLowerOuterR, EyelidOuterR, EyelidUpperR, EyelidUpperInnerR, 
            EyelidUpperOuterR, LipBelowNoseR, LipNasolabialCreaseR, LipUpperInnerR, LipUpperOuterR, 
            NasolabialMiddleR, NasolabialUpperR, NostrilR, SquintInnerR, SquintOuterR;
    }
}