using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.Elements.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Elements.Table.Base
{
    public abstract class TableEditorWindow : MonoBehaviour
    {
        protected readonly List<Row> Rows = new();
        protected TableConfig Config;

        private SimpleAnimatedButton _closeButton;
        [SerializeField] protected Transform contentTransform;
        public abstract string Url { get; set; } 

        public async Task Initialize(TableConfig config)
        {
            Config = config;

            _closeButton = (await Addressables.InstantiateAsync(Config.closeWindowButtonPrefab, transform))
                .GetComponent<SimpleAnimatedButton>();
            
            _closeButton.OnClick.AddListener(Deactivate);
            
            Deactivate();
        }

        private void Deactivate()
        {
            var task = Task.Run(() =>
            {

                foreach (var row in Rows)
                {
                    Destroy(row);
                }
            });

            task.Wait();
            
            Rows.Clear();
            
            gameObject.SetActive(false);
        }

        protected abstract Task GetRows();
       

        public void Activate()
        {
            var action =new Action(async () => {await GetRows(); });
            action.Invoke();
        }
    }
}