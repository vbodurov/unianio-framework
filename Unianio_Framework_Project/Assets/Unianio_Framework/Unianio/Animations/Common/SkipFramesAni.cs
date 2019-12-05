using System;
using UnityEngine;

namespace Unianio.Animations.Common
{
    public class SkipFramesAni : StateHolderAni
    {
        int _lastFrame;
        int _framesToSkip;

        public SkipFramesAni Set(int framesToSkip)
        {
            _framesToSkip = framesToSkip;
            return this;
        }
        public override void Initialize()
        {
            _lastFrame = Time.frameCount + _framesToSkip;
            if (_framesToSkip <= 0)
            {
                Finish();
            }
        }
        public override void Update()
        {
            if(IsFinished) return;
            if (Time.frameCount >= _lastFrame)
            {
                Finish();
            }
        }
    }
}