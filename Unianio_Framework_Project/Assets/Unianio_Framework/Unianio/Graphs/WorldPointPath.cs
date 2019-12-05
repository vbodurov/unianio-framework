using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Graphs
{
    public sealed class WorldPointPath : IExecutorOfProgress
    {
        Func<bool> _canApply;
        Func<WorldPointPath, Vector3> _getSource = p => v3.zero;
        Func<WorldPointPath, Vector3> _getTarget = p => v3.zero;
        Func<WorldPointPath, Vector3> _getControl1;
        Func<WorldPointPath, (Vector3, Vector3)> _getControls1;
        Func<Vector3, Vector3, Vector3> _getControl2;
        Func<Vector3, Vector3, (Vector3, Vector3)> _getControls2;
        Vector3 _current;
        public int ID { get; set; }
        public WorldPointPath Clone => new WorldPointPath();
        public WorldPointPath New(Vector3 source, Vector3 target) => New(a => source, a => target);
        public WorldPointPath New(Vector3 source, Func<WorldPointPath, Vector3> getTarget) => New(a => source, getTarget);
        public WorldPointPath New(Func<WorldPointPath, Vector3> getTarget)
        {
            var current = Current;
            return New(a => current, getTarget);
        }
        public WorldPointPath New(Vector3 target)
        {
            var current = Current;
            return New(a => current, a => target);
        }
        public WorldPointPath New(Func<WorldPointPath, Vector3> getSource, Func<WorldPointPath, Vector3> getTarget)
        {
            _getSource = getSource ?? throw new ArgumentException("getSource cannot be null");
            _getTarget = getTarget ?? throw new ArgumentException("getTarget cannot be null");
            _getControl1 = null;
            _getControls1 = null;
            _getControl2 = null;
            _getControls2 = null;
            _current = v3.zero;
            return this;
        }
        public Vector3 Source => _getSource(this);
        public Vector3 Target => _getTarget(this);
        public Vector3 Current => _current;
        public Vector3 ApplyAndGet(double x)
        {
            Apply(x);
            return _current;
        }
        public WorldPointPath Control(Func<WorldPointPath, Vector3> getControl)
        {
            _getControl1 = getControl;
            if (getControl != null) _getControl2 = null;
            return this;
        }
        public WorldPointPath Controls(Func<WorldPointPath, (Vector3, Vector3)> getControls)
        {
            _getControls1 = getControls;
            if (getControls != null) _getControls1 = null;
            return this;
        }
        public WorldPointPath Control(Func<Vector3, Vector3, Vector3> getControl)
        {
            _getControl2 = getControl;
            if (getControl != null) _getControl1 = null;
            return this;
        }
        public WorldPointPath Controls(Func<Vector3, Vector3, (Vector3, Vector3)> getControls)
        {
            _getControls2 = getControls;
            if (getControls != null) _getControls1 = null;
            return this;
        }
        public WorldPointPath Condition(Func<bool> condition)
        {
            _canApply = condition;
            return this;
        }
        public void Apply(double progress)
        {
            if (_canApply != null && !_canApply()) return;
            var source = _getSource(this);
            var target = _getTarget(this);
            if (_getControls1 != null)
            {
                var controls = _getControls1(this);
                _current = bezierV3(in source, in controls.Item1, in controls.Item2, in target, progress);
            }
            else if (_getControls2 != null)
            {
                var controls = _getControls2(source, target);
                _current = bezierV3(in source, in controls.Item1, in controls.Item2, in target, progress);
            }
            else if (_getControl1 != null)
            {
                var control = _getControl1(this);
                _current = bezierV3(in source, in control, in target, progress);
            }
            else if (_getControl2 != null)
            {
                var control = _getControl2(source, target);
                _current = bezierV3(in source, in control, in target, progress);
            }
            else
            {
                _current = lerp(in source, in target, progress);
            }
        }
        public void Apply(double progress, Func<double, double> func)
        {
            if (_canApply != null && !_canApply()) return;
            if(func != null) progress = func(progress);
            Apply(progress);
        }
    }
}