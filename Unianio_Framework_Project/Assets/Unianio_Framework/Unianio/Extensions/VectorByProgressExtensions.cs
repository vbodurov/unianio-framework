using System;
using System.Collections.Generic;
using Unianio.Animations.Drawing;
using Unianio.Enums;
using Unianio.Graphs;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class VectorByProgressExtensions
    {
        public static ProgressPath ToProgressPath(this QuadraticBezier c) { return new ProgressPath(c); }
        public static ProgressPath ToProgressPath(this CubicBezier c) { return new ProgressPath(c); }
        public static ProgressPath ToProgressPath(this LinearPath c) { return new ProgressPath(c); }
        public static ProgressPath ToProgressPath(this SingleVector c) { return new ProgressPath(c); }
        public static ProgressPath ToProgressPath(this SphericalAnglePath c) { return new ProgressPath(c); }
        public static ProgressPath ToProgressPath(this IVectorByProgress c) { return new ProgressPath(c); }

        public static IVectorByProgress ToWorld(this IVectorByProgress child, Transform t)
        {
            return new PathToWorld(t, child);
        }

        public static Vector3 GetTangentByProgress(this IVectorByProgress vbp, double progress)
        {
            const float step = 1/128f;
            var lastAllowed = 1 - step;
            if (progress > lastAllowed) progress = lastAllowed;
            else if (progress < 0) progress = 0;
            var a = vbp.GetValueByProgress(progress);
            var b = vbp.GetValueByProgress(progress + step);
            return (b - a).normalized;
        }
        public static void GetTangentByProgress(this IVectorByProgress vbp, double progress, out Vector3 tangent)
        {
            const float step = 1/128f;
            var lastAllowed = 1 - step;
            if (progress > lastAllowed) progress = lastAllowed;
            else if (progress < 0) progress = 0;
            var a = vbp.GetValueByProgress(progress);
            var b = vbp.GetValueByProgress(progress + step);
            tangent = (b - a).normalized;
        }

        public static float GetNearestPointAsProgress(this IVectorByProgress vbp, Vector3 from)
        {
            if (vbp.Type == PathType.SingleValue) return 0;

            var step = 1/16f;
            var lastMinDistanceSquared = float.MaxValue;
            var resultProgress = 0f;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                var distanceSquared = fun.distanceSquared.Between(in from, in curr);
                if (distanceSquared < lastMinDistanceSquared)
                {
                    lastMinDistanceSquared = distanceSquared;
                    resultProgress = p;
                }
            }
            for (var i = 0; i < 4; ++i)
            {
                step = step*0.5f;
                if (resultProgress > 0)
                {
                    var p = resultProgress - step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                    }
                }
                if (resultProgress < 1)
                {
                    var p = resultProgress + step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                    }
                }
            }

            return resultProgress;
        }
        public static float GetNearestPointAsProgress(this IVectorByProgress vbp, ref Vector3 from)
        {
            if (vbp.Type == PathType.SingleValue) return 0;

            var step = 1/16f;
            var lastMinDistanceSquared = float.MaxValue;
            var resultProgress = 0f;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                var distanceSquared = 
                    fun.distanceSquared.Between(in from, in curr);
                if (distanceSquared < lastMinDistanceSquared)
                {
                    lastMinDistanceSquared = distanceSquared;
                    resultProgress = p;
                }
            }
            for (var i = 0; i < 4; ++i)
            {
                step = step*0.5f;
                if (resultProgress > 0)
                {
                    var p = resultProgress - step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                    }
                }
                if (resultProgress < 1)
                {
                    var p = resultProgress + step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                    }
                }
            }

            return resultProgress;
        }
        public static float GetNearestPointTo(this IVectorByProgress vbp, ref Vector3 from, out Vector3 resultPoint)
        {
            resultPoint = v3.zero;
            if (vbp.Type == PathType.SingleValue)
            {
                resultPoint = vbp.GetValueByProgress(0);
                return 0;
            }

            var step = 1/16f;
            var lastMinDistanceSquared = float.MaxValue;
            var resultProgress = 0f;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                var distanceSquared = 
                    fun.distanceSquared.Between(in from, in curr);
                if (distanceSquared < lastMinDistanceSquared)
                {
                    lastMinDistanceSquared = distanceSquared;
                    resultProgress = p;
                    resultPoint = curr;
                }
            }
            for (var i = 0; i < 4; ++i)
            {
                step = step*0.5f;
                if (resultProgress > 0)
                {
                    var p = resultProgress - step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
                if (resultProgress < 1)
                {
                    var p = resultProgress + step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
            }

            return resultProgress;
        }
        public static Vector3 GetNearestPointTo(this IVectorByProgress vbp, ref Vector3 from)
        {
            if (vbp.Type == PathType.SingleValue) return vbp.GetValueByProgress(0);

            var step = 1/16f;
            var lastMinDistanceSquared = float.MaxValue;
            var resultProgress = 0f;
            Vector3 resultPoint = v3.zero;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                var distanceSquared = 
                    fun.distanceSquared.Between(in from, in curr);
                if (distanceSquared < lastMinDistanceSquared)
                {
                    lastMinDistanceSquared = distanceSquared;
                    resultProgress = p;
                    resultPoint = curr;
                }
            }
            for (var i = 0; i < 4; ++i)
            {
                step = step*0.5f;
                if (resultProgress > 0)
                {
                    var p = resultProgress - step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
                if (resultProgress < 1)
                {
                    var p = resultProgress + step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
            }

            return resultPoint;
        }
        public static Vector3 GetNearestPointTo(this IVectorByProgress vbp, Vector3 from)
        {
            if (vbp.Type == PathType.SingleValue) return vbp.GetValueByProgress(0);

            var step = 1/16f;
            var lastMinDistanceSquared = float.MaxValue;
            var resultProgress = 0f;
            Vector3 resultPoint = v3.zero;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                var distanceSquared = 
                    fun.distanceSquared.Between(in from, in curr);
                if (distanceSquared < lastMinDistanceSquared)
                {
                    lastMinDistanceSquared = distanceSquared;
                    resultProgress = p;
                    resultPoint = curr;
                }
            }
            for (var i = 0; i < 4; ++i)
            {
                step = step*0.5f;
                if (resultProgress > 0)
                {
                    var p = resultProgress - step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
                if (resultProgress < 1)
                {
                    var p = resultProgress + step;
                    var curr = vbp.GetValueByProgress(p);
                    var distanceSquared = 
                        fun.distanceSquared.Between(in from, in curr);
                    if (distanceSquared < lastMinDistanceSquared)
                    {
                        lastMinDistanceSquared = distanceSquared;
                        resultProgress = p;
                        resultPoint = curr;
                    }
                }
            }

            return resultPoint;
        }
        public static VectorEachFrame<T> PreUpdate<T>(this T vbp, Action<float,T> updateEachFrame) where T : IVectorByProgress
        {
            return new VectorEachFrame<T>(vbp, updateEachFrame);
        }
        public static float GetLength(this IVectorByProgress vbp, int segments = 16)
        {
            if (segments < 4) segments = 4;
            else if (segments > 512) segments = 512;
            if (vbp.Type == PathType.SingleValue)
            {
                return 0;
            }
            if (vbp.Type == PathType.Line)
            {
                var line = (LinearPath)vbp;
                return fun.distance.Between(line.Start, line.End);
            }
            var step = 1/(float)segments;
            var length = 0f;
            Vector3 prev = v3.zero;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                if (p > 0)
                {
                    length += fun.distance.Between(in prev, in curr);
                }
                prev = curr;
            }
            return length;
        }
        public static float GetLengthIgnoreY(this IVectorByProgress vbp)
        {
            if (vbp.Type == PathType.SingleValue)
            {
                return 0;
            }
            if (vbp.Type == PathType.Line)
            {
                var line = (LinearPath)vbp;
                return fun.distance.BetweenIgnoreY(line.Start, line.End);
            }
            const float step = 1/16f;
            var length = 0f;
            Vector3 prev = v3.zero;
            for (var p = 0f; p <= 1f; p += step)
            {
                var curr = vbp.GetValueByProgress(p);
                if (p > 0)
                {
                    length += fun.distance.BetweenIgnoreY(in prev, in curr);
                }
                prev = curr;
            }
            return length;
        }
#if UNIANIO_DEBUG
        public static IVectorByProgress DebugDrawLocal(this IVectorByProgress vbp, Transform t)
        {
            var i = t.GetHashCode();

            if (vbp.Type == PathType.QuadraticBezier)
            {
                var b = (QuadraticBezier)vbp;

                dbg.DrawLine(++i, b.Start.AsWorldPoint(t), b.Control.AsWorldPoint(t), Color.grey);
                dbg.DrawLine(++i, b.Control.AsWorldPoint(t), b.End.AsWorldPoint(t), Color.grey);
            }
            else if (vbp.Type == PathType.CubicBezier)
            {
                var b = (CubicBezier)vbp;

                dbg.DrawLine(++i, b.Start.AsWorldPoint(t), b.Control1.AsWorldPoint(t), Color.grey);
                dbg.DrawLine(++i, b.Control2.AsWorldPoint(t), b.End.AsWorldPoint(t), Color.grey);
            }


            Vector3 prev = Vector3.zero;
            for (var x = 0.0; x <= 1.0; x += 0.03)
            {
                var next = vbp.GetValueByProgress(x).AsWorldPoint(t);

                if (x > 0) dbg.DrawLine(++i, prev, next, Color.red);

                prev = next;
            }
            return vbp;
        }
        public static IVectorByProgress DebugDraw(this IVectorByProgress vbp)
        {
            var i = vbp.GetHashCode();

            if (vbp.Type == PathType.QuadraticBezier)
            {
                var b = (QuadraticBezier)vbp;

                dbg.DrawLine(++i, b.Start, b.Control, Color.grey);
                dbg.DrawLine(++i, b.Control, b.End, Color.grey);
            }
            else if (vbp.Type == PathType.CubicBezier)
            {
                var b = (CubicBezier)vbp;

                dbg.DrawLine(++i, b.Start, b.Control1, Color.grey);
                dbg.DrawLine(++i, b.Control2, b.End, Color.grey);
            }


            Vector3 prev = Vector3.zero;
            for (var x = 0.0; x <= 1.0; x += 0.03)
            {
                var next = vbp.GetValueByProgress(x);

                if(x > 0) dbg.DrawLine(++i, prev, next, Color.red);

                prev = next;
            }
            return vbp;
        }
#endif // #if UNIANIO_DEBUG
        public static DrawPointByProgressAni DrawAni(this IVectorByProgress vbp, double seconds, Color lineColor, Color controlColor)
        {
            var factory = GlobalFactory.Default;
            var ani = factory
                .Get<DrawPointByProgressAni>()
                .Set(seconds, vbp, lineColor, controlColor, false);
            factory.Get<IMessenger>()
                .Invoke(new PlayAni { Ani = ani });
            return ani;
        }
        public static DrawPointByProgressAni DrawAni(this IVectorByProgress vbp, Color color)
        {
            return vbp.DrawAni(9999, color, Color.gray);
        }
        public static DrawPointByProgressAni DrawAni(this IVectorByProgress vbp)
        {
            return vbp.DrawAni(9999, Color.red, Color.gray);
        }
        public static List<Vector3> GetPoints(this IVectorByProgress vbp, int number)
        {
            if(number < 2) throw new ArgumentException("Number of points must be more than 1");
            if(number == 2) return new List<Vector3> { vbp.GetValueByProgress(0), vbp.GetValueByProgress(1) };
            var list = new List<Vector3>();
            var max = (double)(number - 1);
            for (var i = 0; i < number; ++i)
            {
                list.Add(vbp.GetValueByProgress(i/max));
            }
            return list;
        }

        public static CubicBezier GetMirrorCopy(this CubicBezier cb, Vector3 planePoint, Vector3 planeNormal)
        {
            var st = cb.Start;
            var c1 = cb.Control1;
            var c2 = cb.Control2;
            var en = cb.End;

            var stp = fun.point.ProjectOnPlane(in st, in planeNormal, in planePoint);
            var c1p = fun.point.ProjectOnPlane(in c1, in planeNormal, in planePoint);
            var c2p = fun.point.ProjectOnPlane(in c2, in planeNormal, in planePoint);
            var enp = fun.point.ProjectOnPlane(in en, in planeNormal, in planePoint);

            return new CubicBezier(
                        fun.point.MoveAbs(in st, in stp, fun.distance.Between(in st, in stp)*2),
                        fun.point.MoveAbs(in c1, in c1p, fun.distance.Between(in c1, in c1p)*2),
                        fun.point.MoveAbs(in c2, in c2p, fun.distance.Between(in c2, in c2p)*2),
                        fun.point.MoveAbs(in en, in enp, fun.distance.Between(in en, in enp)*2)
                    );
        }
        public static CubicBezier GetMirrorCopy(this CubicBezier cb, ref Vector3 planePoint, ref Vector3 planeNormal)
        {
            var st = cb.Start;
            var c1 = cb.Control1;
            var c2 = cb.Control2;
            var en = cb.End;

            var stp = fun.point.ProjectOnPlane(in st, in planeNormal, in planePoint);
            var c1p = fun.point.ProjectOnPlane(in c1, in planeNormal, in planePoint);
            var c2p = fun.point.ProjectOnPlane(in c2, in planeNormal, in planePoint);
            var enp = fun.point.ProjectOnPlane(in en, in planeNormal, in planePoint);

            return new CubicBezier(
                        fun.point.MoveAbs(in st, in stp, fun.distance.Between(in st, in stp)*2),
                        fun.point.MoveAbs(in c1, in c1p, fun.distance.Between(in c1, in c1p)*2),
                        fun.point.MoveAbs(in c2, in c2p, fun.distance.Between(in c2, in c2p)*2),
                        fun.point.MoveAbs(in en, in enp, fun.distance.Between(in en, in enp)*2)
                    );
        }
        public static QuadraticBezier GetMirrorCopy(this QuadraticBezier cb, Vector3 planePoint, Vector3 planeNormal)
        {
            var st = cb.Start;
            var c0 = cb.Control;
            var en = cb.End;

            Vector3 stp, c0p, enp;
            fun.point.ProjectOnPlane(in st, in planeNormal, in planePoint, out stp);
            fun.point.ProjectOnPlane(in c0, in planeNormal, in planePoint, out c0p);
            fun.point.ProjectOnPlane(in en, in planeNormal, in planePoint, out enp);

            return new QuadraticBezier(
                        fun.point.MoveAbs(in st, in stp, fun.distance.Between(in st, in stp)*2),
                        fun.point.MoveAbs(in c0, in c0p, fun.distance.Between(in c0, in c0p)*2),
                        fun.point.MoveAbs(in en, in enp, fun.distance.Between(in en, in enp)*2)
                    );
        }
        public static QuadraticBezier GetMirrorCopy(this QuadraticBezier cb, ref Vector3 planePoint, ref Vector3 planeNormal)
        {
            var st = cb.Start;
            var c0 = cb.Control;
            var en = cb.End;

            Vector3 stp, c0p, enp;
            fun.point.ProjectOnPlane(in st, in planeNormal, in planePoint, out stp);
            fun.point.ProjectOnPlane(in c0, in planeNormal, in planePoint, out c0p);
            fun.point.ProjectOnPlane(in en, in planeNormal, in planePoint, out enp);

            return new QuadraticBezier(
                        fun.point.MoveAbs(in st, in stp, fun.distance.Between(in st, in stp)*2),
                        fun.point.MoveAbs(in c0, in c0p, fun.distance.Between(in c0, in c0p)*2),
                        fun.point.MoveAbs(in en, in enp, fun.distance.Between(in en, in enp)*2),
                        cb.Func
                    );
        }
        public static LinearPath GetMirrorCopy(this LinearPath lp, Vector3 planePoint, Vector3 planeNormal)
        {
            var st = lp.Start;
            var en = lp.End;

            Vector3 stp, enp;
            fun.point.ProjectOnPlane(in st, in planeNormal, in planePoint, out stp);
            fun.point.ProjectOnPlane(in en, in planeNormal, in planePoint, out enp);

            return new LinearPath(
                        fun.point.MoveAbs(in st, in stp, fun.distance.Between(in st, in stp)*2),
                        fun.point.MoveAbs(in en, in enp, fun.distance.Between(in en, in enp)*2),
                        lp.Func
                    );
        }
    }
}