using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Controllers.NotificationsUIController.Config;
using UI.Elements.Tables.Notifications;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Controllers.NotificationsUIController
{
    public class NotificationsUIController
    {
        private Canvas _canvas;

        private NotificationsTableWindow _notificationsTableWindow;

        [Inject]
        public async UniTaskVoid Initialize(NotificationsControllerUIConfig config, Transform parent) =>
            await InstantiateObjects(config, parent);

        private async Task InstantiateObjects(NotificationsControllerUIConfig config, Transform parent)
        {
            _canvas = (await Addressables.InstantiateAsync(config.canvasReference, parent))
                .GetComponent<Canvas>();

            _notificationsTableWindow =
                (await Addressables.InstantiateAsync(config.notificationsTablePrefabReference, _canvas.transform))
                .GetComponent<NotificationsTableWindow>();
        }
    }
}