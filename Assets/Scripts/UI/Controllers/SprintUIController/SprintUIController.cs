using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Lobbies;
using Loading;
using TMPro;
using UI.Base;
using UI.Controllers.SprintUIController.CanvasContainer;
using UI.Elements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using User;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.Lobbies;
using Zenject;

namespace UI.Controllers.SprintUIController
{
    public class SprintUIController :
        UIController,
        ILoadingEntity,
        IInitialLoadingEndedSubscriber,
        IEnterLobbySuccessSubscriber,
        IDisposable
    {
        public Action Loaded { get; set; }

        private TextMeshProUGUI _sprintRemainingTimeText;
        private TextMeshProUGUI _miniGameRemainingTimeText;
        
        private SimpleAnimatedButton _exitButton;

        [Inject]
        public override async UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent)
        {
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            
            Canvas = (await Addressables.InstantiateAsync(canvasReference, parent)).GetComponent<Canvas>();

            var canvasContainer = Canvas.GetComponent<SprintUICanvasContainer>();

            _sprintRemainingTimeText = canvasContainer.sprintRemainingTimeText;
            _miniGameRemainingTimeText = canvasContainer.miniGameRemainingTimeText;
            _exitButton = canvasContainer.exitButton;
            
            _exitButton.OnClick.AddListener(() =>
            {
                EventBus.EventBus.RaiseEvent<IExitLobbyRequestSubscriber>(async sub => await sub.HandleExitLobbyRequest());
                ToggleVisibility(false);
            });
            
            Loaded?.Invoke();
        }
        
        protected override void ToggleVisibility(bool isActive)
        {
            _sprintRemainingTimeText.gameObject.SetActive(isActive);
            _miniGameRemainingTimeText.gameObject.SetActive(isActive);
            _exitButton.gameObject.SetActive(isActive);
        }
        
        public void UpdateSprintRemainingTime(float time) => _sprintRemainingTimeText.text = time.ToString();
        public void UpdateMiniGameRemainingTime(float time) => _miniGameRemainingTimeText.text = time.ToString();
        
        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
        }

        public async Task HandleInitialLoadingEnded() => ToggleVisibility(false);
        
        public async Task HandleEnterLobby(int lobbyId) => ToggleVisibility(true);
    }
}