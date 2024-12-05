using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Auth;
using JetBrains.Annotations;
using JwtTokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using WebRequests.Contracts.Base;
using WebRequests.Contracts.RenewToken;

namespace WebRequests
{
    public static class WebRequestProvider
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
            },
            Formatting = Formatting.Indented
        };

        [ItemCanBeNull]
        public static async Task<TRes> SendJsonRequest<TReq, TRes>(string url, TReq webRequestDto,
            bool secondRetry = false)
            where TReq : WebRequestDto<TRes>
            where TRes : WebResponseDto, new()
        {
            var requestString = JsonConvert.SerializeObject(webRequestDto, SerializerSettings);
            
            using var webRequest =
                UnityWebRequest.Post(url, requestString,
                    contentType: "application/json");

            if (!string.IsNullOrEmpty(JwtTokenProvider.AccessToken))
                webRequest.SetRequestHeader("Authorization", "Bearer " + JwtTokenProvider.AccessToken);

            var sentRequest = await Send(webRequest);
            
            if (sentRequest.responseCode == 200)
                return JsonConvert.DeserializeObject<TRes>(sentRequest.downloadHandler.text, SerializerSettings);

            if (secondRetry)
            {
                Debug.LogError("second retry failed");
                return null;
            }

            await TryUpdateTokens();

            return await SendJsonRequest<TReq, TRes>(url, webRequestDto, true);
        }

        public static async Task TryUpdateTokens()
        {
            if (string.IsNullOrEmpty(JwtTokenProvider.RefreshToken) ||
                !JwtTokenProvider.CheckValidExpires(JwtTokenProvider.RefreshToken))
            {
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.Handle());
                JwtTokenProvider.RemoveTokens();
                return;
            }

            var renewResponse = await SendJsonRequest<RenewTokenRequest, RenewTokenResponse>(ApiPaths.AUTH_RENEWTOKEN,
                new RenewTokenRequest()
                {
                    RefreshToken = JwtTokenProvider.RefreshToken,
                    Username = new JwtSecurityTokenHandler().ReadJwtToken(JwtTokenProvider.AccessToken)
                        .Claims
                        .FirstOrDefault(c => c.Type == "Username")?.Value
                }, true);

            if (renewResponse == null)
            {
                Debug.Log("something went wrong with recieving new tokens");
                EventBus.EventBus.RaiseEvent<IAuthUIRequestedSubscriber>(sub => sub.Handle());
                JwtTokenProvider.RemoveTokens();
                return;
            }

            JwtTokenProvider.SetTokens(renewResponse.AccessToken,
                renewResponse.RefreshToken);
            
            Debug.Log("new tokens recieved");
        }

        private static async Task<UnityWebRequest> Send(UnityWebRequest webRequest)
        {
            var unityWebRequestAsyncOperation = webRequest.SendWebRequest();

            try
            {
                await unityWebRequestAsyncOperation;
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError("Unregistered " + e.Message + " \n" + e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return unityWebRequestAsyncOperation.webRequest;
        }
    }
}