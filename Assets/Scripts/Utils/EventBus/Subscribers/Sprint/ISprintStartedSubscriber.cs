using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Sprint
{
    public interface ISprintStartedSubscriber : IGlobalSubscriber
    {
        void HandleSprintStarting();
    }
}