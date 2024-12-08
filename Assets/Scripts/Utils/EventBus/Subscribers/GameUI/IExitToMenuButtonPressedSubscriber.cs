using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.GameUI
{
    public interface IExitToMenuButtonPressedSubscriber : IGlobalSubscriber
    {
        void Handle();
    }
}