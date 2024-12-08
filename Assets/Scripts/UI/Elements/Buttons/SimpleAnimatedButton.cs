using System;
using Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements
{
    public class SimpleAnimatedButton : MonoBehaviour, IPointerClickHandler, IDisposable
    {
        private SimplePressDoTweenAnimationComponent _simplePressDoTweenAnimationComponent;

        public void Start() =>
            _simplePressDoTweenAnimationComponent = GetComponent<SimplePressDoTweenAnimationComponent>();

        public Button.ButtonClickedEvent OnClick { get; } = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            _simplePressDoTweenAnimationComponent.HandleWithAction(OnClick.Invoke);
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