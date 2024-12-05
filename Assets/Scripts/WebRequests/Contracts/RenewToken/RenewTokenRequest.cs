using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.RenewToken
{
    public class RenewTokenRequest : WebRequestDto<RenewTokenResponse>
    {
        public string RefreshToken { get; set; }
        public string Username { get; set; }
    }
}