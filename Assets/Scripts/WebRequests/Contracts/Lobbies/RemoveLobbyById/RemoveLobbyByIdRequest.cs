using WebRequests.Contracts.Base;

namespace WebRequests.Contracts.Lobbies.RemoveLobbyById
{
    public class RemoveLobbyByIdRequest : WebRequestDto<RemoveLobbyByIdResponse>
    {
        public int LobbyId { get; set; }
    }
}