using DG.Tweening;
using UnityEngine;

namespace Animation
{
    public class SimpleScaleDoTweenAnimationComponent : MonoBehaviour
    {
        [SerializeField] private GameObject animatedObject;
        [SerializeField] private float duration;
        [SerializeField] private float scaleFactor;
        [SerializeField] private bool isLooped;
        [SerializeField] private AnimationCurve animationCurve;

        private Vector3 _initScale;

        void Start()
        {
            if (!animatedObject)
                animatedObject = gameObject;

            _initScale = animatedObject.transform.localScale;

            var sequence = DOTween.Sequence();

            sequence.Append(
                animatedObject.transform.DOScale(_initScale * scaleFactor, duration).SetEase(animationCurve));
            sequence.Append(animatedObject.transform.DOScale(_initScale, duration).SetEase(animationCurve));

            if (isLooped)
                sequence.SetLoops(-1);
        }
    }
}