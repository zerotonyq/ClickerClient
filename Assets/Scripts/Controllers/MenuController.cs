using System;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI;
using EventBus.Subscribers.SceneManagement;
using SceneManagement.Enum;
using Zenject;

namespace Controllers
{
    public class MenuController : IStartGameButtonPressedSubscriber, IDisposable
    {
        [Inject]
        public void Construct() => EventBus.EventBus.SubscribeToEvent<IStartGameButtonPressedSubscriber>(this);

        public void Handle() =>
            EventBus.EventBus.RaiseEvent<ISceneSwitchRequestedSubscriber>(sub => sub.Handle(GameScene.Game));

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<IStartGameButtonPressedSubscriber>(this);
    }
}