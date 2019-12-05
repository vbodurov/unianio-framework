using System;
using System.Collections.Generic;
using UnityEngine;
using Unianio.Extensions;
using Unianio.Services;
using Unianio.Static;

namespace Unianio
{
    public interface IReadOnlyRange<T>
    {
        T From { get; }
        T To { get; }
    }
    public interface IRange<T>
    {
        T From { get; set; }
        T To { get; set; }
    }
    public class Range<T> : IRange<T>
    {
        private T _from;
        private T _to;

        public Range(T from, T to)
        {
            _from = from;
            _to = to;
        }

        public void Flip()
        {
            var temp = _from;
            _from = _to;
            _to = temp;
        }

        public static Range<T> New()
        {
            return new Range<T>(default(T), default(T));
        }

        public T From {
            get => _from;
            set => _from = value;
        }
        public T To {
            get => _to;
            set => _to = value;
        }
        public Range<T> SetFrom(T from)
        {
            From = from;
            return this;
        }
        public Range<T> SetTo(T to)
        {
            To = to;
            return this;
        }
        public override string ToString() { return "{from:" + From + ",to:" + To + ",type:'Range of " + typeof(T) + "'}"; }
    }

    public struct NumericRange : IRange<float>
    {
        private double _from;
        private double _to;

        public NumericRange(double from, double to)
        {
            _from = from;
            _to = to;
        }
        public void Flip()
        {
            var temp = _from;
            _from = _to;
            _to = temp;
        }
        public float From {
            get => (float)_from;
            set => _from = value;
        }
        public float To {
            get => (float)_to;
            set => _to = value;
        }
        public static float operator *(NumericRange r, double x)
        {
            return r.ValueByProgress(x);
        }
        public static float operator *(NumericRange r, float x)
        {
            return r.ValueByProgress(x);
        }
        public static float operator *(double x, NumericRange r)
        {
            return r.ValueByProgress(x);
        }
        public static float operator *(float x, NumericRange r)
        {
            return r.ValueByProgress(x);
        }
    }
    public static class Rng
    {
        public static NumericRange n(double from, double to)
        {
            return new NumericRange(from, to);
        }
        public static Range<float> f(double from, double to)
        {
            return new Range<float>((float)from, (float)to);
        }
        public static Range<Vector3> v3(Vector3 from, Vector3 to)
        {
            return new Range<Vector3>(from, to);
        }
        public static Range<Vector3> v3(Vector3 from)
        {
            return new Range<Vector3>(from, from);
        }
        public static Range<Vector2> v2(Vector2 from, Vector2 to)
        {
            return new Range<Vector2>(from, to);
        }
        public static Range<Vector2> v2(Vector2 from)
        {
            return new Range<Vector2>(from, from);
        }
        public static Range<Quaternion> q(Quaternion from, Quaternion to)
        {
            return new Range<Quaternion>(from, to);
        }
        public static Range<Quaternion> q(Quaternion from)
        {
            return new Range<Quaternion>(from, from);
        }
        public static Range<Quaternion> localRotationChange(GameObject go, Quaternion change)
        {
            return new Range<Quaternion>(go.transform.localRotation, go.transform.localRotation * change);
        }
        public static Range<Quaternion> localRotationChange(Transform t, Quaternion change)
        {
            return new Range<Quaternion>(t.localRotation, t.localRotation * change);
        }
        public static TimeRange sec(double seconds)
        {
            return new TimeRange().SetTime(seconds);
        }
    }
    public class TimeRange : IRange<float>
    {
        ITimeProvider _timeProvider = TimeProvider.Default;

        float _interval;
        public TimeRange() : this(0, 0) { }
        public TimeRange(double from, double to)
        {
            From = (float)from;
            To = (float)to;
            _interval = To - From;
        }
        public TimeRange ChangeTimeProvider(ITimeProvider tp)
        {
            if (tp != null) _timeProvider = tp;
            return this;
        }
        public float From { get; set; }
        public float To { get; set; }
        public bool IsCyclical { get; set; }
        public float Seconds => To - From;
        public float RemainingSeconds => _timeProvider.time > From ? To - From : Math.Max(To - _timeProvider.time, 0f);
        public TimeRange SetAsCyclical()
        {
            IsCyclical = true;
            return this;
        }
        public TimeRange SetTime(double secondsAfterNow)
        {
            if(secondsAfterNow < 0) secondsAfterNow = 0;
            _interval = (float)secondsAfterNow;
            From = _timeProvider.time;
            To = From + (float)secondsAfterNow;
            return this;
        }
        public TimeRange SetProgress(double currentProgress01)
        {
            var p = currentProgress01.Clamp01().Float();
            From = Time.time - p * _interval;
            To = From + _interval;
            return this;
        }
        public TimeRange SetTimeAndProgress(double secondsAll, double currentProgress01)
        {
            if(secondsAll < 0.001) secondsAll = 0.001;
            var p = currentProgress01.Clamp01().Float();
            _interval = (float)secondsAll;
            From = _timeProvider.time - (p* _interval);
            To = From + _interval;
            return this;
        }
        public float Progress()
        {
            var now = _timeProvider.time;
            if (IsCyclical && To < now)
            {
                var all = To - From;
                if (all.IsZero())
                {
                    return now < From ? 0 : 1;
                }
                var curr = now - From;
                return curr / all;
            }
            return this.Progress(now);
        }
        public bool IsFinished()
        {
            if (IsCyclical) return false; // cyclicals are never finished
            return To <= _timeProvider.time;
        }
        public bool IfIsFinishedReset()
        {
            if (IsCyclical) return false; // cyclicals are never finished
            var finished = To <= _timeProvider.time;
            if (finished)
            {
                Reset();
            }
            return finished;
        }
        public TimeRange Reset()
        {
            SetTime(_interval);
            return this;
        }
        public TimeRange ResizeRemainingTo(double seconds)
        {
            var p = this.Progress();
            var preTime = (seconds*p)/(1 - p);
            From = _timeProvider.time - (float)preTime;
            To = _timeProvider.time + (float)seconds;
            return this;
        }
        public override string ToString() { return "{type:'TimeRange',from:" + From + ",to:" + To + "}"; }

        
    }

    public class TimeTracker
    {
        float _passed;
        float _lastStartTime = -1;

        public float PassedSeconds => _passed + (IsRunning ? Time.time - _lastStartTime : 0);
        public bool IsRunning => _lastStartTime >= 0.0;
        public void Start()
        {
            _lastStartTime = Time.time;
        }
        public void Stop()
        {
            _passed += Time.time - _lastStartTime;
            _lastStartTime = -1;
        }
        public void Reset()
        {
            _lastStartTime = -1;
            _passed = 0;
        }
        public void Toggle(bool starts)
        {
            if(starts) Start();
            else Stop();
        }
        public void Toggle()
        {
            if (!IsRunning) Start();
            else Stop();
        }
    }
    public class InvokerOnce
    {
        public static InvokerOnce New => new InvokerOnce();
        bool _isInvoked = false;
        public bool WhenCall(bool valid, Action action)
        {
            if (!valid) return false;
            if (_isInvoked) return false;
            _isInvoked = true;
            action();
            return true;
        }
    }
    public class SubRange01 : IReadOnlyRange<float>
    {
        readonly float _from;
        readonly float _to;
        readonly float _range;
        bool _isInitialized, _isFinalized;


        public SubRange01(double from, double to)
        {
            _from = (float)Math.Min(from,to);
            _to = (float)Math.Max(from,to);
            _range = _to - _from;
            if(Math.Abs(_range) < 0.00001)
            {
                _to = _from + 0.00002f;
                _range = _to - _from;
            }
        }
        public float From => _from;
        public float To => _to;
        public bool IsInitialized => _isInitialized;
        public bool IsFinalized => _isFinalized;

        public bool IsActive(double x, out float subX01)
        {
            if(x < _from || x > _to)
            {
                subX01 = -1;
                return false;
            }
            if (!_isInitialized) _isInitialized = true;
            subX01 = ((float)x - _from) / _range;
            return true;
        }
        public bool IsActive(double x, out bool isFirstCall, out float subX01)
        {
            if(x < _from || x > _to)
            {
                isFirstCall = false;
                subX01 = -1;
                return false;
            }
            isFirstCall = !_isInitialized;
            if (!_isInitialized) _isInitialized = true;
            subX01 = ((float)x - _from) / _range;
            return true;
        }
        public bool IsActive(double x, out bool isFirstCall, out bool isAfterCall, out float subX01)
        {
            if(x < _from || x > _to)
            {
                if(_isInitialized)
                {
                    isFirstCall = false;
                    if(!_isFinalized)
                    {
                        if(x > _to)
                        {
                            isAfterCall = true;
                            _isFinalized = true;
                        }
                        else isAfterCall = false;
                    }
                    else
                    {
                        isAfterCall = false;
                    }
                }
                else
                {
                    isFirstCall = isAfterCall = false;
                }
                subX01 = -1;
                return false;
            }
            isFirstCall = !_isInitialized;
            isAfterCall = false;
            if (!_isInitialized) _isInitialized = true;
            subX01 = ((float)x - _from) / _range;
            return true;
        }
    }
    public class ProgressionF : Range<float>
    {
        public ProgressionF(float from, float to) : base(from, to) { }
        public ProgressionF With(float x01) { Progress01 = x01; return this; }
        public float Progress01 { get; set; }
        public float Current => fun.lerp(From, To, Progress01);
        public float Progress(float x01) { Progress01 = x01; return Current; }
        public bool IsFinished => Progress01 > 0.999;
    }
    public class ProgressionV3 : Range<Vector3>
    {
        public ProgressionV3(Vector3 from, Vector3 to) : base(from, to) { }
        public ProgressionV3 With(float x01) { Progress01 = x01; return this; }
        public float Progress01 { get; set; }
        public Vector3 Current => fun.lerp(From, To, Progress01);
        public Vector3 Progress(float x) { Progress01 = x; return Current; }
        public bool IsFinished => Progress01 > 0.999;
    }
    public class ProgressionV2 : Range<Vector2>
    {
        public ProgressionV2(Vector2 from, Vector2 to) : base(from, to) { }
        public ProgressionV2 With(float x01) { Progress01 = x01; return this; }
        public float Progress01 { get; set; }
        public Vector2 Current => fun.lerp(From, To, Progress01);
        public Vector2 Progress(float x) { Progress01 = x; return Current; }
        public bool IsFinished => Progress01 > 0.999;
    }
    public class ProgressionQ : Range<Quaternion>
    {
        public ProgressionQ(Quaternion from, Quaternion to) : base(from, to) { }
        public ProgressionQ With(float x01) { Progress01 = x01; return this; }
        public float Progress01 { get; set; }
        public Quaternion Current => fun.slerp(From, To, Progress01);
        public Quaternion Progress(float x) { Progress01 = x; return Current; }
        public bool IsFinished => Progress01 > 0.999;
    }

    public class FuncInTime : IRange<float>
    {
        public static readonly Func<double, double> Linear = x => x;
        readonly TimeRange _time;
        Func<double, double> _func = Linear;
        public static FuncInTime New { get { return new FuncInTime(); } }
        FuncInTime() { _time = new TimeRange(); }
        public FuncInTime As(double secondsFromNow, Func<double, double> func)
        {
            _time.SetTime(secondsFromNow);
            SetFunc(func);
            return this;
        }
        public FuncInTime SetFunc(Func<double, double> func)
        {
            if(func != null) _func = func;
            return this;
        }
        public FuncInTime SetTime(double seconds)
        {
            _time.SetTime(seconds);
            return this;
        }
        public bool IsFinished()
        {
            return _time.IsFinished();
        }
        public FuncInTime SetAsCyclical()
        {
            IsCyclical = true;
            return this;
        }
        public float Progress()
        {
            return (float)_func(_time.Progress());
        }
        public float From { get { return _time.From; } set { _time.From = value; } }
        public float To { get { return _time.To; } set { _time.To = value; } }
        internal bool IsCyclical { get { return _time.IsCyclical; } set { _time.IsCyclical = value; } }
    }
    public static class TimeRangeExtensions
    {
        public static bool IsDefinedAndFinished(this TimeRange tr)
        {
            return tr != null && tr.IsFinished();
        }
    }
    public static class RangeExtensions
    {
        public static IRange<float> EnsureIsWithin(this IRange<float> r, float n)
        {
            if (n < r.From) r.From = n;
            if (n > r.To) r.To = n;
            return r;
        }
        public static IEnumerable<float> EnumerateSteps(this IRange<float> r, int numberSteps)
        {
            if(numberSteps < 1) yield break;
            if (numberSteps == 1)
            {
                yield return r.From;
                yield return r.To;
            }
            else
            {
                var step = (r.To - r.From)/numberSteps;
                var curr = r.From;
                var num = numberSteps - 1;
                yield return curr;
                for (var i = 0; i < num; ++i)
                {
                    curr += step;
                    yield return curr;
                }
                yield return r.To;
            }
        }

        public static float AbsDistance(this IRange<float> r) { return Math.Abs(r.To - r.From); }
        public static double AbsDistance(this IRange<double> r) { return Math.Abs(r.To - r.From); }
        public static float Distance(this IRange<float> r) { return r.To - r.From; }
        public static float Mid(this IRange<float> r) { return r.From + (r.To - r.From) / 2f; }
        public static double Distance(this IRange<double> r) { return r.To - r.From; }
        public static float Distance(this IRange<Vector3> r) { return Vector3.Distance(r.From, r.To); }
        public static int Distance(this IRange<int> r) { return r.To - r.From; }
        public static int AbsDistance(this IRange<int> r) { return Math.Abs(r.To - r.From); }

        public static double ProgressByValue(double from, double to, double value)
        {
            var all = to - from;
            if (all.IsZero())
            {
                return value < from ? 0 : 1;
            }
            var curr = value - from;
            return (curr / all);
        }

        public static double ValueByProgress(double from, double to, double progress, bool clamp)
        {
            if (clamp) progress = progress.Clamp01();
            var all = to - from;
            var part = progress * all;
            return part + from;
        }
        public static Vector2 ValueByProgress(Vector2 from, Vector2 to, double progress, bool clamp)
        {
            if (clamp) progress = progress.Clamp01();
            var t = (float)progress;
            return new Vector2(from.x + ((to.x - from.x) * t), from.y + ((to.y - from.y) * t)); // LERP
        }
        public static Vector3 RotateByProgress(Vector3 from, Vector3 to, double progress, bool clamp)
        {
            if (clamp) progress = progress.Clamp01();
            var t = (float)progress;
            return clamp ? Vector3.Slerp(from, to, t) : Vector3.SlerpUnclamped(from, to, t);
        }
        public static Vector3 ValueByProgress(Vector3 from, Vector3 to, double progress, bool clamp)
        {
            if (clamp) progress = progress.Clamp01();
            var t = (float)progress;
            return new Vector3(from.x + ((to.x - from.x) * t), from.y + ((to.y - from.y) * t), from.z + ((to.z - from.z) * t)); // LERP
        }
        public static Quaternion ValueByProgress(Quaternion from, Quaternion to, double progress, bool clamp)
        {
            if (clamp) progress = progress.Clamp01();
            var t = (float)progress;
            return Quaternion.Slerp(from, to, t);
        }

        public static float ProgressNoClamp(this IRange<float> r, double value)
        {
            return (float)ProgressByValue(r.From, r.To, value);
        }
        public static float Progress(this IRange<float> r, double value)
        {
            return (float)ProgressByValue(r.From, r.To, value).Clamp01();
        }
        public static double Progress(this IRange<double> r, double value)
        {
            return ProgressByValue(r.From, r.To, value).Clamp01();
        }
        public static float Clamp(this IRange<float> r, double value)
        {
            if (value < r.From) return r.From;
            if (value > r.To) return r.To;
            return (float)value;
        }
        public static int ValueByProgress(this IRange<int> r, double progress) { return ValueByProgress(r, progress, true); }
        public static int ValueByProgress(this IRange<int> r, double progress, bool clamp)
        {
            return (int)Math.Round(ValueByProgress(r.From, r.To, progress, clamp));
        }
        public static float ValueByProgress(this IRange<float> r, double progress) { return ValueByProgress(r, progress, true); }
        public static float ValueByProgress(this IRange<float> r, double progress, bool clamp)
        {
            return (float)ValueByProgress(r.From, r.To, progress, clamp);
        }
        public static double ValueByProgress(this IRange<double> r, double progress) { return ValueByProgress(r, progress, true); }
        public static double ValueByProgress(this IRange<double> r, double progress, bool clamp)
        {
            return ValueByProgress(r.From, r.To, progress, clamp);
        }
        public static Quaternion ValueByProgress(this IRange<Quaternion> r, double progress) { return ValueByProgress(r, progress, true); }
        public static Quaternion ValueByProgress(this IRange<Quaternion> r, double progress, bool clamp)
        {
            return ValueByProgress(r.From, r.To, progress, clamp);
        }
        public static Vector3 ValueByProgress(this IRange<Vector3> r, double progress) { return ValueByProgress(r, progress, true); }
        public static Vector3 ValueByProgress(this IRange<Vector3> r, double progress, bool clamp)
        {
            return ValueByProgress(r.From, r.To, progress, clamp);
        }

        public static Vector3 RotateByProgress(this IRange<Vector3> r, double progress) { return RotateByProgress(r, progress, true); }
        public static Vector3 RotateByProgress(this IRange<Vector3> r, double progress, bool clamp)
        {
            return RotateByProgress(r.From, r.To, progress, clamp);
        }
        public static Vector2 ValueByProgress(this IRange<Vector2> r, double progress) { return ValueByProgress(r, progress, true); }
        public static Vector2 ValueByProgress(this IRange<Vector2> r, double progress, bool clamp)
        {
            return ValueByProgress(r.From, r.To, progress, clamp);
        }

        public static bool IsWithin(this IRange<float> r, double n)
        {
            return (r.From >= n && r.To <= n) || (r.To >= n && r.From <= n);
        }
        public static bool IsWithin(this IRange<double> r, double n)
        {
            return (r.From >= n && r.To <= n) || (r.To >= n && r.From <= n);
        }

    }

}
