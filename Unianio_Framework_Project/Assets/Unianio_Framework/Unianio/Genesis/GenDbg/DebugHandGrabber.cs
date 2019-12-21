// TODO:enable
/*using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Animations.Common;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.Genesis.IK;
using Unianio.IK;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Genesis.GenDbg
{
    internal sealed class DebugHandGrabber
    {
        readonly IPlayerManager _player;
        IComplexHuman _human;
        Vector3 _relPivot, _absVecToPivot;
        IAnimation _ani;
        ControllerSide _ltGrab = ControllerSide.None;
        ControllerSide _rtGrab = ControllerSide.None;
        Transform _ltHandle, _rtHandle;
        static readonly Color deselectColor = Color.yellow.WithA(0.3);
        static readonly Color selectColor = Color.magenta.WithA(0.3);
        readonly IDictionary<ControllerSide, IAnimation> _animations = new Dictionary<ControllerSide, IAnimation>
        {
            {ControllerSide.None, get<FuncAni>().AsFinished() },
            {ControllerSide.Left, get<FuncAni>().AsFinished() },
            {ControllerSide.Right, get<FuncAni>().AsFinished() }
        };
        static readonly float[] _bendFactors = new[] {-1.00f, -0.75f, -0.50f, -0.25f, 0.00f, 0.25f, 0.50f, 0.75f, 1.00f};
        int _currBendFactorIndex = 4;
        internal DebugHandGrabber(IPlayerManager player)
        {
            _player = player;
        }
        internal DebugHandGrabber Set(IComplexHuman human)
        {
            _human = human;
            return Initialize();
        }
        DebugHandGrabber Initialize()
        {
            subscribe<BtnTriggerPress>(OnBtnTriggerPress, this);

            subscribe<BtnGripPress>(OnBtnGripPress, this);

            _ltHandle = meshes.CreatePointyCone(new DtCone { bottomRadius = 0.10, height = 0.05, relNoseLen = 2 })
                .SetStandardShaderTransparentColor(deselectColor)
                .transform;
            _rtHandle = meshes.CreatePointyCone(new DtCone { bottomRadius = 0.10, height = 0.05, relNoseLen = 2 })
                .SetStandardShaderTransparentColor(deselectColor)
                .transform;

            _ltHandle.rotation = _human.ArmL.Hand.rotation;
            _ltHandle.position = _human.ArmL.Hand.position;
            _ltHandle.SetParent(_human.ArmL.Hand);

            _rtHandle.rotation = _human.ArmR.Hand.rotation;
            _rtHandle.position = _human.ArmR.Hand.position;
            _rtHandle.SetParent(_human.ArmR.Hand);

            return this;
        }

        void OnBtnGripPress(BtnGripPress e)
        {
            if (e.IsPressed)
            {
                var bf = _bendFactors[(++_currBendFactorIndex)% _bendFactors.Length];
                _human.ArmL.ElbowBendFactor = _human.ArmR.ElbowBendFactor = bf;
            }
        }

        void OnBtnTriggerPress(BtnTriggerPress e)
        {
            if (_ltGrab == ControllerSide.None && e.IsPressed && e.Controller.position.DistanceTo(_human.ArmL.Hand.position) < 0.15)
            {
                _ltGrab = e.Side;
                _ltHandle.gameObject.SetStandardShaderTransparentColor(selectColor);
                OnGrabStarts(_ltGrab, e.Controller, _human.ArmL);
            }
            else if(_ltGrab != ControllerSide.None && !e.IsPressed)
            {
                _ltHandle.gameObject.SetStandardShaderTransparentColor(deselectColor);
                OnGrabStops(_ltGrab, _human.ArmL);
                _ltGrab = ControllerSide.None;
            }
            if (_rtGrab == ControllerSide.None && e.IsPressed && e.Controller.position.DistanceTo(_human.ArmR.Hand.position) < 0.15)
            {
                _rtGrab = e.Side;
                _rtHandle.gameObject.SetStandardShaderTransparentColor(selectColor);
                OnGrabStarts(_rtGrab, e.Controller, _human.ArmR);
            }
            else if (_rtGrab == e.Side && !e.IsPressed)
            {
                _rtHandle.gameObject.SetStandardShaderTransparentColor(deselectColor);
                OnGrabStops(_rtGrab, _human.ArmR);
                _rtGrab = ControllerSide.None;
            }
        }
        void OnGrabStarts(ControllerSide controllerSide, Transform controller, HumArmChain arm)
        {

            var rotDiff = arm.Handle.rotation.AsLocalRot(controller.rotation);
            var posDiff = (arm.Handle.position - controller.position).AsLocalVec(controller);

            _animations[controllerSide].StopIfRunning();
            _animations[controllerSide] = playEndless(a =>
                {
                    arm.Handle.rotation = controller.rotation * rotDiff;
                    arm.Handle.position = controller.position + posDiff.AsWorldVec(controller);
                });
        }
        void OnGrabStops(ControllerSide controllerSide, HumArmChain arm)
        {
            _animations[controllerSide].StopIfRunning();
        }
    }
}*/