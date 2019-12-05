using System.Collections.Generic;
using Unianio.Graphs;
using Unianio.Services.Drawing;
using UnityEngine;

namespace Unianio.Animations.Drawing
{
    public class DrawPointByProgressAni : AnimationBase
    {
        TimeRange _time;
        float _seconds;
        IVectorByProgress _pbp;
        Color _lineColor;
        Color _controlColor;
        bool _doDepthTest;
        ISimpleLineDrawer _lineDrawer;
        Transform _relativeTo;

        public DrawPointByProgressAni()
        {
            IsGlDrawing = true;
        }
        public DrawPointByProgressAni Set(double seconds, IVectorByProgress pbp, Color lineColor, bool doDepthTest)
        {
            return Set(seconds, pbp, lineColor, Color.gray, doDepthTest);
        }
        public DrawPointByProgressAni Set(double seconds, IVectorByProgress pbp, Color lineColor, Color controlColor, bool doDepthTest)
        {
            _seconds = (float)seconds;
            _pbp = pbp;
            _lineColor = lineColor;
            _controlColor = controlColor;
            _doDepthTest = doDepthTest;
            return this;
        }
        public DrawPointByProgressAni RelativeTo(Transform t)
        {
            _relativeTo = t;
            return this;
        }
        public override void Initialize()
        {
            var points = new List<Vector3>();
            const float step = 0.03f;
            for (var x = 0f; x <= 1.0; x += step)
            {
                var next = _pbp.GetValueByProgress(x);

                points.Add(next);
            }

            _lineDrawer = new SimpleLineDrawer(_doDepthTest);
            if (_relativeTo != null) _lineDrawer.Within(_relativeTo);
            _lineDrawer
                .Color(_lineColor)
                    .Sequence(points);

            var type = _pbp.GetType();
            if (type == typeof (QuadraticBezier))
            {
                var b = (QuadraticBezier)_pbp;

                _lineDrawer
                    .Color(_controlColor)
                        .Line(b.Start, b.Control)
                        .Line(b.Control, b.End);
            }
            else if (type == typeof (CubicBezier))
            {
                var b = (CubicBezier)_pbp;

                _lineDrawer
                    .Color(_controlColor)
                        .Line(b.Start, b.Control1)
                        .Line(b.Control2, b.End);
            }

            _time = new TimeRange().SetTime(_seconds);
        }

        public override void Update()
        {
            if(_time.IsFinished()) Finish();
        }

        protected override void OnFinishWhenNotForced()
        {
            if (_lineDrawer != null)
            {
                _lineDrawer.Dispose();
                _lineDrawer = null;
            }
        }

        public override void Draw()
        {
            _lineDrawer.Draw();
        }
    }
}