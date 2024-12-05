﻿using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.MiniGames
{
    public class GetMiniGamesByIdRequest : WebRequestDto<GetMiniGamesByIdResponse>
    {
        public int Id { get; set; }
    }
}