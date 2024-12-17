using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Loading;
using UI.Base;
using UI.Controllers.NotificationsUIController.CanvasContainer;
using UI.Elements.Tables.Notifications;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using Zenject;

namespace UI.Controllers.NotificationsUIController
{
    public class NotificationsUIController :
        UIController,
        ILoadingEntity,
        IInitialLoadingEndedSubscriber,
        IAuthSuccessfullySubscriber,
        IDisposable
    {
        private NotificationsTableWindow _notificationsTableWindow;

        public Action Loaded { get; set; }

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IInitialLoadingEndedSubscriber>(this);

            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent))
                .GetComponent<Canvas>();

            var canvasContainer = Canvas.GetComponent<NotificationsUICanvasContainer>();

            _notificationsTableWindow = canvasContainer.notificationsTableWindow;
            
            Loaded?.Invoke();
        }


        protected override void ToggleVisibility(bool isActive) =>
            _notificationsTableWindow.gameObject.SetActive(isActive);

        public async Task HandleAuthSuccess(AuthResult result)
        {
            //ToggleVisibility(result.Success);
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IInitialLoadingEndedSubscriber>(this);
        }

        public async Task HandleInitialLoadingEnded() => ToggleVisibility(false);
    }
}