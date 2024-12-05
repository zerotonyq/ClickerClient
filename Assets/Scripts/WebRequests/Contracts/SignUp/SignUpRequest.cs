using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.SignUp
{
    public class SignUpRequest : WebRequestDto<SignUpResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}