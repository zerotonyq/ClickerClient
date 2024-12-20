﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI.Auth;
using JwtTokens;
using Newtonsoft.Json;
using UnityEngine;
using User;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using Utils.LocalStorageSaving;
using WebRequests;
using WebRequests.Contracts.SignUp;
using Zenject;

namespace Controllers.UserAuth
{
    public class UserSignUpController : ISignUpRequestedSubscriber, IDisposable
    {
        [Inject]
        public void Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<ISignUpRequestedSubscriber>(this);
        }

        private async Task SignUp(string username, string password)
        {
            var result = await WebRequestProvider.SendJsonRequest<SignUpRequest, SignUpResponse>(
                ApiPaths.AUTH_SIGNUP,
                new SignUpRequest
                {
                    Password = username,
                    Username = password
                });


            if (result == null)
            {
                Debug.LogError("Cannot sign up");
                EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(async sub =>
                    await sub.HandleAuthSuccess(new AuthResult { Success = false }));
                return;
            }

            EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(async sub =>
                await sub.HandleAuthSuccess(new AuthResult { Success = true }));

            ProcessToken(result);

            UserDataProvider.SetUsername(username);
            
            UserDataProvider.SetId(Convert.ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken).Claims
                .First(c => c.Type == "Id")?.Value));

#if UNITY_WEBGL
            LocalStorageHandler.SaveGame(JsonConvert.SerializeObject(result));
#elif UNITY_EDITOR
            PlayerPrefs.SetString("Tokens", JsonConvert.SerializeObject(result));
#endif
        }

        private static void ProcessToken(SignUpResponse authTokensResult)
        {
#if UNITY_WEBGL
            if (string.IsNullOrEmpty(authTokensResult.AccessToken))
            {
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.Handle());
                return;
            }
            JwtTokenProvider.SetTokens(authTokensResult.AccessToken, authTokensResult.RefreshToken);

            EventBus.EventBus.RaiseEvent<IUsernameObtainSubscriber>(sub =>
                sub.Handle(new JwtSecurityTokenHandler().ReadJwtToken(authTokensResult.AccessToken).Claims
                    .FirstOrDefault(c => c.Type == "Username")?.Value));
#elif UNITY_EDITOR

            if (string.IsNullOrEmpty(authTokensResult.AccessToken))
            {
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.HandleAuthUIRequest());
                return;
            }

            JwtTokenProvider.SetTokens(authTokensResult.AccessToken, authTokensResult.RefreshToken);

            UserDataProvider.SetUsername(new JwtSecurityTokenHandler().ReadJwtToken(authTokensResult.AccessToken)
                .Claims
                .FirstOrDefault(c => c.Type == "Username")?.Value);
            
            UserDataProvider.SetId(Convert.ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(authTokensResult.AccessToken).Claims
                .First(c => c.Type == "Id")?.Value));
#endif
        }

        public async void Handle(string username, string password) => await SignUp(username, password);

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<ISignUpRequestedSubscriber>(this);
    }
}