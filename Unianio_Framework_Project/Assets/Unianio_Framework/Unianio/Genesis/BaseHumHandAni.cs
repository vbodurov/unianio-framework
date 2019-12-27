using System;
using System.Collections.Generic;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Moves;
using UnityEngine;

namespace Unianio.Genesis
{
    public abstract class BaseHumHandAni : AnimationManager
    {
        IComplexHuman _human;
        BodySide _side;
        readonly IList<ItemRotation> _actions = new List<ItemRotation>();
        readonly IDictionary<FingerName, IDictionary<int, Transform>> _fingers = new Dictionary<FingerName, IDictionary<int, Transform>>();
        IAnimation _fingersAni;

        protected void Init(IComplexHuman human, BodySide side)
        {
            _human = human;
            _side = side;
            var arm = side == BodySide.Left ? human.ArmL : human.ArmR;
            _fingers[FingerName.Thumb] = new Dictionary<int, Transform> { { 0, arm.Thumb1 }, { 1, arm.Thumb1 }, { 2, arm.Thumb2 }, { 3, arm.Thumb3 } };
            _fingers[FingerName.Index] = new Dictionary<int, Transform> { { 0, arm.Index0 }, { 1, arm.Index1 }, { 2, arm.Index2 }, { 3, arm.Index3 } };
            _fingers[FingerName.Middle] = new Dictionary<int, Transform> { { 0, arm.Middle0 }, { 1, arm.Middle1 }, { 2, arm.Middle2 }, { 3, arm.Middle3 } };
            _fingers[FingerName.Ring] = new Dictionary<int, Transform> { { 0, arm.Ring0 }, { 1, arm.Ring1 }, { 2, arm.Ring2 }, { 3, arm.Ring3 } };
            _fingers[FingerName.Pinky] = new Dictionary<int, Transform> { { 0, arm.Pinky0 }, { 1, arm.Pinky1 }, { 2, arm.Pinky2 }, { 3, arm.Pinky3 } };
        }
        protected void RotFingerToLocal(FingerName fingerName, int index, Vector3 fwLoc, Vector3 upLoc, Func<double, double> func = null)
        {
            var finger = _fingers[fingerName][index];
            var posRot = _human.Initial.Fingers[_side][fingerName][index];
            _actions.Add(new ItemRotation
            {
                Rotate = 
                    move.Rotate(finger.localRotation * v3.fw, finger.localRotation * v3.up,
                                posRot.rotation * fwLoc, posRot.rotation * fwLoc.GetRealUp(upLoc), func),
                Item = finger
            });
        }
        protected void StartFingerRotation(double seconds)
        {
            StartFuncAni(seconds, x =>
            {
                x = Unianio.Static.fun.smootherstep(x);
                for (var i = 0; i < _actions.Count; ++i)
                {
                    var a = _actions[i];

                    a.Item.localRotation = a.Rotate.GetValueByProgress(x);
                }
            })
                .MustBeUnique(ref _fingersAni)
            .OnEnd(a => Finish())
            ;
        }
        protected Vector3 fw_dn_sd(double fwDnDeg, double plusSideDeg)
        {
            var side = _side == BodySide.Left ? Vector3.left : Vector3.right;
            return Vector3.SlerpUnclamped(
                        Vector3.SlerpUnclamped(
                            Vector3.forward, Vector3.down, (float)(fwDnDeg / 90.0)), side, (float)(plusSideDeg / 90.0));

        }
        protected Vector3 fw_sd(double degrees)
        {
            var side = _side == BodySide.Left ? Vector3.left : Vector3.right;
            return Vector3.SlerpUnclamped(v3.forward, side, (float)(degrees / 90.0));
        }
        protected Vector3 up_sd(double degrees)
        {
            var side = _side == BodySide.Left ? Vector3.left : Vector3.right;
            return Vector3.SlerpUnclamped(v3.up, side, (float)(degrees / 90.0));
        }

    }

}