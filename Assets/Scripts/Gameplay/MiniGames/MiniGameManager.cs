using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Gameplay;
using EventBus.Subscribers.Lobbies;
using Gameplay.MiniGames.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Gameplay;
using Utils.EventBus.Subscribers.Lobbies;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using WebRequests;
using WebRequests.Contracts.MiniGames;
using Zenject;

namespace Gameplay.MiniGames
{
    public class MiniGameManager :
        IAuthSuccessfullySubscriber,
        IEnterLobbySuccessSubscriber,
        IExitLobbyRequestSubscriber,
        IMiniGameStartSubscriber,
        IDisposable
    {
        private readonly Dictionary<string, GameObject> _miniGamesPrefabs = new();

        private MiniGame _currentMiniGame;

        private List<string> _miniGamesNames = new();

        private int _currentMiniGameIndex;

        [Inject]
        public async UniTaskVoid Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IMiniGameStartSubscriber>(this);
        }

        public void AddPoints(int points)
        {
            EventBus.EventBus.RaiseEvent<IPointsObtainedSubscriber>(sub => sub.HandlePointsObtained(points));
        }

        public async Task HandleEnterLobby(int lobbyId)
        {
            if (_miniGamesNames.Count == 0)
            {
                Debug.Log("there is no mini games");
                return;
            }

            _currentMiniGameIndex = 0;
            await LoadNext();
        }

        private async Task LoadNext()
        {
            TryUnloadCurrent();

            _currentMiniGame = (await Addressables.InstantiateAsync(_miniGamesNames[_currentMiniGameIndex]))
                .GetComponent<MiniGame>();

            await _currentMiniGame.Initialize(this);

            _currentMiniGame.Start();

            UpdateCurrentMiniGameIndex();
        }

        private void TryUnloadCurrent()
        {
            if (!_currentMiniGame)
                return;

            _currentMiniGame.End();

            Addressables.ReleaseInstance(_currentMiniGame.gameObject);
        }

        private void UpdateCurrentMiniGameIndex()
        {
            _currentMiniGameIndex = (_currentMiniGameIndex + 1 >= _miniGamesNames.Count || _miniGamesNames.Count == 1)
                ? 0
                : _currentMiniGameIndex + 1;
        }

        public async Task HandleAuthSuccess(AuthResult result)
        {
            if (!result.Success)
            {
                Debug.Log("Cannot get mini game's names because of auth error");
                return;
            }

            await GetMiniGames();
        }

        private async Task GetMiniGames()
        {
            var names = await GetMiniGamesNames();

            foreach (var name in names)
            {
                if (_miniGamesPrefabs.ContainsKey(name))
                    continue;

                var miniGame = await Addressables.LoadAssetAsync<GameObject>(name);

                _miniGamesPrefabs.TryAdd(name, miniGame);
            }
        }

        private async Task<List<string>> GetMiniGamesNames()
        {
            var result =
                await WebRequestProvider.SendJsonRequest<GetMiniGamesByIdRequest, GetMiniGamesByIdResponse>(
                    ApiPaths.USERS_MINIGAMES_GETMINIGAMEINFOS,
                    new GetMiniGamesByIdRequest());

            if (result == null)
            {
                Debug.Log("error response null get mini games");
                return null;
            }

            var names = result.MiniGameDtos.Select(dto => dto.AddressableName).ToList();

            if (!await ValidateAddressables(names))
            {
                Debug.Log("error addressables validation");
                return null;
            }

            _miniGamesNames = names;

            return _miniGamesNames;
        }

        private async Task<bool> ValidateAddressables(List<string> names)
        {
            var resourceLocations = await Addressables.LoadResourceLocationsAsync(names);

            return resourceLocations.All(location => names.Contains(location.PrimaryKey));
        }

        public Task HandleExitLobbyRequest()
        {
            TryUnloadCurrent();
            return Task.CompletedTask;
        }

        public async Task HandleMiniGameStart()
        {
            await LoadNext();
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbySuccessSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IMiniGameStartSubscriber>(this);
        }
    }
}