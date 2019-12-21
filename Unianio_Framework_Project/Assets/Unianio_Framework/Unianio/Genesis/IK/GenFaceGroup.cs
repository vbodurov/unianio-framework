using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Graphs;
using Unianio.Human;
using Unianio.IK;
using Unianio.Rigged;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis.IK
{
    public sealed class GenFaceGroup : IBlinkController
    {
        readonly IComplexHuman _human;
        readonly HumanFaceConfig _config;

        HandlePath _pathLowerFaceRig, _pathUpperFaceRig,
            _pathJaw,
            // inner circle around the mouth:
            _pathLipCornerL, _pathLipLowerOuterL, _pathLipLowerInnerL, _pathLipLowerMiddle, _pathLipLowerInnerR, _pathLipLowerOuterR, _pathLipCornerR,
            _pathLipUpperOuterL, _pathLipUpperInnerL, _pathLipUpperMiddle, _pathLipUpperInnerR, _pathLipUpperOuterR,
            // eyes:
            _pathEyeL, _pathEyeR,
            // eyelids:
            _pathEyelidUpperInnerL, _pathEyelidUpperL, _pathEyelidUpperOuterL, _pathEyelidLowerInnerL, _pathEyelidLowerL, _pathEyelidLowerOuterL,
            _pathEyelidUpperInnerR, _pathEyelidUpperR, _pathEyelidUpperOuterR, _pathEyelidLowerInnerR, _pathEyelidLowerR, _pathEyelidLowerOuterR,
            _pathEyelidInnerL, _pathEyelidOuterL, _pathEyelidInnerR, _pathEyelidOuterR,
            // eyebrows:
            _pathMidNoseBridge, _pathBrowOuterL, _pathBrowMidL, _pathBrowInnerL, _pathCenterBrow, _pathBrowInnerR, _pathBrowMidR, _pathBrowOuterR,
            //  cheeks:
            _pathCheekUpperL, _pathCheekLowerL, _pathCheekLowerR, _pathCheekUpperR, _pathNasolabialMouthCornerL, _pathNasolabialMouthCornerR,
            _pathLipBelowNoseL, _pathLipNasolabialCreaseL, _pathNasolabialMiddleL, _pathNasolabialUpperL, _pathNostrilL, _pathSquintInnerL, 
            _pathSquintOuterL, _pathJawClenchL, _pathNasolabialLowerL, 
            _pathLipBelowNoseR, _pathLipNasolabialCreaseR, _pathNasolabialMiddleR, _pathNasolabialUpperR, _pathNostrilR, _pathSquintInnerR, 
            _pathSquintOuterR, _pathJawClenchR, _pathNasolabialLowerR
            ;
        float _blinkUpL, _blinkUpR, _blinkDnL, _blinkDnR, _eyeCurveShift;
        readonly NumericPath _pathBlink, _pathBlinkUpL, _pathBlinkUpR, _pathBlinkDnL, _pathBlinkDnR;
        (HandlePath path, Func<bool> condition, HumBoneHandler bone)[] _allBones;

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

            Initial = new GenFaceGroupInitialStats(this);

            _allBones = new[]
           {
                (path: PathLipCornerL, condition: (Func<bool>)null, bone: LipCornerL ),
                (path: PathLipLowerOuterL, condition: (Func<bool>)null, bone: LipLowerOuterL ),
                (path: PathLipLowerInnerL, condition: (Func<bool>)null, bone: LipLowerInnerL ),
                (path: PathLipLowerMiddle, condition: (Func<bool>)null, bone: LipLowerMiddle ),
                (path: PathLipLowerInnerR, condition: (Func<bool>)null, bone: LipLowerInnerR ),
                (path: PathLipLowerOuterR, condition: (Func<bool>)null, bone: LipLowerOuterR ),
                (path: PathLipCornerR, condition: (Func<bool>)null, bone: LipCornerR ),
                (path: PathLipUpperOuterL, condition: (Func<bool>)null, bone: LipUpperOuterL ),
                (path: PathLipUpperInnerL, condition: (Func<bool>)null, bone: LipUpperInnerL ),
                (path: PathLipUpperMiddle, condition: (Func<bool>)null, bone: LipUpperMiddle ),
                (path: PathLipUpperInnerR, condition: (Func<bool>)null, bone: LipUpperInnerR ),
                (path: PathLipUpperOuterR, condition: (Func<bool>)null, bone: LipUpperOuterR ),
                (path: PathEyelidUpperInnerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperInnerL),
                (path: PathEyelidUpperL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperL),
                (path: PathEyelidUpperOuterL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperOuterL),
                (path: PathEyelidLowerInnerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerInnerL),
                (path: PathEyelidLowerL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerL),
                (path: PathEyelidLowerOuterL, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerOuterL),
                (path: PathEyelidUpperInnerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperInnerR),
                (path: PathEyelidUpperR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperR),
                (path: PathEyelidUpperOuterR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidUpperOuterR),
                (path: PathEyelidLowerInnerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerInnerR),
                (path: PathEyelidLowerR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerR),
                (path: PathEyelidLowerOuterR, condition: () => h.AniBlink.IsNotRunning(), bone: EyelidLowerOuterR),
                (path: PathLipBelowNoseR, condition: (Func<bool>)null, bone: LipBelowNoseR ),
                (path: PathLipNasolabialCreaseR, condition: (Func<bool>)null, bone: LipNasolabialCreaseR ),
                (path: PathNasolabialMiddleR, condition: (Func<bool>)null, bone: NasolabialMiddleR ),
                (path: PathNasolabialUpperR, condition: (Func<bool>)null, bone: NasolabialUpperR ),
                (path: PathNostrilR, condition: (Func<bool>)null, bone: NostrilR ),
                (path: PathSquintInnerR, condition: (Func<bool>)null, bone: SquintInnerR ),
                (path: PathSquintOuterR, condition: (Func<bool>)null, bone: SquintOuterR ),
                (path: PathJawClenchR, condition: (Func<bool>)null, bone: JawClenchR ),
                (path: PathNasolabialLowerR, condition: (Func<bool>)null, bone: NasolabialLowerR ),
                (path: PathLipBelowNoseL, condition: (Func<bool>)null, bone: LipBelowNoseL ),
                (path: PathLipNasolabialCreaseL, condition: (Func<bool>)null, bone: LipNasolabialCreaseL ),
                (path: PathNasolabialMiddleL, condition: (Func<bool>)null, bone: NasolabialMiddleL ),
                (path: PathNasolabialUpperL, condition: (Func<bool>)null, bone: NasolabialUpperL ),
                (path: PathNostrilL, condition: (Func<bool>)null, bone: NostrilL ),
                (path: PathSquintInnerL, condition: (Func<bool>)null, bone: SquintInnerL ),
                (path: PathSquintOuterL, condition: (Func<bool>)null, bone: SquintOuterL ),
                (path: PathJawClenchL, condition: (Func<bool>)null, bone: JawClenchL ),
                (path: PathNasolabialLowerL, condition: (Func<bool>)null, bone: NasolabialLowerL ),
                (path: PathNasolabialMouthCornerL, condition: (Func<bool>)null, bone: NasolabialMouthCornerL ),
                (path: PathNasolabialMouthCornerR, condition: (Func<bool>)null, bone: NasolabialMouthCornerR ),
                (path: PathCheekUpperL, condition: (Func<bool>)null, bone: CheekUpperL ),
                (path: PathCheekUpperR, condition: (Func<bool>)null, bone: CheekUpperR ),
                (path: PathBrowInnerL, condition: (Func<bool>)null, bone: BrowInnerL ),
                (path: PathBrowInnerR, condition: (Func<bool>)null, bone: BrowInnerR ),
                (path: PathBrowMidL, condition: (Func<bool>)null,bone:BrowMidL ),
                (path: PathBrowMidR, condition: (Func<bool>)null, bone: BrowMidR ),
                (path: PathBrowOuterL, condition: (Func<bool>)null, bone: BrowOuterL ),
                (path: PathBrowOuterR, condition: (Func<bool>)null, bone: BrowOuterR ),
                (path: PathCenterBrow, condition: (Func<bool>)null, bone: CenterBrow ),
                (path: PathJaw, condition: () => h.AniJaw.IsNotRunning(), bone: LowerJaw ),
            };
        }
        public NumericPath PathEyeCurveShift => throw new NotImplementedException();
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

        public (HandlePath path, Func<bool> condition, HumBoneHandler bone)[] AllBones => _allBones;
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


        public HandlePath PathJaw => _pathJaw ?? (_pathJaw = new HandlePath(LowerJaw));
        public HandlePath PathLowerFaceRig => _pathLowerFaceRig ?? (_pathLowerFaceRig = new HandlePath(LowerFaceRig));
        public HandlePath PathUpperFaceRig => _pathUpperFaceRig ?? (_pathUpperFaceRig = new HandlePath(UpperFaceRig));
        // inner circle around the mouth:
        public HandlePath PathLipCornerL => _pathLipCornerL ?? (_pathLipCornerL = new HandlePath(LipCornerL));
        public HandlePath PathLipLowerOuterL => _pathLipLowerOuterL ?? (_pathLipLowerOuterL = new HandlePath(LipLowerOuterL));
        public HandlePath PathLipLowerInnerL => _pathLipLowerInnerL ?? (_pathLipLowerInnerL = new HandlePath(LipLowerInnerL));
        public HandlePath PathLipLowerMiddle => _pathLipLowerMiddle ?? (_pathLipLowerMiddle = new HandlePath(LipLowerMiddle));
        public HandlePath PathLipLowerInnerR => _pathLipLowerInnerR ?? (_pathLipLowerInnerR = new HandlePath(LipLowerInnerR));
        public HandlePath PathLipLowerOuterR => _pathLipLowerOuterR ?? (_pathLipLowerOuterR = new HandlePath(LipLowerOuterR));
        public HandlePath PathLipCornerR => _pathLipCornerR ?? (_pathLipCornerR = new HandlePath(LipCornerR));
        public HandlePath PathLipUpperOuterL => _pathLipUpperOuterL ?? (_pathLipUpperOuterL = new HandlePath(LipUpperOuterL));
        public HandlePath PathLipUpperInnerL => _pathLipUpperInnerL ?? (_pathLipUpperInnerL = new HandlePath(LipUpperInnerL));
        public HandlePath PathLipUpperMiddle => _pathLipUpperMiddle ?? (_pathLipUpperMiddle = new HandlePath(LipUpperMiddle));
        public HandlePath PathLipUpperInnerR => _pathLipUpperInnerR ?? (_pathLipUpperInnerR = new HandlePath(LipUpperInnerR));
        public HandlePath PathLipUpperOuterR => _pathLipUpperOuterR ?? (_pathLipUpperOuterR = new HandlePath(LipUpperOuterR));


        public HandlePath PathLipBelowNoseR => _pathLipBelowNoseR ?? (_pathLipBelowNoseR = new HandlePath(LipBelowNoseR));
        public HandlePath PathLipNasolabialCreaseR => _pathLipNasolabialCreaseR ?? (_pathLipNasolabialCreaseR = new HandlePath(LipNasolabialCreaseR));
        public HandlePath PathNasolabialMiddleR => _pathNasolabialMiddleR ?? (_pathNasolabialMiddleR = new HandlePath(NasolabialMiddleR));
        public HandlePath PathNasolabialUpperR => _pathNasolabialUpperR ?? (_pathNasolabialUpperR = new HandlePath(NasolabialUpperR));
        public HandlePath PathNostrilR => _pathNostrilR ?? (_pathNostrilR = new HandlePath(NostrilR));
        public HandlePath PathSquintInnerR => _pathSquintInnerR ?? (_pathSquintInnerR = new HandlePath(SquintInnerR));
        public HandlePath PathSquintOuterR => _pathSquintOuterR ?? (_pathSquintOuterR = new HandlePath(SquintOuterR));
        public HandlePath PathJawClenchR => _pathJawClenchR ?? (_pathJawClenchR = new HandlePath(JawClenchR));
        public HandlePath PathNasolabialLowerR => _pathNasolabialLowerR ?? (_pathNasolabialLowerR = new HandlePath(NasolabialLowerR));
        public HandlePath PathLipBelowNoseL => _pathLipBelowNoseL ?? (_pathLipBelowNoseL = new HandlePath(LipBelowNoseL));
        public HandlePath PathLipNasolabialCreaseL => _pathLipNasolabialCreaseL ?? (_pathLipNasolabialCreaseL = new HandlePath(LipNasolabialCreaseL));
        public HandlePath PathNasolabialMiddleL => _pathNasolabialMiddleL ?? (_pathNasolabialMiddleL = new HandlePath(NasolabialMiddleL));
        public HandlePath PathNasolabialUpperL => _pathNasolabialUpperL ?? (_pathNasolabialUpperL = new HandlePath(NasolabialUpperL));
        public HandlePath PathNostrilL => _pathNostrilL ?? (_pathNostrilL = new HandlePath(NostrilL));
        public HandlePath PathSquintInnerL => _pathSquintInnerL ?? (_pathSquintInnerL = new HandlePath(SquintInnerL));
        public HandlePath PathSquintOuterL => _pathSquintOuterL ?? (_pathSquintOuterL = new HandlePath(SquintOuterL));
        public HandlePath PathJawClenchL => _pathJawClenchL ?? (_pathJawClenchL = new HandlePath(JawClenchL));
        public HandlePath PathNasolabialLowerL => _pathNasolabialLowerL ?? (_pathNasolabialLowerL = new HandlePath(NasolabialLowerL));

        public HandlePath[] ArrPathLipCorners => new[] { PathLipCornerL, PathLipCornerR };
        public HandlePath[] ArrPathLipLowerOuters => new[] { PathLipLowerOuterL, PathLipLowerOuterR };
        public HandlePath[] ArrPathLipLowerInners => new[] { PathLipLowerInnerL, PathLipLowerInnerR };
        public HandlePath[] ArrPathLipUpperOuters => new[] { PathLipUpperOuterL, PathLipUpperOuterR };
        public HandlePath[] ArrPathLipUpperInners => new[] { PathLipUpperInnerL, PathLipUpperInnerR };
        public HandlePath[] ArrPathLipBelowNoses => new[] { PathLipBelowNoseL, PathLipBelowNoseR };
        public HandlePath[] ArrPathLipNasolabialCreases => new[] { PathLipNasolabialCreaseL, PathLipNasolabialCreaseR };
        public HandlePath[] ArrPathNasolabialMiddles => new[] { PathNasolabialMiddleL, PathNasolabialMiddleR };
        public HandlePath[] ArrPathNasolabialUppers => new[] { PathNasolabialUpperL, PathNasolabialUpperR };
        public HandlePath[] ArrPathNostrils => new[] { PathNostrilL, PathNostrilR };
        public HandlePath[] ArrPathSquintInners => new[] { PathSquintInnerL, PathSquintInnerR };
        public HandlePath[] ArrPathSquintOuters => new[] { PathSquintOuterL, PathSquintOuterR };
        public HandlePath[] ArrPathJawClenches => new[] { PathJawClenchL, PathJawClenchR };
        public HandlePath[] ArrPathNasolabialLowers => new[] { PathNasolabialLowerL, PathNasolabialLowerR };
        //internal HandlePath[] Arrs => new[] { L, R };

        // eyes:
        public HandlePath PathEyeL => _pathEyeL ?? (_pathEyeL = new HandlePath(EyeL));
        public HandlePath PathEyeR => _pathEyeR ?? (_pathEyeR = new HandlePath(EyeR));
        public HandlePath[] ArrPathEyes => new[] { PathEyeL, PathEyeR };


        // eyelids:
        public HandlePath PathEyelidUpperInnerL => _pathEyelidUpperInnerL ?? (_pathEyelidUpperInnerL = new HandlePath(EyelidUpperInnerL));
        public HandlePath PathEyelidUpperL => _pathEyelidUpperL ?? (_pathEyelidUpperL = new HandlePath(EyelidUpperL));
        public HandlePath PathEyelidUpperOuterL => _pathEyelidUpperOuterL ?? (_pathEyelidUpperOuterL = new HandlePath(EyelidUpperOuterL));
        public HandlePath PathEyelidLowerInnerL => _pathEyelidLowerInnerL ?? (_pathEyelidLowerInnerL = new HandlePath(EyelidLowerInnerL));
        public HandlePath PathEyelidLowerL => _pathEyelidLowerL ?? (_pathEyelidLowerL = new HandlePath(EyelidLowerL));
        public HandlePath PathEyelidLowerOuterL => _pathEyelidLowerOuterL ?? (_pathEyelidLowerOuterL = new HandlePath(EyelidLowerOuterL));
        public HandlePath PathEyelidUpperInnerR => _pathEyelidUpperInnerR ?? (_pathEyelidUpperInnerR = new HandlePath(EyelidUpperInnerR));
        public HandlePath PathEyelidUpperR => _pathEyelidUpperR ?? (_pathEyelidUpperR = new HandlePath(EyelidUpperR));
        public HandlePath PathEyelidUpperOuterR => _pathEyelidUpperOuterR ?? (_pathEyelidUpperOuterR = new HandlePath(EyelidUpperOuterR));
        public HandlePath PathEyelidLowerInnerR => _pathEyelidLowerInnerR ?? (_pathEyelidLowerInnerR = new HandlePath(EyelidLowerInnerR));
        public HandlePath PathEyelidLowerR => _pathEyelidLowerR ?? (_pathEyelidLowerR = new HandlePath(EyelidLowerR));
        public HandlePath PathEyelidLowerOuterR => _pathEyelidLowerOuterR ?? (_pathEyelidLowerOuterR = new HandlePath(EyelidLowerOuterR));
        public HandlePath PathEyelidInnerL => _pathEyelidInnerL ?? (_pathEyelidInnerL = new HandlePath(EyelidInnerL));
        public HandlePath PathEyelidOuterL => _pathEyelidOuterL ?? (_pathEyelidOuterL = new HandlePath(EyelidOuterL));
        public HandlePath PathEyelidInnerR => _pathEyelidInnerR ?? (_pathEyelidInnerR = new HandlePath(EyelidInnerR));
        public HandlePath PathEyelidOuterR => _pathEyelidOuterR ?? (_pathEyelidOuterR = new HandlePath(EyelidOuterR));


        public HandlePath[] ArrPathEyelidUpperInners => new[] { PathEyelidUpperInnerL, PathEyelidUpperInnerR };
        public HandlePath[] ArrPathEyelidUppers => new[] { PathEyelidUpperL, PathEyelidUpperR };
        public HandlePath[] ArrPathEyelidUpperOuters => new[] { PathEyelidUpperOuterL, PathEyelidUpperOuterR };
        public HandlePath[] ArrPathEyelidLowerInners => new[] { PathEyelidLowerInnerL, PathEyelidLowerInnerR };
        public HandlePath[] ArrPathEyelidLowers => new[] { PathEyelidLowerL, PathEyelidLowerR };
        public HandlePath[] ArrPathEyelidLowerOuters => new[] { PathEyelidLowerOuterL, PathEyelidLowerOuterR };
        public HandlePath[] ArrPathEyelidInners => new[] { PathEyelidInnerL, PathEyelidInnerR };
        public HandlePath[] ArrPathEyelidOuters => new[] { PathEyelidOuterL, PathEyelidOuterR };


        // eyebrows:
        public HandlePath PathMidNoseBridge => _pathMidNoseBridge ?? (_pathMidNoseBridge = new HandlePath(MidNoseBridge));
        public HandlePath PathBrowOuterL => _pathBrowOuterL ?? (_pathBrowOuterL = new HandlePath(BrowOuterL));
        public HandlePath PathBrowMidL => _pathBrowMidL ?? (_pathBrowMidL = new HandlePath(BrowMidL));
        public HandlePath PathBrowInnerL => _pathBrowInnerL ?? (_pathBrowInnerL = new HandlePath(BrowInnerL));
        public HandlePath PathCenterBrow => _pathCenterBrow ?? (_pathCenterBrow = new HandlePath(CenterBrow));
        public HandlePath PathBrowInnerR => _pathBrowInnerR ?? (_pathBrowInnerR = new HandlePath(BrowInnerR));
        public HandlePath PathBrowMidR => _pathBrowMidR ?? (_pathBrowMidR = new HandlePath(BrowMidR));
        public HandlePath PathBrowOuterR => _pathBrowOuterR ?? (_pathBrowOuterR = new HandlePath(BrowOuterR));
        public HandlePath[] ArrPathBrowOuters => new[] { PathBrowOuterL, PathBrowOuterR };
        public HandlePath[] ArrPathBrowMids => new[] { PathBrowMidL, PathBrowMidR };
        public HandlePath[] ArrPathBrowInners => new[] { PathBrowInnerL, PathBrowInnerR };

        //  cheeks:
        public HandlePath PathCheekUpperL => _pathCheekUpperL ?? (_pathCheekUpperL = new HandlePath(CheekUpperL));
        public HandlePath PathCheekLowerL => _pathCheekLowerL ?? (_pathCheekLowerL = new HandlePath(CheekLowerL));
        public HandlePath PathCheekLowerR => _pathCheekLowerR ?? (_pathCheekLowerR = new HandlePath(CheekLowerR));
        public HandlePath PathCheekUpperR => _pathCheekUpperR ?? (_pathCheekUpperR = new HandlePath(CheekUpperR));
        public HandlePath PathNasolabialMouthCornerL => _pathNasolabialMouthCornerL ?? (_pathNasolabialMouthCornerL = new HandlePath(NasolabialMouthCornerL));
        public HandlePath PathNasolabialMouthCornerR => _pathNasolabialMouthCornerR ?? (_pathNasolabialMouthCornerR = new HandlePath(NasolabialMouthCornerR));
        public HandlePath[] ArrPathCheekUppers => new[] { PathCheekUpperL, PathCheekUpperR };
        public HandlePath[] ArrPathCheekLowers => new[] { PathCheekLowerL, PathCheekLowerR };
        public HandlePath[] ArrPathNasolabialMouthCorners => new[] { PathNasolabialMouthCornerL, PathNasolabialMouthCornerR };
    }
}
