using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.SignIn
{
    public class SignInResponse : WebResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}