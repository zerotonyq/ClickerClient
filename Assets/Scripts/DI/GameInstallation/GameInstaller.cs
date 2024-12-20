﻿using Controllers.UserAuth;
using DG.Tweening;
using Gameplay.Lobbies;
using Gameplay.MiniGames;
using Gameplay.Points;
using Gameplay.Sprints;
using Loading;
using UserAuth;
using Zenject;

namespace DI.GameInstallation
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            DOTween.Clear();

            Container.BindInterfacesAndSelfTo<UserSignInController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UserSignUpController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MiniGameManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LobbiesManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<SprintManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PointsManager>().AsSingle().NonLazy();
        }
    }
}