using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Lobbies.SetLobbyById
{
    public class SetLobbyByIdRequest : WebRequestDto<SetLobbyByIdResponse>
    {
        public int UserId { get; set; }
        public int? LobbyId { get; set; }
    }
}