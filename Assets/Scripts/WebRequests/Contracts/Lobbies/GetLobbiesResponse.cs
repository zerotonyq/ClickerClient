using System.Collections.Generic;
using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Lobbies
{
    public class GetLobbiesResponse : WebResponseDto
    {
        public IEnumerable<LobbyDto> Lobbies { get; set; }
    }
}