using System;
using System.Text;
using Unianio.Enums;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Graphs
{
    public static class sbp // scalar by progress
    {
        public static RangeByProgress Range(double from, double to)
        {
            return new RangeByProgress(from, to);
        }
        public static RangeByProgress Range(double from, double to, Func<double,double> func)
        {
            return new RangeByProgress(from, to, func);
        }
    }
    public static class vbp // vector by progress
    {
        public static QuadraticBezier CurveRelToStart(Vector3 start, Vector3 control, Vector3 end)
        {
            return new QuadraticBezier(start, start+control, start+end);
        }
        public static CubicBezier CurveRelToStart(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
        {
            return new CubicBezier(start, start+control1, start+control2, start+end);
        }
        public static QuadraticBezier CurveControlRelToStart(Vector3 start, Vector3 control, Vector3 end)
        {
            return new QuadraticBezier(start, start+control, end);
        }
        public static QuadraticBezier CurveControlRelToStart(Vector3 start, Vector3 control, Vector3 end, Func<double,double> function)
        {
            return new QuadraticBezier(start, start+control, end, function);
        }
        public static CubicBezier CurveControlsRelToStart(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
        {
            return new CubicBezier(start, start+control1, start+control2, end);
        }
        public static QuadraticBezier CurveControlRelToEnd(Vector3 start, Vector3 control, Vector3 end)
        {
            return new QuadraticBezier(start, end+control, end);
        }
        public static QuadraticBezier CurveControlRelToEnd(Vector3 start, Vector3 control, Vector3 end, Func<double,double> function)
        {
            return new QuadraticBezier(start, end+control, end, function);
        }
        public static QuadraticBezier CurveControlRelToMid(Vector3 start, Vector3 control, Vector3 end)
        {
            return new QuadraticBezier(start, Vector3.Lerp(start,end,0.5f)+control, end);
        }
        public static DynamicQuadraticBezier CurveControlRelToMid(Vector3 start, Func<Vector3> getControl, Func<Vector3> getEnd)
        {
            return new DynamicQuadraticBezier(start, () => Vector3.Lerp(start, getEnd(), 0.5f) + getControl(), getEnd);
        }
        public static QuadraticBezier CurveControlRelToMid(Vector3 start, Vector3 control, Vector3 end, Func<double,double> function)
        {
            return new QuadraticBezier(start, Vector3.Lerp(start,end,0.5f)+control, end, function);
        }
        public static CubicBezier CurveControlsRelToEnd(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
        {
            return new CubicBezier(start, end+control1, end+control2, end);
        }
        public static CubicBezier CurveControlsRelToEdges(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
        {
            return new CubicBezier(start, start+control1, end+control2, end);
        }
        public static CubicBezier CurveControlsRelToEdges(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, Func<double,double> function)
        {
            return new CubicBezier(start, start+control1, end+control2, end,function);
        }
        public static LinearPath LineRelToStart(Vector3 start, Vector3 end)
        {
            return new LinearPath(start, start + end);
        }
        public static LinearPath LineRelToStart(Vector3 start, Vector3 end, Func<double,double> func)
        {
            return new LinearPath(start, start + end,func);
        }

        public static QuadraticBezier Curve(Vector3 start, Vector3 control, Vector3 end)
        {
            return new QuadraticBezier(start, control, end);
        }
        public static QuadraticBezier CurveOffAxis(Vector3 start, Vector3 end, double meters, Vector3 axis)
        {
            return new QuadraticBezier(start, fun.point.MidAwayFromAxis(in start, in end, meters, in axis), end);
        }
        public static QuadraticBezier CurveOffAxis(Vector3 start, Vector3 end, double meters, Vector3 axis, Vector3 axisPoint)
        {
            return new QuadraticBezier(start, fun.point.MidAwayFromAxis(in start, in end, meters, in axis, in axisPoint), end);
        }
        public static CubicBezier Curve(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end)
        {
            return new CubicBezier(start, control1, control2, end);
        }
        public static QuadraticBezier CurveAs(Vector3 start, Vector3 end, Func<Vector3,Vector3,Vector3> getControl)
        {
            return new QuadraticBezier(start, getControl(start,end), end);
        }
        public static CubicBezier CurveAs(Vector3 start, Vector3 end, Func<Vector3,Vector3,Vector3> getControl1, Func<Vector3,Vector3,Vector3> getControl2)
        {
            return new CubicBezier(start, getControl1(start,end), getControl2(start,end), end);
        }
        public static QuadraticBezier CurveAs(Vector3 start, Func<Vector3,Vector3> getEnd, Func<Vector3,Vector3,Vector3> getControl)
        {
            var end = getEnd(start);
            return new QuadraticBezier(start, getControl(start,end), end);
        }
        public static CubicBezier CurveAs(Vector3 start, Func<Vector3,Vector3> getEnd, Func<Vector3,Vector3,Vector3> getControl1, Func<Vector3,Vector3,Vector3> getControl2)
        {
            var end = getEnd(start);
            return new CubicBezier(start, getControl1(start,end), getControl2(start,end), end);
        }
        public static LinearPath Line(Vector3 startAndEnd)
        {
            return new LinearPath(startAndEnd, startAndEnd);
        }
        public static LinearPath Line(Vector3 start, Vector3 end)
        {
            return new LinearPath(start, end);
        }
        public static LinearPath LineAs(Vector3 start, Func<Vector3,Vector3> getEnd)
        {
            return new LinearPath(start, getEnd(start));
        }
        
        // to be used with PreUpdate and changed on each frame
        public static SphericalAnglePath Sphere(in Vector3 start)
        {
            return new SphericalAnglePath(start, start);
        }
        public static SphericalAnglePath Sphere(in Vector3 start, in Vector3 end)
        {
            return new SphericalAnglePath(start, end);
        }
        public static SphericalAnglePath Sphere(in Vector3 start, in Vector3 end, Func<double,double> func)
        {
            return new SphericalAnglePath(start, end, func);
        }
        public static SphericalAnglePathWithMovingEnd SphereWithMovingEnd(in Vector3 start, in Vector3 end0, in Vector3 end1)
        {
            return new SphericalAnglePathWithMovingEnd(start, end0, end1);
        }
        public static SphericalAnglePathWithMovingEnd SphereWithMovingEnd(in Vector3 start, in Vector3 end0, in Vector3 end1, Func<double, double> func)
        {
            return new SphericalAnglePathWithMovingEnd(start, end0, end1, func);
        }
        public static DualSphericalAnglePath TwoSpheres(in Vector3 start, in Vector3 middle, in Vector3 end)
        {
            return new DualSphericalAnglePath(start, middle, end);
        }
        public static DualSphericalAnglePath TwoSpheres(
            Vector3 start, Vector3 middle, Vector3 end, 
            Func<double,double> func1, Func<double,double> func2)
        {
            return new DualSphericalAnglePath(start, middle, end, func1, func2);
        }
        public static CirclePath Circle(in Vector3 start, in Vector3 center, in Vector3 normal, double degrees)
        {
            return new CirclePath(start, center, normal, degrees);
        }
        public static AngleAxisPath AngleAxis(in Vector3 vector, double degrees, in Vector3 axis)
        {
            return new AngleAxisPath(vector, degrees, axis);
        }
        public static AngleAxisEllipsePath AngleAxisEllipse(in Vector3 vector, double angle, Vector3 axis,
            double ellipseFactor, Vector3 ellipseDir, bool correctAxis)
        {
            return new AngleAxisEllipsePath(vector, angle, axis, ellipseFactor, ellipseDir, correctAxis);
        }
        public static AngleAxisPath AngleAxis(in Vector3 vector, double degrees, in Vector3 axis, Func<double,double> func)
        {
            return new AngleAxisPath(vector, degrees, axis, func);
        }
        public static VectorsAndAxisPath VectorsAndAxis(in Vector3 start, in Vector3 end, in Vector3 axis)
        {
            return new VectorsAndAxisPath(start, end, axis);
        }
        public static VectorsAndAxisPath VectorsAndAxis(in Vector3 start, in Vector3 end, in Vector3 axis, bool takeLongestPath)
        {
            return new VectorsAndAxisPath(start, end, axis, takeLongestPath, null, null);
        }
        /// <param name="start">initial vector</param>
        /// <param name="end">target vctor</param>
        /// <param name="axis">rotation axis</param>
        /// <param name="takeLongestPath">if true rotates by longest</param>
        /// <param name="rotationFunc">rotation function (progress01 => progress01)</param>
        /// <param name="fromAxisFunc">distance from axis function (from_degrees,to_degrees,progress => degrees)</param>
        public static VectorsAndAxisPath VectorsAndAxis(
            in Vector3 start, in Vector3 end, in Vector3 axis, bool takeLongestPath, 
            Func<double,double> rotationFunc, Func<double,double,double,double> fromAxisFunc)
        {
            return new VectorsAndAxisPath(start, end, axis, takeLongestPath, rotationFunc, fromAxisFunc);
        }
        public static AngleAxisEllipsePath AngleAxisEllipse(in Vector3 vector, double angle, in Vector3 axis,
            double ellipseFactor, Vector3 ellipseDir, bool correctAxis, Func<double,double> func)
        {
            return new AngleAxisEllipsePath(vector, angle, axis, ellipseFactor, ellipseDir, correctAxis,func);
        }
        public static ComplexVectorPath Complex(params IVectorByProgressInRange[] paths)
        {
            return new ComplexVectorPath(paths);
        }
        public static ComplexQuaternionPath Complex(params IQuaternionByProgressInRange[] paths)
        {
            return new ComplexQuaternionPath(paths);
        }
        public static RotateByProgress Rotate(Func<double,Quaternion> eachFrame)
        {
            return new RotateByProgress(eachFrame);
        }
        public static QuaternionSlerpByProgress Rotate(in Quaternion from, in Quaternion to)
        {
            return new QuaternionSlerpByProgress(from, to);
        }
        public static RotateByProgress Rotate(Quaternion from, Quaternion to, Func<double,double> func)
        {
            if(func == null) func = x => x;
            return new RotateByProgress(x => Quaternion.Slerp(from, to, (float)func(x)));
        }
        // armMove is quadratic bezier in model space
        //vbp.Wrapped(armMove, (x,o)=>o.GetValueByProgress(x).AsWorldPoint(m)).DrawAni();
        public static WrappedPath Wrapped(IVectorByProgress child, Func<double, IVectorByProgress, Vector3> wrapper) 
        {
            return new WrappedPath(child, wrapper);
        }
        public static WrappedRotation WrappedRotation(IQuaternionByProgress child, Func<double, IQuaternionByProgress, Quaternion> wrapper)
        {
            return new WrappedRotation(child, wrapper);
        }
        public static VectorEachFrame<T> PreUpdate<T>(T child, Action<float,T> updateEachFrame) 
            where T : IVectorByProgress
        {
            return new VectorEachFrame<T>(child, updateEachFrame);
        }
        public static SingleVector Single(in Vector3 value)
        {
            return new SingleVector(value);
        }
        public static SingleQuaternion Single(in Quaternion value)
        {
            return new SingleQuaternion(value);
        }
        public static VectorByProgressInRange InRange(double from, double to, IVectorByProgress source)
        {
            return new VectorByProgressInRange(from, to, source);
        }
        public static DynamicVector DynamicVector(Func<double,Vector3> func)
        {
            return new DynamicVector(func);
        }
        public static DynamicRotation DynamicRotation(Func<double,Quaternion> func)
        {
            return new DynamicRotation(func);
        }
        public static QuadraticBezier Curve(in Vector3 start, in Vector3 control, in Vector3 end, Func<double,double> func)
        {
            return new QuadraticBezier(start, control, end, func);
        }public static CubicBezier Curve(in Vector3 start, in Vector3 control1, in Vector3 control2, in Vector3 end, Func<double,double> func)
        {
            return new CubicBezier(start, control1, control2, end, func);
        }
        public static LinearPath Line(in Vector3 start, in Vector3 end, Func<double,double> func)
        {
            return new LinearPath(start, end, func);
        }
        public static LinearPath LineAs(in Vector3 start, Func<Vector3,Vector3> getEnd, Func<double,double> func)
        {
            return new LinearPath(start, getEnd(start), func);
        }


        public static LinearPath2D Line2D(in Vector2 start, in Vector2 end, Func<double, double> func = null)
        {
            return new LinearPath2D(start, end, func);
        }
        public static SingleVector Point(in Vector3 point)
        {
            return (SingleVector)point;
        }
        public static RangeByProgress Numbers(double start, double end, Func<double,double> function = null)
        {
            return new RangeByProgress(start, end, function);
        }

        public static IVectorByProgress ComputeUp(in Vector3 generalUp, Func<Vector3> getForward)
        {
            return new UpVectorComputer(generalUp, getForward);
        }

        public class UpVectorComputer : IVectorByProgress
        {
            private Vector3 _generalUp;
            private readonly Func<Vector3> _getForward;
            public UpVectorComputer(Vector3 generalUp, Func<Vector3> getForward)
            {
                _generalUp = generalUp;
                _getForward = getForward;
            }
            public Func<double, double> Func
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            PathType IVectorByProgress.Type => PathType.SphericalAnglePath;

            Vector3 IVectorByProgress.GetValueByProgress(double progress)
            {
                var fw = _getForward();
                Vector3 up;
                fun.vector.GetRealUp(in fw, in _generalUp, out up);
                return up;
            }
            Vector3 IVectorByProgress.GetDirectionByProgress(double progress) { throw new NotImplementedException(); }
        }
    }
}