using System.Collections.Generic;
using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.MiniGames
{
    public class GetMiniGamesByIdResponse : WebResponseDto
    {
        public List<string> Names { get; set; }
    }
}