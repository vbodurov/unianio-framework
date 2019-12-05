using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public sealed class DirectionPath : IExecutorOfProgress
    {
        static readonly IVectorByProgress EmptyPath = vbp.Single(v3.zero);
        readonly Action<Vector3> _setter;
        readonly Func<Vector3> _getter;
        readonly Func<double, double> _func;
        readonly Transform _model, _root;
        IVectorByProgress _path;
        Func<bool> _canApply;
        HandleSpace _space = HandleSpace.None;

        public DirectionPath(Action<Vector3> setter, Func<Vector3> getter, Transform model, Transform root, Func<double, double> func = null)
        {
            _setter = setter ?? throw new ArgumentException("Setter is required");
            _getter = getter ?? throw new ArgumentException("Getter is required");
            _model = model;
            _root = root;
            _func = func ?? (Func<double, double>)(n => n);
        }
        public int ID { get; set; }
        public DirectionPath Clone => new DirectionPath(_setter, _getter, _model, _root);
        public DirectionPath New
        {
            get
            {
                _path = EmptyPath;
                _canApply = null;
                return this;
            }
        }
        public DirectionPath ToWorld(IVectorByProgress worldPath)
        {
            _path = worldPath;
            _space = HandleSpace.World;
            return this;
        }
        public DirectionPath ToModel(IVectorByProgress modelPath)
        {
            if (_model == null) return ToWorld(modelPath);
            _path = modelPath;
            _space = HandleSpace.Model;
            return this;
        }
        public DirectionPath ToLocal(IVectorByProgress localPath)
        {
            if (_root == null) return ToWorld(localPath);
            _path = localPath;
            _space = HandleSpace.Local;
            return this;
        }
        public DirectionPath ToWorld(in Vector3 worldDir)
        {
            var from = _getter();
            return ToWorld(vbp.Sphere(in from, in worldDir));
        }
        public DirectionPath ToModel(in Vector3 modelDir)
        {
            if (_model == null) return ToWorld(in modelDir);
            var from = _getter().AsLocalDir(_model);
            return ToModel(vbp.Sphere(in from, in modelDir));
        }
        public DirectionPath ToLocal(in Vector3 localDir)
        {
            if (_root == null) return ToWorld(in localDir);
            var from = _getter().AsLocalDir(_root);
            return ToLocal(vbp.Sphere(in from, in localDir));
        }
        public DirectionPath To2World(in Vector3 worldDir1, double progress2start, in Vector3 worldDir2, Func<double, double> function = null)
        {
            var worldDir0 = _getter();
            return ToWorld(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in worldDir0, in worldDir1, function)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in worldDir1, in worldDir2, function))
                ));
        }
        public DirectionPath To2World(in Vector3 worldDir1, double progress2start, in Vector3 worldDir2, Func<double, double> function1, Func<double, double> function2)
        {
            var worldDir0 = _getter();
            return ToWorld(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in worldDir0, in worldDir1, function1)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in worldDir1, in worldDir2, function2))
            ));
        }
        public DirectionPath To2Model(in Vector3 modelDir1, double progress2start, in Vector3 modelDir2, Func<double, double> function = null)
        {
            if (_model == null) return To2World(in modelDir1, progress2start, in modelDir2, function);
            var modelDir0 = _getter().AsLocalDir(_model);
            return ToModel(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in modelDir0, in modelDir1, function)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in modelDir1, in modelDir2, function))
            ));
        }
        public DirectionPath To2Model(in Vector3 modelDir1, double progress2start, in Vector3 modelDir2, Func<double, double> function1, Func<double, double> function2)
        {
            if (_model == null) return To2World(in modelDir1, progress2start, in modelDir2, function1, function2);
            var modelDir0 = _getter().AsLocalDir(_model);
            return ToModel(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in modelDir0, in modelDir1, function1)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in modelDir1, in modelDir2, function2))
            ));
        }
        public DirectionPath To2Local(in Vector3 localDir1, double progress2start, in Vector3 localDir2, Func<double, double> function = null)
        {
            if (_root == null) return To2World(in localDir1, progress2start, in localDir2, function);
            var localDir0 = _getter().AsLocalDir(_root);
            return ToModel(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in localDir0, in localDir1, function)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in localDir1, in localDir2, function))
            ));
        }
        public DirectionPath To2Local(in Vector3 localDir1, double progress2start, in Vector3 localDir2, Func<double, double> function1, Func<double, double> function2)
        {
            if (_root == null) return To2World(in localDir1, progress2start, in localDir2, function1, function2);
            var localDir0 = _getter().AsLocalDir(_root);
            return ToModel(vbp.Complex(
                vbp.InRange(0.0, progress2start, vbp.Sphere(in localDir0, in localDir1, function1)),
                vbp.InRange(progress2start, 1.0, vbp.Sphere(in localDir1, in localDir2, function2))
            ));
        }
        public DirectionPath SetCondition(Func<bool> condition)
        {
            _canApply = condition;
            return this;
        }
        public void Apply(double progress)
        {
            if (_canApply != null && !_canApply()) return;
            var dir = _path.GetValueByProgress(progress);
            if(_space == HandleSpace.Model) _setter(dir.AsWorldDir(_model));
            else if (_space == HandleSpace.Local) _setter(dir.AsWorldDir(_root));
            else _setter(dir);
        }
        public void Apply(double progress, Func<double, double> func)
        {
            if (_canApply != null && !_canApply()) return;
            progress = func(progress);
            Apply(progress);
        }
    }
}