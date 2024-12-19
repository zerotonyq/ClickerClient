using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using EventBus.Subscribers.Gameplay;
using EventBus.Subscribers.Sprint;
using Gameplay.Lobbies;
using Gameplay.Points;
using UI.Controllers.SprintUIController;
using UnityEngine;
using User;
using Utils;
using Utils.EventBus.Subscribers.Gameplay;
using Utils.EventBus.Subscribers.Lobbies;
using Utils.EventBus.Subscribers.Sprint;
using WebRequests;
using WebRequests.Contracts.Lobbies.SetLobbyById;
using WebRequests.Contracts.Sprint.GetSprintRemainingTimeByLobbyId;
using WebRequests.Contracts.Sprint.GetWinnerByLobbyId;
using WebRequests.Contracts.Sprint.UpdateSprintPointsById;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Gameplay.Sprints
{
    public class SprintManager : 
        IEnterLobbySuccessSubscriber,
        IExitLobbyRequestSubscriber,
        IDisposable
    {
        private Stopwatch _sprintStopwatch = new();
        private Stopwatch _miniGameStopwatch = new();

        private float _currentSprintRemainingTimeSeconds;
        private float _currentMiniGameRemainingTimeSeconds;
        
        private float _initialMiniGameRemainingTimeSeconds;
        private float _initialSprintRemainingTimeSeconds;

        private UpdateBehaviour _sprintUpdateBehaviour;

        private SprintUIController _sprintUIController;

        [Inject]
        public void Initialize(SprintUIController sprintUIController)
        {
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);

            _sprintUIController = sprintUIController;

            _sprintUpdateBehaviour = new GameObject("_updateBehaviour1").AddComponent<UpdateBehaviour>();
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
        }

        public async Task HandleEnterLobby(int lobbyId)
        {
            Debug.Log("Get current sprint remaining time with lobby id: " + lobbyId);
            var response = await WebRequestProvider
                .SendJsonRequest<GetSprintRemainingTimeByLobbyIdRequest, GetSprintRemainingTimeByLobbyIdResponse>(
                    ApiPaths.USERS_SPRINT_GETCURRENTSPRINTREMAININGTIMEBYLOBBYID,
                    new GetSprintRemainingTimeByLobbyIdRequest() { LobbyId = lobbyId });

            if (response == null)
            {
                Debug.LogError("Get current sprint remaining time by lobby id request error");
                return;
            }

            StartTimeCounting(response.RemainingTimeSeconds);
            
            EventBus.EventBus.RaiseEvent<ISprintStartedSubscriber>(sub => sub.HandleSprintStarting());

        }

        private void StartTimeCounting(float remainingTimeSeconds)
        {
            _initialSprintRemainingTimeSeconds = remainingTimeSeconds;
            _initialMiniGameRemainingTimeSeconds = 3f;

            _sprintStopwatch.Start();
            _miniGameStopwatch.Start();

            _sprintUpdateBehaviour.Execute(async () =>
            {
                var updateSprintTime = await UpdateSprintTime();
                var updateMiniGameTime = UpdateMiniGameTime();

                _sprintUIController.UpdateMiniGameRemainingTime(updateMiniGameTime);
                _sprintUIController.UpdateSprintRemainingTime(updateSprintTime);
            });
        }

        private async Task SendResults()
        {
            var response = await WebRequestProvider
                .SendJsonRequest<UpdateSprintPointsByIdRequest, UpdateSprintPointsByIdResponse>(
                    ApiPaths.USERS_UPDATESPRINTPOINTSBYID,
                    new UpdateSprintPointsByIdRequest
                        { UserId = UserDataProvider.Id, Points = PointsManager.CurrentPoints });

            if (response == null) Debug.LogError("Error sending update sprint points by id");
        }
        
        private async Task<int> GetWinnerId()
        {
            var response = await WebRequestProvider
                .SendJsonRequest<GetWinnerByLobbyIdRequest, GetWinnerByLobbyIdResponse>(
                    ApiPaths.USERS_GETWINNERBYLOBBYID,
                    new GetWinnerByLobbyIdRequest
                        { LobbyId = LobbiesManager.CurrentLobbyId});

            if (response == null) Debug.LogError("Error getting winner by lobby id");

            return response.WinnerId;
        }

        private async Task<float> UpdateSprintTime()
        {
            _currentSprintRemainingTimeSeconds =
                (float)(_initialSprintRemainingTimeSeconds - _sprintStopwatch.Elapsed.TotalSeconds);

            if (_currentSprintRemainingTimeSeconds >= 0) return _currentSprintRemainingTimeSeconds;

            await ProcessSprintEnding();

            return _currentSprintRemainingTimeSeconds;
        }

        private async Task ProcessSprintEnding()
        {
            Debug.Log("SPRINT ENDED ");
            
            _sprintStopwatch.Stop();
            _sprintStopwatch.Reset();

            _miniGameStopwatch.Stop();
            _miniGameStopwatch.Reset();

            _currentSprintRemainingTimeSeconds = 0;
            _currentMiniGameRemainingTimeSeconds = 0;
            
            _sprintUIController.UpdateMiniGameRemainingTime(0);
            _sprintUIController.UpdateSprintRemainingTime(0);
            
            _sprintUpdateBehaviour.Stop();

            await SendResults();

            EventBus.EventBus.RaiseEvent<ISprintEndedSubscriber>(sub => sub.HandleSprintEnding());
            
            await Task.Delay(TimeSpan.FromSeconds(3));

            var winnerId = await GetWinnerId();

            EventBus.EventBus.RaiseEvent<IWinnerObtainedSubscriber>(sub => sub.HandleWinnerObtain(winnerId));
            
            await Task.Delay(TimeSpan.FromSeconds(3));

            await HandleEnterLobby(LobbiesManager.CurrentLobbyId);
        }

        private float UpdateMiniGameTime()
        {
            _currentMiniGameRemainingTimeSeconds = (float)(_initialMiniGameRemainingTimeSeconds - _miniGameStopwatch.Elapsed.TotalSeconds);

            if (_currentMiniGameRemainingTimeSeconds > 0) return _currentMiniGameRemainingTimeSeconds;
            
            _miniGameStopwatch.Restart();
            
            _currentSprintRemainingTimeSeconds = 0;
            
            EventBus.EventBus.RaiseEvent<IMiniGameStartSubscriber>(async sub => await sub.HandleMiniGameStart());
            
            return _currentMiniGameRemainingTimeSeconds;

        }

        public async Task HandleExitLobbyRequest()
        {
            await ProcessSprintEnding();
        }
    }
}