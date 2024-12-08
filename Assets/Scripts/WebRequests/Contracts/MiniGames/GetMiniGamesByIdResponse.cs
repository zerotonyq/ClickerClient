using System.Collections.Generic;
using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.MiniGames
{
    public class GetMiniGamesByIdResponse : WebResponseDto
    {
        public IEnumerable<MiniGameDto> MiniGameDtos { get; set; }
    }
}