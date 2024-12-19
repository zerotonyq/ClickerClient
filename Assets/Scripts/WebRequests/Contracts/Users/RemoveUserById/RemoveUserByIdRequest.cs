using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Users.RemoveUser
{
    public class RemoveUserByIdRequest : WebRequestDto<RemoveUserByIdResponse>
    {
        public int Id { get; set; }
    }
}