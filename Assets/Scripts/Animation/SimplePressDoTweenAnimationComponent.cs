using System;
using DG.Tweening;
using UnityEngine;

namespace Animation
{
    public class SimplePressDoTweenAnimationComponent : MonoBehaviour
    {
        [SerializeField] private GameObject animatedObject;
        [SerializeField] private float duration;
        [SerializeField] private float scaleFactor;
        [SerializeField] private AnimationCurve animationCurve;
        private Vector3 _initScale;

        private Sequence _currentSequence;

        public void Start()
        {
            if (!animatedObject)
                animatedObject = gameObject;
            _initScale = animatedObject.transform.localScale;
        }

        public void HandleWithAction(Action action)
        {
            _currentSequence = DOTween.Sequence();

            _currentSequence.Append(animatedObject.transform.DOScale(_initScale * scaleFactor, duration)
                .SetEase(animationCurve));
            _currentSequence.Append(animatedObject.transform.DOScale(_initScale, duration).SetEase(animationCurve));
            _currentSequence.AppendCallback(action.Invoke);
        }
    }
}