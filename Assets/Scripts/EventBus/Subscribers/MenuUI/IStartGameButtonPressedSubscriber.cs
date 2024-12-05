using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI
{
    public interface IStartGameButtonPressedSubscriber : IGlobalSubscriber
    {
        void Handle();
    }
}