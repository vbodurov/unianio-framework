using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unianio.Services.Drawing
{
    internal interface ISimpleLineDrawer : IDisposableHolder
    {
        void Draw();
        ISimpleLineDrawer Within(Transform transform);
        ISimpleLineDrawer Color(Color color);
        ISimpleLineDrawer Color(uint color);
        ISimpleLineDrawer Line(Vector3 from, Vector3 to);
        ISimpleLineDrawer Vector(Vector3 vector3);
        ISimpleLineDrawer VectorRelTo(Vector3 vector3, Vector3 relativeTo);
        ISimpleLineDrawer Sequence(IEnumerable<Vector3> list);
        Transform Transform { get; }
        ISimpleLineDrawer Line(Func<Transform, LineDef> getPoints);
        ISimpleLineDrawer Vector(Func<Transform, Vector3> getVector);
        ISimpleLineDrawer VectorRelTo(Func<Transform, LineDef> getPoints);
        bool RemoveLine(LineDef lineDef);
    }
    /*
        var drawerLines = new List<LineDef> {LineDef.New,LineDef.New};
        ISimpleLineDrawer drawer = new SimpleLineDrawer(false);
        drawer.Line(t => drawerLines[0]).Line(t => drawerLines[1]);
        StartEndlessDrawFuncAni(a => drawer.Draw());
        ...
        drawerLines[1].SetFrom(p1).SetTo(join.Parent.TransformPoint(border)).SetColor(color);
    */
    internal sealed class SimpleLineDrawer : ISimpleLineDrawer
    {
        private readonly ISimpleLineDrawer _drawer;
        private readonly Dictionary<int, List<LineData>> _linesByTransformId = 
            new Dictionary<int, List<LineData>>();
        private readonly Material _material;
        private GameObject _gameObject;
        private Transform _transform;
        private Color _color;
        

        internal SimpleLineDrawer(bool doDepthTest)
        {
            _drawer = this;

            var shader = Shader.Find ("Hidden/Internal-Colored");
	        _material = new Material(shader)
	        {
	            hideFlags = HideFlags.HideAndDontSave
	        };
	        // Set blend mode to show destination alpha channel.
            var blend = doDepthTest
                ? UnityEngine.Rendering.BlendMode.OneMinusDstAlpha
                : UnityEngine.Rendering.BlendMode.Zero;
            _material.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstAlpha);
            _material.SetInt ("_DstBlend", (int)blend);

            // Turn off backface culling, depth writes, depth test.
            if (!doDepthTest)
            {
                _material.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                _material.SetInt ("_ZWrite", 0);
                _material.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            }
            
            _color = Color.white;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Color(Color color)
        {
            _color = color;
            return this;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Color(uint color)
        {
            _color = fun.color.Parse(color);
            return this;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Within(Transform transform)
        {
            _transform = transform;
            return this;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Line(Vector3 from, Vector3 to)
        {
            if (_color == Color.clear) return this;
            
            GetLineData().Add(new LineData
            {
                Color = _color,
                Transform = _transform,
                Points = Rng.v3(from, to)
            });

            return this;
        }

        ISimpleLineDrawer ISimpleLineDrawer.Line(Func<Transform,LineDef> getPoints)
        {
            if (_color == Color.clear) return this;
            
            GetLineData().Add(new LineData
            {
                Color = _color,
                Transform = _transform,
                GetPoints = getPoints
            });

            return this;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Vector(Vector3 vector)
        {
            return _drawer.Line(v3.zero, vector);
        }
        ISimpleLineDrawer ISimpleLineDrawer.Vector(Func<Transform,Vector3> getVector)
        {
            return _drawer.Line(t => LineDef.New.SetFrom(v3.zero).SetTo(getVector(t)).SetColor(_color));
        }
        ISimpleLineDrawer ISimpleLineDrawer.VectorRelTo(Vector3 vector3, Vector3 relativeTo)
        {
            return _drawer.Line(relativeTo, relativeTo + vector3);
        }
        ISimpleLineDrawer ISimpleLineDrawer.VectorRelTo(Func<Transform,LineDef> getPoints)
        {
            var child = LineDef.New;
            return _drawer.Line(t =>
            {
                var ld = getPoints(t);
                return child.SetFrom(ld.To).SetTo(ld.To + ld.From).SetColor(ld.Color ?? _color);
            });
        }

        bool ISimpleLineDrawer.RemoveLine(LineDef lineDef)
        {
            foreach (var list in _linesByTransformId.Values)
            {
                var found = list.FirstOrDefault(ld =>
                {
                    if (ld.GetPoints == null) return false;
                    var def = ld.GetPoints(ld.Transform);
                    return def != null && def == lineDef;
                });
                list.Remove(found);
                return true;
            }
            return false;
        }
        ISimpleLineDrawer ISimpleLineDrawer.Sequence(IEnumerable<Vector3> list)
        {
            if (list == null) return this;
            var prev = v3.zero;
            var isFirst = true;
            foreach (var curr in list)
            {
                if (!isFirst)
                {
                    _drawer.Line(prev, curr);
                }
                isFirst = false;
                prev = curr;
            }
            return this;
        }
        Transform ISimpleLineDrawer.Transform => _gameObject == null ? null : _gameObject.transform;

        void ISimpleLineDrawer.Draw()
        {
            _material.SetPass(0);
            foreach (var list in _linesByTransformId.Values)
            {
                var count = list.Count;
                if(count == 0) continue;

                for (var i = 0; i < count; ++i)
                {
                    var curr = list[i];
                    if (i == 0)
                    {
                        GL.PushMatrix();
                        GL.MultMatrix(curr.Transform.localToWorldMatrix);
                        GL.Begin(GL.LINES);
                    }

                    
                    var getter = curr.GetPoints;
                    if (getter != null)
                    {
                        var p = getter(curr.Transform);

                        GL.Color(p.Color ?? curr.Color);

                        GL.Vertex(p.From);
                        GL.Vertex(p.To);
                    }
                    else
                    {
                        GL.Color(curr.Color);
                        GL.Vertex(curr.Points.From);
                        GL.Vertex(curr.Points.To);
                    }
                }
                GL.End();
                GL.PopMatrix(); 
            }
            
        }

        private List<LineData> GetLineData()
        {
            if (_transform == null)
            {
                _gameObject = new GameObject("line_drawer_"+ShortGuid.New().ToString("X"));
                _transform = _gameObject.transform;
            }
            var id = _transform.GetInstanceID();
            List<LineData> list;
            if (!_linesByTransformId.TryGetValue(id, out list))
            {
                _linesByTransformId[id] = list = new List<LineData>();
            }
            return list;
        }

        internal class LineData
        {
            internal Transform Transform;
            internal Range<Vector3> Points;
            internal Func<Transform,LineDef> GetPoints;
            internal Color Color;
        }

        private bool _isDestroyed;
        void IDisposable.Dispose()
        {
            if (_gameObject != null)
            {
                Object.Destroy(_gameObject);
                _gameObject = null;
            }
            _isDestroyed = true;
        }
        bool IDisposableHolder.IsDisposed => _isDestroyed;
    }

    internal class LineDef
    {
        internal Vector3 From;
        internal Vector3 To;
        internal Color? Color;

        internal LineDef SetFrom(Vector3 from)
        {
            From = from;
            return this;
        }
        internal LineDef SetTo(Vector3 to)
        {
            To = to;
            return this;
        }
        internal LineDef SetColor(Color color)
        {
            Color = color;
            return this;
        }
        internal static LineDef New => new LineDef();
    }
}