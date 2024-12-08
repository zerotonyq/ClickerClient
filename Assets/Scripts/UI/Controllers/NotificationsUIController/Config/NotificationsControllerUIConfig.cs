using UI.Controllers.Base.Config;
using UI.Elements.Tables.Base.SimpleTable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace UI.Controllers.NotificationsUIController.Config
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(NotificationsControllerUIConfig),
        fileName = "Default" + nameof(NotificationsControllerUIConfig))]
    public class NotificationsControllerUIConfig : UIConfig
    {
        [FormerlySerializedAs("lobbiesTablePrefabReference")] public AssetReferenceGameObject notificationsTablePrefabReference;
        public SimpleTableConfig TableConfig;
    }
}