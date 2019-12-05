using System;
using UnityEngine;

namespace Unianio.Services.Drawing
{
    public class RenderedLine : IDisposable
    {
        private LineRenderer _lineRenderer;
        private float _lineSize;

        public RenderedLine(float lineSize = 0.002f)
        {
            init(lineSize);
        }

        private void init(float lineSize)
        {
            if (_lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                _lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                _lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                _lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if (_lineRenderer == null)
            {
                init(0.2f);
            }

            //Set color
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;

            //Set width
            _lineRenderer.startWidth = _lineSize;
            _lineRenderer.endWidth = _lineSize;

            //Set line count which is 2
            _lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }

        public void Dispose()
        {
            if (_lineRenderer != null)
            {
                UnityEngine.Object.Destroy(_lineRenderer.gameObject);
            }
        }
    }
}