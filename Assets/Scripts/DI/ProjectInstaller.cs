using AssetsInitialization;
using Zenject;


namespace DI
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings() => Container.Bind<AddressablesInitializer>().AsTransient().NonLazy();
    }
}