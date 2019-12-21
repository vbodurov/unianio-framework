using Unianio.Services;
using static Unianio.Static.fun;

namespace Unianio.Animations
{
    public sealed class SceneHolder
    {
        readonly AnimationHolder _mainAnimations = new AnimationHolder();
        readonly AnimationHolder _lateAnimations = new AnimationHolder();
        readonly AnimationHolder _earlyAnimations = new AnimationHolder();
        readonly AnimationHolder _fixedUpdateAnimations = new AnimationHolder();
        IUnitySceneRootService _rootService;

        public SceneHolder Initialize()
        {
            subscribe<PlayAni>(e =>
            {
                if (e.QueueIndex == aniQueue.MainQueue)
                    _mainAnimations.AddAnimation(e.Ani);
                else if (e.QueueIndex == aniQueue.LateQueue)
                    _lateAnimations.AddAnimation(e.Ani);
                else if (e.QueueIndex == aniQueue.EarlyQueue)
                    _earlyAnimations.AddAnimation(e.Ani);
                else if (e.QueueIndex == aniQueue.FixedUpdateQueue)
                    _fixedUpdateAnimations.AddAnimation(e.Ani);
            }, this);

            if (UnianioConfig.UseAnimatedHumans) play<IHumanManager>(aniQueue.LateQueue);

            _rootService = get<IUnitySceneRootService>() ?? new VoidUnitySceneRootService();
            _rootService.Initialize();
            

            return this;
        }
        internal void Update()
        {
            _rootService.Update();

            if (_earlyAnimations.NumberAnimations != 0)
            {
                _earlyAnimations.Update();
            }

            _mainAnimations.Update();
        }

        internal void LateUpdate()
        {
            if (_lateAnimations.NumberAnimations != 0)
            {
                _lateAnimations.Update();
            }
        }

        internal void FixedUpdate() // usually before update
        {
            if (_fixedUpdateAnimations.NumberAnimations != 0)
            {
                _fixedUpdateAnimations.Update();
            }
        }
        internal void Draw()
        {
            if(_mainAnimations.NumberAnimationToDraw != 0)
            {
                _mainAnimations.Draw();
            }
        }
    }
}