using UI.Controllers.Base.Config;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Controllers.GameUIController.Config
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(GameUIConfig), fileName = "Default" + nameof(GameUIConfig))]
    public class GameUIConfig : UIConfig
    {
        public AssetReferenceGameObject startButtonReference;
        public AssetReferenceGameObject authWindow;
        public AssetReferenceGameObject usernameText;
    }
}