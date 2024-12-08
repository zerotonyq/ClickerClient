using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Elements.Table.Base;
using UI.Elements.Tables.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebRequests;
using WebRequests.Contracts.Lobbies;

namespace UI.Elements.Table
{
    public class LobbiesTableWindow : WebRequestTableWindow
    {
        public override string Url { get; set; } = ApiPaths.USERS_LOBBIES_GETLOBBIES;

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
                var lobbyRow = (await Addressables.InstantiateAsync(Config.rowPrefab, contentTransform))
                    .GetComponent<Row<int>>();

                lobbyRow.Initialize(lobbyDto.Id, 0);
                
                Rows.Add(lobbyRow);
            }
        }
    }
}