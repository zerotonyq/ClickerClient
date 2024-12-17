using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.EventBus.Subscribers.Loading;
using Zenject;

namespace Loading
{
    public class LoadingManager : IDisposable
    {
        private int _loadingEntitiesCount;
        private int _currentLoadingEntitiesCount;

        private List<ILoadingEntity> _loadingEntities;

        [Inject]
        public void Initialize(List<ILoadingEntity> loadingEntities)
        {
            _loadingEntities = loadingEntities;

            _loadingEntitiesCount = loadingEntities.Count;

            for (var i = 0; i < _loadingEntitiesCount; i++)
            {
                var name = loadingEntities[i].GetType().Name;

                loadingEntities[i].Loaded += () => IncreaseCount(name);

                EventBus.EventBus.SubscribeToEvent(loadingEntities[i] as IInitialLoadingEndedSubscriber);
            }
        }

        private void IncreaseCount(string name)
        {
            ++_currentLoadingEntitiesCount;

            if (_currentLoadingEntitiesCount < _loadingEntitiesCount) return;

            Debug.Log("All loading entities are successfully loaded all needed resources");

            EventBus.EventBus.RaiseEvent<IInitialLoadingEndedSubscriber>(async sub =>
                await sub.HandleInitialLoadingEnded());

            foreach (var loadingEntity in _loadingEntities)
            {
                EventBus.EventBus.UnsubscribeFromEvent(loadingEntity as IInitialLoadingEndedSubscriber);
            }
        }

        public void Dispose()
        {
            _loadingEntities.Clear();
            _loadingEntities = null;
        }
    }
}