using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface ISignInRequestedSubscriber : IGlobalSubscriber
    {
        void Handle(string username, string password);
    }
}