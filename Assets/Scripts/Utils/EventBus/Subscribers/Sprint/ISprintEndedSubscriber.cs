using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Sprint
{
    public interface ISprintEndedSubscriber : IGlobalSubscriber
    {
        void HandleSprintEnding();
    }
}