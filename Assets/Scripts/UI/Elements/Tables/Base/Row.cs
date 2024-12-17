using UnityEngine;

namespace UI.Elements.Table.Base
{
    public abstract class Row<T> : MonoBehaviour
    {
        public int ID { get; protected set; }
        public abstract void Initialize(int id, T data);
    }
}