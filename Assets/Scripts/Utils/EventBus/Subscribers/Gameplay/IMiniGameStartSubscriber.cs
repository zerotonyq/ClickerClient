using System.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace Utils.EventBus.Subscribers.Gameplay
{
    public interface IMiniGameStartSubscriber : IGlobalSubscriber
    {
        Task HandleMiniGameStart();
    }
}