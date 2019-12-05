using System;
using UnityEngine;

namespace Unianio.Animations.Common
{
    public class FramesPerSecondAni : AnimationBase
    {
        int _frames = 0; // Frames drawn over the interval
        long _secs;
        float _framesAvg,_lastTime;
        Action<float> _setFpsUi;

        public FramesPerSecondAni Set(Action<float> setFpsUi)
        {
            _setFpsUi = setFpsUi;

            return this;
        }

        public override void Update()
        {
            ++_frames;
            var time = Time.realtimeSinceStartup;
            if (time - _lastTime >= 0.999f)
            {
                ++_secs;

                if (_framesAvg < 1) _framesAvg = _frames;

                _framesAvg = Average(_framesAvg, _frames, 8);
                
                var fps = _secs < 30 ? _frames : Math.Round(_framesAvg);
                _setFpsUi((float)fps);
                _lastTime = time;
                _frames = 0;
            }
        }


        static float Average(double lastAverage, double current, int count)
        {
            return (count <= 1.0f) ? (float)current : ((float)lastAverage * (count - 1.0f) + (float)current) / count;
        }

    }
}