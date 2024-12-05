using System;
using EventBus.Subscribers.GameUI;
using EventBus.Subscribers.SceneManagement;
using SceneManagement.Enum;
using Zenject;

namespace Controllers
{
    public class GameController :
        IExitToMenuButtonPressedSubscriber,
        IDisposable,
        IInitializable
    {

        [Inject]
        public void Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<IExitToMenuButtonPressedSubscriber>(this);
        }


        void IExitToMenuButtonPressedSubscriber.Handle()
        {
            EventBus.EventBus.RaiseEvent<ISceneSwitchRequestedSubscriber>(sub => sub.Handle(GameScene.Menu));
        }


        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<IExitToMenuButtonPressedSubscriber>(this);
    }
}