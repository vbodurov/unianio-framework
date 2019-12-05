using System;
using Unianio.Services.Drawing;
using UnityEngine;

namespace Unianio.Animations.Drawing
{
    public class DrawLinesAni : AnimationBase
    {
        ISimpleLineDrawer _lineDrawer;
        TimeRange _time;
        float _seconds;
        Transform _relativeTo;

        public DrawLinesAni()
        {
            IsGlDrawing = true;
        }
        public DrawLinesAni Set(double seconds, bool doDepthTest)
        {
            _seconds = (float)seconds;
            _lineDrawer = new SimpleLineDrawer(doDepthTest);
            return this;
        }
        public DrawLinesAni RelativeTo(Transform t)
        {
            _relativeTo = t;
            return this;
        }
        public DrawLinesAni AddLine(Vector3 a, Vector3 b, Color color)
        {
            if(_lineDrawer == null) throw new ArgumentException("Call first DrawLinesAni.Set method");
            _lineDrawer.Color(color);
            _lineDrawer.Line(a, b);
            return this;
        }
        public override void Initialize()
        {   
            if (_relativeTo != null) _lineDrawer.Within(_relativeTo);
            _time = new TimeRange().SetTime(_seconds);
        }
        public override void Update()
        {
            if(_time.IsFinished()) Finish();
        }
        public override void Draw()
        {
            _lineDrawer.Draw();
        }
        protected override void OnFinishWhenNotForced()
        {
            if (_lineDrawer != null)
            {
                _lineDrawer.Dispose();
                _lineDrawer = null;
            }
        }
    }
}