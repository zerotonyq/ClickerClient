using System.Collections.Generic;
using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Users.GetUsers
{
    public class GetUsersResponse : WebResponseDto
    {
        public List<UserDto> UserDtos { get; set; }
    }
}