using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace UI.Elements.Table.Base
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(WebRequestTableConfig), fileName = "Default" + nameof(WebRequestTableConfig))]
    public class WebRequestTableConfig : ScriptableObject
    {
        public AssetReferenceGameObject rowPrefab;
    }
}