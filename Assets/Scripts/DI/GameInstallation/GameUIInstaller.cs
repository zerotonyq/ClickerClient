using Gameplay.Points;
using Gameplay.Sprints;
using Loading;
using UI.Controllers.AdminUIController;
using UI.Controllers.AuthUIController;
using UI.Controllers.GameTitleUIController;
using UI.Controllers.LobbiesUIController;
using UI.Controllers.NotificationsUIController;
using UI.Controllers.PointsUIController;
using UI.Controllers.SprintUIController;
using UnityEngine.AddressableAssets;
using Zenject;

namespace DI.GameInstallation
{
    public class GameUIInstaller : MonoInstaller
    {
        public AssetReferenceGameObject authCanvas;
        public AssetReferenceGameObject adminCanvas;
        public AssetReferenceGameObject lobbiesCanvas;
        public AssetReferenceGameObject notificationsCanvas;
        public AssetReferenceGameObject gameTitleCanvas;
        public AssetReferenceGameObject pointsCanvas;
        public AssetReferenceGameObject sprintsCanvas;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AuthUIController>().AsSingle()
                .WithArguments(authCanvas, transform)
                .WhenInjectedInto<LoadingManager>();
            
            Container.BindInterfacesAndSelfTo<AdminUIController>().AsSingle()
                .WithArguments(adminCanvas, transform)
                .WhenInjectedInto<LoadingManager>();
            
            Container.BindInterfacesAndSelfTo<LobbiesUIController>().AsSingle()
                .WithArguments(lobbiesCanvas, transform)
                .WhenInjectedInto<LoadingManager>();
            
            Container.BindInterfacesAndSelfTo<NotificationsUIController>().AsSingle()
                .WithArguments(notificationsCanvas, transform)
                .WhenInjectedInto<LoadingManager>();
            
            Container.BindInterfacesAndSelfTo<GameTitleUIController>().AsSingle()
                .WithArguments(gameTitleCanvas, transform)
                .WhenInjectedInto<LoadingManager>();
            
            Container.BindInterfacesAndSelfTo<PointsUIController>().AsSingle()
                .WithArguments(pointsCanvas, transform)
                .WhenInjectedInto(typeof(LoadingManager), typeof(PointsManager));
            
            Container.BindInterfacesAndSelfTo<SprintUIController>().AsSingle()
                .WithArguments(sprintsCanvas, transform)
                .WhenInjectedInto(typeof(LoadingManager), typeof(SprintManager));

            Container.BindInterfacesAndSelfTo<LoadingManager>().AsSingle().NonLazy();
        }
    }
}