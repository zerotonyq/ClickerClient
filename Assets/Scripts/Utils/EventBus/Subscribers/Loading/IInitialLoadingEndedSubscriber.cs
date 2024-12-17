using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Loading
{
    public interface IInitialLoadingEndedSubscriber : IGlobalSubscriber
    {
        Task HandleInitialLoadingEnded();
    }
}