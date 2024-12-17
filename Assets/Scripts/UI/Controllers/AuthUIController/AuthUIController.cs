using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Loading;
using UI.Base;
using UI.Controllers.AuthUIController.CanvasContainer;
using UI.Elements.Auth;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using Zenject;

namespace UI.Controllers.AuthUIController
{
    public class AuthUIController :
        UIController,
        ILoadingEntity,
        IAuthUIRequestedSubscriber,
        IAuthSuccessfullySubscriber,
        IInitialLoadingEndedSubscriber,
        IDisposable
    {
        private AuthWindow _authWindow;

        private bool _needToSignIn;

        public Action Loaded { get; set; }

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAuthUIRequestedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);

            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            var canvasContainer = Canvas.GetComponent<AuthUICanvasContainer>();

            _authWindow = canvasContainer.AuthWindow;

            Loaded?.Invoke();
        }

        public void HandleAuthUIRequest()
        {
            ToggleVisibility(true);
        }

        public  Task HandleInitialLoadingEnded()
        {
            return Task.CompletedTask;
        }
        
        public Task HandleAuthSuccess(AuthResult result)
        {
            ToggleVisibility(false);
            return Task.CompletedTask;
        }

        protected override void ToggleVisibility(bool isActive)
        {
            _authWindow.gameObject.SetActive(isActive);
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthUIRequestedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
        }
    }
}