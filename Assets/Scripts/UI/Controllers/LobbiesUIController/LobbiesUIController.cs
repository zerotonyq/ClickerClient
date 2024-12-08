using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.MenuUI.Auth;
using UI.Controllers.LobbiesUIController.Config;
using UI.Elements;
using UI.Elements.Table;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Controllers.LobbiesUIController
{
    public class LobbiesUIController :
        IAuthSuccessfullySubscriber,
        IDisposable
    {
        private Canvas _canvas;

        private LobbiesTableWindow _lobbiesTableWindow;

        private SimpleAnimatedButton _getLobbiesButton;
        
        [Inject]
        public async UniTaskVoid Initialize(LobbiesControllerUIConfig config, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);

            await InstantiateObjects(config, parent);
        }

        private async Task InstantiateObjects(LobbiesControllerUIConfig config, Transform parent)
        {
            _canvas = (await Addressables.InstantiateAsync(config.canvasReference, parent))
                .GetComponent<Canvas>();
            
            _lobbiesTableWindow =
                (await Addressables.InstantiateAsync(config.lobbiesTablePrefabReference, _canvas.transform))
                .GetComponent<LobbiesTableWindow>();
            
            _getLobbiesButton =
                (await Addressables.InstantiateAsync(config.openLobbiesWindowButtonReference, _canvas.transform))
                .GetComponent<SimpleAnimatedButton>();


            await _lobbiesTableWindow.Initialize(config.lobbiesSearchWindowConfig);
            _lobbiesTableWindow.gameObject.SetActive(false);
            
            _getLobbiesButton.OnClick.AddListener(() =>
            {
                _getLobbiesButton.gameObject.SetActive(false);
                _lobbiesTableWindow.gameObject.SetActive(true);
                _lobbiesTableWindow.Activate();
            });
        }

        public async Task HandleAuthSuccess(AuthResult result)
        {
            if (!_getLobbiesButton || !_lobbiesTableWindow)
                return;

            _getLobbiesButton.gameObject.SetActive(result.Success);
            _lobbiesTableWindow.gameObject.SetActive(result.Success);
        }

        public void Dispose()
        {
            _getLobbiesButton?.Dispose();
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
        }
    }
}