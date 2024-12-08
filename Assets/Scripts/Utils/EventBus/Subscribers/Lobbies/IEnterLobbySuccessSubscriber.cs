using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Lobbies
{
    public interface IEnterLobbySuccessSubscriber : IGlobalSubscriber
    {
        void HandleEnterLobby(int lobbyId);
    }
}