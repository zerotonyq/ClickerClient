using EventBus.Subscribers.Base;
using SceneManagement.Enum;

namespace EventBus.Subscribers.SceneManagement
{
    public interface ISceneSwitchRequestedSubscriber : IGlobalSubscriber
    {
        void Handle(GameScene gameScene);
    }
}