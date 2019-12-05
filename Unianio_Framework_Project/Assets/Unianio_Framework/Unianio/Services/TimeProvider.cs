using UnityEditor;
using UnityEngine;

namespace Unianio.Services
{
    public interface ITimeProvider
    {
        float time { get; }
        bool IsPaused { get; }
    }
    // define dependency in constructor as
    // [NamedDependency(uniqueRP.TrackTime)]IPausableTimeProvider pausableTime
    public interface IPausableTimeProvider : ITimeProvider
    {
        float Delay { get; }
        bool TryPause();
        bool TryStart();
        void TogglePause();
    }

    public sealed class TimeProvider : ITimeProvider
    {
        public static readonly ITimeProvider Default = new TimeProvider();
        float ITimeProvider.time => Time.time;
        bool ITimeProvider.IsPaused => false;
    }
    public sealed class PausableTimeProvider : IPausableTimeProvider
    {
        readonly IPausableTimeProvider _this;
        double _delay, _lastPauseTime;
        bool _isPaused;
        public PausableTimeProvider() => _this = this;
        float ITimeProvider.time => _isPaused ? (float)(_lastPauseTime - _delay) : (float)(Time.time - _delay);
        float IPausableTimeProvider.Delay => (float) _delay;
        bool ITimeProvider.IsPaused => _isPaused;
        bool IPausableTimeProvider.TryPause()
        {
            if (_isPaused) return false;
            _lastPauseTime = Time.time;
            return _isPaused = true;
        }
        bool IPausableTimeProvider.TryStart()
        {
            if (!_isPaused) return false;
            _delay += (Time.time - _lastPauseTime);
            return _isPaused = false;
        }

        void IPausableTimeProvider.TogglePause()
        {
            if (_isPaused) _this.TryStart();
            else _this.TryPause();
        }

    }
}