using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Common;
using Loading;
using TMPro;
using UI.Base;
using UI.Controllers.GameTitleUIController.CanvasContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.Lobbies;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using Zenject;

namespace UI.Controllers.GameTitleUIController
{
    public class GameTitleUIController :
        UIController,
        ILoadingEntity,
        IAuthSuccessfullySubscriber,
        IAuthUIRequestedSubscriber,
        IInitialLoadingEndedSubscriber,
        IUsernameObtainSubscriber,
        IEnterLobbySuccessSubscriber,
        IExitLobbyRequestSubscriber,
        IDisposable
    {
        private TextMeshProUGUI _usernameText;

        public Action Loaded { get; set; }

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IAuthUIRequestedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);

            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            // todo get _usernameText from canvas container  
            var canvasContainer = Canvas.GetComponent<GameTitleCanvasContainer>();

            _usernameText = canvasContainer.usernameText;

            Loaded?.Invoke();
        }

        public async Task HandleInitialLoadingEnded() => ToggleVisibility(true);

        protected override void ToggleVisibility(bool isActive) => Canvas.gameObject.SetActive(isActive);

        public async Task HandleAuthSuccess(AuthResult result) => ToggleVisibility(result.Success);

        public void HandleAuthUIRequest() => ToggleVisibility(false);

        public void HandleUsernameObtained(string username) => _usernameText.text = username;

        public Task HandleEnterLobby(int lobbyId)
        {
            ToggleVisibility(false);
            return Task.CompletedTask;
        }

        public Task HandleExitLobbyRequest()
        {
            ToggleVisibility(true);
            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IAuthUIRequestedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IUsernameObtainSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
        }
    }
}