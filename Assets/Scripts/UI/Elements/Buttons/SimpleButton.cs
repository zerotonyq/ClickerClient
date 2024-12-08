using System;
using Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements
{
    public class SimpleButton : MonoBehaviour, IPointerClickHandler, IDisposable
    {
        public Button.ButtonClickedEvent OnClick { get; } = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void Dispose()
        {
            OnClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            OnClick.RemoveAllListeners();
        }
        
    }
}