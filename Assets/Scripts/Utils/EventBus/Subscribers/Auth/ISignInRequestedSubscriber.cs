using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface ISignInRequestedSubscriber : IGlobalSubscriber
    {
        void HandleSignInRequest(string username, string password);
    }
}