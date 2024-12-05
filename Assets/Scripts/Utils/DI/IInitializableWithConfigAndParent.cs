using System.Threading.Tasks;
using UnityEngine;

namespace Utils.DI
{
    public interface IInitializableWithConfigAndParent<in T, in TP>
        where T : Config.Config
        where TP : Transform
    {
        Task Initialize(T config, TP parent);
    }
}