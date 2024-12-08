using UI.Controllers.Base.Config;
using UI.Elements.Table.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Controllers.LobbiesUIController.Config
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(LobbiesControllerUIConfig),
        fileName = "Default" + nameof(LobbiesControllerUIConfig))]
    public class LobbiesControllerUIConfig : UIConfig
    {
        public AssetReferenceGameObject lobbiesTablePrefabReference;
        public AssetReferenceGameObject openLobbiesWindowButtonReference;
        public AssetReferenceGameObject lobbyNameLabelReference;
        public AssetReferenceGameObject lobbyPlayersCountTextReference;
        public WebRequestTableConfig lobbiesSearchWindowConfig;
    }
}