using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface ISignUpRequestedSubscriber : IGlobalSubscriber
    {
        void Handle(string username, string password);
    }
}