using Unianio.Services;

namespace Unianio.Extensions
{
    public static class MessengerExtensions
    {
        public static T InvokeAndReturn<T>(this IMessenger messenger, T e) where T : BaseEvent
        {
            messenger.Invoke(e);
            return e;
        }
    }
}