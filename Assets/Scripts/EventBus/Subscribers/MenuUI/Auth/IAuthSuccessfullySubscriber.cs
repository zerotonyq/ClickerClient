using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.MenuUI.Auth
{
    public interface IAuthSuccessfullySubscriber : IGlobalSubscriber
    {
        void Handle(AuthResult result);
    }

    public struct AuthResult
    {
        public bool Success;
        public string Message;
    }
}