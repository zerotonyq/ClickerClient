using System;
using System.Collections.Generic;
using EventBus.Subscribers.Base;
using UnityEngine;

namespace EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<IGlobalSubscriber>> Subscribers = new();

        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : class, IGlobalSubscriber
        {
            if (!Subscribers.TryGetValue(typeof(TSubscriber), out List<IGlobalSubscriber> subscribers))
            {
                Debug.LogWarning("there is no subscribe type to raise event from " + typeof(TSubscriber));
                return;
            }

            foreach (var subscriber in subscribers)
            {
                action?.Invoke(subscriber as TSubscriber);
            }
        }

        public static void SubscribeToEvent<TSubscriber>(TSubscriber subscriber)
            where TSubscriber : class, IGlobalSubscriber
        {
            if (!Subscribers.ContainsKey(typeof(TSubscriber)))
                Subscribers.Add(typeof(TSubscriber), new List<IGlobalSubscriber>());

            var subscribers = Subscribers[typeof(TSubscriber)];

            if (subscribers.Contains(subscriber))
                return;

            subscribers.Add(subscriber);
        }

        public static void UnsubscribeFromEvent<TSubscriber>(TSubscriber subscriber)
            where TSubscriber : class, IGlobalSubscriber
        {
            if (!Subscribers.TryGetValue(typeof(TSubscriber), out List<IGlobalSubscriber> subscribers))
                return;

            if (!subscribers.Contains(subscriber))
                return;

            subscribers.Remove(subscriber);
        }
    }
}