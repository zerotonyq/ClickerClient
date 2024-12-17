using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Common
{
    public interface IUsernameObtainSubscriber : IGlobalSubscriber
    {
        void HandleUsernameObtained(string username);
    }
}