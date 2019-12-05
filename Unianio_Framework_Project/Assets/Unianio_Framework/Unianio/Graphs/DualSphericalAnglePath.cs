using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public class DualSphericalAnglePath : IStartEndVectorByProgress
    {
        Vector3 _start, _middle, _end;
        readonly Func<double, double> _func1, _func2;

        public DualSphericalAnglePath(
            Vector3 startVector, Vector3 middleVector, Vector3 endVector) 
        {
            _start = startVector;
            _middle = middleVector;
            _end = endVector;
        }
        public DualSphericalAnglePath(
            Vector3 startVector, Vector3 middleVector, Vector3 endVector,
            Func<double,double> func1, Func<double,double> func2) 
        {
            _start = startVector;
            _middle = middleVector;
            _end = endVector;
            _func1 = func1;
            _func2 = func2;
        }
        public PathType Type => PathType.SphericalAnglePath;
        public Vector3 Start
        {
            get => _start;
            set => _start = value;
        }
        internal Vector3 Middle
        {
            get => _middle;
            set => _middle = value;
        }
        public Vector3 End
        {
            get => _end;
            set => _end = value;
        }
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public Func<double, double> Func1 => _func1;

        public Func<double, double> Func2 => _func2;

        public Vector3 GetValueByProgress(double progress)
        {
            var isFirstPart = progress <= 0.5;

            if (isFirstPart)
            {
                progress = progress*2.0;
                if (_func1 != null) progress = _func1(progress);
            }
            else
            {
                progress = (progress - 0.5)*2.0;
                if (_func2 != null) progress = _func2(progress);
            }

            var t = (float)progress.Clamp01();
            return isFirstPart 
                ? Vector3.Slerp(_start, _middle, t) 
                : Vector3.Slerp(_middle, _end, t);
        }
        public Vector3 GetDirectionByProgress(double progress) { throw new NotImplementedException(); }
    }
}