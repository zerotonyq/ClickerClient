using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface ISignOutObtainedSubscriber : IGlobalSubscriber
    {
        void HandleSignOut();
    }
}