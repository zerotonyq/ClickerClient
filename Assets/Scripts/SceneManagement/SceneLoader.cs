using System;
using EventBus.Subscribers.SceneManagement;
using SceneManagement.Enum;
using UnityEngine.SceneManagement;
using Zenject;
//<#@ assembly name="$(SolutionDir)/Assets/Packages/Newtonsoft.Json.13.0.3/lib/netstandard2.0/Newtonsoft.Json.dll" #>

namespace SceneManagement
{
    public class SceneLoader : ISceneSwitchRequestedSubscriber, IDisposable, IInitializable
    {
        [Inject]
        public void Construct() => EventBus.EventBus.SubscribeToEvent<ISceneSwitchRequestedSubscriber>(this);

        private bool _isLoadingStarted;

        private void LoadAsync(GameScene gameScene)
        {
            if (gameScene == GameScene.None)
                return;

            if (_isLoadingStarted)
                return;

            var handler = SceneManager.LoadSceneAsync((int)gameScene - 1);
            _isLoadingStarted = true;
            handler.completed += operation => { _isLoadingStarted = false; };
        }

        public void Handle(GameScene gameScene) => LoadAsync(gameScene);

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<ISceneSwitchRequestedSubscriber>(this);

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}