using UI.Controllers.Base.Config;
using UI.Elements.Table.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Controllers.AdminUIController.Config
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(AdminUIConfig), fileName = "Default" + nameof(AdminUIConfig))]
    public class AdminUIConfig : UIConfig
    {
        public AssetReferenceGameObject miniGamesWindow;
        public AssetReferenceGameObject leaguesWindow;
        public AssetReferenceGameObject usersWindow;
        public AssetReferenceGameObject lobbiesWindow;
        
        public TableConfig miniGamesWindowConfig;
        public TableConfig leaguesWindowConfig;
        public TableConfig usersWindowConfig;
        public TableConfig lobbiesWindowConfig;

        public AssetReferenceGameObject miniGamesWindowOpenButton;
        public AssetReferenceGameObject leaguesWindowOpenButton;
        public AssetReferenceGameObject usersWindowOpenButton;
        public AssetReferenceGameObject lobbiesWindowOpenButton;
    }
}