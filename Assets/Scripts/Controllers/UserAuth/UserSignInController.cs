using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Auth;
using EventBus.Subscribers.Common;
using EventBus.Subscribers.MenuUI.Auth;
using EventBus.Subscribers.Roles;
using JwtTokens;
using JwtTokens.Contracts;
using Newtonsoft.Json;
using UnityEngine;
using WebRequests;
using WebRequests.Contracts.SignIn;
using Zenject;

namespace Controllers.UserAuth
{
    public class UserSignInController : ISignInRequestedSubscriber, IDisposable
    {
        [Inject]
        public async UniTaskVoid Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<ISignInRequestedSubscriber>(this);

            await ProcessToken(null);
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
                EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(sub =>
                    sub.Handle(new AuthResult { Success = false, Message = "Невозможно войти" }));
                return;
            }

            EventBus.EventBus.RaiseEvent<IAuthSuccessfullySubscriber>(sub =>
                sub.Handle(new AuthResult { Success = true, Message = "Удачно" }));

            await ProcessToken(new JwtTokensData()
                { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken });

            EventBus.EventBus.RaiseEvent<IUsernameObtainSubscriber>(sub => sub.Handle(username));
        }

        private static async Task ProcessToken(JwtTokensData tokensData)
        {
            tokensData ??=
                JsonConvert.DeserializeObject<JwtTokensData>(PlayerPrefs.GetString("Tokens", defaultValue: "{}"));

            if (string.IsNullOrEmpty(tokensData.AccessToken) ||
                string.IsNullOrEmpty(tokensData.RefreshToken))
            {
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.Handle());
                return;
            }

            JwtTokenProvider.SetTokens(tokensData.AccessToken, tokensData.RefreshToken);


            if (!JwtTokenProvider.CheckValidExpires(JwtTokenProvider.AccessToken))
                await WebRequestProvider.TryUpdateTokens();

            var claims = new JwtSecurityTokenHandler().ReadJwtToken(tokensData.AccessToken).Claims;

            EventBus.EventBus.RaiseEvent<IUsernameObtainSubscriber>(sub =>
                sub.Handle(claims.FirstOrDefault(c => c.Type == "Username")?.Value));

            var rolesRaw = claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            ProcessRoles(rolesRaw);
        }

        private static void ProcessRoles(string rolesRaw)
        {
            if (string.IsNullOrEmpty(rolesRaw))
                return;

            var roles = rolesRaw.Split(',');

            if (roles.Contains("admin")) EventBus.EventBus.RaiseEvent<IAdminRoleObtainedSubscriber>(async sub => await sub.HandleAdminRoleObtained());
        }

        public async void Handle(string username, string password) => await SignIn(username, password);

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<ISignInRequestedSubscriber>(this);
    }
}