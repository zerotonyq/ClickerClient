using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.GameUI;
using EventBus.Subscribers.Lobbies;
using EventBus.Subscribers.MenuUI.Auth;
using EventBus.Subscribers.Roles;
using Loading;
using UI.Base;
using UI.Controllers.AdminUIController.CanvasContainer;
using UI.Elements;
using UI.Elements.Tables.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.DI;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.Lobbies;
using Zenject;

namespace UI.Controllers.AdminUIController
{
    public class AdminUIController :
        UIController,
        ILoadingEntity,
        IAdminRoleObtainedSubscriber,
        ISignOutObtainedSubscriber,
        IInitialLoadingEndedSubscriber,
        IEnterLobbySuccessSubscriber,
        IExitLobbyRequestSubscriber,
        IGetLobbiesSubscriber,
        IMainScreenRequestSubscriber,
        IDisposable
    {
        private bool _activated;
        private void Deactivate() => _activated = false;

        public Action Loaded { get; set; }

        private AdminUICanvasContainer _adminUICanvasContainer;

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAdminRoleObtainedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<ISignOutObtainedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IGetLobbiesSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IMainScreenRequestSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);

            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            _adminUICanvasContainer = Canvas.GetComponent<AdminUICanvasContainer>();

            _adminUICanvasContainer.leaguesWindow.Initialize();
            _adminUICanvasContainer.miniGamesWindow.Initialize();
            _adminUICanvasContainer.usersWindow.Initialize();
            _adminUICanvasContainer.lobbiesWindow.Initialize();

            _adminUICanvasContainer.leaguesWindowOpenButton.OnClick.AddListener(() =>
            {
                _adminUICanvasContainer.leaguesWindow.gameObject.SetActive(true);
                _adminUICanvasContainer.leaguesWindow.Activate();
            });

            _adminUICanvasContainer.miniGamesWindowOpenButton.OnClick.AddListener(() =>
            {
                _adminUICanvasContainer.miniGamesWindow.gameObject.SetActive(true);
                _adminUICanvasContainer.miniGamesWindow.Activate();
            });

            _adminUICanvasContainer.usersWindowOpenButton.OnClick.AddListener(() =>
            {
                _adminUICanvasContainer.usersWindow.gameObject.SetActive(true);
                _adminUICanvasContainer.usersWindow.Activate();
            });

            _adminUICanvasContainer.lobbiesWindowOpenButton.OnClick.AddListener(() =>
            {
                _adminUICanvasContainer.lobbiesWindow.gameObject.SetActive(true);
                _adminUICanvasContainer.lobbiesWindow.Activate();
            });

            Loaded?.Invoke();
        }

        public async Task HandleAdminRoleObtained()
        {
            Debug.Log("ADMIN");
            await Activate();
        }

        public void HandleSignOut() => Deactivate();

        private async Task Activate()
        {
            _activated = true;
            ToggleVisibility(true);
        }
        
        public async Task HandleInitialLoadingEnded() => ToggleVisibility(false);

        protected override void ToggleVisibility(bool isActive)
        {
            _adminUICanvasContainer.leaguesWindow.Deactivate();
            _adminUICanvasContainer.miniGamesWindow.Deactivate();
            _adminUICanvasContainer.usersWindow.Deactivate();
            _adminUICanvasContainer.lobbiesWindow.Deactivate();
            
            _adminUICanvasContainer.leaguesWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.miniGamesWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.usersWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.lobbiesWindowOpenButton.gameObject.SetActive(isActive);
        }
        
        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAdminRoleObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISignOutObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IGetLobbiesSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IMainScreenRequestSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
        }

        public Task HandleEnterLobby(int lobbyId)
        {
            ToggleVisibility(false);
            return Task.CompletedTask;
        }

        public Task HandleGetLobbies()
        {
            ToggleVisibility(false);
            Debug.Log("GET LOBBIES");
            return Task.CompletedTask;
        }

        public void HandleMainScreenRequest()
        {
            ToggleVisibility(true);
        }

        public Task HandleExitLobbyRequest()
        {
            ToggleVisibility(true);
            return Task.CompletedTask;
        }
    }
}