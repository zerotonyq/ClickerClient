#nullable enable
using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.SignIn
{
    public class SignInRequest : WebRequestDto<SignInResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}