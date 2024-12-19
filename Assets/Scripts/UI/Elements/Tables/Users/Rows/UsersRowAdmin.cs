using System.Threading.Tasks;
using EventBus.Subscribers.Base;
using TMPro;
using UI.Elements.Table.Base;
using UnityEngine;
using Utils.EventBus.Subscribers.Users;
using WebRequests.Contracts.Users;

namespace UI.Elements.Tables.Users.Rows
{
    public class UsersRowAdmin : Row<UserDto>
    {
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private SimpleButton deleteUserButton;
        
        public override void Initialize(int id, UserDto data)
        {
            ID = id;

            idText.text = id.ToString();

            nameText.text = data.Username;
            
            deleteUserButton.OnClick.AddListener(() =>
            {
                EventBus.EventBus.RaiseEvent<IDeleteUserRequestSubscriber<UsersRowAdmin>>(sub => sub.HandleDeleteUserRequest(this));
            });
        }
    }
}