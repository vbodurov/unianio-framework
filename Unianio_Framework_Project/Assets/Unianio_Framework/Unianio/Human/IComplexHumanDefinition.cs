using System.Collections.Generic;
using Unianio.Enums;
using Unianio.Genesis.IK.Input;
using Unianio.Human.Input;
using Unianio.MakeHuman;
using UnityEngine;

namespace Unianio.Human
{
    public interface IComplexHumanDefinition
    {
        HumanoidType HumanoidType { get; }
        bool HasCompletedImport { get; }
        string Persona { get; }
        Transform Model { get; }
        HumanBoneInput Root { get; }
        HumanBoneInput Hip { get; }
        GenFaceInput GenFace { get; }
        MHFaceInput MHFace { get; }
        HumanArmInput ArmL { get; }
        HumanArmInput ArmR { get; }
        HumanLegInput LegL { get; }
        HumanLegInput LegR { get; }
        HumanSpineInput Spine { get; }
        HumanTongueInput Tongue { get; }
        HumanBoneInput Pelvis { get; }
        HumanBoneInput NeckUpper { get; }
        HumanBoneInput Head { get; }
        HumanBoneInput BreastL { get; }
        HumanBoneInput BreastR { get; }
        float Height { get; }
        IDictionary<string, Transform> BonesByName { get; }
        bool HasBreasts { get; }
    }

}