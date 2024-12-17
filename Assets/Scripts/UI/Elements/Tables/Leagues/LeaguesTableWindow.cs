using System.Threading.Tasks;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Leagues.Rows;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests.Contracts.Leagues;

namespace UI.Elements.Tables.Leagues
{
    public class LeaguesTableWindow: WebRequestTableWindow<LeagueDto>, IDeleteLobbyRequestSubscriber<LeaguesRowAdmin>
    {
        public override string Url { get; set; }
        protected override Task GetRows()
        {
            throw new System.NotImplementedException();
        }

        
        public Task HandleDeleteLobbyRequest(LeaguesRowAdmin row)
        {
            throw new System.NotImplementedException();
        }
    }
}