using UnityEngine;

namespace UI.Elements.Table.Base
{
    public abstract class Row<T> : MonoBehaviour
    {
        public abstract void Initialize(int id, T data);
    }
}