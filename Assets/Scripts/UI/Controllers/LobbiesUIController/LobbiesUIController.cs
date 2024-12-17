using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.GameUI;
using EventBus.Subscribers.Lobbies;
using Loading;
using UI.Base;
using UI.Controllers.LobbiesUIController.CanvasContainer;
using UI.Elements;
using UI.Elements.Table;
using UI.Elements.Tables.Lobbies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.Lobbies;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using Zenject;

namespace UI.Controllers.LobbiesUIController
{
    public class LobbiesUIController :
        UIController,
        ILoadingEntity,
        IAuthSuccessfullySubscriber,
        IAuthUIRequestedSubscriber,
        IInitialLoadingEndedSubscriber,
        IMainScreenRequestSubscriber,
        IEnterLobbySuccessSubscriber,
        IDisposable, IExitLobbyRequestSubscriber
    {
        private LobbiesTableWindow _lobbiesTableWindow;

        private SimpleAnimatedButton _getLobbiesButton;

        public Action Loaded { get; set; }

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IInitialLoadingEndedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IMainScreenRequestSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);

            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            var canvasContainer = Canvas.GetComponent<LobbiesUICanvasContainer>();

            _lobbiesTableWindow = canvasContainer.lobbiesTableWindow;
            _getLobbiesButton = canvasContainer.getLobbiesButton;
            
            _lobbiesTableWindow.Initialize();

            _getLobbiesButton.OnClick.AddListener(() =>
            {
                _getLobbiesButton.gameObject.SetActive(false);
                _lobbiesTableWindow.gameObject.SetActive(true);
                _lobbiesTableWindow.Activate();
            });

            Loaded?.Invoke();
        }

        public Task HandleAuthSuccess(AuthResult result)
        {
            ToggleVisibility(result.Success);
            return Task.CompletedTask;
        }

        public void HandleAuthUIRequest()
        {
            ToggleVisibility(false);
        }
        
        public Task HandleInitialLoadingEnded()
        {
            ToggleVisibility(true);
            return Task.CompletedTask;
        }

        public void HandleMainScreenRequest() => ToggleVisibility(true);

        public Task HandleEnterLobby(int lobbyId)
        {
            ToggleVisibility(false);
            _lobbiesTableWindow.Deactivate();
            return Task.CompletedTask;
        }
        
        public Task HandleExitLobbyRequest()
        {
            ToggleVisibility(true);
            return Task.CompletedTask;
        }
        
        protected override void ToggleVisibility(bool isActive)
        {
            _getLobbiesButton.gameObject.SetActive(isActive);
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IInitialLoadingEndedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IMainScreenRequestSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
        }
    }
}