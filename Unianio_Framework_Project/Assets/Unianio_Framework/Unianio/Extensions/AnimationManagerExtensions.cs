using Unianio.Animations;

namespace Unianio.Extensions
{
    public static class AnimationManagerExtensions
    {
        public static AnimationManager AppendAnis(this AnimationManager am, params IAnimation[] anis)
        {
            for (int i = 0; i < anis.Length; i++)
            {
                am.AddAnimation(anis[i]);
            }
            return am;
        }
        public static AnimationManager FinishAllAndClearThem(this AnimationManager am)
        {
            am.FinishAllAnimationsInside();
            am.ClearAll();
            return am;
        }
    }
}