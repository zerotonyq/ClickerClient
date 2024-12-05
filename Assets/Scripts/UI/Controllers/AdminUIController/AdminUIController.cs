using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.MenuUI.Auth;
using EventBus.Subscribers.Roles;
using UI.Controllers.AdminUIController.Config;
using UI.Elements;
using UI.Elements.Table.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Controllers.AdminUIController
{
    public class AdminUIController :
        IAdminRoleObtainedSubscriber,
        ISignOutObtainedSubscriber,
        IDisposable
    {
        private bool _activated;

        private void Deactivate() => _activated = false;

        private async Task Activate()
        {
            _activated = true;
            await InstantiateObjects(_config, _parent);
        }

        private Canvas _mainCanvas;
        
        private AdminUIConfig _config;
        private Transform _parent;

        private TableEditorWindow _leaguesWindow;
        private TableEditorWindow _miniGamesWindow;
        private TableEditorWindow _usersWindow;
        private TableEditorWindow _lobbiesWindow;

        private SimpleAnimatedButton _leaguesWindowOpenButton;
        private SimpleAnimatedButton _miniGamesWindowOpenButton;
        private SimpleAnimatedButton _usersWindowOpenButton;
        private SimpleAnimatedButton _lobbiesWindowOpenButton;

        [Inject]
        public void Initialize(AdminUIConfig config, Transform parent)
        {
            _parent = parent;
            _config = config;

            EventBus.EventBus.SubscribeToEvent<IAdminRoleObtainedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<ISignOutObtainedSubscriber>(this);
        }

        private async Task InstantiateObjects(AdminUIConfig config, Transform parent)
        {
            _mainCanvas = (await Addressables.InstantiateAsync(config.canvasReference, parent)).GetComponent<Canvas>();
            
            var canvasTransform = _mainCanvas.transform;

            _leaguesWindow = (await Addressables.InstantiateAsync(config.leaguesWindow, canvasTransform))
                .GetComponent<TableEditorWindow>();
            _usersWindow = (await Addressables.InstantiateAsync(config.usersWindow, canvasTransform))
                .GetComponent<TableEditorWindow>();
            _lobbiesWindow = (await Addressables.InstantiateAsync(config.lobbiesWindow, canvasTransform))
                .GetComponent<TableEditorWindow>();
            _miniGamesWindow = (await Addressables.InstantiateAsync(config.miniGamesWindow, canvasTransform))
                .GetComponent<TableEditorWindow>();

            await _leaguesWindow.Initialize(config.leaguesWindowConfig);
            await _miniGamesWindow.Initialize(config.miniGamesWindowConfig);
            await _usersWindow.Initialize(config.usersWindowConfig);
            await _lobbiesWindow.Initialize(config.lobbiesWindowConfig);

            _leaguesWindowOpenButton =
                (await Addressables.InstantiateAsync(config.leaguesWindowOpenButton, canvasTransform))
                .GetComponent<SimpleAnimatedButton>();
            _miniGamesWindowOpenButton =
                (await Addressables.InstantiateAsync(config.miniGamesWindowOpenButton, canvasTransform))
                .GetComponent<SimpleAnimatedButton>();
            _usersWindowOpenButton =
                (await Addressables.InstantiateAsync(config.usersWindowOpenButton, canvasTransform))
                .GetComponent<SimpleAnimatedButton>();
            _lobbiesWindowOpenButton =
                (await Addressables.InstantiateAsync(config.lobbiesWindowOpenButton, canvasTransform))
                .GetComponent<SimpleAnimatedButton>();

            _leaguesWindowOpenButton.OnClick.AddListener(() =>
            {
                _leaguesWindow.gameObject.SetActive(true);
                _leaguesWindow.Activate();
            });

            _miniGamesWindowOpenButton.OnClick.AddListener(() =>
            {
                _miniGamesWindow.gameObject.SetActive(true);
                _miniGamesWindow.Activate();
            });

            _usersWindowOpenButton.OnClick.AddListener(() =>
            {
                _usersWindow.gameObject.SetActive(true);
                _usersWindow.Activate();
            });

            _lobbiesWindowOpenButton.OnClick.AddListener(() =>
            {
                _lobbiesWindow.gameObject.SetActive(true);
                _lobbiesWindow.Activate();
            });
        }


        public async Task HandleAdminRoleObtained()
        {
            await Activate();
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAdminRoleObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISignOutObtainedSubscriber>(this);
        }

        public void HandleSignOut()
        {
            Deactivate();
        }
    }
}