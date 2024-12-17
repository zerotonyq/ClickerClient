using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.MenuUI.Auth
{
    public interface IAuthUIRequestedSubscriber : IGlobalSubscriber
    {
        void HandleAuthUIRequest();
    }
}