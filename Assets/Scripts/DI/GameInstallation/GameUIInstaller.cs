using Controllers.UserAuth;
using UI.Controllers.AdminUIController;
using UI.Controllers.AdminUIController.Config;
using UI.Controllers.MenuUIController;
using UI.Controllers.MenuUIController.Config;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DI.GameInstallation
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private GameUIConfig gameUIConfig;
        [SerializeField] private AdminUIConfig adminUiConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameUIConfig);
            Container.BindInstance(adminUiConfig);
            
            Container.BindExecutionOrder<GameUIController>(-10);
            Container.BindExecutionOrder<UserSignInController>(-20);
            Container.BindInterfacesAndSelfTo<GameUIController>().AsSingle().WithArguments(transform).NonLazy();
            Container.BindInterfacesAndSelfTo<AdminUIController>().AsSingle().WithArguments(transform).NonLazy();
        }
    }
}