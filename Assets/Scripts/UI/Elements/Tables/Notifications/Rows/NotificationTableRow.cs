using TMPro;
using UI.Elements.Table.Base;
using UnityEngine;

namespace UI.Elements.Tables.Notifications.Rows
{
    public class NotificationTableRow : Row<string>
    {
        [SerializeField] private TextMeshProUGUI notificationMessageText;
        public override void Initialize(int id, string data)
        {
            notificationMessageText.text = data;
        }
    }
}