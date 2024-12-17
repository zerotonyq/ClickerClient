using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Lobbies
{
    public interface IEnterLobbySuccessSubscriber : IGlobalSubscriber
    {
        Task HandleEnterLobby(int lobbyId);
    }
}