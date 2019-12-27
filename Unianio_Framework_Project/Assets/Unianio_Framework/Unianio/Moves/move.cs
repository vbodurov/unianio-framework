using System;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Moves
{
    public static class move
    {
        public static LineMove Line(in Vector3 from, in Vector3 to, Func<double, double> func = null)
        {
            return new LineMove(from, to, func);
        }
        public static LineMove Line(Func<LineMove, Vector3> from, Func<LineMove, Vector3> to, Func<double, double> func = null)
        {
            return new LineMove(from, to, func);
        }

        public static QuadraticBezierMove Curve(in Vector3 from, in Vector3 control, in Vector3 to, Func<double, double> func = null)
        {
            return new QuadraticBezierMove(from, control, to, func);
        }
        public static QuadraticBezierMove Curve(
            Func<QuadraticBezierMove, Vector3> from, 
            Func<QuadraticBezierMove, Vector3> control,
            Func<QuadraticBezierMove, Vector3> to, Func<double, double> func = null)
        {
            return new QuadraticBezierMove(from, control, to, func);
        }

        public static CubicBezierMove Curve(in Vector3 from, in Vector3 control1, in Vector3 control2, in Vector3 to, Func<double, double> func = null)
        {
            return new CubicBezierMove(from, control1, control2, to, func);
        }
        public static CubicBezierMove Curve(
            Func<CubicBezierMove, Vector3> from,
            Func<CubicBezierMove, Vector3> control1,
            Func<CubicBezierMove, Vector3> control2,
            Func<CubicBezierMove, Vector3> to, Func<double, double> func = null)
        {
            return new CubicBezierMove(from, control1, control2, to, func);
        }
        
        public static RotationMove Rotate(in Quaternion from, in Quaternion to, Func<double, double> func = null)
        {
            return new RotationMove(from, to, func);
        }
        public static RotationMove Rotate(
            in Vector3 fromFw, in Vector3 fromUp, 
            in Vector3 toFw, in Vector3 toUp, Func<double, double> func = null)
        {
            return new RotationMove(
                Quaternion.LookRotation(fromFw, fromFw.GetRealUp(in fromUp)),
                Quaternion.LookRotation(toFw, toFw.GetRealUp(in toUp)), func);
        }
        public static RotationMove Rotate(Func<RotationMove, Quaternion> from, Func<RotationMove, Quaternion> to, Func<double, double> func = null)
        {
            return new RotationMove(from, to, func);
        }


        public static NumberMove Number(double from, double to, Func<double, double> func = null)
        {
            return new NumberMove(from, to, func);
        }
        public static NumberMove Number(Func<NumberMove, double> from, Func<NumberMove, double> to, Func<double, double> func = null)
        {
            return new NumberMove(from, to, func);
        }

        public static VectorByProgressInRange InRange(double from, double to, IVectorByProgress progress)
        {
            return new VectorByProgressInRange(from, to, progress);
        }
        public static QuaternionByProgressInRange InRange(double from, double to, IQuaternionByProgress progress)
        {
            return new QuaternionByProgressInRange(from, to, progress);
        }


        public static CompositeVectorMove Composite(params IVectorByProgressInRange[] nodes)
        {
            return new CompositeVectorMove(nodes);
        }
        public static CompositeQuaternionMove Composite(params IQuaternionByProgressInRange[] nodes)
        {
            return new CompositeQuaternionMove(nodes);
        }

    }
}