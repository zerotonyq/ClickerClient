using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Lobbies
{
    public interface IExitLobbyRequestSubscriber : IGlobalSubscriber
    {
        Task HandleExitLobbyRequest();
    }
}