using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Sprint.GetWinnerByLobbyId
{
    public class GetWinnerByLobbyIdResponse : WebResponseDto
    {
        public int WinnerId { get; set; }
    }
}