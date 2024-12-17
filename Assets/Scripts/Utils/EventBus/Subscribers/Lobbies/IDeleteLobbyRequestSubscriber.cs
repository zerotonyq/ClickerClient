using System.Threading.Tasks;
using EventBus.Subscribers.Base;
using UI.Elements.Table.Base;

namespace Utils.EventBus.Subscribers.Lobbies
{
    public interface IDeleteLobbyRequestSubscriber<T> : IGlobalSubscriber
    {
        Task HandleDeleteLobbyRequest(T row);
    }
}