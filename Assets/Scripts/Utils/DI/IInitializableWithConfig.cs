using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Utils.DI
{
    public interface IInitializableWithConfig<in T> where T : Config.Config
    {
        Task Initialize(T config);
    }


}