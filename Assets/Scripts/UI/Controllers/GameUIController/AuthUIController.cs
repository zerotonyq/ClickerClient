using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Auth;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI.Auth;
using TMPro;
using UI.Controllers.GameUIController.Config;
using UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils.DI;
using Zenject;

namespace UI.Controllers.GameUIController
{
    public class AuthUIController :
        IInitializableWithConfigAndParent<GameUIConfig, Transform>,
        IDisposable,
        IUsernameObtainSubscriber,
        IAuthUIRequestedSubscriber
    {
        private Canvas _canvas;

        private AuthWindow _authWindow;

        private TextMeshProUGUI _usernameText;

        private string _currentUsername = "";
        private bool _needToSignIn;

        [Inject]
        public async Task Initialize(GameUIConfig config, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IAuthUIRequestedSubscriber>(this);
            
            await InstantiateObjects(config, parent);
            _canvas.worldCamera = Camera.main;
        }

        private async Task InstantiateObjects(GameUIConfig config, Transform parent)
        {
            _canvas = (await Addressables.InstantiateAsync(config.canvasReference, parent)).GetComponent<Canvas>();

            AsyncOperationHandle<GameObject> authWindowOpHandle =
                Addressables.InstantiateAsync(config.authWindow, _canvas.transform);
            authWindowOpHandle.Completed += opHandle =>
            {
                _authWindow = opHandle.Result.GetComponent<AuthWindow>();
                _authWindow.Canvas.worldCamera = Camera.main;
                ToggleAuthWindow(_needToSignIn);
            };

            var usernameTextHandle = Addressables.InstantiateAsync(config.usernameText, _canvas.transform);

            usernameTextHandle.Completed += opHandle =>
            {
                _usernameText = opHandle.Result.GetComponent<TextMeshProUGUI>();
                _usernameText.text = _currentUsername;
            };
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IAuthUIRequestedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
        }

        private void ToggleAuthWindow(bool i) => _authWindow?.gameObject.SetActive(i);

        public void Handle(string username)
        {
            _currentUsername = username;
            TryChangeUsernameText(username);
        }

        private void TryChangeUsernameText(string username)
        {
            if (_usernameText != null) _usernameText.text = username;
        }


        public void Handle()
        {
            _needToSignIn = true;
            ToggleAuthWindow(_needToSignIn);
        }
    }
}