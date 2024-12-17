using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Elements.Table.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Elements.Tables.Base.SimpleTable
{
    public abstract class SimpleTableWindow<T, R> : MonoBehaviour where R : Row<T>
    {
        protected readonly List<R> Rows = new();
        
        [SerializeField] protected Transform contentTransform;
        
        [SerializeField] protected SimpleTableConfig config;
        
        public abstract Task Initialize();

        protected abstract Task AddRow(T rowData);

        protected abstract Task RemoveRow(int id);
    }
}