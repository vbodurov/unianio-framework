using System;
using System.Collections.Generic;
using System.Linq;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.Genesis.IK.Input;
using Unianio.Human;
using Unianio.Human.Input;
using Unianio.Rigged;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.MakeHuman
{
    public class MakeHumanDefinition : IComplexHumanDefinition
    {
        PersonDetails _person;   
        readonly Transform _model;
        readonly HumanArmInput _armL, _armR;
        readonly HumanLegInput _legL, _legR;
        readonly HumanSpineInput _spine;
        readonly HumanTongueInput _tongue;
        readonly MHFaceInput _face;
        readonly HumanBoneInput _root, _hip, _pelvis, _upperNeck, _head, _breastL, _breastR;
        readonly float _height;
        readonly IDictionary<string, Transform> _bones;
        readonly bool _hasCompletedImport;

        public MakeHumanDefinition(PersonDetails personDetails, Transform model)
        {
            _person = personDetails;
            _model = model;

            var d = _model.PlaceChildrenInDictionary();
            var top = _model.GetDirectChildren();
            _hasCompletedImport = d.ContainsKey(mhBone.pelvis);
            var root = top.FirstOrDefault(e => !e.name.Contains("_"));
            if(root == null) throw new Exception("Cannot find root");
            var hip = d[mhBone.root];
            var pelvis = d.GetOrDefault(mhBone.pelvis);
            var head = d[mhBone.head];

            _height = head.position.y - _model.position.y + 0.2f;



            // spine:
            var abdomenLower = d[mhBone.spine04];
            var abdomenUpper = d[mhBone.spine03];
            var chestLower = d[mhBone.spine02];
            var chestUpper = d[mhBone.spine01];
            var neckLower = d[mhBone.neck01];
            var neckUpper = d[mhBone.neck02];

            // other
            var breastL = d[mhBone.breast_L];
            var breastR = d[mhBone.breast_R];

            // tongue:
            var tongue01 = d[mhBone.tongue01];
            var tongue02 = d[mhBone.tongue02];
            var tongue03 = d[mhBone.tongue03];
            var tongue04 = d[mhBone.tongue04];

            // arm left:
            var armRootL = d.GetOrDefault(mhBone.arm_root_L);
            var collarL = d[mhBone.clavicle_L];
            var shoulderL = d[mhBone.upperarm01_L];//d[mhBone.upperarm01_L];
//            var shoulderL = d[mhBone.shoulder01_L];//d[mhBone.upperarm01_L];
            var shoulderL_2 = d[mhBone.shoulder01_L];
            var shoulderTwistL = d[mhBone.upperarm02_L];
            var forearmL = d[mhBone.lowerarm01_L];
            var forearmTwistL = d[mhBone.lowerarm02_L];
            var handL = d[mhBone.wrist_L];
            var index0L = d[mhBone.metacarpal1_L];
            var index1L = d[mhBone.finger2_1_L];
            var index2L = d[mhBone.finger2_2_L];
            var index3L = d[mhBone.finger2_3_L];
            var middle0L = d[mhBone.metacarpal2_L];
            var middle1L = d[mhBone.finger3_1_L];
            var middle2L = d[mhBone.finger3_2_L];
            var middle3L = d[mhBone.finger3_3_L];
            var ring0L = d[mhBone.metacarpal3_L];
            var ring1L = d[mhBone.finger4_1_L];
            var ring2L = d[mhBone.finger4_2_L];
            var ring3L = d[mhBone.finger4_3_L];
            var pinky0L = d[mhBone.metacarpal4_L];
            var pinky1L = d[mhBone.finger5_1_L];
            var pinky2L = d[mhBone.finger5_2_L];
            var pinky3L = d[mhBone.finger5_3_L];

            var thumb1L = d[mhBone.finger1_1_L];
            var thumb2L = d[mhBone.finger1_2_L];
            var thumb3L = d[mhBone.finger1_3_L];

            // arm right:
            var armRootR = d.GetOrDefault(mhBone.arm_root_R);
            var collarR = d[mhBone.clavicle_R];//d[mhBone.shoulder01_R];
            var shoulderR = d[mhBone.upperarm01_R];//d[mhBone.upperarm01_R];
//            var shoulderR = d[mhBone.shoulder01_R];//d[mhBone.upperarm01_R];
            var shoulderR_2 = d[mhBone.shoulder01_R];
            var shoulderTwistR = d[mhBone.upperarm02_R];
            var forearmR = d[mhBone.lowerarm01_R];
            var forearmTwistR = d[mhBone.lowerarm02_R];
            var handR = d[mhBone.wrist_R];
            var index0R = d[mhBone.metacarpal1_R];
            var index1R = d[mhBone.finger2_1_R];
            var index2R = d[mhBone.finger2_2_R];
            var index3R = d[mhBone.finger2_3_R];
            var middle0R = d[mhBone.metacarpal2_R];
            var middle1R = d[mhBone.finger3_1_R];
            var middle2R = d[mhBone.finger3_2_R];
            var middle3R = d[mhBone.finger3_3_R];
            var ring0R = d[mhBone.metacarpal3_R];
            var ring1R = d[mhBone.finger4_1_R];
            var ring2R = d[mhBone.finger4_2_R];
            var ring3R = d[mhBone.finger4_3_R];
            var pinky0R = d[mhBone.metacarpal4_R];
            var pinky1R = d[mhBone.finger5_1_R];
            var pinky2R = d[mhBone.finger5_2_R];
            var pinky3R = d[mhBone.finger5_3_R];

            var thumb1R = d[mhBone.finger1_1_R];
            var thumb2R = d[mhBone.finger1_2_R];
            var thumb3R = d[mhBone.finger1_3_R];

            // face:
            var eyeL = d[mhBone.eye_L];
            var eyeR = d[mhBone.eye_R];
            eyeL.localPosition = eyeL.localPosition + v3.bk.By(0.001);
            eyeR.localPosition = eyeR.localPosition + v3.bk.By(0.001);

            var eyelidUpL = d[mhBone.orbicularis03_L];
            var eyelidDnL = d[mhBone.orbicularis04_L];
            var eyelidUpR = d[mhBone.orbicularis03_R];
            var eyelidDnR = d[mhBone.orbicularis04_R];
            var jaw = d[mhBone.jaw];

            // face-mouth-lower
            var special04 = d[mhBone.special04];
            var oris04_L = d[mhBone.oris04_L];
            var oris03_L = d[mhBone.oris03_L];
            var oris04_R = d[mhBone.oris04_R];
            var oris03_R = d[mhBone.oris03_R];
            var oris01 = d[mhBone.oris01];
            var oris02 = d[mhBone.oris02];

            // face-mouth-upper
            var special01 = d[mhBone.special01];
            var oris05 = d[mhBone.oris05];
            var oris06 = d[mhBone.oris06];
            var oris06_L = d[mhBone.oris06_L];
            var oris07_L = d[mhBone.oris07_L];
            var oris06_R = d[mhBone.oris06_R];
            var oris07_R = d[mhBone.oris07_R];

            // face-smile
            var levator02_L = d[mhBone.levator02_L];
            var levator03_L = d[mhBone.levator03_L];
            var levator04_L = d[mhBone.levator04_L];
            var levator05_L = d[mhBone.levator05_L];
            var levator02_R = d[mhBone.levator02_R];
            var levator03_R = d[mhBone.levator03_R];
            var levator04_R = d[mhBone.levator04_R];
            var levator05_R = d[mhBone.levator05_R];

            // face-cheek
            var temporalis02_L = d[mhBone.temporalis02_L];
            var risorius02_L = d[mhBone.risorius02_L];
            var risorius03_L = d[mhBone.risorius03_L];
            var temporalis02_R = d[mhBone.temporalis02_R];
            var risorius02_R = d[mhBone.risorius02_R];
            var risorius03_R = d[mhBone.risorius03_R];

            // eyebrows
            var temporalis01_R = d[mhBone.temporalis01_R];
            var oculi02_R = d[mhBone.oculi02_R];
            var oculi01_R = d[mhBone.oculi01_R];
            var temporalis01_L = d[mhBone.temporalis01_L];
            var oculi02_L = d[mhBone.oculi02_L];
            var oculi01_L = d[mhBone.oculi01_L];

            // leg left
            var thighBendL = d[mhBone.upperleg01_L];
            var thighTwistL = d[mhBone.upperleg02_L];
            var shinL = d[mhBone.lowerleg01_L];
            var footL = d[mhBone.foot_L];
            var footHolderL = footL;
            Transform metatarsalsL = null;
            var toeL = d[mhBone.toe1_1_L];
            var toeHolderL = toeL;

            // leg right
            var thighBendR = d[mhBone.upperleg01_R];
            var thighTwistR = d[mhBone.upperleg02_R];
            var shinR = d[mhBone.lowerleg01_R];
            var footR = d[mhBone.foot_R];
            var footHolderR = footR;
            Transform metatarsalsR = null;
            var toeR = d[mhBone.toe1_1_R];
            var toeHolderR = toeR;

            _hasCompletedImport = footHolderL != null;

            _root = new HumanBoneInput(BodyPart.EntireBody, _model, root);
            _hip = new HumanBoneInput(BodyPart.Hip, _model, hip);
            _armL = new HumanArmInput(part: BodyPart.ArmL, model: _model, armRoot: armRootL,
                collar: collarL, shoulder: shoulderL, shoulder_2: shoulderL_2, shoulderTwist: shoulderTwistL, forearm: forearmL, forearmTwist: forearmTwistL, hand: handL,
                index0: index0L, index1: index1L, index2: index2L, index3: index3L,
                middle0: middle0L, middle1: middle1L, middle2: middle2L, middle3: middle3L,
                ring0: ring0L, ring1: ring1L, ring2: ring2L, ring3: ring3L,
                pinky0: pinky0L, pinky1: pinky1L, pinky2: pinky2L, pinky3: pinky3L,
                thumb1: thumb1L, thumb2: thumb2L, thumb3: thumb3L);
            _armR = new HumanArmInput(part: BodyPart.ArmR, model: _model, armRoot: armRootR,
                collar: collarR, shoulder: shoulderR, shoulder_2: shoulderR_2, shoulderTwist: shoulderTwistR, forearm: forearmR, forearmTwist: forearmTwistR, hand: handR,
                index0: index0R, index1: index1R, index2: index2R, index3: index3R,
                middle0: middle0R, middle1: middle1R, middle2: middle2R, middle3: middle3R,
                ring0: ring0R, ring1: ring1R, ring2: ring2R, ring3: ring3R,
                pinky0: pinky0R, pinky1: pinky1R, pinky2: pinky2R, pinky3: pinky3R,
                thumb1: thumb1R, thumb2: thumb2R, thumb3: thumb3R);
            _legL = new HumanLegInput(part: BodyPart.LegL, model: _model, thighBend: thighBendL, thighTwist: thighTwistL,
                shin: shinL, foot: footL, footHolder: footHolderL, metatarsals: metatarsalsL, toe: toeL, toeHolder: toeHolderL, bigToe: null, bigToe2: null,
                smallToe11: null, smallToe12: null, smallToe21: null, smallToe22: null,
                smallToe31: null, smallToe32: null, smallToe41: null, smallToe42: null);
            _legR = new HumanLegInput(part: BodyPart.LegR, model: _model, thighBend: thighBendR, thighTwist: thighTwistR,
                shin: shinR, foot: footR, footHolder: footHolderR, metatarsals: metatarsalsR, toe: toeR, toeHolder: toeHolderR, 
                bigToe: null, bigToe2: null,
                smallToe11: null, smallToe12: null, smallToe21: null, smallToe22: null,
                smallToe31: null, smallToe32: null, smallToe41: null, smallToe42: null);
            _face = new MHFaceInput(
                eyeL: eyeL, eyeR: eyeR, jaw: jaw, 
                eyelidUpL: eyelidUpL, eyelidDnL: eyelidDnL, 
                eyelidUpR: eyelidUpR, eyelidDnR: eyelidDnR,
                special04: special04,
                oris04_L: oris04_L,
                oris03_L: oris03_L,
                oris04_R: oris04_R,
                oris03_R: oris03_R,
                oris01: oris01,
                oris02: oris02,
                special01: special01,
                oris05: oris05,
                oris06: oris06,
                oris06_L: oris06_L,
                oris07_L: oris07_L,
                oris06_R: oris06_R,
                oris07_R: oris07_R,
                levator02_L: levator02_L,
                levator03_L: levator03_L,
                levator04_L: levator04_L,
                levator05_L: levator05_L,
                levator02_R: levator02_R,
                levator03_R: levator03_R,
                levator04_R: levator04_R,
                levator05_R: levator05_R,
                temporalis02_L: temporalis02_L,
                risorius02_L: risorius02_L,
                risorius03_L: risorius03_L,
                temporalis02_R: temporalis02_R,
                risorius02_R: risorius02_R,
                risorius03_R: risorius03_R,
                temporalis01_R: temporalis01_R,
                oculi02_R: oculi02_R,
                oculi01_R: oculi01_R,
                temporalis01_L: temporalis01_L,
                oculi02_L: oculi02_L,
                oculi01_L: oculi01_L);
            _tongue = new HumanTongueInput(_model, tongue01, tongue02, tongue03, tongue04);
            _spine = new HumanSpineInput(_model, hip, abdomenLower, abdomenUpper, chestLower, chestUpper, neckLower, neckUpper);
            _pelvis = new HumanBoneInput(BodyPart.Pelvis, _model, pelvis);
            _upperNeck = new HumanBoneInput(BodyPart.Neck, _model, neckUpper);
            _head = new HumanBoneInput(BodyPart.Head, _model, head);
            if (breastL != null) _breastL = new HumanBoneInput(BodyPart.BreastL, _model, breastL);
            if (breastR != null) _breastR = new HumanBoneInput(BodyPart.BreastR, _model, breastR);

            _bones = d;
        }
        HumanoidType IComplexHumanDefinition.HumanoidType => HumanoidType.MakeHuman;
        bool IComplexHumanDefinition.HasCompletedImport => _hasCompletedImport;
        string IComplexHumanDefinition.Persona => _person.Persona;
        Transform IComplexHumanDefinition.Model => _model;
        HumanBoneInput IComplexHumanDefinition.Root => _root;
        HumanBoneInput IComplexHumanDefinition.Hip => _hip;
        GenFaceInput IComplexHumanDefinition.GenFace => null;
        MHFaceInput IComplexHumanDefinition.MHFace => _face;
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