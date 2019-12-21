using UnityEngine;

namespace Unianio.Genesis.IK
{
    public class GenFaceGroupInitialStats
    {
        public GenFaceGroupInitialStats(GenFaceGroup face)
        {
            LocRotEyelidUpperInnerL = face.EyelidUpperInnerL.localRotation;
            LocRotEyelidUpperL = face.EyelidUpperL.localRotation;
            LocRotEyelidUpperOuterL = face.EyelidUpperOuterL.localRotation;
            LocRotEyelidUpperInnerR = face.EyelidUpperInnerR.localRotation;
            LocRotEyelidUpperR = face.EyelidUpperR.localRotation;
            LocRotEyelidUpperOuterR = face.EyelidUpperOuterR.localRotation;
            LocRotEyelidLowerInnerL = face.EyelidLowerInnerL.localRotation;
            LocRotEyelidLowerL = face.EyelidLowerL.localRotation;
            LocRotEyelidLowerOuterL = face.EyelidLowerOuterL.localRotation;
            LocRotEyelidLowerInnerR = face.EyelidLowerInnerR.localRotation;
            LocRotEyelidLowerR = face.EyelidLowerR.localRotation;
            LocRotEyelidLowerOuterR = face.EyelidLowerOuterR.localRotation;
            LocRotLipCornerL = face.LipCornerL.localRotation;
            LocRotLipLowerOuterL = face.LipLowerOuterL.localRotation;
            LocRotLipLowerInnerL = face.LipLowerInnerL.localRotation;
            LocRotLipLowerMiddle = face.LipLowerMiddle.localRotation;
            LocRotLipLowerInnerR = face.LipLowerInnerR.localRotation;
            LocRotLipLowerOuterR = face.LipLowerOuterR.localRotation;
            LocRotLipCornerR = face.LipCornerR.localRotation;
            LocRotLipUpperOuterL = face.LipUpperOuterL.localRotation;
            LocRotLipUpperInnerL = face.LipUpperInnerL.localRotation;
            LocRotLipUpperMiddle = face.LipUpperMiddle.localRotation;
            LocRotLipUpperInnerR = face.LipUpperInnerR.localRotation;
            LocRotLipUpperOuterR = face.LipUpperOuterR.localRotation;
            LocRotEyeL = face.EyeL.localRotation;
            LocRotEyeR = face.EyeR.localRotation;
            LocRotMidNoseBridge = face.MidNoseBridge.localRotation;
            LocRotBrowOuterL = face.BrowOuterL.localRotation;
            LocRotBrowMidL = face.BrowMidL.localRotation;
            LocRotBrowInnerL = face.BrowInnerL.localRotation;
            LocRotCenterBrow = face.CenterBrow.localRotation;
            LocRotBrowInnerR = face.BrowInnerR.localRotation;
            LocRotBrowMidR = face.BrowMidR.localRotation;
            LocRotBrowOuterR = face.BrowOuterR.localRotation;
            LocRotCheekUpperL = face.CheekUpperL.localRotation;
            LocRotCheekLowerL = face.CheekLowerL.localRotation;
            LocRotCheekLowerR = face.CheekLowerR.localRotation;
            LocRotCheekUpperR = face.CheekUpperR.localRotation;
            LocRotNasolabialMouthCornerL = face.NasolabialMouthCornerL.localRotation;
            LocRotNasolabialMouthCornerR = face.NasolabialMouthCornerR.localRotation;

            LocRotEyelidInnerL = face.EyelidInnerL.localRotation;
            LocRotEyelidInnerR = face.EyelidInnerR.localRotation;
            LocRotEyelidOuterL = face.EyelidOuterL.localRotation;
            LocRotEyelidOuterR = face.EyelidOuterR.localRotation;
            LocRotLipBelowNoseL = face.LipBelowNoseL.localRotation;
            LocRotLipBelowNoseR = face.LipBelowNoseR.localRotation;
            LocRotLipNasolabialCreaseL = face.LipNasolabialCreaseL.localRotation;
            LocRotLipNasolabialCreaseR = face.LipNasolabialCreaseR.localRotation;
            LocRotNasolabialMiddleL = face.NasolabialMiddleL.localRotation;
            LocRotNasolabialMiddleR = face.NasolabialMiddleR.localRotation;
            LocRotNasolabialUpperL = face.NasolabialUpperL.localRotation;
            LocRotNasolabialUpperR = face.NasolabialUpperR.localRotation;
            LocRotNostrilL = face.NostrilL.localRotation;
            LocRotNostrilR = face.NostrilR.localRotation;
            LocRotSquintInnerL = face.SquintInnerL.localRotation;
            LocRotSquintInnerR = face.SquintInnerR.localRotation;
            LocRotSquintOuterL = face.SquintOuterL.localRotation;
            LocRotSquintOuterR = face.SquintOuterR.localRotation;
            LocRotJawClenchL = face.JawClenchL.localRotation;
            LocRotJawClenchR = face.JawClenchR.localRotation;
            LocRotNasolabialLowerL = face.NasolabialLowerL.localRotation;
            LocRotNasolabialLowerR = face.NasolabialLowerR.localRotation;
            LocRotLowerJaw = face.LowerJaw.localRotation;


            LocPosEyelidUpperInnerL = face.EyelidUpperInnerL.localPosition;
            LocPosEyelidUpperL = face.EyelidUpperL.localPosition;
            LocPosEyelidUpperOuterL = face.EyelidUpperOuterL.localPosition;
            LocPosEyelidUpperInnerR = face.EyelidUpperInnerR.localPosition;
            LocPosEyelidUpperR = face.EyelidUpperR.localPosition;
            LocPosEyelidUpperOuterR = face.EyelidUpperOuterR.localPosition;
            LocPosEyelidLowerInnerL = face.EyelidLowerInnerL.localPosition;
            LocPosEyelidLowerL = face.EyelidLowerL.localPosition;
            LocPosEyelidLowerOuterL = face.EyelidLowerOuterL.localPosition;
            LocPosEyelidLowerInnerR = face.EyelidLowerInnerR.localPosition;
            LocPosEyelidLowerR = face.EyelidLowerR.localPosition;
            LocPosEyelidLowerOuterR = face.EyelidLowerOuterR.localPosition;
            LocPosLipCornerL = face.LipCornerL.localPosition;
            LocPosLipLowerOuterL = face.LipLowerOuterL.localPosition;
            LocPosLipLowerInnerL = face.LipLowerInnerL.localPosition;
            LocPosLipLowerMiddle = face.LipLowerMiddle.localPosition;
            LocPosLipLowerInnerR = face.LipLowerInnerR.localPosition;
            LocPosLipLowerOuterR = face.LipLowerOuterR.localPosition;
            LocPosLipCornerR = face.LipCornerR.localPosition;
            LocPosLipUpperOuterL = face.LipUpperOuterL.localPosition;
            LocPosLipUpperInnerL = face.LipUpperInnerL.localPosition;
            LocPosLipUpperMiddle = face.LipUpperMiddle.localPosition;
            LocPosLipUpperInnerR = face.LipUpperInnerR.localPosition;
            LocPosLipUpperOuterR = face.LipUpperOuterR.localPosition;
            LocPosEyeL = face.EyeL.localPosition;
            LocPosEyeR = face.EyeR.localPosition;
            LocPosMidNoseBridge = face.MidNoseBridge.localPosition;
            LocPosBrowOuterL = face.BrowOuterL.localPosition;
            LocPosBrowMidL = face.BrowMidL.localPosition;
            LocPosBrowInnerL = face.BrowInnerL.localPosition;
            LocPosCenterBrow = face.CenterBrow.localPosition;
            LocPosBrowInnerR = face.BrowInnerR.localPosition;
            LocPosBrowMidR = face.BrowMidR.localPosition;
            LocPosBrowOuterR = face.BrowOuterR.localPosition;
            LocPosCheekUpperL = face.CheekUpperL.localPosition;
            LocPosCheekLowerL = face.CheekLowerL.localPosition;
            LocPosCheekLowerR = face.CheekLowerR.localPosition;
            LocPosCheekUpperR = face.CheekUpperR.localPosition;
            LocPosNasolabialMouthCornerL = face.NasolabialMouthCornerL.localPosition;
            LocPosNasolabialMouthCornerR = face.NasolabialMouthCornerR.localPosition;
            LocPosLowerJaw = face.LowerJaw.localPosition;

            LocPosEyelidInnerL = face.EyelidInnerL.localPosition;
            LocPosEyelidInnerR = face.EyelidInnerR.localPosition;
            LocPosEyelidOuterL = face.EyelidOuterL.localPosition;
            LocPosEyelidOuterR = face.EyelidOuterR.localPosition;
            LocPosLipBelowNoseL = face.LipBelowNoseL.localPosition;
            LocPosLipBelowNoseR = face.LipBelowNoseR.localPosition;
            LocPosLipNasolabialCreaseL = face.LipNasolabialCreaseL.localPosition;
            LocPosLipNasolabialCreaseR = face.LipNasolabialCreaseR.localPosition;
            LocPosNasolabialMiddleL = face.NasolabialMiddleL.localPosition;
            LocPosNasolabialMiddleR = face.NasolabialMiddleR.localPosition;
            LocPosNasolabialUpperL = face.NasolabialUpperL.localPosition;
            LocPosNasolabialUpperR = face.NasolabialUpperR.localPosition;
            LocPosNostrilL = face.NostrilL.localPosition;
            LocPosNostrilR = face.NostrilR.localPosition;
            LocPosSquintInnerL = face.SquintInnerL.localPosition;
            LocPosSquintInnerR = face.SquintInnerR.localPosition;
            LocPosSquintOuterL = face.SquintOuterL.localPosition;
            LocPosSquintOuterR = face.SquintOuterR.localPosition;
            LocPosJawClenchL = face.JawClenchL.localPosition;
            LocPosJawClenchR = face.JawClenchR.localPosition;
            LocPosNasolabialLowerL = face.NasolabialLowerL.localPosition;
            LocPosNasolabialLowerR = face.NasolabialLowerR.localPosition;



            ArrLocPosEyelidUpperInners = new[] { LocPosEyelidUpperInnerL, LocPosEyelidUpperInnerR };
            ArrLocPosEyelidUppers = new[] { LocPosEyelidUpperL, LocPosEyelidUpperR };
            ArrLocPosEyelidUpperOuters = new[] { LocPosEyelidUpperOuterL, LocPosEyelidUpperOuterR };
            ArrLocPosEyelidLowerInners = new[] { LocPosEyelidLowerInnerL, LocPosEyelidLowerInnerR };
            ArrLocPosEyelidLowers = new[] { LocPosEyelidLowerL, LocPosEyelidLowerR };
            ArrLocPosEyelidLowerOuters = new[] { LocPosEyelidLowerOuterL, LocPosEyelidLowerOuterR };
            ArrLocPosLipCorners = new[] { LocPosLipCornerL, LocPosLipCornerR };
            ArrLocPosLipLowerOuters = new[] { LocPosLipLowerOuterL, LocPosLipLowerOuterR };
            ArrLocPosLipLowerInners = new[] { LocPosLipLowerInnerL, LocPosLipLowerInnerR };
            ArrLocPosLipUpperOuters = new[] { LocPosLipUpperOuterL, LocPosLipUpperOuterR };
            ArrLocPosLipUpperInners = new[] { LocPosLipUpperInnerL, LocPosLipUpperInnerR };
            ArrLocPosEyes = new[] { LocPosEyeL, LocPosEyeR };
            ArrLocPosBrowOuters = new[] { LocPosBrowOuterL, LocPosBrowOuterR };
            ArrLocPosBrowMids = new[] { LocPosBrowMidL, LocPosBrowMidR };
            ArrLocPosBrowInners = new[] { LocPosBrowInnerL, LocPosBrowInnerR };
            ArrLocPosCheekUppers = new[] { LocPosCheekUpperL, LocPosCheekUpperR };
            ArrLocPosCheekLowers = new[] { LocPosCheekLowerL, LocPosCheekLowerR };
            ArrLocPosNasolabialMouthCorners = new[] { LocPosNasolabialMouthCornerL, LocPosNasolabialMouthCornerR };

            ArrLocPosEyelidInners = new[] { LocPosEyelidInnerL, LocPosEyelidInnerR };
            ArrLocPosEyelidOuters = new[] { LocPosEyelidOuterL, LocPosEyelidOuterR };
            ArrLocPosLipBelowNoses = new[] { LocPosLipBelowNoseL, LocPosLipBelowNoseR };
            ArrLocPosLipNasolabialCreases = new[] { LocPosLipNasolabialCreaseL, LocPosLipNasolabialCreaseR };
            ArrLocPosNasolabialMiddles = new[] { LocPosNasolabialMiddleL, LocPosNasolabialMiddleR };
            ArrLocPosNasolabialUppers = new[] { LocPosNasolabialUpperL, LocPosNasolabialUpperR };
            ArrLocPosNostrils = new[] { LocPosNostrilL, LocPosNostrilR };
            ArrLocPosSquintInners = new[] { LocPosSquintInnerL, LocPosSquintInnerR };
            ArrLocPosSquintOuters = new[] { LocPosSquintOuterL, LocPosSquintOuterR };
            ArrLocPosJawClenches = new[] { LocPosJawClenchL, LocPosJawClenchR };
            ArrLocPosNasolabialLowers = new[] { LocPosNasolabialLowerL, LocPosNasolabialLowerR };

            ArrLocRotEyelidUpperInners = new[] { LocRotEyelidUpperInnerL, LocRotEyelidUpperInnerR };
            ArrLocRotEyelidUppers = new[] { LocRotEyelidUpperL, LocRotEyelidUpperR };
            ArrLocRotEyelidUpperOuters = new[] { LocRotEyelidUpperOuterL, LocRotEyelidUpperOuterR };
            ArrLocRotEyelidLowerInners = new[] { LocRotEyelidLowerInnerL, LocRotEyelidLowerInnerR };
            ArrLocRotEyelidLowers = new[] { LocRotEyelidLowerL, LocRotEyelidLowerR };
            ArrLocRotEyelidLowerOuters = new[] { LocRotEyelidLowerOuterL, LocRotEyelidLowerOuterR };
            ArrLocRotLipCorners = new[] { LocRotLipCornerL, LocRotLipCornerR };
            ArrLocRotLipLowerOuters = new[] { LocRotLipLowerOuterL, LocRotLipLowerOuterR };
            ArrLocRotLipLowerInners = new[] { LocRotLipLowerInnerL, LocRotLipLowerInnerR };
            ArrLocRotLipUpperOuters = new[] { LocRotLipUpperOuterL, LocRotLipUpperOuterR };
            ArrLocRotLipUpperInners = new[] { LocRotLipUpperInnerL, LocRotLipUpperInnerR };
            ArrLocRotEyes = new[] { LocRotEyeL, LocRotEyeR };
            ArrLocRotBrowOuters = new[] { LocRotBrowOuterL, LocRotBrowOuterR };
            ArrLocRotBrowMids = new[] { LocRotBrowMidL, LocRotBrowMidR };
            ArrLocRotBrowInners = new[] { LocRotBrowInnerL, LocRotBrowInnerR };
            ArrLocRotCheekUppers = new[] { LocRotCheekUpperL, LocRotCheekUpperR };
            ArrLocRotCheekLowers = new[] { LocRotCheekLowerL, LocRotCheekLowerR };
            ArrLocRotNasolabialMouthCorners = new[] { LocRotNasolabialMouthCornerL, LocRotNasolabialMouthCornerR };

            ArrLocRotEyelidInners = new[] { LocRotEyelidInnerL, LocRotEyelidInnerR };
            ArrLocRotEyelidOuters = new[] { LocRotEyelidOuterL, LocRotEyelidOuterR };
            ArrLocRotLipBelowNoses = new[] { LocRotLipBelowNoseL, LocRotLipBelowNoseR };
            ArrLocRotLipNasolabialCreases = new[] { LocRotLipNasolabialCreaseL, LocRotLipNasolabialCreaseR };
            ArrLocRotNasolabialMiddles = new[] { LocRotNasolabialMiddleL, LocRotNasolabialMiddleR };
            ArrLocRotNasolabialUppers = new[] { LocRotNasolabialUpperL, LocRotNasolabialUpperR };
            ArrLocRotNostrils = new[] { LocRotNostrilL, LocRotNostrilR };
            ArrLocRotSquintInners = new[] { LocRotSquintInnerL, LocRotSquintInnerR };
            ArrLocRotSquintOuters = new[] { LocRotSquintOuterL, LocRotSquintOuterR };
            ArrLocRotJawClenches = new[] { LocRotJawClenchL, LocRotJawClenchR };
            ArrLocRotNasolabialLowers = new[] { LocRotNasolabialLowerL, LocRotNasolabialLowerR };
        }

        public readonly Vector3 LocPosEyelidUpperInnerL, LocPosEyelidUpperL, LocPosEyelidUpperOuterL, 
            LocPosEyelidUpperInnerR, LocPosEyelidUpperR, LocPosEyelidUpperOuterR, LocPosEyelidLowerInnerL,
            LocPosEyelidLowerL, LocPosEyelidLowerOuterL, LocPosEyelidLowerInnerR, LocPosEyelidLowerR,
            LocPosEyelidLowerOuterR,LocPosLipCornerL,LocPosLipLowerOuterL,LocPosLipLowerInnerL,LocPosLipLowerMiddle,
            LocPosLipLowerInnerR, LocPosLipLowerOuterR, LocPosLipCornerR, LocPosLipUpperOuterL,LocPosLipUpperInnerL,
            LocPosLipUpperMiddle,LocPosLipUpperInnerR,LocPosLipUpperOuterR,LocPosEyeL,LocPosEyeR,
            LocPosMidNoseBridge,LocPosBrowOuterL,LocPosBrowMidL,LocPosBrowInnerL,LocPosCenterBrow,LocPosBrowInnerR,
            LocPosBrowMidR,LocPosBrowOuterR, LocPosCheekUpperL, LocPosCheekLowerL, LocPosCheekLowerR, LocPosCheekUpperR,
            LocPosNasolabialMouthCornerL, LocPosNasolabialMouthCornerR,
            LocPosEyelidInnerL, LocPosEyelidOuterL, LocPosEyelidInnerR, LocPosEyelidOuterR, LocPosLipBelowNoseL, 
            LocPosLipNasolabialCreaseL, LocPosNasolabialMiddleL, LocPosNasolabialUpperL, LocPosNostrilL, 
            LocPosSquintInnerL, LocPosSquintOuterL, LocPosJawClenchL, LocPosNasolabialLowerL, LocPosLipBelowNoseR, 
            LocPosLipNasolabialCreaseR, LocPosNasolabialMiddleR, LocPosNasolabialUpperR, LocPosNostrilR, 
            LocPosSquintInnerR, LocPosSquintOuterR, LocPosJawClenchR, LocPosNasolabialLowerR,
            LocPosLowerJaw;

        public readonly Vector3[] ArrLocPosEyelidUpperInners, ArrLocPosEyelidUppers, ArrLocPosEyelidUpperOuters,
            ArrLocPosEyelidLowerInners,ArrLocPosEyelidLowers, ArrLocPosEyelidLowerOuters, 
            ArrLocPosLipCorners, ArrLocPosLipLowerOuters, ArrLocPosLipLowerInners, 
            ArrLocPosLipUpperOuters, ArrLocPosLipUpperInners, ArrLocPosEyes, 
            ArrLocPosBrowOuters, ArrLocPosBrowMids, ArrLocPosBrowInners, 
            ArrLocPosCheekUppers, ArrLocPosCheekLowers, ArrLocPosNasolabialMouthCorners,
            ArrLocPosEyelidInners, ArrLocPosEyelidOuters, ArrLocPosLipBelowNoses,
            ArrLocPosLipNasolabialCreases, ArrLocPosNasolabialMiddles, ArrLocPosNasolabialUppers,
            ArrLocPosNostrils, ArrLocPosSquintInners, ArrLocPosSquintOuters, ArrLocPosJawClenches,
            ArrLocPosNasolabialLowers;

        public Quaternion LocRotEyelidUpperInnerL, LocRotEyelidUpperL, LocRotEyelidUpperOuterL, 
            LocRotEyelidUpperInnerR, LocRotEyelidUpperR, LocRotEyelidUpperOuterR, LocRotEyelidLowerInnerL, 
            LocRotEyelidLowerL, LocRotEyelidLowerOuterL, LocRotEyelidLowerInnerR, LocRotEyelidLowerR, 
            LocRotEyelidLowerOuterR, LocRotLipCornerL, LocRotLipLowerOuterL, LocRotLipLowerInnerL, 
            LocRotLipLowerMiddle, LocRotLipLowerInnerR, LocRotLipLowerOuterR, LocRotLipCornerR, 
            LocRotLipUpperOuterL, LocRotLipUpperInnerL, LocRotLipUpperMiddle, LocRotLipUpperInnerR, 
            LocRotLipUpperOuterR, LocRotEyeL, LocRotEyeR, LocRotMidNoseBridge, LocRotBrowOuterL, 
            LocRotBrowMidL, LocRotBrowInnerL, LocRotCenterBrow, LocRotBrowInnerR, LocRotBrowMidR, 
            LocRotBrowOuterR, LocRotCheekUpperL, LocRotCheekLowerL, LocRotCheekLowerR, LocRotCheekUpperR,
            LocRotNasolabialMouthCornerL, LocRotNasolabialMouthCornerR,
            LocRotEyelidInnerL,LocRotEyelidOuterL,LocRotEyelidInnerR,LocRotEyelidOuterR,LocRotLipBelowNoseL,
            LocRotLipNasolabialCreaseL,LocRotNasolabialMiddleL,LocRotNasolabialUpperL,LocRotNostrilL,
            LocRotSquintInnerL,LocRotSquintOuterL,LocRotJawClenchL,LocRotNasolabialLowerL,LocRotLipBelowNoseR,
            LocRotLipNasolabialCreaseR,LocRotNasolabialMiddleR,LocRotNasolabialUpperR,LocRotNostrilR,
            LocRotSquintInnerR,LocRotSquintOuterR,LocRotJawClenchR,LocRotNasolabialLowerR,
            LocRotLowerJaw;

        public Quaternion[] ArrLocRotEyelidUpperInners, ArrLocRotEyelidUppers, ArrLocRotEyelidUpperOuters,
            ArrLocRotEyelidLowerInners, ArrLocRotEyelidLowers, ArrLocRotEyelidLowerOuters,
            ArrLocRotLipCorners, ArrLocRotLipLowerOuters, ArrLocRotLipLowerInners,
            ArrLocRotLipUpperOuters, ArrLocRotLipUpperInners,ArrLocRotEyes, ArrLocRotBrowOuters,
            ArrLocRotBrowMids, ArrLocRotBrowInners,ArrLocRotCheekUppers, ArrLocRotCheekLowers,
            ArrLocRotNasolabialMouthCorners,
            ArrLocRotEyelidInners,ArrLocRotEyelidOuters,ArrLocRotLipBelowNoses,ArrLocRotLipNasolabialCreases,
            ArrLocRotNasolabialMiddles,ArrLocRotNasolabialUppers,ArrLocRotNostrils,
            ArrLocRotSquintInners,ArrLocRotSquintOuters,ArrLocRotJawClenches,ArrLocRotNasolabialLowers;


    }
}
