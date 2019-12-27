using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unianio.Animations;
using Unianio.Animations.Common;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Extensions;
using Unianio.Services;
using Unianio.Services.Drawing;
using Unianio.Static;
using UnityEngine;

namespace Unianio
{
    public static class dbg
    {
        public static System.Diagnostics.Stopwatch StartStopwatch()
        {
            return System.Diagnostics.Stopwatch.StartNew();
        }

        public static void SavePosition(Transform handle, Transform model = null)
        {
            SavePosition(1, handle, model);
        }
        public static void SavePosition(int i, Transform handle, Transform model = null)
        {
            dbg.OnShiftNumKeyDown(i, () =>
            {
                if (dbg.blns[i]) return;// so that it runs only once
                dbg.blns[i] = true;
                var sp = model == null ? ObjectSpace.Local : ObjectSpace.Model;
                var space = sp == ObjectSpace.Local ? handle.parent : model;
                var pref = sp == ObjectSpace.Local ? "Local" : "Model";
                //var space = h.Model;
                var p = handle.position.AsLocalPoint(space);
                var f = (handle.rotation * v3.fw).AsLocalDir(space);
                var u = (handle.rotation * v3.up).AsLocalDir(space);
                var list0 = new List<string>();
                if (fun.abs(p.x) > 0.0001) list0.Add($"v3.rt * {p.x.Round(3)}f");
                if (fun.abs(p.y) > 0.0001) list0.Add($"v3.up * {p.y.Round(3)}f");
                if (fun.abs(p.z) > 0.0001) list0.Add($"v3.fw * {p.z.Round(3)}f");
                if (list0.Count == 0) list0.Add("v3.zero");
                //dbg.log($".LocalPosLineToTarget({list0.JoinAsString(" + ")})");
                dbg.log($".{pref}PosLineToTarget(move_rt_up_fw({p.x.Round(3)},{p.y.Round(3)},{p.z.Round(3)}))");

                fun.vector.ProjectOnPlane(in f, in v3.rt, out var fProj);
                var degToUp = fun.angle.BetweenVectorsSignedInDegrees(in v3.fw, in fProj, in v3.lt);
                var norm0 = fun.abs(degToUp) >= 0.001 ? fun.vector.GetNormal(Vector3.forward, in fProj) : Vector3.right;
                var norm1 = fun.vector.GetNormal(in fProj, in norm0);
                var degToNo = fun.angle.BetweenVectorsSignedInDegrees(in fProj, in f, in norm1);
                var str1 = $"rotate.FwToUpAndNormal({degToUp.Round()}, {degToNo.Round()})";

                fun.vector.ProjectOnPlane(in u, in v3.rt, out var uProj);
                degToUp = fun.angle.BetweenVectorsSignedInDegrees(in v3.fw, in uProj, in v3.lt);
                norm0 = fun.abs(degToUp) >= 0.001 ? fun.vector.GetNormal(Vector3.forward, in uProj) : Vector3.right;
                norm1 = fun.vector.GetNormal(in uProj, in norm0);
                degToNo = fun.angle.BetweenVectorsSignedInDegrees(in uProj, in u, in norm1);
                var str2 = $"rotate.FwToUpAndNormal({degToUp.Round()}, {degToNo.Round()})";

                dbg.log($".{pref}RotToTarget({str1}, {str2})");
            });

            dbg.OnShiftNumKeyDown(i+1, () => { dbg.blns[i] = false; });
        }
        public static void PopulatePositionsAndDisable(string nameTemplate, Vector3[] vectors)
        {
            for(var i = 0; i < vectors.Length; ++i)
            {
                var go = GameObject.Find(nameTemplate.Args(i));
                if(go == null) continue;
                var pos = go.transform.position;
                go.SetActive(false);
//log("var t"+i+" = new Vector3("+pos.x+"f,"+pos.y+"f,"+pos.z+"f);");
                vectors[i] = pos;
            }
        }
        public static string CreateMap(string prefix)
        {
            prefix = prefix.Trim();

            var sb = new StringBuilder();
            sb.Append("new []{");
            var i = 0;
            for(; i < 99999; ++i)
            {
                var go = GameObject.Find(prefix+" ("+i+")");
                if(go == null && i == 0) go = GameObject.Find(prefix);
                if(go == null) break;
                var p = go.transform.position;
                if(i > 0) sb.Append(", ");
                sb.Append("new Vector3("+p.x+"f,"+p.y+"f,"+p.z+"f)");
            }
            sb.Append("}");
            sb.Insert(0,"/*count="+i+"; index=0-"+(i-1)+"*/\n");
            return sb.ToString();
        }
        public static void DistributeMap(string prefix, Vector3[] points)
        {
            prefix = prefix.Trim();
            var sb = new StringBuilder();
            sb.Append("new []{");
            var i = 0;
            for(; i < 99999; ++i)
            {
                var go = GameObject.Find(prefix+" ("+i+")");
                if(go == null && i == 0) go = GameObject.Find(prefix);
                if(go == null) break;
                go.transform.position = points[i];
            }
        }
        public static Color GetColor(object obj) => Colors[obj.GetHashCode().Abs() % Colors.Length];
        public static readonly Color[] Colors = { Color.red, Color.blue, Color.green, Color.magenta, Color.cyan, Color.yellow, Color.black, Color.white, new Color(1.0f, 0.6f, 0.3f, 1.0f) };
        static string _append = "";
        static string _center = "";
        static string _right = "";
        public static object center
        {
            get { return _center; }
#if UNIANIO_DEBUG
            set { _center = (value??"").ToString(); }
#endif // #if UNIANIO_DEBUG
        }
        public static object right
        {
            get { return _right; }
            set { _right = (value??"").ToString(); }
        }
        public static object append
        {
            get { return _append; }
            set
            {
                if(value == null) return;
                var str = value.ToString();
                if(str == string.Empty) return;
                _append += str+"\n";
            }
        }

        private static int _currColor = 0;
        public static Color diffColorEachCall => Colors[(_currColor++) % Colors.Length];
        public static Color colorByIndex(int i) => Colors[Math.Abs(i) % Colors.Length];


        

        static bool _appendInitialized;
        public static readonly string filePath = Application.persistentDataPath + @"/__log.txt";
        public static void log(object arg0)
        {
            if (!_appendInitialized) clearLog();
            System.IO.File.AppendAllText(filePath, Convert.ToString(arg0) + "\r\n");
        }
        public static void log(object arg0, object arg1)
        {
            if (!_appendInitialized) clearLog();
            System.IO.File.AppendAllText(filePath, Convert.ToString(arg0) + "\t"+ Convert.ToString(arg1) + "\r\n");
        }
        public static void log(object arg0, object arg1, object arg2)
        {
            if (!_appendInitialized) clearLog();
            System.IO.File.AppendAllText(filePath, Convert.ToString(arg0) + "\t" + Convert.ToString(arg1) + "\t" + Convert.ToString(arg2) + "\r\n");
        }
        public static void log(params object[] args)
        {
            if (!_appendInitialized) clearLog();
            System.IO.File.AppendAllText(filePath, string.Join("\t", args.Select<object,string>(Convert.ToString).ToArray()) + "\r\n");
        }
        public static void clearLog()
        {
            System.IO.File.WriteAllText(filePath, "");
            _appendInitialized = true;
        }
        public static void xlog(object arg0) { if (isXact) log(arg0); }
        public static void xlog(object arg0, object arg1) { if (isXact) log(arg0, arg1); }
        public static void xlog(object arg0, object arg1, object arg2) { if (isXact) log(arg0, arg1, arg2); }
        public static void xlog(params object[] args) { if (isXact) log(args); }


        static bool _dLogInitialized;
        static readonly List<string> _dLogs = new List<string>();
        public static void dlog(object arg0)
        {
            if (!_dLogInitialized)
            {
                fun.subscribe<EndGame>(e => SaveDlog(), _dLogs);

                _dLogs.Clear();
                _dLogInitialized = true;
            }
            _dLogs.Add(Convert.ToString(arg0));
        }
        public static void SaveDlog()
        {
            System.IO.File.WriteAllText(filePath, _dLogs.JoinAsString("\r\n"));
            _dLogs.Clear();
        }
        public static void dlog(object arg0, object arg1) { dlog(arg0 + "\t" + arg1); }
        public static void dlog(object arg0, object arg1, object arg2) { dlog(arg0 + "\t" + arg1 + "\t" + arg2); }
        public static void dlog(params object[] args) { dlog(args.JoinAsString("\t")); }
        public static EndlessFuncAni OnShiftNumKeyDownOrEvent<T>(int num, Action<T> action) where T : BaseEvent
        {
            var eAni = GlobalFactory.Default.Get<EndlessFuncAni>();
            GlobalFactory.Default.Get<IMessenger>().Subscribe(eAni, action);

            var key = (KeyCode)(48+num);
            return
                eAni.SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (!hasPrev && hasNext && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) action(null);
                    })
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }
        public static EndlessFuncAni OnShiftNumKeyDown(int num, Action action)
        {
            var key = (KeyCode)(48+num);
            return
                GlobalFactory.Default.Get<EndlessFuncAni>().SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (!hasPrev && hasNext && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) action();
                    })
                    .AsUniqueNamed("Debug_OnShiftNumKeyDown_"+num)
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }
        public static EndlessFuncAni OnShiftNumKeyUp(int num, Action action)
        {
            var key = (KeyCode)(48+num);
            return
                GlobalFactory.Default.Get<EndlessFuncAni>().SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (hasPrev && !hasNext && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                            action();
                    })
                    .AsUniqueNamed("Debug_OnShiftNumKeyUp_" + num)
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }
        public static EndlessFuncAni OnShiftKeyDown(KeyCode key, Action action)
        {
            return
                GlobalFactory.Default.Get<EndlessFuncAni>().SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (!hasPrev && hasNext && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) action();
                    })
                    .AsUniqueNamed("Debug_OnShiftKeyDown_" + key)
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }
        public static EndlessFuncAni OnShiftKeyUp(KeyCode key, Action action)
        {
            return
                GlobalFactory.Default.Get<EndlessFuncAni>().SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (hasPrev && !hasNext && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                            action();
                    })
                    .AsUniqueNamed("Debug_OnShiftKeyUp_" + key)
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }
        public static EndlessFuncAni OnArrowDown(Direction dir, Action action)
        {
            var key = 
                dir == Direction.Forward 
                ? KeyCode.UpArrow 
                : dir == Direction.Back 
                ? KeyCode.DownArrow 
                : dir == Direction.Left
                ? KeyCode.LeftArrow 
                : dir == Direction.Right
                ? KeyCode.RightArrow : KeyCode.Delete;

            if(key == KeyCode.Delete)
                throw new ArgumentException("Direction "+dir+" is not allowed, use Forward|Back|Left|Right");
            return
                GlobalFactory.Default.Get<EndlessFuncAni>().SetState(false)
                    .Set(ani =>
                    {
                        var hasNext = Input.GetKey(key);
                        var hasPrev = (bool)ani.State;
                        ani.State = hasNext;
                        if (hasPrev && !hasNext)
                            action();
                    })
                .Play(GlobalFactory.Default.Get<IMessenger>());
        }

        static bool _needsBoolMark;
        static bool _inFrameFirst;
        static IDictionary<string, object> _inFrame;
        public static void startInFrameLog(bool needsBoolMark, Func<string> getHeaders = null)
        {
            _needsBoolMark = needsBoolMark;
            if(_needsBoolMark && !isXact) return;
            if (_inFrame == null)
            {
                _inFrameFirst = true;
                _inFrame = new Dictionary<string, object>();
            }
            else
            {
                _inFrame.Clear();
                _inFrameFirst = false;
            }
            if (getHeaders != null)
            {
                foreach (var arr in getHeaders().Split(new[] {',', ';', '|'}).Select(s => s.Split(':').Select(e => e.Trim()).ToArray()))
                {
                    frameLog(arr[0], arr.Length > 1 ? arr[1] : "");
                }
            }
        }
        public static void frameLog(string name, object val)
        {
            if(_needsBoolMark && !isXact) return;
            if(_inFrame == null) throw new Exception("Call dbg.startInFrameLog before calling frameLog");
            _inFrame[name] = val;
        }
        public static void frameLogMany(params object[] vals)
        {
            if(_needsBoolMark && !isXact) return;
            if(_inFrame == null) throw new Exception("Call dbg.startInFrameLog before calling frameLogMany");
            if(vals == null || vals.Length==0) return;
            if(vals.Length%2!=0) throw new Exception("dbg.frameLogMany must have even number of arguments");
            for(var i = 0; i < vals.Length; i+=2)
                _inFrame[vals[i].ToString()] = vals[i+1];
        }
        /*internal static void endInFrameLog()
        {
            if(_needsBoolMark && !bln1) return;
            if (_inFrameFirst)
            {
                log(_inFrame.Keys.Cast<object>().ToArray());
            }
            log(_inFrame.Values.ToArray());
        }*/

        public static void pauseNextFrame()
        {
#if UNITY_EDITOR
            GlobalFactory.Default
                .Get<IMessenger>()
                .Invoke(new PlayAni{
                    Ani = GlobalFactory.Default
                    .Get<SkipFramesAni>()
                    .Set(1)
                    .Then(() => UnityEditor.EditorApplication.isPaused = true)
                });
#endif
        }
        public static void pause()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
        public static void xpause()
        {
#if UNITY_EDITOR
            if (isXact) UnityEditor.EditorApplication.isPaused = true;
#endif
        }
        public static void unpause()
        {
            UnityEditor.EditorApplication.isPaused = false;
        }
        public static bool isXact;
        public static readonly bool[] blns = new bool[16];
        public static int int1;
        public static float flo1;
        public static float flo2;
        public static Vector3 vec1;
        public static Transform tr1;
        public static object obj;
        public static Func<double, double> fun1;
        public static Transform EnsureTransform(Vector3 ini)
        {
            if (tr1 == null)
            {
                tr1 = fun.meshes.CreatePointyCone(
                        new DtCone {name = "testobj", height = 0.05, bottomRadius = 0.05, topRadius = 0.001f})
                        .SetStandardShaderTransparentColor(1,0,0,1)
                        .transform;
                tr1.position = ini;
            }
            return tr1;
        }
        public static Transform EnsureTransform() { return EnsureTransform(Vector3.zero); }
        public static string st
        {
            get
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                return trace.ToString(); //StackTraceUtility.ExtractStackTrace();
            }
        }
        public static string stln
        {
            get
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                return trace.ToString().Replace("\n"," ").Replace("\r"," "); //StackTraceUtility.ExtractStackTrace();
            }
        }
//        private static readonly IDictionary<int, RenderedLine> _lineById = new Dictionary<int, RenderedLine>();
        private static readonly ISimpleLineDrawer _drawer = new SimpleLineDrawer(false);
        private static EndlessDrawFuncAni _drawAni;
        private static readonly IDictionary<int, LineDef> _lineById = new Dictionary<int, LineDef>();

        public static void DrawBezier(int id, in Vector3 from, in Vector3 c1, in Vector3 to)
            => DrawBezier(id, in from, in c1, in to, Color.red);
        public static void DrawBezier(int id, in Vector3 from, in Vector3 c1, in Vector3 to, in Color color)
        {
            const int segments = 16;
            var part = (1f / segments);
            for (var i = 1; i < segments; ++i)
            {
                var idx = (1 << 20) + 1000 * id + i;
                var a = fun.bezierV3(in from, in c1, in to, part * (i - 1));
                var b = fun.bezierV3(in from, in c1, in to, part * i);
                DrawLine(idx, in a, in b, in color);
            }
        }
        public static void DrawBezier(int id, in Vector3 from, in Vector3 c1, in Vector3 c2, in Vector3 to)
            => DrawBezier(id, in from, in c1, in c2, in to, Color.red);
        public static void DrawBezier(int id, in Vector3 from, in Vector3 c1, in Vector3 c2, in Vector3 to, in Color color)
        {
            const int segments = 16;
            var part = (1f / segments);
            for (var i = 1; i < segments; ++i)
            {
                var idx = (1 << 20) + 1000 * id + i;
                var a = fun.bezierV3(in from, in c1, in c2, in to, part * (i - 1));
                var b = fun.bezierV3(in from, in c1, in c2, in to, part * i);
                DrawLine(idx, in a, in b, in color);
            }
        }

        public static void DrawLine(int id, in Vector3 from, in Vector3 to) { DrawLine(id, in from, in to, Color.red); }
        public static void DrawLine(int id, in Vector3 from, in Vector3 to, in Color color)
        {
            LineDef lineDef;
            if (!_lineById.TryGetValue(id, out lineDef))
            {
                lineDef = new LineDef { From = from, To = to, Color = color  };
                _lineById[id] = lineDef;
                
                _drawer.Line(t => lineDef);
                if (_drawAni == null)
                {
                    GlobalFactory.Default.Get<IMessenger>()
                        .Invoke(new PlayAni {Ani = _drawAni = new EndlessDrawFuncAni().Set(a => { _drawer.Draw(); })});
                }
            }
            lineDef.From = from;
            lineDef.To = to;
            lineDef.Color = color;

//            RenderedLine line;
//            if(!_lineById.TryGetValue(id, out line))
//            {
//                _lineById[id] = line = new RenderedLine();
//            }
//            line.DrawLine(from, to, color);
        }
        public static void DrawAxis(int id, Vector3 point, Vector3 axis) { DrawAxis(id,point,axis,Color.red); }
        public static void DrawAxis(int id, Transform t, Vector3 axis) { DrawAxis(id,t,axis,Color.red); }
        public static void DrawAxis(int id, Transform t, Vector3 axis, Color color)
        {
            DrawLine(id, t.position, t.position + axis, color);
        }
        public static void DrawAxis(int id, Vector3 point, Vector3 axis, Color color)
        {
            DrawLine(id, point, point + axis, color);
        }

        public static void DrawOrient(int id, Vector3 pos, Vector3 fw, Vector3 up, double len = 0.2)
        {
            DrawLine((1 << 20) + 1000*id, pos, pos + fw*((float)len), Color.blue);
            DrawLine((1 << 20) + 1000*id+1, pos, pos + up*((float)len), Color.green);
            DrawLine((1 << 20) + 1000*id+2, pos, pos + fun.vector.GetNormal(up,fw)*((float)len), Color.red);
        }
        public static void DrawOrient(int id, Transform t, double len = 0.2)
        {
            DrawLine((1 << 20) + 1000*id, t.position, t.position + t.forward*((float)len), Color.blue);
            DrawLine((1 << 20) + 1000*id+1, t.position, t.position + t.up*((float)len), Color.green);
            DrawLine((1 << 20) + 1000*id+2, t.position, t.position + t.right*((float)len), Color.red);
        }
        public static void DrawOrient(int id, Vector3 pos, Quaternion rot, double len = 0.2)
        {
            var fw = rot*Vector3.forward;
            var up = rot*Vector3.up;
            DrawLine((1 << 20) + 1000*id, pos, pos + fw*((float)len), Color.blue);
            DrawLine((1 << 20) + 1000*id+1, pos, pos + up*((float)len), Color.green);
            DrawLine((1 << 20) + 1000*id+2, pos, pos + fun.vector.GetNormal(up,fw)*((float)len), Color.red);
        }
        public static void DrawMultiColorChain(int id, params Vector3[] points)
        {
            for (var i = 1; i < points.Length; ++i)
            {
                DrawLine((1 << 22) + 1000 * id + i + points.GetHashCode(), points[i - 1], points[i], GetColor(i));
            }
        }
        public static void DrawPointersChain(int id, params Vector3[] points)
        {
            for (var i = 0; i < points.Length; ++i)
            {
                var cid = (1 << 23) + 1000 * id + i + points.GetHashCode();
                var p = points[i];
                var c = GetColor(i);
                DrawLineTo(cid, p, c);
            }
        }
        public static void DrawAltPointersChain(int id, params Vector3[] points)
        {
            for (var i = 0; i < points.Length; ++i)
            {
                var cid = (1 << 24) + 1000 * id + i + points.GetHashCode();
                var p = points[i];
                var c = GetColor(i);
                DrawAltLineTo(cid, p, c);
            }
        }
        public static void DrawChain(int id, Color color, params Vector3[] points)
        {
            for (var i = 1; i < points.Length; ++i)
            {
                DrawLine((1 << 21) + 1000*id+i, points[i-1], points[i], color);
            }
        }
        public static void DrawChainMultiColor(int id, params Vector3[] points)
        {
            for (var i = 1; i < points.Length; ++i)
            {
                DrawLine((1 << 21) + 1000 * id + i, points[i - 1], points[i], dbg.GetColor(i));
            }
        }
        public static void DrawChain(int id, Color color, IEnumerable<Vector3> points)
        {
            var i = 0;
            foreach (var p in points.Pair())
            {
                DrawLine((1 << 21) + 1000 * id + (++i), p.Item1, p.Item2, color);
            }
        }
        public static void DrawCube(int id, Vector3 cubeCenter, Vector3 forward, Vector3 up, Vector3 scale) { DrawCube(id,Color.red,cubeCenter,forward,up,scale); }
        public static void DrawCube(int id, Color color, Vector3 cubeCenter, Vector3 forward, Vector3 up, Vector3 scale)
        {
            Vector3 right;
            fun.vector.GetNormal(in up, in forward, out right);

            right = right*(scale.x/2f);
            forward = forward.normalized*(scale.z/2f);
            up = up.normalized*(scale.y/2f);
            var c = cubeCenter;
            id = (1 << 22) + 1000*id;
            DrawLine(id+ 0, c-right-forward+up, c+right-forward+up, color);
            DrawLine(id+ 1, c+right-forward+up, c+right-forward-up, color);
            DrawLine(id+ 2, c+right-forward-up, c-right-forward-up, color);
            DrawLine(id+ 3, c-right-forward-up, c-right-forward+up, color);
            DrawLine(id+ 4, c-right+forward+up, c+right+forward+up, color);
            DrawLine(id+ 5, c+right+forward+up, c+right+forward-up, color);
            DrawLine(id+ 6, c+right+forward-up, c-right+forward-up, color);
            DrawLine(id+ 7, c-right+forward-up, c-right+forward+up, color);
            DrawLine(id+ 8, c-right-forward+up, c-right+forward+up, color);
            DrawLine(id+ 9, c+right-forward+up, c+right+forward+up, color);
            DrawLine(id+10, c+right-forward-up, c+right+forward-up, color);
            DrawLine(id+11, c-right-forward-up, c-right+forward-up, color);
        }
        public static void DrawCircle(int id, Vector3 circleCenter, Vector3 up, double radius) { DrawCircle(id,Color.red,circleCenter,up,radius);  }
        public static void DrawCircle(int id, Color color, Vector3 circleCenter, Vector3 up, double radius)
        {
            const int numberSteps = 32;
            const int numberStepsPlusOne = numberSteps+1;
            id = (1 << 23) + 1000*id;
            Vector3 normX,normY;
            fun.vector.ComputeRandomXYAxesForPlane(in up, out normX, out normY);
            var original = circleCenter + normX*(float)radius;
            var curr = original;
            var prev = original;
            var degPerStep = 360/(double)numberSteps;
            for(var i = 0; i < numberStepsPlusOne; ++i)
            {
                if(i > 0)
                {
                    curr = original.RotateAbout(in circleCenter, in up, degPerStep*i);
                    DrawLine(id+i, curr, prev, color);
                }
                prev = curr;
            }
        }
        public static void DrawSphere(int id, Vector3 circleCenter,  double radius) { DrawSphere(id,Color.red,circleCenter,radius); }
        public static void DrawSphere(int id, Color color, Vector3 circleCenter,  double radius)
        {
            const int numberSteps = 32;
            
            id = (1 << 24) + 1000*id;
            var up = v3.up;
            Vector3 normX,normY;
            fun.vector.ComputeRandomXYAxesForPlane(in up, out normX, out normY);
            var degPerStep = 360/(double)numberSteps;
            const int numberStepsPlusOne = numberSteps+1;
            var original = circleCenter + normX*(float)radius;
            var curr = original;
            var prev = original;
            
            for (var i = 0; i < numberStepsPlusOne; ++i)
            {
                if(i > 0)
                {
                    curr = original.RotateAbout(in circleCenter, in up, degPerStep*i);
                    DrawLine(id+i, curr, prev, color);
                }
                prev = curr;
            }
            original = circleCenter + up*(float)radius;
            curr = original;
            for(var i = 0; i < numberStepsPlusOne; ++i)
            {
                if(i > 0)
                {
                    curr = original.RotateAbout(in circleCenter, in normY, degPerStep*i);
                    DrawLine(id+i+numberStepsPlusOne, curr, prev, color);
                }
                prev = curr;
            }
            original = circleCenter + up*(float)radius;
            curr = original;
            for(var i = 0; i < numberStepsPlusOne; ++i)
            {
                if(i > 0)
                {
                    curr = original.RotateAbout(in circleCenter, in normX, degPerStep*i);
                    DrawLine(id+i+numberStepsPlusOne+numberStepsPlusOne, curr, prev, color);
                }
                prev = curr;
            }
        }
        public static void DrawLineTo(int id, Vector3 to) { DrawLineTo(id, to, Color.red); }
        public static void DrawLineTo(int id, Vector3 to, Color color)
        {
            DrawLine(id, new Vector3(1111, 1111, 1111), to, color);
        }
        public static void DrawAltLineTo(int id, Vector3 to) { DrawAltLineTo(id, to, Color.red); }
        public static void DrawAltLineTo(int id, Vector3 to, Color color)
        {
            DrawLine(id, new Vector3(-1111, 1111, -1111), to, color);
        }
        public static void DrawTri(int id, Vector3 triangleCenter) { DrawTri(id, triangleCenter, Color.red); }
        public static void DrawTri(int id, Vector3 triangleCenter, Color color)
        {
            const float pad = 0.015f;
            DrawLine((1 << 22) + 1000*id, triangleCenter+v3.unitX*pad+v3.unitY*-pad, triangleCenter+v3.unitX*-pad+v3.unitY*-pad, color);
            DrawLine((1 << 22) + 1000*id+1, triangleCenter+v3.unitX*-pad+v3.unitY*-pad, triangleCenter+v3.unitY*pad, color);
            DrawLine((1 << 22) + 1000*id+2, triangleCenter+v3.unitY*pad, triangleCenter+v3.unitX*pad+v3.unitY*-pad, color);
        }

        private static Transform _plane1;
        private static Transform _plane2;
        public static Transform DrawPlane1(Vector3 normal, Vector3 point, double side = 2)
        {
            _plane1 = _plane1 ?? fun.meshes.CreateTwoSidedSquarePlane(new DtSquarePlane {length = side, width = side})
                .SetStandardShaderTransparentColor(1,1,0,0.5).transform;
            _plane1.position = point;
            Vector3 xNorm, yNorm;
            fun.vector.ComputeRandomXYAxesForPlane(in normal, out xNorm, out yNorm);
            _plane1.rotation = Quaternion.LookRotation(normal, yNorm);
            return _plane1;
        }
        public static Transform DrawPlane2(Vector3 normal, Vector3 point, double side = 2)
        {
            _plane2 = _plane2 ?? fun.meshes.CreateTwoSidedSquarePlane(new DtSquarePlane {length = side, width = side})
                .SetStandardShaderTransparentColor(0,1,1,0.5).transform;
            _plane2.position = point;
            Vector3 xNorm, yNorm;
            fun.vector.ComputeRandomXYAxesForPlane(in normal, out xNorm, out yNorm);
            _plane2.rotation = Quaternion.LookRotation(normal, yNorm);
            return _plane2;
        }
        public static void ClearPlane1()
        {
            if(_plane1 == null) return;
             UnityEngine.Object.Destroy(_plane1.gameObject);
            _plane1 = null;
        }
        public static void ClearPlane2()
        {
            if(_plane2 == null) return;
            UnityEngine.Object.Destroy(_plane2.gameObject);
            _plane2 = null;
        }

        public static void ClearLine(int id)
        {
            //RenderedLine line;
            LineDef line;
            if (_lineById.TryGetValue(id, out line))
            {
                //line.Dispose();
                _lineById.Remove(id);
                _drawer.RemoveLine(line);
            }
        }
        public static void ClearAllLines()
        {
            foreach (LineDef lineDef in _lineById.Values)
            {
                _drawer.RemoveLine(lineDef);
            }
            _lineById.Clear();
        }

        public static void ClearNumLines(int id, int num)
        {
            ClearLine(id);
            if (num > 0) for(var i = 1; i <= num; ++i) ClearLine(id+i);
        }
        public static void ClearLines(int id1,int id2) { ClearLine(id1);ClearLine(id2); }
        public static void ClearLines(int id1,int id2,int id3) { ClearLine(id1);ClearLine(id2);ClearLine(id3); }
        public static void ClearLines(int id1,int id2,int id3,int id4) { ClearLine(id1);ClearLine(id2);ClearLine(id3);ClearLine(id4); }
        public static void ClearLines(int id1,int id2,int id3,int id4,int id5) { ClearLine(id1);ClearLine(id2);ClearLine(id3);ClearLine(id4);ClearLine(id5); }
        public static void ClearLines(int id1,int id2,int id3,int id4,int id5,int id6) { ClearLine(id1);ClearLine(id2);ClearLine(id3);ClearLine(id4);ClearLine(id5);ClearLine(id6); }
        public static void XDrawLine(int id, Vector3 from, Vector3 to, Color color){ if(isXact) { DrawLine(id,from,to,color); } }
        public static void XDrawAxis(int id, Vector3 from, Vector3 dir, Color color) { if (isXact) { DrawAxis(id, from, dir, color); } }
        public static void XDrawOrient(int id, Transform t, double length) { if (isXact) { DrawOrient(id, t, length); } }
        public static void XDrawLineTo(int id, Vector3 to, Color color){ if(isXact) { DrawLineTo(id,to,color); } }
        public static void XDrawTri(int baseId, Vector3 center, Color color){ if(isXact) { DrawTri(baseId,center,color); } }
        public static void XClearLine(int id){ if(isXact) { ClearLine(id); } }
        public static void XClearLines(int id1, int id2){ if(isXact) { ClearLines(id1,id2); } }
        public static void XClearLines(int id1, int id2, int id3){ if(isXact) { ClearLines(id1,id2,id3); } }
        public static void XClearNumLines(int id, int num){ if(isXact) { ClearNumLines(id, num); } }

        private static int _newRedTintColoId;
        private static readonly Color[] _redTintColors = new Color[] {new Color(1,0,0),new Color(0.5f,0,0),new Color(0.8f,0.2f,0)};
        public static Color NewRedTintColor { get { return _redTintColors[(++_newRedTintColoId)%_redTintColors.Length]; } }
        private static int _newBlueTintColoId;
        private static readonly Color[] _blueTintColors = new Color[] {new Color(0,0,1),new Color(0,0,0.5f),new Color(0,0.2f,0.8f)};
        public static Color NewBlueTintColor { get { return _blueTintColors[(++_newBlueTintColoId)%_blueTintColors.Length]; } }
        private static int _newId;
        
        public static int NewId { get { return ++_newId; } }
        public static class ani
        {
            private static readonly IGlobalFactory _create = GlobalFactory.Default; 
            private static readonly IMessenger _messenger = _create.Get<IMessenger>();

            public static TAni Play<TAni>(TAni ani) where TAni : IAnimation
            {
                _messenger.Invoke(new PlayAni {Ani = ani});
                return ani;
            }

            public static TAni Play<TAni>() where TAni : IAnimation
            {
                return Play(_create.Get<TAni>());
            }

            public static FuncAni StartFuncAni(double seconds, Action<float> action)
            {
                return 
                    Play(_create
                        .Get<FuncAni>()
                            .Set(seconds, action));
            }

            public static EndlessFuncAni StartEndlessFuncAni(Action<EndlessFuncAni> action)
            {
                return
                    Play(_create
                    .Get<EndlessFuncAni>()
                        .Set(action));
            }
            public static EndlessFuncAni OnKeyUp(KeyCode code, Action action)
            {
                return
                    Play(_create
                    .Get<EndlessFuncAni>()
                        .Set(ani =>
                        {
                            if (Input.GetKeyUp(code)) action();
                        }));
            }
            public static FramesPerSecondAni StartFramesPerSecondAni(Action<float> setFpsUi)
            {
                return
                    Play(_create
                    .Get<FramesPerSecondAni>()
                        .Set(setFpsUi));
            }

            public static T StartAni<T>() where T : IAnimation
            {
                return
                    Play(_create
                    .Get<T>());
            }

            public static WaitAni WaitSeconds(double seconds)
            {
                return
                    Play(_create
                    .Get<WaitAni>()
                        .Set(seconds));
            }

            public static T WaitSecondsThenStart<T>(double seconds) where T : IAnimation
            {
                return
                    Play(_create
                    .Get<WaitAni>().Set(seconds))
                        .ThenStart<T>();
            }
        }


        public static Transform CreateFakeObj(string name, double posX, double posY, double posZ, double rotX, double rotY, double rotZ)
        {
            var go = new GameObject(name);
            go.transform.position = new Vector3((float)posX, (float)posY, (float)posZ);
            go.transform.rotation = Quaternion.Euler(new Vector3((float)rotX, (float)rotY, (float)rotZ));
            return go.transform;
        }

        static bool _toggleBln;
        public static bool NextBln() => _toggleBln = !_toggleBln;
    }

}