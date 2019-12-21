using System; using Unianio.Extensions;
using Unianio.Graphs;
using Unianio.Human;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.MakeHuman
{
    public sealed class MHFaceGroup : IBlinkController
    {
        readonly IComplexHuman _human;
        readonly HumanFaceConfig _config;

        HandlePath _pathJaw, _pathEyeL, _pathEyeR, _pathEyelidUpL, _pathEyelidDnL,
            _pathEyelidUpR, _pathEyelidDnR, _pathSpecial04, _pathOris04L, _pathOris03L,
            _pathOris04R, _pathOris03R, _pathOris01, _pathOris02, _pathSpecial01, _pathOris05,
            _pathOris06, _pathOris06L, _pathOris07L, _pathOris06R, _pathOris07R, _pathLevator02L,
            _pathLevator03L, _pathLevator04L, _pathLevator05L, _pathLevator02R, _pathLevator03R,
            _pathLevator04R, _pathLevator05R, _pathTemporalis02L, _pathRisorius02L, _pathRisorius03L,
            _pathTemporalis02R, _pathRisorius02R, _pathRisorius03R, _pathTemporalis01R, _pathOculi02R,
            _pathOculi01R, _pathTemporalis01L, _pathOculi02L, _pathOculi01L;
        float _blinkUpL, _blinkUpR, _blinkDnL, _blinkDnR, _eyeCurveShift;
        readonly NumericPath _pathBlink, _pathBlinkUpL, _pathBlinkUpR, _pathBlinkDnL, _pathBlinkDnR, _pathEyeCurveShift;
        (HandlePath path, Func<bool> condition, HumBoneHandler bone)[] _allBones;

        public MHFaceGroup(IComplexHuman human, HumanFaceConfigSource configSource)
        {
            _human = human;
            var h = human;
            _config = configSource?.GetConfig() ?? new HumanFaceConfig();
            Model = human.Model;
            var face = human.Definition.MHFace;
            _eyeCurveShift = 0;
            // face
            Jaw = new HumBoneHandler(Model, face.Jaw);

            // eyes
            EyeL = new HumBoneHandler(Model, face.EyeL);
            EyeR = new HumBoneHandler(Model, face.EyeR);

            // eyelids
            EyelidUpL = new HumBoneHandler(Model, face.EyelidUpL);
            EyelidDnL = new HumBoneHandler(Model, face.EyelidDnL);
            EyelidUpR = new HumBoneHandler(Model, face.EyelidUpR);
            EyelidDnR = new HumBoneHandler(Model, face.EyelidDnR);

            // mouth
            Special04 = new HumBoneHandler(Model, face.Special04);
            Oris04L = new HumBoneHandler(Model, face.Oris04L);
            Oris03L = new HumBoneHandler(Model, face.Oris03L);
            Oris04R = new HumBoneHandler(Model, face.Oris04R);
            Oris03R = new HumBoneHandler(Model, face.Oris03R);
            Oris01 = new HumBoneHandler(Model, face.Oris01);
            Oris02 = new HumBoneHandler(Model, face.Oris02);
            Special01 = new HumBoneHandler(Model, face.Special01);
            Oris05 = new HumBoneHandler(Model, face.Oris05);
            Oris06 = new HumBoneHandler(Model, face.Oris06);
            Oris06L = new HumBoneHandler(Model, face.Oris06L);
            Oris07L = new HumBoneHandler(Model, face.Oris07L);
            Oris06R = new HumBoneHandler(Model, face.Oris06R);
            Oris07R = new HumBoneHandler(Model, face.Oris07R);
            Levator02L = new HumBoneHandler(Model, face.Levator02L);
            Levator03L = new HumBoneHandler(Model, face.Levator03L);
            Levator04L = new HumBoneHandler(Model, face.Levator04L);
            Levator05L = new HumBoneHandler(Model, face.Levator05L);
            Levator02R = new HumBoneHandler(Model, face.Levator02R);
            Levator03R = new HumBoneHandler(Model, face.Levator03R);
            Levator04R = new HumBoneHandler(Model, face.Levator04R);
            Levator05R = new HumBoneHandler(Model, face.Levator05R);
            Temporalis02L = new HumBoneHandler(Model, face.Temporalis02L);
            Risorius02L = new HumBoneHandler(Model, face.Risorius02L);
            Risorius03L = new HumBoneHandler(Model, face.Risorius03L);
            Temporalis02R = new HumBoneHandler(Model, face.Temporalis02R);
            Risorius02R = new HumBoneHandler(Model, face.Risorius02R);
            Risorius03R = new HumBoneHandler(Model, face.Risorius03R);
            Temporalis01R = new HumBoneHandler(Model, face.Temporalis01R);
            Oculi02R = new HumBoneHandler(Model, face.Oculi02R);
            Oculi01R = new HumBoneHandler(Model, face.Oculi01R);
            Temporalis01L = new HumBoneHandler(Model, face.Temporalis01L);
            Oculi02L = new HumBoneHandler(Model, face.Oculi02L);
            Oculi01L = new HumBoneHandler(Model, face.Oculi01L);

            _pathBlink =
                new NumericPath(n => Blink01 = n, () => Blink01)
                ;
            _pathBlinkUpL =
                new NumericPath(n => BlinkUpL01 = n, () => BlinkUpL01)
                ;
            _pathBlinkUpR =
                new NumericPath(n => BlinkUpR01 = n, () => BlinkUpR01)
                ;
            _pathBlinkDnL =
                new NumericPath(n => BlinkDnL01 = n, () => BlinkDnL01)
                ;
            _pathBlinkDnR =
                new NumericPath(n => BlinkDnR01 = n, () => BlinkDnR01)
                ;
            _pathEyeCurveShift =
                new NumericPath(n => EyeCurveShiftM11 = n, () => EyeCurveShiftM11)
                ;

            _allBones = new[]
            {
                (path: PathJaw, condition: () => h.AniJaw.IsNotRunning(), bone: Jaw),
                (path: PathEyeL, condition: (Func<bool>)null, bone: EyeL),
                (path: PathEyeR, condition: (Func<bool>)null, bone: EyeR),
                (path: PathEyelidUpL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpL),
                (path: PathEyelidDnL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidDnL),
                (path: PathEyelidUpR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpR),
                (path: PathEyelidDnR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidDnR),
                (path: PathSpecial04, condition: (Func<bool>)null, bone: Special04),
                (path: PathOris04L, condition: (Func<bool>)null, bone: Oris04L),
                (path: PathOris03L, condition: (Func<bool>)null, bone: Oris03L),
                (path: PathOris04R, condition: (Func<bool>)null, bone: Oris04R),
                (path: PathOris03R, condition: (Func<bool>)null, bone: Oris03R),
                (path: PathOris01, condition: (Func<bool>)null, bone: Oris01),
                (path: PathOris02, condition: (Func<bool>)null, bone: Oris02),
                (path: PathSpecial01, condition: (Func<bool>)null, bone: Special01),
                (path: PathOris05, condition: (Func<bool>)null, bone: Oris05),
                (path: PathOris06, condition: (Func<bool>)null, bone: Oris06),
                (path: PathOris06L, condition: (Func<bool>)null, bone: Oris06L),
                (path: PathOris07L, condition: (Func<bool>)null, bone: Oris07L),
                (path: PathOris06R, condition: (Func<bool>)null, bone: Oris06R),
                (path: PathOris07R, condition: (Func<bool>)null, bone: Oris07R),
                (path: PathLevator02L, condition: (Func<bool>)null, bone: Levator02L),
                (path: PathLevator03L, condition: (Func<bool>)null, bone: Levator03L),
                (path: PathLevator04L, condition: (Func<bool>)null, bone: Levator04L),
                (path: PathLevator05L, condition: (Func<bool>)null, bone: Levator05L),
                (path: PathLevator02R, condition: (Func<bool>)null, bone: Levator02R),
                (path: PathLevator03R, condition: (Func<bool>)null, bone: Levator03R),
                (path: PathLevator04R, condition: (Func<bool>)null, bone: Levator04R),
                (path: PathLevator05R, condition: (Func<bool>)null, bone: Levator05R),
                (path: PathTemporalis02L, condition: (Func<bool>)null, bone: Temporalis02L),
                (path: PathRisorius02L, condition: (Func<bool>)null, bone: Risorius02L),
                (path: PathRisorius03L, condition: (Func<bool>)null, bone: Risorius03L),
                (path: PathTemporalis02R, condition: (Func<bool>)null, bone: Temporalis02R),
                (path: PathRisorius02R, condition: (Func<bool>)null, bone: Risorius02R),
                (path: PathRisorius03R, condition: (Func<bool>)null, bone: Risorius03R),
                (path: PathTemporalis01R, condition: (Func<bool>)null, bone: Temporalis01R),
                (path: PathOculi02R, condition: (Func<bool>)null, bone: Oculi02R),
                (path: PathOculi01R, condition: (Func<bool>)null, bone: Oculi01R),
                (path: PathTemporalis01L, condition: (Func<bool>)null, bone: Temporalis01L),
                (path: PathOculi02L, condition: (Func<bool>)null, bone: Oculi02L),
                (path: PathOculi01L, condition: (Func<bool>)null, bone: Oculi01L),
            };
        }
        public (HandlePath path, Func<bool> condition, HumBoneHandler bone)[] AllBones => _allBones;
        public Transform Model { get; }
        public HumBoneHandler Jaw { get; }
        public HumBoneHandler EyeL { get; }
        public HumBoneHandler EyeR { get; }
        public HumBoneHandler EyelidUpL { get; }
        public HumBoneHandler EyelidDnL { get; }
        public HumBoneHandler EyelidUpR { get; }
        public HumBoneHandler EyelidDnR { get; }
        public HumBoneHandler Special04 { get; }
        public HumBoneHandler Oris04L { get; }
        public HumBoneHandler Oris03L { get; }
        public HumBoneHandler Oris04R { get; }
        public HumBoneHandler Oris03R { get; }
        public HumBoneHandler Oris01 { get; }
        public HumBoneHandler Oris02 { get; }
        public HumBoneHandler Special01 { get; }
        public HumBoneHandler Oris05 { get; }
        public HumBoneHandler Oris06 { get; }
        public HumBoneHandler Oris06L { get; }
        public HumBoneHandler Oris07L { get; }
        public HumBoneHandler Oris06R { get; }
        public HumBoneHandler Oris07R { get; }
        public HumBoneHandler Levator02L { get; }
        public HumBoneHandler Levator03L { get; }
        public HumBoneHandler Levator04L { get; }
        public HumBoneHandler Levator05L { get; }
        public HumBoneHandler Levator02R { get; }
        public HumBoneHandler Levator03R { get; }
        public HumBoneHandler Levator04R { get; }
        public HumBoneHandler Levator05R { get; }
        public HumBoneHandler Temporalis02L { get; }
        public HumBoneHandler Risorius02L { get; }
        public HumBoneHandler Risorius03L { get; }
        public HumBoneHandler Temporalis02R { get; }
        public HumBoneHandler Risorius02R { get; }
        public HumBoneHandler Risorius03R { get; }
        public HumBoneHandler Temporalis01R { get; }
        public HumBoneHandler Oculi02R { get; }
        public HumBoneHandler Oculi01R { get; }
        public HumBoneHandler Temporalis01L { get; }
        public HumBoneHandler Oculi02L { get; }
        public HumBoneHandler Oculi01L { get; }

        public HandlePath PathJaw => _pathJaw ?? (_pathJaw = new HandlePath(Jaw));
        public HandlePath PathEyeL => _pathEyeL ?? (_pathEyeL = new HandlePath(EyeL));
        public HandlePath PathEyeR => _pathEyeR ?? (_pathEyeR = new HandlePath(EyeR));
        public HandlePath PathEyelidUpL => _pathEyelidUpL ?? (_pathEyelidUpL = new HandlePath(EyelidUpL));
        public HandlePath PathEyelidDnL => _pathEyelidDnL ?? (_pathEyelidDnL = new HandlePath(EyelidDnL));
        public HandlePath PathEyelidUpR => _pathEyelidUpR ?? (_pathEyelidUpR = new HandlePath(EyelidUpR));
        public HandlePath PathEyelidDnR => _pathEyelidDnR ?? (_pathEyelidDnR = new HandlePath(EyelidDnR));
        public HandlePath PathSpecial04 => _pathSpecial04 ?? (_pathSpecial04 = new HandlePath(Special04));
        public HandlePath PathOris04L => _pathOris04L ?? (_pathOris04L = new HandlePath(Oris04L));
        public HandlePath PathOris03L => _pathOris03L ?? (_pathOris03L = new HandlePath(Oris03L));
        public HandlePath PathOris04R => _pathOris04R ?? (_pathOris04R = new HandlePath(Oris04R));
        public HandlePath PathOris03R => _pathOris03R ?? (_pathOris03R = new HandlePath(Oris03R));
        public HandlePath PathOris01 => _pathOris01 ?? (_pathOris01 = new HandlePath(Oris01));
        public HandlePath PathOris02 => _pathOris02 ?? (_pathOris02 = new HandlePath(Oris02));
        public HandlePath PathSpecial01 => _pathSpecial01 ?? (_pathSpecial01 = new HandlePath(Special01));
        public HandlePath PathOris05 => _pathOris05 ?? (_pathOris05 = new HandlePath(Oris05));
        public HandlePath PathOris06 => _pathOris06 ?? (_pathOris06 = new HandlePath(Oris06));
        public HandlePath PathOris06L => _pathOris06L ?? (_pathOris06L = new HandlePath(Oris06L));
        public HandlePath PathOris07L => _pathOris07L ?? (_pathOris07L = new HandlePath(Oris07L));
        public HandlePath PathOris06R => _pathOris06R ?? (_pathOris06R = new HandlePath(Oris06R));
        public HandlePath PathOris07R => _pathOris07R ?? (_pathOris07R = new HandlePath(Oris07R));
        public HandlePath PathLevator02L => _pathLevator02L ?? (_pathLevator02L = new HandlePath(Levator02L));
        public HandlePath PathLevator03L => _pathLevator03L ?? (_pathLevator03L = new HandlePath(Levator03L));
        public HandlePath PathLevator04L => _pathLevator04L ?? (_pathLevator04L = new HandlePath(Levator04L));
        public HandlePath PathLevator05L => _pathLevator05L ?? (_pathLevator05L = new HandlePath(Levator05L));
        public HandlePath PathLevator02R => _pathLevator02R ?? (_pathLevator02R = new HandlePath(Levator02R));
        public HandlePath PathLevator03R => _pathLevator03R ?? (_pathLevator03R = new HandlePath(Levator03R));
        public HandlePath PathLevator04R => _pathLevator04R ?? (_pathLevator04R = new HandlePath(Levator04R));
        public HandlePath PathLevator05R => _pathLevator05R ?? (_pathLevator05R = new HandlePath(Levator05R));
        public HandlePath PathTemporalis02L => _pathTemporalis02L ?? (_pathTemporalis02L = new HandlePath(Temporalis02L));
        public HandlePath PathRisorius02L => _pathRisorius02L ?? (_pathRisorius02L = new HandlePath(Risorius02L));
        public HandlePath PathRisorius03L => _pathRisorius03L ?? (_pathRisorius03L = new HandlePath(Risorius03L));
        public HandlePath PathTemporalis02R => _pathTemporalis02R ?? (_pathTemporalis02R = new HandlePath(Temporalis02R));
        public HandlePath PathRisorius02R => _pathRisorius02R ?? (_pathRisorius02R = new HandlePath(Risorius02R));
        public HandlePath PathRisorius03R => _pathRisorius03R ?? (_pathRisorius03R = new HandlePath(Risorius03R));
        public HandlePath PathTemporalis01R => _pathTemporalis01R ?? (_pathTemporalis01R = new HandlePath(Temporalis01R));
        public HandlePath PathOculi02R => _pathOculi02R ?? (_pathOculi02R = new HandlePath(Oculi02R));
        public HandlePath PathOculi01R => _pathOculi01R ?? (_pathOculi01R = new HandlePath(Oculi01R));
        public HandlePath PathTemporalis01L => _pathTemporalis01L ?? (_pathTemporalis01L = new HandlePath(Temporalis01L));
        public HandlePath PathOculi02L => _pathOculi02L ?? (_pathOculi02L = new HandlePath(Oculi02L));
        public HandlePath PathOculi01L => _pathOculi01L ?? (_pathOculi01L = new HandlePath(Oculi01L));
        public NumericPath PathEyeCurveShift => _pathEyeCurveShift;
        public NumericPath PathBlink => _pathBlink;
        public NumericPath PathBlinkUpL => _pathBlinkUpL;
        public NumericPath PathBlinkUpR => _pathBlinkUpR;
        public NumericPath PathBlinkDnL => _pathBlinkDnL;
        public NumericPath PathBlinkDnR => _pathBlinkDnR;
        /// <summary>
        /// 1 = ark up like in smile
        /// 0 = normal
        /// -1 = ark down like when looking down
        /// </summary>
        public double EyeCurveShiftM11
        {
            get => _eyeCurveShift;
            set
            {
                _eyeCurveShift = ((float)value).Clamp(-1f, 1f);
                ApplyBlink();
            }
        }
        /// <summary>
        /// 0 = open eyes
        /// 1 = closed eyes
        /// </summary>
        public double Blink01
        {
            get => avgOf(_blinkUpR, _blinkDnL, _blinkDnL, _blinkDnR);
            set
            {
                var blink01 = (float)value;
                BlinkUpL01 = BlinkUpR01 = BlinkDnL01 = BlinkDnR01 = blink01;
            }
        }

        float EyelidMaxAngle => _config.EyelidMaxAngle;
        public double BlinkUpL01
        {
            get => _blinkUpL;
            set
            {
                var x = CalculateBlinkUp(_blinkUpL = (float)value, _eyeCurveShift);
                EyelidUpL.localRotation = EyelidUpL.IniLocalRot * Quaternion.AngleAxis(EyelidMaxAngle * x, v3.rt);
            }
        }
        public double BlinkUpR01
        {
            get => _blinkUpR;
            set
            {
                var x = CalculateBlinkUp(_blinkUpR = (float)value, _eyeCurveShift);
                EyelidUpR.localRotation = EyelidUpR.IniLocalRot * Quaternion.AngleAxis(EyelidMaxAngle * x, v3.rt);
            }
        }
        public double BlinkDnL01
        {
            get => _blinkDnL;
            set
            {
                var x = CalculateBlinkDn(_blinkDnL = (float)value, _eyeCurveShift);
                EyelidDnL.localRotation = EyelidDnL.IniLocalRot * Quaternion.AngleAxis(EyelidMaxAngle * x, v3.lt);
            }
        }
        public double BlinkDnR01
        {
            get => _blinkDnR;
            set
            {
                var x = CalculateBlinkDn(_blinkDnR = (float)value, _eyeCurveShift);
                EyelidDnR.localRotation = EyelidDnR.IniLocalRot * Quaternion.AngleAxis(EyelidMaxAngle * x, v3.lt);
            }
        }

        void ApplyBlink()
        {
            BlinkUpL01 = _blinkUpL;
            BlinkUpR01 = _blinkUpR;
            BlinkDnL01 = _blinkDnL;
            BlinkDnR01 = _blinkDnR;
        }

        /// <summary>
        /// n01
        /// 0 = open eyes
        /// 1 = closed eyes
        /// 
        /// eyeCurveShift
        /// 1 = ark up like in smile
        /// 0 = normal
        /// -1 = ark down like when looking down
        ///
        /// upper lip
        /// eyeCurveShift | open n01 = 1 |  close n01 = 0
        /// 1 = SMILE     |       0      |       0.5
        /// 0 = NORMAL    |       0      |       1.0
        ///-1 = LOOK DN   |       1      |       1.5
        ///                  max(-x, 0)        1-x*0.5
        /// 
        /// lower lip
        /// eyeCurveShift | open n01 = 1 |  close n01 = 0
        /// 1 = SMILE     |       1      |       1.5
        /// 0 = NORMAL    |       0      |       1.0
        ///-1 = LOOK DN   |       0      |       0.5
        ///                  max(x, 0)         1+x*0.5
        /// </summary>
        float CalculateBlinkUp(float n01, float eyeCurveShift)
            => lerp(
                max(-eyeCurveShift, 0),
                1 - eyeCurveShift * 0.5, 
                n01);
        float CalculateBlinkDn(float n01, float eyeCurveShift)
            => lerp(
                max(eyeCurveShift, 0),
                1 + eyeCurveShift * 0.5,
                n01);
    }
}