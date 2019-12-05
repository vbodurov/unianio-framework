//TODO:enable
/*using System;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.IK;
using Unianio.Rigged.IK;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Graphs
{
    public class HandlePath : IExecutorOfProgress
    {
        readonly IManipulator3D _manipulator;
        IVectorByProgress _pos, _forward, _up, _scale;
        IQuaternionByProgress _rotate;
        HandleSpace _posSpace = HandleSpace.None;
        HandleSpace _rotWithTargetSpace = HandleSpace.None;
        HandleSpace _rotWithQuaternionSpace = HandleSpace.None;
        HandleSpace _scaSpace = HandleSpace.None;
        Func<bool> _canApply;
        

        public HandlePath(IManipulator3D manipulator)
        {
            _manipulator = manipulator;
            InitialOrientationHolder = _manipulator as IInitialOrientationHolder;
            ArmChain = _manipulator as HumArmChain;
            LegChain = _manipulator as HumLegChain;
        }
        public HumArmChain ArmChain { get; }
        public HumLegChain LegChain { get; }
        public IInitialOrientationHolder InitialOrientationHolder { get; }
        public IManipulator3D Manipulator => _manipulator;
        public int ID { get; set; }
        public HandlePath Clone => new HandlePath(_manipulator);
        public IVectorByProgress PositionChange => _pos;
        public IVectorByProgress ForwardChange => _forward;
        public IVectorByProgress UpChange => _up;
        public IVectorByProgress ScaleChange => _scale;
        public HandleSpace PositionSpace => _posSpace;
        public HandleSpace RotationWithTargetSpace => _rotWithTargetSpace;
        public HandleSpace RotationWithQuaternionSpace => _rotWithQuaternionSpace;

        public HandlePath New
        {
            get
            {
                _posSpace = HandleSpace.None;
                _rotWithTargetSpace = HandleSpace.None;
                _rotWithQuaternionSpace = HandleSpace.None;
                _scaSpace = HandleSpace.None;
                _canApply = null;
                return this;
            }
        }
        public HandlePath Void() => this;
        public HandlePath SetCondition(Func<bool> canApply)
        {
            _canApply = canApply;
            return this;
        }
        public HandlePath BindWorldRotationTo(Transform obj)
        {
            obj.rotation = _manipulator.Manipulator.rotation;
            return this;
        }
        public HandlePath LocalSca(IVectorByProgress scaInLocalSpace)
        {
            _scaSpace = HandleSpace.Local;
            _scale = scaInLocalSpace;
            return this;
        }
        public HandlePath WorldPos(IVectorByProgress posInWorldSpace)
        {
            _posSpace = HandleSpace.World;
            _pos = posInWorldSpace;
            return this;
        }
        public HandlePath WorldPosFixed()
        {
            WorldPos(vbp.Single(_manipulator.Manipulator.position));
            return this;
        }
        public HandlePath WorldRotFixed()
        {
            WorldRot(vbp.Single(_manipulator.Manipulator.forward), vbp.Single(_manipulator.Manipulator.up));
            return this;
        }
        public HandlePath ModelPosFixed()
        {
            ModelPos(vbp.Single(_manipulator.Manipulator.position.AsLocalPoint(_manipulator.Model)));
            return this;
        }
        public HandlePath ModelRotFixed()
        {
            ModelRot(vbp.Single(_manipulator.Manipulator.forward.AsLocalDir(_manipulator.Model)), vbp.Single(_manipulator.Manipulator.up.AsLocalDir(_manipulator.Model)));
            return this;
        }
        public HandlePath LocalPosFixed()
        {
            LocalPos(vbp.Single(_manipulator.LocalPos));
            return this;
        }
        public HandlePath LocalRotFixed()
        {
            LocalRot(vbp.Single(_manipulator.LocalFw), vbp.Single(_manipulator.LocalUp));
            return this;
        }
        public HandlePath WorldRot(IVectorByProgress forwardInWorldSpace, IVectorByProgress upInWorldSpace)
        {
            _rotWithTargetSpace = HandleSpace.World;
            _forward = forwardInWorldSpace;
            _up = upInWorldSpace;
            return this;
        }
        public HandlePath WorldRot(IQuaternionByProgress rotateInWorldSpace)
        {
            _rotWithQuaternionSpace = HandleSpace.World;
            _rotate = rotateInWorldSpace;
            return this;
        }
        public HandlePath ModelPos(IVectorByProgress posInModelSpace)
        {
            _posSpace = HandleSpace.Model;
            _pos = posInModelSpace;
            return this;
        }
        public HandlePath ModelRot(IVectorByProgress forwardInModelSpace, IVectorByProgress upInModelSpace)
        {
            _rotWithTargetSpace = HandleSpace.Model;
            _forward = forwardInModelSpace;
            _up = upInModelSpace;
            return this;
        }
        public HandlePath ModelPosLineToTarget(Vector3 posInModel)
        {
            return ModelPos(vbp.Line(_manipulator.ModelPos, posInModel));
        }
        public HandlePath ToInitialInModel()
        {
            var holder = InitialOrientationHolder;
            if (holder == null) return this;
            ModelPos(
                vbp.Line(_manipulator.ModelPos, holder.IniModelPos));
            ModelRot(
                vbp.Rotate(
                    lookAt(_manipulator.ModelFw, _manipulator.ModelUp),
                    lookAt(holder.IniModelFw, holder.IniModelUp)));
            return this;
        }
        public HandlePath ToInitialPositionInModel()
        {
            var holder = InitialOrientationHolder;
            if (holder == null) return this;
            ModelPos(
                vbp.Line(_manipulator.ModelPos, holder.IniModelPos));
            return this;
        }
        public HandlePath ToInitialInLocal()
        {
            var holder = InitialOrientationHolder;
            if (holder == null) return this;
            LocalPos(
                vbp.Line(_manipulator.LocalPos, holder.IniLocalPos));
            LocalRot(
                vbp.Rotate(
                    lookAt(_manipulator.LocalFw, _manipulator.LocalUp),
                    lookAt(holder.IniLocalFw, holder.IniLocalUp)));
            return this;
        }
        public HandlePath ToInitialPositionInLocal()
        {
            var holder = InitialOrientationHolder;
            if (holder == null) return this;
            LocalPos(
                vbp.Line(_manipulator.LocalPos, holder.IniLocalPos));
            return this;
        }
        public HandlePath ModelPosTo2Targets(
            IVectorByProgress posInModel1, double progress2start, IVectorByProgress posInModel2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, posInModel1),
                    vbp.InRange(progress2start, 1, posInModel2));

            return ModelPos(complex);
        }
        public HandlePath ModelPosLineTo2Targets(
            Vector3 posInModel1, double progress2start,
            Vector3 posInModel2, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.ModelPos, posInModel1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(posInModel1, posInModel2, function)));

            return ModelPos(complex);
        }

        public HandlePath ModelPosLineTo2Targets(
            Vector3 posInModel1, double progress2start,
            Vector3 posInModel2, Func<double, double> function1, Func<double, double> function2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.ModelPos, posInModel1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(posInModel1, posInModel2, function2)));

            return ModelPos(complex);
        }

        public HandlePath ModelPosCurveTo2Targets(
            Vector3 posInModel1, Vector3 controlInModel1, double progress2start,
            Vector3 posInModel2, Vector3 controlInModel2, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Curve(_manipulator.ModelPos, controlInModel1, posInModel1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Curve(posInModel1, controlInModel2, posInModel2, function)));

            return ModelPos(complex);
        }
        public HandlePath ModelPosCurveTo2Targets(
            Vector3 posInModel1, Vector3 controlInModel1, double progress2start,
            Vector3 posInModel2, Vector3 controlInModel2, Func<double, double> function1, Func<double, double> function2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Curve(_manipulator.ModelPos, controlInModel1, posInModel1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Curve(posInModel1, controlInModel2, posInModel2, function2)));

            return ModelPos(complex);
        }

        public HandlePath LocalPosCurveTo2Targets(
            Vector3 posInLocal1, Vector3 controlInLocal1, double progress2start,
            Vector3 posInLocal2, Vector3 controlInLocal2, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Curve(_manipulator.LocalPos, controlInLocal1, posInLocal1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Curve(posInLocal1, controlInLocal2, posInLocal2, function)));

            return LocalPos(complex);
        }
        public HandlePath LocalPosCurveTo2Targets(
            Vector3 posInLocal1, Vector3 controlInLocal1, double progress2start,
            Vector3 posInLocal2, Vector3 controlInLocal2, Func<double, double> function1, Func<double, double> function2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Curve(_manipulator.LocalPos, controlInLocal1, posInLocal1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Curve(posInLocal1, controlInLocal2, posInLocal2, function2)));

            return LocalPos(complex);
        }

        public HandlePath WrapPos(Func<double, IVectorByProgress, Vector3> function)
        {
            _pos = new WrappedPath(_pos, function);
            return this;
        }
        public HandlePath WrapForward(Func<double, IVectorByProgress, Vector3> function)
        {
            _forward = new WrappedPath(_forward, function);
            return this;
        }
        public HandlePath WrapUp(Func<double, IVectorByProgress, Vector3> function)
        {
            _up = new WrappedPath(_up, function);
            return this;
        }

        public HandlePath ModelPosLineToTarget(Vector3 posInModel, Func<double, double> function)
        {
            return ModelPos(vbp.Line(_manipulator.ModelPos, posInModel, function));
        }

        public HandlePath ModelPosLineToTargetWrapped(Vector3 posInModel, Func<double, IVectorByProgress, Vector3> wrapperFunc)
        {
            return ModelPos(
                    vbp.Wrapped(
                            vbp.Line(_manipulator.ModelPos, posInModel),
                            wrapperFunc
                        )
                    );
        }

        public HandlePath ModelPosLineToTargetWrapped(Vector3 posInModel, Func<double, IVectorByProgress, Vector3> wrapperFunc, Func<double, double> function)
        {
            return ModelPos(
                    vbp.Wrapped(
                            vbp.Line(_manipulator.ModelPos, posInModel, function),
                            wrapperFunc
                        )
                    );
        }

        public HandlePath DynamicModelPos(Func<double, Vector3> func)
        {
            return ModelPos(vbp.DynamicVector(func));
        }
        public HandlePath DynamicModelRot(Func<double, Quaternion> func)
        {
            return ModelRot(vbp.DynamicRotation(func));
        }

        public HandlePath DynamicWorldPos(Func<double, Vector3> func)
        {
            return WorldPos(vbp.DynamicVector(func));
        }
        public HandlePath DynamicWorldRot(Func<double, Quaternion> func)
        {
            return WorldRot(vbp.DynamicRotation(func));
        }

        public HandlePath DynamicLocalPos(Func<double, Vector3> func)
        {
            return LocalPos(vbp.DynamicVector(func));
        }
        public HandlePath DynamicLocalRot(Func<double, Quaternion> func)
        {
            return LocalRot(vbp.DynamicRotation(func));
        }

        public HandlePath LocalPosLineToTarget(in Vector3 posInLocal)
        {
            return LocalPos(vbp.Line(_manipulator.LocalPos, posInLocal));
        }
        public HandlePath LocalRotOffElbowWithUp(Vector3 upInLocal, double powerToBlend = 8)
        {
            var arm = ArmChain;
            if(arm == null) throw new ArgumentException("LocalRotOffElbowWithUp can only be used with HumArmChain this is "+this.Manipulator);
            var iniLocRot = arm.Handle.localRotation;
            return LocalRot(vbp.DynamicRotation(x =>
            {
                var locFw = arm.Forearm.DirTo(arm.Handle).AsLocalDir(arm.Handle.parent);
                return slerp(in iniLocRot, lookAt(in locFw, locFw.GetRealUp(upInLocal)), 1 - pow(1 - x, powerToBlend));
            }));
        }
        public HandlePath LocalRotOffElbowWithUp(Vector3 upInLocal1, Vector3 upInLocal2, double powerToBlend = 8)
        {
            var arm = ArmChain;
            if (arm == null) throw new ArgumentException("LocalRotOffElbowWithUp can only be used with HumArmChain this is " + this.Manipulator);
            var iniLocRot = arm.Handle.localRotation;
            return LocalRot(vbp.DynamicRotation(x =>
            {
                var upInLocal = slerp2(iniLocRot * v3.up, upInLocal1, 0.5, upInLocal2, x);
                var locFw = arm.Forearm.DirTo(arm.Handle).AsLocalDir(arm.Handle.parent);
                return slerp(in iniLocRot, lookAt(in locFw, locFw.GetRealUp(upInLocal)), 1 - pow(1 - x, powerToBlend));
            }));
        }
        public HandlePath ModelRotOffElbowWithUp(Vector3 upInModel, double powerToBlend = 8)
        {
            var arm = ArmChain;
            if (arm == null) throw new ArgumentException("ModelRotOffElbowWithUp can only be used with HumArmChain this is " + this.Manipulator);
            var iniModRot = lookAt(arm.Handle.forward.AsLocalDir(arm.Model), arm.Handle.up.AsLocalDir(arm.Model));
            return ModelRot(vbp.DynamicRotation(x =>
            {
                var modFw = arm.Forearm.DirTo(arm.Handle).AsLocalDir(arm.Model);
                return slerp(in iniModRot, lookAt(in modFw, modFw.GetRealUp(upInModel)), 1 - pow(1 - x, powerToBlend));
            }));
        }
        public HandlePath ModelRotOffElbowWithUp(Vector3 upInLocal1, Vector3 upInLocal2, double powerToBlend = 8)
        {
            var arm = ArmChain;
            if (arm == null) throw new ArgumentException("ModelRotOffElbowWithUp can only be used with HumArmChain this is " + this.Manipulator);
            var iniModRot = lookAt(arm.Handle.forward.AsLocalDir(arm.Model), arm.Handle.up.AsLocalDir(arm.Model));
            return ModelRot(vbp.DynamicRotation(x =>
            {
                var upInModel = slerp2(iniModRot * v3.up, upInLocal1, 0.5, upInLocal2, x);
                var modFw = arm.Forearm.DirTo(arm.Handle).AsLocalDir(arm.Model);
                return slerp(in iniModRot, lookAt(in modFw, modFw.GetRealUp(upInModel)), 1 - pow(1 - x, powerToBlend));
            }));
        }
        public HandlePath LocalScaleLineToTarget(in Vector3 scaleInLocal)
        {
            return LocalSca(vbp.Line(_manipulator.LocalScale, scaleInLocal));
        }

        public HandlePath LocalPosLineToTarget(in Vector3 posInLocal, Func<double, double> function)
        {
            return LocalPos(vbp.Line(_manipulator.LocalPos, posInLocal, function));
        }
        public HandlePath LocalPosTo2Targets(
            IVectorByProgress posInLocal1, double progress2start, IVectorByProgress posInLocal2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, posInLocal1),
                    vbp.InRange(progress2start, 1, posInLocal2));

            return LocalPos(complex);
        }
        public HandlePath LocalPosLineTo2Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.LocalPos, posInLocal1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(posInLocal1, posInLocal2, function)));

            return LocalPos(complex);
        }

        public HandlePath LocalPosLineTo2Targets(
            IVectorByProgress path1, double progress2start,
            in Vector3 posInLocal2, Func<double, double> function2 = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, path1),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(path1.GetValueByProgress(1), posInLocal2, function2)));

            return LocalPos(complex);
        }
        public HandlePath LocalPosLineTo2Targets(
            in Vector3 posInLocal1, double progress2start,
            IVectorByProgress path2, Func<double, double> function1 = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.LocalPos, posInLocal1, function1)),
                    vbp.InRange(progress2start, 1, path2));

            return LocalPos(complex);
        }
        public HandlePath ModelPosLineTo2Targets(
            IVectorByProgress path1, double progress2start,
            in Vector3 posInLocal2, Func<double, double> function2 = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, path1),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(path1.GetValueByProgress(1), posInLocal2, function2)));

            return ModelPos(complex);
        }
        public HandlePath ModelPosLineTo2Targets(
            in Vector3 posInLocal1, double progress2start,
            IVectorByProgress path2, Func<double, double> function1 = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.ModelPos, posInLocal1, function1)),
                    vbp.InRange(progress2start, 1, path2));

            return ModelPos(complex);
        }

        public HandlePath LocalPosLineTo2Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, Func<double, double> function1, Func<double, double> function2)
        {
            var line1 = vbp.Line(_manipulator.LocalPos, posInLocal1, function1);
            var line2 = vbp.Line(line1.GetValueByProgress(1), posInLocal2, function2);

            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, line1),
                    vbp.InRange(progress2start, 1, line2));

            return LocalPos(complex);
        }
        public HandlePath LocalPosLineTo3Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, double progress3start,
            in Vector3 posInLocal3, Func<double, double> function = null)
        {
            var line1 = vbp.Line(_manipulator.LocalPos, posInLocal1, function);
            var line2 = vbp.Line(line1.GetValueByProgress(1), posInLocal2, function);
            var line3 = vbp.Line(line2.GetValueByProgress(1), posInLocal3, function);

            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, line1),
                    vbp.InRange(progress2start, progress3start, line2),
                    vbp.InRange(progress3start, 1, line3));

            return LocalPos(complex);
        }
        public HandlePath LocalPosLineTo3Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, double progress3start,
            in Vector3 posInLocal3, Func<double, double> function1, Func<double, double> function2, Func<double, double> function3)
        {
            var line1 = vbp.Line(_manipulator.LocalPos, posInLocal1, function1);
            var line2 = vbp.Line(line1.GetValueByProgress(1), posInLocal2, function2);
            var line3 = vbp.Line(line2.GetValueByProgress(1), posInLocal3, function3);

            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start, line1),
                    vbp.InRange(progress2start, progress3start, line2),
                    vbp.InRange(progress3start, 1, line3));

            return LocalPos(complex);
        }
        public HandlePath ModelPosLineTo3Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, double progress3start,
            in Vector3 posInLocal3, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.ModelPos, posInLocal1, function)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Line(posInLocal1, posInLocal2, function)),
                    vbp.InRange(progress3start, 1,
                        vbp.Line(posInLocal2, posInLocal3, function)));

            return ModelPos(complex);
        }
        public HandlePath ModelPosLineTo3Targets(
            in Vector3 posInLocal1, double progress2start,
            in Vector3 posInLocal2, double progress3start,
            in Vector3 posInLocal3, Func<double, double> function1, Func<double, double> function2, Func<double, double> function3)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.ModelPos, posInLocal1, function1)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Line(posInLocal1, posInLocal2, function2)),
                    vbp.InRange(progress3start, 1,
                        vbp.Line(posInLocal2, posInLocal3, function3)));

            return ModelPos(complex);
        }

        public HandlePath LocalPosCurveToTargetRelToMiddle(
            in Vector3 targetInLocal, in Vector3 control1RelToMiddle, 
            Func<double, double> function = null)
        {
            var dist = distance.Between(_manipulator.LocalPos, targetInLocal);
            return LocalPos(vbp.Curve(_manipulator.LocalPos, 
                    Vector3.Lerp(_manipulator.LocalPos, targetInLocal, 0.5f) + control1RelToMiddle * dist,
                targetInLocal, function));
        }
        public HandlePath LocalPosCurveToTargetRelToMiddle(in Vector3 targetInLocal,
            in Vector3 control1Dir, double relControl1Len,
            Func<double, double> function = null)
        {
            return LocalPosCurveToTargetRelToMiddle(in targetInLocal,
                in control1Dir, relControl1Len,
                out var curve, function);
        }
        public HandlePath LocalPosCurveToTargetRelToMiddle(in Vector3 targetInLocal,
            in Vector3 control1Dir, double relControl1Len,
            in Vector3 control2Dir, double relControl2Len,
            Func<double, double> function = null)
        {
            return LocalPosCurveToTargetRelToMiddle(in targetInLocal,
                in control1Dir, relControl1Len,
                in control2Dir, relControl2Len,
                out var curve, function);
        }
        public HandlePath LocalPosCurveToTargetRelToMiddle(in Vector3 targetInLocal,
            in Vector3 control1Dir, double relControl1Len,
            out QuadraticBezier curve,
            Func<double, double> function = null)
        {
            var dist = distance.Between(_manipulator.LocalPos, targetInLocal);
            return LocalPos(curve = vbp.Curve(_manipulator.LocalPos,
                lerp(_manipulator.LocalPos, in targetInLocal, 0.5f) + control1Dir * (float)(dist * relControl1Len),
                targetInLocal, function));
        }
        public HandlePath LocalPosCurveToTargetRelToMiddle(in Vector3 targetInLocal,
            in Vector3 control1Dir, double relControl1Len,
            in Vector3 control2Dir, double relControl2Len,
            out CubicBezier curve,
            Func<double, double> function = null)
        {
            var dist = distance.Between(_manipulator.LocalPos, in targetInLocal);
            return LocalPos(curve = vbp.Curve(_manipulator.LocalPos,
                lerp(_manipulator.LocalPos, in targetInLocal, 0.3333f) + control1Dir * (float)(dist * relControl1Len),
                lerp(_manipulator.LocalPos, in targetInLocal, 0.6666f) + control2Dir * (float)(dist * relControl2Len),
                targetInLocal, function));
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(
            in Vector3 targetInModel, in Vector3 control1RelToMiddle, 
            Func<double, double> function = null)
        {
            var dist = fun.distance.Between(_manipulator.ModelPos, targetInModel);
            return ModelPos(vbp.Curve(_manipulator.ModelPos, 
                lerp(_manipulator.ModelPos, in targetInModel, 0.5f) + control1RelToMiddle * dist,
                targetInModel, function));
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(in Vector3 targetInModel,
            in Vector3 control1Dir, double relControl1Len)
        {
            return ModelPosCurveToTargetRelToMiddle(in targetInModel,
                in control1Dir, relControl1Len,
                out var curve, null);
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(in Vector3 targetInModel,
            in Vector3 control1Dir, double relControl1Len, 
            Func<double, double> function)
        {
            return ModelPosCurveToTargetRelToMiddle(in targetInModel,
                in control1Dir, relControl1Len,
                out var curve, function);
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(in Vector3 targetInModel,
            in Vector3 control1Dir, double relControl1Len,
            in Vector3 control2Dir, double relControl2Len, 
            Func<double, double> function = null)
        {
            return ModelPosCurveToTargetRelToMiddle(in targetInModel,
                in control1Dir, relControl1Len,
                in control2Dir, relControl2Len,
                out var curve, function);
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(in Vector3 targetInModel,
            in Vector3 control1Dir, double relControl1Len,
            out QuadraticBezier curve,
            Func<double, double> function = null)
        {
            var dist = distance.Between(_manipulator.ModelPos, targetInModel);
            return ModelPos(curve = vbp.Curve(_manipulator.ModelPos,
                lerp(_manipulator.ModelPos, in targetInModel, 0.5f) + control1Dir * (float)(dist * relControl1Len),
                targetInModel, function));
        }
        public HandlePath ModelPosCurveToTargetRelToMiddle(in Vector3 targetInModel,
            in Vector3 control1Dir, double relControl1Len,
            in Vector3 control2Dir, double relControl2Len,
            out CubicBezier curve,
            Func<double, double> function = null)
        {
            var dist = distance.Between(_manipulator.ModelPos, in targetInModel);
            return ModelPos(curve = vbp.Curve(_manipulator.ModelPos,
                lerp(_manipulator.ModelPos, in targetInModel, 0.3333f) + control1Dir * (float)(dist * relControl1Len),
                lerp(_manipulator.ModelPos, in targetInModel, 0.6666f) + control2Dir * (float)(dist * relControl2Len),
                targetInModel, function));
        }
        public HandlePath ModelRotToTargetBy(Vector3 fw, Vector3 approximateUp, Func<Quaternion> getModelRot, double maxDegreesPerFrame)
        {
            return ModelRot(vbp.DynamicRotation(x =>
                getModelRot().RotateTowards(
                    lookAt(fw, fw.GetRealUp(approximateUp)),
                    maxDegreesPerFrame)));
        }
        public HandlePath ModelRotToTarget(in Vector3 fw, in Vector3 approximateUp)
        {
            return ModelRot(vbp.Sphere(_manipulator.ModelFw, fw), vbp.Sphere(_manipulator.ModelUp, fw.GetRealUp(approximateUp)));
        }
        public HandlePath ModelRotToTarget(in Vector3 fw, in Vector3 approximateUp, Func<double, double> function)
        {
            return ModelRot(
                vbp.Sphere(_manipulator.ModelFw, fw, function), 
                vbp.Sphere(_manipulator.ModelUp, fw.GetRealUp(approximateUp), function));
        }
        public HandlePath ModelRotTo2Targets(
            IVectorByProgress fw1, IVectorByProgress up1, double progress2start,
            IVectorByProgress fw2, IVectorByProgress up2)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start, fw1),
                    vbp.InRange(progress2start, 1, up1));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start, fw2),
                    vbp.InRange(progress2start, 1, up2));
            return ModelRot(complexFw, complexUp);

        }
        public HandlePath ModelRotTo2Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2,
            Func<double, double> function = null)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelFw, fw1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1, fw2, function)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelUp, approximateUp1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function)));

            return ModelRot(complexFw, complexUp);

        }
        public HandlePath ModelRotTo2Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2,
            Func<double, double> function1, Func<double, double> function2)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelFw, fw1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1, fw2, function2)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelUp, approximateUp1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function2)));

            return ModelRot(complexFw, complexUp);

        }
        public HandlePath ModelRotTo3Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2, double progress3start,
            in Vector3 fw3, in Vector3 approximateUp3, Func<double, double> function = null)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelFw, fw1, function)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1, fw2, function)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw2, fw3, function)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelUp, approximateUp1, function)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp2), approximateUp3, function)));

            return ModelRot(complexFw, complexUp);

        }
        public HandlePath ModelRotTo3Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2, double progress3start,
            in Vector3 fw3, in Vector3 approximateUp3,
            Func<double, double> function1, Func<double, double> function2, Func<double, double> function3)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelFw, fw1, function1)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1, fw2, function2)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw2, fw3, function3)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.ModelUp, approximateUp1, function1)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function2)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp2), approximateUp3, function3)));

            return ModelRot(complexFw, complexUp);

        }
        public HandlePath LocalRotTo3Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2, double progress3start,
            in Vector3 fw3, in Vector3 approximateUp3, Func<double, double> function = null)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalFw, fw1, function)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1, fw2, function)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw2, fw3, function)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalUp, approximateUp1, function)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp2), approximateUp3, function)));

            return LocalRot(complexFw, complexUp);

        }
        public HandlePath LocalRotTo3Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2, double progress3start,
            in Vector3 fw3, in Vector3 approximateUp3,
            Func<double, double> function1, Func<double, double> function2, Func<double, double> function3)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalFw, fw1, function1)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1, fw2, function2)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw2, fw3, function3)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalUp, approximateUp1, function1)),
                    vbp.InRange(progress2start, progress3start,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function2)),
                    vbp.InRange(progress3start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp2), approximateUp3, function3)));

            return LocalRot(complexFw, complexUp);

        }
        public HandlePath LocalRotTo2Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2,
            Func<double, double> function = null)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalFw, fw1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1, fw2, function)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalUp, approximateUp1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function)));

            return LocalRot(complexFw, complexUp);

        }
        public HandlePath LocalRotTo2Targets(
            in Vector3 fw1, in Vector3 approximateUp1, double progress2start,
            in Vector3 fw2, in Vector3 approximateUp2,
            Func<double, double> function1, Func<double, double> function2)
        {
            var complexFw =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalFw, fw1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1, fw2, function2)));
            var complexUp =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Sphere(_manipulator.LocalUp, approximateUp1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Sphere(fw1.GetRealUp(approximateUp1), approximateUp2, function2)));

            return LocalRot(complexFw, complexUp);

        }

        public HandlePath LocalRotToTargetBy(Vector3 fw, Vector3 approximateUp, Func<Quaternion> getLocalRot, double maxDegreesPerFrame)
        {
            return LocalRot(vbp.DynamicRotation(x =>
                getLocalRot().RotateTowards(
                    lookAt(in fw, fw.GetRealUp(approximateUp)),
                    maxDegreesPerFrame)));
        }
        public HandlePath LocalRotToTarget(in Quaternion targetInLocal)
        {
            var fw = targetInLocal * Vector3.forward;
            return LocalRot(
                        vbp.Sphere(_manipulator.LocalFw, in fw),
                        vbp.Sphere(_manipulator.LocalUp, fw.GetRealUp(targetInLocal * Vector3.up)));
        }
        public HandlePath LocalRotToTarget(in Quaternion targetInLocal, Func<double, double> function)
        {
            var fw = targetInLocal * Vector3.forward;
            return LocalRot(
                vbp.Sphere(_manipulator.LocalFw, in fw, function),
                vbp.Sphere(_manipulator.LocalUp, fw.GetRealUp(targetInLocal * Vector3.up), function));
        }
        public HandlePath LocalRotToTarget(in Vector3 fw, in Vector3 approximateUp)
        {
            return LocalRot(vbp.Sphere(_manipulator.LocalFw, in fw), vbp.Sphere(_manipulator.LocalUp, fw.GetRealUp(approximateUp)));
        }
        public HandlePath LocalRotToTarget(in Vector3 fw, in Vector3 approximateUp, Func<double, double> function)
        {
            return LocalRot(vbp.Sphere(_manipulator.LocalFw, in fw, function), vbp.Sphere(_manipulator.LocalUp, fw.GetRealUp(approximateUp), function));
        }

        public HandlePath ModelRotToTarget(in Quaternion targetInModel, Func<double, double> function = null)
        {
            var fw = targetInModel * Vector3.forward;
            return ModelRot(vbp.Sphere(_manipulator.ModelFw, fw, function), vbp.Sphere(_manipulator.ModelUp, fw.GetRealUp(targetInModel * Vector3.up), function));
        }
        public HandlePath WorldRotToTarget(in Vector3 fw, in Vector3 approximateUp, Func<double, double> function = null)
        {
            return WorldRot(vbp.Sphere(_manipulator.Manipulator.forward, fw, function), vbp.Sphere(_manipulator.Manipulator.up, fw.GetRealUp(approximateUp), function));
        }
        public HandlePath WorldRotToTarget(Func<Vector3> fw, Func<Vector3> approximateUp, Func<double, double> function = null)
        {
            var iniFw = _manipulator.Manipulator.forward;
            var iniUp = _manipulator.Manipulator.up;
            function = function ?? (Func<double,double>)(x=>x);
            return WorldRot(
                vbp.DynamicVector(x => slerp(iniFw, fw(), function(x))), 
                vbp.DynamicVector(x => slerp(iniUp, fw().GetRealUp(approximateUp()), function(x))));
        }

        public HandlePath WorldRotToTarget(Quaternion targetInWorld, Func<double, double> function = null)
        {
            return WorldRot(vbp.Rotate(_manipulator.Manipulator.rotation, targetInWorld, function));
        }
        public HandlePath WorldPosLineToTarget(Vector3 posInWorld, Func<double, double> function = null)
        {
            return WorldPos(vbp.Line(_manipulator.Manipulator.position, posInWorld, function));
        }
        public HandlePath WorldPosLineTo2Targets(
            in Vector3 posInWorld1, double progress2start,
            in Vector3 posInWorld2, Func<double, double> function = null)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.Manipulator.position, posInWorld1, function)),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(posInWorld1, posInWorld2, function)));

            return WorldPos(complex);
        }

        public HandlePath WorldPosLineTo2Targets(
            in Vector3 posInWorld1, double progress2start,
            in Vector3 posInWorld2, Func<double, double> function1, Func<double, double> function2)
        {
            var complex =
                vbp.Complex(
                    vbp.InRange(0, progress2start,
                        vbp.Line(_manipulator.Manipulator.position, posInWorld1, function1)),
                    vbp.InRange(progress2start, 1,
                        vbp.Line(posInWorld1, posInWorld2, function2)));

            return WorldPos(complex);
        }
        public HandlePath WorldPosCurveToTargetRelToEnd(in Vector3 targetInWorld, in Vector3 control1RelToEnd, Func<double, double> function = null)
        {
            var dist = fun.distance.Between(_manipulator.Manipulator.position, targetInWorld);
            return WorldPos(vbp.Curve(_manipulator.Manipulator.position, targetInWorld + control1RelToEnd * dist, targetInWorld, function));
        }
        public HandlePath WorldPosCurveToTargetRelToMiddle(in Vector3 targetInWorld, in Vector3 control1RelToMiddle, Func<double, double> function = null)
        {
            var dist = fun.distance.Between(_manipulator.Manipulator.position, targetInWorld);
            return WorldPos(vbp.Curve(_manipulator.Manipulator.position, 
                lerp(_manipulator.Manipulator.position, in targetInWorld, 0.5f) + control1RelToMiddle * dist,
                targetInWorld, function));
        }
        public HandlePath WorldPosCurveToTargetRelToMiddleAbsControl(in Vector3 posInWorld, in Vector3 control1RelToMiddle, Func<double, double> function = null)
        {
            return WorldPos(vbp.Curve(_manipulator.Manipulator.position, 
                lerp(_manipulator.Manipulator.position, in posInWorld, 0.5f) + control1RelToMiddle, 
                posInWorld, function));
        }
        public HandlePath ModelRot(IQuaternionByProgress rotateInModelSpace)
        {
            _rotWithQuaternionSpace = HandleSpace.Model;
            _rotate = rotateInModelSpace;
            return this;
        }
        public HandlePath LocalPos(IVectorByProgress posInLocalSpace)
        {
            _posSpace = HandleSpace.Local;
            _pos = posInLocalSpace;
            return this;
        }
        public HandlePath LocalRot(IVectorByProgress forwardInLocalSpace, IVectorByProgress upInLocalSpace)
        {
            _rotWithTargetSpace = HandleSpace.Local;
            _forward = forwardInLocalSpace;
            _up = upInLocalSpace;
            return this;
        }
        public HandlePath LocalRot(IQuaternionByProgress rotateInLocalSpace)
        {
            _rotWithQuaternionSpace = HandleSpace.Local;
            _rotate = rotateInLocalSpace;
            return this;
        }
        public HandlePath AsNowInModel()
        {
            ModelPosLineToTarget(_manipulator.ModelPos);
            ModelRotToTarget(_manipulator.ModelFw, _manipulator.ModelUp);
            return this;
        }
        public void Reset()
        {
            var b = New;
        }
        public void Apply(double progress)
        {
            if (_canApply != null && !_canApply()) return;
            TryApplyPosition(progress);
            TryApplyRotation(progress);
            TryApplyScale(progress);
        }
        public void Apply(double progress, Func<double, double> func)
        {
            if (_canApply != null && !_canApply()) return;
            progress = func(progress);
            TryApplyPosition(progress);
            TryApplyRotation(progress);
            TryApplyScale(progress);
        }
        bool TryApplyRotation(double progress)
        {
            var rotChange = false;
            if (_rotWithQuaternionSpace != HandleSpace.None)
            {
                switch (_rotWithQuaternionSpace)
                {
                    case HandleSpace.World:
                        rotChange = _manipulator.TrySetWorldRot(_rotate.GetValueByProgress(progress));
                        break;
                    case HandleSpace.Model:
                        rotChange = _manipulator.TrySetModelRot(_rotate.GetValueByProgress(progress));
                        break;
                    case HandleSpace.Local:
                        rotChange = _manipulator.TrySetLocalRot(_rotate.GetValueByProgress(progress));
                        break;
                }
            }
            else if (_rotWithTargetSpace != HandleSpace.None)
            {
                switch (_rotWithTargetSpace)
                {
                    case HandleSpace.World:
                        rotChange = _manipulator.TrySetWorldRot(_forward.GetValueByProgress(progress), _up.GetValueByProgress(progress));
                        break;
                    case HandleSpace.Model:
                        rotChange = _manipulator.TrySetModelRot(_forward.GetValueByProgress(progress), _up.GetValueByProgress(progress));
                        break;
                    case HandleSpace.Local:
                        rotChange = _manipulator.TrySetLocalRot(Quaternion.LookRotation(_forward.GetValueByProgress(progress), _up.GetValueByProgress(progress)));
                        break;
                }
            }
            return rotChange;
        }
        bool TryApplyPosition(double progress)
        {
            var posChange = false;
            switch (_posSpace)
            {
                case HandleSpace.World:
                    posChange = _manipulator.TrySetWorldPos(_pos.GetValueByProgress(progress));
                    break;
                case HandleSpace.Model:
                    posChange = _manipulator.TrySetModelPos(_pos.GetValueByProgress(progress));
                    break;
                case HandleSpace.Local:
                    posChange = _manipulator.TrySetLocalPos(_pos.GetValueByProgress(progress));
                    break;
            }
            return posChange;
        }
        bool TryApplyScale(double progress)
        {
            var scaChange = false;
            switch (_scaSpace)
            {
                case HandleSpace.None:
                    break;
                case HandleSpace.Local:
                    scaChange = _manipulator.TrySetLocalScale(_scale.GetValueByProgress(progress));
                    break;
                default:
                    throw new Exception("HandlePath.Scale allows only local space");
            }
            return scaChange;
        }
    }
}*/