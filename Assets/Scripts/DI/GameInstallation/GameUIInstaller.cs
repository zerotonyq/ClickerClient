using Controllers.UserAuth;
using UI.Controllers.AdminUIController;
using UI.Controllers.AdminUIController.Config;
using UI.Controllers.GameUIController;
using UI.Controllers.GameUIController.Config;
using UI.Controllers.LobbiesUIController;
using UI.Controllers.LobbiesUIController.Config;
using UI.Controllers.NotificationsUIController;
using UI.Controllers.NotificationsUIController.Config;
using UnityEngine;
using Zenject;

namespace DI.GameInstallation
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private GameUIConfig gameUIConfig;
        [SerializeField] private AdminUIConfig adminUiConfig;
        [SerializeField] private LobbiesControllerUIConfig lobbiesControllerUIConfig;
        [SerializeField] private NotificationsControllerUIConfig notificationsControllerUIConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameUIConfig);
            Container.BindInstance(adminUiConfig);
            Container.BindInstance(lobbiesControllerUIConfig);
            Container.BindInstance(notificationsControllerUIConfig);
            
            Container.BindExecutionOrder<AuthUIController>(-10);
            Container.BindExecutionOrder<UserSignInController>(-20);
            
            Container.BindInterfacesAndSelfTo<AuthUIController>().AsSingle().WithArguments(transform).NonLazy();
            Container.BindInterfacesAndSelfTo<AdminUIController>().AsSingle().WithArguments(transform).NonLazy();
            Container.BindInterfacesAndSelfTo<LobbiesUIController>().AsSingle().WithArguments(transform).NonLazy();
            Container.BindInterfacesAndSelfTo<NotificationsUIController>().AsSingle().WithArguments(transform).NonLazy();
        }
    }
}