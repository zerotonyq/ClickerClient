using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.GameUI
{
    public interface IMainScreenRequestSubscriber : IGlobalSubscriber
    {
        void HandleMainScreenRequest();
    }
}