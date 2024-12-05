using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Gameplay;
using Gameplay.MiniGames.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebRequests;
using WebRequests.Contracts.MiniGames;
using Zenject;

namespace Gameplay.MiniGames
{
    public class MiniGameManager : IMiniGameStartSubscriber, IDisposable
    {
        private readonly Dictionary<string, GameObject> _miniGamesPrefabs = new();

        private MiniGame _currentMiniGame;

        private List<string> _miniGamesNames;

        private int _currentMiniGameIndex;

        [Inject]
        public async UniTaskVoid Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<IMiniGameStartSubscriber>(this);
            await GetMiniGames();
        }

        private int Points { get; set; }

        public void AddPoints(int points)
        {
            Points += points;
        }

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<IMiniGameStartSubscriber>(this);

        public async void HandleMiniGameStart()
        {
            UpdateCurrentMiniGameIndex();

            var miniGameName = _miniGamesNames[_currentMiniGameIndex];

            _currentMiniGame.End();

            Addressables.ReleaseInstance(_currentMiniGame.gameObject);

            _currentMiniGame = (await Addressables.InstantiateAsync(miniGameName)).GetComponent<MiniGame>();

            await _currentMiniGame.Initialize(this);

            _currentMiniGame.Start();
        }

        private void UpdateCurrentMiniGameIndex()
        {
            if (_currentMiniGameIndex >= _miniGamesNames.Count)
                _currentMiniGameIndex = 0;

            ++_currentMiniGameIndex;
        }

        private async Task<List<string>> GetMiniGamesNames()
        {
            var result =
                await WebRequestProvider.SendJsonRequest<GetMiniGamesByIdRequest, GetMiniGamesByIdResponse>(
                    ApiPaths.USERS_GETMINIGAMESBYID,
                    new GetMiniGamesByIdRequest() { Id = 1 });

            if (result == null)
                return null;

            if (!await ValidateAddressables(result.Names))
                return null;

            _miniGamesNames = result.Names;

            return result.Names;
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
    }
}