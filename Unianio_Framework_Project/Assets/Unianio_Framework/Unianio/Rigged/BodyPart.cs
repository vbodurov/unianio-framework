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
    public enum BodyPart 
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
        NeckLower = 24,
        Neck = 25,
        ToesL = 26,
        ToesR = 27,
        BreastL = 28,
        BreastR = 29,
        FingerThumbL = 30,
        FingerIndexL = 31,
        FingerMiddleL = 32,
        FingerRingL = 33,
        FingerPinkyL = 34,
        FingerThumbR = 35,
        FingerIndexR = 36,
        FingerMiddleR = 37,
        FingerRingR = 38,
        FingerPinkyR = 39,
        FootL = 40,
        FootR = 41,
        Tongue = 42,
        Hip = 43,
        Face = 44,
        Beard = 45,
        CollarL = 46,
        CollarR = 47,
        Braid = 48,
    }
}