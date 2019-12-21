using Unianio.Services;

namespace Unianio
{
    public interface ITimeProviderHolder
    {
        void SetTimeProvider(IPausableTimeProvider pausableTime);
    }
}