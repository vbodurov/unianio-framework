using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public class DynamicVector : IVectorByProgress
    {
        readonly Func<double, Vector3> _func;
        public DynamicVector(Func<double, Vector3> func)
        {
            _func = func;
        }
        public PathType Type { get { return PathType.DynamicPath; } }
        public Vector3 GetValueByProgress(double progress)
        {
            return _func(progress);
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            throw new NotImplementedException();
        }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}