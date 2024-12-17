using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Elements.Table.Base;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Lobbies.Rows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Lobbies;
using WebRequests;
using WebRequests.Contracts.Lobbies;
using WebRequests.Contracts.Lobbies.RemoveLobbyById;

namespace UI.Elements.Tables.Lobbies
{
    public class LobbiesTableWindow : WebRequestTableWindow<LobbyDto>, IDeleteLobbyRequestSubscriber<LobbiesRowAdmin>
    {
        public override string Url { get; set; } = ApiPaths.USERS_LOBBIES_GETLOBBIES;

        public override void Initialize()
        {
            base.Initialize();
            if (isAdmin)
            {
                EventBus.EventBus.SubscribeToEvent<IDeleteLobbyRequestSubscriber<LobbiesRowAdmin>>(this);
            }
        }

        protected override async Task GetRows()
        {
            var result =
                await WebRequestProvider.SendJsonRequest<GetLobbiesRequest, GetLobbiesResponse>(
                    Url,
                    new GetLobbiesRequest());
            
            if (result == null)
            {
                Debug.LogError("Невозможно получить лобби");
                return;
            }
            

            foreach (var lobbyDto in result.Lobbies)
            {
                var lobbyRow = (await Addressables.InstantiateAsync(config.rowPrefab, contentTransform))
                    .GetComponent<Row<LobbyDto>>();
                
                
                lobbyRow.Initialize(lobbyDto.Id, lobbyDto);
                
                Rows.Add(lobbyRow);
            }
        }

        public void OnDestroy()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IDeleteLobbyRequestSubscriber<LobbiesRowAdmin>>(this);
        }

        public async Task HandleDeleteLobbyRequest(LobbiesRowAdmin row)
        {
            /*var result =
                await WebRequestProvider.SendJsonRequest<RemoveLobbyByIdRequest, RemoveLobbyByIdResponse>(
                    ApiPaths.ADMIN_ADMINLOBBIES_REMOVELOBBYBYID,
                    new RemoveLobbyByIdRequest()
                    {
                        LobbyId = row.ID
                    });
            
            if (result == null)
            {
                Debug.LogError("Невозможно удалить лобби");
                return;
            }*/
            Debug.Log("ROW WITH ID " + row.ID + " DESTROYED");
            DestroyRow(row);
        }
    }
}