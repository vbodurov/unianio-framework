using System;
using System.Collections.Generic;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis.IK.Input;
using Unianio.Human;
using Unianio.Human.Input;
using Unianio.MakeHuman;
using Unianio.Rigged;
using UnityEngine;

namespace Unianio.Genesis
{
    public class Genesis8Definition : IComplexHumanDefinition
    {
        readonly PersonDetails _person;
        readonly Transform _model;
        readonly GenFaceInput _face;
        readonly HumanArmInput _armL, _armR;
        readonly HumanLegInput _legL, _legR;
        readonly HumanSpineInput _spine;
        readonly HumanTongueInput _tongue;
        readonly HumanBoneInput _root, _hip, _pelvis, _upperNeck, _head, _breastL, _breastR;
        readonly float _height;
        readonly IDictionary<string, Transform> _bones;
        readonly bool _hasCompletedImport;

        public Genesis8Definition(PersonDetails personDetails, Transform model)
        {
            _person = personDetails;
            _model = model;

            var d = _model.PlaceChildrenInDictionary();

            // key bones:
            var root = d.GetFirst(genBone.genesis8roots);
            var hip = d[genBone.hip];
            var pelvis = d[genBone.pelvis];
            var head = d[genBone.head];

            _height = head.position.y - _model.position.y + 0.2f;
            // spine:
            var abdomenLower = d[genBone.abdomenLower];
            var abdomenUpper = d[genBone.abdomenUpper];
            var chestLower = d[genBone.chestLower];
            var chestUpper = d[genBone.chestUpper];
            var neckLower = d[genBone.neckLower];
            var neckUpper = d[genBone.neckUpper];


            // tongue:
            var tongue01 = d[genBone.tongue01];
            var tongue02 = d[genBone.tongue02];
            var tongue03 = d[genBone.tongue03];
            var tongue04 = d[genBone.tongue04];

            // arm left:
            var collarL = d[genBone.lCollar];
            var shoulderL = d[genBone.lShldrBend];
            var shoulderTwistL = d[genBone.lShldrTwist];
            var forearmL = d[genBone.lForearmBend];
            var forearmTwistL = d[genBone.lForearmTwist];
            var handL = d[genBone.lHand];
            var index0L = d[genBone.lCarpal1];
            var index1L = d[genBone.lIndex1];
            var index2L = d[genBone.lIndex2];
            var index3L = d[genBone.lIndex3];
            var middle0L = d[genBone.lCarpal2];
            var middle1L = d[genBone.lMid1];
            var middle2L = d[genBone.lMid2];
            var middle3L = d[genBone.lMid3];
            var ring0L = d[genBone.lCarpal3];
            var ring1L = d[genBone.lRing1];
            var ring2L = d[genBone.lRing2];
            var ring3L = d[genBone.lRing3];
            var pinky0L = d[genBone.lCarpal4];
            var pinky1L = d[genBone.lPinky1];
            var pinky2L = d[genBone.lPinky2];
            var pinky3L = d[genBone.lPinky3];
            var thumb1L = d[genBone.lThumb1];
            var thumb2L = d[genBone.lThumb2];
            var thumb3L = d[genBone.lThumb3];

            // arm right:
            var collarR = d[genBone.rCollar];
            var shoulderR = d[genBone.rShldrBend];
            var shoulderTwistR = d[genBone.rShldrTwist];
            var forearmR = d[genBone.rForearmBend];
            var forearmTwistR = d[genBone.rForearmTwist];
            var handR = d[genBone.rHand];
            var index0R = d[genBone.rCarpal1];
            var index1R = d[genBone.rIndex1];
            var index2R = d[genBone.rIndex2];
            var index3R = d[genBone.rIndex3];
            var middle0R = d[genBone.rCarpal2];
            var middle1R = d[genBone.rMid1];
            var middle2R = d[genBone.rMid2];
            var middle3R = d[genBone.rMid3];
            var ring0R = d[genBone.rCarpal3];
            var ring1R = d[genBone.rRing1];
            var ring2R = d[genBone.rRing2];
            var ring3R = d[genBone.rRing3];
            var pinky0R = d[genBone.rCarpal4];
            var pinky1R = d[genBone.rPinky1];
            var pinky2R = d[genBone.rPinky2];
            var pinky3R = d[genBone.rPinky3];
            var thumb1R = d[genBone.rThumb1];
            var thumb2R = d[genBone.rThumb2];
            var thumb3R = d[genBone.rThumb3];

            // face:
            var eyeL = d[genBone.lEye];
            var eyeR = d[genBone.rEye];
            var earL = d[genBone.lEar];
            var earR = d[genBone.rEar];
            var lowerJaw = d[genBone.lowerJaw];
            var lowerFaceRig = d[genBone.lowerFaceRig];
            var lowerTeeth = d[genBone.lowerTeeth];
            var upperTeeth = d[genBone.upperTeeth];
            var upperFaceRig = d[genBone.upperFaceRig];
            var belowJaw = d[genBone.BelowJaw];
            var chin = d[genBone.Chin];
            var cheekLowerL = d[genBone.lCheekLower];
            var lipBelow = d[genBone.LipBelow];
            var lipLowerMiddle = d[genBone.LipLowerMiddle];
            var jawClenchL = d[genBone.lJawClench];
            var lipCornerL = d[genBone.lLipCorner];
            var lipLowerInnerL = d[genBone.lLipLowerInner];
            var lipLowerOuterL = d[genBone.lLipLowerOuter];
            var nasolabialLowerL = d[genBone.lNasolabialLower];
            var nasolabialMouthCornerL = d[genBone.lNasolabialMouthCorner];
            var cheekLowerR = d[genBone.rCheekLower];
            var jawClenchR = d[genBone.rJawClench];
            var lipCornerR = d[genBone.rLipCorner];
            var lipLowerInnerR = d[genBone.rLipLowerInner];
            var lipLowerOuterR = d[genBone.rLipLowerOuter];
            var nasolabialLowerR = d[genBone.rNasolabialLower];
            var nasolabialMouthCornerR = d[genBone.rNasolabialMouthCorner];
            var centerBrow = d[genBone.CenterBrow];
            var browInnerL = d[genBone.lBrowInner];
            var browMidL = d[genBone.lBrowMid];
            var browOuterL = d[genBone.lBrowOuter];
            var cheekUpperL = d[genBone.lCheekUpper];
            var eyelidInnerL = d[genBone.lEyelidInner];
            var eyelidLowerL = d[genBone.lEyelidLower];
            var eyelidLowerInnerL = d[genBone.lEyelidLowerInner];
            var eyelidLowerOuterL = d[genBone.lEyelidLowerOuter];
            var eyelidOuterL = d[genBone.lEyelidOuter];
            var eyelidUpperL = d[genBone.lEyelidUpper];
            var eyelidUpperInnerL = d[genBone.lEyelidUpperInner];
            var eyelidUpperOuterL = d[genBone.lEyelidUpperOuter];
            var lipUpperMiddle = d[genBone.LipUpperMiddle];
            var lipBelowNoseL = d[genBone.lLipBelowNose];
            var lipNasolabialCreaseL = d[genBone.lLipNasolabialCrease];
            var lipUpperInnerL = d[genBone.lLipUpperInner];
            var lipUpperOuterL = d[genBone.lLipUpperOuter];
            var nasolabialMiddleL = d[genBone.lNasolabialMiddle];
            var nasolabialUpperL = d[genBone.lNasolabialUpper];
            var nostrilL = d[genBone.lNostril];
            var squintInnerL = d[genBone.lSquintInner];
            var squintOuterL = d[genBone.lSquintOuter];
            var midNoseBridge = d[genBone.MidNoseBridge];
            var nose = d[genBone.Nose];
            var browInnerR = d[genBone.rBrowInner];
            var browMidR = d[genBone.rBrowMid];
            var browOuterR = d[genBone.rBrowOuter];
            var cheekUpperR = d[genBone.rCheekUpper];
            var eyelidInnerR = d[genBone.rEyelidInner];
            var eyelidLowerR = d[genBone.rEyelidLower];
            var eyelidLowerInnerR = d[genBone.rEyelidLowerInner];
            var eyelidLowerOuterR = d[genBone.rEyelidLowerOuter];
            var eyelidOuterR = d[genBone.rEyelidOuter];
            var eyelidUpperR = d[genBone.rEyelidUpper];
            var eyelidUpperInnerR = d[genBone.rEyelidUpperInner];
            var eyelidUpperOuterR = d[genBone.rEyelidUpperOuter];
            var lipBelowNoseR = d[genBone.rLipBelowNose];
            var lipNasolabialCreaseR = d[genBone.rLipNasolabialCrease];
            var lipUpperInnerR = d[genBone.rLipUpperInner];
            var lipUpperOuterR = d[genBone.rLipUpperOuter];
            var nasolabialMiddleR = d[genBone.rNasolabialMiddle];
            var nasolabialUpperR = d[genBone.rNasolabialUpper];
            var nostrilR = d[genBone.rNostril];
            var squintInnerR = d[genBone.rSquintInner];
            var squintOuterR = d[genBone.rSquintOuter];
            var breastL = d[genBone.lPectoral];
            var breastR = d[genBone.rPectoral];

            // leg left
            var thighBendL = d[genBone.lThighBend];
            var thighTwistL = d[genBone.lThighTwist];
            var shinL = d[genBone.lShin];
            var footL = d[genBone.lFoot];
            var footHolderL = d.GetOrDefault(genBone.lFoot + "_Holder");
            var metatarsalsL = d[genBone.lMetatarsals];
            var toeL = d[genBone.lToe];
            var toeHolderL = d.GetOrDefault(genBone.lToe + "_Holder");
            var bigToeL = d[genBone.lBigToe];
            var bigToe2L = d[genBone.lBigToe_2];
            var smallToe11L = d[genBone.lSmallToe1];
            var smallToe12L = d[genBone.lSmallToe1_2];
            var smallToe21L = d[genBone.lSmallToe2];
            var smallToe22L = d[genBone.lSmallToe2_2];
            var smallToe31L = d[genBone.lSmallToe3];
            var smallToe32L = d[genBone.lSmallToe3_2];
            var smallToe41L = d[genBone.lSmallToe4];
            var smallToe42L = d[genBone.lSmallToe4_2];

            // leg right
            var thighBendR = d[genBone.rThighBend];
            var thighTwistR = d[genBone.rThighTwist];
            var shinR = d[genBone.rShin];
            var footR = d[genBone.rFoot];
            var footHolderR = d.GetOrDefault(genBone.rFoot + "_Holder");
            var metatarsalsR = d[genBone.rMetatarsals];
            var toeR = d[genBone.rToe];
            var toeHolderR = d.GetOrDefault(genBone.rToe + "_Holder");
            var bigToeR = d[genBone.rBigToe];
            var bigToe2R = d[genBone.rBigToe_2];
            var smallToe11R = d[genBone.rSmallToe1];
            var smallToe12R = d[genBone.rSmallToe1_2];
            var smallToe21R = d[genBone.rSmallToe2];
            var smallToe22R = d[genBone.rSmallToe2_2];
            var smallToe31R = d[genBone.rSmallToe3];
            var smallToe32R = d[genBone.rSmallToe3_2];
            var smallToe41R = d[genBone.rSmallToe4];
            var smallToe42R = d[genBone.rSmallToe4_2];

            _hasCompletedImport = footHolderL != null;
            
            _root = new HumanBoneInput(BodyPart.EntireBody, _model, root);
            _hip = new HumanBoneInput(BodyPart.Hip, _model, hip);
            _face = new GenFaceInput(eyeL: eyeL, eyeR: eyeR, earL: earL, earR: earR, lowerJaw: lowerJaw, 
                lowerFaceRig: lowerFaceRig, lowerTeeth: lowerTeeth, upperTeeth: upperTeeth, upperFaceRig: upperFaceRig, belowJaw: belowJaw, chin: chin,
                cheekLowerL: cheekLowerL, lipBelow: lipBelow, lipLowerMiddle: lipLowerMiddle, jawClenchL: jawClenchL, 
                lipCornerL: lipCornerL, lipLowerInnerL: lipLowerInnerL, lipLowerOuterL: lipLowerOuterL, 
                nasolabialLowerL: nasolabialLowerL, nasolabialMouthCornerL: nasolabialMouthCornerL, 
                cheekLowerR: cheekLowerR, jawClenchR: jawClenchR, lipCornerR: lipCornerR, 
                lipLowerInnerR: lipLowerInnerR, lipLowerOuterR: lipLowerOuterR, 
                nasolabialLowerR: nasolabialLowerR, nasolabialMouthCornerR: nasolabialMouthCornerR, 
                centerBrow: centerBrow, browInnerL: browInnerL, browMidL: browMidL, browOuterL: browOuterL, cheekUpperL: cheekUpperL, 
                eyelidInnerL: eyelidInnerL, eyelidLowerL: eyelidLowerL, eyelidLowerInnerL: eyelidLowerInnerL, 
                eyelidLowerOuterL: eyelidLowerOuterL, eyelidOuterL: eyelidOuterL, eyelidUpperL: eyelidUpperL, 
                eyelidUpperInnerL: eyelidUpperInnerL, eyelidUpperOuterL: eyelidUpperOuterL, lipUpperMiddle: lipUpperMiddle, 
                lipBelowNoseL: lipBelowNoseL, lipNasolabialCreaseL: lipNasolabialCreaseL, lipUpperInnerL: lipUpperInnerL, 
                lipUpperOuterL: lipUpperOuterL, nasolabialMiddleL: nasolabialMiddleL, nasolabialUpperL: nasolabialUpperL, 
                nostrilL: nostrilL, squintInnerL: squintInnerL, squintOuterL: squintOuterL, midNoseBridge: midNoseBridge, 
                nose: nose, browInnerR: browInnerR, browMidR: browMidR, browOuterR: browOuterR, cheekUpperR: cheekUpperR, 
                eyelidInnerR: eyelidInnerR, eyelidLowerR: eyelidLowerR, eyelidLowerInnerR: eyelidLowerInnerR, 
                eyelidLowerOuterR: eyelidLowerOuterR, eyelidOuterR: eyelidOuterR, eyelidUpperR: eyelidUpperR, 
                eyelidUpperInnerR: eyelidUpperInnerR, eyelidUpperOuterR: eyelidUpperOuterR, lipBelowNoseR: lipBelowNoseR, 
                lipNasolabialCreaseR: lipNasolabialCreaseR, lipUpperInnerR: lipUpperInnerR, lipUpperOuterR: lipUpperOuterR, 
                nasolabialMiddleR: nasolabialMiddleR, nasolabialUpperR: nasolabialUpperR, nostrilR: nostrilR, 
                squintInnerR: squintInnerR, squintOuterR: squintOuterR);
            _armL = new HumanArmInput(part: BodyPart.ArmL, model: _model, armRoot:null,
                collar: collarL, shoulder: shoulderL, shoulder_2: null, shoulderTwist: shoulderTwistL, forearm: forearmL, forearmTwist: forearmTwistL, hand: handL,
                index0: index0L, index1: index1L, index2: index2L, index3: index3L,
                middle0: middle0L, middle1: middle1L, middle2: middle2L, middle3: middle3L,
                ring0: ring0L, ring1: ring1L, ring2: ring2L, ring3: ring3L,
                pinky0: pinky0L, pinky1: pinky1L, pinky2: pinky2L, pinky3: pinky3L,
                thumb1: thumb1L, thumb2: thumb2L, thumb3: thumb3L);
            _armR = new HumanArmInput(part: BodyPart.ArmR, model: _model, armRoot: null,
                collar: collarR, shoulder: shoulderR, shoulder_2: null, shoulderTwist: shoulderTwistR, forearm: forearmR, forearmTwist: forearmTwistR, hand: handR,
                index0: index0R, index1: index1R, index2: index2R, index3: index3R,
                middle0: middle0R, middle1: middle1R, middle2: middle2R, middle3: middle3R,
                ring0: ring0R, ring1: ring1R, ring2: ring2R, ring3: ring3R,
                pinky0: pinky0R, pinky1: pinky1R, pinky2: pinky2R, pinky3: pinky3R,
                thumb1: thumb1R, thumb2: thumb2R, thumb3: thumb3R);
            _legL = new HumanLegInput(part: BodyPart.LegL, model: _model, thighBend: thighBendL, thighTwist: thighTwistL, 
                shin: shinL, foot: footL, footHolder: footHolderL, metatarsals: metatarsalsL, toe: toeL, toeHolder: toeHolderL, bigToe: bigToeL, bigToe2: bigToe2L, 
                smallToe11: smallToe11L, smallToe12: smallToe12L, smallToe21: smallToe21L, smallToe22: smallToe22L, 
                smallToe31: smallToe31L, smallToe32: smallToe32L, smallToe41: smallToe41L, smallToe42: smallToe42L);
            _legR = new HumanLegInput(part: BodyPart.LegR, model: _model, thighBend: thighBendR, thighTwist: thighTwistR, 
                shin: shinR, foot: footR, footHolder: footHolderR, metatarsals: metatarsalsR, toe: toeR, toeHolder: toeHolderR, bigToe: bigToeR, bigToe2: bigToe2R, 
                smallToe11: smallToe11R, smallToe12: smallToe12R, smallToe21: smallToe21R, smallToe22: smallToe22R, 
                smallToe31: smallToe31R, smallToe32: smallToe32R, smallToe41: smallToe41R, smallToe42: smallToe42R);
            _tongue = new HumanTongueInput(_model, tongue01, tongue02, tongue03, tongue04);
            _spine = new HumanSpineInput(_model, hip, abdomenLower, abdomenUpper, chestLower, chestUpper, neckLower, neckUpper);
            _pelvis = new HumanBoneInput(BodyPart.Pelvis, _model, pelvis);
            _upperNeck = new HumanBoneInput(BodyPart.Neck, _model, neckUpper);
            _head = new HumanBoneInput(BodyPart.Head, _model, head);
            if (breastL != null) _breastL = new HumanBoneInput(BodyPart.BreastL, _model, breastL);
            if (breastR != null) _breastR = new HumanBoneInput(BodyPart.BreastR, _model, breastR);

            _bones = d;
        }
        HumanoidType IComplexHumanDefinition.HumanoidType => HumanoidType.Genesis8;
        bool IComplexHumanDefinition.HasCompletedImport => _hasCompletedImport;
        string IComplexHumanDefinition.Persona => _person.Persona;
        Transform IComplexHumanDefinition.Model => _model;
        HumanBoneInput IComplexHumanDefinition.Root => _root;
        HumanBoneInput IComplexHumanDefinition.Hip => _hip;
        GenFaceInput IComplexHumanDefinition.GenFace => _face;
        MHFaceInput IComplexHumanDefinition.MHFace => null;
        HumanArmInput IComplexHumanDefinition.ArmL => _armL;
        HumanArmInput IComplexHumanDefinition.ArmR => _armR;
        HumanLegInput IComplexHumanDefinition.LegL => _legL;
        HumanLegInput IComplexHumanDefinition.LegR => _legR;
        HumanSpineInput IComplexHumanDefinition.Spine => _spine;
        HumanTongueInput IComplexHumanDefinition.Tongue => _tongue;
        HumanBoneInput IComplexHumanDefinition.Pelvis => _pelvis;
        HumanBoneInput IComplexHumanDefinition.NeckUpper => _upperNeck;
        HumanBoneInput IComplexHumanDefinition.Head => _head;
        HumanBoneInput IComplexHumanDefinition.BreastL => _breastL;
        HumanBoneInput IComplexHumanDefinition.BreastR => _breastR;
        float IComplexHumanDefinition.Height => _height * _model.localScale.x;
        IDictionary<string, Transform> IComplexHumanDefinition.BonesByName => _bones;
        bool IComplexHumanDefinition.HasBreasts => _person.HasBreasts;

    }
}
