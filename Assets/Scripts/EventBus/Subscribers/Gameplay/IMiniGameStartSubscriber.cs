using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Gameplay
{
    public interface IMiniGameStartSubscriber : IGlobalSubscriber
    {
        void HandleMiniGameStart();
    }
}