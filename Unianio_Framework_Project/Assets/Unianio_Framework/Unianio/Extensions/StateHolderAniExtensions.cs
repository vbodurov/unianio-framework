using Unianio.Animations.Common;

namespace Unianio.Extensions
{
    public static class StateHolderAniExtensions
    {
        public static T SetState<T>(this T ani, object state) where T : StateHolderAni
        {
            ani.State = state;
            return ani;
        }
    }
}