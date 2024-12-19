using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Users
{
    public interface IDeleteUserRequestSubscriber<T> : IGlobalSubscriber
    {
        Task HandleDeleteUserRequest(T row);
    }
}