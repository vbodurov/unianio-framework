namespace Unianio.Animations
{
    public interface IAnimationHolder : IAnimation
    {
        void AddAnimation(IAnimation a);
        void ClearAll();
    }
}