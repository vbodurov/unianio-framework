//TODO:enable
/*using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Genesis;
using Unianio.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Graphs
{
    public class FacePath : IExecutorOfProgress
    {
        const float MaxNeckTurnAngle = 60;

        readonly bool _isGenesis8;
        readonly IComplexHuman _human;
        Func<bool> _canApply;
        HandleSpace _space = HandleSpace.World;
        Quaternion _iniNeckLower, _iniNeckUpper;
        Vector3 _faceTarget, _lookAtTarget, _up, _iniUpDir;
        bool _hasFaceTarget, _hasLookTarget;
        

        public FacePath(IComplexHuman human)
        {
            _human = human;
            _isGenesis8 = _human.IsGenesis8();
        }
        public int ID { get; set; }
        public FacePath SetCondition(Func<bool> canApply)
        {
            _canApply = canApply;
            return this;
        }
        public FacePath InWorldSpace()
        {
            _space = HandleSpace.World;
            _iniNeckLower = _human.Spine.NeckLower.rotation;
            _iniNeckUpper = _human.NeckUpper.rotation;
            _iniUpDir = _human.NeckUpper.up;
            _hasFaceTarget = _hasLookTarget = false;
            _up = v3.up;
            return this;
        }
        public FacePath InModelSpace()
        {
            _space = HandleSpace.Model;
            _iniNeckLower = lookAt(_human.Spine.NeckLower.forward.AsLocalDir(_human), _human.Spine.NeckLower.up.AsLocalDir(_human));
            _iniNeckUpper = lookAt(_human.NeckUpper.forward.AsLocalDir(_human), _human.NeckUpper.up.AsLocalDir(_human));
            _iniUpDir = _human.NeckUpper.up;
            _hasFaceTarget = _hasLookTarget = false;
            _up = v3.up;
            return this;
        }
        public FacePath WithUp(in Vector3 up)
        {
            _up = up;
            return this;
        }
        public FacePath WithUpIf(in Vector3 up, bool condition)
        {
            if(condition) _up = up;
            return this;
        }
        public FacePath ToFace(in Vector3 faceTarget)
        {
            _faceTarget = faceTarget;
            _hasFaceTarget = true;
            return this;
        }
        public FacePath ToLookAt(in Vector3 lookAtTarget)
        {
            _lookAtTarget = lookAtTarget;
            _hasLookTarget = true;
            return this;
        }
        public FacePath GlanceAroundTarget(in Vector3 target, float time, double speed = 2, double howWide = 1.8)
        {
            return ToLookAt(target + _human.Head.lt.By(0.5 * howWide * sign(noise(time / speed))));
        }
        public FacePath GlanceAroundTargetIf(in Vector3 target, float time, bool condition, double speed = 2, double howWide = 1.8)
        {
            if(condition) ToLookAt(target + _human.Head.lt.By(0.5 * howWide * sign(noise(time / speed))));
            return this;
        }
        public FacePath ToLookAtIf(in Vector3 lookAtTarget, bool condition)
        {
            _lookAtTarget = lookAtTarget;
            _hasLookTarget = condition;
            return this;
        }
        public FacePath ShakeHeadHorz(float x, float headShakeMaxAngle = 5, float headShakeSpeed = 2)
        {
            //var angle = headShakeMaxAngle * pow(sin(pow(x, 0.5) * PI), 2) * sin((_time.time - _t0) * headShakeSpeed);
            var angle = headShakeMaxAngle * sin(x * PI * headShakeSpeed);// * sqrt(1 - pow(1 - 2 * (x % 1), 2));
            var turn = Quaternion.AngleAxis(angle, _human.Head.up);
            _human.Spine.NeckLower.rotation = turn * _human.Spine.NeckLower.rotation;
            _human.NeckUpper.rotation = turn * _human.NeckUpper.rotation;
            ApplyLook();
            return this;
        }
        public FacePath ShakeHeadVert(float x, float headShakeMaxAngle = 5, float headShakeSpeed = 2)
        {
            //            var angle = headShakeMaxAngle * pow(sin(pow(x, 0.5) * PI), 2) * sin((_time.time - _t0) * headShakeSpeed);
            var angle = headShakeMaxAngle * sin(x * PI * headShakeSpeed);// * sqrt(1 - pow(1 - 2 * (x % 1), 2));
            var turn = Quaternion.AngleAxis(angle, _human.Head.right);
            _human.Spine.NeckLower.rotation = turn * _human.Spine.NeckLower.rotation;
            _human.NeckUpper.rotation = turn * _human.NeckUpper.rotation;
            ApplyLook();
            return this;
        }
        public FacePath ApplyAnd(double progress)
        {
            Apply(progress, null);
            return this;
        }
        public void Apply(double progress)
        {
            Apply(progress, null);
        }
        public void Apply(double progress, Func<double, double> func)
        {
            if (_canApply != null && !_canApply()) return;
            
            ApplyFacing(progress, func);
            ApplyLook();
        }

        void ApplyFacing(double progress, Func<double, double> func)
        {
            var x = func?.Invoke(progress) ?? progress;

            var upDir = _up;
            var faceTarget = _faceTarget;
            var iniNeckLower = _iniNeckLower;
            var iniNeckUpper = _iniNeckUpper;
            var iniUpDir = _iniUpDir;

            if (_space == HandleSpace.Model)
            {
                if (_hasFaceTarget)
                {
                    upDir = upDir.AsWorldDir(_human);
                    faceTarget = faceTarget.AsWorldPoint(_human);
                    iniNeckLower = lookAt(
                        (_iniNeckLower * v3.fw).AsWorldDir(_human),
                        (_iniNeckLower * v3.up).AsWorldDir(_human));
                    iniNeckUpper = lookAt(
                        (_iniNeckUpper * v3.fw).AsWorldDir(_human),
                        (_iniNeckUpper * v3.up).AsWorldDir(_human));
                }
            }
            if (_hasFaceTarget)
            {
                var dirLoToTarg = _human.Spine.NeckLower.position.DirTo(faceTarget);
                var dirUpToTarg = _human.NeckUpper.position.DirTo(faceTarget);

                var tarRotLo = lookAt(dirLoToTarg, slerp(iniUpDir, upDir, x));
                var tarRotUp = lookAt(dirUpToTarg, slerp(iniUpDir, upDir, x));

                var curRotLo = slerp(in iniNeckLower, in tarRotLo, x);
                var curRotUp = slerp(in iniNeckUpper, in tarRotUp, x);
                var centerRot = lookAt(_human.Spine.AbdomenUpper.up.Negate(), _human.Spine.AbdomenUpper.forward);

                if (angle.Between(in curRotLo, in centerRot) > MaxNeckTurnAngle)
                {
                    curRotLo = centerRot.RotateTowards(curRotLo, MaxNeckTurnAngle);
                }
                if (angle.Between(in curRotUp, in centerRot) > MaxNeckTurnAngle)
                {
                    curRotUp = centerRot.RotateTowards(curRotUp, MaxNeckTurnAngle);
                }

                _human.Spine.NeckLower.rotation = curRotLo;
                _human.NeckUpper.rotation = curRotUp;
            }
        }
        void ApplyLook()
        {

            var lookAtTarget = _lookAtTarget;
            if (_space == HandleSpace.Model)
            {
                if (_hasLookTarget) lookAtTarget = lookAtTarget.AsWorldPoint(_human);
            }

            if (_hasLookTarget)
            {
                var eyeL = _isGenesis8 ? _human.GenFace.EyeL : _human.MHFace.EyeL;
                var eyeR = _isGenesis8 ? _human.GenFace.EyeR : _human.MHFace.EyeR;


                var fwLt = eyeL.position.DirTo(in lookAtTarget);
                var fwRt = eyeR.position.DirTo(in lookAtTarget);

                var fwIniLt = eyeL.IniLocalFw.AsWorldDir(eyeL.Holder.parent);
                var fwIniRt = eyeR.IniLocalFw.AsWorldDir(eyeR.Holder.parent);

                if (angleIsMoreThan(in fwLt, in fwIniLt, 20)) fwLt = fwIniLt.RotateTowards(in fwLt, 20);
                if (angleIsMoreThan(in fwRt, in fwIniRt, 20)) fwRt = fwIniRt.RotateTowards(in fwRt, 20);

                eyeL.rotation = lookAt(in fwLt, _human.Head.up);
                eyeR.rotation = lookAt(in fwRt, _human.Head.up);
            }
        }
        
    }
}*/