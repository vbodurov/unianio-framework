using System;

namespace Unianio.Rigged
{
    /*
        http://bodurov.com/ed/

        EachLine((line, i, s)=>{
            var a = line.trim().split('=');    
            if(a.length < 1) return long;
            return "        "+a[0].trim()+" = " + i+",";
        },{i:0});
     */
    public enum HumanoidPart 
    {
        Nothing = 0,
        EntireBody = 1,
        ArmL = 2,
        ArmR = 3,
        Pelvis = 4,
        LegL = 5,
        LegR = 6,
        Spine = 7,
        Head = 8,
        EyeL = 9,
        EyeR = 10,
        BothEyelids = 11,
        EyelidL = 12,
        EyelidR = 13,
        EyebrowsCenter = 14,
        EyebrowsSide = 15,
        LipL = 16,
        LipR = 17,
        Jaw = 18,
        HandL = 19,
        HandR = 20,
        ShoulderL = 21,
        ShoulderR = 22,
        Torso = 23,
        Neck = 24,
        ToesL = 25,
        ToesR = 26,
        BreastL = 27,
        BreastR = 28,
        FingerThumbL = 29,
        FingerIndexL = 30,
        FingerMiddleL = 31,
        FingerRingL = 32,
        FingerPinkyL = 33,
        FingerThumbR = 34,
        FingerIndexR = 35,
        FingerMiddleR = 36,
        FingerRingR = 37,
        FingerPinkyR = 38,
        FootL = 39,
        FootR = 40,
        Tongue = 41,
        Hip = 42,
        Face = 43,
        Beard = 44,
        CollarL = 45,
        CollarR = 46,
        Braid = 47,
    }
}