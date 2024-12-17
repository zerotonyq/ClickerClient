using EventBus.Subscribers.Lobbies;
using TMPro;
using UI.Elements.Table.Base;
using UnityEngine;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests.Contracts.Lobbies;

namespace UI.Elements.Tables.Lobbies.Rows
{
    public class LobbiesRowAdmin : Row<LobbyDto>
    {
        [SerializeField] private SimpleButton deleteLobbyButton;
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private TextMeshProUGUI playersCountText;
        [SerializeField] private TextMeshProUGUI leagueText;
        public override void Initialize(int id, LobbyDto dto)
        {
            ID = id;
            idText.text = dto.Id.ToString();
            
            leagueText.text = dto.LeagueId.ToString();
            
            playersCountText.text = dto.PlayersCount + "/" + dto.MaxPlayerCount;
            
            deleteLobbyButton.OnClick.AddListener(() =>
            {
                EventBus.EventBus.RaiseEvent<IDeleteLobbyRequestSubscriber<LobbiesRowAdmin>>(async sub => await sub.HandleDeleteLobbyRequest(this));
            });
        }
        
    }
}