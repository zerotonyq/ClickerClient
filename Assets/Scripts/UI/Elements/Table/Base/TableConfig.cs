using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace UI.Elements.Table.Base
{
    [CreateAssetMenu(menuName = "CreateConfig/" + nameof(TableConfig), fileName = "Default" + nameof(TableConfig))]
    public class TableConfig : ScriptableObject
    {
        public AssetReferenceGameObject rowPrefab;
        public AssetReferenceGameObject closeWindowButtonPrefab;
    }
}