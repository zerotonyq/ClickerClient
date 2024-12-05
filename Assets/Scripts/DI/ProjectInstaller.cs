using AssetsInitialization;
using SceneManagement;
using Zenject;


namespace DI
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<AddressablesInitializer>().AsTransient().NonLazy();
        }
    }
}