using System.Threading.Tasks;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Lobbies.Rows;
using UI.Elements.Tables.MiniGames.Rows;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests.Contracts.Lobbies;
using WebRequests.Contracts.MiniGames;

namespace UI.Elements.Tables.MiniGames
{
    public class MiniGamesTableWindow : WebRequestTableWindow<MiniGameDto>, IDeleteLobbyRequestSubscriber<MiniGamesRowAdmin>
    {
        public override string Url { get; set; }
        protected override Task GetRows()
        {
            throw new System.NotImplementedException();
        }

        public Task HandleDeleteLobbyRequest(MiniGamesRowAdmin row)
        {
            throw new System.NotImplementedException();
        }
    }
}