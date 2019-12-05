using System;
using Unianio.RSG;

namespace Unianio.Animations
{
    public interface IIdHolder
    {
        ulong ID { get; }
    }
    public interface IAnimation : IDisposable, IIdHolder, IInitializable, IUpdatable
    {
        bool IsFinished { get; }
        bool IsForcedToFinish { get; set; }
        bool IsDisposed { get; }
        bool IsInitialized { get; set; }
        bool IsGlDrawing { get; }
        ulong Label { get; set; }
        int Priority { get; set; }
        string UniquenessId { get; set; }
        int PlayerStartedOnFrame { get; }
        IAnimation MarkAsPlayerAction();
        void Draw();
        void Finish();
        void SilentFinish();
        void EnsureUniqueness();
        IAnimation FollowBy();
        void SetFollowing(Func<IAnimation> getAnimation);
        IAnimation ClearAllFollowUpActions();
        void WhenInitCall(Action<IAnimation> onInit);
        void WhenEndsCall(Action<IAnimation> onEnd);
        void SetOnInit(Action<IAnimation> onInit);
        void SetOnEnd(Action<IAnimation> onEnd);
        IPromise<IAnimation> CreatePromise();
        IPromise<IAnimation> Promise { get; }
    }
}