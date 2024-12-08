using EventBus.Subscribers.Lobbies;
using UI.Elements.Table.Base;
using UnityEngine;

namespace UI.Elements.Table.Admin
{
    public class LobbiesRowUser : Row<int>
    {
        [SerializeField] private SimpleButton _enterLobbyButton;
        public override void Initialize(int id, int p)
        {
            _enterLobbyButton.OnClick.AddListener(() =>
            {
                EventBus.EventBus.RaiseEvent<IEnterLobbyRequestSubscriber>(sub => sub.HandleEnterLobbyRequest(id));
            });
        }
    }
}