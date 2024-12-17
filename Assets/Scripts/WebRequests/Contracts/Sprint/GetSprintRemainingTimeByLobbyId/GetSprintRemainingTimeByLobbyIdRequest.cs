using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Sprint.GetSprintRemainingTimeByLobbyId
{
    public class GetSprintRemainingTimeByLobbyIdRequest : WebRequestDto<GetSprintRemainingTimeByLobbyIdResponse>
    {
        public int LobbyId { get; set; }
    }
}