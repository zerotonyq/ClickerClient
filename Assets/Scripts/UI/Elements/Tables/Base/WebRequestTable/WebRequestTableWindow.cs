using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EventBus.Subscribers.GameUI;
using UI.Elements.Table.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace UI.Elements.Tables.Base
{
    public abstract class WebRequestTableWindow<T> : MonoBehaviour
    {
        protected readonly List<Row<T>> Rows = new();
        
        [SerializeField] protected WebRequestTableConfig config;

        [SerializeField] private SimpleAnimatedButton closeButton;
        
        [SerializeField] protected Transform contentTransform;

        [SerializeField] protected bool isAdmin;
        public abstract string Url { get; set; } 

        public virtual void Initialize()
        {
            closeButton.OnClick.AddListener(() =>
            {
                Deactivate();
                EventBus.EventBus.RaiseEvent<IMainScreenRequestSubscriber>(sub => sub.HandleMainScreenRequest());
            });
            
            Deactivate();
        }

        public virtual void Deactivate()
        {
            StartCoroutine(DestroyRowsCoroutine());
        }

        private IEnumerator DestroyRowsCoroutine()
        {
            foreach (var row in Rows)
            {
                Addressables.ReleaseInstance(row.gameObject);
                yield return new WaitForSeconds(0.2f);
            }
            
            Rows.Clear();
            gameObject.SetActive(false);
        }

        public virtual void DestroyRow(Row<T> row)
        {
            Addressables.ReleaseInstance(row.gameObject);
            Rows.Remove(row);
        }

        protected abstract Task GetRows();
       

        public void Activate()
        {
            var action =new Action(async () => {await GetRows(); });
            action.Invoke();
        }
    }
}