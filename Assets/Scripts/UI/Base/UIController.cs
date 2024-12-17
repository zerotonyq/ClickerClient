using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UI.CanvasLayerManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Base
{
    public abstract class UIController
    {
        protected abstract void ToggleVisibility(bool isActive);

        private Canvas _canvas;
         protected Canvas Canvas
         {
             get => _canvas;
             set
             {
                 _canvas = value;
                 _canvas.sortingOrder = CanvasUIManager.Layers[GetType()];
                 _canvas.worldCamera = Camera.main;
             }
         }

         public abstract UniTaskVoid Initialize(AssetReferenceGameObject canvasReference, Transform parent);
    }
}