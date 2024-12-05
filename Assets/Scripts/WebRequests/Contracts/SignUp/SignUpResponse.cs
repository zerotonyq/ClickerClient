using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.SignUp
{
    public class SignUpResponse : WebResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}