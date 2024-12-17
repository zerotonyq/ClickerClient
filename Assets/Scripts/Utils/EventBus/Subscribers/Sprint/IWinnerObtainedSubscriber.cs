using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Sprint
{
    public interface IWinnerObtainedSubscriber : IGlobalSubscriber
    {
        void HandleWinnerObtain(int winnerId);
    }
}