using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using JwtTokens.Contracts;
using Newtonsoft.Json;
using UnityEngine;

namespace JwtTokens
{
    public class JwtTokenProvider
    {
        public static string AccessToken { get; private set; }
        public static string RefreshToken { get; private set; }
        
        public static void SetTokens(string access, string refresh)
        {
            AccessToken = access;
            RefreshToken = refresh;
            
#if UNITY_WEBGL
            LocalStorageHandler.SaveGame(JsonConvert.SerializeObject(renewResponse));
#elif UNITY_EDITOR
            PlayerPrefs.SetString("Tokens", JsonConvert.SerializeObject(new JwtTokensData {AccessToken =  access, RefreshToken = refresh}));
#endif
        }

        public static void RemoveTokens()
        {
            AccessToken = "";
            RefreshToken = "";
            PlayerPrefs.DeleteKey("Tokens");
        }

        public static bool CheckValidExpires(string token)
        {
            var tics = new JwtSecurityTokenHandler()
                .ReadJwtToken(token).Claims
                .FirstOrDefault(c => c.Type == "exp")?.Value;

            var longTics = long.Parse(tics);
            
            return DateTimeOffset.FromUnixTimeSeconds(longTics).ToUniversalTime() > DateTime.Now.ToUniversalTime();
        }
    }
}