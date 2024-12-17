using UI.Elements;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Leagues;
using UI.Elements.Tables.Lobbies;
using UI.Elements.Tables.MiniGames;
using UI.Elements.Tables.Users;

namespace UI.Controllers.AdminUIController.CanvasContainer
{
    public class AdminUICanvasContainer : UI.Base.CanvasContainer
    {
        public LeaguesTableWindow leaguesWindow;
        public MiniGamesTableWindow miniGamesWindow;
        public UsersTableWindow usersWindow;
        public LobbiesTableWindow lobbiesWindow;

        public SimpleAnimatedButton leaguesWindowOpenButton;
        public SimpleAnimatedButton miniGamesWindowOpenButton;
        public SimpleAnimatedButton usersWindowOpenButton;
        public SimpleAnimatedButton lobbiesWindowOpenButton;
    }
}