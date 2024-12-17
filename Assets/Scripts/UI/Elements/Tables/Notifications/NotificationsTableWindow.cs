using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Notifications;
using UI.Elements.Tables.Base.SimpleTable;
using UI.Elements.Tables.Notifications.Rows;
using UnityEngine.AddressableAssets;

namespace UI.Elements.Tables.Notifications
{
    public class NotificationsTableWindow : SimpleTableWindow<string, NotificationTableRow>, 
        INotificationRequestSubscriber, IDisposable
    {
        
        public override async Task Initialize()
        {
            
            EventBus.EventBus.SubscribeToEvent<INotificationRequestSubscriber>(this);
        }

        protected override async Task AddRow(string rowData)
        {
            var lobbyRow = (await Addressables.InstantiateAsync(config.rowPrefab, contentTransform))
                .GetComponent<NotificationTableRow>();

            lobbyRow.Initialize(Rows.Count + 1, rowData);
                
            Rows.Add(lobbyRow);
        }

        protected override Task RemoveRow(int id)
        {
            Destroy(Rows[id]);

            Rows.RemoveAt(id);
            
            return null;
        }

        public async Task HandleNotificationRequest(string message) => await AddRow(message);

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<INotificationRequestSubscriber>(this);
    }
}