using System;
using Unianio.Enums;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Graphs
{
    public class ProgressPath : IVectorByProgress
    {
        protected readonly IVectorByProgress _child;
        float _progress = 0;
        bool _isInitialized;
        float _length;
        Vector3 _prevObjectPosition;

        public ProgressPath(IVectorByProgress child)
        {
            _child = child;
        }
        public PathType Type => _child.Type;
        public Func<double, double> Func
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public Vector3 GetValueByProgress(double progress)
        {
            return _child.GetValueByProgress(progress);
        }
        public Vector3 GetDirectionByProgress(double progress)
        {
            return _child.GetDirectionByProgress(progress);
        }
        public Vector3 SetProgressAndGetValue(double progress)
        {
            Progress = (float)progress;
            return _child.GetValueByProgress(Progress);
        }
        public float Progress
        {
            get { return _progress; }
            set { _progress = value.Clamp01(); }
        }
        public Vector3 GetValue()
        {
            return GetValueByProgress(Progress);
        }
        public ProgressPath SetProgress(double progress)
        {
            Progress = (float)progress;
            return this;
        }
        public Vector3 GetNextDirection(Vector3 currObjectPosition, Vector3 currObjectForward, bool ignoreY)
        {
            if (!_isInitialized)
            {
                _length = ignoreY ? _child.GetLengthIgnoreY() : _child.GetLength();
                _isInitialized = true;
            }
            else
            {
                var progressChange = (currObjectPosition - _prevObjectPosition).magnitude;
                Progress += progressChange/_length;
            }
            _prevObjectPosition = currObjectPosition;
            return GetValue() - currObjectPosition;
        }
        public bool IsFinished => _progress > 0.999f;
    }
}