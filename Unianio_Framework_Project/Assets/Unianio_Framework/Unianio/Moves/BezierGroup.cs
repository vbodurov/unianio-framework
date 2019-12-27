using System;
using System.Collections.Generic;
using UnityEngine;

/*
http://cubic-bezier.com/#.97,.37,.24,.85
var gety =
  beziers(b => b.from(-1,-1)
    .to(0,0).curve(.26,-0.63,.24,.85)
    .to(1,1).curve(.55,.18,.79,1.5))
.run(x);
*/
namespace Unianio.Moves
{
    public interface IBezierGroup
    {
        float run(double x);
        int FragmentsCount { get; }
        IEnumerable<IBezierFragment> Fragments { get; }
    }
    public interface IBezierGroupBuilder
    {
        Vector2 From { get; }
        double LastLimit { get; }
        IBezierGroup setup(Action<IBezierGroupBuilder> setup);
        IBezierGroupBuilder from(double x, double y);
        IBezierFragmentBuilder to(double x, double y);
    }
    public interface IBezierFragment
    {
        double run(double x);
        IBezierGroupBuilder GroupBuilder { get; }
        Vector2 From { get; }
        Vector2 To { get; }
        Vector2 B { get; }
        Vector2 C { get; }
    }
    public interface IBezierFragmentBuilder
    {
        IBezierFragmentBuilder to(double x, double y);
        IBezierFragmentBuilder curve(double bx, double by, double cx, double cy);
    }
    public sealed class BezierGroup : IBezierGroup, IBezierGroupBuilder
    {
        private readonly IBezierGroupBuilder _builder;
        private RangeSet<IBezierFragment> _fragments;
        private bool _isSetUpFunctionInvoked = false;
        private Vector2 _from;

        internal BezierGroup()
        {
            _builder = this;
            _from = new Vector2(0,0);
            _fragments = new RangeSet<IBezierFragment>(0);
        }

        internal static IBezierGroup beziers(Action<IBezierGroupBuilder> setupGroup)
        {
            return ((IBezierGroupBuilder)new BezierGroup()).setup(setupGroup);
        }

        int IBezierGroup.FragmentsCount { get { return _fragments.Count; } }

        float IBezierGroup.run(double x)
        {
            IBezierFragment fragment;
            if (_fragments.TryFind(x, out fragment))
            {
                return (float)fragment.run(x);
            }
            return 0.0f;
        }
        Vector2 IBezierGroupBuilder.From { get { return _from; } }
        double IBezierGroupBuilder.LastLimit { get { return _fragments.LastLimit; } }

        IEnumerable<IBezierFragment> IBezierGroup.Fragments { get { return _fragments; } }
        IBezierGroup IBezierGroupBuilder.setup(Action<IBezierGroupBuilder> setup)
        {
            if (!_isSetUpFunctionInvoked)
            {
                setup(this);
                _isSetUpFunctionInvoked = true;
            }
            return this;
        }
        IBezierGroupBuilder IBezierGroupBuilder.from(double x, double y)
        {
            _from = new Vector2((float)x,(float)y);
            _fragments = new RangeSet<IBezierFragment>(x);
            return _builder;
        }
        IBezierFragmentBuilder IBezierGroupBuilder.to(double x, double y)
        {
            var lastPoint = _fragments.Count == 0 ? _from : _fragments.LastValue.To;
            var fragment = new BezierFragment(this, lastPoint, new Vector2((float)x,(float)y));
            _fragments.Add(x, fragment);
            return fragment;
        }
    }
    public sealed class BezierFragment : IBezierFragment, IBezierFragmentBuilder
    {
        private readonly IBezierGroupBuilder _groupBuilder;
        private readonly Vector2 _from;
        private readonly Vector2 _to;
        private readonly double _xRange;
        private readonly double _yRange;
        private Vector2 _b;
        private Vector2 _c;


        internal BezierFragment(IBezierGroupBuilder groupBuilder, Vector2 from, Vector2 to)
        {
            _groupBuilder = groupBuilder;
            _from = from;
            _to = to;
            _b = new Vector2(0.1f, 0.5f);
            _c = new Vector2(0.5f, 0.1f);

            _xRange = _to.x - _from.x;
            _yRange = _to.y - _from.y;
        }

        IBezierGroupBuilder IBezierFragment.GroupBuilder { get { return _groupBuilder; } }
        Vector2 IBezierFragment.From { get { return _from; } }
        Vector2 IBezierFragment.To { get { return _to; } }
        Vector2 IBezierFragment.B { get { return _b; } }
        Vector2 IBezierFragment.C { get { return _c; } }

        double IBezierFragment.run(double x)
        {
            return BezierFunc.GetY(
                    x,
                    _from.x,  _from.y,
                    _b.x, _b.y,
                    _c.x, _c.y,
                    _to.x, _to.y);
        }
        IBezierFragmentBuilder IBezierFragmentBuilder.to(double x, double y)
        {
            return _groupBuilder.to(x, y);
        }
        IBezierFragmentBuilder IBezierFragmentBuilder.curve(double bx, double by, double cx, double cy)
        {
            _b = new Vector2((float)bx, (float)by);
            _c = new Vector2((float)cx, (float)cy);
            return this;
        }

        
    }

}