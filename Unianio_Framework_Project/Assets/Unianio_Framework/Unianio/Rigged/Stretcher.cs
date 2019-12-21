using UnityEngine;

namespace Unianio.Rigged
{
    public class Stretcher
    {
        readonly Vector3 _horzDir, _vertDir, _frontDir;
        readonly double _horzLimit, _vertLimit, _frontLimit;
        double _horz01, _vert01, _front01;

        public Stretcher(Vector3 horzDir, double horzLimit, Vector3 vertDir, double vertLimit, Vector3 frontDir, double frontLimit)
        {
            _horzDir = horzDir;
            _horzLimit = horzLimit;
            _vertDir = vertDir;
            _vertLimit = vertLimit;
            _frontDir = frontDir;
            _frontLimit = frontLimit;
            _horz01 = _vert01 = _front01 = 0;
        }
        public static Stretcher Get(Vector3 horzDir, double horzLimit) 
            => new Stretcher(horzDir, horzLimit, v3.zero, 0, v3.zero, 0);
        public static Stretcher Get(Vector3 horzDir, double horzLimit, Vector3 vertDir, double vertLimit) 
            => new Stretcher(horzDir, horzLimit, vertDir, vertLimit, v3.zero, 0);
        public static Stretcher Get(Vector3 horzDir, double horzLimit, Vector3 vertDir, double vertLimit, Vector3 frontDir, double frontLimit)
            => new Stretcher(horzDir, horzLimit, vertDir, vertLimit, frontDir, frontLimit);
        public Stretcher Horz(double horz01)
        {
            _horz01 = horz01;
            return this;
        }
        public Stretcher Vert(double vert01)
        {
            _vert01 = vert01;
            return this;
        }
        public Stretcher Front(double front01)
        {
            _front01 = front01;
            return this;
        }
        public Stretcher New
        {
            get
            {
                _horz01 = _vert01 = _front01 = 0;
                return this;
            }
        }
        public Vector3 V3 =>
            _horzDir * ((float)(_horzLimit * _horz01)) +
            _vertDir * ((float)(_vertLimit * _vert01)) +
            _frontDir * ((float)(_frontLimit * _front01));

        public Vector3 HorzDir => _horzDir;
        public Vector3 VertDir => _vertDir;
        public Vector3 FrontDir => _frontDir;
    }
}