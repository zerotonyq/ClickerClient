using System;
using System.Threading.Tasks;
using EventBus.Subscribers.Lobbies;
using UnityEngine;
using WebRequests;
using WebRequests.Contracts.Lobbies.SetLobbyById;
using Zenject;

namespace Gameplay.Lobbies
{
    public class LobbiesManager : IEnterLobbyRequestSubscriber, IDisposable
    {
        public int CurrentLobbyId { get; private set; }
        
        [Inject]
        public void Initialize() => EventBus.EventBus.SubscribeToEvent<IEnterLobbyRequestSubscriber>(this);


        public async Task HandleEnterLobbyRequest(int id)
        {
            Debug.Log("ENTER LOBBY WITH id " + id);
            var response = await WebRequestProvider.SendJsonRequest<SetLobbyByIdRequest, SetLobbyByIdResponse>(
                ApiPaths.USERS_SETLOBBYBYID, new SetLobbyByIdRequest(){UserId = 1, LobbyId = id});
            
            if(response == null)
                Debug.LogError("Handle enter lobby request error");

            CurrentLobbyId = id;
            
            EventBus.EventBus.RaiseEvent<IEnterLobbySuccessSubscriber>(sub => sub.HandleEnterLobby(id));
        }

        public void Dispose() => EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbyRequestSubscriber>(this);
    }
}