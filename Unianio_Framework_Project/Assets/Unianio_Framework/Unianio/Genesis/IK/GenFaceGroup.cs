using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Human;
using Unianio.IK;
using Unianio.Moves;
using Unianio.Rigged;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis.IK
{
    public sealed class GenFaceGroup : IBlinkController
    {
        readonly IComplexHuman _human;
        readonly HumanFaceConfig _config;

        Mover<HumBoneHandler> _moveLowerFaceRig, _moveUpperFaceRig,
            _moveJaw,
            // inner circle around the mouth:
            _moveLipCornerL, _moveLipLowerOuterL, _moveLipLowerInnerL, _moveLipLowerMiddle, _moveLipLowerInnerR, _moveLipLowerOuterR, _moveLipCornerR,
            _moveLipUpperOuterL, _moveLipUpperInnerL, _moveLipUpperMiddle, _moveLipUpperInnerR, _moveLipUpperOuterR,
            // eyes:
            _moveEyeL, _moveEyeR,
            // eyelids:
            _moveEyelidUpperInnerL, _moveEyelidUpperL, _moveEyelidUpperOuterL, _moveEyelidLowerInnerL, _moveEyelidLowerL, _moveEyelidLowerOuterL,
            _moveEyelidUpperInnerR, _moveEyelidUpperR, _moveEyelidUpperOuterR, _moveEyelidLowerInnerR, _moveEyelidLowerR, _moveEyelidLowerOuterR,
            _moveEyelidInnerL, _moveEyelidOuterL, _moveEyelidInnerR, _moveEyelidOuterR,
            // eyebrows:
            _moveMidNoseBridge, _moveBrowOuterL, _moveBrowMidL, _moveBrowInnerL, _moveCenterBrow, _moveBrowInnerR, _moveBrowMidR, _moveBrowOuterR,
            //  cheeks:
            _moveCheekUpperL, _moveCheekLowerL, _moveCheekLowerR, _moveCheekUpperR, _moveNasolabialMouthCornerL, _moveNasolabialMouthCornerR,
            _moveLipBelowNoseL, _moveLipNasolabialCreaseL, _moveNasolabialMiddleL, _moveNasolabialUpperL, _moveNostrilL, _moveSquintInnerL, 
            _moveSquintOuterL, _moveJawClenchL, _moveNasolabialLowerL, 
            _moveLipBelowNoseR, _moveLipNasolabialCreaseR, _moveNasolabialMiddleR, _moveNasolabialUpperR, _moveNostrilR, _moveSquintInnerR, 
            _moveSquintOuterR, _moveJawClenchR, _moveNasolabialLowerR
            ;
        float _blinkUpL, _blinkUpR, _blinkDnL, _blinkDnR, _eyeCurveShift;
        readonly NumericMover _moveBlink, _moveBlinkUpL, _moveBlinkUpR, _moveBlinkDnL, _moveBlinkDnR;
        (IExecutorOfProgress path, Func<bool> condition, HumBoneHandler bone)[] _allBones;

        public readonly HumBoneHandler LowerJaw;

        public readonly GenFaceGroupInitialStats Initial;
        

        public GenFaceGroup(IComplexHuman human, HumanFaceConfigSource configSource)
        {
            _human = human;
            _config = configSource?.GetConfig() ?? new HumanFaceConfig();
            var h = human;
            Model = human.Model;
            var face = human.Definition.GenFace;
            _eyeCurveShift = 0;

            // face
            LowerJaw = new HumBoneHandler(Model, face.LowerJaw);
            LowerFaceRig = new HumBoneHandler(Model, face.LowerFaceRig);
            UpperFaceRig = new HumBoneHandler(Model, face.UpperFaceRig);

            // eyes
            EyeL = new HumBoneHandler(Model, face.EyeL);
            EyeR = new HumBoneHandler(Model, face.EyeR);

            // eyebrows
            BrowOuterL = new HumBoneHandler(Model, face.BrowOuterL);
            BrowMidL = new HumBoneHandler(Model, face.BrowMidL);
            BrowInnerL = new HumBoneHandler(Model, face.BrowInnerL);
            CenterBrow = new HumBoneHandler(Model, face.CenterBrow);
            MidNoseBridge = new HumBoneHandler(Model, face.MidNoseBridge);
            BrowInnerR = new HumBoneHandler(Model, face.BrowInnerR);
            BrowMidR = new HumBoneHandler(Model, face.BrowMidR);
            BrowOuterR = new HumBoneHandler(Model, face.BrowOuterR);

            // cheeks and nose
            CheekUpperL = new HumBoneHandler(Model, face.CheekUpperL);
            CheekLowerL = new HumBoneHandler(Model, face.CheekLowerL);
            CheekLowerR = new HumBoneHandler(Model, face.CheekLowerR);
            CheekUpperR = new HumBoneHandler(Model, face.CheekUpperR);
            NasolabialMouthCornerL = new HumBoneHandler(Model, face.NasolabialMouthCornerL);
            NasolabialMouthCornerR = new HumBoneHandler(Model, face.NasolabialMouthCornerR);

            LipBelowNoseL = new HumBoneHandler(Model, face.LipBelowNoseL);
            LipNasolabialCreaseL = new HumBoneHandler(Model, face.LipNasolabialCreaseL);
            NasolabialMiddleL = new HumBoneHandler(Model, face.NasolabialMiddleL);
            NasolabialUpperL = new HumBoneHandler(Model, face.NasolabialUpperL);
            NostrilL = new HumBoneHandler(Model, face.NostrilL);
            SquintInnerL = new HumBoneHandler(Model, face.SquintInnerL);
            SquintOuterL = new HumBoneHandler(Model, face.SquintOuterL);
            JawClenchL = new HumBoneHandler(Model, face.JawClenchL);
            NasolabialLowerL = new HumBoneHandler(Model, face.NasolabialLowerL);

            LipBelowNoseR = new HumBoneHandler(Model, face.LipBelowNoseR);
            LipNasolabialCreaseR = new HumBoneHandler(Model, face.LipNasolabialCreaseR);
            NasolabialMiddleR = new HumBoneHandler(Model, face.NasolabialMiddleR);
            NasolabialUpperR = new HumBoneHandler(Model, face.NasolabialUpperR);
            NostrilR = new HumBoneHandler(Model, face.NostrilR);
            SquintInnerR = new HumBoneHandler(Model, face.SquintInnerR);
            SquintOuterR = new HumBoneHandler(Model, face.SquintOuterR);
            JawClenchR = new HumBoneHandler(Model, face.JawClenchR);
            NasolabialLowerR = new HumBoneHandler(Model, face.NasolabialLowerR);

            // eyelids
            EyelidUpperInnerL = new HumBoneHandler(Model, face.EyelidUpperInnerL);
            EyelidUpperL = new HumBoneHandler(Model, face.EyelidUpperL);
            EyelidUpperOuterL = new HumBoneHandler(Model, face.EyelidUpperOuterL);
            EyelidLowerOuterL = new HumBoneHandler(Model, face.EyelidLowerOuterL);
            EyelidLowerL = new HumBoneHandler(Model, face.EyelidLowerL);
            EyelidLowerInnerL = new HumBoneHandler(Model, face.EyelidLowerInnerL);

            EyelidUpperInnerR = new HumBoneHandler(Model, face.EyelidUpperInnerR);
            EyelidUpperR = new HumBoneHandler(Model, face.EyelidUpperR);
            EyelidUpperOuterR = new HumBoneHandler(Model, face.EyelidUpperOuterR);
            EyelidLowerOuterR = new HumBoneHandler(Model, face.EyelidLowerOuterR);
            EyelidLowerR = new HumBoneHandler(Model, face.EyelidLowerR);
            EyelidLowerInnerR = new HumBoneHandler(Model, face.EyelidLowerInnerR);
        

            EyelidInnerL = new HumBoneHandler(Model, face.EyelidInnerL);
            EyelidOuterL = new HumBoneHandler(Model, face.EyelidOuterL);
            EyelidInnerR = new HumBoneHandler(Model, face.EyelidInnerR);
            EyelidOuterR = new HumBoneHandler(Model, face.EyelidOuterR);

            // mouth
            LipCornerL = new HumBoneHandler(Model, face.LipCornerL);
            LipLowerOuterL = new HumBoneHandler(Model, face.LipLowerOuterL);
            LipLowerInnerL = new HumBoneHandler(Model, face.LipLowerInnerL);
            LipLowerMiddle = new HumBoneHandler(Model, face.LipLowerMiddle);
            LipLowerInnerR = new HumBoneHandler(Model, face.LipLowerInnerR);
            LipLowerOuterR = new HumBoneHandler(Model, face.LipLowerOuterR);
            LipCornerR = new HumBoneHandler(Model, face.LipCornerR);
            LipUpperOuterL = new HumBoneHandler(Model, face.LipUpperOuterL);
            LipUpperInnerL = new HumBoneHandler(Model, face.LipUpperInnerL);
            LipUpperMiddle = new HumBoneHandler(Model, face.LipUpperMiddle);
            LipUpperInnerR = new HumBoneHandler(Model, face.LipUpperInnerR);
            LipUpperOuterR = new HumBoneHandler(Model, face.LipUpperOuterR);

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

            Initial = new GenFaceGroupInitialStats(this);

            _allBones = new[]
           {
                (path: (IExecutorOfProgress)MoveLipCornerL, condition: (Func<bool>)null, bone: LipCornerL ),
                (path: MoveLipLowerOuterL, condition: (Func<bool>)null, bone: LipLowerOuterL ),
                (path: MoveLipLowerInnerL, condition: (Func<bool>)null, bone: LipLowerInnerL ),
                (path: MoveLipLowerMiddle, condition: (Func<bool>)null, bone: LipLowerMiddle ),
                (path: MoveLipLowerInnerR, condition: (Func<bool>)null, bone: LipLowerInnerR ),
                (path: MoveLipLowerOuterR, condition: (Func<bool>)null, bone: LipLowerOuterR ),
                (path: MoveLipCornerR, condition: (Func<bool>)null, bone: LipCornerR ),
                (path: MoveLipUpperOuterL, condition: (Func<bool>)null, bone: LipUpperOuterL ),
                (path: MoveLipUpperInnerL, condition: (Func<bool>)null, bone: LipUpperInnerL ),
                (path: MoveLipUpperMiddle, condition: (Func<bool>)null, bone: LipUpperMiddle ),
                (path: MoveLipUpperInnerR, condition: (Func<bool>)null, bone: LipUpperInnerR ),
                (path: MoveLipUpperOuterR, condition: (Func<bool>)null, bone: LipUpperOuterR ),
                (path: MoveEyelidUpperInnerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperInnerL),
                (path: MoveEyelidUpperL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperL),
                (path: MoveEyelidUpperOuterL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperOuterL),
                (path: MoveEyelidLowerInnerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerInnerL),
                (path: MoveEyelidLowerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerL),
                (path: MoveEyelidLowerOuterL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerOuterL),
                (path: MoveEyelidUpperInnerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperInnerR),
                (path: MoveEyelidUpperR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperR),
                (path: MoveEyelidUpperOuterR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperOuterR),
                (path: MoveEyelidLowerInnerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerInnerR),
                (path: MoveEyelidLowerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerR),
                (path: MoveEyelidLowerOuterR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerOuterR),
                (path: MoveLipBelowNoseR, condition: (Func<bool>)null, bone: LipBelowNoseR ),
                (path: MoveLipNasolabialCreaseR, condition: (Func<bool>)null, bone: LipNasolabialCreaseR ),
                (path: MoveNasolabialMiddleR, condition: (Func<bool>)null, bone: NasolabialMiddleR ),
                (path: MoveNasolabialUpperR, condition: (Func<bool>)null, bone: NasolabialUpperR ),
                (path: MoveNostrilR, condition: (Func<bool>)null, bone: NostrilR ),
                (path: MoveSquintInnerR, condition: (Func<bool>)null, bone: SquintInnerR ),
                (path: MoveSquintOuterR, condition: (Func<bool>)null, bone: SquintOuterR ),
                (path: MoveJawClenchR, condition: (Func<bool>)null, bone: JawClenchR ),
                (path: MoveNasolabialLowerR, condition: (Func<bool>)null, bone: NasolabialLowerR ),
                (path: MoveLipBelowNoseL, condition: (Func<bool>)null, bone: LipBelowNoseL ),
                (path: MoveLipNasolabialCreaseL, condition: (Func<bool>)null, bone: LipNasolabialCreaseL ),
                (path: MoveNasolabialMiddleL, condition: (Func<bool>)null, bone: NasolabialMiddleL ),
                (path: MoveNasolabialUpperL, condition: (Func<bool>)null, bone: NasolabialUpperL ),
                (path: MoveNostrilL, condition: (Func<bool>)null, bone: NostrilL ),
                (path: MoveSquintInnerL, condition: (Func<bool>)null, bone: SquintInnerL ),
                (path: MoveSquintOuterL, condition: (Func<bool>)null, bone: SquintOuterL ),
                (path: MoveJawClenchL, condition: (Func<bool>)null, bone: JawClenchL ),
                (path: MoveNasolabialLowerL, condition: (Func<bool>)null, bone: NasolabialLowerL ),
                (path: MoveNasolabialMouthCornerL, condition: (Func<bool>)null, bone: NasolabialMouthCornerL ),
                (path: MoveNasolabialMouthCornerR, condition: (Func<bool>)null, bone: NasolabialMouthCornerR ),
                (path: MoveCheekUpperL, condition: (Func<bool>)null, bone: CheekUpperL ),
                (path: MoveCheekUpperR, condition: (Func<bool>)null, bone: CheekUpperR ),
                (path: MoveBrowInnerL, condition: (Func<bool>)null, bone: BrowInnerL ),
                (path: MoveBrowInnerR, condition: (Func<bool>)null, bone: BrowInnerR ),
                (path: MoveBrowMidL, condition: (Func<bool>)null,bone:BrowMidL ),
                (path: MoveBrowMidR, condition: (Func<bool>)null, bone: BrowMidR ),
                (path: MoveBrowOuterL, condition: (Func<bool>)null, bone: BrowOuterL ),
                (path: MoveBrowOuterR, condition: (Func<bool>)null, bone: BrowOuterR ),
                (path: MoveCenterBrow, condition: (Func<bool>)null, bone: CenterBrow ),
                (path: MoveJaw, condition: () => h.AniJaw.IsNotRunning(), bone: LowerJaw ),
            };
        }
        public NumericMover MoveEyeCurveShift => throw new NotImplementedException();
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
                // TODO: implement
                //_eyeCurveShift = ((float) value).Clamp(-1f, 1f);
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
                var blink01 = _blinkDnL = _blinkDnR = _blinkUpL = _blinkUpR = value.Float();

                GetUpperEyelidRotations(_human.Persona, blink01,
                    out var upperTurnInner, out var upperTurn, out var upperTurnOuter);
                GetLowerEyelidRotations(_human.Persona, blink01,
                    out var lowerTurnInner, out var lowerTurn, out var lowerTurnOuter);

                ApplyUpperBlinkL(in upperTurnInner, in upperTurn, in upperTurnOuter);
                ApplyUpperBlinkR(in upperTurnInner, in upperTurn, in upperTurnOuter);
                ApplyLowerBlinkL(in lowerTurnInner, in lowerTurn, in lowerTurnOuter);
                ApplyLowerBlinkR(in lowerTurnInner, in lowerTurn, in lowerTurnOuter);
            }
        }
        public double BlinkUpL01
        {
            get => _blinkUpL;
            set
            {
                var blink01 = _blinkUpL = value.Float();

                GetUpperEyelidRotations(_human.Persona, blink01,
                    out var upperTurnInner, out var upperTurn, out var upperTurnOuter);

                ApplyUpperBlinkL(in upperTurnInner, in upperTurn, in upperTurnOuter);
            }
        }
        public double BlinkUpR01
        {
            get => _blinkUpR;
            set
            {
                var blink01 = _blinkUpR = value.Float();

                GetUpperEyelidRotations(_human.Persona, blink01,
                    out var upperTurnInner, out var upperTurn, out var upperTurnOuter);

                ApplyUpperBlinkR(in upperTurnInner, in upperTurn, in upperTurnOuter);
            }
        }
        public double BlinkDnL01
        {
            get => _blinkDnL;
            set
            {
                var blink01 = _blinkDnL = value.Float();

                GetLowerEyelidRotations(_human.Persona, blink01,
                    out var lowerTurnInner, out var lowerTurn, out var lowerTurnOuter);

                ApplyLowerBlinkL(in lowerTurnInner, in lowerTurn, in lowerTurnOuter);
            }
        }
        public double BlinkDnR01
        {
            get => _blinkDnR;
            set
            {
                var blink01 = _blinkDnR = value.Float();

                GetLowerEyelidRotations(_human.Persona, blink01,
                    out var lowerTurnInner, out var lowerTurn, out var lowerTurnOuter);

                ApplyLowerBlinkR(in lowerTurnInner, in lowerTurn, in lowerTurnOuter);
            }
        }
        void ApplyUpperBlinkL(in Quaternion upperTurnInner, in Quaternion upperTurn, in Quaternion upperTurnOuter)
        {
            EyelidUpperInnerL.localRotation = Initial.LocRotEyelidUpperInnerL * upperTurnInner;
            EyelidUpperL.localRotation = Initial.LocRotEyelidUpperL * upperTurn;
            EyelidUpperOuterL.localRotation = Initial.LocRotEyelidUpperOuterL * upperTurnOuter;
        }
        void ApplyUpperBlinkR(in Quaternion upperTurnInner, in Quaternion upperTurn, in Quaternion upperTurnOuter)
        {
            EyelidUpperInnerR.localRotation = Initial.LocRotEyelidUpperInnerR * upperTurnInner;
            EyelidUpperR.localRotation = Initial.LocRotEyelidUpperR * upperTurn;
            EyelidUpperOuterR.localRotation = Initial.LocRotEyelidUpperOuterR * upperTurnOuter;
        }
        void ApplyLowerBlinkL(in Quaternion lowerTurnInner, in Quaternion lowerTurn, in Quaternion lowerTurnOuter)
        {
            EyelidLowerInnerL.localRotation = Initial.LocRotEyelidLowerInnerL * lowerTurnInner;
            EyelidLowerL.localRotation = Initial.LocRotEyelidLowerL * lowerTurn;
            EyelidLowerOuterL.localRotation = Initial.LocRotEyelidLowerOuterL * lowerTurnOuter;
        }
        void ApplyLowerBlinkR(in Quaternion lowerTurnInner, in Quaternion lowerTurn, in Quaternion lowerTurnOuter)
        {
            EyelidLowerInnerR.localRotation = Initial.LocRotEyelidLowerInnerR * lowerTurnInner;
            EyelidLowerR.localRotation = Initial.LocRotEyelidLowerR * lowerTurn;
            EyelidLowerOuterR.localRotation = Initial.LocRotEyelidLowerOuterR * lowerTurnOuter;
        }
        void GetUpperEyelidRotations(string persona, float value01, out Quaternion upperTurnInner, out Quaternion upperTurn, out Quaternion upperTurnOuter)
        {
            upperTurnInner = Quaternion.AngleAxis(+21 * value01, v3.right);
            upperTurn = Quaternion.AngleAxis(+20 * value01, v3.right);
            upperTurnOuter = Quaternion.AngleAxis(+22 * value01, v3.right);
        }
        void GetLowerEyelidRotations(string persona, float value01, out Quaternion lowerTurnInner, out Quaternion lowerTurn, out Quaternion lowerTurnOuter)
        {
            lowerTurnInner = Quaternion.AngleAxis(+10 * value01, v3.left);
            lowerTurn = Quaternion.AngleAxis(+8 * value01, v3.left);
            lowerTurnOuter = Quaternion.AngleAxis(+6 * value01, v3.left);
        }

        public (IExecutorOfProgress path, Func<bool> condition, HumBoneHandler bone)[] AllBones => _allBones;
        public Transform Model { get; }
        public HumBoneHandler LowerFaceRig { get; }
        public HumBoneHandler UpperFaceRig { get; }
        // inner circle around the mouth:
        public HumBoneHandler LipCornerL { get; }
        public HumBoneHandler LipLowerOuterL { get; }
        public HumBoneHandler LipLowerInnerL { get; }
        public HumBoneHandler LipLowerMiddle { get; }
        public HumBoneHandler LipLowerInnerR { get; }
        public HumBoneHandler LipLowerOuterR { get; }
        public HumBoneHandler LipCornerR { get; }
        public HumBoneHandler LipUpperOuterL { get; }
        public HumBoneHandler LipUpperInnerL { get; }
        public HumBoneHandler LipUpperMiddle { get; }
        public HumBoneHandler LipUpperInnerR { get; }
        public HumBoneHandler LipUpperOuterR { get; }
        public HumBoneHandler[] ArrLipCorners => new[] { LipCornerL, LipCornerR };
        public HumBoneHandler[] ArrLipLowerInners => new[] { LipLowerInnerL, LipLowerInnerR };
        public HumBoneHandler[] ArrLipLowerOuters => new[] { LipLowerOuterL, LipLowerOuterR };
        public HumBoneHandler[] ArrLipUpperOuters => new[] { LipUpperOuterL, LipUpperOuterR };
        public HumBoneHandler[] ArrLipUpperInners => new[] { LipUpperInnerL, LipUpperInnerR };

        // eyes:
        public HumBoneHandler EyeL { get; }
        public HumBoneHandler EyeR { get; }
        public HumBoneHandler[] ArrEyes => new[] { EyeL, EyeR };
        // eyelids:
        public HumBoneHandler EyelidUpperInnerL { get; }
        public HumBoneHandler EyelidUpperL { get; }
        public HumBoneHandler EyelidUpperOuterL { get; }
        public HumBoneHandler EyelidLowerInnerL { get; }
        public HumBoneHandler EyelidLowerL { get; }
        public HumBoneHandler EyelidLowerOuterL { get; }
        public HumBoneHandler EyelidUpperInnerR { get; }
        public HumBoneHandler EyelidUpperR { get; }
        public HumBoneHandler EyelidUpperOuterR { get; }
        public HumBoneHandler EyelidLowerInnerR { get; }
        public HumBoneHandler EyelidLowerR { get; }
        public HumBoneHandler EyelidLowerOuterR { get; }
        public HumBoneHandler EyelidInnerL { get; }
        public HumBoneHandler EyelidOuterL { get; }
        public HumBoneHandler EyelidInnerR { get; }
        public HumBoneHandler EyelidOuterR { get; }
        public HumBoneHandler[] ArrEyelidUpperInners => new[] { EyelidUpperInnerL, EyelidUpperInnerR };
        public HumBoneHandler[] ArrEyelidUppers => new[] { EyelidUpperL, EyelidUpperR };
        public HumBoneHandler[] ArrEyelidUpperOuters => new[] { EyelidUpperOuterL, EyelidUpperOuterR };
        public HumBoneHandler[] ArrEyelidLowerInners => new[] { EyelidLowerInnerL, EyelidLowerInnerR };
        public HumBoneHandler[] ArrEyelidLowers => new[] { EyelidLowerL, EyelidLowerR };
        public HumBoneHandler[] ArrEyelidLowerOuters => new[] { EyelidLowerOuterL, EyelidLowerOuterR };
        public HumBoneHandler[] ArrEyelidInners => new[] { EyelidInnerL, EyelidInnerR };
        public HumBoneHandler[] ArrEyelidOuters => new[] { EyelidOuterL, EyelidOuterR };
        // eyebrows: 
        public HumBoneHandler MidNoseBridge { get; }
        public HumBoneHandler BrowOuterL { get; }
        public HumBoneHandler BrowMidL { get; }
        public HumBoneHandler BrowInnerL { get; }
        public HumBoneHandler CenterBrow { get; }
        public HumBoneHandler BrowInnerR { get; }
        public HumBoneHandler BrowMidR { get; }
        public HumBoneHandler BrowOuterR { get; }
        public HumBoneHandler[] ArrBrowOuters => new[] { BrowOuterL, BrowOuterR };
        public HumBoneHandler[] ArrBrowMids => new[] { BrowMidL, BrowMidR };
        public HumBoneHandler[] ArrBrowInners => new[] { BrowInnerL, BrowInnerR };
        //  cheeks:
        public HumBoneHandler CheekUpperL { get; }
        public HumBoneHandler CheekLowerL { get; }
        public HumBoneHandler CheekLowerR { get; }
        public HumBoneHandler CheekUpperR { get; }
        public HumBoneHandler NasolabialMouthCornerL { get; }
        public HumBoneHandler NasolabialMouthCornerR { get; }
        public HumBoneHandler LipBelowNoseL { get; }
        public HumBoneHandler LipNasolabialCreaseL { get; }
        public HumBoneHandler NasolabialMiddleL { get; }
        public HumBoneHandler NasolabialUpperL { get; }
        public HumBoneHandler NostrilL { get; }
        public HumBoneHandler SquintInnerL { get; }
        public HumBoneHandler SquintOuterL { get; }
        public HumBoneHandler JawClenchL { get; }
        public HumBoneHandler NasolabialLowerL { get; }
        public HumBoneHandler LipBelowNoseR { get; }
        public HumBoneHandler LipNasolabialCreaseR { get; }
        public HumBoneHandler NasolabialMiddleR { get; }
        public HumBoneHandler NasolabialUpperR { get; }
        public HumBoneHandler NostrilR { get; }
        public HumBoneHandler SquintInnerR { get; }
        public HumBoneHandler SquintOuterR { get; }
        public HumBoneHandler JawClenchR { get; }
        public HumBoneHandler NasolabialLowerR { get; }
        public HumBoneHandler[] ArrCheekUppers => new[] { CheekUpperL, CheekUpperR };
        public HumBoneHandler[] ArrCheekLowers => new[] { CheekLowerL, CheekLowerR };
        public HumBoneHandler[] ArrNasolabialMouthCorners => new[] { NasolabialMouthCornerL, NasolabialMouthCornerR };
        public HumBoneHandler[] ArrLipBelowNoses => new[] { LipBelowNoseL, LipBelowNoseR };
        public HumBoneHandler[] ArrLipNasolabialCreases => new[] { LipNasolabialCreaseL, LipNasolabialCreaseR };
        public HumBoneHandler[] ArrNasolabialMiddles => new[] { NasolabialMiddleL, NasolabialMiddleR };
        public HumBoneHandler[] ArrNasolabialUppers => new[] { NasolabialUpperL, NasolabialUpperR };
        public HumBoneHandler[] ArrNostrils => new[] { NostrilL, NostrilR };
        public HumBoneHandler[] ArrSquintInners => new[] { SquintInnerL, SquintInnerR };
        public HumBoneHandler[] ArrSquintOuters => new[] { SquintOuterL, SquintOuterR };
        public HumBoneHandler[] ArrJawClenchs => new[] { JawClenchL, JawClenchR };
        public HumBoneHandler[] ArrNasolabialLowers => new[] { NasolabialLowerL, NasolabialLowerR };


        public Mover<HumBoneHandler> MoveJaw => _moveJaw ?? (_moveJaw = new Mover<HumBoneHandler>(LowerJaw));
        public Mover<HumBoneHandler> MoveLowerFaceRig => _moveLowerFaceRig ?? (_moveLowerFaceRig = new Mover<HumBoneHandler>(LowerFaceRig));
        public Mover<HumBoneHandler> MoveUpperFaceRig => _moveUpperFaceRig ?? (_moveUpperFaceRig = new Mover<HumBoneHandler>(UpperFaceRig));
        // inner circle around the mouth:
        public Mover<HumBoneHandler> MoveLipCornerL => _moveLipCornerL ?? (_moveLipCornerL = new Mover<HumBoneHandler>(LipCornerL));
        public Mover<HumBoneHandler> MoveLipLowerOuterL => _moveLipLowerOuterL ?? (_moveLipLowerOuterL = new Mover<HumBoneHandler>(LipLowerOuterL));
        public Mover<HumBoneHandler> MoveLipLowerInnerL => _moveLipLowerInnerL ?? (_moveLipLowerInnerL = new Mover<HumBoneHandler>(LipLowerInnerL));
        public Mover<HumBoneHandler> MoveLipLowerMiddle => _moveLipLowerMiddle ?? (_moveLipLowerMiddle = new Mover<HumBoneHandler>(LipLowerMiddle));
        public Mover<HumBoneHandler> MoveLipLowerInnerR => _moveLipLowerInnerR ?? (_moveLipLowerInnerR = new Mover<HumBoneHandler>(LipLowerInnerR));
        public Mover<HumBoneHandler> MoveLipLowerOuterR => _moveLipLowerOuterR ?? (_moveLipLowerOuterR = new Mover<HumBoneHandler>(LipLowerOuterR));
        public Mover<HumBoneHandler> MoveLipCornerR => _moveLipCornerR ?? (_moveLipCornerR = new Mover<HumBoneHandler>(LipCornerR));
        public Mover<HumBoneHandler> MoveLipUpperOuterL => _moveLipUpperOuterL ?? (_moveLipUpperOuterL = new Mover<HumBoneHandler>(LipUpperOuterL));
        public Mover<HumBoneHandler> MoveLipUpperInnerL => _moveLipUpperInnerL ?? (_moveLipUpperInnerL = new Mover<HumBoneHandler>(LipUpperInnerL));
        public Mover<HumBoneHandler> MoveLipUpperMiddle => _moveLipUpperMiddle ?? (_moveLipUpperMiddle = new Mover<HumBoneHandler>(LipUpperMiddle));
        public Mover<HumBoneHandler> MoveLipUpperInnerR => _moveLipUpperInnerR ?? (_moveLipUpperInnerR = new Mover<HumBoneHandler>(LipUpperInnerR));
        public Mover<HumBoneHandler> MoveLipUpperOuterR => _moveLipUpperOuterR ?? (_moveLipUpperOuterR = new Mover<HumBoneHandler>(LipUpperOuterR));


        public Mover<HumBoneHandler> MoveLipBelowNoseR => _moveLipBelowNoseR ?? (_moveLipBelowNoseR = new Mover<HumBoneHandler>(LipBelowNoseR));
        public Mover<HumBoneHandler> MoveLipNasolabialCreaseR => _moveLipNasolabialCreaseR ?? (_moveLipNasolabialCreaseR = new Mover<HumBoneHandler>(LipNasolabialCreaseR));
        public Mover<HumBoneHandler> MoveNasolabialMiddleR => _moveNasolabialMiddleR ?? (_moveNasolabialMiddleR = new Mover<HumBoneHandler>(NasolabialMiddleR));
        public Mover<HumBoneHandler> MoveNasolabialUpperR => _moveNasolabialUpperR ?? (_moveNasolabialUpperR = new Mover<HumBoneHandler>(NasolabialUpperR));
        public Mover<HumBoneHandler> MoveNostrilR => _moveNostrilR ?? (_moveNostrilR = new Mover<HumBoneHandler>(NostrilR));
        public Mover<HumBoneHandler> MoveSquintInnerR => _moveSquintInnerR ?? (_moveSquintInnerR = new Mover<HumBoneHandler>(SquintInnerR));
        public Mover<HumBoneHandler> MoveSquintOuterR => _moveSquintOuterR ?? (_moveSquintOuterR = new Mover<HumBoneHandler>(SquintOuterR));
        public Mover<HumBoneHandler> MoveJawClenchR => _moveJawClenchR ?? (_moveJawClenchR = new Mover<HumBoneHandler>(JawClenchR));
        public Mover<HumBoneHandler> MoveNasolabialLowerR => _moveNasolabialLowerR ?? (_moveNasolabialLowerR = new Mover<HumBoneHandler>(NasolabialLowerR));
        public Mover<HumBoneHandler> MoveLipBelowNoseL => _moveLipBelowNoseL ?? (_moveLipBelowNoseL = new Mover<HumBoneHandler>(LipBelowNoseL));
        public Mover<HumBoneHandler> MoveLipNasolabialCreaseL => _moveLipNasolabialCreaseL ?? (_moveLipNasolabialCreaseL = new Mover<HumBoneHandler>(LipNasolabialCreaseL));
        public Mover<HumBoneHandler> MoveNasolabialMiddleL => _moveNasolabialMiddleL ?? (_moveNasolabialMiddleL = new Mover<HumBoneHandler>(NasolabialMiddleL));
        public Mover<HumBoneHandler> MoveNasolabialUpperL => _moveNasolabialUpperL ?? (_moveNasolabialUpperL = new Mover<HumBoneHandler>(NasolabialUpperL));
        public Mover<HumBoneHandler> MoveNostrilL => _moveNostrilL ?? (_moveNostrilL = new Mover<HumBoneHandler>(NostrilL));
        public Mover<HumBoneHandler> MoveSquintInnerL => _moveSquintInnerL ?? (_moveSquintInnerL = new Mover<HumBoneHandler>(SquintInnerL));
        public Mover<HumBoneHandler> MoveSquintOuterL => _moveSquintOuterL ?? (_moveSquintOuterL = new Mover<HumBoneHandler>(SquintOuterL));
        public Mover<HumBoneHandler> MoveJawClenchL => _moveJawClenchL ?? (_moveJawClenchL = new Mover<HumBoneHandler>(JawClenchL));
        public Mover<HumBoneHandler> MoveNasolabialLowerL => _moveNasolabialLowerL ?? (_moveNasolabialLowerL = new Mover<HumBoneHandler>(NasolabialLowerL));

        public Mover<HumBoneHandler>[] ArrPathLipCorners => new[] { MoveLipCornerL, MoveLipCornerR };
        public Mover<HumBoneHandler>[] ArrPathLipLowerOuters => new[] { MoveLipLowerOuterL, MoveLipLowerOuterR };
        public Mover<HumBoneHandler>[] ArrPathLipLowerInners => new[] { MoveLipLowerInnerL, MoveLipLowerInnerR };
        public Mover<HumBoneHandler>[] ArrPathLipUpperOuters => new[] { MoveLipUpperOuterL, MoveLipUpperOuterR };
        public Mover<HumBoneHandler>[] ArrPathLipUpperInners => new[] { MoveLipUpperInnerL, MoveLipUpperInnerR };
        public Mover<HumBoneHandler>[] ArrPathLipBelowNoses => new[] { MoveLipBelowNoseL, MoveLipBelowNoseR };
        public Mover<HumBoneHandler>[] ArrPathLipNasolabialCreases => new[] { MoveLipNasolabialCreaseL, MoveLipNasolabialCreaseR };
        public Mover<HumBoneHandler>[] ArrPathNasolabialMiddles => new[] { MoveNasolabialMiddleL, MoveNasolabialMiddleR };
        public Mover<HumBoneHandler>[] ArrPathNasolabialUppers => new[] { MoveNasolabialUpperL, MoveNasolabialUpperR };
        public Mover<HumBoneHandler>[] ArrPathNostrils => new[] { MoveNostrilL, MoveNostrilR };
        public Mover<HumBoneHandler>[] ArrPathSquintInners => new[] { MoveSquintInnerL, MoveSquintInnerR };
        public Mover<HumBoneHandler>[] ArrPathSquintOuters => new[] { MoveSquintOuterL, MoveSquintOuterR };
        public Mover<HumBoneHandler>[] ArrPathJawClenches => new[] { MoveJawClenchL, MoveJawClenchR };
        public Mover<HumBoneHandler>[] ArrPathNasolabialLowers => new[] { MoveNasolabialLowerL, MoveNasolabialLowerR };
        //internal HandlePath[] Arrs => new[] { L, R };

        // eyes:
        public Mover<HumBoneHandler> MoveEyeL => _moveEyeL ?? (_moveEyeL = new Mover<HumBoneHandler>(EyeL));
        public Mover<HumBoneHandler> MoveEyeR => _moveEyeR ?? (_moveEyeR = new Mover<HumBoneHandler>(EyeR));
        public Mover<HumBoneHandler>[] ArrPathEyes => new[] { MoveEyeL, MoveEyeR };


        // eyelids:
        public Mover<HumBoneHandler> MoveEyelidUpperInnerL => _moveEyelidUpperInnerL ?? (_moveEyelidUpperInnerL = new Mover<HumBoneHandler>(EyelidUpperInnerL));
        public Mover<HumBoneHandler> MoveEyelidUpperL => _moveEyelidUpperL ?? (_moveEyelidUpperL = new Mover<HumBoneHandler>(EyelidUpperL));
        public Mover<HumBoneHandler> MoveEyelidUpperOuterL => _moveEyelidUpperOuterL ?? (_moveEyelidUpperOuterL = new Mover<HumBoneHandler>(EyelidUpperOuterL));
        public Mover<HumBoneHandler> MoveEyelidLowerInnerL => _moveEyelidLowerInnerL ?? (_moveEyelidLowerInnerL = new Mover<HumBoneHandler>(EyelidLowerInnerL));
        public Mover<HumBoneHandler> MoveEyelidLowerL => _moveEyelidLowerL ?? (_moveEyelidLowerL = new Mover<HumBoneHandler>(EyelidLowerL));
        public Mover<HumBoneHandler> MoveEyelidLowerOuterL => _moveEyelidLowerOuterL ?? (_moveEyelidLowerOuterL = new Mover<HumBoneHandler>(EyelidLowerOuterL));
        public Mover<HumBoneHandler> MoveEyelidUpperInnerR => _moveEyelidUpperInnerR ?? (_moveEyelidUpperInnerR = new Mover<HumBoneHandler>(EyelidUpperInnerR));
        public Mover<HumBoneHandler> MoveEyelidUpperR => _moveEyelidUpperR ?? (_moveEyelidUpperR = new Mover<HumBoneHandler>(EyelidUpperR));
        public Mover<HumBoneHandler> MoveEyelidUpperOuterR => _moveEyelidUpperOuterR ?? (_moveEyelidUpperOuterR = new Mover<HumBoneHandler>(EyelidUpperOuterR));
        public Mover<HumBoneHandler> MoveEyelidLowerInnerR => _moveEyelidLowerInnerR ?? (_moveEyelidLowerInnerR = new Mover<HumBoneHandler>(EyelidLowerInnerR));
        public Mover<HumBoneHandler> MoveEyelidLowerR => _moveEyelidLowerR ?? (_moveEyelidLowerR = new Mover<HumBoneHandler>(EyelidLowerR));
        public Mover<HumBoneHandler> MoveEyelidLowerOuterR => _moveEyelidLowerOuterR ?? (_moveEyelidLowerOuterR = new Mover<HumBoneHandler>(EyelidLowerOuterR));
        public Mover<HumBoneHandler> MoveEyelidInnerL => _moveEyelidInnerL ?? (_moveEyelidInnerL = new Mover<HumBoneHandler>(EyelidInnerL));
        public Mover<HumBoneHandler> MoveEyelidOuterL => _moveEyelidOuterL ?? (_moveEyelidOuterL = new Mover<HumBoneHandler>(EyelidOuterL));
        public Mover<HumBoneHandler> MoveEyelidInnerR => _moveEyelidInnerR ?? (_moveEyelidInnerR = new Mover<HumBoneHandler>(EyelidInnerR));
        public Mover<HumBoneHandler> MoveEyelidOuterR => _moveEyelidOuterR ?? (_moveEyelidOuterR = new Mover<HumBoneHandler>(EyelidOuterR));


        public Mover<HumBoneHandler>[] ArrPathEyelidUpperInners => new[] { MoveEyelidUpperInnerL, MoveEyelidUpperInnerR };
        public Mover<HumBoneHandler>[] ArrPathEyelidUppers => new[] { MoveEyelidUpperL, MoveEyelidUpperR };
        public Mover<HumBoneHandler>[] ArrPathEyelidUpperOuters => new[] { MoveEyelidUpperOuterL, MoveEyelidUpperOuterR };
        public Mover<HumBoneHandler>[] ArrPathEyelidLowerInners => new[] { MoveEyelidLowerInnerL, MoveEyelidLowerInnerR };
        public Mover<HumBoneHandler>[] ArrPathEyelidLowers => new[] { MoveEyelidLowerL, MoveEyelidLowerR };
        public Mover<HumBoneHandler>[] ArrPathEyelidLowerOuters => new[] { MoveEyelidLowerOuterL, MoveEyelidLowerOuterR };
        public Mover<HumBoneHandler>[] ArrPathEyelidInners => new[] { MoveEyelidInnerL, MoveEyelidInnerR };
        public Mover<HumBoneHandler>[] ArrPathEyelidOuters => new[] { MoveEyelidOuterL, MoveEyelidOuterR };


        // eyebrows:
        public Mover<HumBoneHandler> MoveMidNoseBridge => _moveMidNoseBridge ?? (_moveMidNoseBridge = new Mover<HumBoneHandler>(MidNoseBridge));
        public Mover<HumBoneHandler> MoveBrowOuterL => _moveBrowOuterL ?? (_moveBrowOuterL = new Mover<HumBoneHandler>(BrowOuterL));
        public Mover<HumBoneHandler> MoveBrowMidL => _moveBrowMidL ?? (_moveBrowMidL = new Mover<HumBoneHandler>(BrowMidL));
        public Mover<HumBoneHandler> MoveBrowInnerL => _moveBrowInnerL ?? (_moveBrowInnerL = new Mover<HumBoneHandler>(BrowInnerL));
        public Mover<HumBoneHandler> MoveCenterBrow => _moveCenterBrow ?? (_moveCenterBrow = new Mover<HumBoneHandler>(CenterBrow));
        public Mover<HumBoneHandler> MoveBrowInnerR => _moveBrowInnerR ?? (_moveBrowInnerR = new Mover<HumBoneHandler>(BrowInnerR));
        public Mover<HumBoneHandler> MoveBrowMidR => _moveBrowMidR ?? (_moveBrowMidR = new Mover<HumBoneHandler>(BrowMidR));
        public Mover<HumBoneHandler> MoveBrowOuterR => _moveBrowOuterR ?? (_moveBrowOuterR = new Mover<HumBoneHandler>(BrowOuterR));
        public Mover<HumBoneHandler>[] ArrPathBrowOuters => new[] { MoveBrowOuterL, MoveBrowOuterR };
        public Mover<HumBoneHandler>[] ArrPathBrowMids => new[] { MoveBrowMidL, MoveBrowMidR };
        public Mover<HumBoneHandler>[] ArrPathBrowInners => new[] { MoveBrowInnerL, MoveBrowInnerR };

        //  cheeks:
        public Mover<HumBoneHandler> MoveCheekUpperL => _moveCheekUpperL ?? (_moveCheekUpperL = new Mover<HumBoneHandler>(CheekUpperL));
        public Mover<HumBoneHandler> MoveCheekLowerL => _moveCheekLowerL ?? (_moveCheekLowerL = new Mover<HumBoneHandler>(CheekLowerL));
        public Mover<HumBoneHandler> MoveCheekLowerR => _moveCheekLowerR ?? (_moveCheekLowerR = new Mover<HumBoneHandler>(CheekLowerR));
        public Mover<HumBoneHandler> MoveCheekUpperR => _moveCheekUpperR ?? (_moveCheekUpperR = new Mover<HumBoneHandler>(CheekUpperR));
        public Mover<HumBoneHandler> MoveNasolabialMouthCornerL => _moveNasolabialMouthCornerL ?? (_moveNasolabialMouthCornerL = new Mover<HumBoneHandler>(NasolabialMouthCornerL));
        public Mover<HumBoneHandler> MoveNasolabialMouthCornerR => _moveNasolabialMouthCornerR ?? (_moveNasolabialMouthCornerR = new Mover<HumBoneHandler>(NasolabialMouthCornerR));
        public Mover<HumBoneHandler>[] ArrPathCheekUppers => new[] { MoveCheekUpperL, MoveCheekUpperR };
        public Mover<HumBoneHandler>[] ArrPathCheekLowers => new[] { MoveCheekLowerL, MoveCheekLowerR };
        public Mover<HumBoneHandler>[] ArrPathNasolabialMouthCorners => new[] { MoveNasolabialMouthCornerL, MoveNasolabialMouthCornerR };
    }
}
