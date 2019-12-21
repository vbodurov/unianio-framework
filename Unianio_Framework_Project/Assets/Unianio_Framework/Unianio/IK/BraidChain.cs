using System;
using Unianio.Extensions;
using Unianio.PhysicsAgents;
using Unianio.Rigged;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.IK
{
    public class BraidChain : BaseIkChain
    {
        delegate void EnforceConstraint(int index, ref Vector3 point);

        readonly IComplexHuman _human;
        readonly Transform[] _nodes;
        readonly Vector3[] _tempPositions, _tempFw, _iniRootPositions;
        readonly float[] _joinLengths;
        readonly IPendulumPhysicsAgent _pendulum;
        readonly Vector3 _handleIniLocPos;
        readonly float _headToHandleDist;

        public BraidChain(IComplexHuman human, params Transform[] nodes) : base(HumanoidPart.Braid)
        {
            _human = human;
            _nodes = nodes ?? throw new ArgumentException("No braid nodes found");
            _tempPositions = new Vector3[_nodes.Length + 2]; // 1 x each node + 1 root + 1 handler
            _tempFw = new Vector3[_nodes.Length + 1]; // for each temp position except root

            _joinLengths = new float[_nodes.Length + 1];
            _joinLengths[0] = _human.Head.position.DistanceTo(_nodes[0].position);
            for (var i = 1; i < _nodes.Length; ++i)
            {
                var last = _nodes[i - 1];
                var curr = _nodes[i];
                _joinLengths[i] = vDist(last.position, curr.position);
            }
            const float LastLen = 0.1f;
            _joinLengths[_joinLengths.Length - 1] = LastLen;
            var lastNode = nodes[nodes.Length - 1]; // hair6
            _handle = CreateHandle(_human.Head.Holder, HumanoidPart.Braid, false, lastNode.position + lastNode.forward.By(LastLen), human.dn, human.bk);

            _iniRootPositions = new Vector3[_nodes.Length + 1]; // for each position except root
            for (var i = 0; i < _nodes.Length; ++i)
            {
                _iniRootPositions[i] = _nodes[i].position.AsLocalPoint(_human.Head);
            }
            _iniRootPositions[_iniRootPositions.Length - 1] = _handle.position.AsLocalPoint(_human.Head);


            _headToHandleDist = _handle.position.DistanceTo(_human.Head.position);
            _handleIniLocPos = _handle.localPosition;
            _pendulum = new PendulumPhysicsAgent(
                stiffness: 0.005, mass: 0.50, damping: 0.85, gravity: 0);
        }

        public override void Update()
        {


            var chest = _human.Spine.ChestUpper;
            var headPos = _human.Head.Holder.position;
            var headHorzRot = _human.Head.Holder.rotation.AsHorizontal();
            var origTargPos = _handleIniLocPos.AsWorldPoint(in headHorzRot, in headPos);
            var origDist = origTargPos.DistanceTo(in headPos);
            var targPos =  
                _pendulum.Compute(in origTargPos, 0,
                    (ref Vector3 pos, ref Vector3 velocity, ref Vector3 force) =>
                    {

                        var origDir = headPos.DirTo(in origTargPos);
                        var currDir = headPos.DirTo(in pos, out var currDist);
                        var diviation = 1f - dot(in origDir, in currDir).Clamp01();
                        var maxShrink = diviation * origDist * 0.5f;
                        if (currDist < (origDist - maxShrink)) pos = headPos + currDir * (origDist - maxShrink);


                        point.EnforceWithinSphere(in pos, _human.Head.position, _headToHandleDist * 1.2, out pos);

                        if (TryEnforcePlane(ref pos))
                        {
                            velocity *= -0.5f;
                        }

                        
                    });
          
//dbg.DrawLineTo(1, _handle.position);
//dbg.DrawSphere(1,Color.magenta, _human.Head.position, _headToHandleDist * 1.2);
//dbg.DrawPlane1(chest.up.ToDn(20), V3(0, 0.06, 0).AsWorldPoint(chest));
            _handle.position = targPos;

            var currHandlePos = _handle.localPosition;

            var hasPositionChange = !_prevHandlePos.IsEqual(in currHandlePos, 0.00002);

            if (hasPositionChange)
            {
                ProcessMove(true, false);
            }

            _prevHandlePos = currHandlePos;
        }

        protected override void ProcessMove(bool hasPositionChange, bool hasRotationChange)
        {
            AssignDefaultValues(_tempPositions);
//for (int i = 0; i < _tempPositions.Length; i++) dbg.DrawAltLineTo(i+100, _tempPositions[i], dbg.GetColor(i));
            
            const int NumFabrikIterations = 4;
            FABRIK(_handle.position, _tempPositions, _joinLengths, NumFabrikIterations, 
                (int index, ref Vector3 p) =>
                {
                    if(index < 4) return;
                    TryEnforcePlane(ref p);
                });

            RotateBonesToTargets(_tempPositions);
        }

        bool TryEnforcePlane(ref Vector3 p)
        {
            var chest = _human.Spine.ChestUpper;
            return point.EnforceAbovePlane(in p, chest.up.RotDn(20), V3(0, 0.06, 0).AsWorldPoint(chest), out p);
        }
        void AssignDefaultValues(Vector3[] points)
        {
            points[0] = _human.Head.position;
            for (var i = 1; i < points.Length; ++i)
            {
                points[i] = _iniRootPositions[i - 1].AsWorldPoint(_human.Head);
            }

        }
        static void FABRIK(
            in Vector3 handlePos,
            Vector3[] points,
            float[] lengths,
            int repetitions, 
            EnforceConstraint func)
        {
            var lastIndex = points.Length - 1;

            for (var j = 0; j < repetitions; ++j)
            {
                points[lastIndex] = handlePos;

                // forward (handle) to (root)
                for (var i = lastIndex; i > 1; --i) // important that i > 1 because i > 0 would move root [0]
                {
                    var len = lengths[i - 1];

                    var dir = (points[i - 1] - points[i]).normalized;
                    points[i - 1] = points[i] + dir * len;

                    // appply constraint
                    func(i - 1, ref points[i - 1]);
                }
                // and backward (root) to (handle)
                for (var i = 0; i < lastIndex; ++i)
                {
                    var len = lengths[i];

                    var dir = (points[i + 1] - points[i]).normalized;
                    points[i + 1] = points[i] + dir * len;
                }

                const double MinDist = 0.001 * 0.001;
                if (distanceSquared.BetweenAsDouble(in points[lastIndex], in handlePos) < MinDist)
                {
                    return;
                }
            }
        }
        void RotateBonesToTargets(Vector3[] points)
        {
            // compute forward directions
            for (var i = 1; i < points.Length; ++i)
            {
                _tempFw[i - 1] = (points[i] - points[i - 1]).normalized; //.ToUnit(ref ifZero);
            }


            //            var degPerFrame = DegreesPerSec * fun.smoothDeltaTime;
            // apply rotations
            var headRot = _human.Head.rotation;
            var headBk = headRot * v3.bk;
            var headDn = headRot * v3.dn;
            var upDir = headBk;
            for (var i = 0; i < _nodes.Length; ++i)
            {

                var fw = _tempFw[i + 1];
                upDir = fw.GetRealUp(in upDir);
                _nodes[i].rotation = Quaternion.LookRotation(fw, upDir);
            }

//for (int i = 0; i < points.Length; i++) dbg.DrawLineTo(i+1000, points[i], dbg.GetColor(i));
        }
    }
}