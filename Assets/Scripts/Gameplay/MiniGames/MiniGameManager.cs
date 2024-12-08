using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Gameplay;
using EventBus.Subscribers.MenuUI;
using EventBus.Subscribers.MenuUI.Auth;
using Gameplay.MiniGames.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebRequests;
using WebRequests.Contracts.MiniGames;
using Zenject;

namespace Gameplay.MiniGames
{
    public class MiniGameManager :
        IAuthSuccessfullySubscriber,
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
            EventBus.EventBus.SubscribeToEvent<IMiniGameStartSubscriber>(this);
        }

        private int Points { get; set; }

        public void AddPoints(int points)
        {
            Points += points;
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IAuthSuccessfullySubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IMiniGameStartSubscriber>(this);
        }

        public async Task HandleMiniGameStart()
        {
            if (_miniGamesNames.Count == 0)
            {
                Debug.Log("there is no mini games");
                return;
            }

            foreach (var na in _miniGamesNames)
            {
                Debug.Log(na);
            }
            if (_currentMiniGame)
            {
                _currentMiniGame.End();

                Addressables.ReleaseInstance(_currentMiniGame.gameObject);
            }

            _currentMiniGame = (await Addressables.InstantiateAsync(_miniGamesNames[_currentMiniGameIndex]))
                .GetComponent<MiniGame>();

            await _currentMiniGame.Initialize(this);

            _currentMiniGame.Start();
            
            UpdateCurrentMiniGameIndex();
        }

        private void UpdateCurrentMiniGameIndex()
        {
            if (_currentMiniGameIndex >= _miniGamesNames.Count)
                _currentMiniGameIndex = 0;

            ++_currentMiniGameIndex;
        }

        private async Task<bool> ValidateAddressables(List<string> names)
        {
            var resourceLocations = await Addressables.LoadResourceLocationsAsync(names);

            return resourceLocations.All(location => names.Contains(location.PrimaryKey));
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

        public async Task HandleAuthSuccess(AuthResult result)
        {
            await GetMiniGames();
        }
    }
}