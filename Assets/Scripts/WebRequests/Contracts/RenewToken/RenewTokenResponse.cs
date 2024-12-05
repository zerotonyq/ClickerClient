using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.RenewToken
{
    public class RenewTokenResponse : WebResponseDto
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}