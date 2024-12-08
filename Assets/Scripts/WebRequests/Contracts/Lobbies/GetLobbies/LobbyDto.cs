namespace WebRequests.Contracts.Lobbies
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public int PlayersCount { get; set; }
        public int MaxPlayerCount { get; set; }
    }
}