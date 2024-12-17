using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Sprint.GetWinnerByLobbyId
{
    public class GetWinnerByLobbyIdRequest : WebRequestDto<GetWinnerByLobbyIdResponse>
    {
        public int LobbyId { get; set; }
    }
}