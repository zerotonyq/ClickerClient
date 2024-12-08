using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Notifications
{
    public interface INotificationRequestSubscriber : IGlobalSubscriber
    {
        Task HandleNotificationRequest(string message);
    }
}