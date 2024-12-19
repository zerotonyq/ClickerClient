using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Elements.Table.Base;
using UI.Elements.Tables.Base;
using UI.Elements.Tables.Base.WebRequestTable;
using UI.Elements.Tables.Users.Rows;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.EventBus.Subscribers.Users;
using WebRequests;
using WebRequests.Contracts.Users;
using WebRequests.Contracts.Users.GetUsers;
using WebRequests.Contracts.Users.RemoveUser;

namespace UI.Elements.Tables.Users
{
    public class UsersTableWindow: WebRequestTableWindow<UserDto>, IDeleteUserRequestSubscriber<UsersRowAdmin>
    {
        public override string Url { get; set; } = ApiPaths.ADMIN_ADMINUSERS_GETUSERS;

        public override void Initialize()
        {
            base.Initialize();
            
            if(isAdmin)
                EventBus.EventBus.SubscribeToEvent<IDeleteUserRequestSubscriber<UsersRowAdmin>>(this);
        }

        protected override async Task GetRows()
        {
            var result =
                await WebRequestProvider.SendJsonRequest<GetUsersRequest, GetUsersResponse>(
                    Url,
                    new GetUsersRequest());
            
            if (result == null)
            {
                Debug.LogError("Невозможно получить пользователей");
                return;
            }
            

            foreach (var userDto in result.UserDtos)
            {
                var userRow = (await Addressables.InstantiateAsync(config.rowPrefab, contentTransform))
                    .GetComponent<Row<UserDto>>();
                
                
                userRow.Initialize(userDto.Id, userDto);
                
                Rows.Add(userRow);
            }
        }
        
        public void OnDestroy()
        {
            EventBus.EventBus.UnsubscribeFromEvent<IDeleteUserRequestSubscriber<UsersRowAdmin>>(this);
        }

        public async Task HandleDeleteUserRequest(UsersRowAdmin row)
        {
            var result =
                await WebRequestProvider.SendJsonRequest<RemoveUserByIdRequest, RemoveUserByIdResponse>(
                    ApiPaths.ADMIN_ADMINUSERS_REMOVEUSERBYID,
                    new RemoveUserByIdRequest()
                    {
                        Id = row.ID
                    });
            
            if (result == null)
            {
                Debug.LogError("Невозможно удалить пользователя");
                return;
            }

            Debug.Log("ROW WITH ID " + row.ID + " DESTROYED");
            
            DestroyRow(row);
        }
    }
}