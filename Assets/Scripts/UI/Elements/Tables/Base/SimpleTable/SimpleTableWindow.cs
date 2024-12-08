using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Elements.Table.Base;
using UnityEngine;

namespace UI.Elements.Tables.Base.SimpleTable
{
    public abstract class SimpleTableWindow<T, R> : MonoBehaviour where R : Row<T>
    {
        protected readonly List<R> Rows = new();
        
        [SerializeField] protected Transform contentTransform;
        
        protected SimpleTableConfig Config;
        
        public async virtual Task Initialize(SimpleTableConfig config) => Config = config;

        protected abstract Task AddRow(T rowData);
        protected abstract Task RemoveRow(int id);
       
    }
}