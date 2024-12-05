using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace AssetsInitialization
{
    public class AddressablesInitializer : IInitializable
    {
        public async void Initialize() => await Addressables.InitializeAsync();
    }
}