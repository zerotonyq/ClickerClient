using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Sprint;
using Loading;
using TMPro;
using UI.Base;
using UI.Controllers.PointsUIController.CanvasContainer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using User;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.Lobbies;
using Utils.EventBus.Subscribers.Sprint;
using Zenject;

namespace UI.Controllers.PointsUIController
{
    public class PointsUIController :
        UIController,
        ILoadingEntity,
        IInitialLoadingEndedSubscriber,
        IEnterLobbySuccessSubscriber,
        IExitLobbyRequestSubscriber,
        IWinnerObtainedSubscriber,
        ISprintStartedSubscriber,
        IDisposable
    {
        private TextMeshProUGUI _pointsText;
        private TextMeshProUGUI _winnerText;
        public Action Loaded { get; set; }

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IWinnerObtainedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<ISprintStartedSubscriber>(this);
            
            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            var canvasContainer = Canvas.GetComponent<PointsUICanvasContainer>();

            _pointsText = canvasContainer.pointsText;
            _winnerText = canvasContainer.winnerText;

            Loaded?.Invoke();
        }

        protected override void ToggleVisibility(bool isActive)
        {
            _pointsText.gameObject.SetActive(isActive);
            _winnerText.gameObject.SetActive(isActive);
        }

        public void UpdatePointsText(int points)
        {
            _pointsText.text = points.ToString();
        }
        
        public Task HandleInitialLoadingEnded()
        {
            ToggleVisibility(false);
            return Task.CompletedTask;
        }

        public Task HandleEnterLobby(int lobbyId)
        {
            ToggleVisibility(true);
            return Task.CompletedTask;
        }

        public Task HandleExitLobbyRequest()
        {
            ToggleVisibility(false);
            return Task.CompletedTask;
        }

        public void HandleWinnerObtain(int winnerId)
        {
            if (winnerId == UserDataProvider.Id)
            {
                _winnerText.text = "Вы победили!";
            }
            else
            {
                _winnerText.text = "Победил пользователь с id " + winnerId;
            }
        }
        
        public void HandleSprintStarting()
        {
            _winnerText.text = "Кто же победит?..";
        }
        
        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IWinnerObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISprintStartedSubscriber>(this);
        }

    }
}