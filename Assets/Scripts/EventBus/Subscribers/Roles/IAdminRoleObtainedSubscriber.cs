using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.Base;

namespace EventBus.Subscribers.Roles
{
    public interface IAdminRoleObtainedSubscriber : IGlobalSubscriber
    {
        Task HandleAdminRoleObtained();
    }
}