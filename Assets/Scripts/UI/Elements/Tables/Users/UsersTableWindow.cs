using System.Threading.Tasks;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Users.Rows;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests.Contracts.Users;

namespace UI.Elements.Tables.Users
{
    public class UsersTableWindow: WebRequestTableWindow<UserDto>, IDeleteLobbyRequestSubscriber<UsersRowAdmin>
    {
        public override string Url { get; set; }
        protected override Task GetRows()
        {
            throw new System.NotImplementedException();
        }

        public Task HandleDeleteLobbyRequest(UsersRowAdmin row)
        {
            throw new System.NotImplementedException();
        }
    }
}