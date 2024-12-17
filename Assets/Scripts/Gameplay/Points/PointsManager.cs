using System;
using EventBus.Subscribers.Gameplay;
using UI.Controllers.PointsUIController;
using UnityEngine;
using Utils.EventBus.Subscribers.Sprint;
using Zenject;

namespace Gameplay.Points
{
    public class PointsManager : IPointsObtainedSubscriber, ISprintEndedSubscriber, ISprintStartedSubscriber, IDisposable
    {
        private PointsUIController _pointsUIController;

        public static int CurrentPoints { get; private set; }

        private bool _canObtainPoints; 

        private void AddPoints(int points)
        {
            if (CurrentPoints + points < 0)
            {
                Debug.LogError("Wrong points addition. must be greater than zero");
                return;
            }

            CurrentPoints += points;

            _pointsUIController.UpdatePointsText(CurrentPoints);
        }
        
        [Inject]
        public void Initialize(PointsUIController pointsUIController)
        {
            EventBus.EventBus.SubscribeToEvent<IPointsObtainedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<ISprintEndedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<ISprintStartedSubscriber>(this);
            _pointsUIController = pointsUIController;
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IPointsObtainedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISprintEndedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<ISprintStartedSubscriber>(this);
            _pointsUIController?.Dispose();
        }

        public void HandlePointsObtained(int points)
        {
            if (!_canObtainPoints)
                return;
            
            AddPoints(points);
        }

        public void HandleSprintEnding()
        {
            _canObtainPoints = false;
            AddPoints(-CurrentPoints);
        }

        public void HandleSprintStarting()
        {
            _canObtainPoints = true;
        }
    }
}