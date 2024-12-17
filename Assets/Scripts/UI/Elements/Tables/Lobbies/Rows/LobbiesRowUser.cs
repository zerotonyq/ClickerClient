using EventBus.Subscribers.Lobbies;
using TMPro;
using UI.Elements.Table.Base;
using UnityEngine;
using WebRequests.Contracts.Lobbies;

namespace UI.Elements.Tables.Lobbies.Rows
{
    public class LobbiesRowUser : Row<LobbyDto>
    {
        [SerializeField] private SimpleButton enterLobbyButton;
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private TextMeshProUGUI playersCountText;
        [SerializeField] private TextMeshProUGUI leagueText;
        public override void Initialize(int id, LobbyDto dto)
        {
            idText.text = dto.Id.ToString();
            
            leagueText.text = dto.LeagueId.ToString();
            
            playersCountText.text = dto.PlayersCount + "/" + dto.MaxPlayerCount;
            
            enterLobbyButton.OnClick.AddListener(() =>
            {
                EventBus.EventBus.RaiseEvent<IEnterLobbyRequestSubscriber>(async sub => await sub.HandleEnterLobbyRequest(id));
            });
        }
    }
}