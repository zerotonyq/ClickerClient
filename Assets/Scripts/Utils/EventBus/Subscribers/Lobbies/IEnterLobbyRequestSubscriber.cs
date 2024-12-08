using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Lobbies
{
    public interface IEnterLobbyRequestSubscriber : IGlobalSubscriber
    {
        Task HandleEnterLobbyRequest(int id);
    }
}