using System; using Unianio.Extensions;
using Unianio.Human;
using Unianio.IK;
using Unianio.Moves;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.MakeHuman
{
    public sealed class MHFaceGroup : IBlinkController
    {
        readonly IComplexHuman _human;
        readonly HumanFaceConfig _config;

        Mover<HumBoneHandler> _moveJaw, _moveEyeL, _moveEyeR, _moveEyelidUpL, _moveEyelidDnL,
            _moveEyelidUpR, _moveEyelidDnR, _moveSpecial04, _moveOris04L, _moveOris03L,
            _moveOris04R, _moveOris03R, _moveOris01, _moveOris02, _moveSpecial01, _moveOris05,
            _moveOris06, _moveOris06L, _moveOris07L, _moveOris06R, _moveOris07R, _moveLevator02L,
            _moveLevator03L, _moveLevator04L, _moveLevator05L, _moveLevator02R, _moveLevator03R,
            _moveLevator04R, _moveLevator05R, _moveTemporalis02L, _moveRisorius02L, _moveRisorius03L,
            _moveTemporalis02R, _moveRisorius02R, _moveRisorius03R, _moveTemporalis01R, _moveOculi02R,
            _moveOculi01R, _moveTemporalis01L, _moveOculi02L, _moveOculi01L;
        float _blinkUpL, _blinkUpR, _blinkDnL, _blinkDnR, _eyeCurveShift;
        readonly NumericMover _moveBlink, _moveBlinkUpL, _moveBlinkUpR, _moveBlinkDnL, _moveBlinkDnR, _moveEyeCurveShift;
        (IExecutorOfProgress path, Func<bool> condition, HumBoneHandler bone)[] _allBones;

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

            _moveBlink =
                new NumericMover(n => Blink01 = n, () => Blink01)
                ;
            _moveBlinkUpL =
                new NumericMover(n => BlinkUpL01 = n, () => BlinkUpL01)
                ;
            _moveBlinkUpR =
                new NumericMover(n => BlinkUpR01 = n, () => BlinkUpR01)
                ;
            _moveBlinkDnL =
                new NumericMover(n => BlinkDnL01 = n, () => BlinkDnL01)
                ;
            _moveBlinkDnR =
                new NumericMover(n => BlinkDnR01 = n, () => BlinkDnR01)
                ;
            _moveEyeCurveShift =
                new NumericMover(n => EyeCurveShiftM11 = n, () => EyeCurveShiftM11)
                ;

            _allBones = new[]
            {
                (path: (IExecutorOfProgress)MoveJaw, condition: () => h.AniJaw.IsNotRunning(), bone: Jaw),
                (path: (IExecutorOfProgress)MoveEyeL, condition: (Func<bool>)null, bone: EyeL),
                (path: MoveEyeR, condition: (Func<bool>)null, bone: EyeR),
                (path: MoveEyelidUpL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpL),
                (path: MoveEyelidDnL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidDnL),
                (path: MoveEyelidUpR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpR),
                (path: MoveEyelidDnR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidDnR),
                (path: MoveSpecial04, condition: (Func<bool>)null, bone: Special04),
                (path: MoveOris04L, condition: (Func<bool>)null, bone: Oris04L),
                (path: MoveOris03L, condition: (Func<bool>)null, bone: Oris03L),
                (path: MoveOris04R, condition: (Func<bool>)null, bone: Oris04R),
                (path: MoveOris03R, condition: (Func<bool>)null, bone: Oris03R),
                (path: MoveOris01, condition: (Func<bool>)null, bone: Oris01),
                (path: MoveOris02, condition: (Func<bool>)null, bone: Oris02),
                (path: MoveSpecial01, condition: (Func<bool>)null, bone: Special01),
                (path: MoveOris05, condition: (Func<bool>)null, bone: Oris05),
                (path: MoveOris06, condition: (Func<bool>)null, bone: Oris06),
                (path: MoveOris06L, condition: (Func<bool>)null, bone: Oris06L),
                (path: MoveOris07L, condition: (Func<bool>)null, bone: Oris07L),
                (path: MoveOris06R, condition: (Func<bool>)null, bone: Oris06R),
                (path: MoveOris07R, condition: (Func<bool>)null, bone: Oris07R),
                (path: MoveLevator02L, condition: (Func<bool>)null, bone: Levator02L),
                (path: MoveLevator03L, condition: (Func<bool>)null, bone: Levator03L),
                (path: MoveLevator04L, condition: (Func<bool>)null, bone: Levator04L),
                (path: MoveLevator05L, condition: (Func<bool>)null, bone: Levator05L),
                (path: MoveLevator02R, condition: (Func<bool>)null, bone: Levator02R),
                (path: MoveLevator03R, condition: (Func<bool>)null, bone: Levator03R),
                (path: MoveLevator04R, condition: (Func<bool>)null, bone: Levator04R),
                (path: MoveLevator05R, condition: (Func<bool>)null, bone: Levator05R),
                (path: MoveTemporalis02L, condition: (Func<bool>)null, bone: Temporalis02L),
                (path: MoveRisorius02L, condition: (Func<bool>)null, bone: Risorius02L),
                (path: MoveRisorius03L, condition: (Func<bool>)null, bone: Risorius03L),
                (path: MoveTemporalis02R, condition: (Func<bool>)null, bone: Temporalis02R),
                (path: MoveRisorius02R, condition: (Func<bool>)null, bone: Risorius02R),
                (path: MoveRisorius03R, condition: (Func<bool>)null, bone: Risorius03R),
                (path: MoveTemporalis01R, condition: (Func<bool>)null, bone: Temporalis01R),
                (path: MoveOculi02R, condition: (Func<bool>)null, bone: Oculi02R),
                (path: MoveOculi01R, condition: (Func<bool>)null, bone: Oculi01R),
                (path: MoveTemporalis01L, condition: (Func<bool>)null, bone: Temporalis01L),
                (path: MoveOculi02L, condition: (Func<bool>)null, bone: Oculi02L),
                (path: MoveOculi01L, condition: (Func<bool>)null, bone: Oculi01L),
            };
        }
        public (IExecutorOfProgress path, Func<bool> condition, HumBoneHandler bone)[] AllBones => _allBones;
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

        public Mover<HumBoneHandler> MoveJaw => _moveJaw ?? (_moveJaw = new Mover<HumBoneHandler>(Jaw));
        public Mover<HumBoneHandler> MoveEyeL => _moveEyeL ?? (_moveEyeL = new Mover<HumBoneHandler>(EyeL));
        public Mover<HumBoneHandler> MoveEyeR => _moveEyeR ?? (_moveEyeR = new Mover<HumBoneHandler>(EyeR));
        public Mover<HumBoneHandler> MoveEyelidUpL => _moveEyelidUpL ?? (_moveEyelidUpL = new Mover<HumBoneHandler>(EyelidUpL));
        public Mover<HumBoneHandler> MoveEyelidDnL => _moveEyelidDnL ?? (_moveEyelidDnL = new Mover<HumBoneHandler>(EyelidDnL));
        public Mover<HumBoneHandler> MoveEyelidUpR => _moveEyelidUpR ?? (_moveEyelidUpR = new Mover<HumBoneHandler>(EyelidUpR));
        public Mover<HumBoneHandler> MoveEyelidDnR => _moveEyelidDnR ?? (_moveEyelidDnR = new Mover<HumBoneHandler>(EyelidDnR));
        public Mover<HumBoneHandler> MoveSpecial04 => _moveSpecial04 ?? (_moveSpecial04 = new Mover<HumBoneHandler>(Special04));
        public Mover<HumBoneHandler> MoveOris04L => _moveOris04L ?? (_moveOris04L = new Mover<HumBoneHandler>(Oris04L));
        public Mover<HumBoneHandler> MoveOris03L => _moveOris03L ?? (_moveOris03L = new Mover<HumBoneHandler>(Oris03L));
        public Mover<HumBoneHandler> MoveOris04R => _moveOris04R ?? (_moveOris04R = new Mover<HumBoneHandler>(Oris04R));
        public Mover<HumBoneHandler> MoveOris03R => _moveOris03R ?? (_moveOris03R = new Mover<HumBoneHandler>(Oris03R));
        public Mover<HumBoneHandler> MoveOris01 => _moveOris01 ?? (_moveOris01 = new Mover<HumBoneHandler>(Oris01));
        public Mover<HumBoneHandler> MoveOris02 => _moveOris02 ?? (_moveOris02 = new Mover<HumBoneHandler>(Oris02));
        public Mover<HumBoneHandler> MoveSpecial01 => _moveSpecial01 ?? (_moveSpecial01 = new Mover<HumBoneHandler>(Special01));
        public Mover<HumBoneHandler> MoveOris05 => _moveOris05 ?? (_moveOris05 = new Mover<HumBoneHandler>(Oris05));
        public Mover<HumBoneHandler> MoveOris06 => _moveOris06 ?? (_moveOris06 = new Mover<HumBoneHandler>(Oris06));
        public Mover<HumBoneHandler> MoveOris06L => _moveOris06L ?? (_moveOris06L = new Mover<HumBoneHandler>(Oris06L));
        public Mover<HumBoneHandler> MoveOris07L => _moveOris07L ?? (_moveOris07L = new Mover<HumBoneHandler>(Oris07L));
        public Mover<HumBoneHandler> MoveOris06R => _moveOris06R ?? (_moveOris06R = new Mover<HumBoneHandler>(Oris06R));
        public Mover<HumBoneHandler> MoveOris07R => _moveOris07R ?? (_moveOris07R = new Mover<HumBoneHandler>(Oris07R));
        public Mover<HumBoneHandler> MoveLevator02L => _moveLevator02L ?? (_moveLevator02L = new Mover<HumBoneHandler>(Levator02L));
        public Mover<HumBoneHandler> MoveLevator03L => _moveLevator03L ?? (_moveLevator03L = new Mover<HumBoneHandler>(Levator03L));
        public Mover<HumBoneHandler> MoveLevator04L => _moveLevator04L ?? (_moveLevator04L = new Mover<HumBoneHandler>(Levator04L));
        public Mover<HumBoneHandler> MoveLevator05L => _moveLevator05L ?? (_moveLevator05L = new Mover<HumBoneHandler>(Levator05L));
        public Mover<HumBoneHandler> MoveLevator02R => _moveLevator02R ?? (_moveLevator02R = new Mover<HumBoneHandler>(Levator02R));
        public Mover<HumBoneHandler> MoveLevator03R => _moveLevator03R ?? (_moveLevator03R = new Mover<HumBoneHandler>(Levator03R));
        public Mover<HumBoneHandler> MoveLevator04R => _moveLevator04R ?? (_moveLevator04R = new Mover<HumBoneHandler>(Levator04R));
        public Mover<HumBoneHandler> MoveLevator05R => _moveLevator05R ?? (_moveLevator05R = new Mover<HumBoneHandler>(Levator05R));
        public Mover<HumBoneHandler> MoveTemporalis02L => _moveTemporalis02L ?? (_moveTemporalis02L = new Mover<HumBoneHandler>(Temporalis02L));
        public Mover<HumBoneHandler> MoveRisorius02L => _moveRisorius02L ?? (_moveRisorius02L = new Mover<HumBoneHandler>(Risorius02L));
        public Mover<HumBoneHandler> MoveRisorius03L => _moveRisorius03L ?? (_moveRisorius03L = new Mover<HumBoneHandler>(Risorius03L));
        public Mover<HumBoneHandler> MoveTemporalis02R => _moveTemporalis02R ?? (_moveTemporalis02R = new Mover<HumBoneHandler>(Temporalis02R));
        public Mover<HumBoneHandler> MoveRisorius02R => _moveRisorius02R ?? (_moveRisorius02R = new Mover<HumBoneHandler>(Risorius02R));
        public Mover<HumBoneHandler> MoveRisorius03R => _moveRisorius03R ?? (_moveRisorius03R = new Mover<HumBoneHandler>(Risorius03R));
        public Mover<HumBoneHandler> MoveTemporalis01R => _moveTemporalis01R ?? (_moveTemporalis01R = new Mover<HumBoneHandler>(Temporalis01R));
        public Mover<HumBoneHandler> MoveOculi02R => _moveOculi02R ?? (_moveOculi02R = new Mover<HumBoneHandler>(Oculi02R));
        public Mover<HumBoneHandler> MoveOculi01R => _moveOculi01R ?? (_moveOculi01R = new Mover<HumBoneHandler>(Oculi01R));
        public Mover<HumBoneHandler> MoveTemporalis01L => _moveTemporalis01L ?? (_moveTemporalis01L = new Mover<HumBoneHandler>(Temporalis01L));
        public Mover<HumBoneHandler> MoveOculi02L => _moveOculi02L ?? (_moveOculi02L = new Mover<HumBoneHandler>(Oculi02L));
        public Mover<HumBoneHandler> MoveOculi01L => _moveOculi01L ?? (_moveOculi01L = new Mover<HumBoneHandler>(Oculi01L));
        public NumericMover MoveEyeCurveShift => _moveEyeCurveShift;
        public NumericMover MoveBlink => _moveBlink;
        public NumericMover MoveBlinkUpL => _moveBlinkUpL;
        public NumericMover MoveBlinkUpR => _moveBlinkUpR;
        public NumericMover MoveBlinkDnL => _moveBlinkDnL;
        public NumericMover MoveBlinkDnR => _moveBlinkDnR;
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