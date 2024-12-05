using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Auth;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI;
using EventBus.Subscribers.MenuUI.Auth;
using EventBus.Subscribers.Roles;
using TMPro;
using UI.Controllers.MenuUIController.Config;
using UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils.DI;
using Zenject;

namespace UI.Controllers.MenuUIController
{
    public class GameUIController :
        IInitializableWithConfigAndParent<GameUIConfig, Transform>,
        IDisposable,
        IUsernameObtainSubscriber,
        IAuthUIRequestedSubscriber,
        IAuthSuccessfullySubscriber
    {
        private SimpleAnimatedButton _startButton;
        
        private Canvas _canvas;
        
        private AuthWindow _authWindow;

        private TextMeshProUGUI _usernameText;
        
        private string _currentUsername = "";
        private bool _needToSignIn;

        [Inject]
        public async Task Initialize(GameUIConfig config, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IAuthUIRequestedSubscriber>(this);
           

            await InstantiateObjects(config, parent);
            _canvas.worldCamera = Camera.main;

            Bind();
        }

        private async Task InstantiateObjects(GameUIConfig config, Transform parent)
        {
            _canvas = (await Addressables.InstantiateAsync(config.canvasReference, parent)).GetComponent<Canvas>();

            _startButton = (await Addressables.InstantiateAsync(config.startButtonReference, _canvas.transform))
                .GetComponent<SimpleAnimatedButton>();
            _startButton.gameObject.SetActive(false);


            AsyncOperationHandle<GameObject> authWindowOpHandle = Addressables.InstantiateAsync(config.authWindow, _canvas.transform);
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

        private void Bind() =>
            _startButton.OnClick.AddListener(() =>
                EventBus.EventBus.RaiseEvent<IStartGameButtonPressedSubscriber>(sub => sub.Handle()));

        public void Dispose()
        {
            _startButton.OnClick.RemoveAllListeners();
            EventBus.EventBus.UnsubscribeFromEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IAuthUIRequestedSubscriber>(this);
            
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

        public void Handle(AuthResult result) => _startButton.gameObject.SetActive(result.Success);
        
        public void HandleAdminRoleObtained()
        {
            
        }
    }
}