using Unianio.Animations;
using Unianio.Events;
using Unianio.Services;
using Unianio.Static;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio
{
    public class UnianioSetup : MonoBehaviour
    {

        SceneHolder _aniHolder;
        
        void Start()
        {
            var factory = GlobalFactory.Default;
            _aniHolder =
                factory
                    .Get<SceneHolder>()
                    .Initialize();

            Messenger.Subscribe<CommandStartCoroutine>(this, OnCommandStartCoroutine);
        }
        void OnCommandStartCoroutine(CommandStartCoroutine e) 
            => StartCoroutine(e.Enumerator);

        void FixedUpdate()
        {
            _aniHolder.FixedUpdate();
        }
        void Update()
        {
            fun.frame();

            _aniHolder.Update();

        }

        void LateUpdate()
        {
            _aniHolder.LateUpdate();
        }
        

#if UNITY_EDITOR
        void OnDrawGizmos() 
            => _aniHolder?.Draw();
#endif
        void OnRenderObject() 
            => _aniHolder?.Draw();
        
    }
}