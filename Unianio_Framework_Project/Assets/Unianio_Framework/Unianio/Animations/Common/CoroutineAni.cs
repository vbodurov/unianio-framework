using System;
using System.Collections;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Animations.Common
{
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
    public class CoroutineAni : AnimationBase
    {
        IEnumerator _enumerator;
        Action<bool> _onComplete;
        double _timeoutSeconds;
        float _startTime;
        public CoroutineAni Set(IEnumerator enumerator, double timeoutSeconds = -1, Action<bool> onComplete = null)
        {
            _enumerator = enumerator;
            _timeoutSeconds = timeoutSeconds;
            _onComplete = onComplete;
            return this;
        }
        public override void Initialize()
        {
            _startTime = Time.time;
        }
        public override void Update()
        {
            if (_timeoutSeconds > 0 && _startTime.ToNow() > _timeoutSeconds)
            {
                _onComplete?.Invoke(false);
                Finish();
                return;
            }

            if (!_enumerator.MoveNext())
            {
                _onComplete?.Invoke(true);
                Finish();
            }
        }
    }
}