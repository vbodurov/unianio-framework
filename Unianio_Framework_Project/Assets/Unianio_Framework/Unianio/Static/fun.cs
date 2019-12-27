using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unianio.Animations;
using Unianio.Enums;
using Unianio.Extensions;
using Unianio.Services;
using UnityEngine;
using Unianio.Rigged;
using Unianio.Genesis;
using Unianio.Genesis.IK;
using Unianio.Animations.Common;
using Unianio.IK;
using Unianio.Moves;
using Unianio.RSG;

namespace Unianio.Static
{
    public static partial class fun
    {
        public const float RTD = (float)(180 / Math.PI); // radians to degrees  
        public const float DTR = (float)(Math.PI / 180); // degrees to radians

        const int NumberFrames = 60;
        public static float framesPerSec = 90f;
        public static float smoothDeltaTime = 1/90f;
        static int _smoothDeltaTimeFrame = 0;
        public static void frame()
        {
            if (_smoothDeltaTimeFrame < NumberFrames) ++_smoothDeltaTimeFrame;
            smoothDeltaTime = statistics.Average(smoothDeltaTime, Time.deltaTime / Time.timeScale, _smoothDeltaTimeFrame);
            framesPerSec = (1/smoothDeltaTime);
        }
//        public static float framesIf90Fps(double f)
//        {
//            return (framesPerSec*(float)f)/90f;
//        }
//        public static int numFramesIf90Fps(double f)
//        {
//            return (int)Math.Round((framesPerSec*f)/90.0);
//        }
//        public static void setTimeScale(double scale)
//        {
//            Time.timeScale = (float)scale;
//            Application.targetFrameRate = max((int)(90 * Time.timeScale), 30);
//            QualitySettings.vSyncCount = 0;
//        }
        public static List<IPromise<IAnimation>> aniPromises() => new List<IPromise<IAnimation>>();
        public static int abs(int n) => n < 0 ? n*-1 : n;
        public static float abs(float n) => n < 0 ? n*-1 : n; 
        public static float abs(double n) { return n < 0 ? (float)n*-1 : (float)n; }
        public static int sign(double n) => n < 0 ? -1 : 1; 
        public static int min(int a, int b) => a > b ? b : a;
        public static int max(int a, int b) => a < b ? b : a;
        public static float min(double a, double b) => a > b ? (float)b : (float)a; 
        public static float max(double a, double b) => a < b ? (float)b : (float)a; 
        public static float sqrt(double n) => (float)Math.Sqrt(n); 
        public static float sin(double n) => (float)Math.Sin(n);
        public static float cos(double n) => (float)Math.Cos(n); 
        public static float atan2(double a, double b) => (float)Math.Atan2(a, b); 
        public static float pow(double n, double p) => (float)Math.Pow(n, p); 
        public static float tan(double n) => (float)Math.Tan(n);
        public static float zigzag(double x) => abs((x + 1) % 2 - 1);
        public static Vector3 mid(in Vector3 a, in Vector3 b) => lerp(in a, in b, 0.5);
        public static Vector3 mid(in Vector3 a, in Vector3 b, in Vector3 c)
        {
            var ab = mid(in a, in b);
            return statistics.LerpAverage(in ab, in c, 3);
        }

        public static Vector2 lerp2d(in Vector2 a, in Vector2 b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return new Vector2(a.x + (b.x - a.x) * (float)t, a.y + (b.y - a.y) * (float)t);
        }
        public static Vector3 lerp(in Vector3 a, in Vector3 b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return new Vector3(a.x + (b.x - a.x) * (float)t, a.y + (b.y - a.y) * (float)t, a.z + (b.z - a.z) * (float)t);
        }
        public static Vector3 lerpXYZ(in Vector3 a, in Vector3 b, double tX, double tY, double tZ)
        {
            return new Vector3(a.x + (b.x - a.x) * (float)tX, a.y + (b.y - a.y) * (float)tY, a.z + (b.z - a.z) * (float)tZ);
        }
        public static Vector3 lerp(in Vector3 a, in Vector3 b, double tXZ, double tY)
        {
            return new Vector3(a.x + (b.x - a.x) * (float)tXZ, a.y + (b.y - a.y) * (float)tY, a.z + (b.z - a.z) * (float)tXZ);
        }
        public static Vector3 lerp(in Vector3 a, in Vector3 b, double tX, double tY, double tZ)
        {
            return new Vector3(a.x + (b.x - a.x) * (float)tX, a.y + (b.y - a.y) * (float)tY, a.z + (b.z - a.z) * (float)tZ);
        }

        public static FuncAni blinkAni(IComplexHuman human, double seconds, Func<double, double> func = null)
        {
            if (func == null) func = x => sin(x * PI);
            return get<FuncAni>().Set(seconds,
                        ani =>
                        {
                            human.AniBlink?.ClearAllFollowUpActions().StopIfRunning();
                            human.AniBlink = ani;
                        },
                        x =>
                        {
                            human.Blinks.Blink01 = func(x);
                        })
                    .AsUniqueNamed(unique.Blink + human.ID)
                ;
        }
        
        public static Quaternion lerp(in Quaternion a, in Quaternion b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return Quaternion.LerpUnclamped(a, b, (float)t);
        }
        public static Vector3 bezierV3(in Vector3 start, in Vector3 control, in Vector3 end, double t)
        {
            return BezierFunc.GetPointQuadratic(t, in start, in control, in end);
        }
        public static Vector3 bezierV3(in Vector3 start, in Vector3 control1, in Vector3 control2, in Vector3 end, double t)
            => BezierFunc.GetPointCubic(t, in start, in control1, in control2, in end);

        public static Vector3 bezierV3RelVec(in Vector3 start, in Vector3 controlVecFromCenter, in Vector3 end, double t)
        {
            var center = lerp(in start, in end, 0.5);
            return BezierFunc.GetPointQuadratic(t, in start, center + controlVecFromCenter, in end);
        }
        public static Vector3 bezierV3RelVec(in Vector3 start, in Vector3 controlVecFromStart, in Vector3 controlVecFromEnd, in Vector3 end, double t)
        {
            return BezierFunc.GetPointCubic(t, in start, start + controlVecFromStart, end + controlVecFromEnd, in end);
        }
        public static Vector3 bezierV3RelDir(in Vector3 start, in Vector3 controlDir, double controlRelOfDist, in Vector3 end, double t)
        {
            var distance = fun.distance.Between(in start, in end);
            var center = lerp(in start, in end, 0.5);
            return BezierFunc.GetPointQuadratic(t, in start, center + controlDir * (float)(controlRelOfDist * distance), in end);
        }
        public static Vector3 bezierV3RelDir(in Vector3 start, in Vector3 control1Dir, double control1RelOfDist, in Vector3 control2Dir, double control2RelOfDist, in Vector3 end, double t)
        {
            var distance = fun.distance.Between(in start, in end);
            var thirdOfDist = distance / 3f;
            var center1 = start.MoveTowards(in end, thirdOfDist);
            var center2 = start.MoveTowards(in end, thirdOfDist * 2f);
            return BezierFunc.GetPointCubic(t, in start, 
                center1 + control1Dir * (float)(control1RelOfDist * distance), 
                center2 + control2Dir * (float)(control2RelOfDist * distance),
                in end);
        }
        public static float lerp(double a, double b, double t)
            => (float)(a + (b - a) * t);


        public static float smoothstep(double x) => smoothstep(0, 1, x);

        public static float smootherstep(double x) => smootherstep(0, 1, x);

        public static float smoothstep(double edge0, double edge1, double x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
            // Evaluate polynomial
            return (float)(x * x * (3 - 2 * x));
        }

        public static float smootherstep(double edge0, double edge1, double x)
        {
            // Scale, and clamp x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
            // Evaluate polynomial
            return (float)(x * x * x * (x * (x * 6 - 15) + 10));
        }

        public static bool hasNotRunOnce(bool precondition, ref bool hasRun)
        {
            if (!precondition) return false;
            if (hasRun) return false;
            hasRun = true;
            return true;
        }
        public static string GetUniquenessId<TAni>() where TAni : IAnimation
            => "unq_" +OptimizeNameForEncription<TAni>();
        
        public static string GetUniquenessIdFor<TAni>(object id) where TAni : IAnimation
            => "unqid_" +OptimizeNameForEncription<TAni>()+"_"+id;
        
        private static string OptimizeNameForEncription<TAni>() where TAni : IAnimation
        {
            var name = typeof (TAni).FullName;
            return name.Length > 10 ? name : ToAscii(name);//encrypted names are always less than 10
        }
        private static string ToAscii(string s)
            => s.Select(c => ((int) c).ToString()).JoinAsString("_");
        
        public static float uniFun(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1)
        {
            double y;
            if (x < f1StartX) y = f0(x);
            else y = f1(x);
            return (float)y;
        }
        public static Transform[] getGameObjectSequence(string name)
        {
            var list = new List<Transform>();
            var go = GameObject.Find(name);
            if(go != null) list.Add(go.transform);
            var i = 0;
            var numNotFound = 0;
            while (true)
            {
                go = GameObject.Find(name + " (" + i + ")");
                if(go != null)
                {
                    numNotFound = 0;
                    list.Add(go.transform);
                }
                else
                {
                    ++numNotFound;
                }
                if(numNotFound > 3) break;
                ++i;
            }
            return list.ToArray();
        }

        public static float split01in2(double x01, double boundary, ref bool isBoundaryReached, Action whenBoundaryReached = null)
        {
            if (!isBoundaryReached)
            {
                if (x01 > boundary)
                {
                    isBoundaryReached = true;
                    whenBoundaryReached?.Invoke();
                    x01 = 0;
                }
                else x01 = x01.FromRangeTo01Clamped(0f, boundary);
            }
            else
            {
                x01 = x01.FromRangeTo01Clamped(boundary, 1f);
            }
            return (float) x01;
        }
        public static float uniFun(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1,
            double f2StartX, Func<double, double> f2)
        {
            double y;
            if (x < f1StartX) y = f0(x);
            else if (x < f2StartX) y = f1(x);
            else y = f2(x);
            return (float)y;
        }
        public static float uniFun(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1,
            double f2StartX, Func<double, double> f2,
            double f3StartX, Func<double, double> f3)
        {
            double y;
            if (x < f1StartX) y = f0(x);
            else if (x < f2StartX) y = f1(x);
            else if (x < f3StartX) y = f2(x);
            else y = f3(x);
            return (float)y;
        }
        public static float uniFun(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1,
            double f2StartX, Func<double, double> f2,
            double f3StartX, Func<double, double> f3,
            double f4StartX, Func<double, double> f4)
        {
            double y;
            if (x < f1StartX) y = f0(x);
            else if (x < f2StartX) y = f1(x);
            else if (x < f3StartX) y = f2(x);
            else if (x < f4StartX) y = f3(x);
            else y = f4(x);
            return (float)y;
        }
        public static float uniFun(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1,
            double f2StartX, Func<double, double> f2,
            double f3StartX, Func<double, double> f3,
            double f4StartX, Func<double, double> f4,
            double f5StartX, Func<double, double> f5)
        {
            double y;
            if (x < f1StartX) y = f0(x);
            else if (x < f2StartX) y = f1(x);
            else if (x < f3StartX) y = f2(x);
            else if (x < f4StartX) y = f3(x);
            else if (x < f5StartX) y = f4(x);
            else y = f5(x);
            return (float)y;
        }
        public static float uniFun01(double x, Func<double, double> f0,
            double f1StartX, Func<double, double> f1)
        {
            double y;
            if (x < f1StartX) y = f0(x.FromRangeTo01(0, f1StartX));
            else y = f1(x.FromRangeTo01(f1StartX, 1));
            return (float)y;
        }
        public static float uniFun01(double x, Func<double, double> f0,
           double f1StartX, Func<double, double> f1,
           double f2StartX, Func<double, double> f2)
        {
            double y;
            if (x < f1StartX) y = f0(x.FromRangeTo01(0, f1StartX));
            else if (x < f2StartX) y = f1(x.FromRangeTo01(f1StartX, f2StartX));
            else y = f2(x.FromRangeTo01(f2StartX, 1));
            return (float)y;
        }
        public static float uniFun01(double x, Func<double, double> f0,
           double f1StartX, Func<double, double> f1,
           double f2StartX, Func<double, double> f2,
           double f3StartX, Func<double, double> f3)
        {
            double y;
            if (x < f1StartX) y = f0(x.FromRangeTo01(0, f1StartX));
            else if (x < f2StartX) y = f1(x.FromRangeTo01(f1StartX, f2StartX));
            else if (x < f3StartX) y = f2(x.FromRangeTo01(f2StartX, f3StartX));
            else y = f3(x.FromRangeTo01(f3StartX, 1));
            return (float)y;
        }
        public static float uniFun01(double x, Func<double, double> f0,
           double f1StartX, Func<double, double> f1,
           double f2StartX, Func<double, double> f2,
           double f3StartX, Func<double, double> f3,
           double f4StartX, Func<double, double> f4)
        {
            double y;
            if (x < f1StartX) y = f0(x.FromRangeTo01(0, f1StartX));
            else if (x < f2StartX) y = f1(x.FromRangeTo01(f1StartX, f2StartX));
            else if (x < f3StartX) y = f2(x.FromRangeTo01(f2StartX, f3StartX));
            else if (x < f4StartX) y = f3(x.FromRangeTo01(f3StartX, f4StartX));
            else y = f4(x.FromRangeTo01(f4StartX, 1));
            return (float)y;
        }
        public static float uniFun01(double x, Func<double, double> f0,
           double f1StartX, Func<double, double> f1,
           double f2StartX, Func<double, double> f2,
           double f3StartX, Func<double, double> f3,
           double f4StartX, Func<double, double> f4,
           double f5StartX, Func<double, double> f5)
        {
            double y;
            if (x < f1StartX) y = f0(x.FromRangeTo01(0, f1StartX));
            else if (x < f2StartX) y = f1(x.FromRangeTo01(f1StartX, f2StartX));
            else if (x < f3StartX) y = f2(x.FromRangeTo01(f2StartX, f3StartX));
            else if (x < f4StartX) y = f3(x.FromRangeTo01(f3StartX, f4StartX));
            else if (x < f5StartX) y = f4(x.FromRangeTo01(f4StartX, f5StartX));
            else y = f5(x.FromRangeTo01(f5StartX, 1));
            return (float)y;
        }

        /// <summary>
        /// key0 = 0
        /// key1 is parameter
        /// key2 = 1
        /// </summary>
        /*
            const double limit = 0.2;
            return uniFun01(x,  
                       n => smoothstep(0, 1, n)*limit , 
                       0.2,  
                       n => pow(sin(n*PI), 2)*(1-limit )+limit );
         */
        public static double threeKeyCycle(double x, double key1 = 0.3)
        {
            if(key1 < 0 || key1 > 1) throw new ArgumentException("key1 must be between 0 and 1");
            return uniFun01(x,
                n => smoothstep(0, 1, n) * key1,
                0.2,
                n => pow(sin(n * PI), 2) * (1 - key1) + key1);
        }

        /// <summary>
        /// key0 = 0
        /// key1 is parameter
        /// key2 = 1
        /// </summary>
        /*
            const double limit = 0.3;
            return uniFun(x,  
                       n => pow(sin((n*PI)), 2) , 
                       0.5,  
                        n => pow(sin(n*PI), 2)*(1-limit )+limit);
        */
        public static double threeKeyCycle01(double x, double key1 = 0.3)
        {
            if (key1 < 0 || key1 > 1) throw new ArgumentException("key1 must be between 0 and 1");
            return uniFun(x,
                n => pow(sin(n * PI), 2),
                0.5,
                n => pow(sin(n * PI), 2) * (1 - key1) + key1);
        }


        static IMessenger _messenger;
        public const string HolderSuffix = "_Holder";
        public const string HandleSuffix = "_Handle";
        public const float PI = (float)Math.PI;
        public const float TwoPI = (float)Math.PI * 2;
        public const float HalfPI = (float)(Math.PI * 0.5);
        public const float Ratio_1_3 = (float)(1f / 3f);
        public const float Ratio_2_3 = (float)(2f / 3f);
        public const float DegInRadians90 = (float)(Math.PI * 0.5);
        public const float DegInRadians180 = (float)(Math.PI);
        public static readonly float[] DotProductByDegree = { 1f, 0.9998477f, 0.9993908f, 0.9986295f, 0.9975641f, 0.9961947f, 0.9945219f, 0.9925461f, 0.9902681f, 0.9876884f, 0.9848077f, 0.9816272f, 0.9781476f, 0.9743701f, 0.9702957f, 0.9659258f, 0.9612617f, 0.9563048f, 0.9510565f, 0.9455186f, 0.9396926f, 0.9335804f, 0.9271839f, 0.9205049f, 0.9135454f, 0.9063078f, 0.8987941f, 0.8910065f, 0.8829476f, 0.8746197f, 0.8660254f, 0.8571673f, 0.8480481f, 0.8386706f, 0.8290376f, 0.8191521f, 0.809017f, 0.7986355f, 0.7880108f, 0.7771459f, 0.7660445f, 0.7547096f, 0.7431449f, 0.7313538f, 0.7193398f, 0.7071067f, 0.6946584f, 0.6819984f, 0.6691306f, 0.656059f, 0.6427876f, 0.6293204f, 0.6156615f, 0.601815f, 0.5877852f, 0.5735765f, 0.5591929f, 0.5446391f, 0.5299193f, 0.5150381f, 0.5f, 0.4848096f, 0.4694716f, 0.4539905f, 0.4383711f, 0.4226182f, 0.4067366f, 0.3907312f, 0.3746066f, 0.3583679f, 0.3420201f, 0.3255681f, 0.309017f, 0.2923717f, 0.2756374f, 0.258819f, 0.2419218f, 0.224951f, 0.2079117f, 0.1908091f, 0.1736482f, 0.1564345f, 0.1391731f, 0.1218693f, 0.1045284f, 0.08715588f, 0.06975645f, 0.05233604f, 0.03489941f, 0.01745242f, 0f, -0.01745248f, -0.03489947f, -0.05233586f, -0.06975651f, -0.0871557f, -0.1045285f, -0.1218693f, -0.139173f, -0.1564344f, -0.1736481f, -0.1908089f, -0.2079116f, -0.224951f, -0.2419218f, -0.258819f, -0.2756375f, -0.2923716f, -0.3090171f, -0.3255682f, -0.3420202f, -0.3583679f, -0.3746065f, -0.3907312f, -0.4067366f, -0.4226184f, -0.4383712f, -0.4539905f, -0.4694716f, -0.4848095f, -0.5000001f, -0.515038f, -0.5299193f, -0.5446391f, -0.5591928f, -0.5735766f, -0.5877852f, -0.601815f, -0.6156615f, -0.6293204f, -0.6427877f, -0.656059f, -0.6691307f, -0.6819984f, -0.6946582f, -0.7071067f, -0.7193398f, -0.7313538f, -0.7431448f, -0.7547096f, -0.7660444f, -0.777146f, -0.7880107f, -0.7986356f, -0.8090171f, -0.819152f, -0.8290374f, -0.8386706f, -0.8480481f, -0.8571672f, -0.8660253f, -0.8746197f, -0.8829476f, -0.8910065f, -0.8987941f, -0.9063078f, -0.9135456f, -0.9205048f, -0.9271837f, -0.9335804f, -0.9396925f, -0.9455187f, -0.9510566f, -0.9563048f, -0.9612616f, -0.9659257f, -0.9702957f, -0.9743701f, -0.9781476f, -0.9816272f, -0.9848078f, -0.9876882f, -0.9902682f, -0.9925461f, -0.9945219f, -0.9961947f, -0.9975641f, -0.9986296f, -0.9993908f, -0.9998477f, -1f };
        public static readonly Color HandleColor = new Color(0.75f, 0.0f, 0.75f, 1f);

//        public static T mdl<T>(IGenHuman h, T mark, T emily)
//            => h.Persona == HumanKind.Mark ? mark : emily;

//        private static IPlayerSettings _settings;
//        public static bool isMusicOn
//        {
//            get
//            {
//                if (_settings == null) _settings = get<IPlayerSettings>();
//                return _settings.IsMusicOn;
//            }
//        }
        public static bool angleOfDotProductIsLessThan(double dotProduct, double degreesAngle)
        {
            var degrees = (int)Math.Round(degreesAngle);
            if (degrees < 0) degrees *= -1;
            if (degrees > 360) degrees = degrees % 360;
            if (degrees > 180) degrees = 180 - (degrees % 180);
            return dotProduct > DotProductByDegree[degrees];
        }
        public static bool angleOfDotProductIsMoreThan(double dotProduct, double degreesAngle)
        {
            var degrees = (int)Math.Round(degreesAngle);
            if (degrees < 0) degrees *= -1;
            if (degrees > 360) degrees = degrees % 360;
            if (degrees > 180) degrees = 180 - (degrees % 180);
            return dotProduct < DotProductByDegree[degrees];
        }
        public static bool angleIsLessThan(in Vector3 dir1, in Vector3 dir2, double degreesAngle)
        {
            var degrees = (int)Math.Round(degreesAngle);
            if (degrees < 0) degrees *= -1;
            if (degrees > 360) degrees = degrees % 360;
            if (degrees > 180) degrees = 180 - (degrees % 180);
            return dot(in dir1, in dir2) > DotProductByDegree[degrees];
        }
        public static bool angleIsMoreThan(in Vector3 dir1, in Vector3 dir2, double degreesAngle)
        {
            var degrees = (int)Math.Round(degreesAngle);
            if (degrees < 0) degrees *= -1;
            if (degrees > 360) degrees = degrees % 360;
            if (degrees > 180) degrees = 180 - (degrees % 180);
            return dot(in dir1, in dir2) < DotProductByDegree[degrees];
        }
        public static T onesIn<T>(int numberTimes, T state) where T : class
        {
            if (randomBool(1.0 / max(numberTimes, 1))) return state;
            return null;
        }
        public static readonly Color colorRed = new Color(1f, 0.0f, 0.0f, 1f);
        public static readonly Color colorLightRed = new Color(1f, 0.5f, 0.5f, 1f);
        public static readonly Color colorOrange = new Color(1f, 0.5f, 0.0f, 1f);
        public static readonly Color colorYellow = new Color(1f, 0.9215686f, 0.01568628f, 1f);
        public static readonly Color colorLightYellow = new Color(1f, 0.9f, 0.5f, 1f);
        public static readonly Color colorGreen = new Color(0.0f, 1f, 0.0f, 1f);
        public static readonly Color colorDarkGreen = new Color(0.0f, 0.5f, 0.0f, 1f);
        public static readonly Color colorCyan = new Color(0.0f, 1f, 1f, 1f);
        public static readonly Color colorBlue = new Color(0.0f, 0.0f, 1f, 1f);
        public static readonly Color colorLightGreen = new Color(0.2f, 0.8f, 0.2f, 1f);
        public static readonly Color colorMagenta = new Color(1f, 0.0f, 1f, 1f);
        public static readonly Color colorWhite = new Color(1f, 1f, 1f, 1f);
        public static readonly Color colorBlack = new Color(0.0f, 0.0f, 0.0f, 1f);
        public static readonly Color colorGrey = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color colorBrown = new Color(0.6f, 0.3f, 0.0f, 1f);
        public static readonly Color colorPink = new Color(1.00f, 0.75f, 0.80f, 1f);

        public static T get<T>() => GlobalFactory.Default.Get<T>();
        public static object get(Type t) => GlobalFactory.Default.Get(t);

        public static T get<T>(object name) => GlobalFactory.Default.Get<T>(Convert.ToString(name));
        public static object get(object name, Type t) => GlobalFactory.Default.Get(Convert.ToString(name), t);

        public static void register<T>(T instance) => GlobalFactory.Default.SetInstance<T>(instance);
        public static void register<T>(object name, T instance) => GlobalFactory.Default.SetInstance<T>(Convert.ToString(name), instance);
        public static void unregisterInstanceOfType<T>(object name) =>
            GlobalFactory.Default.RemoveInstanceOfType(Convert.ToString(name), typeof(T));

        public static Color rgba(double r, double g, double b, double a) { return new Color((float)r, (float)g, (float)b, (float)a); }
        public static Vector3 V3(double n) { return new Vector3((float)n, (float)n, (float)n); }
        public static Vector3 V3(double x, double y, double z) { return new Vector3((float)x, (float)y, (float)z); }
        public static Vector2 V2(double n) { return new Vector2((float)n, (float)n); }
        public static Vector2 V2(double x, double y) { return new Vector2((float)x, (float)y); }
        public static float max(double a, double b, double c) { return max(max(a, b), c); }
        public static float max(double a, double b, double c, double d) { return max(max(max(a, b), c), d); }
        public static float max(double a, double b, double c, double d, double e) { return max(max(max(max(a, b), c), d), e); }
        public static float max(double a, double b, double c, double d, double e, double f) { return max(max(max(max(max(a, b), c), d), e), f); }
        public static float min(double a, double b, double c) { return min(min(a, b), c); }
        public static float min(double a, double b, double c, double d) { return min(min(min(a, b), c), d); }
        public static float min(double a, double b, double c, double d, double e) { return min(min(min(min(a, b), c), d), e); }
        public static float min(double a, double b, double c, double d, double e, double f) { return min(min(min(min(min(a, b), c), d), e), f); }
        public static float maxOfTheAbs(double a, double b)
        {
            return (float)(abs(a) > abs(b) ? a : b);
        }

        public static float degByOppAdj(double opposite, double adjucent) { return (float)Math.Atan2(opposite, adjucent) * RTD; }
        public static float noise(double x) { return NoiseGenerator.Generate(x); }
        public static float exp(double n) { return (float)(Math.Exp(n)); }
        public static float avgOf(double a, double b) { return lerp(a, b, 0.5); }
        public static float avgOf(double a, double b, double c)
            => avg(avgOf(a,b), c, 3);
        public static float avgOf(double a, double b, double c, double d)
            => avg(avg(avgOf(a, b), c, 3), d, 4);
        public static float avg(double lastAverage, double current, int count)
        {
            return (count <= 1.0f) ? (float)current : ((float)lastAverage * (count - 1.0f) + (float)current) / count;
        }
        public static float avg(double lastAverage, double current, int count, int countLimit)
        {
            if (count > countLimit) count = countLimit;
            return avg(lastAverage, current, count);
        }
        public static float valueOverTime01(double iniTime, double duration, Func<double, double> func)
        {
            duration = duration.Abs();
            if (duration < 0.0001) duration = 0.0001;
            var x = ((Time.time - iniTime) / duration).Clamp01();
            return (float)func(x);
        }
        public static float valueOverTimeEndless(double iniTime, double duration, Func<double, double> func)
        {
            duration = duration.Abs();
            if (duration < 0.0001) duration = 0.0001;
            var x = ((Time.time - iniTime) / duration);
            return (float)func(x);
        }
        public static void ensudeHandRotation(IHumArmChain chain, in Vector3 localUp)
        {
            chain.CalculateArmBend(out var midPos, out var length);
            var handlePos = chain.Control.position;
            var dirShoulderToHand = chain.Shoulder.DirTo(in handlePos);
            vector.ProjectOnPlane(localUp.AsWorldDir(chain.ArmRoot), in dirShoulderToHand, out var projUp);
            var elbowPos = midPos + projUp * length;
            chain.Control.rotation = lookAt(elbowPos.DirTo(in handlePos), in projUp);
        }
        public static void ensureHandsRotation(
            IHumArmChain a, in Vector3 aLocalUp, 
            IHumArmChain b, in Vector3 bLocalUp)
        {
            ensudeHandRotation(a, in aLocalUp);
            ensudeHandRotation(b, in bLocalUp);
        }
        public static void update(IUpdatable a) { a.Update(); }
        public static void update(IUpdatable a, IUpdatable b) { a.Update(); b.Update(); }
        public static void update(IUpdatable a, IUpdatable b, IUpdatable c) { a.Update(); b.Update(); c.Update(); }
        public static void update(IUpdatable a, IUpdatable b, IUpdatable c, IUpdatable d) { a.Update(); b.Update(); c.Update(); d.Update(); }
        public static void update(IUpdatable a, IUpdatable b, IUpdatable c, IUpdatable d, IUpdatable e) { a.Update(); b.Update(); c.Update(); d.Update(); e.Update(); }
        public static void update(IUpdatable a, IUpdatable b, IUpdatable c, IUpdatable d, IUpdatable e, IUpdatable f) { a.Update(); b.Update(); c.Update(); d.Update(); e.Update(); f.Update(); }
        public static void apply(double x, IExecutorOfProgress path1)
        {
            path1.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2)
        {
            path1.Apply(x);
            path2.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5, IExecutorOfProgress path6)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
            path6.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5, IExecutorOfProgress path6, IExecutorOfProgress path7)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
            path6.Apply(x);
            path7.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5, IExecutorOfProgress path6, IExecutorOfProgress path7, IExecutorOfProgress path8)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
            path6.Apply(x);
            path7.Apply(x);
            path8.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5, IExecutorOfProgress path6, IExecutorOfProgress path7, IExecutorOfProgress path8, IExecutorOfProgress path9)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
            path6.Apply(x);
            path7.Apply(x);
            path8.Apply(x);
            path9.Apply(x);
        }
        public static void apply(double x, IExecutorOfProgress path1, IExecutorOfProgress path2, IExecutorOfProgress path3, IExecutorOfProgress path4, IExecutorOfProgress path5, IExecutorOfProgress path6, IExecutorOfProgress path7, IExecutorOfProgress path8, IExecutorOfProgress path9, IExecutorOfProgress path10)
        {
            path1.Apply(x);
            path2.Apply(x);
            path3.Apply(x);
            path4.Apply(x);
            path5.Apply(x);
            path6.Apply(x);
            path7.Apply(x);
            path8.Apply(x);
            path9.Apply(x);
            path10.Apply(x);
        }

        public static void applyAll(double x, IEnumerable<IExecutorOfProgress> paths)
        {
            foreach (var p in paths)
            {
                p.Apply(x);
            }
        }
        /*public static void apply(double x, params IExecutorOfProgress[] paths)
        {
            for (var i = 0; i < paths.Length; ++i)
            {
                paths[i].Apply(x);
            }
        }
        public static void apply(double x, IList<IExecutorOfProgress> paths)
        {
            for (var i = 0; i < paths.Count; ++i)
            {
                paths[i].Apply(x);
            }
        }
        public static void apply(double x, params HandlePath[][] paths)
        {
            for (var i = 0; i < paths.Length; ++i)
            {
                for(var j = 0; j < paths[i].Length; ++j)
                {
                    paths[i][j].Apply(x);
                }
            }
        }*/
        public static double increaseChanceForExtremes1(double x) { return bezier(x, 0.25, 0.00, 0.75, 1.00); }
        public static double increaseChanceForExtremes2(double x) { return bezier(x, 0.50, 0.00, 0.50, 1.00); }
        public static double increaseChanceForExtremes3(double x) { return bezier(x, 0.75, 0.00, 0.25, 1.00); }
        public static double increaseChanceForExtremes4(double x) { return bezier(x, 0.90, 0.00, 0.10, 1.00); }
        public static double funFastSlowFast1(double x) { return bezier(x, 0.00, 0.50, 1.00, 0.50); }
        public static double funFastSlowFast2(double x) { return bezier(x, 0.00, 0.80, 1.00, 0.20); }
        public static double funFastSlowFast3(double x) { return bezier(x, 0.00, 1.00, 1.00, 0.00); }
        public static double funSlowFastSlow1(double x) { return bezier(x, 0.50, 0.00, 0.50, 1.00); }
        public static double funSlowFastSlow2(double x) { return bezier(x, 0.80, 0.00, 0.20, 1.00); }
        public static double funSlowFastSlow3(double x) { return bezier(x, 1.00, 0.00, 0.00, 1.00); }
        public static double funUnchanged(double x) { return x; }
        public static double funSlowToFast1(double x) { return pow(x, 2); }
        public static double funSlowToFast2(double x) { return pow(x, 3); }
        public static double funSlowToFast3(double x) { return pow(x, 4); }
        public static double funFastToSlow1(double x) { return (1.0 - Math.Pow(1.0 - x, 2)); }//1 - pow(1 - x, 2)
        public static double funFastToSlow2(double x) { return (1.0 - Math.Pow(1.0 - x, 3)); }
        public static double funFastToSlow3(double x) { return (1.0 - Math.Pow(1.0 - x, 4)); }
        public static double funFastToSlow(double x, double power) => (1.0 - Math.Pow(1.0 - x, power));
        public static double funFlatTopSine1M1(double x, double flatness) // flatness = 1 ~ 1000
        {
            return sqrt((1.0 + flatness * flatness) / (1.0 + flatness * flatness * cos(x * PI) * cos(x * PI))) * cos(x * PI);
        }
        public static double funFlatTopSineM11(double x, double flatness) // flatness = 1 ~ 1000
        {
            return sqrt((1.0 + flatness * flatness) / (1.0 + flatness * flatness * cos(x * PI - PI) * cos(x * PI - PI))) * cos(x * PI - PI);
        }
        public static double funFlatTopSine01(double x, double flatness) // flatness = 1 ~ 1000
        {
            return 0.5 - sqrt((1.0 + flatness * flatness) / (1.0 + flatness * flatness * cos(x * PI) * cos(x * PI))) * cos(x * PI) * 0.5;
        }
        public static double funFlatTopSine10(double x, double flatness) // flatness = 1 ~ 1000
        {
            return 0.5 + sqrt((1.0 + flatness * flatness) / (1.0 + flatness * flatness * cos(x * PI) * cos(x * PI))) * cos(x * PI) * 0.5;
        }

        public static double funcToRange(double x01, double minNum, double maxNum, Func<double, double> func01)
        {
            var x = func01(x01);
            return x.From01ToRange(minNum, maxNum);
        }
        public static TValue ifElseAssign<TValue>(bool ifTrue, TValue value, ref TValue thenAssign, ref TValue otherwiseAssign)
        {
            if (ifTrue) thenAssign = value;
            else otherwiseAssign = value;
            return value;
        }
        public static void ifElseDo(bool ifTrue, Action thenDo, Action otherwiseDo)
        {
            if (ifTrue) thenDo();
            else otherwiseDo();
        }
        public static void ifElseDo(bool ifTrue, Action thenDo, bool ifElse, Action elseDo, Action otherwiseDo)
        {
            if (ifTrue) thenDo();
            else if (ifElse) elseDo();
            else otherwiseDo();
        }
        public static TOutput ifElse<TOutput>(bool ifTrue, TOutput then, TOutput otherwise)
        {
            return ifTrue ? then : otherwise;
        }
        public static TOutput ifElse<TInput, TOutput>(TInput current,
            TInput ifIs, TOutput then, TOutput otherwise) where TInput : struct
        {
            return current.Equals(ifIs) ? then : otherwise;
        }
        public static TOutput ifElse<TInput, TOutput>(TInput current,
            TInput ifIs1, TOutput then1,
            TInput ifIs2, TOutput then2, TOutput otherwise) where TInput : struct
        {
            return current.Equals(ifIs1) ? then1 : current.Equals(ifIs2) ? then2 : otherwise;
        }
        public static TOutput ifElse<TInput, TOutput>(TInput current,
            TInput ifIs1, TOutput then1,
            TInput ifIs2, TOutput then2,
            TInput ifIs3, TOutput then3, TOutput otherwise) where TInput : struct
        {
            return current.Equals(ifIs1) ? then1 : current.Equals(ifIs2) ? then2 : current.Equals(ifIs3) ? then3 : otherwise;
        }
        public static TOutput ifElse<TInput, TOutput>(TInput current,
            TInput ifIs1, TOutput then1,
            TInput ifIs2, TOutput then2,
            TInput ifIs3, TOutput then3,
            TInput ifIs4, TOutput then4, TOutput otherwise) where TInput : struct
        {
            return current.Equals(ifIs1) ? then1 : current.Equals(ifIs2) ? then2 : current.Equals(ifIs3) ? then3 : current.Equals(ifIs4) ? then4 : otherwise;
        }
        public static TOutput ifElse<TInput, TOutput>(TInput current,
            TInput ifIs1, TOutput then1,
            TInput ifIs2, TOutput then2,
            TInput ifIs3, TOutput then3,
            TInput ifIs4, TOutput then4,
            TInput ifIs5, TOutput then5, TOutput otherwise) where TInput : struct
        {
            return current.Equals(ifIs1) ? then1 : current.Equals(ifIs2) ? then2 : current.Equals(ifIs3) ? then3 : current.Equals(ifIs4) ? then4 : current.Equals(ifIs5) ? then5 : otherwise;
        }
        /// <summary>
        /// Note: the dot product must be of unit vectors
        /// </summary>
        /// <returns>degrees of that dot product</returns>
        public static float dpToDeg(double dotProduct)
        {
            return (float)(Math.Acos(dotProduct.Clamp01()) * RTD);
        }
        /// <summary>
        /// Note: the dot product must be of unit vectors
        /// </summary>
        /// <returns>radians of that dot product</returns>
        public static float dpToRad(double dotProduct)
        {
            return (float)Math.Acos(dotProduct.Clamp01());
        }

        public static Color lerp(in Color a, in Color b, double t)
        {
            return new Color(a.r + (b.r - a.r) * (float)t, a.g + (b.g - a.g) * (float)t, a.b + (b.b - a.b) * (float)t, a.a + (b.a - a.a) * (float)t);
        }
        public static void lerp(in Vector3 a, in Vector3 b, double t, out Vector3 r)
        {
            r = new Vector3(a.x + (b.x - a.x) * (float)t, a.y + (b.y - a.y) * (float)t, a.z + (b.z - a.z) * (float)t);
        }
        public static Vector3 lerpOnBezierControlRelToMiddle(in Vector3 start, in Vector3 controlRelToMiddle, in Vector3 end, double t)
        {
            return BezierFunc.GetPointQuadratic(t, start, lerp(in start, in end, 0.5) + controlRelToMiddle, end);
        }
        /*
         toX0 ------
                    --------- toX1
                    --------- fromX1
         fromX0 ----
         */
//        public static float lerpPlane(double fromX0, double toX0, double fromX1, double toX1, double x01, double fromTo01)
//        {
//            return lerp(lerp(fromX0, toX0, x01), lerp(fromX1, toX1, x01), fromTo01);
//        }
        public static float vDist(in Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorY = a.y - b.y;
            var vectorZ = a.z - b.z;
            return (float)Math.Sqrt((((double)vectorX * (double)vectorX) + ((double)vectorY * (double)vectorY)) + ((double)vectorZ * (double)vectorZ));
        }
        public static float vDist(Transform a, Transform b)
        {
            return vDist(a.position, b.position);
        }
        public static float vDistBy(in Vector3 a, in Vector3 b, double by)
        {
            var vectorX = a.x - b.x;
            var vectorY = a.y - b.y;
            var vectorZ = a.z - b.z;
            return (float)(by * Math.Sqrt((((double)vectorX * (double)vectorX) + ((double)vectorY * (double)vectorY)) + ((double)vectorZ * (double)vectorZ)));
        }
        public static float vDistSqr(in Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorY = a.y - b.y;
            var vectorZ = a.z - b.z;
            return (float)((((double)vectorX * (double)vectorX) + ((double)vectorY * (double)vectorY)) + ((double)vectorZ * (double)vectorZ));
        }
        public static float vHorzDist(in Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorZ = a.z - b.z;
            return (float)Math.Sqrt(((double)vectorX * (double)vectorX) + ((double)vectorZ * (double)vectorZ));
        }
        public static float vHorzDistSqr(in Vector3 a, in Vector3 b)
        {
            var vectorX = a.x - b.x;
            var vectorZ = a.z - b.z;
            return (float)(((double)vectorX * (double)vectorX) + ((double)vectorZ * (double)vectorZ));
        }
        public static double dotAsDouble(in Vector3 lhs, in Vector3 rhs)
        {
            return ((double)lhs.x * (double)rhs.x + (double)lhs.y * (double)rhs.y + (double)lhs.z * (double)rhs.z);
        }
        public static float dot2D(double lhsX, double lhsY, double rhsX, double rhsY)
        {
            return (float)(lhsX * rhsX + lhsY * rhsY);
        }
        public static float dot2D(in Vector2 lhs, in Vector2 rhs)
        {
            return (float)((double)lhs.x * (double)rhs.x + (double)lhs.y * (double)rhs.y);
        }
        public static float dot(in Vector3 lhs, in Vector3 rhs)
        {
            return (float)((double)lhs.x * (double)rhs.x + (double)lhs.y * (double)rhs.y + (double)lhs.z * (double)rhs.z);
        }
        public static float dotHorz(Vector3 lhs, Vector3 rhs)
        {
            return (float)((double)lhs.x * (double)rhs.x + (double)lhs.z * (double)rhs.z);
        }
        public static float dotHorz(in Vector3 lhs, in Vector3 rhs)
        {
            return (float)((double)lhs.x * (double)rhs.x + (double)lhs.z * (double)rhs.z);
        }
        public static float dot(in Quaternion a, in Quaternion b)
        {
            return (float)((double)a.x * (double)b.x + (double)a.y * (double)b.y + (double)a.z * (double)b.z + (double)a.w * (double)b.w);
        }
        public static Vector3 centroidUnit(in Vector3 a, in Vector3 b, in Vector3 c)
        {
            Vector3 cen;
            fun.triangle.GetCentroid(in a, in b, in c, out cen);
            return cen.normalized;
        }

        public static Vector3 move_rt_up_fw(double rtMeters, double upMeters, double fwMeters)
        {
            var vec = v3.zero;
            if (abs(rtMeters) >= 0.001) vec += (v3.rt * (float)rtMeters);
            if (abs(upMeters) >= 0.001) vec += (v3.up * (float)upMeters);
            if (abs(fwMeters) >= 0.001) vec += (v3.fw * (float)fwMeters);
            return vec;
        }
        public static Vector3 up_fw(double degrees) { return Vector3.SlerpUnclamped(Vector3.up, Vector3.forward, (float)(degrees / 90.0)); }
        public static Vector3 up_rt(double degrees) { return Vector3.SlerpUnclamped(Vector3.up, Vector3.right, (float)(degrees / 90.0)); }
        public static Vector3 up_lt(double degrees) { return Vector3.SlerpUnclamped(Vector3.up, Vector3.left, (float)(degrees / 90.0)); }
        public static Vector3 up_bk(double degrees) { return Vector3.SlerpUnclamped(Vector3.up, Vector3.back, (float)(degrees / 90.0)); }
        public static Vector3 dn_rt(double degrees) { return Vector3.SlerpUnclamped(Vector3.down, Vector3.right, (float)(degrees / 90.0)); }
        public static Vector3 dn_lt(double degrees) { return Vector3.SlerpUnclamped(Vector3.down, Vector3.left, (float)(degrees / 90.0)); }
        public static Vector3 fw_rt(double degrees) { return Vector3.SlerpUnclamped(Vector3.forward, Vector3.right, (float)(degrees / 90.0)); }
        public static Vector3 fw_lt(double degrees) { return Vector3.SlerpUnclamped(Vector3.forward, Vector3.left, (float)(degrees / 90.0)); }
        public static Vector3 fw_up(double degrees) { return Vector3.SlerpUnclamped(Vector3.forward, Vector3.up, (float)(degrees / 90.0)); }
        public static Vector3 fw_dn(double degrees) { return Vector3.SlerpUnclamped(Vector3.forward, Vector3.down, (float)(degrees / 90.0)); }
        public static Vector3 bk_rt(double degrees) { return Vector3.SlerpUnclamped(Vector3.back, Vector3.right, (float)(degrees / 90.0)); }
        public static Vector3 bk_lt(double degrees) { return Vector3.SlerpUnclamped(Vector3.back, Vector3.left, (float)(degrees / 90.0)); }
        public static Vector3 bk_up(double degrees) { return Vector3.SlerpUnclamped(Vector3.back, Vector3.up, (float)(degrees / 90.0)); }
        public static Vector3 bk_dn(double degrees) { return Vector3.SlerpUnclamped(Vector3.back, Vector3.down, (float)(degrees / 90.0)); }

        public static Vector3 fw_dn_rt(double degreesFwDn, double degreesToRt)
        {
            var dir = Vector3.SlerpUnclamped(Vector3.forward, Vector3.down, (float)(degreesFwDn / 90.0));
            return Vector3.SlerpUnclamped(dir, Vector3.right, (float)(degreesToRt / 90.0));
        }

        public static Vector3 slerp(in Vector3 a, double t, in Vector3 b)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return Vector3.SlerpUnclamped(a, b, (float)t);
        }
        public static Vector3 slerp(in Vector3 a, in Vector3 b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return Vector3.SlerpUnclamped(a, b, (float)t);
        }
        public static Vector3 slerp(in Vector3 a, double ab, in Vector3 b, double abc, in Vector3 c)
        {
            return Vector3.SlerpUnclamped(Vector3.SlerpUnclamped(a, b, (float)ab), c, (float)abc);
        }
        public static Quaternion slerp(in Quaternion b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return Quaternion.identity;
            if (t.IsEqual(1, 0.00001)) return b;
            return Quaternion.SlerpUnclamped(Quaternion.identity, b, (float)t);
        }
        public static Quaternion slerp(in Quaternion a, in Quaternion b, double t)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return Quaternion.SlerpUnclamped(a, b, (float)t);
        }
        public static Quaternion slerp(in Quaternion a, double t, in Quaternion b)
        {
            if (t.IsEqual(0, 0.00001)) return a;
            if (t.IsEqual(1, 0.00001)) return b;
            return Quaternion.SlerpUnclamped(a, b, (float)t);
        }
        public static Vector3 slerpFacingDir(in Vector3 a, in Vector3 b, in Vector3 dir, double t)
        {
            var midFw = slerp(in a, in b, 0.5);
            if (vector.PointSameDirection(in dir, in midFw))
            {
                return slerp(in a, in b, t); // we must rotate less than 180 degrees - default slerp works OK
            }
            // we must rotate more than 180 degrees
            return t <= 0.5
                ? slerp(in a, in midFw, t.FromRangeTo01(0.0, 0.5))
                : slerp(in midFw, in b, t.FromRangeTo01(0.5, 1.0));
        }
        public static Quaternion slerpFacingDir(in Quaternion a, in Quaternion b, in Vector3 dir, double t)
        {
            var mid = slerp(in a, in b, 0.5);
            var midFw = mid * v3.fw;
            if (vector.PointSameDirection(in dir, in midFw))
            {
                return slerp(in a, in b, t); // we must rotate less than 180 degrees - default slerp works OK
            }
            // we must rotate more than 180 degrees
            var midOpp = lookAt(mid * v3.bk, mid * v3.up);
            return t <= 0.5 
                ? slerp(in a, in midOpp, t.FromRangeTo01(0.0, 0.5)) 
                : slerp(in midOpp, in b, t.FromRangeTo01(0.5, 1.0));
        }
        public static void slerp(in Vector3 a, in Vector3 b, double t, out Vector3 d)
        {
            if (t.IsEqual(0, 0.00001))
            {
                d = a;
                return;
            }

            if (t.IsEqual(1, 0.00001))
            {
                d = b;
                return;
            }
            d = Vector3.SlerpUnclamped(a, b, (float)t);
        }
        public static Quaternion slerp2(in Quaternion a, in Quaternion b, double bStart, in Quaternion c, double t)
        {
            if(bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = "+ bStart);
            if (t <= bStart) return slerp(in a, in b, t.FromRangeTo01(0, bStart));
            return slerp(in b, in c, t.FromRangeTo01(bStart, 1));
        }
        public static Quaternion slerp2(in Quaternion a, in Quaternion b, double bStart, in Quaternion c, double t, Func<double, double> f1, Func<double, double> f2)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return slerp(in a, in b, f1(t.FromRangeTo01(0, bStart)));
            return slerp(in b, in c, f2(t.FromRangeTo01(bStart, 1)));
        }
        public static Vector3 slerp2(in Vector3 a, in Vector3 b, double bStart, in Vector3 c, double t)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return slerp(in a, in b, t.FromRangeTo01(0, bStart));
            return slerp(in b, in c, t.FromRangeTo01(bStart, 1));
        }
        public static Vector3 slerp2(in Vector3 a, in Vector3 b, double bStart, in Vector3 c, double t, Func<double, double> f1, Func<double, double> f2)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return slerp(in a, in b, f1(t.FromRangeTo01(0, bStart)));
            return slerp(in b, in c, f2(t.FromRangeTo01(bStart, 1)));
        }
        public static Color lerp2(in Color a, in Color b, double bStart, in Color c, double t)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return lerp(in a, in b, t.FromRangeTo01(0, bStart));
            return lerp(in b, in c, t.FromRangeTo01(bStart, 1));
        }
        public static Vector3 lerp2(in Vector3 a, in Vector3 b, double bStart, in Vector3 c, double t)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return lerp(in a, in b, t.FromRangeTo01(0, bStart));
            return lerp(in b, in c, t.FromRangeTo01(bStart, 1));
        }
        public static Vector3 lerp2(in Vector3 a, in Vector3 b, double bStart, in Vector3 c, double t, Func<double, double> f1, Func<double, double> f2)
        {
            if (bStart < 0 || bStart > 1) throw new InvalidEnumArgumentException("Limit must be between 0 and 1 it was = " + bStart);
            if (t <= bStart) return lerp(in a, in b, f1(t.FromRangeTo01(0, bStart)));
            return lerp(in b, in c, f2(t.FromRangeTo01(bStart, 1)));
        }
        public static Quaternion lookAt(in Vector3 fwDir, in Vector3 upDir) => Quaternion.LookRotation(fwDir, upDir);
        public static Quaternion lookAt(in Vector3 target, in Vector3 source, in Vector3 upDir) => Quaternion.LookRotation((target - source).normalized, upDir);
        public static Quaternion lookAtHorz(in Vector3 target, in Vector3 source) => Quaternion.LookRotation((target - source).ToHorzUnit(), v3.up);
        public static int ceil(double n)
        {
            return (int)Math.Ceiling(n);
        }
        public static int floor(double n)
        {
            return (int)Math.Floor(n);
        }

        public static Vector3 rotRel(double latitudeDegrees, double longitudeDegrees)
        {
            latitudeDegrees = latitudeDegrees % 180.0;
            longitudeDegrees = longitudeDegrees % 90.0;
            var dir = Quaternion.AngleAxis((float)-latitudeDegrees, v3.up) * Vector3.forward;
            return dir.RotateTowards(longitudeDegrees > 0 ? v3.up : v3.down, longitudeDegrees);
        }
        public static Vector3 rotAbs(double latitudeDegrees, double longitudeDegrees, Transform m)
        {
            latitudeDegrees = latitudeDegrees % 180.0;
            longitudeDegrees = longitudeDegrees % 90.0;
            var dir = Quaternion.AngleAxis((float)-latitudeDegrees, m.up) * m.forward;
            return dir.RotateTowards(longitudeDegrees > 0 ? m.up : -m.up, longitudeDegrees);
        }

        public static Vector3 vecNorm(in Vector3 lhs, in Vector3 rhs)
        {
            Vector3 norm;
            vector.GetNormal(in lhs, in rhs, out norm);
            return norm;
        }
        public static void vecNorm(in Vector3 lhs, in Vector3 rhs, out Vector3 norm)
        {
            vector.GetNormal(in lhs, in rhs, out norm);
        }


        public static Vector3 pntNorm(in Vector3 a, in Vector3 b, in Vector3 c)
        {
            point.GetNormal(in a, in b, in c, out var norm);
            return norm;
        }
        public static void pntNorm(in Vector3 a, in Vector3 b, in Vector3 c, out Vector3 norm)
        {
            point.GetNormal(in a, in b, in c, out norm);
        }
        public static Vector3 pntNormSameSideAs(in Vector3 a, in Vector3 b, in Vector3 c, in Vector3 sideDir)
        {
            point.GetNormal(in a, in b, in c, out var norm);
            vector.EnsurePointSameDirAs(in norm, in sideDir, out norm);
            return norm;
        }
        public static void pntNormSameSideAs(in Vector3 a, in Vector3 b, in Vector3 c, in Vector3 sideDir, out Vector3 norm)
        {
            point.GetNormal(in a, in b, in c, out norm);
            vector.EnsurePointSameDirAs(in norm, in sideDir, out norm);
        }


        /// <summary>
        /// random between min and max
        /// </summary>
        /// <param name="min">minimum random value</param>
        /// <param name="max">maximum random value</param>
        /// <returns></returns>
        public static float rnd(double min, double max)
        {
            return random.Between(min, max);
        }
        public static float rnd(double min, double max, Func<double, double> probability)
        {
            return random.Between(min, max, probability);
        }
        public static void execRnd(Action a, Action b)
        {
            if (number01 < 0.5) a();
            else b();
        }
        public static void execRnd(Action a, Action b, Action c)
        {
            var n = random.Between(0, 2);
            if (n == 0) a();
            else if (n == 1) b();
            else c();
        }
        public static void execRnd(Action a, Action b, Action c, Action d)
        {
            var n = random.Between(0, 3);
            if (n == 0) a();
            else if (n == 1) b();
            else if (n == 2) c();
            else d();
        }
        public static void execRnd(double probabilityA, Action a, double probabilityB, Action b, Action c)
        {
            var p = number01;
            if (p < probabilityA) a();
            else if (p < (probabilityA + probabilityB)) b();
            else c();
        }
        public static T getRnd<T>(double probabilityA, Func<T> a, double probabilityB, Func<T> b, Func<T> c)
        {
            var p = number01;
            if (p < probabilityA) return a();
            if (p < (probabilityA + probabilityB)) return b();
            return c();
        }
        public static T getRnd<T>(double probabilityA, Func<T> a, Func<T> b)
        {
            var p = number01;
            if (p < probabilityA) return a();
            return b();
        }
        public static int rndInt(int min, int max)
        {
            return random.Between(min, max);
        }
        public static T rndOf<T>(T a, T b)
        {
            var n = random.Between(0, 1);
            return n == 0 ? a : b;
        }
        public static T rndOf<T>(T a, T b, T c)
        {
            var n = random.Between(0, 2);

            return n == 0 ? a : n == 1 ? b : c;
        }
        public static T rndOf<T>(T a, T b, T c, T d)
        {
            var n = random.Between(0, 3);

            return n == 0 ? a : n == 1 ? b : n == 2 ? c : d;
        }
        public static T rndOf<T>(T a, T b, T c, T d, T e)
        {
            var n = random.Between(0, 4);

            return n == 0 ? a : n == 1 ? b : n == 2 ? c : n == 3 ? d : e;
        }
        public static T rndOf<T>(params T[] arr)
        {
            var i = random.Between(0, arr.Length - 1);

            return arr[i];
        }
        public static T rndOf<T>(IList<T> arr)
        {
            var i = random.Between(0, arr.Count - 1);

            return arr[i];
        }
        public static T rndOf<T>(Func<T> a, Func<T> b)
        {
            var n = random.Between(0, 1);
            return n == 0 ? a() : b();
        }
        public static T rndOf<T>(Func<T> a, Func<T> b, Func<T> c)
        {
            var n = random.Between(0, 2);

            return n == 0 ? a() : n == 1 ? b() : c();
        }
        public static T rndOf<T>(Func<T> a, Func<T> b, Func<T> c, Func<T> d)
        {
            var n = random.Between(0, 3);

            return n == 0 ? a() : n == 1 ? b() : n == 2 ? c() : d();
        }
        public static T rndOf<T>(Func<T> a, Func<T> b, Func<T> c, Func<T> d, Func<T> e)
        {
            var n = random.Between(0, 4);

            return n == 0 ? a() : n == 1 ? b() : n == 2 ? c() : n == 3 ? d() : e();
        }
        public static T rndOfExcept<T>(T[] arr, T except)
        {
            if (arr.Length == 1) return arr[0];
            T curr;
            var i = 0;
            do
            {
                curr = rndOf(arr);
                if (++i > 16) break;// check to prevent endless loop
            }
            while (curr.Equals(except));
            return curr;
        }
        public static float rndNumOf(params double[] arr)
        {
            return (float)rndOf(arr);
        }
        public static float rndNumOf(double a, double b)
        {
            return (float)rndOf(a, b);
        }
        public static float rndNumOf(double a, double b, double c)
        {
            return (float)rndOf(a, b, c);
        }
        public static float number01 { get { return random.number01; } }
        public static float numberM11 { get { return random.number01.From01ToMin11(); } }
        public static bool randomBool(double probability)
        {
            return random.Bool(probability);
        }
        public static float bezier(double x, double bx, double by, double cx, double cy)
        {
            return (float)BezierFunc.GetY(x, bx, by, cx, cy);
        }
        public static float bezier(double x, double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy)
        {
            return (float)BezierFunc.GetY(x, ax, ay, bx, by, cx, cy, dx, dy);
        }
        public static float bezier2parts(double x,
            double ax1, double ay1, double bx1, double by1, double cx1, double cy1,
            double ax2, double ay2, double bx2, double by2, double cx2, double cy2,
            double dx2, double dy2)
        {
            if (x <= ax2)
            {
                return (float)BezierFunc.GetY(x, ax1, ay1, bx1, by1, cx1, cy1, ax2, ay2);
            }
            return (float)BezierFunc.GetY(x, ax2, ay2, bx2, by2, cx2, cy2, dx2, dy2);
        }
        public static float bezier3parts(double x,
            double ax1, double ay1, double bx1, double by1, double cx1, double cy1,
            double ax2, double ay2, double bx2, double by2, double cx2, double cy2,
            double ax3, double ay3, double bx3, double by3, double cx3, double cy3,
            double dx3, double dy3)
        {
            if (x <= ax2)
            {
                return (float)BezierFunc.GetY(x, ax1, ay1, bx1, by1, cx1, cy1, ax2, ay2);
            }
            if (x <= ax3)
            {
                return (float)BezierFunc.GetY(x, ax2, ay2, bx2, by2, cx2, cy2, ax3, ay3);
            }
            return (float)BezierFunc.GetY(x, ax3, ay3, bx3, by3, cx3, cy3, dx3, dy3);
        }
        public static float bezier4parts(double x,
            double ax1, double ay1, double bx1, double by1, double cx1, double cy1,
            double ax2, double ay2, double bx2, double by2, double cx2, double cy2,
            double ax3, double ay3, double bx3, double by3, double cx3, double cy3,
            double ax4, double ay4, double bx4, double by4, double cx4, double cy4,
            double dx4, double dy4)
        {

            if (x <= ax2)
            {
                return (float)BezierFunc.GetY(x, ax1, ay1, bx1, by1, cx1, cy1, ax2, ay2);
            }
            if (x <= ax3)
            {
                return (float)BezierFunc.GetY(x, ax2, ay2, bx2, by2, cx2, cy2, ax3, ay3);
            }
            if (x <= ax4)
            {
                return (float)BezierFunc.GetY(x, ax3, ay3, bx3, by3, cx3, cy3, ax4, ay4);
            }
            return (float)BezierFunc.GetY(x, ax4, ay4, bx4, by4, cx4, cy4, dx4, dy4);
        }
        public static int round(double d)
        {
            return (int)Math.Round(d);
        }
        public static float round(double d, int places)
        {
            return (float)Math.Round(d, places);
        }
        public static IBezierGroup beziers(Action<IBezierGroupBuilder> setup)
        {
            return ((IBezierGroupBuilder)new BezierGroup()).setup(setup);
        }

        public static bool isEvenFrame => Time.frameCount % 2 == 0;
        public static bool isFrame(int frame) { return Time.frameCount % frame == 0; }
        public static float range01(double progress, double min, double max)
        {
            return (float)(min + progress * (max - min));
        }
        public static float range11(double progress, double min, double max)
        {
            progress = 0.5f * progress + 0.5f;
            return (float)(min + progress * (max - min));
        }

        public static float clamp(double n, double min, double max)
        {
            return (float)(n < min ? min : n > max ? max : n);
        }
        public static float clamp01(double n)
        {
            return (float)(n < 0 ? 0 : n > 1 ? 1 : n);
        }
        public static float clampMin11(double n)
        {
            return (float)(n < -1 ? -1 : n > 1 ? 1 : n);
        }
        public static float ratio01ByRange(double value, double from, double to, bool clamp)
        {
            var all = to - from;
            if (all.IsZero())
            {
                return value < from ? 0 : 1;
            }
            var curr = value - from;
            var ratioRaw = curr / all;
            return (float)(clamp ? ratioRaw.Clamp01() : ratioRaw);
        }
        public static float ratioMin11ByRange(double value, double from, double to, bool clamp)
        {
            var all = to - from;
            if (all.IsZero())
            {
                return value < from ? 0 : 1;
            }
            var curr = value - from;
            var ratioRaw = curr / all;
            return (float)(clamp ? ratioRaw.From01ToMin11().ClampMin11() : ratioRaw.From01ToMin11());
        }

        public static Transform wrapTransformInHolder(Transform current, Vector3 lookFwWorldDir, Vector3 lookUpWorldDir)
        {
            return wrapTransformInHolder(current.parent, current, lookFwWorldDir, lookUpWorldDir);
        }
        public static Transform wrapTransformInHolder(Transform parent, Transform current, Vector3 lookFwWorldDir, Vector3 lookUpWorldDir)
        {
            // already wrapped
            if (current.parent != null && current.parent.name == current.name + HolderSuffix) return current.parent;
            var currentHolder = new GameObject(current.name + HolderSuffix);
            var holder = currentHolder.transform;
            holder.position = current.position;
            holder.rotation = Quaternion.LookRotation(lookFwWorldDir, lookFwWorldDir.GetRealUp(lookUpWorldDir));
            holder.SetParent(parent);
            current.SetParent(holder);
            var children = current.ForEachDirectChild(new List<Transform>(), (t, list) => list.Add(t));
            foreach (var child in children)
            {
                child.SetParent(holder);
            }
            return holder;
        }
        public static Transform CreateHandle(
            Transform handleParent, BodyPart part, bool isHandleVisibled,
            Vector3 worldPosition, Vector3 worldForward, Vector3 worldUp)
        {
            GameObject handle;
            if (isHandleVisibled)
            {
                handle =
                    meshes.CreatePointyCone(new DtCone { name = part + HandleSuffix })
                        .SetMaterial(m => m.color = HandleColor);//m.SetStandardShaderRenderingModeTransparent()
                const float scale = 0.1f;
                handle.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                handle = new GameObject(part + HandleSuffix);
            }
            if(handleParent != null) handle.transform.SetParent(handleParent);
            handle.transform.position = worldPosition;
            handle.transform.LookAt(worldPosition + worldForward, worldUp);
            return handle.transform;
        }
        public static SkipFramesAni skipFramesThen(int frames, Action then)
        {
            var skipAni = get<SkipFramesAni>().Set(frames);
            skipAni.Then(then);
            fire(new PlayAni { Ani = skipAni, QueueIndex = aniQueue.MainQueue });
            return skipAni;
        }
        public static SkipFramesAni skipFramesThenFire<TEvent>(int frames, Func<TEvent> getEvent) where TEvent : BaseEvent
        {
            var skipAni = get<SkipFramesAni>().Set(frames);
            skipAni.Then(() => fire(getEvent()));
            fire(new PlayAni { Ani = skipAni, QueueIndex = aniQueue.MainQueue });
            return skipAni;
        }
        public static WaitAni waitFor(double seconds)
        {
            var waitAni = get<WaitAni>().Set(seconds);
            fire(new PlayAni { Ani = waitAni, QueueIndex = aniQueue.MainQueue });
            return waitAni;
        }
        public static WaitAni waitThen(double seconds, Action then)
        {
            var waitAni = get<WaitAni>().Set(seconds);
            waitAni.Then(then);
            fire(new PlayAni { Ani = waitAni, QueueIndex = aniQueue.MainQueue });
            return waitAni;
        }
        public static WaitAni waitThenFire<TEvent>(double seconds, Func<TEvent> getEvent) where TEvent : BaseEvent
        {
            var waitAni = get<WaitAni>().Set(seconds);
            waitAni.Then(() => fire(getEvent()));
            fire(new PlayAni { Ani = waitAni, QueueIndex = aniQueue.MainQueue });
            return waitAni;
        }
        public static TAni waitThenPlay<TAni>(double seconds, int queueIndex = 0) where TAni : IAnimation
        {
            var wait = get<WaitAni>().Set(seconds);
            var ani = get<TAni>();
            wait.SetFollowing(() => ani);
            fire(new PlayAni { Ani = wait, QueueIndex = queueIndex });
            return ani;
        }
        public static TAni waitThenPlayGlobal<TAni>(double seconds, TAni ani, int queueIndex = aniQueue.MainQueue) where TAni : IAnimation
        {
            var wait = get<WaitAni>().Set(seconds);
            wait.SetFollowing(() => ani);
            fire(new PlayAni { Ani = wait, QueueIndex = queueIndex });
            return ani;
        }
        public static FuncAni waitThenPlayFor(double secondsWait, double secondsPlay, Action<float> update) 
        {
            var wait = get<WaitAni>().Set(secondsWait);
            var ani = get<FuncAni>().Set(secondsPlay, update);
            wait.SetFollowing(() => ani);
            fire(new PlayAni { Ani = wait, QueueIndex = aniQueue.MainQueue });
            return ani;
        }
        public static IntervalAni repeatOn(double seconds, Action<IntervalAni> repeatThis)
        {
            var waitAni = get<IntervalAni>().Set(seconds, repeatThis);
            fire(new PlayAni { Ani = waitAni, QueueIndex = aniQueue.MainQueue });
            return waitAni;
        }
        public static FuncAni playFor(double seconds, Action<float> update, int queueIndex = aniQueue.MainQueue) 
        {
            var ani = get<FuncAni>().Set(seconds, update);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static FuncAni playFor(double seconds, Action<FuncAni> init, Action<float> update, int queueIndex = aniQueue.MainQueue)
        {
            var ani = get<FuncAni>().Set(seconds, init, update);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static FuncAni playFor(double seconds, Action<FuncAni> init, Action<FuncAni, float> update, int queueIndex = aniQueue.MainQueue)
        {
            var ani = get<FuncAni>().Set(seconds, init, update);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        /*
         WWW www = new WWW("https://docs.unity3d.com/ScriptReference/WWW.html");
        runCoroutine(www, timeoutSeconds:0.1, onComplete:success =>
        {
            Debug.Log($"{success}");
            if (success)
            {
                Debug.Log(www.text);
            }
            www.Dispose();
        });
         */
        public static CoroutineAni runCoroutine(IEnumerator enumerator, double timeoutSeconds = -1, Action<bool> onComplete = null, int queueIndex = aniQueue.MainQueue)
        {
            var ani = get<CoroutineAni>().Set(enumerator, timeoutSeconds, onComplete);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static TAni play<TAni>(int queueIndex = aniQueue.MainQueue) where TAni : IAnimation
        {
            var ani = get<TAni>();
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static TAni play<TAni>(TAni ani, int queueIndex = aniQueue.MainQueue) where TAni : IAnimation
        {
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static void hands<T>(IComplexHuman human, double seconds = 0.5) where T : IHandAni, IAnimation
        {
            play<T>().Set(human, BodySide.LT, seconds);
            play<T>().Set(human, BodySide.RT, seconds);
        }
        public static void hands<TLeft, TRight>(IComplexHuman human, double seconds = 0.5)
            where TLeft : IHandAni, IAnimation
            where TRight : IHandAni, IAnimation
        {
            play<TLeft>().Set(human, BodySide.LT, seconds);
            play<TRight>().Set(human, BodySide.RT, seconds);
        }
        public static void handsPlayOn<T>(IComplexHuman human, AnimationManager am, double seconds = 0.5) where T : IHandAni, IAnimation
        {
            ((IAnimation)get<T>().Set(human, BodySide.LT, seconds)).PlayOn(am);
            ((IAnimation)get<T>().Set(human, BodySide.RT, seconds)).PlayOn(am);
        }
        public static void handsPlayOn<TLeft, TRight>(IComplexHuman human, AnimationManager am, double seconds = 0.5)
            where TLeft : IHandAni, IAnimation
            where TRight : IHandAni, IAnimation
        {
            ((IAnimation)get<TLeft>().Set(human, BodySide.LT, seconds)).PlayOn(am);
            ((IAnimation)get<TRight>().Set(human, BodySide.RT, seconds)).PlayOn(am);
        }
        public static EndlessFuncAni playEndless(Action<EndlessFuncAni> update, int queueIndex = aniQueue.MainQueue)
        {
            var ani = get<EndlessFuncAni>().Set(update);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static EndlessFuncAni playEndless(Action<EndlessFuncAni> init, Action<EndlessFuncAni> update, int queueIndex = aniQueue.MainQueue)
        {
            var ani = get<EndlessFuncAni>().Set(init, update);
            fire(new PlayAni { Ani = ani, QueueIndex = queueIndex });
            return ani;
        }
        public static IMessenger Messenger
            => _messenger ?? (_messenger = GlobalFactory.Default.Get<IMessenger>());

        //        internal ProbabilityBuilder With(double probability, Action action)
        //        {
        //            return new ProbabilityBuilder().Or(probability, action);
        //        }

        /// <summary>
        /// if no subscribers, the event WILL NOT be queued
        /// </summary>
        public static int fireButDontQueue<T>(T ev) where T : BaseEvent
        {
            if (ev == null) return 0;
            return Messenger.Invoke(ev);
        }
        /// <summary>
        /// if no subscribers, the event WILL be queued
        /// </summary>
        public static int fire<T>(T ev) where T : BaseEvent
        {
            if (ev == null) return 0;
            return Messenger.InvokeOrQueue(ev);
        }
        /// <summary>
        /// if no subscribers, the event WILL NOT be queued
        /// </summary>
        public static T fireAndReturn<T>(T ev) where T : BaseEvent
        {
            fireButDontQueue(ev);
            return ev;
        }
        /// <summary>
        /// if no subscribers, the event WILL NOT be queued
        /// </summary>
        public static T fireEventAndReturn<T>() where T : BaseEvent, new()
        {
            return fireAndReturn(new T());
        }

        //TODO:enable
        /*
        public static void stopIfRunning(IAnimatedHumanoid om, HumanoidPart part)
        {
            stopIfRunningExcept(om, part, NoAni.Finished);
        }
        public static void stopIfRunningExcept(IAnimatedHumanoid cm, HumanoidPart part, IAnimation except)
        {
            if ((part & HumanoidPart.EntireBody) > 0 && cm.AniEntireBody != except) cm.AniEntireBody.StopIfRunning();
            if ((part & HumanoidPart.ArmL) > 0 && cm.AniArmL != except) cm.AniArmL.StopIfRunning();
            if ((part & HumanoidPart.ArmR) > 0 && cm.AniArmR != except) cm.AniArmR.StopIfRunning();
            if ((part & HumanoidPart.LegL) > 0 && cm.AniLegL != except) cm.AniLegL.StopIfRunning();
            if ((part & HumanoidPart.LegR) > 0 && cm.AniLegR != except) cm.AniLegR.StopIfRunning();
            if ((part & HumanoidPart.Pelvis) > 0 && cm.AniTorso != except) cm.AniTorso.StopIfRunning();
            if ((part & HumanoidPart.HandL) > 0 && cm.AniHandL != except) cm.AniHandL.StopIfRunning();
            if ((part & HumanoidPart.HandR) > 0 && cm.AniHandR != except) cm.AniHandR.StopIfRunning();
            if ((part & HumanoidPart.Head) > 0 && cm.AniHead != except) cm.AniHead.StopIfRunning();
//            if ((part & HumanoidPart.Walk) > 0 && cm.AniWalk != except) cm.AniWalk.StopIfRunning();
            if ((part & HumanoidPart.BreastL) > 0 && cm.AniBreastL != except) cm.AniBreastL.StopIfRunning();
            if ((part & HumanoidPart.BreastR) > 0 && cm.AniBreastR != except) cm.AniBreastR.StopIfRunning();
            if ((part & HumanoidPart.BottomL) > 0 && cm.AniBottomL != except) cm.AniBottomL.StopIfRunning();
            if ((part & HumanoidPart.BottomR) > 0 && cm.AniBottomR != except) cm.AniBottomR.StopIfRunning();
        }
        public static void stopIfRunning(IAnimation a1, IAnimation a2) { a1.StopIfRunning(); a2.StopIfRunning(); }
        public static void stopIfRunning(IAnimation a1, IAnimation a2, IAnimation a3) { a1.StopIfRunning(); a2.StopIfRunning(); a3.StopIfRunning(); }
        public static void stopIfRunning(IAnimation a1, IAnimation a2, IAnimation a3, IAnimation a4) { a1.StopIfRunning(); a2.StopIfRunning(); a3.StopIfRunning(); a4.StopIfRunning(); }
        public static void stopIfRunning(IAnimation a1, IAnimation a2, IAnimation a3, IAnimation a4, IAnimation a5) { a1.StopIfRunning(); a2.StopIfRunning(); a3.StopIfRunning(); a4.StopIfRunning(); a5.StopIfRunning(); }
        public static bool anyIsRunning(IAnimation a1, IAnimation a2) { return a1.IsRunning() || a2.IsRunning(); }
        public static bool anyIsRunning(IAnimation a1, IAnimation a2, IAnimation a3) { return a1.IsRunning() || a2.IsRunning() || a3.IsRunning(); }
        public static bool anyIsRunning(IAnimation a1, IAnimation a2, IAnimation a3, IAnimation a4) { return a1.IsRunning() || a2.IsRunning() || a3.IsRunning() || a4.IsRunning(); }
        public static bool anyIsRunning(IAnimation a1, IAnimation a2, IAnimation a3, IAnimation a4, IAnimation a5) { return a1.IsRunning() || a2.IsRunning() || a3.IsRunning() || a4.IsRunning() || a5.IsRunning(); }
        public static bool anyIsRunning(IAnimatedHumanoid om, HumanoidPart part)
        {
            return anyIsRunningExcept(om, part, NoAni.Finished);
        }
        public static bool anyIsRunningExcept(IAnimatedHumanoid cm, HumanoidPart part, IAnimation except)
        {
            if ((part & HumanoidPart.EntireBody) > 0 && cm.AniEntireBody.IsRunning() && cm.AniEntireBody != except) return true;
            if ((part & HumanoidPart.ArmL) > 0 && cm.AniArmL.IsRunning() && cm.AniArmL != except) return true;
            if ((part & HumanoidPart.ArmR) > 0 && cm.AniArmR.IsRunning() && cm.AniArmR != except) return true;
            if ((part & HumanoidPart.LegL) > 0 && cm.AniLegL.IsRunning() && cm.AniLegL != except) return true;
            if ((part & HumanoidPart.LegR) > 0 && cm.AniLegR.IsRunning() && cm.AniLegR != except) return true;
            if ((part & HumanoidPart.Pelvis) > 0 && cm.AniTorso.IsRunning() && cm.AniTorso != except) return true;
            if ((part & HumanoidPart.HandL) > 0 && cm.AniHandL.IsRunning() && cm.AniHandL != except) return true;
            if ((part & HumanoidPart.HandR) > 0 && cm.AniHandR.IsRunning() && cm.AniHandR != except) return true;
            if ((part & HumanoidPart.Head) > 0 && cm.AniHead.IsRunning() && cm.AniHead != except) return true;
//            if ((part & HumanoidPart.Walk) > 0 && cm.AniWalk.IsRunning() && cm.AniWalk != except) return true;
            if ((part & HumanoidPart.BreastL) > 0 && cm.AniBreastL.IsRunning() && cm.AniBreastL != except) return true;
            if ((part & HumanoidPart.BreastR) > 0 && cm.AniBreastR.IsRunning() && cm.AniBreastR != except) return true;
            if ((part & HumanoidPart.BottomL) > 0 && cm.AniBottomL.IsRunning() && cm.AniBottomL != except) return true;
            if ((part & HumanoidPart.BottomR) > 0 && cm.AniBottomR.IsRunning() && cm.AniBottomR != except) return true;
            return false;
        }
        */
        public static void forceEndNamedAnyOf(object requester, params string[] names)
        {
            foreach (var name in names)
            {
                fireButDontQueue(new EnsureNamedUniqueEvent(name, requester));
            }
        }
        public static void forceEndNamed(string name, object requester)
        {
            fireButDontQueue(new EnsureNamedUniqueEvent(name, requester));
        }
        public static void clearEventQueue()
        {
            Messenger.ClearEventQueue();
        }
        public static void subscribe<T>(Action<T> action, object subscriber) where T : BaseEvent
        {
            Messenger.Subscribe<T>(subscriber, action);
        }
        public static void subscribe<T>(object name, Action<T> action, object subscriber) where T : BaseEvent { Messenger.Subscribe<T>(subscriber, name.ToString(), action); }
        public static void subscribe<T>(IIdHolder idHolder, Action<T> action, object subscriber) where T : BaseEvent { Messenger.Subscribe<T>(subscriber, idHolder.ID.ToString(), action); }
        public static void unsubscribe(object subscriber)
        {
            Messenger.Unsubscribe(subscriber);
        }
        public static void unsubscribeFromEvent<T>() where T : BaseEvent
        {
            Messenger.UnsubscribeByArg<T>();
        }
        public static void unsubscribeFromEvent<T>(object subscriber) where T : BaseEvent
        {
            Messenger.UnsubscribeByArg<T>(subscriber);
        }
        public static void unsubscribeFromEvent<T>(object name, object subscriber) where T : BaseEvent
        {
            Messenger.UnsubscribeByArg<T>(subscriber, name.ToString());
        }

    }

    
}