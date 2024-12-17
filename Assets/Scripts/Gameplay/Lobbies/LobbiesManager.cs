using System;
using System.Threading.Tasks;
using EventBus.Subscribers.Lobbies;
using UnityEngine;
using User;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests;
using WebRequests.Contracts.Lobbies.SetLobbyById;
using Zenject;

namespace Gameplay.Lobbies
{
    public class LobbiesManager : 
        IEnterLobbyRequestSubscriber,
        IExitLobbyRequestSubscriber,
        IDisposable
    {
        public static int CurrentLobbyId { get; private set; }
        
        [Inject]
        public void Initialize()
        {
            EventBus.EventBus.SubscribeToEvent<IEnterLobbyRequestSubscriber>(this);
            EventBus.EventBus.SubscribeToEvent<IExitLobbyRequestSubscriber>(this);
        }


        public async Task HandleEnterLobbyRequest(int id)
        {
            Debug.Log("ENTER LOBBY WITH id " + id);
            var response = await WebRequestProvider.SendJsonRequest<SetLobbyByIdRequest, SetLobbyByIdResponse>(
                ApiPaths.USERS_SETLOBBYBYID, new SetLobbyByIdRequest {UserId = UserDataProvider.Id, LobbyId = id});
            
            if(response == null)
                Debug.LogError("Handle enter lobby request error");

            CurrentLobbyId = id;
            
            EventBus.EventBus.RaiseEvent<IEnterLobbySuccessSubscriber>(sub => sub.HandleEnterLobby(id));
        }

        public void Dispose()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IEnterLobbyRequestSubscriber>(this);
            EventBus.EventBus.UnsubscribeFromEvent<IExitLobbyRequestSubscriber>(this);
        }

        public async Task HandleExitLobbyRequest()
        {
            Debug.Log("EXIT LOBBY WITH id " + CurrentLobbyId);
            var response = await WebRequestProvider.SendJsonRequest<SetLobbyByIdRequest, SetLobbyByIdResponse>(
                ApiPaths.USERS_SETLOBBYBYID, new SetLobbyByIdRequest {UserId = UserDataProvider.Id, LobbyId = null});
            
            if(response == null)
                Debug.LogError("Handle exit lobby request error");

            CurrentLobbyId = -1;
            
            //entBus.EventBus.RaiseEvent<IEnterLobbySuccessSubscriber>(sub => sub.HandleEnterLobby(id));
        }
    }
}