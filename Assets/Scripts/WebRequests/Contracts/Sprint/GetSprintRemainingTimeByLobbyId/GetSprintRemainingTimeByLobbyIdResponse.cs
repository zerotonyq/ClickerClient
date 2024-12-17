using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Sprint.GetSprintRemainingTimeByLobbyId
{
    public class GetSprintRemainingTimeByLobbyIdResponse : WebResponseDto
    {
        public float RemainingTimeSeconds { get; set; }
    }
}