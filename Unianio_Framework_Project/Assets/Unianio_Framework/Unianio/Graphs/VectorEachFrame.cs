using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class VectorEachFrame<T> : IVectorByProgress where T : IVectorByProgress
    {
        readonly T _child;
        readonly Action<float, T> _update;
        internal VectorEachFrame(T child, Action<float, T> update)
        {
            _child = child;
            _update = update;
        }
        public PathType Type => _child.Type;

        public Vector3 GetValueByProgress(double progress)
        {
            _update((float) progress, _child);
            return _child.GetValueByProgress(progress);
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return _child.GetDirectionByProgress(progress);
        }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}