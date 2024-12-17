using EventBus.Subscribers.Common;

namespace User
{
    public class UserDataProvider
    {
        public static string Username { get; set; }

        public static int Id { get; set; }
        
        public static void SetUsername(string username)
        {
            Username = username;

            EventBus.EventBus.RaiseEvent<IUsernameObtainSubscriber>(sub =>
                sub.HandleUsernameObtained(Username));
        }

        public static void SetId(int id) => Id = id;
    }
}