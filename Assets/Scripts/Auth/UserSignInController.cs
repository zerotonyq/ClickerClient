using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI.Auth;
using EventBus.Subscribers.Roles;
using JwtTokens;
using JwtTokens.Contracts;
using Newtonsoft.Json;
using UnityEngine;
using User;
using Utils.EventBus.Subscribers.Loading;
using Utils.EventBus.Subscribers.MenuUI.Auth;
using WebRequests;
using WebRequests.Contracts.SignIn;
using Zenject;

namespace UserAuth
{
    public class UserSignInController :
        ISignInRequestedSubscriber,
        IInitialLoadingEndedSubscriber,
        IDisposable
    {
        [Inject]
        public async UniTaskVoid Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<ISignInRequestedSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IInitialLoadingEndedSubscriber>(this);
        }

        private async Task SignIn(string username, string password)
        {
            var result = await WebRequestProvider.SendJsonRequest<SignInRequest, SignInResponse>(
                ApiPaths.AUTH_SIGNIN,
                new SignInRequest
                {
                    Password = username,
                    Username = password
                });

            if (result == null)
            {
                Debug.LogError("Cannot sign in");
                EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(async sub =>
                    await sub.HandleAuthSuccess(new AuthResult { Success = false }));
                return;
            }

            await ProcessToken(new JwtTokensData()
                { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken });

            UserDataProvider.SetUsername(username);
            
            UserDataProvider.SetId(Convert.ToInt32(new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken).Claims
                .First(c => c.Type == "Id")?.Value));
        }

        private static async Task ProcessToken(JwtTokensData tokensData)
        {
            tokensData ??=
                JsonConvert.DeserializeObject<JwtTokensData>(PlayerPrefs.GetString("Tokens", defaultValue: "{}"));

            if (string.IsNullOrEmpty(tokensData.AccessToken) ||
                string.IsNullOrEmpty(tokensData.RefreshToken))
            {
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.HandleAuthUIRequest());
                return;
            }

            JwtTokenProvider.SetTokens(tokensData.AccessToken, tokensData.RefreshToken);

            if (!JwtTokenProvider.CheckValidExpires(JwtTokenProvider.AccessToken))
                await WebRequestProvider.TryUpdateTokens();

            var claims = new JwtSecurityTokenHandler().ReadJwtToken(tokensData.AccessToken).Claims;

            EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(async sub =>
                await sub.HandleAuthSuccess(new AuthResult { Success = true }));

            UserDataProvider.SetUsername(claims.FirstOrDefault(c => c.Type == "Username")?.Value);
            
            UserDataProvider.SetId(Convert.ToInt32(claims.First(c => c.Type == "Id")?.Value));

            ProcessRoles(claims.FirstOrDefault(c => c.Type == "Role")?.Value);
        }

        private static void ProcessRoles(string rolesRaw)
        {
            if (string.IsNullOrEmpty(rolesRaw))
                return;

            var roles = rolesRaw.Split(',');

            if (roles.Contains("admin"))
                EventBus.EventBus.RaiseEvent<IAdminRoleObtainedSubscriber>(async sub =>
                    await sub.HandleAdminRoleObtained());
        }

        public async void HandleSignInRequest(string username, string password) => await SignIn(username, password);

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<ISignInRequestedSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IInitialLoadingEndedSubscriber>(this);
        }

        public async Task HandleInitialLoadingEnded() => await ProcessToken(null);
    }
}