using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
using Zenject;

namespace UI.Controllers.AdminUIController
{
    public class AdminUIController :
        UIController,
        ILoadingEntity,
        IAdminRoleObtainedSubscriber,
        ISignOutObtainedSubscriber,
        IInitialLoadingEndedSubscriber,
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
            _adminUICanvasContainer.leaguesWindow.gameObject.SetActive(false);
            _adminUICanvasContainer.miniGamesWindow.gameObject.SetActive(false);
            _adminUICanvasContainer.usersWindow.gameObject.SetActive(false);
            _adminUICanvasContainer.lobbiesWindow.gameObject.SetActive(false);
            
            _adminUICanvasContainer.leaguesWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.miniGamesWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.usersWindowOpenButton.gameObject.SetActive(isActive);
            _adminUICanvasContainer.lobbiesWindowOpenButton.gameObject.SetActive(isActive);
        }
        
        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAdminRoleObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISignOutObtainedSubscriber>(this);
        }
    }
}