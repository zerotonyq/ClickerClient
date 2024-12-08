using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Elements.Tables.Base.SimpleTable
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(SimpleTableConfig), fileName = "Default" + nameof(SimpleTableConfig))]
    public class SimpleTableConfig : ScriptableObject
    {
        public AssetReferenceGameObject rowPrefab;
    }
    
}