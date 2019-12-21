using System;
using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Extensions;
using Unianio.Genesis.State;
using Unianio.Graphs;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis
{
    public class GenesisFaceAni : AnimationManager
    {
        double _seconds;
        IComplexHuman _human;
        SetGenesisFace _face;
        int _order;
        public GenesisFaceAni Set(double seconds, IComplexHuman human, SetGenesisFace face, int order = 0)
        {
//dbg.log("Set GenFace ",seconds,Label,Time.time);
            _seconds = seconds;
            _human = human;
            if(!human.IsGenesis8()) throw new ArgumentException($"Human {human.Persona} is not genesis 8 it is {human.Definition.HumanoidType}");
            _face = face;
            _order = order;
            return this.AsUniqueNamed(unique.Face + human.ID + "-" + order);
        }
        public override void Initialize()
        {
//if(_order == 0) dbg.log(_seconds, Time.time, Label);

            var paths = new List<IExecutorOfProgress>();

            var nan = v3.nan;
            var vLipCornerL = nan; var vLipCornerR = nan;
            var vNasolabialLowerL = nan; var vNasolabialLowerR = nan;
            var vNasolabialMouthCornerL = nan; var vNasolabialMouthCornerR = nan;
            var vCheekLowerL = nan; var vCheekLowerR = nan;
            var vLipUpperMiddle = nan; var vLipLowerMiddle = nan;
            var vLipUpperInnerL = nan; var vLipUpperInnerR = nan;
            var vLipUpperOuterL = nan; var vLipUpperOuterR = nan;
            var vLipLowerInnerL = nan; var vLipLowerInnerR = nan;
            var vLipLowerOuterL = nan; var vLipLowerOuterR = nan;
            var vCenterBrow = nan;
            var vBrowInnerL = nan; var vBrowInnerR = nan;
            var vBrowMidL = nan; var vBrowMidR = nan;
            var vBrowOuterL = nan; var vBrowOuterR = nan;

            var vCheekUpperL = nan; var vCheekUpperR = nan;
            var vSquintOuterL = nan; var vSquintOuterR = nan;

            var h = _human;

            if (_face.Jaw != null)
            {
                var val01 = _face.Jaw.Value;
                h.PathJaw.New
                    .LocalRotToTarget(h.GenFace.LowerJaw.IniLocalFw.RotateAbout(v3.rt, 30 * val01), v3.up, _face.FuncJaw).AddTo(paths)
                    ;

                AddToV3(ref vLipCornerL, v3.up.By(0.03 * val01));
                AddToV3(ref vLipCornerR, v3.up.By(0.03 * val01));
                AddToV3(ref vNasolabialLowerL, v3.up.By(0.02 * val01));
                AddToV3(ref vNasolabialLowerR, v3.up.By(0.02 * val01));
                AddToV3(ref vNasolabialMouthCornerL, v3.up.By(0.03 * val01));
                AddToV3(ref vNasolabialMouthCornerR, v3.up.By(0.03 * val01));
                AddToV3(ref vCheekLowerL, v3.up.By(0.02 * val01));
                AddToV3(ref vCheekLowerR, v3.up.By(0.02 * val01));
            }
            if (_face.MouthCenter != null)
            {
                var val01 = _face.MouthCenter.Value;

                AddToV3(ref vLipUpperMiddle, v3.up.By(0.0085 * val01));
                AddToV3(ref vLipLowerMiddle, v3.dn.By(0.0085 * val01));
            }
            if (_face.MouthStretch != null)
            {
                var val01 = _face.MouthStretch.Value;

                AddToV3(ref vLipCornerL, v3.lt.By(0.01 * val01) + v3.bk.By(0.01 * val01));
                AddToV3(ref vLipUpperOuterL, v3.lt.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipUpperInnerL, v3.lt.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterL, v3.lt.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipLowerInnerL, v3.lt.By(0.005 * val01));
                AddToV3(ref vNasolabialMouthCornerL, v3.lt.By(0.005 * val01) + v3.bk.By(0.02 * val01));
                
                AddToV3(ref vLipCornerR, v3.rt.By(0.01 * val01) + v3.bk.By(0.01 * val01));
                AddToV3(ref vLipUpperOuterR, v3.rt.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipUpperInnerR, v3.rt.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterR, v3.rt.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipLowerInnerR, v3.rt.By(0.005 * val01));
                AddToV3(ref vNasolabialMouthCornerR, v3.rt.By(0.005 * val01) + v3.bk.By(0.02 * val01));
            }
            if (_face.MouthSmile != null)
            {
                var val01 = _face.MouthSmile.Value;

                AddToV3(ref vLipCornerL, v3.up.By(0.009 * val01) + v3.bk.By(0.01 * val01));
                AddToV3(ref vLipUpperOuterL, v3.up.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipUpperInnerL, v3.up.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterL, v3.up.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipLowerInnerL, v3.up.By(0.005 * val01));
                AddToV3(ref vNasolabialMouthCornerL, v3.up.By(0.005 * val01) + v3.bk.By(0.02 * val01));

                AddToV3(ref vLipUpperMiddle, v3.up.By(0.007 * val01));
                AddToV3(ref vLipLowerMiddle, v3.dn.By(0.002 * val01));

                AddToV3(ref vLipCornerR, v3.up.By(0.009 * val01) + v3.bk.By(0.01 * val01));
                AddToV3(ref vLipUpperOuterR, v3.up.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipUpperInnerR, v3.up.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterR, v3.up.By(0.008 * val01) + v3.bk.By(0.005 * val01));
                AddToV3(ref vLipLowerInnerR, v3.up.By(0.005 * val01));
                AddToV3(ref vNasolabialMouthCornerR, v3.up.By(0.005 * val01) + v3.bk.By(0.02 * val01));
            }
            if (_face.MouthSides != null)
            {
                var val01 = _face.MouthSides.Value;

                AddToV3(ref vLipUpperOuterL, v3.up.By(0.008 * val01));
                AddToV3(ref vLipUpperInnerL, v3.up.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterL, v3.dn.By(0.008 * val01));
                AddToV3(ref vLipLowerInnerL, v3.dn.By(0.005 * val01));

                AddToV3(ref vLipUpperOuterR, v3.up.By(0.008 * val01));
                AddToV3(ref vLipUpperInnerR, v3.up.By(0.005 * val01));
                AddToV3(ref vLipLowerOuterR, v3.dn.By(0.008 * val01));
                AddToV3(ref vLipLowerInnerR, v3.dn.By(0.005 * val01));
            }
            if (_face.EyebrowsInner != null)
            {
                var val01 = _face.EyebrowsInner.Value;

                AddToV3(ref vCenterBrow, v3.up.By(0.01 * val01));
                AddToV3(ref vBrowInnerL, v3.up.By(0.005 * val01));
                AddToV3(ref vBrowInnerR, v3.up.By(0.005 * val01));
            }
            if (_face.EyebrowsMiddle != null)
            {
                var val01 = _face.EyebrowsMiddle.Value;

                AddToV3(ref vBrowMidL, v3.up.By(0.01 * val01));
                AddToV3(ref vBrowMidR, v3.up.By(0.01 * val01));
            }
            if (_face.EyebrowsOuter != null)
            {
                var val01 = _face.EyebrowsOuter.Value;

                AddToV3(ref vBrowOuterL, v3.up.By(0.01 * val01));
                AddToV3(ref vBrowOuterR, v3.up.By(0.01 * val01));
            }
            if (_face.Cheeks != null)
            {
                var val01 = _face.Cheeks.Value;

                AddToV3(ref vCheekUpperR, v3.up.By(0.01 * val01));
                AddToV3(ref vSquintOuterR, v3.up.By(0.006 * val01));

                AddToV3(ref vCheekUpperL, v3.up.By(0.01 * val01));
                AddToV3(ref vSquintOuterL, v3.up.By(0.006 * val01));
            }
            if (_face.EyesLo != null || _face.EyesUp != null)
            {
                var valLo01 = _face.EyesLo ?? 0;
                var valUp01 = _face.EyesUp ?? 0;


                h.GenFace.PathBlinkUpL.AddTo(paths)
                    .New.ToTarget(valUp01).SetCondition(() => h.AniBlink.IsNotRunning())
                    ;
                h.GenFace.PathBlinkUpR.AddTo(paths)
                    .New.ToTarget(valUp01).SetCondition(() => h.AniBlink.IsNotRunning())
                    ;
                h.GenFace.PathBlinkDnL.AddTo(paths)
                    .New.ToTarget(valLo01).SetCondition(() => h.AniBlink.IsNotRunning())
                    ;
                h.GenFace.PathBlinkDnR.AddTo(paths)
                    .New.ToTarget(valLo01).SetCondition(() => h.AniBlink.IsNotRunning())
                    ;
            }
            if (_face.Nose != null)
            {
                var val01 = _face.Nose.Value;

                var upRitL = h.GenFace.NostrilL.IniLocalUp.RotateTowards(v3.rt, 15 * val01);
                h.GenFace.PathNostrilL.AddTo(paths).New
                    .LocalRotToTarget(h.GenFace.NostrilL.IniLocalFw, upRitL, _face.FuncNose)
                    .LocalPosLineToTarget(h.GenFace.NostrilL.IniLocalPos + v3.up.By(0.005 * val01), _face.FuncNose)
                    ;

                var upRitR = h.GenFace.NostrilR.IniLocalUp.RotateTowards(v3.lt, 15 * val01);
                h.GenFace.PathNostrilR.AddTo(paths).New
                    .LocalRotToTarget(h.GenFace.NostrilR.IniLocalFw, upRitR, _face.FuncNose)
                    .LocalPosLineToTarget(h.GenFace.NostrilR.IniLocalPos + v3.up.By(0.005 * val01), _face.FuncNose)
                    ;

                h.GenFace.PathMidNoseBridge.AddTo(paths).New
                    .LocalPosLineToTarget(h.GenFace.MidNoseBridge.IniLocalPos + v3.up.By(0.01 * val01), _face.FuncNose);
            }

            if (_face.Tongue != null)
            {
                var val01 = _face.Tongue.Value;
                h.PathTongue.AddTo(paths).New
                    .LocalRotToTarget(h.Tongue.IniLocalFw.RotateTowards(v3.up, 20 * val01), h.Tongue.IniLocalUp, _face.FuncTongue);
            }


            if (vLipCornerL.IsNotNan())
                h.GenFace.PathLipCornerL.New
                    .LocalPosLineToTarget(h.GenFace.LipCornerL.IniLocalPos + vLipCornerL).AddTo(paths);

            if (vLipCornerR.IsNotNan())
                h.GenFace.PathLipCornerR.New
                    .LocalPosLineToTarget(h.GenFace.LipCornerR.IniLocalPos + vLipCornerR).AddTo(paths);

            if (vLipUpperOuterL.IsNotNan())
                h.GenFace.PathLipUpperOuterL.New
                    .LocalPosLineToTarget(h.GenFace.LipUpperOuterL.IniLocalPos + vLipUpperOuterL).AddTo(paths);

            if (vLipUpperOuterR.IsNotNan())
                h.GenFace.PathLipUpperOuterR.New
                    .LocalPosLineToTarget(h.GenFace.LipUpperOuterR.IniLocalPos + vLipUpperOuterR).AddTo(paths);

            if (vLipUpperInnerL.IsNotNan())
                h.GenFace.PathLipUpperInnerL.New
                    .LocalPosLineToTarget(h.GenFace.LipUpperInnerL.IniLocalPos + vLipUpperInnerL).AddTo(paths);

            if (vLipUpperInnerR.IsNotNan())
                h.GenFace.PathLipUpperInnerR.New
                    .LocalPosLineToTarget(h.GenFace.LipUpperInnerR.IniLocalPos + vLipUpperInnerR).AddTo(paths);

            if (vLipLowerOuterL.IsNotNan())
                h.GenFace.PathLipLowerOuterL.New
                    .LocalPosLineToTarget(h.GenFace.LipLowerOuterL.IniLocalPos + vLipLowerOuterL).AddTo(paths);

            if (vLipLowerOuterR.IsNotNan())
                h.GenFace.PathLipLowerOuterR.New
                    .LocalPosLineToTarget(h.GenFace.LipLowerOuterR.IniLocalPos + vLipLowerOuterR).AddTo(paths);

            if (vLipLowerInnerL.IsNotNan())
                h.GenFace.PathLipLowerInnerL.New
                    .LocalPosLineToTarget(h.GenFace.LipLowerInnerL.IniLocalPos + vLipLowerInnerL).AddTo(paths);

            if (vLipLowerInnerR.IsNotNan())
                h.GenFace.PathLipLowerInnerR.New
                    .LocalPosLineToTarget(h.GenFace.LipLowerInnerR.IniLocalPos + vLipLowerInnerR).AddTo(paths);



            if (vNasolabialLowerL.IsNotNan())
                h.GenFace.PathNasolabialLowerL.New
                    .LocalPosLineToTarget(h.GenFace.NasolabialLowerL.IniLocalPos + vNasolabialLowerL).AddTo(paths);

            if (vNasolabialLowerR.IsNotNan())
                h.GenFace.PathNasolabialLowerR.New
                    .LocalPosLineToTarget(h.GenFace.NasolabialLowerR.IniLocalPos + vNasolabialLowerR).AddTo(paths);

            if(vNasolabialMouthCornerL.IsNotNan())
                h.GenFace.PathNasolabialMouthCornerL.New
                    .LocalPosLineToTarget(h.GenFace.NasolabialMouthCornerL.IniLocalPos + vNasolabialMouthCornerL).AddTo(paths);

            if (vNasolabialMouthCornerR.IsNotNan())
                h.GenFace.PathNasolabialMouthCornerR.New
                    .LocalPosLineToTarget(h.GenFace.NasolabialMouthCornerR.IniLocalPos + vNasolabialMouthCornerR).AddTo(paths);

            if (vCheekLowerL.IsNotNan())
                h.GenFace.PathCheekLowerL.New
                    .LocalPosLineToTarget(h.GenFace.CheekLowerL.IniLocalPos + vCheekLowerL).AddTo(paths);

            if (vCheekLowerR.IsNotNan())
                h.GenFace.PathCheekLowerR.New
                    .LocalPosLineToTarget(h.GenFace.CheekLowerR.IniLocalPos + vCheekLowerR).AddTo(paths);

            if (vLipUpperMiddle.IsNotNan())
                h.GenFace.PathLipUpperMiddle.New
                    .LocalPosLineToTarget(h.GenFace.LipUpperMiddle.IniLocalPos + vLipUpperMiddle).AddTo(paths);

            if (vLipLowerMiddle.IsNotNan())
                h.GenFace.PathLipLowerMiddle.New
                    .LocalPosLineToTarget(h.GenFace.LipLowerMiddle.IniLocalPos + vLipLowerMiddle).AddTo(paths);

            if (vCenterBrow.IsNotNan())
                h.GenFace.PathCenterBrow.New
                    .LocalPosLineToTarget(h.GenFace.CenterBrow.IniLocalPos + vCenterBrow).AddTo(paths);

            if (vBrowInnerL.IsNotNan())
                h.GenFace.PathBrowInnerL.New
                    .LocalPosLineToTarget(h.GenFace.BrowInnerL.IniLocalPos + vBrowInnerL).AddTo(paths);

            if (vBrowInnerR.IsNotNan())
                h.GenFace.PathBrowInnerR.New
                    .LocalPosLineToTarget(h.GenFace.BrowInnerR.IniLocalPos + vBrowInnerR).AddTo(paths);

            if (vBrowMidL.IsNotNan())
                h.GenFace.PathBrowMidL.New
                    .LocalPosLineToTarget(h.GenFace.BrowMidL.IniLocalPos + vBrowMidL).AddTo(paths);

            if (vBrowMidR.IsNotNan())
                h.GenFace.PathBrowMidR.New
                    .LocalPosLineToTarget(h.GenFace.BrowMidR.IniLocalPos + vBrowMidR).AddTo(paths);

            if (vBrowOuterL.IsNotNan())
                h.GenFace.PathBrowOuterL.New
                    .LocalPosLineToTarget(h.GenFace.BrowOuterL.IniLocalPos + vBrowOuterL).AddTo(paths);

            if (vBrowOuterR.IsNotNan())
                h.GenFace.PathBrowOuterR.New
                    .LocalPosLineToTarget(h.GenFace.BrowOuterR.IniLocalPos + vBrowOuterR).AddTo(paths);

            if (vCheekUpperL.IsNotNan())
                h.GenFace.PathCheekUpperL.New
                    .LocalPosLineToTarget(h.GenFace.CheekUpperL.IniLocalPos + vCheekUpperL).AddTo(paths);

            if (vCheekUpperR.IsNotNan())
                h.GenFace.PathCheekUpperR.New
                    .LocalPosLineToTarget(h.GenFace.CheekUpperR.IniLocalPos + vCheekUpperR).AddTo(paths);

            if (vSquintOuterL.IsNotNan())
                h.GenFace.PathSquintOuterL.New
                    .LocalPosLineToTarget(h.GenFace.SquintOuterL.IniLocalPos + vSquintOuterL).AddTo(paths);

            if (vSquintOuterR.IsNotNan())
                h.GenFace.PathSquintOuterR.New
                    .LocalPosLineToTarget(h.GenFace.SquintOuterR.IniLocalPos + vSquintOuterR).AddTo(paths);

            StartFuncAni(_seconds, x =>
                {
                    if (_face.Func != null) x = _face.Func(x).Float();
                    for (var i = 0; i < paths.Count; i++)
                    {
                        paths[i].Apply(x);
                    }
                })
                .Then(Finish);
        }

        void AddToV3(ref Vector3 vec, in Vector3 toAdd)
        {
            if (vec.IsNan()) vec = toAdd;
            else vec = vec + toAdd;
        }
    }
}