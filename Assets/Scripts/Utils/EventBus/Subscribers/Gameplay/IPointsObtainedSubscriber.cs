using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Gameplay
{
    public interface IPointsObtainedSubscriber : IGlobalSubscriber
    {
        void HandlePointsObtained(int points);
    }
}