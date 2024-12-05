using EventBus.Subscribers.Base;

namespace EventBus.Auth
{
    public interface IAuthUIRequestedSubscriber : IGlobalSubscriber
    {
        void Handle();
    }
}